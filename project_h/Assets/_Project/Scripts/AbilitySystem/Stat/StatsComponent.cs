using System;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class StatsComponent : MonoBehaviour
{
    [Space]
    [SerializeField]
    private StatOverride[] statOverrides;
    private Stat[] stats;

    public Stat HPStat => hpStat;
    public Stat MoveSpeedStat => moveSpeedStat;
    public Stat SearchRangeStat => searchRangeStat;
    public Stat AttackRangeStat => attackRangeStat;
    public Stat[] Stats => stats;

    [SerializeField, ReadOnly] private Stat hpStat;
    [SerializeField, ReadOnly] private Stat moveSpeedStat;
    [SerializeField, ReadOnly] private Stat searchRangeStat;
    [SerializeField, ReadOnly] private Stat attackRangeStat;

    public Entity Owner { get; private set; }

    public virtual void Setup(Entity interactionObject, StatOverride[] statOverrides)
    {
        Owner = interactionObject;

        stats = statOverrides.Select(x => x.CreateStat()).ToArray();

        hpStat =  GetStat("HP");
        moveSpeedStat = GetStat("MOVE_SPEED");
        searchRangeStat = GetStat("RANGE_SEARCH");
        attackRangeStat = GetStat("RANGE_ATTACK");
    }

    private void OnDestroy()
    {
        foreach (var stat in stats)
            Destroy(stat);
        stats = null;
    }

    public Stat GetStat(Stat stat)
    {
        Debug.Assert(stat != null, $"Stats::GetStat - stat은 null이 될 수 없습니다.");
        return stats.FirstOrDefault(x => x.ID == stat.ID);
    }

    public Stat GetStat(int id)
    {
        return stats.FirstOrDefault(x => x.ID == id);
    }

    // Try Use GetStat(int)
    public Stat GetStat(string codeName)
    {
        Debug.Assert(codeName != null, "Stats::GetStat - stat의 codeName은 null이 될 수 없습니다.");
        return stats.FirstOrDefault(x => x.CodeName == codeName);
    }

    public bool TryGetStat(Stat stat, out Stat outStat)
    {
        Debug.Assert(stat != null, $"Stats::TryGetStat - stat은 null이 될 수 없습니다.");

        outStat = stats.FirstOrDefault(x => x.ID == stat.ID);
        return outStat != null;
    }

    public float GetValue(Stat stat)
        => GetStat(stat).Value;

    public bool HasStat(Stat stat)
    {
        Debug.Assert(stat != null, $"Stats::HasStat - stat은 null이 될 수 없습니다.");
        return stats.Any(x => x.ID == stat.ID);
    }


    public void SetDefaultValue(Stat stat, float value)
        => GetStat(stat).DefaultValue = value;

    public float GetDefaultValue(Stat stat)
        => GetStat(stat).DefaultValue;

    public void IncreaseDefaultValue(Stat stat, float value)
        => GetStat(stat).DefaultValue += value;

    public void SetBonusValue(Stat stat, object key, float value)
        => GetStat(stat).SetBonusValue(key, value);
    public void SetBonusValue(Stat stat, object key, object subKey, float value)
        => GetStat(stat).SetBonusValue(key, subKey, value);

    public float GetBonusValue(Stat stat)
        => GetStat(stat).BonusValue;
    public float GetBonusValue(Stat stat, object key)
        => GetStat(stat).GetBonusValue(key);
    public float GetBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).GetBonusValue(key, subKey);
    
    public void RemoveBonusValue(Stat stat, object key)
        => GetStat(stat).RemoveBonusValue(key);
    public void RemoveBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).RemoveBonusValue(key, subKey);

    public bool ContainsBonusValue(Stat stat, object key)
        => GetStat(stat).ContainsBonusValue(key);
    public bool ContainsBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).ContainsBonusValue(key, subKey);

}