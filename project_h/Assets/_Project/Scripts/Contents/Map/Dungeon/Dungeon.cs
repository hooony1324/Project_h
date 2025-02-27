using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using static Define;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dungeon : InitOnce
{
    public static readonly int TileUnit = 20; // Grid한칸 안에 들어갈 타일 개수 : TileUnit * TileUnit 개

    private static readonly int TileSize = 1; // 타일 1개의 World 크기
    private static readonly int TileWorldSize = TileUnit * TileSize;

    private static readonly int _roomWidth = TileWorldSize * 2;
    private static readonly int _roomHeight = TileWorldSize;
    private static readonly int _doorOffsetX = TileWorldSize / 2;
    private static readonly int _doorOffsetY = TileWorldSize / 4;

    [SerializeField] int _gridSizeX = 20;
    [SerializeField] int _gridSizeY = 20;
    private DungeonRoom _startRoom;
    private DungeonRoom _bossRoom;

    private List<DungeonRoom> _rooms = new List<DungeonRoom>();
    private List<Vector2Int> _openedIndexes = new List<Vector2Int>();   // 방을 설치할 수 있는 Grid 위치
    private HashSet<Vector2Int> _visitedIndexes = new HashSet<Vector2Int>();

    private int[,] _roomGrid;
    private bool _generationComplete = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _roomGrid = new int[_gridSizeX, _gridSizeY];

        GameObject rooms = Util.FindChild(gameObject, "Rooms");
        foreach (Transform child in rooms.transform)
        {
            DungeonRoom room = child.GetComponent<DungeonRoom>();
            if (room != null)
                _rooms.Add(room);
        }

        _startRoom = _rooms.First();
        _bossRoom = _rooms.Find(x => x.GetComponent<DungeonRoom>().RoomType == EDungeonRoomType.BossMonster);
        if (_bossRoom != null)
        {
            _bossRoom.transform.SetAsLastSibling();
        }

        Vector2Int initialRoomIndex = new Vector2Int(_gridSizeX / 2, _gridSizeY / 2);
        _openedIndexes.Add(initialRoomIndex);

        return true;
    }

    public async Awaitable GenerateDungeon()
    {
        PlaceRoomsRandomly();
        GenerateDoors();
        InitMonsterWaves();
        await Managers.Map.CurrentMap.NavMeshSurface2D.BuildNavMeshAsync();
    }


    #region Room Generation

    private void PlaceRoomsRandomly()
    {       
        List<DungeonRoom> failedRooms = new List<DungeonRoom>();

        // 바로 배치 가능한 방은 빠르게 생성
        foreach (DungeonRoom room in _rooms)
        {
            if (room.RoomType == EDungeonRoomType.BossMonster)
            {
                failedRooms.Add(room);
                continue;
            }
            
            if (!TryPlaceRoom(room))
                failedRooms.Add(room);
        }

        // 한번 배치 실패했던 방들 다시 생성시도
        foreach (DungeonRoom room in failedRooms)
        {
            if (!ForcePlaceRoom(room))
                Debug.Log("최종 방 배치 실패, Grid를 더 늘려야 합니다.");
        }

        Debug.Log("Place Rooms Completed!");
        _generationComplete = true;
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

            // 방이 차지하는 그리드 셀을 표시
            _roomGrid[gridPosition.x, gridPosition.y] = 1;

            // 다음 방 설치가능한 그리드 셀들을 등록
            RegisterAdjacentIndexes(gridPosition);
        }

        // 실제 방 위치 설정
        dungeonRoom.transform.position = GetIsometricPosition(chosenIndex);
        dungeonRoom.SpawnedIndex = chosenIndex; // 방의 소환된 인덱스 설정
    }

    private void RegisterAdjacentIndexes(Vector2Int gridIndex)
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
    #endregion

    #region Door Generation with MST

    void InitMonsterWaves()
    {
        foreach (var room in _rooms)
        {
            room.InitMonsterWaves();
        }
    }

    // 인접한 방들 끼리만 edge생성 > Kruskal 알고리즘으로 선택
    private List<Edge> _edges = new List<Edge>();
    private Dictionary<DungeonRoom, DungeonRoom> _rootMap = new Dictionary<DungeonRoom, DungeonRoom>();

    private void GenerateDoors()
    {
        CreateEdges();
        InitializeUnionFind();

        foreach (var edge in _edges)
        {
            DungeonRoom roomA = edge.RoomA;
            DungeonRoom roomB = edge.RoomB;

            if (FindRoot(roomA) != FindRoot(roomB))
            {
                Union(roomA, roomB);
                PlaceDoorsBetweenRooms(roomA, roomB);
            }
        }

        _rooms.First().GetComponent<DungeonRoom>().UnlockRoom();
    }

    private void CreateEdges()
    {
        _edges.Clear();

        for (int i = 0; i < _rooms.Count; i++)
        {
            for (int j = i + 1; j < _rooms.Count; j++)
            {
                DungeonRoom roomA = _rooms[i];
                DungeonRoom roomB = _rooms[j];

                if (IsAdjacent(roomA, roomB))
                {
                    float distance = Vector3.Distance(roomA.transform.position, roomB.transform.position);
                    _edges.Add(new Edge(roomA, roomB, distance));
                }
            }
        }

        _edges.Sort((edge1, edge2) => edge1.Distance.CompareTo(edge2.Distance));
    }

    private bool IsAdjacent(DungeonRoom roomA, DungeonRoom roomB)
    {
        Vector2Int[] gridA = roomA.SpawnedGrid;
        Vector2Int[] gridB = roomB.SpawnedGrid;

        foreach (var posA in gridA)
        {
            foreach (var posB in gridB)
            {
                if (Vector2Int.Distance(posA, posB) == 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PlaceDoorsBetweenRooms(DungeonRoom roomA, DungeonRoom roomB)
    {
        Vector2Int[] gridA = roomA.SpawnedGrid;
        Vector2Int[] gridB = roomB.SpawnedGrid;

        // 두 방 사이 인접한 Grid를 찾음(무조건 1개 이상)
        int indexA = 0;
        int indexB = 0;
        for (int i = 0; i < gridA.Length; i++)
        {
            for (int j = 0; j < gridB.Length; j++)
            {
                if (Vector2Int.Distance(gridA[i], gridB[j]) == 1)
                {
                    indexA = i;
                    indexB = j;
                    break;
                }
            }
        }
        
        // doorDir -> 상/하/좌/우 중 하나
        Vector2Int doorADir = gridB[indexB] - gridA[indexA];
        Vector2Int doorBDir = gridA[indexA] - gridB[indexB];

        // Grid의 중심 Position + Door 방향에 따른 Position
        Vector3 gridAPos = GetIsometricPosition(gridA[indexA]);
        Vector3 gridBPos = GetIsometricPosition(gridB[indexB]);

        Vector3 doorAOffset = GetDoorVector(doorADir, 2);
        Vector3 doorBOffset = GetDoorVector(doorBDir, 2);

        Vector3 doorAPos = gridAPos + doorAOffset;
        Vector3 doorBPos = gridBPos + doorBOffset;

        DungeonDoor doorA = roomA.SpawnDoor(doorAPos);
        DungeonDoor doorB = roomB.SpawnDoor(doorBPos);

        doorA.SetInfo(roomA, roomB, doorBPos, doorB);
        doorB.SetInfo(roomB, roomA, doorAPos, doorA);
    }

    private void InitializeUnionFind()
    {
        _rootMap.Clear();
        foreach (var room in _rooms)
        {
            _rootMap[room] = room;
        }
    }

    private DungeonRoom FindRoot(DungeonRoom room)
    {
        if (_rootMap[room] != room)
            _rootMap[room] = FindRoot(_rootMap[room]);

        return _rootMap[room];
    }

    private void Union(DungeonRoom roomA, DungeonRoom roomB)
    {
        DungeonRoom rootA = FindRoot(roomA);
        DungeonRoom rootB = FindRoot(roomB);

        if (rootA != rootB)
            _rootMap[rootB] = rootA;
    }

    class Edge : IComparable<Edge>
    {
        public DungeonRoom RoomA { get; }
        public DungeonRoom RoomB { get; }
        public float Distance { get; }

        public Edge(DungeonRoom roomA, DungeonRoom roomB, float distance)
        {
            RoomA = roomA;
            RoomB = roomB;
            Distance = distance;
        }

        public int CompareTo(Edge other)
            => Distance.CompareTo(other.Distance);
    }
    #endregion 

    private bool IsIndexValid(Vector2Int gridIndex)
    {
        bool isRangeValid = gridIndex.x >= 0 && gridIndex.x < _gridSizeX && gridIndex.y >= 0 && gridIndex.y < _gridSizeY;
        if (!isRangeValid)
            return false;

        bool isGridValid = _roomGrid[gridIndex.x, gridIndex.y] != 1;
        return isGridValid;
    }

    private Vector3 GetIsometricPosition(Vector2Int gridIndex)
    {
        int x = gridIndex.x;
        int y = gridIndex.y;

        float isoX = (x - y) * _roomWidth * 0.5f;
        float isoY = (x + y) * _roomHeight * 0.5f;
        return new Vector3(isoX, isoY, 0);
    }

    private Vector3 GetDoorVector(Vector2Int direction, float ratio = 1f)
    {
        // 경계선보다 살짝 안쪽에 위치
        Vector2 doorOffset = new Vector2(1, 0.5f) * ratio;

        if (direction == Vector2Int.up)
            return new Vector3(-_doorOffsetX + doorOffset.x, _doorOffsetY - doorOffset.y);
        else if (direction == Vector2Int.right)
            return new Vector3(_doorOffsetX - doorOffset.x, _doorOffsetY - doorOffset.y);
        else if (direction == Vector2Int.down)
            return new Vector3(_doorOffsetX - doorOffset.x, -_doorOffsetY + doorOffset.y);
        else if (direction == Vector2Int.left)
            return new Vector3(-_doorOffsetX + doorOffset.x, -_doorOffsetY + doorOffset.y);
        
        return Vector3.zero;
    }

    private Vector2Int GetGridIndex(Vector3 position)
    {
        float isoX = position.x;
        float isoY = position.y;

        int x = Mathf.RoundToInt((isoY / _roomHeight) + (isoX / _roomWidth));
        int y = Mathf.RoundToInt((isoY / _roomHeight) - (isoX / _roomWidth));

        return new Vector2Int(x, y);
    }

    #if UNITY_EDITOR
    [Space]
    [Header("Test Spawn")]
    [ButtonAttribute("TestSpawn")]
    public bool test = false;
    public void TestSpawn()
    {        
        PlaceRoomsRandomly();
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

    [ButtonAttribute("TestDoors")]
    public bool door = false;
    public void TestDoors()
    {
        GenerateDoors();
    }

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