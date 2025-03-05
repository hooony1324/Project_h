using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SleepAction))]
public class SleepActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var removeTargetCategoryProperty = property.FindPropertyRelative("removeTargetCategory");
        var dotCategoryProperty = property.FindPropertyRelative("dotCategory"); 

        var adjustedPosition = position;
        adjustedPosition.y += 25f;
        position = EditorGUI.PrefixLabel(adjustedPosition, label);

        // 필드 레이아웃 계산
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float fullWidth = position.width;
        
        // 첫 번째 줄 - 첫 번째 속성
        Rect removeTargetCategoryRect = new Rect(position.x, position.y, fullWidth, lineHeight);
        
        // 두 번째 줄 - 두 번째 속성
        Rect dotCategoryRect = new Rect(position.x, removeTargetCategoryRect.y + lineHeight + spacing, fullWidth, lineHeight);
        
        // 필드 그리기 (속성 이름이 명확하지 않아 일반적인 레이블 사용)
        if (removeTargetCategoryProperty != null)
            EditorGUI.PropertyField(removeTargetCategoryRect, removeTargetCategoryProperty, new GUIContent("적용시 제거할 카테고리"));
        
        if (dotCategoryProperty != null)
            EditorGUI.PropertyField(dotCategoryRect, dotCategoryProperty, new GUIContent("수면 방해하지 않는 카테고리"));

        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        
        // 총 2줄의 필드와 간격 계산
        return lineHeight * 3 + spacing * 3;
    }
} 