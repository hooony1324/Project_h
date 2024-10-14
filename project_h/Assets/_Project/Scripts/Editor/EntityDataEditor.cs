using UnityEngine;
using Codice.CM.Client.Gui;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(EntityData), true)]
public class EntityDataEditor : Editor
{
    SerializedProperty entitySpriteProperty;

    private void OnEnable()
    {
        entitySpriteProperty = serializedObject.FindProperty("entitySprite");
    }


    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
    
        EditorGUILayout.ObjectField(GUIContent.none, entitySpriteProperty.objectReferenceValue,
                 typeof(Sprite), false, GUILayout.Height(100));

        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}