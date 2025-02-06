using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EntityData), true)]
public class EntityDataEditor : Editor
{
    SerializedProperty spriteProperty;
    SerializedProperty idProperty;
    SerializedProperty entityNameProperty;

    protected virtual void OnEnable()
    {
        spriteProperty = serializedObject.FindProperty("sprite");
        idProperty = serializedObject.FindProperty("id");
        entityNameProperty = serializedObject.FindProperty("entityName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 스프라이트 프리뷰를 상단에 배치
        EditorGUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(GUIContent.none, spriteProperty.objectReferenceValue,
                 typeof(Sprite), false, GUILayout.Height(120));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(5);

        // 기본 정보 섹션
        EditorGUILayout.LabelField("기본 정보", EditorStyles.boldLabel);
        using (new EditorGUI.IndentLevelScope())
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(idProperty);
            GUI.enabled = true;

            // entityName 변경 로직
            EditorGUI.BeginChangeCheck();
            var prevCodeName = entityNameProperty.stringValue;
            EditorGUILayout.DelayedTextField(entityNameProperty);

            if (EditorGUI.EndChangeCheck())
            {
                var assetPath = AssetDatabase.GetAssetPath(target);
                EntityData entityData = (EntityData)target;
                var newName = $"{entityData.GetAssetPrefix().ToUpper()}_{entityNameProperty.stringValue.ToUpper()}";
                entityNameProperty.stringValue = newName;
                serializedObject.ApplyModifiedProperties();
                
                target.name = newName;

                var message = AssetDatabase.RenameAsset(assetPath, newName);
                if (!string.IsNullOrEmpty(message))
                {
                    entityNameProperty.stringValue = prevCodeName;
                    target.name = prevCodeName;
                    serializedObject.ApplyModifiedProperties();
                    Debug.LogError($"Failed to rename asset: {message}");
                }
            }

            EditorGUILayout.PropertyField(spriteProperty);
        }

        EditorGUILayout.Space(10);

        // 캐릭터 설정 섹션
        EditorGUILayout.LabelField("캐릭터 설정", EditorStyles.boldLabel);
        using (new EditorGUI.IndentLevelScope())
        {
            DrawPropertiesExcluding(serializedObject, "m_Script", "sprite", "id", "entityName", "statOverrides");
        }

        EditorGUILayout.Space(10);

        // Stats 섹션
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("스탯 설정", EditorStyles.boldLabel);
        if (GUILayout.Button("Load Stats", GUILayout.Width(100)))
        {
            EntityData entityData = (EntityData)target;
            entityData.LoadStats();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();

        using (new EditorGUI.IndentLevelScope())
        {
            SerializedProperty statOverridesProperty = serializedObject.FindProperty("statOverrides");
            EditorGUILayout.PropertyField(statOverridesProperty, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif