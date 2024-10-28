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
    public MapInfo Info { get; private set; }
    public string MapName { get; private set; }

    public NavMeshSurface NavMeshSurface2D => _navMeshSurface;

    private int _minX;
    private int _maxX;
    private int _minY;
    private int _maxY;
    private NavMeshSurface _navMeshSurface;
    public void Init()
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

        Info = map.GetComponent<MapInfo>();
        MapName = MapName;
    }

}