using UnityEngine;
using UnityEditor;

/// <summary>
/// [HelpBox("This is some help text for Data.", HelpBoxMessageType.Info)]
/// public string data;
/// </summary>
[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxAttributeDrawer : DecoratorDrawer {

    public override float GetHeight() {
        var helpBoxAttribute = attribute as HelpBoxAttribute;
        if (helpBoxAttribute == null) return base.GetHeight();

        // currentViewWidth 대신 가상의 너비를 설정
        float width = EditorGUIUtility.singleLineHeight * 15; // 가로 너비를 임의로 설정

        // EditorStyles.helpBox를 사용하여 높이 계산
        var helpBoxStyle = EditorStyles.helpBox;
        return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.text), width) + 4);
    }

    public override void OnGUI(Rect position) {
        var helpBoxAttribute = attribute as HelpBoxAttribute;
        if (helpBoxAttribute == null) return;

        // HelpBox 표시
        EditorGUI.HelpBox(position, helpBoxAttribute.text, GetMessageType(helpBoxAttribute.messageType));
    }

    private MessageType GetMessageType(HelpBoxMessageType helpBoxMessageType) {
        switch (helpBoxMessageType) {
            default:
            case HelpBoxMessageType.None: return MessageType.None;
            case HelpBoxMessageType.Info: return MessageType.Info;
            case HelpBoxMessageType.Warning: return MessageType.Warning;
            case HelpBoxMessageType.Error: return MessageType.Error;
        }
    }
}

