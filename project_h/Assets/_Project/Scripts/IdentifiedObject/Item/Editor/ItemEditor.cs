using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    private SerializedProperty idProperty;
    private SerializedProperty spriteProperty;
    private SerializedProperty isAllowMultipleProperty;
    private SerializedProperty isEquipmentProperty;
    private SerializedProperty itemAcquireActionsProperty;
    private SerializedProperty descriptionProperty;

    private GUIStyle textAreaStyle;

    private void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        idProperty = serializedObject.FindProperty("id");
        spriteProperty = serializedObject.FindProperty("itemHolderSprite");
        isAllowMultipleProperty = serializedObject.FindProperty("isAllowMultiple");
        isEquipmentProperty = serializedObject.FindProperty("isEquipment");
        itemAcquireActionsProperty = serializedObject.FindProperty("itemAcquireActions");
        descriptionProperty = serializedObject.FindProperty("description");
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
    }

    public override void OnInspectorGUI()
    {
        StyleSetup();

        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("기본 정보", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        EditorGUILayout.PropertyField(idProperty, new GUIContent("아이템 ID"));
        EditorGUILayout.PropertyField(spriteProperty, new GUIContent("아이템 이미지"));
        
        EditorGUI.indentLevel--;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("아이템 설정", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        EditorGUILayout.PropertyField(isAllowMultipleProperty, new GUIContent("중복 획득 가능"));
        EditorGUILayout.PropertyField(isEquipmentProperty, new GUIContent("장비 창에 표시"));
        EditorGUI.indentLevel--;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("아이템 효과", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        EditorGUILayout.PropertyField(itemAcquireActionsProperty, new GUIContent("획득 시 효과"));
        
        EditorGUI.indentLevel--;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("설명", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        
        descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue,
                    textAreaStyle, GUILayout.Height(60));
        
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
