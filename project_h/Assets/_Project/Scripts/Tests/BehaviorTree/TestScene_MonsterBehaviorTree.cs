using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Behavior;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;


public class TestScene_MonsterBehaviorScene : BaseScene
{
    [UnderlineTitle("Hero")]
    [SerializeField] int heroDataID;
    [SerializeField] Transform heroSpawnPos;
    [SerializeField, ReadOnly] List<Skill> registeredHeroSkills = new();

    [Space(20)]
    [SerializeField] Skill DefaultAttackSkill;
    [SerializeField] Skill DodgeSkill;
    [SerializeField] Skill PassiveSkill;

    [Space(20)]
    [Button("RegisterHeroTestSkill")] public bool registerHeroTestSkill;
    [Button("UnregisterHeroTestSkill")] public bool unregisterHeroTestSkill;

    [Space(10)]
    [Button("ExecuteHeroTestSkill", 50, ButtonColor.Green)] public bool executeHeroTestSkill;

    Hero hero;

    
    [Space(20)]
    [UnderlineTitle("Monster")]
    [SerializeField] bool EnablePatrol = false;
    bool prevEnablePatrol;
    [SerializeField] int monsterDataID;
    [SerializeField] Transform monsterSpawnPos;
    [SerializeField, ReadOnly] List<Skill> registeredMonsterSkills = new();

    [Space(20)]
    [SerializeField] Skill monsterTestSkill;
    [Button("RegisterMonsterTestSkill")] public bool registerMosnterTestSkill;
    [Button("UnregisterMonsterTestSkill")] public bool UnregisterMosnterTestSkill;

    [Space(10)]
    [Button("ExecuteMonsterTestSkill", 50, ButtonColor.Green)] public bool executeMonsterTestSkill;

    Monster monster;

    [Space(20)]
    [UnderlineTitle("Drop Item")]
    [SerializeField] int dropTableID = -1;
    [Button("DropItem")] public bool dropItem;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

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
        Managers.Data.Init();

        await LoadAddressableAssets();

        monster = Managers.Object.Spawn<Monster>(monsterSpawnPos.position.With(z: 0));
        monster.SetData(Managers.Data.GetMonsterData(monsterDataID));

        hero = Managers.Object.Spawn<Hero>(heroSpawnPos.position.With(z: 0));
        hero.SetData(Managers.Data.GetHeroData(heroDataID));

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
        
        EventBus<TEST_HeroSpawnEvent>.Raise(new TEST_HeroSpawnEvent
        {
        });

        // joystick
        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        Managers.UI.Joystick = Managers.UI.ShowSceneUI<UI_Joystick>();

        Managers.Game.Cam.transform.position = hero.Position;
        Managers.Game.Cam.Target = hero;
        Managers.Game.PlayerController.SetControlTarget(hero);

        RegisterHeroTestSkill();
        RegisterMonsterTestSkill();
    }

    public void RegisterHeroTestSkill()
    {
        if (PassiveSkill != null)
        {
            if (hero.SkillSystem.Find(PassiveSkill) != null)
            {
                Debug.Log($"{PassiveSkill.CodeName}이미 등록어있습니다");
            }
            else
            {
                hero.SkillSystem.RegisterWithoutCost(PassiveSkill);
            }
        }

        if (DefaultAttackSkill != null)
        {
            if (hero.SkillSystem.Find(DefaultAttackSkill) != null)
            {
                Debug.Log($"{DefaultAttackSkill.CodeName}이미 등록어있습니다");
            }
            else
            {
                hero.SkillSystem.RegisterWithoutCost(DefaultAttackSkill);
                hero.SkillSystem.DefaultAttack = hero.SkillSystem.Find(DefaultAttackSkill);
            }
        }


        if (DodgeSkill != null)
        {
            if (hero.SkillSystem.Find(DodgeSkill) != null)
            {
                Debug.Log($"{DodgeSkill.CodeName}이미 등록어있습니다");
            }
            else
            {
                hero.SkillSystem.RegisterWithoutCost(DodgeSkill);
                hero.SkillSystem.Dodge = hero.SkillSystem.Find(DodgeSkill);
            }
        }

        Managers.UI.Joystick.SetupActionButtons(hero);

        registeredHeroSkills = hero.SkillSystem.OwnSkills.ToList();

        Debug.Log($"{DefaultAttackSkill?.CodeName}, {DodgeSkill?.CodeName}, {PassiveSkill?.CodeName}이 등록되었습니다");
    }

    public void UnregisterHeroTestSkill()
    {
        if (DefaultAttackSkill)
            hero.SkillSystem.Unregister(DefaultAttackSkill);
        if (DodgeSkill)
            hero.SkillSystem.Unregister(DodgeSkill);
        if (PassiveSkill)            
            hero.SkillSystem.Unregister(PassiveSkill);

        hero.SkillSystem.DefaultAttack = null;
        hero.SkillSystem.Dodge = null;

        registeredHeroSkills = hero.SkillSystem.OwnSkills.ToList();

        Debug.Log($"{DefaultAttackSkill?.CodeName}, {DodgeSkill?.CodeName}, {PassiveSkill?.CodeName}이 해제되었습니다");
    }

    public void ExecuteHeroTestSkill()
    {
        if (PassiveSkill == null)
        {
            Debug.Log("heroTestSkill에 테스트 할 스킬을 설정해주세요");
            return;
        }

        Skill skill = hero.SkillSystem.Find(PassiveSkill);
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

    public void DropItem()
    {
        Managers.Dungeon.TestDropItem(dropTableID, Vector3.zero);
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