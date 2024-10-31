using UnityEngine;
using Codice.CM.Client.Gui;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(EntityData), true)]
public class EntityDataEditor : Editor
{
    SerializedProperty spriteProperty;

    private void OnEnable()
    {
        spriteProperty = serializedObject.FindProperty("sprite");
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
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