using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EntityData), true)]
public class EntityDataEditor : Editor
{
    SerializedProperty spriteProperty;
    SerializedProperty entityIdProperty;

    protected virtual void OnEnable()
    {
        spriteProperty = serializedObject.FindProperty("sprite");
        entityIdProperty = serializedObject.FindProperty("entityId");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all serialized fields
        EditorGUILayout.PropertyField(spriteProperty);
        DrawPropertiesExcluding(serializedObject, "m_Script", "entityId", "sprite");

        // entityId 변경 시 에셋 이름 변경 로직
        EditorGUI.BeginChangeCheck();
        var prevCodeName = entityIdProperty.stringValue;
        EditorGUILayout.DelayedTextField(entityIdProperty);

        if (EditorGUI.EndChangeCheck())
        {
            var assetPath = AssetDatabase.GetAssetPath(target);
            EntityData entityData = (EntityData)target;

            // entityId를 대문자로 변환
            var newName = $"{entityData.GetAssetPrefix().ToUpper()}_{entityIdProperty.stringValue.ToUpper()}";
            entityIdProperty.stringValue = newName;

            serializedObject.ApplyModifiedProperties();
            
            target.name = newName;

            var message = AssetDatabase.RenameAsset(assetPath, newName);
            if (!string.IsNullOrEmpty(message))
            {
                // 실패시 원래 값으로 되돌림
                entityIdProperty.stringValue = prevCodeName;
                target.name = prevCodeName;
                serializedObject.ApplyModifiedProperties();
                Debug.LogError($"Failed to rename asset: {message}");
            }
        }

        // Load Stats 버튼
        if (GUILayout.Button("Load Stats"))
        {
            EntityData entityData = (EntityData)target;
            entityData.LoadStats();
            EditorUtility.SetDirty(target);
        }

        // Sprite 프리뷰
        GUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(GUIContent.none, spriteProperty.objectReferenceValue,
                 typeof(Sprite), false, GUILayout.Height(100));
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif