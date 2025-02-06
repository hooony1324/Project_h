using UnityEngine;

[System.Serializable]
public abstract class DungeonSpawnAction : DungeonRoomAction
{
    [SerializeField] protected string spawnEffect = "CFXR3 Hit Misc F Smoke";
    [SerializeField] protected Transform spawnPoint;
}