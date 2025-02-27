using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolRandomPosition", story: "[Agent] patrols randomly selected position, [SearchRange], [DistanceToTarget]", category: "Action", id: "fcaf6339de3ae3af97503a83f6a4b8bf")]
public partial class PatrolRandomPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> SearchRange;
    [SerializeReference] public BlackboardVariable<float> DistanceToTarget;
    
    Entity entity;
    float waitingDuration;
    float waitingTime;
    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        if (entity.Target != null)
            return Status.Failure;

        Vector2 nextPos = entity.Position;
        nextPos = nextPos.RandomPointInAnnulus(2, 6);

        entity.Movement.Destination = nextPos;

        waitingDuration = Random.Range(1f, 4f);
        waitingTime = 0;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!Managers.Hero.MainHero.IsDead)
        {
            if (entity.Target != null)
                return Status.Failure;

            if (DistanceToTarget.Value <= SearchRange.Value)
            {
                entity.Target = Managers.Hero.MainHero;
                return Status.Failure;
            }       
        }
        if (entity.Movement.AtDestination)
            waitingTime += Time.deltaTime;
            
        if (waitingTime >= waitingDuration)
            return Status.Success;
            
        return Status.Running;
    }

}

