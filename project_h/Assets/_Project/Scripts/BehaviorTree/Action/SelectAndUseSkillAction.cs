using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelectAndUseSkill", story: "[Agent] selects and use skill", category: "Action", id: "539d0ab9382ee63cf34bd80112ae2d0e")]
public partial class SelectAndUseSkillAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    Entity entity;
    Skill selectedSkill;

    protected override Status OnStart()
    {
        entity = entity == null ? Agent.Value.GetComponent<Entity>() : entity;

        // select skill
        if (selectedSkill == null && entity.SkillSystem.OwnSkills.Count > 0)
        {
            var activeSkills = entity.SkillSystem.OwnSkills.Where(x => !x.IsPassive).ToArray();
            int randomIdx = Random.Range(0, activeSkills.Length);
            selectedSkill = activeSkills[randomIdx];
        }

        // Target이 있어야 Skill Use가능
        entity.Target = Managers.Hero.MainHero;

        if (selectedSkill == null)
            return Status.Failure;

        if (selectedSkill.IsUseable)
        {
            entity.Movement.Stop();
            selectedSkill.Use();
            return Status.Running;
        }
        
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (selectedSkill.IsActivated == false)
            return Status.Failure;

        if (selectedSkill.IsFinished)
        {
            selectedSkill = null;
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

