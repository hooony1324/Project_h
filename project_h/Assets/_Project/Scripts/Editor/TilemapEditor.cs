using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(Tilemap))]
public class TilemapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("타일 모두 지우기"))
        {
            var tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
            if (tilemap == null)
                return;

            tilemap.ClearAllTiles();

            EditorUtility.SetDirty(tilemap);
        }
    }

}