using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatScaleFloat
{
    public float defaultValue;
    public Stat scaleStat;

    // Reduce타입 Stat일 경우 편집가능
    public float reduceMinValue;

    public float GetValue(StatsComponent stats)
    {
        if (scaleStat && stats.TryGetStat(scaleStat, out var stat))
        {
            if (stat.IsPercentType)
                return defaultValue * (1 + stat.Value);
            else if (stat.IsReduceType)
            {
                float reducedValue = defaultValue - (defaultValue - reduceMinValue) / stat.MaxValue * stat.Value;
                return Mathf.Clamp(reducedValue, reduceMinValue, defaultValue);
            }
            else
                return defaultValue * stat.Value;
        }
        else
            return defaultValue;
    }
}