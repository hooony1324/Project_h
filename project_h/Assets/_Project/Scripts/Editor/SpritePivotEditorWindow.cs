using UnityEngine;
using UnityEditor;

public class SpritePivotEditorWindow : EditorWindow
{
    // Custom Pivot Values
    private Vector2 customPivot = new Vector2(0.5f, 0.5f);

    [MenuItem("Tools/Sprite Pivot Editor")]
    public static void ShowWindow()
    {
        GetWindow<SpritePivotEditorWindow>("Sprite Pivot Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Set Pivot for Selected Sprites", EditorStyles.boldLabel);

        // X and Y fields for custom pivot
        customPivot.x = EditorGUILayout.FloatField("Pivot X", customPivot.x);
        customPivot.y = EditorGUILayout.FloatField("Pivot Y", customPivot.y);

        if (GUILayout.Button("Apply Pivot to Selected Sprites"))
        {
            SetPivotForSelectedSprites();
        }
    }

    private void SetPivotForSelectedSprites()
    {
        // Get selected objects in the editor
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            // Ensure the selected object is a sprite
            if (textureImporter != null && textureImporter.textureType == TextureImporterType.Sprite)
            {
                // Get the current TextureImporterSettings
                TextureImporterSettings settings = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(settings);

                // Set the sprite pivot to custom and apply new pivot values
                settings.spriteAlignment = (int)SpriteAlignment.Custom;
                settings.spritePivot = customPivot;

                // Apply changes to the texture
                textureImporter.SetTextureSettings(settings);
                EditorUtility.SetDirty(textureImporter);
                textureImporter.SaveAndReimport();
            }
        }

        // Refresh the AssetDatabase to show changes
        AssetDatabase.Refresh();
    }
}
