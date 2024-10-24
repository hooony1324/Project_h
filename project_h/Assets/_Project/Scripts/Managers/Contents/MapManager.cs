using System;
using System.Collections;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEditor.AI;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public enum ECellCollisionType
{
    Wall,
    CharacterWalkable,
    CameraWalkable,
}

public class MapManager
{
    public GameObject Map { get; private set; }
    public string MapName { get; private set; }
    public NavMeshSurface NavMeshSurface2D => _navMeshSurface;

    private int _minX;
    private int _maxX;
    private int _minY;
    private int _maxY;
    private NavMeshSurface _navMeshSurface;
    private IEnumerator _bakeMeshCoroutine;

    public void Init()
    {
        GameObject NavMesh = GameObject.Find("NavMesh");
        _navMeshSurface = NavMesh.GetComponent<NavMeshSurface>();
        _bakeMeshCoroutine = BakeNavMeshAsync();
    }
    public void LoadMap(string mapName, Vector3 position)
    {
        DestroyMap();

        GameObject map = Managers.Resource.Instantiate(mapName);
        map.transform.position = position;
        map.name = $"@Map_{mapName}";

        Map = map;
        MapName = mapName;

        _navMeshSurface.RemoveData();
        Managers.Coroutines.Register(_bakeMeshCoroutine);
    }

    private IEnumerator BakeNavMeshAsync()
    {
        AsyncOperation bakeMeshAsync = _navMeshSurface.BuildNavMeshAsync();

        while (!bakeMeshAsync.isDone)
        {
            yield return null;
        }

        

        // Teleport Hero
        Map dungeon = Map.GetComponent<Map>();
        Managers.Object.Hero.Movement.AgentEnabled = false;
        Managers.Object.Hero.transform.position = dungeon.StartPosition;
        Managers.Object.Hero.Movement.AgentEnabled = true;
        Managers.Coroutines.UnRegister(_bakeMeshCoroutine);
    }

    public void DestroyMap()
    {
        if (Map != null)
            Managers.Resource.Destroy(Map);
    }

}