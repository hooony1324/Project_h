using UnityEngine;

[System.Serializable]
public class StatCost : Cost
{
    [SerializeField]
    private Stat stat;
    [SerializeField]
    private StatScaleFloat value;

    public override string Description => stat.DisplayName;

    public override bool HasEnoughCost(Entity entity)
        => entity.StatsComponent.GetValue(stat) >= value.GetValue(entity.StatsComponent);

    public override void UseCost(Entity entity)
        => entity.StatsComponent.IncreaseDefaultValue(stat, -value.GetValue(entity.StatsComponent));

    public override void UseDeltaCost(Entity entity)
        => entity.StatsComponent.IncreaseDefaultValue(stat, -value.GetValue(entity.StatsComponent) * Time.deltaTime);

    public override float GetValue(Entity entity) => value.GetValue(entity.StatsComponent);

    public override object Clone()
        => new StatCost() { stat = stat, value = value };
}
