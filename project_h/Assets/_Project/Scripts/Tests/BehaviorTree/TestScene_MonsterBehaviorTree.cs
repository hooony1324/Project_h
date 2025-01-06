using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Behavior;
using UnityEditor.Rendering;
using UnityEngine;
using Object = UnityEngine.Object;


public class TestScene_MonsterBehaviorScene : BaseScene
{
    [UnderlineTitle("Hero")]
    [SerializeField] string heroDataName;
    [SerializeField] Transform heroSpawnPos;
    [SerializeField, ReadOnly] List<Skill> registeredHeroSkills = new();

    [Space(20)]
    [SerializeField] Skill heroTestSkill;
    [Button("RegisterHeroTestSkill")] public bool registerHeroTestSkill;
    [Button("UnregisterHeroTestSkill")] public bool unregisterHeroTestSkill;

    [Space(10)]
    [Button("ExecuteHeroTestSkill", 50, ButtonColor.Green)] public bool executeHeroTestSkill;

    Hero hero;

    
    [Space(20)]
    [UnderlineTitle("Monster")]
    [SerializeField] bool EnablePatrol = false;
    bool prevEnablePatrol;
    [SerializeField] string monsterDataName;
    [SerializeField] Transform monsterSpawnPos;
    [SerializeField, ReadOnly] List<Skill> registeredMonsterSkills = new();

    [Space(20)]
    [SerializeField] Skill monsterTestSkill;
    [Button("RegisterMonsterTestSkill")] public bool registerMosnterTestSkill;
    [Button("UnregisterMonsterTestSkill")] public bool UnregisterMosnterTestSkill;

    [Space(10)]
    [Button("ExecuteMonsterTestSkill", 50, ButtonColor.Green)] public bool executeMonsterTestSkill;

    Monster monster;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Data.Init();
        prevEnablePatrol = EnablePatrol;

        return true;
    }

    private async UniTask LoadAddressableAssets()
    {
        bool bResourceLoaded = false;

        Managers.Resource.LoadAllAsync<Object>("PreGame", (key, current, total) =>
        {
            if (current == total)
            {
                bResourceLoaded = true;
            }
        });

        await UniTask.WaitUntil(() => bResourceLoaded);
    }

    async void Start()
    {
        await LoadAddressableAssets();

        Managers.Data.Init();

        monster = Managers.Object.Spawn<Monster>(monsterSpawnPos.position.With(z: 0));
        monster.SetData(Managers.Data.GetMonsterData(monsterDataName));

        hero = Managers.Object.Spawn<Hero>(heroSpawnPos.position.With(z: 0));
        hero.SetData(Managers.Data.GetHeroData(heroDataName));

        var monsterSkillsToUnregister = monster.SkillSystem.OwnSkills.ToList();
        foreach (Skill monsterSkill in monsterSkillsToUnregister)
        {
            monster.SkillSystem.Unregister(monsterSkill);
        }
        registeredMonsterSkills = monster.SkillSystem.OwnSkills.ToList();
        monster.SkillSystem.RemoveEffectAll();

        var heroSkillsToUnregister = hero.SkillSystem.OwnSkills.ToList();
        foreach (Skill heroSkill in heroSkillsToUnregister)
        {
            hero.SkillSystem.Unregister(heroSkill);
        }
        registeredHeroSkills = hero.SkillSystem.OwnSkills.ToList();
        hero.SkillSystem.RemoveEffectAll();

        Managers.Hero.SetMainHero(hero);
    }

    public void RegisterHeroTestSkill()
    {
        if (hero.SkillSystem.Find(heroTestSkill) != null)
        {
            Debug.Log($"{heroTestSkill.CodeName}이미 등록어있습니다");
            return;
        }

        hero.SkillSystem.RegisterWithoutCost(heroTestSkill);
        registeredHeroSkills = hero.SkillSystem.OwnSkills.ToList();

        Debug.Log($"{heroTestSkill.CodeName}이 등록되었습니다");
    }

    public void UnregisterHeroTestSkill()
    {
        hero.SkillSystem.Unregister(heroTestSkill);
        registeredHeroSkills = hero.SkillSystem.OwnSkills.ToList();

        Debug.Log($"{heroTestSkill.CodeName}이 해제되었습니다");
    }

    public void ExecuteHeroTestSkill()
    {
        if (heroTestSkill == null)
        {
            Debug.Log("heroTestSkill에 테스트 할 스킬을 설정해주세요");
            return;
        }

        Skill skill = hero.SkillSystem.Find(heroTestSkill);
        if (skill != null && skill.IsUseable)
            skill.Use();
    }

    public void RegisterMonsterTestSkill()
    {
        if (monster.SkillSystem.Find(monsterTestSkill) != null)
        {
            Debug.Log($"{monsterTestSkill.CodeName}이미 등록어있습니다");
            return;
        }
        
        monster.SkillSystem.RegisterWithoutCost(monsterTestSkill);
        registeredMonsterSkills = monster.SkillSystem.OwnSkills.ToList();

        Debug.Log($"{monsterTestSkill.CodeName}이 등록되었습니다");
    }

    public void UnregisterMonsterTestSkill()
    {
        monster.SkillSystem.Unregister(monsterTestSkill);
        registeredMonsterSkills = monster.SkillSystem.OwnSkills.ToList();

        Debug.Log($"{monsterTestSkill.CodeName}이 해제되었습니다");
    }

    public void ExecuteMonsterTestSkill()
    {
        if (monsterTestSkill == null)
        {
            Debug.Log("monsterTestSkill에 테스트 할 스킬을 설정해주세요");
            return;
        }

        Skill skill = monster.SkillSystem.Find(monsterTestSkill);
        if (skill != null && skill.IsUseable)
            skill.Use();
    }

    public override void Clear()
    {
        throw new NotImplementedException();
    }


    void OnValidate()
    {
        if (!Application.isPlaying || EnablePatrol == prevEnablePatrol)
            return;

        BehaviorGraphAgent monsterBehavior = monster.GetComponent<BehaviorGraphAgent>();
        monsterBehavior.SetVariableValue("EnablePatrol", EnablePatrol);
        prevEnablePatrol = EnablePatrol;
    }
}