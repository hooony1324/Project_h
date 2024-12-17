using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequireStatCondition : EntityCondition
{
    [SerializeField]
    private Stat stat;
    [SerializeField]
    private float needValue;

    public override string Description => $"{stat.DisplayName} {needValue}";

    public override bool IsPass(Entity entity)
        => entity.StatsComponent.GetStat(stat)?.Value >= needValue;

    public override object Clone()
        => new RequireStatCondition() { stat = stat, needValue = needValue };
}
