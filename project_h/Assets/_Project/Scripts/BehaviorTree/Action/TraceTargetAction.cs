using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TraceTarget", story: "[Agent] traces Target up to [AttackRange]", category: "Action", id: "a51d3a203437abdbf3dc980daad32835")]
public partial class TraceTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> AttackRange;

    Entity entity;

    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        if (Managers.Hero.MainHero == null)
            return Status.Failure;

        entity.Movement.TraceTarget = Managers.Hero.MainHero.transform;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float distance = Vector2.Distance(Agent.Value.transform.position, Managers.Hero.MainHero.transform.position);

        if (distance > AttackRange.Value)
            return Status.Running;
        else
        {
            entity.Movement.Stop();
            return Status.Success;
        }
    }


}

