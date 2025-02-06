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
        
        // 기존 override 값들을 Dictionary에 저장
        var existingOverrides = statOverrides?
            .Where(x => x.IsUseOverride)
            .ToDictionary(x => x.Stat.ID);
        
        // 새로운 StatOverride 배열 생성
        statOverrides = stats.Select(x => {
            // 기존에 override 되어있던 stat인 경우 해당 값을 유지
            if (existingOverrides != null && existingOverrides.TryGetValue(x.ID, out var existingOverride))
            {
                return existingOverride;
            }
            return new StatOverride(x);
        }).ToArray();
    }

    #endif
}