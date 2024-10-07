using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Stats : MonoBehaviour
{
    [Space]
    [SerializeField]
    private StatOverride[] statOverrides;
    private Stat[] stats;

    [SerializeField] private Stat hpStat;
    [SerializeField] private Stat moveSpeedStat;
    [SerializeField] private Stat levelStat;
    [SerializeField] private Stat searchRangeStat;
    [SerializeField] private Stat attackRangeStat;
    public Stat HPStat { get; private set; }
    public Stat MoveSpeedStat { get; private set; }
    public Stat LevelStat { get; private set; }
    public Stat SearchRangeStat { get; private set; }
    public Stat AttackRangeStat { get; private set; }

    public Entity Owner { get; private set; }

    public virtual void Setup(Entity interactionObject)
    {
        Owner = interactionObject;
        stats = statOverrides.Select(x => x.CreateStat()).ToArray();

        HPStat = hpStat ? GetStat(hpStat) : null;
        MoveSpeedStat = moveSpeedStat ? GetStat(moveSpeedStat) : null;
        LevelStat = levelStat ? GetStat(levelStat) : null;
        SearchRangeStat = searchRangeStat ? GetStat(searchRangeStat) : null;
        AttackRangeStat = attackRangeStat ? GetStat(attackRangeStat) : null;
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

#if UNITY_EDITOR
    [ContextMenu("LoadStats")]
    private void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat").OrderBy(x => x.ID);
        statOverrides = stats.Select(x => new StatOverride(x)).ToArray();
    }
#endif

}