using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DealDamageAction))]
public class DealDamageActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var defaultDamageProperty = property.FindPropertyRelative("defaultDamage");
        var bonusDamageStatProperty = property.FindPropertyRelative("bonusDamageStat");
        var bonusDamageStatFactorProperty = property.FindPropertyRelative("bonusDamageStatFactor");
        var bonusDamagePerLevelProperty = property.FindPropertyRelative("bonusDamagePerLevel");
        var bonusDamagePerStackProperty = property.FindPropertyRelative("bonusDamagePerStack");
        var resultMinDamageProperty = property.FindPropertyRelative("resultMinDamage");
        var resultMaxDamageProperty = property.FindPropertyRelative("resultMaxDamage");

        var adjustedPosition = position;
        adjustedPosition.y += 25f;
        position = EditorGUI.PrefixLabel(adjustedPosition, label);

        // 필드 레이아웃 계산
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float fullWidth = position.width;

        // 첫 번째 줄 - defaultDamage
        Rect defaultDamageRect = new Rect(position.x, position.y, fullWidth, lineHeight);

        // 두 번째 줄 - bonusDamageStat
        Rect bonusDamageStatRect = new Rect(position.x, defaultDamageRect.y + lineHeight + spacing, fullWidth, lineHeight);

        // 세 번째 줄 - bonusDamageStatFactor
        Rect bonusDamageStatFactorRect = new Rect(position.x, bonusDamageStatRect.y + lineHeight + spacing, fullWidth, lineHeight);

        // 네 번째 줄 - bonusDamagePerLevel
        Rect bonusDamagePerLevelRect = new Rect(position.x, bonusDamageStatFactorRect.y + lineHeight + spacing, fullWidth, lineHeight);

        // 다섯 번째 줄 - bonusDamagePerStack
        Rect bonusDamagePerStackRect = new Rect(position.x, bonusDamagePerLevelRect.y + lineHeight + spacing, fullWidth, lineHeight);

        // 여섯 번째 줄 - resultMinDamage와 resultMaxDamage (한 줄에 나란히 배치)
        float halfWidth = (fullWidth - 10f) / 2; // 10f는 두 필드 사이의 간격
        Rect resultMinDamageRect = new Rect(position.x, bonusDamagePerStackRect.y + lineHeight + spacing * 3, halfWidth, lineHeight);
        Rect resultMaxDamageRect = new Rect(position.x + halfWidth + 10f, bonusDamagePerStackRect.y + lineHeight + spacing * 3, halfWidth, lineHeight);
       
        // 필드 그리기
        EditorGUI.PropertyField(defaultDamageRect, defaultDamageProperty, new GUIContent("기본 데미지"));
        EditorGUI.PropertyField(bonusDamageStatRect, bonusDamageStatProperty, new GUIContent("Bonus 데미지 스탯"));
        EditorGUI.PropertyField(bonusDamageStatFactorRect, bonusDamageStatFactorProperty, new GUIContent("Bonus 데미지 스탯 계수"));
        EditorGUI.PropertyField(bonusDamagePerLevelRect, bonusDamagePerLevelProperty, new GUIContent("레벨당 추가 데미지"));
        EditorGUI.PropertyField(bonusDamagePerStackRect, bonusDamagePerStackProperty, new GUIContent("스택당 추가 데미지"));
        // Min/Max 데미지 필드 (레이블 포함)
        EditorGUI.PropertyField(resultMinDamageRect, resultMinDamageProperty, new GUIContent("Result Min Value"));
        EditorGUI.PropertyField(resultMaxDamageRect, resultMaxDamageProperty, new GUIContent("Result Max Value"));

        // 데미지 계산 공식 안내
        Rect helpBoxRect = new Rect(position.x, resultMaxDamageRect.y + 25 + lineHeight + spacing, fullWidth, lineHeight * 2);
        EditorGUI.HelpBox(
            helpBoxRect,
            "데미지 계산 공식: (기본 + (보너스 스탯값 * 스탯 계수) + (Effect레벨 * 레벨당 추가 데미지)) + ((스택 - 1) * 스택당 추가 데미지)",
            MessageType.Info
        );

        EditorGUI.EndProperty();
    }

    // 프로퍼티 드로어의 높이 계산 (필드 개수에 맞게 조정)
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        // 총 4줄의 필드와 간격 계산
        return lineHeight * 8 + spacing * 25;
    }
}

