using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TraceTarget", story: "[Agent] traces Target up to [AttackRange] by [DistanceToTarget]", category: "Action", id: "a51d3a203437abdbf3dc980daad32835")]
public partial class TraceTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> AttackRange;
    [SerializeReference] public BlackboardVariable<float> DistanceToTarget;
    Entity entity;

    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        if (Managers.Hero.MainHero.IsDead)
            return Status.Failure;

        if (entity.Target == null)
            return Status.Failure;
        
        entity.Movement.TraceTarget = entity.Target.transform;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Managers.Hero.MainHero.IsDead)
            return Status.Failure;

        if (!entity.IsInState<EntityDefaultState>())
        {
            entity.Movement.Stop();
            return Status.Failure;
        }

        float distance = DistanceToTarget.Value;
        if (distance > AttackRange.Value)
        {
            return Status.Running;
        }
        else
        {
            entity.Movement.Stop();
            return Status.Success;
        }
    }


}

