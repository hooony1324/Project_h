using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Define;

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

    private int _minX;
    private int _maxX;
    private int _minY;
    private int _maxY;


    public void LoadMap(string mapName)
    {
        DestroyMap();

        GameObject map = Managers.Resource.Instantiate(mapName);
        map.transform.position = Vector3.zero;
        map.name = $"@Map_{mapName}";

        Map = map;
        MapName = mapName;

        
    }

    public void DestroyMap()
    {
        if (Map != null)
            Managers.Resource.Destroy(Map);
    }
}