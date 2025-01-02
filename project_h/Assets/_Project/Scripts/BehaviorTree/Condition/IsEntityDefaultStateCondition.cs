using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsEntityDefaultState", story: "[Agent] is in DefaultState(EntityStateMachine)", category: "Conditions", id: "22d0d6803d67c6d7b977bf14d837f6ab")]
public partial class IsEntityDefaultStateCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    Entity entity;
    public override void OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;
    }

    public override bool IsTrue()
    {    
        return !entity.IsDead && entity.IsInState<EntityDefaultState>();
    }


}
