using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatScaleFloat
{
    public float defaultValue;
    public Stat scaleStat;

    // Reduce타입 Stat일 경우 편집가능
    public float resultMinValue;
    public float resultMaxValue;

    public float GetValue(StatsComponent stats)
    {
        float resultValue = defaultValue;
        if (scaleStat && stats.TryGetStat(scaleStat, out var stat))
        {
            if (stat.IsPercentType)
                resultValue = Mathf.Clamp(defaultValue * (1 + stat.Value), resultMinValue, resultMaxValue);
            else if (stat.IsReduceType)
            {
                float reducedValue = defaultValue - (defaultValue - resultMinValue) / stat.MaxValue * stat.Value;
                resultValue = Mathf.Clamp(reducedValue, resultMinValue, defaultValue);
            }
            else
                resultValue = Mathf.Clamp(defaultValue * stat.Value, resultMinValue, resultMaxValue);
        }
        
        return resultValue;            
    }
}