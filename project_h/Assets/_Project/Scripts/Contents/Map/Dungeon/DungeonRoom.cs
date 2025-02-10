using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public class DungeonRoom : InitOnce
{
    [SerializeField] private EPattern roomPattern;
    [SerializeField] private ERoomDirection roomDirection = ERoomDirection.UP;
    [SerializeField] private EDungeonRoomType roomType = EDungeonRoomType.None;

    // TODO : 자물쇠 방, 한다면 => roomVisitCondition;
    [SerializeReference, SubclassSelector] 
    private DungeonRoomAction roomVisitedAction = null;
    [SerializeReference, SubclassSelector] 
    private DungeonRoomAction roomClearedAction = null;

    private Transform _teleportPoint;
    private Vector2Int[] _generatedGrid;
    private MonsterWaveController _waveController;

    public EDungeonRoomType RoomType => roomType;
    public DungeonRoomAction RoomVisitedAction => roomVisitedAction;
    public DungeonRoomAction RoomClearedAction => roomClearedAction;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _teleportPoint = Util.FindChild(gameObject, "TeleportPoint").transform;
        _generatedGrid = GetPatternIndexes();
        _waveController = GetComponent<MonsterWaveController>();

        CreateCollisionBorder();

        return true;
    }

    #region Dungeon Generation
    enum ERoomDirection
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
    }

    enum EPattern
    {
        _1x1,
        _1x2,
        _1x3,
        _2x2,
        _3x3,
        _J,
        _L,
        _T,
        _Z,
    }

    ///<summary> Dungeon Grid에 소환된 Index </summary>
    private Vector2Int _spawnedIndex;
    public Vector2Int SpawnedIndex 
    {
        get => _spawnedIndex;
        set
        {
            _spawnedIndex = value;
            _spawnedGrid = Array.ConvertAll(_generatedGrid, grid => grid + value);
        } 
    }
    
    /// <summary> Dungeon Grid에 소환된 Indexes </summary>
    public Vector2Int[] SpawnedGrid => _spawnedGrid;
    private Vector2Int[] _spawnedGrid;
    private readonly List<DungeonDoor> _doors = new List<DungeonDoor>();

    public Vector3 TeleportPosition => _teleportPoint.position;
    public List<DungeonDoor> Doors => _doors;

    public Vector2Int[] GetPatternIndexes()
    {
        Vector2Int[] rotatedIndexes = GetPattern();

        for (int i = 0; i < rotatedIndexes.Length; i++)
        {
            Vector2Int p = rotatedIndexes[i];
            rotatedIndexes[i] = roomDirection switch
            {
                // UP이 기본 형태
                ERoomDirection.RIGHT => new Vector2Int(p.y, -p.x),
                ERoomDirection.DOWN => new Vector2Int(-p.x, -p.y),
                ERoomDirection.LEFT => new Vector2Int(-p.y, p.x),
                _ => p
            };
        }

        return rotatedIndexes;
    }

    private Vector2Int[] GetPattern()
    {
        switch (roomPattern)
        {
            case DungeonRoom.EPattern._1x1: return new Vector2Int[] { new Vector2Int(0, 0) };
            case DungeonRoom.EPattern._1x2: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0) };
            case DungeonRoom.EPattern._1x3: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) };
            case DungeonRoom.EPattern._2x2: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };
            case DungeonRoom.EPattern._3x3: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2)};
            case DungeonRoom.EPattern._L: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(1, 0) };
            case DungeonRoom.EPattern._J: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(2, 0) };
            case DungeonRoom.EPattern._T: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(1, 1) };
            case DungeonRoom.EPattern._Z: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1) };
            default:
                Debug.LogWarning("Unknown pattern!");
                return new Vector2Int[0];
        }
    }

    /// <summary> ex) L모양 패턴의 특정 인덱스에서 선택한 방향에 Door를 소환한다 </summary>
    public DungeonDoor SpawnDoor(Vector3 spawnPosition)
    {
        DungeonDoor door = Managers.Object.Spawn<DungeonDoor>(spawnPosition);
        door.transform.position = spawnPosition;
        _doors.Add(door);

        return door;
    }

    public void CreateCollisionBorder()
    {
        Tilemap tilemap = Util.FindChild(gameObject, "Tilemap_Room").GetComponent<Tilemap>();
        Tilemap collisionTilemap = Util.FindChild(gameObject, "Tilemap_Collider").GetComponent<Tilemap>();

        if (collisionTilemap == null)
            return;

        // tilemap의 범위를 가져옵니다
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;

        // tilemap의 모든 타일을 순회합니다
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);

                // 타일이 존재하고 경계에 위치한 타일인 경우
                if (tile == null)
                    continue;

                if (tile.name.Contains("darksmog"))
                {
                    collisionTilemap.SetTile(tilePosition, tile);
                }
            }
        }
    }

    #endregion

    #region Room State & Wave Control
    public bool IsWaveCleared => _isWaveCleared;
    private bool _isWaveCleared = true;
    private bool _isVisited = false;

    public void LockRoom()
    {
        _doors.ForEach(x => x.Close());
    }

    public void UnlockRoom()
    {
        _doors.ForEach(x => x.Open());
    }

    public void InitMonsterWaves()
    {
        _waveController.InitWaveDatas(this);

        // StartMonsterWaveAction설정한 방만 MonsterWave시작할 수 있음
        if (roomVisitedAction != null && roomVisitedAction.CheckDungeonRoomAction<StartMonsterWaveAction>())
        {
            _isWaveCleared = false;

            _waveController.onWavesCleared += () =>
            {
                roomClearedAction?.Apply(this);
                _isWaveCleared = true;
                UnlockRoom();
            };
        }
    }   

    public async Awaitable StartWave()
    {
        if (_isWaveCleared)
            return;

        LockRoom();

        await Awaitable.WaitForSecondsAsync(1f);

        _waveController.StartWave();
    }


    public void HandleHeroVisited()
    {
        if (_isVisited == false)
        {
            roomVisitedAction?.Apply(this);
            _isVisited = true;
        }
    }

    #endregion
}