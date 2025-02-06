using UnityEngine;


[System.Serializable]
public abstract class IncreaseStatItemAction : ItemAcquireAction
{
    abstract public void Load();
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



[System.Serializable]
public class AddStatItem : IncreaseStatItemAction
{
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

    public override void Load()
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


}


[System.Serializable]
public class MultiplyStatItem : IncreaseStatItemAction
{
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
                    stat.DefaultValue *= data.value;
                    break;
                case StatItemData.EValueType.MaxValue:
                    stat.MaxValue *= data.value;
                    break;
            }

        }
  
        Managers.Inventory.AddItem(owner);
    }

    public override void Load()
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
                    stat.DefaultValue *= data.value;
                    break;
                case StatItemData.EValueType.MaxValue:
                    stat.MaxValue *= data.value;
                    break;
            }
        }
    }
}
