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

        // 직렬화 객체를 업데이트하기 전에 null 체크 추가
        if (serializedObject == null)
            return;

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

        // roomVisitedAction이 null이 아닌지 확인
        if (dungeonRoom.RoomVisitedAction != null && dungeonRoom.RoomVisitedAction is StartMonsterWaveAction)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(roomClearedActionProperty);
            EditorGUI.indentLevel--;
        }

        Transform spawnPoint = Util.FindChild<Transform>(dungeonRoom.gameObject, "SpawnPoint");

        if (spawnPoint != null)
        {
            // null 체크 추가
            if (dungeonRoom.RoomVisitedAction != null)
            {
                DungeonSpawnAction roomVisitedAction = dungeonRoom.RoomVisitedAction as DungeonSpawnAction;
                if (roomVisitedAction != null)
                {
                    SerializedProperty spawnPointProperty = roomVisitedActionProperty.FindPropertyRelative("spawnPoint");
                    if (spawnPointProperty != null)
                        spawnPointProperty.objectReferenceValue = spawnPoint;
                }
            }

            if (dungeonRoom.RoomClearedAction != null)
            {
                DungeonSpawnAction roomClearedAction = dungeonRoom.RoomClearedAction as DungeonSpawnAction;
                if (roomClearedAction != null)
                {
                    SerializedProperty spawnPointProperty = roomClearedActionProperty.FindPropertyRelative("spawnPoint");
                    if (spawnPointProperty != null)
                        spawnPointProperty.objectReferenceValue = spawnPoint;
                }
            }
        }



        serializedObject.ApplyModifiedProperties();
    }

}