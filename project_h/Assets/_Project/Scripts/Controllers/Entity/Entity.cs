using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using static Define;


public enum EEntityControlType
{
    Player,
    NonPlayer,
}

/// <summary>
/// 전투를 하는 객체
/// </summary>
public abstract class Entity : BaseObject
{
    public delegate void TakeDamageHandler(Entity entity, Entity instigator, object causer, float damage);
    public delegate void DeadHandler(Entity entity);
    public event TakeDamageHandler onTakeDamage;
    public event DeadHandler onDead;

    [SerializeField]
    private EEntityControlType controlType;
    public EEntityControlType ControlType 
    {
        get => controlType; 
        set
        {
            controlType = value;
        }
    }

    [SerializeField]
    private Category[] categories;

    public Animator Animator {get; private set; }
    public Stats Stats { get; private set; }
    public EntityMovement Movement;
    public MonoStateMachine<Entity> StateMachine { get; private set; }
    public SkillSystem SkillSystem;

    // 공격 혹은 치유와 같이 대상 지정
    public Entity Target;
    public Category[] Categories => categories;
    [SerializeField] protected Category enemyCategory;

    public bool IsPlayer => controlType == EEntityControlType.Player;
    public bool IsEnemyTargeted => Target != null && Target.HasCategory(enemyCategory);
    public virtual bool IsDead => Stats.HPStat != null && Mathf.Approximately(Stats.HPStat.DefaultValue, 0f);
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        Animator = GetComponent<Animator>();

        onTakeDamage += SpawnDamageText;

        SortingGroup sg = Util.GetOrAddComponent<SortingGroup>(gameObject);
        sg.sortingOrder = SortingLayers.ENTITY;        

        return true;
    }

    public virtual void SetData(EntityData data)
    {
        transform.localScale = new Vector3(data.Scale, data.Scale, 1);
        Animator.runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>(data.AnimatorControllerName);

        Stats = GetComponent<Stats>();
        Stats.Setup(this, data.StatOverrides);

        Movement = GetComponent<EntityMovement>();
        Movement.Setup(this);

        SkillSystem = GetComponent<SkillSystem>();
        SkillSystem.Setup(this);
        SkillSystem.onSkillTargetSelectionCompleted += ReserveSkill;

        StateMachine = GetComponent<MonoStateMachine<Entity>>();
        StateMachine.Setup(this);
    }


    private Coroutine coSearchingEnemy;
    public bool EnableSearching
    {
        set
        {
            if (IsDead)
                return;

            if (value)
            {
                if (coSearchingEnemy == null)
                    StartCoroutine("SearchingEnemy");
            }
            else
            {
                if (coSearchingEnemy != null)
                {
                    StopCoroutine(coSearchingEnemy);
                    coSearchingEnemy = null;
                }
            }
        }
    }
    private IEnumerator SearchingEnemy()
    {
        while (true)
        {
            FindNearestEnemy();
            yield return WaitFor.Seconds(0.5f);
        }
    }

    public virtual bool FindNearestEnemy()
    {
        if (Target != null)
            return false;

        float searchRange = Stats.GetValue(Stats.SearchRangeStat);
        var colliders = Physics2D.OverlapCircleAll(Position, searchRange);

        float nearestDistance = Mathf.Infinity;
        Entity nearestEnemy = null;
        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            if (entity == this || entity.IsDead)
                continue;

            if (!entity.HasCategory(enemyCategory))
                continue;

            float distance = Vector2.Distance(entity.Position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = entity;
            }
        }

        if (nearestEnemy)
        {
            Target = nearestEnemy;
            return true;
        }

        Target = null;
        return false;
    }

    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (result.resultMessage != SearchResultMessage.OutOfRange ||
            !skill.IsInState<SearchingTargetState>())
            return;
        
        SkillSystem.ReserveSkill(skill);

        var selectionResult = skill.TargetSelectionResult;
        if (selectionResult.selectedTarget)
            Movement.TraceTarget = selectionResult.selectedTarget.transform;
    }

    public void Roll()
    {
        SkillSystem.CancelAll();
        SkillSystem.Roll.Use();
    }

    // instigator : 이즈리얼
    // causer : 이즈리얼 Q
    public void TakeDamage(Entity instigator, object causer, float damage)
    {
        if (IsDead)
            return;

        float prevValue = Stats.HPStat.DefaultValue;
        Stats.HPStat.DefaultValue -= damage;

        onTakeDamage?.Invoke(this, instigator, causer, damage);

        if (Mathf.Approximately(Stats.HPStat.DefaultValue, 0f))
            OnDead(); 
    }

    private void SpawnDamageText(Entity entity, Entity instigator, object causer, float damage)
    {
        Managers.Object.SpawnFloatingText(entity.CenterPosition, damage.ToString());
    }

    private void OnDead()
    {
        if (Movement)
            Movement.enabled = false;

        SkillSystem.CancelAll(true);

        onDead?.Invoke(this);
    }
    public bool HasCategory(Category category) => categories.Any(x => x.ID == category.ID);

    public bool IsInState<T>() where T : State<Entity>
        => StateMachine.IsInState<T>();

    public bool IsInState<T>(int layer) where T : State<Entity>
        => StateMachine.IsInState<T>(layer);
}