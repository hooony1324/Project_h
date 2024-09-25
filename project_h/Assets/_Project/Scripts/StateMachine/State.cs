using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// EntityType은 State를 소유하는 Entity의 Type
// StateMachine의 EntityType과 일치해야함
public abstract class State<EntityType>
{
    public StateMachine<EntityType> Owner { get; private set; }
    public EntityType Entity { get; private set; }
    // State가 StateMachine에 등록된 Layer 번호
    public int Layer { get; private set; }

    // StatMachine에서 사용할 Setup함수
    public void Setup(StateMachine<EntityType> owner, EntityType entity, int layer)
    {
        Owner = owner;
        Entity = entity;
        Layer = layer;

        Setup();
    }

    protected virtual void Setup() { }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }

    public virtual bool OnReceiveMessage(int message, object data) => false;

}