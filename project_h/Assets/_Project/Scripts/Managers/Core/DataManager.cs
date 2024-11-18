using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private Dictionary<string /*dataName*/, MonsterData> _monsterDatas = new Dictionary<string, MonsterData>(); 
    private Dictionary<string /*dataName*/, HeroData> _heroDatas = new Dictionary<string, HeroData>(); 
    public void Init()
    {
        foreach (MonsterData monsterData in Resources.LoadAll<MonsterData>("GameDesign/MonsterData"))
        {
            if (!_monsterDatas.ContainsKey(monsterData.name))
                _monsterDatas.Add(monsterData.name, monsterData);
        }

        foreach (HeroData heroData in Resources.LoadAll<HeroData>("GameDesign/HeroData"))
        {
            if (!_heroDatas.ContainsKey(heroData.name))
                _heroDatas.Add(heroData.name, heroData);
        }
    }

    public MonsterData GetMonsterData(string dataName)
    {
        _monsterDatas.TryGetValue(dataName, out var monsterData);
        return monsterData;
    }

    public HeroData GetHeroData(string dataName)
    {
        _heroDatas.TryGetValue(dataName, out var heroData);
        return heroData;
    }

}

