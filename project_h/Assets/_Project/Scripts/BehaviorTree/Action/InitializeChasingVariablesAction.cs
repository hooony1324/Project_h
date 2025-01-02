using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InitializeChasingVariables", story: "[Agent] sets [SearchRange] and [AttackRange]", category: "Action", id: "3c7a79daddf845c46a2752e0f02b95e3")]
public partial class InitializeChasingVariablesAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> SearchRange;
    [SerializeReference] public BlackboardVariable<float> AttackRange;

    Entity entity;

    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        SearchRange.Value = entity.StatsComponent.SearchRangeStat.Value;
        AttackRange.Value = entity.StatsComponent.AttackRangeStat.Value;

        return Status.Success;
    }
}

