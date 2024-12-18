using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

public class DungeonRoom : InitOnce
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _teleportPoint = Util.FindChild(gameObject, "TeleportPoint").transform;
        _generatedGrid = GetPatternIndexes();
        _waveController = Util.FindChild(gameObject, "MonsterWaveController").GetComponent<MonsterWaveController>();

        CreateCollisionBorder();

        return true;
    }

    #region Dungeon Generation
    enum RoomDirection
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
    }

    enum Pattern
    {
        _1x1,
        _1x2,
        _2x2,
        _J,
        _L,
        _T,
        _Z,
    }

    [SerializeField] private Pattern _roomPattern;
    [SerializeField] private RoomDirection _roomDirection = RoomDirection.UP;

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
    private Vector2Int[] _generatedGrid;
    private Vector2Int[] _spawnedGrid;
    private readonly List<DungeonDoor> _doors = new List<DungeonDoor>();
    private Transform _teleportPoint;

    public Vector3 TeleportPosition => _teleportPoint.position;
    public List<DungeonDoor> Doors => _doors;

    public Vector2Int[] GetPatternIndexes()
    {
        Vector2Int[] rotatedIndexes = GetPattern();

        for (int i = 0; i < rotatedIndexes.Length; i++)
        {
            Vector2Int p = rotatedIndexes[i];
            rotatedIndexes[i] = _roomDirection switch
            {
                // UP이 기본 형태
                RoomDirection.RIGHT => new Vector2Int(p.y, -p.x),
                RoomDirection.DOWN => new Vector2Int(-p.x, -p.y),
                RoomDirection.LEFT => new Vector2Int(-p.y, p.x),
                _ => p
            };
        }

        return rotatedIndexes;
    }

    private Vector2Int[] GetPattern()
    {
        switch (_roomPattern)
        {
            case DungeonRoom.Pattern._1x1: return new Vector2Int[] { new Vector2Int(0, 0) };
            case DungeonRoom.Pattern._1x2: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0) };
            case DungeonRoom.Pattern._2x2: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };
            case DungeonRoom.Pattern._L: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(1, 0) };
            case DungeonRoom.Pattern._J: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(2, 0) };
            case DungeonRoom.Pattern._T: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(1, 1) };
            case DungeonRoom.Pattern._Z: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1) };
            default:
                Debug.LogWarning("Unknown pattern!");
                return new Vector2Int[0];
        }
    }

    /// <summary> ex) L모양 패턴의 특정 인덱스에서 선택한 방향에 Door를 소환한다 </summary>
    public DungeonDoor SpawnDoor(Vector3 spawnPosition)
    {
        DungeonDoor door = Managers.Object.Spawn<DungeonDoor>(spawnPosition); // TODO : Manager.Object.Instantiate
        // do something
        // ex) targetRoom(BossRoom) > door.Sprite = "BossDoor"

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

    public enum RoomState
    {
        Locked,

        Unlocked,

        WaveStarted, // (Locked)
        Cleared,    // (Unlocked) Monster Wave 다 처리함
    }

    public bool IsWaveCleared => _isWaveCleared;
    private bool _isWaveCleared = false;

    private RoomState _state = RoomState.Locked;

    public RoomState State
    {
        get => _state;
        set
        {
            if (_state == value)
                return;

            _state = value;
            HandleRoomState(_state);
        }
    }

    void HandleRoomState(RoomState state)
    {
        switch (state)
        {
            case RoomState.Locked:
                LockRoom();
                break;
            case RoomState.Unlocked:
                UnlockRoom();
                break;
            case RoomState.WaveStarted:
                LockRoom();
                StartWave();
                break;
            case RoomState.Cleared:
                ClearWave();
                UnlockRoom();
                break;
        }
    }

    void LockRoom()
    {
        _doors.ForEach(x => x.Close());
    }

    void UnlockRoom()
    {
        _doors.ForEach(x => x.Open());
    }

    private MonsterWaveController _waveController;
    public void InitWaveController()
    {
        _waveController.InitWaveDatas(this);
    }

    private async Awaitable StartWave()
    {
        if (_isWaveCleared)
            return;

        _waveController.onWavesCleared += () =>
        {
            State = RoomState.Cleared;
        };

        await Awaitable.WaitForSecondsAsync(1f);

        _waveController.StartWave();
    }

    void ClearWave()
    {
        _isWaveCleared = true;
    }

    public void HandleHeroVisited()
    {
        if (_isWaveCleared == true)
            return;

        State = RoomState.WaveStarted;
    }

    #endregion
}