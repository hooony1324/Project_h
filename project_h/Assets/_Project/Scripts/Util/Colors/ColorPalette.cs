using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorEntry
{
    public ColorEntry(string name, string hexadecimal)
    {
        colorName = name;
        if (ColorUtility.TryParseHtmlString(hexadecimal.ToUpper(), out color))
            Debug.Log($"Color Parsed: {name}");
        else
            Debug.Log($"Color Parse Failed: {name}");

    }
    public string colorName;
    public Color color;
}

[CreateAssetMenu(fileName = "ColorPalette", menuName = "GameDesign/ColorPalette", order = 50)]
public class ColorPalette : ScriptableObject
{
    public ColorEntry[] colors;

    [ContextMenu("LoadStandardColors")]
    private void LoadStandardColors()
    {
        colors = new ColorEntry[]{
            new ColorEntry("Primary Blue", "#007BFF"),
            new ColorEntry("Soft Green", "#28A745"),
            new ColorEntry("Warning Orange", "#FFC107"),
            new ColorEntry("Error Red", "#DC3545"),
            new ColorEntry("Dark Gray", "#343A40"),
            new ColorEntry("Gray", "#B2B2B2"),
            new ColorEntry("Light Gray", "#F8F9FA"),
            new ColorEntry("Highlight Yellow", "#FFD700"),
            new ColorEntry("Purple Accent", "#6F42C1"),
            new ColorEntry("Teal", "#20C997"),
            new ColorEntry("Soft Pink", "#E83E8C")
        };
    }
}
