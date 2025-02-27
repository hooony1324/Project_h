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

        // Target이 있어야 Skill Use가능
        if (entity.Target == null)
            return Status.Failure;

        if (entity.Target.IsDead)
        {
            entity.Target = null;
            return Status.Failure;
        }

        if (entity.SkillSystem.IsSkillReserved)
        {
            return Status.Running;
        }

        // select skill
        if (selectedSkill == null && entity.SkillSystem.OwnSkills.Count > 0)
        {
            var activeSkills = entity.SkillSystem.OwnSkills.Where(x => !x.IsPassive).ToArray();
            var usableSkills = activeSkills.Where(x => x.IsUseable).ToArray();
            
            if (usableSkills.Length == 0)
                return Status.Failure;

            int randomIdx = Random.Range(0, usableSkills.Length);
            selectedSkill = usableSkills[randomIdx];
        }

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
        // 다른 효과에 의해 스킬 중단??
        if (!entity.IsInState<EntityDefaultState>())
        {
            selectedSkill.Cancel();
            selectedSkill = null;
            return Status.Failure;
        }

        if (selectedSkill.IsActivated == false)
        {
            selectedSkill = null;
            return Status.Failure;
        }

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

