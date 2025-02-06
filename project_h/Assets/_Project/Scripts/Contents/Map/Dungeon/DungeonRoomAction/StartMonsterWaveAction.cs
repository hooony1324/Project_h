using UnityEngine;

[System.Serializable]
public class StartMonsterWaveAction : DungeonRoomAction
{
    public override void Apply(DungeonRoom dungeonRoom)
    {
        _ = dungeonRoom.StartWave();
    }
}