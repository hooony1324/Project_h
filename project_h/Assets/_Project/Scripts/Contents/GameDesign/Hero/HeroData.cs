using UnityEngine;

[CreateAssetMenu(fileName = "HERO_", menuName = "GameDesign/HeroData")]
public class HeroData : EntityData
{


    #if UNITY_EDITOR
    public override string GetAssetPrefix() => "HERO";

    #endif
}