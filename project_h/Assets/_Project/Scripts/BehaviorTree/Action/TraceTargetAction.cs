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
    Transform targetTransform;

    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        if (Managers.Hero.MainHero.IsDead)
            return Status.Failure;

        Trace();

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

        float distance = Vector2.Distance(Agent.Value.transform.position, targetTransform.position);

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

    void Trace()
    {
        if (entity.Target != null)
            targetTransform = entity.Target.transform;
        else
            targetTransform = Managers.Hero.MainHero.transform;

        entity.Movement.TraceTarget = targetTransform;
    }

}

