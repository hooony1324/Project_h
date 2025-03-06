using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class HeroManager
{
    public Hero MainHero { get; private set;}

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

    #region Save&Load
    private int heroDataID;
    public int HeroDataID => heroDataID;
    public HeroData CurrentHeroData => Managers.Data.GetHeroData(HeroDataID);
    public List<StatSaveData> StatSaveDatas = new();
    public int DefaultAttackID;
    public int DodgeID;
    public List<int> PassiveSkills { get; } = new();

    // for SaveLoadManager
    public void LoadHeroData(int heroDataID) => this.heroDataID = heroDataID;
    public void LoadStatSaveData(IReadOnlyList<StatSaveData> heroSaveData)
    {
        StatSaveDatas = heroSaveData.ToList();
    }
    public void LoadSkills()
    {
        DefaultAttackID = Managers.SaveLoad.SaveData.DefaultAttackID;
        DodgeID = Managers.SaveLoad.SaveData.DodgeID;
    }

    // Dungeon1 -> Dungeon2 스탯 유지용도
    public void SaveHeroDatas()
    {
        if (MainHero == null)
            return;

        StatSaveDatas.Clear();

        // Stats
        Stat[] stats = MainHero.StatsComponent.Stats;
        foreach (Stat stat in stats)
        {
            StatSaveDatas.Add(new StatSaveData
            { 
                ID = stat.ID, 
                DefaultValue = stat.DefaultValue,
                MaxValue = stat.MaxValue,
            });
        }

        // Default Skills
        DefaultAttackID = MainHero.SkillSystem.DefaultAttack.ID;
        DodgeID = MainHero.SkillSystem.Dodge.ID;

        // Passive Skills
        // - item Acquire시 저장됨
    }

    // 저장된 데이터를 Entity에 적용
    // - Dungeon1 -> Dungeon2 스탯 유지용도
    public void LoadHeroDatas()
    {
        if (MainHero == null)
            return;

        StatsComponent stats = MainHero.StatsComponent;
        foreach (StatSaveData savedStat in StatSaveDatas)
        {
            Stat stat = stats.GetStat(savedStat.ID);
            stat.DefaultValue = savedStat.DefaultValue;
        }

        // Default Skills
        if (DefaultAttackID != 0)
            ChangeDefaultSkill(DefaultAttackID, EDefaultSkillSlot.DefaultAttack);
        if (DodgeID != 0)
            ChangeDefaultSkill(DodgeID, EDefaultSkillSlot.Dodge);

        // Passive Skills
        foreach (int passiveSkillID in PassiveSkills)
        {
            Skill skill = Managers.Data.GetSkillData(passiveSkillID);
            MainHero.SkillSystem.RegisterWithoutCost(skill);
        }
    }
    #endregion

    public void ChangeDefaultSkill(int skillID, EDefaultSkillSlot slot)
    {
        Hero hero = Managers.Hero.MainHero;
        Skill newDefaultSkill = Managers.Data.GetSkillData(skillID);

        if (hero.SkillSystem.Find(newDefaultSkill))
            return;

        switch (slot)
        {
            case EDefaultSkillSlot.DefaultAttack:
                hero.SkillSystem.Unregister(hero.SkillSystem.DefaultAttack);
                Skill newAttackSkill = hero.SkillSystem.RegisterWithoutCost(newDefaultSkill, 1);
                hero.SkillSystem.DefaultAttack = newAttackSkill;
                break;
            case EDefaultSkillSlot.Dodge:
                hero.SkillSystem.Unregister(hero.SkillSystem.Dodge);
                Skill newDodgeSkill = hero.SkillSystem.RegisterWithoutCost(newDefaultSkill, 1);
                hero.SkillSystem.Dodge = newDodgeSkill;
                break;
        }

        Managers.UI.Joystick.SetupActionButtons(hero);
    }
}