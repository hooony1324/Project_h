using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HERO_", menuName = "GameDesign/HeroData")]
public class HeroData : EntityData
{


    #if UNITY_EDITOR
    public override string GetAssetPrefix() => "HERO";

    public override void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat")
            .OrderBy(x => x.ID);
        statOverrides = stats.Select(x => new StatOverride(x)).ToArray();
    }

    #endif
}