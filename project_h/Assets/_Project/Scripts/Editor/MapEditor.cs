using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

#if UNITY_EDITOR
using Newtonsoft.Json;
using UnityEditor;
#endif

#if UNITY_EDITOR
public class MapEditor : MonoBehaviour
{
    [MenuItem("Tools/GenerateMap")]
    private static void GenerateMap()
    {
        GameObject[] gameObjects = Selection.gameObjects;

        foreach (GameObject go in gameObjects)
        {
            Tilemap tm = Util.FindChild<Tilemap>(go, "Tilemap_Collision", true);

            using (var writer = File.CreateText($"Assets/_Project/Data/MapData/{go.name}Collision.txt"))
            {
                writer.WriteLine(tm.cellBounds.xMin);
                writer.WriteLine(tm.cellBounds.xMax);
                writer.WriteLine(tm.cellBounds.yMin);
                writer.WriteLine(tm.cellBounds.yMax);

                for (int y = tm.cellBounds.yMax; y >= tm.cellBounds.yMin; y--)
                {
                    for (int x = tm.cellBounds.xMin; x <= tm.cellBounds.xMax; x++)
                    {
                        TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            if (tile.name.Contains("Character"))
                                writer.Write(Define.MAP_TOOL_CHARACTER_WALKABLE);
                            else
                                writer.Write(Define.MAP_TOOL_CAMERA_WALKABLE);
                        }							
                        else
                            writer.Write(Define.MAP_TOOL_WALL);
                    }
                    writer.WriteLine();
                }
            }
        }

        Debug.Log("Map Collision Generation Complete");
    }

    [MenuItem("Tools/MapEditor/GenerateMonsterTile")]
    private static void GenerateMonsterTile()
    {
        Object[] monsterDatas = Resources.LoadAll<MonsterData>("GameDesign/MonsterData");

        foreach (Object monsterData in monsterDatas)
        {
            MonsterData data = monsterData as MonsterData;
            EntityTile tile = AssetDatabase.LoadAssetAtPath<EntityTile>("Assets/Resources/GameDesign/EntityTileData/Monster");

            if (tile != null)
            {
                tile.entityDataName = data.entityId;
                tile.sprite = data.Sprite;

                EditorUtility.SetDirty(tile);
            }
            else
            {
                tile = ScriptableObject.CreateInstance<EntityTile>();
                tile.entityDataName = data.entityId;
                tile.sprite = data.Sprite;
                
                AssetDatabase.CreateAsset(tile, $"Assets/Resources/GameDesign/EntityTileData/Monster/{data.entityId}.asset");
                EditorUtility.SetDirty(tile);
            }
        }
    }

}
#endif