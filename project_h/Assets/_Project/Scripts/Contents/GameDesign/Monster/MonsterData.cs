using UnityEngine;

[CreateAssetMenu(fileName = "MONSTER_", menuName = "GameDesign/MonsterData")]
public class MonsterData : EntityData
{
    public int DropTableID;

    #if UNITY_EDITOR
    public override string GetAssetPrefix() => "MONSTER";
    #endif
}