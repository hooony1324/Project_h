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

    public NavMeshSurface NavMeshSurface2D => _navMeshSurface;
    private NavMeshSurface _navMeshSurface;
    public void SetNavMesh()
    {
        GameObject navMesh = Managers.Resource.Instantiate("NavMesh");
        navMesh.name = "@NavMesh";
        _navMeshSurface = navMesh.GetComponent<NavMeshSurface>();
    }

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
    }


}