using System;
using System.Linq;


[Serializable]
public class DungeonPlaySaveData
{
    public int ProgressedDungeonId;   // 0아니면 던전 진행 중 종료
    public string HeroDataName;
    public StatSaveData[] StatSaveDatas;

    public void Save()
    {
        ProgressedDungeonId = Managers.Dungeon.CurrentDungeonId;
        HeroDataName = Managers.Hero.HeroDataName;

        if (Managers.Hero.MainHero != null)
        {
            var stats = Managers.Hero.StatSaveDatas;
            StatSaveDatas = stats.Select(x => new StatSaveData { ID = x.ID, DefaultValue = x.DefaultValue }).ToArray();    
        }
        
    }

    public void Load(DungeonPlaySaveData dungeonPlaySaveData)
    {
        ProgressedDungeonId = dungeonPlaySaveData.ProgressedDungeonId;
        HeroDataName = dungeonPlaySaveData.HeroDataName;
        
        if (ProgressedDungeonId != 0)
        {
            DungeonData currentDungeonData = Managers.Data.GetDungeonData(ProgressedDungeonId);
            Managers.Dungeon.SetFirstDungeon(currentDungeonData);            
        }

        if (!string.IsNullOrEmpty(HeroDataName))
        {
            Managers.Hero.SetHeroData(HeroDataName);
        }

        Managers.Hero.LoadStatSaveData(dungeonPlaySaveData.StatSaveDatas);
    }
}

[Serializable]
public class StatSaveData
{
    public int ID;
    public float DefaultValue;
}