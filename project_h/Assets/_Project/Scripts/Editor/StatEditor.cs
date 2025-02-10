using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stat))]
public class StatEditor : IdentifiedObjectEditor
{
    private SerializedProperty isConsumableProperty;
    private SerializedProperty isPercentTypeProperty;
    private SerializedProperty isReduceTypeProperty;
    private SerializedProperty maxValueProperty;
    private SerializedProperty minValueProperty;
    private SerializedProperty defaultValueProperty;

    protected override void OnEnable()
    {
        base.OnEnable();

        isConsumableProperty = serializedObject.FindProperty("isConsumable");
        isPercentTypeProperty = serializedObject.FindProperty("isPercentType");
        isReduceTypeProperty = serializedObject.FindProperty("isReduceType");
        maxValueProperty = serializedObject.FindProperty("maxValue");
        minValueProperty = serializedObject.FindProperty("minValue");
        defaultValueProperty = serializedObject.FindProperty("defaultValue");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (DrawFoldoutTitle("Setting"))
        {
            EditorGUILayout.PropertyField(isConsumableProperty);
            EditorGUILayout.PropertyField(isPercentTypeProperty);
            EditorGUILayout.PropertyField(isReduceTypeProperty);
            EditorGUILayout.PropertyField(maxValueProperty);
            EditorGUILayout.PropertyField(minValueProperty);
            EditorGUILayout.PropertyField(defaultValueProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}