using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;


[System.Serializable]
public abstract class ItemAcquireAction
{
    public abstract void AqcuireAction(Item owner);
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

// 아이템 소지 시 최대체력 +1
[System.Serializable]
public class AddStatItem : ItemAcquireAction
{
    // TODO : 다른 아이템 조합 정보 확인하는 템 생길예정
    // 게임 내에서 수치가 자주 변경될 수 있는 스탯 -> Dynamic
    // 체력회복 +1, 열쇠 +1, 코인 +1
    [SerializeField] private StatItemData[] statDatas;

    public override void AqcuireAction(Item owner) 
    {
        foreach (StatItemData data in statDatas)
        {
            // data를 적용 : temporary -> DefaultValue, 
            Stat stat = Managers.Hero.MainHero.StatsComponent.GetStat(data.id);

            switch (data.valueType)
            {
                case StatItemData.EValueType.DefaultValue:
                    stat.DefaultValue += data.value;
                    break;
                case StatItemData.EValueType.MaxValue:
                    stat.MaxValue += data.value;
                    break;
            }

        }

        
        Managers.Inventory.AddItem(owner);
    }

    public void Load()
    {
        foreach (StatItemData data in statDatas)
        {
            // data를 적용 : temporary -> DefaultValue, 
            Stat stat = Managers.Hero.MainHero.StatsComponent.GetStat(data.id);

            if (data.isDynamicValue)
                continue;

            switch (data.valueType)
            {
                case StatItemData.EValueType.DefaultValue:
                    stat.DefaultValue += data.value;
                    break;
                case StatItemData.EValueType.MaxValue:
                    stat.MaxValue += data.value;
                    break;
            }
        }
    }

    [System.Serializable]
    class StatItemData
    {   
        public int id;
        [Tooltip("isDynamicValue - '현재체력'과 같이 게임 중 변화가 자주 일어나는 값(재시작 시 해당 수치는 복구되지 않음)\nex)체력회복++(Dynamic O) 과 공격속도++(Dynamic X)인 아이템\n - 아이템을 획득 당시 체력은 회복, 공속 획득\n - 게임 재시작시 아이템 재획득하여 스탯 복구, 체력회복X, 공속복구O")]
        public bool isDynamicValue; // 재시작하고 로드, 체크되어 있다면 복구 안함
        public EValueType valueType;
        public float value;

        public enum EValueType
        {
            DefaultValue,
            MaxValue,
        }
    }
}


[System.Serializable]
public class DropGoldAction : ItemAcquireAction
{
    public int goldStatID;
    public override void AqcuireAction(Item owner) 
    {
        Vector2Int range = Managers.Dungeon.CurrentDungeonData != null ? Managers.Dungeon.CurrentDungeonData.GoldDropRange : new Vector2Int(1, 3);
        int goldValue = Random.Range(range.x, range.y);

        Stat stat = Managers.Hero.MainHero.StatsComponent.GetStat(goldStatID);
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
}


[System.Serializable]
public class EliminateMonstersAction : ItemAcquireAction
{
    // 현재 소환된 몬스터를 처치한다
    public override void AqcuireAction(Item owner) 
    {
        
    }
}
