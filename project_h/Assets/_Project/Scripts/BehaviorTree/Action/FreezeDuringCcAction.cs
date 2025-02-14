using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FreezeDuringCC", story: "[Agent] is freezed during CC", category: "Action", id: "dfc4071d441172bd27cd0190eb858c37")]
public partial class FreezeDuringCcAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    Entity entity;
    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        entity.Movement.Stop();
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (entity.IsInState<EntityDefaultState>())
            return Status.Success;

        return Status.Running;
    }
}

