using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DUNGEON_", menuName = "GameDesign/DungeonData")]
public class DungeonData : ScriptableObject, ICloneable
{
    [SerializeField] private string _prefabName;
    [SerializeField] private string _dungeonName;

    // 보상, 시간, 소횐되는 몬스터, 획득 가능 아이템, 추천 레벨등 정보

    public string PrefabName => _prefabName;
    public string DungeonName => _dungeonName;

    public virtual object Clone() => Instantiate(this);
}