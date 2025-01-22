using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    private readonly Dictionary<int, MonsterData> _monsterDatas = new Dictionary<int, MonsterData>(); 
    private readonly Dictionary<int, HeroData> _heroDatas = new Dictionary<int, HeroData>(); 
    private readonly List<DungeonData> _dungeonDatas = new List<DungeonData>();
    private readonly List<Skill> _skillDatas = new List<Skill>();
    private readonly List<Stat> _statDatas = new List<Stat>();
    private readonly Dictionary<int, List<DropData>> _dropTableDatas = new();
    private readonly List<Item> _itemDatas = new();
    private readonly Dictionary<int, SkillFusionData[]> _skillFusionDatas = new Dictionary<int, SkillFusionData[]>();

    public IReadOnlyDictionary<int, MonsterData> MonsterDatas => _monsterDatas;
    public IReadOnlyDictionary<int, HeroData> HeroDatas => _heroDatas;
    public IReadOnlyList<Skill> SkillDatas => _skillDatas;
    public IReadOnlyList<Stat> StatDatas => _statDatas;
    public IReadOnlyDictionary<int, List<DropData>> DropTableDatas => _dropTableDatas;
    public IReadOnlyList<Item> ItemDatas => _itemDatas;
    public IReadOnlyDictionary<int, SkillFusionData[]> SkillFusionDatas => _skillFusionDatas;
    
    public void Init()
    {
        foreach (MonsterData monsterData in Resources.LoadAll<MonsterData>("GameDesign/MonsterData"))
        {
            if (!_monsterDatas.ContainsKey(monsterData.ID))
                _monsterDatas.Add(monsterData.ID, monsterData);
        }

        foreach (HeroData heroData in Resources.LoadAll<HeroData>("GameDesign/HeroData"))
        {
            if (!_heroDatas.ContainsKey(heroData.ID))
                _heroDatas.Add(heroData.ID, heroData);
        }

        foreach (DungeonData dungeonData in Resources.LoadAll<DungeonData>("GameDesign/DungeonData"))
        {
            _dungeonDatas.Add(dungeonData);
        }

        foreach (Skill skill in Resources.LoadAll<Skill>("Skill"))
        {
            _skillDatas.Add(skill);
        }

        foreach (DropTableData droptableData in Resources.LoadAll<DropTableData>("DropTableData"))
        {
            if (!_dropTableDatas.ContainsKey(droptableData.ID))
            {
                _dropTableDatas.Add(droptableData.ID, droptableData.DropList);
            }
        }

        foreach (Item item in Resources.LoadAll<Item>("Item"))
        {
            _itemDatas.Add(item);
        }

        foreach (var skillFusionData in Resources.LoadAll<DefaultSkillFusionData>("DefaultSkillFusionData"))
        {
            _skillFusionDatas.Add(skillFusionData.sourceSkillID, skillFusionData.fusionDatas);
        }
    }

    public MonsterData GetMonsterData(int id)
    {
        _monsterDatas.TryGetValue(id, out var monsterData);
        return monsterData;
    }

    public HeroData GetHeroData(int id)
    {
        _heroDatas.TryGetValue(id, out var heroData);
        return heroData;

    }

    public DungeonData GetDungeonData(int dungeonId)
    {
        return _dungeonDatas.FirstOrDefault(x => x.Id == dungeonId);
    }

    public Skill GetSkillData(string codeName)
    {
        return _skillDatas.FirstOrDefault(x => x.CodeName == codeName);
    }

    public Skill GetSkillData(int id)
    {
        return _skillDatas.FirstOrDefault(x => x.ID == id);
    }

    public Item GetItemData(int id)
    {
        return _itemDatas.FirstOrDefault(x => x.ID == id);
    }

    public Stat GetStatData(int id)
    {
        return _statDatas.FirstOrDefault(x => x.ID == id);
    }

    public IReadOnlyList<SkillFusionData> GetSkillFusionDatas(int id)
    {
        if (_skillFusionDatas.TryGetValue(id, out var fusionData))
        {
            return Array.AsReadOnly(fusionData);
        }
        
        return Array.Empty<SkillFusionData>();
    }


    #if UNITY_EDITOR

    // 테스트용 사용 지양
    public MonsterData GetMonsterData(string monsterName)
    {
        return _monsterDatas.FirstOrDefault(x => x.Value.EntityName == monsterName).Value;
    }

    public HeroData GetHeroData(string heroName)
    {
        return _heroDatas.FirstOrDefault(x => x.Value.EntityName == heroName).Value;
    }
    #endif

}

