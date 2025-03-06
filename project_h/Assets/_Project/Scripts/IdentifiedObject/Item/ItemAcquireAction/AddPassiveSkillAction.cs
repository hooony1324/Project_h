using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;


[System.Serializable]
public class AddPassiveSkillAction : ItemAcquireAction
{
    public int skillID;
    public override void AqcuireAction(Item owner) 
    {
        this.owner = owner;

        Skill skill = Managers.Data.GetSkillData(skillID);
        Managers.Hero.MainHero.SkillSystem.RegisterWithoutCost(skill);

        Managers.Hero.PassiveSkills.Add(skillID);

        Managers.Inventory.AddItem(owner);
    }

    public override void Release()
    {
        Skill skill = Managers.Data.GetSkillData(skillID);
        Managers.Hero.MainHero.SkillSystem.Unregister(skill);

        Managers.Hero.PassiveSkills.Remove(skillID);
    }
}