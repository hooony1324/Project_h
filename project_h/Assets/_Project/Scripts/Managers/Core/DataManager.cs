using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    private readonly Dictionary<string /*dataName*/, MonsterData> _monsterDatas = new Dictionary<string, MonsterData>(); 
    private readonly Dictionary<string /*dataName*/, HeroData> _heroDatas = new Dictionary<string, HeroData>(); 
    private readonly List<DungeonData> _dungeonDatas = new List<DungeonData>();

    public Dictionary<string, HeroData> HeroDatas => _heroDatas;
    public Dictionary<string, MonsterData> MonsterDatas => _monsterDatas;
    
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

        foreach (DungeonData dungeonData in Resources.LoadAll<DungeonData>("GameDesign/DungeonData"))
        {
            _dungeonDatas.Add(dungeonData);
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

    public DungeonData GetDungeonData(int dungeonId)
    {
        return _dungeonDatas.FirstOrDefault(x => x.Id == dungeonId);
    }
}

