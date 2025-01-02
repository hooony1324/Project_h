using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateDistanceToTarget", story: "[Agent] calculates [DistanceToTarget]", category: "Action", id: "b2b2bd389ab6736b2b59d638c0a1d047")]
public partial class UpdateDistanceToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> DistanceToTarget;

    Entity entity;

    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        float distanceToTarget = 999;
        if (Managers.Hero.MainHero != null)
            distanceToTarget = Vector2.Distance(entity.Position, Managers.Hero.MainHero.Position);

        DistanceToTarget.Value = distanceToTarget;
        return Status.Success;
    }
}

