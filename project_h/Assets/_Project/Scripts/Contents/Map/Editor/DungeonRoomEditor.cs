using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using static Define;

[CustomEditor(typeof(DungeonRoom), true)]
public class DungeonRoomEditor : Editor
{
    private SerializedProperty roomPatternProperty;
    private SerializedProperty roomDirectionProperty;
    private SerializedProperty roomTypeProperty;
    private SerializedProperty roomVisitedActionProperty;
    private SerializedProperty roomClearedActionProperty;


    private GUIStyle textAreaStyle;
    private GUIStyle headerStyle;

    protected void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        roomPatternProperty = serializedObject.FindProperty("roomPattern");
        roomDirectionProperty = serializedObject.FindProperty("roomDirection");
        roomTypeProperty = serializedObject.FindProperty("roomType");
        roomVisitedActionProperty = serializedObject.FindProperty("roomVisitedAction");
        roomClearedActionProperty = serializedObject.FindProperty("roomClearedAction");
    }

    private void StyleSetup()
    {
        if (textAreaStyle == null)
        {
            // Style의 기본 양식은 textArea.
            textAreaStyle = new(EditorStyles.textArea);
            // 문자열이 TextBox 밖으로 못 빠져나가게 함.
            textAreaStyle.wordWrap = true;
        }

        if (headerStyle == null)
        {
            headerStyle = new(EditorStyles.boldLabel);
            headerStyle.fontSize = 14;
            headerStyle.alignment = TextAnchor.MiddleLeft;
        }
    }

    public override void OnInspectorGUI()
    {
        StyleSetup();

        serializedObject.Update();
        
        // Room Tiles Generation
        EditorGUILayout.LabelField("Tile Generation", headerStyle);
        CustomEditorUtility.DrawUnderline();
        EditorGUILayout.PropertyField(roomPatternProperty, new GUIContent("Room Pattern"));
        EditorGUILayout.PropertyField(roomDirectionProperty, new GUIContent("Room Direction"));

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

        EditorGUILayout.Space(20);

        // Room Setting
        EditorGUILayout.LabelField("Room Setting", headerStyle);
        CustomEditorUtility.DrawUnderline();
        EditorGUILayout.PropertyField(roomTypeProperty, new GUIContent("Room Type"));

        EditorGUILayout.PropertyField(roomVisitedActionProperty);
        EditorGUILayout.PropertyField(roomClearedActionProperty);

        Transform spawnPoint = Util.FindChild<Transform>(dungeonRoom.gameObject, "SpawnPoint");

        if (spawnPoint != null)
        {
            DungeonSpawnAction roomVisitedAction = dungeonRoom.RoomVisitedAction as DungeonSpawnAction;
            DungeonSpawnAction roomClearedAction = dungeonRoom.RoomClearedAction as DungeonSpawnAction;

            // SpawnPoint 자동 할당
            if (roomVisitedAction != null)
            {
                SerializedProperty spawnPointProperty = roomVisitedActionProperty.FindPropertyRelative("spawnPoint");
                spawnPointProperty.objectReferenceValue = spawnPoint;
            }

            if (roomClearedAction != null)
            {
                SerializedProperty spawnPointProperty = roomClearedActionProperty.FindPropertyRelative("spawnPoint");
                spawnPointProperty.objectReferenceValue = spawnPoint;
            }
        }



        serializedObject.ApplyModifiedProperties();
    }

}