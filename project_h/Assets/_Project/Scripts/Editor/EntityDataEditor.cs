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
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        string newValue = EditorGUILayout.DelayedTextField(entityIdProperty.stringValue);
        entityIdProperty.stringValue = newValue;

        if (EditorGUI.EndChangeCheck())
        {
            var entityData = (EntityData)target;
            if (!string.IsNullOrEmpty(newValue))
            {
                string prefix = entityData.GetAssetPrefix();
                string newId = $"{prefix}_{newValue.ToUpper()}";
                entityData.entityId = newId;

                string assetPath = AssetDatabase.GetAssetPath(entityData);
                AssetDatabase.RenameAsset(assetPath, newId);

                serializedObject.ApplyModifiedProperties();
            }
        }

        if (GUILayout.Button("Load Stats"))
        {
            EntityData entityData = (EntityData)target;
            entityData.LoadStats();

            // 변경 사항을 저장하고 Inspector 업데이트
            EditorUtility.SetDirty(target);
        }

        GUILayout.BeginHorizontal();
    
        EditorGUILayout.ObjectField(GUIContent.none, spriteProperty.objectReferenceValue,
                 typeof(Sprite), false, GUILayout.Height(100));

        GUILayout.EndHorizontal();
    }
}
#endif