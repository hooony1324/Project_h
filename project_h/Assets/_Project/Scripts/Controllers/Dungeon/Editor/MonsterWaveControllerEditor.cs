using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(MonsterWaveController))]
public class MonsterWaveControllerEditor : Editor
{

    SerializedProperty _waveDurationListProperty;
    SerializedProperty _forceSpawnAfterDurationProperty;

    protected virtual void OnEnable()
    {
        _waveDurationListProperty = serializedObject.FindProperty("_waveDurationList");
        _forceSpawnAfterDurationProperty = serializedObject.FindProperty("_forceSpawnAfterDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // _forceSpawnAfterDuration 속성을 직접 그리고 즉시 적용
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_forceSpawnAfterDurationProperty);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        // 나머지 속성들 (_waveDurationList 제외)
        DrawPropertiesExcluding(serializedObject, "_waveDurationList", "_forceSpawnAfterDuration");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Wave Duration List", EditorStyles.boldLabel);
        
        if (!_forceSpawnAfterDurationProperty.boolValue)
            return;

        EditorGUILayout.HelpBox("Edit Wave Durations \n - 각 Tilemap_Wave 오브젝트에 대응하여 Duration을 설정합니다", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit Wave Durations", GUILayout.Width(Screen.width / 2)))
        {
            _waveDurationListProperty.ClearArray();
            
            MonsterWaveController controller = (MonsterWaveController)target;
            
            Transform[] waveObjects = controller.gameObject.GetComponentsInChildren<Transform>()
                .Where(t => t.name.Contains("Tilemap_Wave"))
                .OrderBy(t => t.name)
                .ToArray();

            foreach (Transform waveObject in waveObjects)
            {
                _waveDurationListProperty.arraySize++;
                Debug.Log($"Found wave object: {waveObject.name}");
            }
        }

        if (GUILayout.Button("Clear", GUILayout.Width(Screen.width / 2)))
        {
            _waveDurationListProperty.ClearArray();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUI.indentLevel++;

        if (_waveDurationListProperty.arraySize > 0)
        {
            EditorGUILayout.HelpBox("각 웨이브의 지속시간을 설정합니다\n[ForceSpawnAfterDuratioin:true]시 Duration이 지나면 다음 웨이브를 강제소환 합니다", MessageType.None);        
        }

        for (int i = 0; i < _waveDurationListProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            SerializedProperty waveDuration = _waveDurationListProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(waveDuration, new GUIContent($"Wave {i + 1}"));
            
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                _waveDurationListProperty.DeleteArrayElementAtIndex(i);
                break;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        

        
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}