using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Threading;
using System;
using R3;


public class Monster : Entity
{

    UI_WorldText infoText;
    private CancellationTokenSource _targetSearchCts;
    private CancellationTokenSource _stateControllCts;

    private enum MonsterState
    {
        Idle,
        Patrol,
        Combat
    }

    private MonsterState _currentState = MonsterState.Idle;
    private MonsterState CurrentState
    {
        set
        {
            if (_currentState == value)
                return;

            ResetStateToken();
            _currentState = value;

            HandleStates().Forget();
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        infoText = Util.FindChild<UI_WorldText>(gameObject);
        enemyLayerMask = Util.GetLayerMask("Hero");

        return true;
    }
    
    public override void SetData(EntityData data)
    {
        base.SetData(data);

        MonsterData monsterData = data as MonsterData;

        onDead += HandleOnDead;
        Movement.AgentEnabled = true;
        Movement.enabled = true;

        CurrentState = MonsterState.Patrol;
    }

    void OnEnable()
    {
        ClearCancellationToken();
        
        _targetSearchCts = new CancellationTokenSource();
        _stateControllCts = new CancellationTokenSource();
        TargetSearch().Forget();
    }

    void OnDisable()
    {
        ClearCancellationToken();
    }

    void ClearCancellationToken()
    {
        if (_targetSearchCts != null)
        {
            _targetSearchCts.Cancel();
            _targetSearchCts.Dispose();
            _targetSearchCts = null;
        }

        if (_stateControllCts != null)
        {
            _stateControllCts.Cancel();
            _stateControllCts.Dispose();
            _stateControllCts = null;
        }
    }

    private void HandleOnDead(Entity entity)
    {
        Movement.AgentEnabled = false;
        Target = null;
        Movement.TraceTarget = null;
        Invoke("Despawn", 5.0f);
    }

    private void Despawn()
    {
        Managers.Object.Despawn(this);
    }

    void Update()
    {
        infoText.SetInfo(StateMachine.GetCurrentState().ToString());
    }

    async UniTaskVoid TargetSearch()
    {
        while (this.isActiveAndEnabled)
        {
            SearchTarget();

            await UniTask.Delay(500, cancellationToken: _targetSearchCts.Token);
        }
    }

    private void ResetStateToken()
    {
        if (_stateControllCts != null)
        {
            _stateControllCts.Cancel();
            _stateControllCts.Dispose();
        }
        _stateControllCts = new CancellationTokenSource();
    }


    async UniTask HandleStates()
    {
        try 
        {
            switch (_currentState)
            {
                case MonsterState.Patrol:
                    await HandlePatrolState();
                    break;
                case MonsterState.Combat:
                    await HandleCombatState();
                    break;
            }
        }
        catch (OperationCanceledException)
        {
            // State가 변경되어 취소된 경우 정상적으로 처리
            Debug.Log($"State handling cancelled: {_currentState}");
        }
    }


    async UniTask HandlePatrolState()
    {
        while (this.isActiveAndEnabled)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(1, 5)), cancellationToken: _stateControllCts.Token);
            Vector2 randomPos = transform.position;
            randomPos = randomPos.RandomPointInAnnulus(2, 5);
            NavMesh.Raycast(Position, randomPos, out NavMeshHit hit, NavMesh.AllAreas);     

            if (Movement != null)
                Movement.Destination = hit.position;

            await UniTask.WaitUntil(() => Movement != null && Movement.AtDestination, cancellationToken: _stateControllCts.Token);

            if (Movement != null)
                Movement.Stop();
        }

    }

    async UniTask HandleCombatState()
    {
        while (this.isActiveAndEnabled)
        {
            if (SkillSystem.DefaultAttack.IsUseable)
            {
                SkillSystem.DefaultAttack.Use();
                await UniTask.Delay(TimeSpan.FromSeconds(SkillSystem.DefaultAttack.Cooldown), cancellationToken: _stateControllCts.Token);
            }

            await UniTask.Delay(500, cancellationToken: _stateControllCts.Token);
        }
    }

    void SearchTarget()
    {
        if (Target != null || StatsComponent == null || !isActiveAndEnabled)
            return;

        float searchRange = StatsComponent.GetValue(StatsComponent.SearchRangeStat);
        var colliders = Physics2D.OverlapCircleAll(Position, searchRange, EnemyLayerMask);

        if (colliders == null || colliders.Length == 0)
        {
            Target = null;
            Movement.TraceTarget = null;
            CurrentState = MonsterState.Patrol;
            return;
        }

        float nearestDistance = Mathf.Infinity;
        Entity nearestEnemy = null;

        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            if (entity == null || entity.IsDead)
                continue;

            float distance = Vector2.Distance(entity.Position, Position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = entity;
            }
        }

        // Combat 상태로 변경
        // - SerachRange안에 들어온 적에게 Skill.Use()한다
        // - Skill.Use() 하면 Select의 결과로 Find 혹은 OutOfRange가 반환됨
        // - Find인 경우 바로 Apply한다
        // - OutOfRange인 경우 Reserve하여 사정거리가 될 때 Apply한다
        // -- UseImmediately > SelectTargetImmediate > Skill StateMachine에 Use 명령
        // -- Skill 의 사용상태(Casting, Charge, InAction)에 들어가며 Effect Apply됨
        Target = nearestEnemy;
        CurrentState = MonsterState.Combat;
    }



}