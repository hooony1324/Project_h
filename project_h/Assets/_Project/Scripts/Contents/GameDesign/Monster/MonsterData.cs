using UnityEngine;

[CreateAssetMenu(fileName = "MONSTER_", menuName = "GameDesign/MonsterData")]
public class MonsterData : EntityData
{


    #if UNITY_EDITOR
    public override string GetAssetPrefix() => "MONSTER";
    #endif
}