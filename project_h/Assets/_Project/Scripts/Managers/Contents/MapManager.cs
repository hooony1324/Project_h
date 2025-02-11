using NavMeshPlus.Components;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

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
        GameObject map = Managers.Resource.Instantiate(MapName);
        map.transform.position = Vector3.zero;
        map.name = $"@Map_{MapName}";

        CurrentMap = map.GetComponent<Map>();
        MapName = MapName;

        var dungeon = map.GetComponent<Dungeon>();
        Managers.Dungeon.Setup(dungeon);
    }

    public Vector3 GetLerpedPosition(Vector3 startPos, Vector3 destPos)
    {
        if (NavMesh.Raycast(startPos, destPos, out NavMeshHit hit, NavMesh.GetAreaFromName("Not Walkable")))
            return hit.position;

        return destPos;
    }
}