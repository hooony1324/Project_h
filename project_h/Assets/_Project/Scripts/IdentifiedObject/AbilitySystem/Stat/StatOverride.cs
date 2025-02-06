using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatOverride
{
    [SerializeField]
    private Stat stat;
    [SerializeField]
    private bool isUseOverride;
    [SerializeField]
    private float overrideDefaultValue;
    [SerializeField]
    private float overrideMaxValue;
    public StatOverride(Stat stat)
        => this.stat = stat;


    public Stat Stat => stat;
    public bool IsUseOverride => isUseOverride;

    public Stat CreateStat()
    {
        var newStat = stat.Clone() as Stat;
        if (isUseOverride)
        {
            newStat.DefaultValue = (overrideDefaultValue == 0) ? stat.DefaultValue : overrideDefaultValue;
            newStat.MaxValue = (overrideMaxValue == 0) ? stat.MaxValue : overrideMaxValue;
        }
            
        return newStat;
    }
}
