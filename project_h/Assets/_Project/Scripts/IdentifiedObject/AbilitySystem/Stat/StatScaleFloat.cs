using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatScaleFloat
{
    public float defaultValue;
    public Stat scaleStat;

    public float GetValue(StatsComponent stats)
    {
        if (scaleStat && stats.TryGetStat(scaleStat, out var stat))
        {
            if (stat.IsPercentType)
                return defaultValue * (1 + stat.Value);
            else if (stat.IsReduceType)
            {
                float reducedValue = defaultValue - (defaultValue - stat.ReduceMinValue) / stat.MaxValue * stat.Value;
                return Mathf.Clamp(reducedValue, stat.ReduceMinValue, defaultValue);
            }
            else
                return defaultValue * stat.Value;
        }
        else
            return defaultValue;
    }
}