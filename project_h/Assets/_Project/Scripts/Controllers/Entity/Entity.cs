using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using static Define;
using Unity.Collections;


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
    [SerializeField] public StatsComponent StatsComponent { get; private set; }
    public EntityMovement Movement;
    public MonoStateMachine<Entity> StateMachine { get; private set; }
    public SkillSystem SkillSystem;

    // 공격 혹은 치유와 같이 대상 지정
    public Entity Target;
    public Category[] Categories => categories;
    [SerializeField] protected Category enemyCategory;

    protected int layerMask = 0;
    protected int enemyLayerMask = 0;
    public int EnemyLayerMask => enemyLayerMask;
    public int LayerMask => layerMask;

    public virtual bool IsMoving => Movement.IsMoving; 
    public bool IsPlayer => controlType == EEntityControlType.Player;
    public bool IsEnemyTargeted => Target != null && Target.HasCategory(enemyCategory);
    public virtual bool IsDead => StatsComponent.HPStat != null && Mathf.Approximately(StatsComponent.HPStat.DefaultValue, 0f);

    private Transform _fireSocket;
    private CircleCollider2D _collider;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        Animator = GetComponent<Animator>();
        _collider = GetComponent<CircleCollider2D>();

        onTakeDamage += SpawnDamageText;

        SortingGroup sg = Util.GetOrAddComponent<SortingGroup>(gameObject);
        sg.sortingOrder = SortingLayers.ENTITY;        

        StateMachine = GetComponent<MonoStateMachine<Entity>>();
        StateMachine.Setup(this);

        return true;
    }

    public virtual void SetData(EntityData data)
    {
        transform.localScale = new Vector3(data.Scale, data.Scale, 1);
        Animator.runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>(data.AnimatorControllerName);

        StatsComponent = GetComponent<StatsComponent>();
        StatsComponent.Setup(this, data.StatOverrides);

        Movement = GetComponent<EntityMovement>();
        Movement.Setup(this);

        SkillSystem = GetComponent<SkillSystem>();
        SkillSystem.Setup(this, data);
        SkillSystem.onSkillTargetSelectionCompleted += ReserveSkill;

        Movement.enabled = true;
        Movement.AgentEnabled = true;
        _collider.enabled = true;
    }

    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (result.resultMessage != SearchResultMessage.OutOfRange)
            return;
        
        if (!skill.IsInState<SearchingTargetState>())
            return;
            
        SkillSystem.ReserveSkill(skill);

        var selectionResult = skill.TargetSelectionResult;
        if (selectionResult.selectedTarget)
            Movement.TraceTarget = selectionResult.selectedTarget.transform;
    }

    // instigator : 이즈리얼
    // causer : 이즈리얼 Q
    public void TakeDamage(Entity instigator, object causer, float damage)
    {
        if (IsDead)
            return;

        float prevValue = StatsComponent.HPStat.DefaultValue;

        damage *= StatsComponent.FragilityStat.Value;
        StatsComponent.HPStat.DefaultValue -= damage;

        onTakeDamage?.Invoke(this, instigator, causer, damage);

        if (Mathf.Approximately(StatsComponent.HPStat.DefaultValue, 0f))
            OnDead(); 
    }

    public Transform GetFireSocket()
    {
        if (_fireSocket == null)
            _fireSocket = Util.FindChild<Transform>(gameObject, "FireSocket", recursive: true);

        return _fireSocket;
    }

    private void SpawnDamageText(Entity entity, Entity instigator, object causer, float damage)
    {
        Managers.Object.SpawnFloatingText(entity.CenterPosition, damage.ToString());
    }

    private void OnDead()
    {
        SkillSystem.CancelAll(true);

        _collider.enabled = false;

        onDead?.Invoke(this);
        onDead = null;
    }
    public bool HasCategory(Category category) => categories.Any(x => x.ID == category.ID);
    public bool HasCategory(string categoryName) => categories.Any(x => x.CodeName == categoryName);

    public bool IsInState<T>() where T : State<Entity>
        => StateMachine.IsInState<T>();

    public bool IsInState<T>(int layer) where T : State<Entity>
        => StateMachine.IsInState<T>(layer);
}