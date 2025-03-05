using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatScaleFloat))]
public class StatScaleFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var defaultValueProperty = property.FindPropertyRelative("defaultValue");
        var scaleStatProperty = property.FindPropertyRelative("scaleStat");
        var resultMinValueProperty = property.FindPropertyRelative("resultMinValue");
        var resultMaxValueProperty = property.FindPropertyRelative("resultMaxValue");

        // label을 기준으로 position을 만들어줌
        position = EditorGUI.PrefixLabel(position, label);

        // EditorGUI.indentLevel은 들여쓰기 단계를 설정하는 변수로
        // 들여쓰기 단계에 따라서 property의 x좌표 틀어짐 문제가 있어서 이를 조정해줌
        // 이는 꼭 해줘야하는게 아니라 CustomEditor를 작성하다보면 여러 GUI들이 얽혀서 좌표 문제를 일으키는 경우가 있음
        // 지금이 그 상황이라 적절한 수치를 찾아서 조정해주는 것
        float adjust = EditorGUI.indentLevel * 15f;
        float halfWidth = (position.width * 0.5f) + adjust;

        var defaultValueRect = new Rect(position.x - adjust, position.y, halfWidth - 2.5f, position.height);
        defaultValueProperty.floatValue = EditorGUI.FloatField(defaultValueRect, GUIContent.none, defaultValueProperty.floatValue);

        var scaleStatRect = new Rect(defaultValueRect.x + defaultValueRect.width - adjust + 2.5f, position.y, halfWidth, position.height);
        scaleStatProperty.objectReferenceValue = EditorGUI.ObjectField(scaleStatRect, GUIContent.none, scaleStatProperty.objectReferenceValue, typeof(Stat), false);

        Stat stat = scaleStatProperty.objectReferenceValue as Stat;
        bool isClampable = stat != null && stat.IsClampable;
        if (isClampable)
        {
            float totalWidth = position.width;
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 5f;
            // 한 줄에 Min과 Max를 모두 표시하기 위한 영역 계산
            float half = (totalWidth - spacing) / 2;
            float labelWidth = 120f;
            float fieldWidth = half - labelWidth;
            // Min 값 영역 (왼쪽)
            Rect labelMinRect = new Rect(position.x, position.y + lineHeight + spacing, labelWidth, lineHeight);
            Rect fieldMinRect = new Rect(labelMinRect.x + labelWidth, position.y + lineHeight + spacing, fieldWidth, lineHeight);
            // Max 값 영역 (오른쪽)
            Rect labelMaxRect = new Rect(fieldMinRect.x + fieldWidth + spacing, position.y + lineHeight + spacing, labelWidth, lineHeight);
            Rect fieldMaxRect = new Rect(labelMaxRect.x + labelWidth, position.y + lineHeight + spacing, fieldWidth, lineHeight);
            // Min 값 표시
            EditorGUI.LabelField(labelMinRect, "Result Min Value");
            resultMinValueProperty.floatValue = EditorGUI.FloatField(fieldMinRect, resultMinValueProperty.floatValue);
            // Max 값 표시
            EditorGUI.LabelField(labelMaxRect, "Result Max Value");
            resultMaxValueProperty.floatValue = EditorGUI.FloatField(fieldMaxRect, resultMaxValueProperty.floatValue);
        }

        EditorGUI.EndProperty();
    }
}
