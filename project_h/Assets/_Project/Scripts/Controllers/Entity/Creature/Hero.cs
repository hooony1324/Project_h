using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NavMeshPlus.Extensions;
using UnityEngine;
using static Define;

public class Hero : Entity
{
    
    private CancellationTokenSource _searchTargetCts;

    public bool IsMainHero { get; set; }

    UI_WorldText infoText;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Hero;
        infoText = Util.FindChild<UI_WorldText>(gameObject);
        enemyLayerMask = Util.GetLayerMask("Monster");

        return true;
    }
    


    void OnEnable()
    {
        _searchTargetCts = new CancellationTokenSource();
        SearchTargetAsync().Forget();
    }

    void OnDisable()
    {
        if (_searchTargetCts != null)
        {
            _searchTargetCts.Cancel();
            _searchTargetCts.Dispose();
            _searchTargetCts = null;
        }
    }

    public override void SetData(EntityData data)
    {
        base.SetData(data);
        
        
    }

    void Update()
    {
        infoText.SetInfo(StateMachine.GetCurrentState().ToString());

        // 자동 공격 시스템
        // 강제로 움직이고 있으면 공격 안함
        // 근처에 적이 있으면 공격
        if (Movement.IsForcedMoving)
            return;

        if (Target == null)
            return;

        // if (SkillSystem.DefaultAttack.IsUseable)
        // {
        //     SkillSystem.DefaultAttack.Use();
        // }

    }

    async UniTaskVoid SearchTargetAsync()
    {
        while (true)
        {
            if (_searchTargetCts == null || _searchTargetCts.IsCancellationRequested)
            {
                break;
            }

            if (StatsComponent == null)
            {
                await UniTask.NextFrame();
                continue;
            }

            float searchRange = StatsComponent.GetValue(StatsComponent.SearchRangeStat);
            var colliders = Physics2D.OverlapCircleAll(Position, searchRange, EnemyLayerMask);

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

            Target = nearestEnemy;

            await UniTask.Delay(500, cancellationToken: _searchTargetCts.Token);
        
        }
    }
}