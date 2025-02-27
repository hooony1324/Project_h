using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;


[System.Serializable]
public abstract class ItemAcquireAction
{
    public abstract void AqcuireAction(Item owner);
    public virtual void Release() {}
    public virtual bool IsSpawnable => true;

    public bool IsActionType<T>() where T : ItemAcquireAction
    {
        if (this.GetType() == typeof(T))
            return true;

        return false;
    }
}

[System.Serializable]
public class SelectDefaultSkill : ItemAcquireAction
{
    [SerializeField] private EDefaultSkillSlot slotType;
    public override bool IsSpawnable => GetSkillFusionDatas(slotType).Count() > 0;

    public override void AqcuireAction(Item owner) 
    {
        var skillFusionDatas = GetSkillFusionDatas(slotType);
        
        if (skillFusionDatas.Count == 1)
        {
            Managers.Hero.ChangeDefaultSkill(skillFusionDatas[0].resultSkillID, slotType);
        }
        else
        {
            ItemSelectorInfo[] selectorInfos = skillFusionDatas.Select(data =>
            new ItemSelectorInfo
            {
                selectSkillID = data.targetSkillID,
                resultSkillID = data.resultSkillID,
            }).ToArray();

            UI_ItemSelectorPopup itemSelectorPopup = Managers.UI.ShowPopupUI<UI_ItemSelectorPopup>();
            itemSelectorPopup.Setup(selectorInfos);
        }
    }

    private IReadOnlyList<SkillFusionData> GetSkillFusionDatas(EDefaultSkillSlot slotType)
    {
        int sourceSkillID = 0;
        switch (slotType)
        {
            case EDefaultSkillSlot.DefaultAttack:
                sourceSkillID = Managers.Hero.MainHero.SkillSystem.DefaultAttack.ID;
                break;
            case EDefaultSkillSlot.Dodge:
                sourceSkillID = Managers.Hero.MainHero.SkillSystem.Dodge.ID;
                break;
        }

        return Managers.Data.GetSkillFusionDatas(sourceSkillID);
    }
}


[System.Serializable]
public class DungeonGoldAcquireAction : ItemAcquireAction
{
    public int goldStatID;
    public override void AqcuireAction(Item owner) 
    {
        Vector2Int range = Managers.Dungeon.CurrentDungeonData != null ? Managers.Dungeon.CurrentDungeonData.GoldDropRange : new Vector2Int(1, 3);
        int goldValue = Random.Range(range.x, range.y);

        Stat stat = Managers.Hero.MainHero.StatsComponent.GoldsStat;
        stat.DefaultValue += goldValue;
    }
}

[System.Serializable]
public class AddPassiveSkillAction : ItemAcquireAction
{
    public int skillID;
    public override void AqcuireAction(Item owner) 
    {
        Skill skill = Managers.Data.GetSkillData(skillID);
        Managers.Hero.MainHero.SkillSystem.RegisterWithoutCost(skill);

        Managers.Hero.PassiveSkills.Add(skillID);
    }

    public override void Release()
    {
        Skill skill = Managers.Data.GetSkillData(skillID);
        Managers.Hero.MainHero.SkillSystem.Unregister(skill);

        Managers.Hero.PassiveSkills.Remove(skillID);
    }
}


[System.Serializable]
public class EliminateMonstersAction : ItemAcquireAction
{
    // 현재 소환된 몬스터를 처치한다
    public override void AqcuireAction(Item owner) 
    {
        
    }
}
