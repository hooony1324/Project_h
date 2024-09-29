#if UNITY_EDITOR

using Sirenix.OdinInspector;

[System.Serializable]
[InlineProperty]
public struct StatScaleFloat
{
    [HorizontalGroup("Row"), HideLabel]
    public float defaultValue;

    [HorizontalGroup("Row"), HideLabel]
    public Stat scaleStat;

    public float GetValue(Stats stats)
    {
        if (scaleStat && stats.TryGetStat(scaleStat, out var stat))
            return defaultValue * (1 + stat.Value);
        else
            return defaultValue;
    }
}

#endif