using UnityEngine;

/// <summary>
/// [ButtonAttribute("TestBake")]
/// public bool testBake;
/// public void TestBake()
/// </summary>
public enum ButtonColor
{
    Default,    // 기본 GUI 색상
    Red,        // 빨간색
    Green,      // 초록색
    Blue,       // 파란색
    Yellow,     // 노란색
    Gray,       // 회색
    Orange,     // 주황색
    Purple      // 보라색
}

public class ButtonAttribute : PropertyAttribute
{
    public string MethodName { get; }
    public float Height { get; }
    public ButtonColor ColorType { get; }

    public ButtonAttribute(string methodName, float height = 20f, ButtonColor color = ButtonColor.Default)
    {
        MethodName = methodName;
        Height = height;
        ColorType = color;
    }

    public Color GetColor()
    {
        return ColorType switch
        {
            ButtonColor.Red => new Color(1f, 0.4f, 0.4f),
            ButtonColor.Green => new Color(0.4f, 1f, 0.4f),
            ButtonColor.Blue => new Color(0.4f, 0.4f, 1f),
            ButtonColor.Yellow => new Color(1f, 1f, 0.4f),
            ButtonColor.Gray => new Color(0.7f, 0.7f, 0.7f),
            ButtonColor.Orange => new Color(1f, 0.6f, 0.1f),
            ButtonColor.Purple => new Color(0.6f, 0.4f, 1f),
            _ => GUI.backgroundColor
        };
    }
}