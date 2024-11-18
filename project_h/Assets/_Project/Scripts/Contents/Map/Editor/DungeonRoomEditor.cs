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
        EditorGUILayout.HelpBox("Generate Editable Area", MessageType.None);

        if (GUILayout.Button("Generate Room Tiles"))
        {
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>("Assets/_Project/Art/Tiles/darksmog.asset");
            Tilemap tilemap = Util.FindChild(dungeonRoom.gameObject, "Tilemap_Room").GetComponent<Tilemap>();

            tilemap.ClearAllTiles();
        
            Vector2Int[] basePattern = dungeonRoom.GetPatternIndexes();
            int unitSize = Dungeon.TileUnit;
            int range = unitSize / 2;

            // 타일맵에 타일 배치
            foreach (var basePos in basePattern)
            {
                // 각 패턴 위치에 대해 TileUnit 크기로 타일을 채움
                for (int x = -range; x < range; x++)
                {
                    for (int y = -range; y < range; y++)
                    {
                        Vector3Int tilePos = new Vector3Int(basePos.x * unitSize + x, basePos.y * unitSize + y, 0);
                        tilemap.SetTile(tilePos, tile);
                    }
                }
            }
            
            EditorUtility.SetDirty(tilemap);
        }
    }

}