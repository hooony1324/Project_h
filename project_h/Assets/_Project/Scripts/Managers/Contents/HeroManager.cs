using System.Linq;
using UnityEngine;

public class HeroManager
{
    public Hero MainHero { get; private set;}

    private string _heroDataName;
    public string HeroDataName => _heroDataName;
    public void SetHeroData(string heroDataName) => _heroDataName = heroDataName;
    public HeroData CurrentHeroData => Managers.Data.GetHeroData(HeroDataName);

    public StatSaveData[] StatSaveDatas { get; private set;}

    public void SetMainHero(Hero hero)
    {
        MainHero = hero;
    }

    public void TeleportHero(Vector3 position)
    {
        MainHero.Movement.AgentEnabled = false;
        MainHero.transform.position = position;

        MainHero.Movement.AgentEnabled = true;
    }

    // 던전 1 플레이 중 게임 중단 > 재시작 > 던전1 의 시작 시점 HeroStats 복구용
    public void LoadStatSaveData(StatSaveData[] heroSaveData)
    {
        StatSaveDatas = heroSaveData.ToArray();
    }

    // Dungeon1 -> Dungeon2 스탯 유지용도
    public void SaveStats()
    {
        Stat[] stats = MainHero.StatsComponent.Stats;
        StatSaveDatas = new StatSaveData[stats.Count()];
        int i = 0;
        foreach (Stat stat in stats)
        {
            StatSaveDatas[i] = new StatSaveData{ ID = stat.ID, DefaultValue = stat.DefaultValue };
            i++;   
        }
    }

    // Dungeon1 -> Dungeon2 스탯 유지용도
    public void LoadStats()
    {
        StatsComponent stats = MainHero.StatsComponent;
        foreach (StatSaveData savedStat in StatSaveDatas)
        {
            Stat stat = stats.GetStat(savedStat.ID);
            stat.DefaultValue = savedStat.DefaultValue;
        }
    }
}