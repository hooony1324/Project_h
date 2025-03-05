using UnityEngine;
using UnityEditor;
using DG.Tweening;

[CustomPropertyDrawer(typeof(KnockbackAction))]
public class KnockbackActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var removeTargetCategoryProperty = property.FindPropertyRelative("removeTargetCategory");
        var knockbackDurationProperty = property.FindPropertyRelative("knockbackDuration");
        var knockbackEaseProperty = property.FindPropertyRelative("knockbackEase");

        var adjustedPosition = position;
        adjustedPosition.y += 25f;
        position = EditorGUI.PrefixLabel(adjustedPosition, label);

        // 필드 레이아웃 계산
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float fullWidth = position.width;
        
        // 첫 번째 줄 - removeTargetCategory
        Rect removeTargetCategoryRect = new Rect(position.x, position.y, fullWidth, lineHeight);
        
        // 두 번째 줄 - knockbackDuration
        Rect knockbackDurationRect = new Rect(position.x, removeTargetCategoryRect.y + lineHeight + spacing, fullWidth, lineHeight);
        
        // 세 번째 줄 - knockbackEase
        Rect knockbackEaseRect = new Rect(position.x, knockbackDurationRect.y + lineHeight + spacing, fullWidth, lineHeight);
        
        // 필드 그리기
        EditorGUI.PropertyField(removeTargetCategoryRect, removeTargetCategoryProperty, new GUIContent("적용시 제거할 카테고리"));
        EditorGUI.PropertyField(knockbackDurationRect, knockbackDurationProperty, new GUIContent("넉백 지속시간"));
        EditorGUI.PropertyField(knockbackEaseRect, knockbackEaseProperty, new GUIContent("넉백 Ease Graph"));

        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        
        // 총 3줄의 필드와 간격 계산
        return lineHeight * 3 + spacing * 4;
    }
} 