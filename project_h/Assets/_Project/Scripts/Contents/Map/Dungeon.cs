using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Tilemaps;
using UnityEditor.Localization.Plugins.XLIFF.V12;





#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dungeon : InitOnce
{
    public static readonly int TileUnit = 20;   // 타일 개수 20 x 20이 기본크기
    private int maxAttempts = 10;              // 최대 시도 횟수

    [SerializeField] int _roomWidth = 40;
    [SerializeField] int _roomHeight = 20;

    [SerializeField] int _gridSizeX = 20;
    [SerializeField] int _gridSizeY = 20;

    private List<DungeonRoom> _rooms = new List<DungeonRoom>();
    private List<Vector2Int> _openedIndexes = new List<Vector2Int>();   // 방을 설치할 수 있는 Grid 위치
    private HashSet<Vector2Int> _visitedIndexes = new HashSet<Vector2Int>();

    private int[,] _roomGrid;
    private bool _generationComplete = false;

    [Space]
    [Header("Test Spawn")]
    [ButtonAttribute("TestSpawn")]
    public bool test = false;
    public void TestSpawn()
    {
        StartCoroutine(PlaceAllRooms());
    }

    [ButtonAttribute("TestClean")]
    public bool clean = false;
    public void TestClean()
    {
        _generationComplete = false;
        _openedIndexes.Clear();
        _openedIndexes.Add(new Vector2Int(_gridSizeX / 2, _gridSizeY / 2));
        _visitedIndexes.Clear();
        _roomGrid.ClearWith(0);
        _rooms.ForEach(room => room.transform.position = Vector3.one * -100);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        GameObject rooms = Util.FindChild(gameObject, "Rooms");
        foreach (Transform child in rooms.transform)
        {
            DungeonRoom room = child.GetComponent<DungeonRoom>();
            if (room != null)
                _rooms.Add(room);
        }

        _roomGrid = new int[_gridSizeX, _gridSizeY];

        Vector2Int initialRoomIndex = new Vector2Int(_gridSizeX / 2, _gridSizeY / 2);
        _openedIndexes.Add(initialRoomIndex);

        return true;
    }

    private IEnumerator PlaceAllRooms()
    {
        List<DungeonRoom> failedRooms = new List<DungeonRoom>();
        foreach (DungeonRoom room in _rooms)
        {
            if (!TryPlaceRoom(room))
                failedRooms.Add(room);
        }

        foreach (DungeonRoom room in failedRooms)
        {
            if (!ForcePlaceRoom(room))
                Debug.Log("최종 방 배치 실패, Grid를 더 늘려야 합니다.");
        }

        Debug.Log("Place Rooms Completed!");
        _generationComplete = true;
        yield return null;
    }

    private bool TryPlaceRoom(DungeonRoom dungeonRoom)
    {
        if (_openedIndexes.Count == 0) 
            return false;

        int openedIndex = Random.Range(0, _openedIndexes.Count);
        Vector2Int chosenIndex = _openedIndexes[openedIndex];

        if (!CanPlaceRoom(dungeonRoom, chosenIndex))
            return false;

        // 소환 예정 Index는 등록 해제
        _openedIndexes.RemoveAt(openedIndex); 
        _openedIndexes.Shuffle();

        // 겹치지 않으면 배치
        PlaceRoom(dungeonRoom, chosenIndex);

        return true;
    }

    private bool ForcePlaceRoom(DungeonRoom dungeonRoom)
    {
        if (_openedIndexes.Count == 0) 
            return false;

        foreach (Vector2Int openedIndex in _openedIndexes)
        {
            if (!CanPlaceRoom(dungeonRoom, openedIndex))
                continue;
            
            _openedIndexes.Remove(openedIndex); 
            PlaceRoom(dungeonRoom, openedIndex);
            return true;
        }

        return false;
    }

    private bool CanPlaceRoom(DungeonRoom dungeonRoom, Vector2Int gridIndex)
    {
        Vector2Int[] patternOffsets = dungeonRoom.GetPatternIndexes();
        foreach (var offset in patternOffsets)
        {
            Vector2Int gridPosition = gridIndex + new Vector2Int(offset.x, offset.y);

            if (!IsIndexValid(gridPosition))
            {
                Debug.LogWarning($"{dungeonRoom.name}의 일부가 그리드 범위를 벗어납니다.");
                return false;
            }

            if (_roomGrid[gridPosition.x, gridPosition.y] == 1)
            {
                Debug.LogWarning($"{dungeonRoom.name} 위치가 다른 방과 겹칩니다.");
                return false;
            }
        }

        return true;
    }

    private void PlaceRoom(DungeonRoom dungeonRoom, Vector2Int chosenIndex)
    {
        Vector2Int[] patternOffsets = dungeonRoom.GetPatternIndexes();
        foreach (var offset in patternOffsets)
        {
            Vector2Int gridPosition = chosenIndex + new Vector2Int(offset.x, offset.y);
            _roomGrid[gridPosition.x, gridPosition.y] = 1; // 방이 차지하는 그리드 셀을 표시
            AddAdjacentIndexes(gridPosition);
        }

        // 실제 방 위치 설정
        dungeonRoom.transform.position = GetIsometricPosition(chosenIndex);
        dungeonRoom.SpawnedIndex = chosenIndex; // 방의 소환된 인덱스 설정
    }

    private void AddAdjacentIndexes(Vector2Int gridIndex)
    {
        // 주변 인덱스(상하좌우)를 추가
        Vector2Int[] adjacentOffsets = {
            new Vector2Int(-1, 0), new Vector2Int(1, 0),   // 좌우
            new Vector2Int(0, -1), new Vector2Int(0, 1)    // 상하
        };

        foreach (var offset in adjacentOffsets)
        {
            Vector2Int adjacentIndex = gridIndex + offset;

            if (_visitedIndexes.Contains(adjacentIndex))
                continue;

            if (adjacentIndex.x < 0 || adjacentIndex.x >= _gridSizeX ||
                adjacentIndex.y < 0 || adjacentIndex.y >= _gridSizeY)
                continue;

            if (_roomGrid[adjacentIndex.x, adjacentIndex.y] != 0)
                continue;

            // 범위 내에 있고, 아직 방이 설치되지 않은 위치만 추가
            _openedIndexes.Add(adjacentIndex);
            _visitedIndexes.Add(adjacentIndex);
            
        }
    }

    private int CountAdjacentRooms(Vector2Int gridIndex)
    {
        int x = gridIndex.x;
        int y = gridIndex.y;
        int count = 0;

        if (x > 0 && _roomGrid[x - 1, y] != 0)
            count++;
        if (x <  _gridSizeX - 1 && _roomGrid[x + 1, y] != 0)
            count++;
        if (y > 0 && _roomGrid[x, y - 1] != 0)
            count++;
        if (y < _gridSizeY - 1 && _roomGrid[x, y + 1] != 0)
            count++;

        return count;
    }

    private bool IsIndexValid(Vector2Int gridIndex)
    {
        bool isRangeValid = gridIndex.x >= 0 && gridIndex.x < _gridSizeX && gridIndex.y >= 0 && gridIndex.y < _gridSizeY;
        if (!isRangeValid)
            return false;

        bool isGridValid = _roomGrid[gridIndex.x, gridIndex.y] != 1;
        return isGridValid;
    }

    private Vector2Int GetGridIndexFromPosition(Vector3 worldPosition)
    {
        // 등각 좌표계에서 그리드 인덱스로 변환
        float gridX = (worldPosition.x / _roomWidth + worldPosition.y / _roomHeight) / 2f;
        float gridY = (worldPosition.y / _roomHeight - worldPosition.x / _roomWidth) / 2f;

        return new Vector2Int(Mathf.RoundToInt(gridX), Mathf.RoundToInt(gridY));
    }

    private Vector3 GetIsometricPosition(Vector2Int gridIndex)
    {
        int x = gridIndex.x;
        int y = gridIndex.y;
        float isoX = (x - y) * _roomWidth * 0.5f;
        float isoY = (x + y) * _roomHeight * 0.5f;
        return new Vector3(isoX, isoY, 0);
    }
    
    private Vector2Int GetGridIndex(Vector3 position)
    {
        float isoX = position.x;
        float isoY = position.y;

        int x = Mathf.RoundToInt((isoY / _roomHeight) + (isoX / _roomWidth));
        int y = Mathf.RoundToInt((isoY / _roomHeight) - (isoX /_roomWidth));

        return new Vector2Int(x, y);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeX; y++)
            {
                Vector3 position = GetIsometricPosition(new Vector2Int(x, y));
                DrawIsometricCell(position);
                DrawRoomLabel(position, $"({x}, {y})");
            }
        }
    }
    private void DrawIsometricCell(Vector3 center)
    {
        Gizmos.color = new Color(0, 1, 1, 0.2f);

        Vector2Int gridIndex = GetGridIndex(center);
        if (_generationComplete && _roomGrid[gridIndex.x, gridIndex.y] == 1)
            Gizmos.color = new Color(1, 0, 0, 0.2f);            

        // 등각 격자의 다이아몬드 형태 네 꼭짓점 계산
        Vector3 top = center + new Vector3(0, _roomHeight * 0.5f, 0);
        Vector3 bottom = center + new Vector3(0, -_roomHeight * 0.5f, 0);
        Vector3 left = center + new Vector3(-_roomWidth * 0.5f, 0, 0);
        Vector3 right = center + new Vector3(_roomWidth * 0.5f, 0, 0);

        // Gizmos로 다이아몬드 모양의 셀 그리기
        Gizmos.DrawLine(top, right);
        Gizmos.DrawLine(right, bottom);
        Gizmos.DrawLine(bottom, left);
        Gizmos.DrawLine(left, top);
    }

    private void DrawRoomLabel(Vector3 position, string text)
    {
        Handles.Label(position, text);
    }

    #endif
}