using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

public class DungeonRoom : InitOnce
{
    public enum Pattern
    {
        _1x1,
        _1x2,
        _2x1,
        _2x2,
        _L,
        _J,
        _T,
        _Z,
    }
    [SerializeField] private Pattern _patternType;
    public Pattern PatternType => _patternType;

    enum DoorDirection
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
        Center,
        Max,
    }

    ///<summary> Dungeon Grid에 소환된 Index </summary>
    public Vector2Int SpawnedIndex { get; set; }

    public Bounds RendererBounds => _tilemapRenderer.bounds;

    [SerializeField] private List<DungeonDoor> _doors = new List<DungeonDoor>();
    private TilemapRenderer _tilemapRenderer;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _tilemapRenderer = Util.FindChild<TilemapRenderer>(gameObject, "Tilemap_Walkable");

        return true;
    }

    public Vector2Int[] GetPatternIndexes()
    {
        switch (_patternType)
        {
            // TODO: 여기보다 더 다양한 모양의 맵이 생성되야 된다면
            // 1. switch-case가 아닌 클래스로 분할
            // 2. 회전 정보 적용
            // 3. 회전한 방 생성할 Editor
            case DungeonRoom.Pattern._1x1: return new Vector2Int[] { new Vector2Int(0, 0) };
            case DungeonRoom.Pattern._1x2: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0) };
            case DungeonRoom.Pattern._2x1: return new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1) };
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
}