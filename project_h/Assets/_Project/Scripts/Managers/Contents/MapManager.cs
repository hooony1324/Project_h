using NavMeshPlus.Components;
using UnityEngine;

public enum ECellCollisionType
{
    Wall,
    CharacterWalkable,
    CameraWalkable,
}

public class MapManager
{
    public Map CurrentMap { get; private set; }
    public string MapName { get; private set; }

    public void SetMap(string mapName)
    {
        MapName = mapName;
    }
    
    public void LoadMap()
    {
        Managers.Dungeon.Clear();

        GameObject map = Managers.Resource.Instantiate(MapName);
        map.transform.position = Vector3.zero;
        map.name = $"@Map_{MapName}";

        CurrentMap = map.GetComponent<Map>();
        MapName = MapName;

        var dungeon = map.GetComponent<Dungeon>();
        Managers.Dungeon.SetDungeon(dungeon);
    }


}