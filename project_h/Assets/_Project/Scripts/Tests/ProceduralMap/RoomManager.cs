using System.Collections.Generic;
using System.Xml.Resolvers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomManager : MonoBehaviour
{
    [SerializeField] int _roomWidth = 40;
    [SerializeField] int _roomHeight = 20;

    [SerializeField] int _gridSizeX = 10;
    [SerializeField] int _gridSizeY = 10;

    [SerializeField] List<Room> _roomPrefabs_1x1 = new List<Room>();
    [SerializeField] List<Room> _roomPrefabs_1x2 = new List<Room>();
    [SerializeField] List<Room> _roomPrefabs_2x1 = new List<Room>();
    [SerializeField] List<Room> _roomPrefabs_2x2 = new List<Room>();

    private int[,] _roomGrid;
    private bool _generationComplete = false;

    [ButtonAttribute("TestSpawn")]
    public bool test = false;
    public void TestSpawn()
    {
        
    }

    private void Start()
    {
        _roomGrid = new int[_gridSizeX, _gridSizeY];

        Vector2Int initialRoomIndex = new Vector2Int(_gridSizeX / 2, _gridSizeY / 2);

    }

    private IEnumerator GenerateMap()
    {
        yield return null;



    }






    
    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
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

    private bool IsPositionValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < _gridSizeX && pos.y >= 0 && pos.y < _gridSizeY;
    }
    private Vector3 GetIsometricPosition(int x, int y)
    {
        float isoX = (x - y) * _roomWidth * 0.5f;
        float isoY = (x + y) * _roomHeight * 0.5f;
        return new Vector3(isoX, isoY, 0);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.2f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeX; y++)
            {
                Vector3 position = GetIsometricPosition(x, y);
                DrawIsometricCell(position);
                DrawRoomLabel(position, $"({x}, {y})");
            }
        }
    }
    private void DrawIsometricCell(Vector3 center)
    {
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
