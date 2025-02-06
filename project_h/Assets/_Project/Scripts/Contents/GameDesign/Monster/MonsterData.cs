using UnityEngine;

[CreateAssetMenu(fileName = "MONSTER_", menuName = "GameDesign/MonsterData")]
public class MonsterData : EntityData
{
    public int DropGroupID;

    #if UNITY_EDITOR
    public override string GetAssetPrefix() => "MONSTER";
    #endif
}