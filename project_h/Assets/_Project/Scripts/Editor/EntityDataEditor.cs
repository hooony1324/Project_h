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
        GUILayout.BeginHorizontal();
    
        EditorGUILayout.ObjectField(GUIContent.none, spriteProperty.objectReferenceValue,
                 typeof(Sprite), false, GUILayout.Height(100));

        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}