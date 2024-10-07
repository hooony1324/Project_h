using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum EEntityControlType
{
    Player,
    NonPlayer,
}


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

    public bool IsPlayer => controlType == EEntityControlType.Player;
    public virtual bool IsDead => Stats.HPStat != null && Mathf.Approximately(Stats.HPStat.DefaultValue, 0f);
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        Animator = GetComponent<Animator>();

        Stats = GetComponent<Stats>();
        Stats.Setup(this);

        Movement = GetComponent<EntityMovement>();
        Movement.Setup(this);

        StateMachine = GetComponent<MonoStateMachine<Entity>>();
        StateMachine.Setup(this);

        SkillSystem = GetComponent<SkillSystem>();
        SkillSystem.Setup(this);

        onTakeDamage += SpawnDamageText;

        return true;
    }

    public virtual void SetInfo(int templateId)
    {
        
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