using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using Mono.Cecil;
using PlasticPipe.PlasticProtocol.Messages;

[CustomEditor(typeof(DungeonRoom), true)]
public class DungeonRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DungeonRoom dungeonRoom = (DungeonRoom)target;
        EditorGUILayout.HelpBox("Generate Walkable Area", MessageType.None);

        if (GUILayout.Button("Generate Tiles"))
        {
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>("Assets/_Project/Art/Tiles/stone.asset");
            Tilemap tilemap = Util.FindChild(dungeonRoom.gameObject, "Tilemap_Walkable").GetComponent<Tilemap>();

            tilemap.ClearAllTiles();
        
            Vector2Int[] basePattern = dungeonRoom.GetPatternIndexes();
            int Unit = Dungeon.TileUnit;
            int Range = Unit / 2;

            // 타일맵에 타일 배치
            foreach (var basePos in basePattern)
            {
                // 각 패턴 위치에 대해 TileUnit 크기로 타일을 채움
                for (int x = -Range; x < Range; x++)
                {
                    for (int y = -Range; y < Range; y++)
                    {
                        Vector3Int tilePos = new Vector3Int(basePos.x * Unit + x, basePos.y * Unit + y, 0);
                        tilemap.SetTile(tilePos, tile);
                    }
                }
            }
            
            EditorUtility.SetDirty(tilemap);
        }
    }

}