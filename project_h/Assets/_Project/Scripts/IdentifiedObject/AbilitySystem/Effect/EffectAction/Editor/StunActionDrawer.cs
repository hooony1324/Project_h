using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StunAction))]
public class StunActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var removeTargetCategoryProperty = property.FindPropertyRelative("removeTargetCategory");

        var adjustedPosition = position;
        adjustedPosition.y += 25f;
        position = EditorGUI.PrefixLabel(adjustedPosition, label);

        // 필드 레이아웃 계산
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float fullWidth = position.width;
        
        // 첫 번째 줄 - removeTargetCategory
        Rect removeTargetCategoryRect = new Rect(position.x, position.y, fullWidth, lineHeight);
        
        // 필드 그리기
        EditorGUI.PropertyField(removeTargetCategoryRect, removeTargetCategoryProperty, new GUIContent("적용시 제거할 카테고리"));

        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        
        // 총 1줄의 필드와 간격 계산
        return lineHeight * 2 + spacing * 4;
    }
} 