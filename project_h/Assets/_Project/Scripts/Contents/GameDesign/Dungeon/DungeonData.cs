using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DUNGEON_", menuName = "GameDesign/DungeonData")]
public class DungeonData : ScriptableObject, ICloneable
{
    /// <summary> Id가 0이면 던전 없음을 의미 </summary>
    [SerializeField] private int _id;
    [SerializeField] private int _prevDungeonId;
    [SerializeField] private int _nextDungeonId;
    [SerializeField] private bool _isFinalDungeon;
    public int Id => _id;
    public int PrevDungeonId => _prevDungeonId;
    public int NextDungeonId => _nextDungeonId;


    [SerializeField] private string _prefabName;
    [SerializeField] private string _dungeonName;
    public string PrefabName => _prefabName;
    public string DungeonName => _dungeonName;


    public bool HasNextDungeon => _nextDungeonId != 0;
    public bool HasPrevDungeon => _prevDungeonId != 0;
    public bool IsFinalDungeon => _isFinalDungeon;

    public virtual object Clone() => Instantiate(this);
}