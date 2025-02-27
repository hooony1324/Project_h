using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsTargetFound", story: "[Agent] has Target to Attack", category: "Conditions", id: "75def9aae540b22f320bb324d1cb64ba")]
public partial class IsTargetFoundCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    Entity entity;
    public override void OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;
    }

    public override bool IsTrue()
    {
        return entity.Target != null;
    }
}
