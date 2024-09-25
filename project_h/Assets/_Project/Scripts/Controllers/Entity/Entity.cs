using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public enum EEntityControlType
{
    Player,
    NotPlayer,
}


public abstract class Entity : BaseObject
{
    [SerializeField]
    private EEntityControlType controlType;

    public Animator Animator {get; private set; }
    public Stats Stats { get; private set; }
    public EntityMovement Movement;
    public MonoStateMachine<Entity> StateMachine { get; private set; }
    public SkillSystem SkillSystem;
    public Transform Target;

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



        return true;
    }

    public virtual void SetInfo(int templateId)
    {

    }



}