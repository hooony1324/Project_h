using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VFolders.Libs;

#if UNITY_EDITOR
[CustomEditor(typeof(RectTransform))]
[CanEditMultipleObjects]
public class RectTransformInspector : Editor
{
    static Type inspectorType;
    static MethodInfo OnDisableMethod;
    static MethodInfo OnEnableMethod;
    static MethodInfo OnSceneGUIMethod;
    static FieldInfo ShowLayoutOptionsField;

    Editor inspectorInstance;
    Action inspector_OnDisable;
    Action inspector_OnEnable;
    Action inspector_OnSceneGUI;

    static RectTransformInspector()
    {
        var type = typeof(Editor).Assembly.GetType("UnityEditor.RectTransformEditor");
        inspectorType = type;

        OnDisableMethod = type.GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance);
        OnEnableMethod = type.GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
        OnSceneGUIMethod = type.GetMethod("OnSceneGUI", BindingFlags.NonPublic | BindingFlags.Instance);
        ShowLayoutOptionsField = type.GetField("ShowLayoutOptions", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    void OnEnable()
    {
        inspectorInstance = CreateEditor(targets, inspectorType);
        inspector_OnDisable = (Action)Delegate.CreateDelegate(typeof(Action), inspectorInstance, OnDisableMethod, true);
        inspector_OnEnable = (Action)Delegate.CreateDelegate(typeof(Action), inspectorInstance, OnEnableMethod, true);
        inspector_OnSceneGUI = (Action)Delegate.CreateDelegate(typeof(Action), inspectorInstance, OnSceneGUIMethod, true);

        inspector_OnEnable?.Invoke();
    }
    public override void OnInspectorGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    inspectorInstance?.OnInspectorGUI();

                    var indentPerLevel = 15F;
                    var lastRect = GUILayoutUtility.GetLastRect();
                    var singleLineHeight = EditorGUIUtility.singleLineHeight;

                    lastRect = DrawButton(lastRect, EditorGUI.indentLevel * indentPerLevel, 0, singleLineHeight,"S", x => x.localScale = Vector3.one);
                    lastRect = DrawButton(lastRect, EditorGUI.indentLevel * indentPerLevel, singleLineHeight + 2, singleLineHeight, "R", x => x.localEulerAngles = Vector3.zero);
                    lastRect = DrawButton(lastRect, EditorGUI.indentLevel * indentPerLevel, singleLineHeight + 10, singleLineHeight, "P", x => x.pivot = Vector3.one * 0.5F);

                    var showLayoutOptions = (bool)(ShowLayoutOptionsField?.GetValue(inspectorInstance) ?? false);
                    if (showLayoutOptions)
                    {
                        lastRect = DrawButton(lastRect, (EditorGUI.indentLevel + 1) * indentPerLevel, singleLineHeight + 4, singleLineHeight, "a", x => x.anchorMax = Vector3.one * 0.5F);
                        lastRect = DrawButton(lastRect, (EditorGUI.indentLevel + 1) * indentPerLevel, singleLineHeight + 2, singleLineHeight, "i", x => x.anchorMin = Vector3.one * 0.5F);
                    }

                    lastRect = DrawButton(lastRect, EditorGUIUtility.labelWidth, singleLineHeight + 20, singleLineHeight, "S", x => x.sizeDelta = new Vector2(100, 100));
                    lastRect = DrawButton(lastRect, EditorGUIUtility.labelWidth, singleLineHeight + 18, singleLineHeight, "P", x => x.anchoredPosition3D = Vector3.zero);
                }
            }
        }

        EditorGUILayout.Space(15f);
        EditorGUILayout.LabelField("비율로 사이즈 조절");
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUIUtility.labelWidth = 80f;
            EditorGUILayout.PrefixLabel("Preset");

            // for Child Layout
            GUILayout.Width(100);
            if (GUILayout.Button("Set Width(0.9 x 0.8)"))
            {
                GameObject[] objs = Selection.gameObjects;

                foreach (var obj in objs)
                {
                    RectTransform rt = obj.GetComponent<RectTransform>();
                    if (rt == null)
                        continue;

                    float cachePosY = rt.anchoredPosition.y;
                    float cacheHeight = rt.rect.height;
                    rt.anchorMin = new Vector2(0.05f, 0.1f);
                    rt.anchorMax = new Vector2(0.95f, 0.9f);
                    rt.offsetMin = new Vector2(0, 0);  // new Vector2(left, bottom)
                    rt.offsetMax = new Vector2(0, 0);  // new Vector2(right, top)
                }
            }

            // for Popup UI
            if (GUILayout.Button("Set Width&Height(0.4 x 0.6)"))
            {
                GameObject[] objs = Selection.gameObjects;

                foreach (var obj in objs)
                {
                    RectTransform rt = obj.GetComponent<RectTransform>();
                    if (rt == null)
                        continue;
                    rt.anchorMin = new Vector2(0.3f, 0.2f);
                    rt.anchorMax = new Vector2(0.7f, 0.8f);
                    rt.offsetMin = new Vector2(0, 0);
                    rt.offsetMax = new Vector2(0, 0);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        // 황금률(1 : 1.6)
        // (0.1875f ~ 0.8125f)
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUIUtility.labelWidth = 90f;
            EditorGUILayout.PrefixLabel("Golden(1 : 1.6)");
            if (GUILayout.Button("Set Width"))
            {
                GameObject[] objs = Selection.gameObjects;
                foreach (var obj in objs)
                {
                    RectTransform rt = obj.GetComponent<RectTransform>();
                    if (rt == null)
                        continue;

                    rt.anchorMin = new Vector2(0.1875f, rt.anchorMin.y);
                    rt.anchorMax = new Vector2(0.8125f, rt.anchorMax.y);
                    rt.offsetMin = new Vector2(0, rt.offsetMin.y);  // new Vector2(left, bottom)
                    rt.offsetMax = new Vector2(0, rt.offsetMax.y);  // new Vector2(right, top)
                }
            }

            if (GUILayout.Button("Set Height"))
            {
                GameObject[] objs = Selection.gameObjects;

                foreach (var obj in objs)
                {
                    RectTransform rt = obj.GetComponent<RectTransform>();
                    if (rt == null)
                        continue;

                    rt.anchorMin = new Vector2(rt.anchorMin.x, 0.1875f);
                    rt.anchorMax = new Vector2(rt.anchorMax.x, 0.8125f);
                    rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
                    rt.offsetMax = new Vector2(rt.offsetMax.x, 0);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

    }
    Rect DrawButton(Rect previousRect, float x, float offsetY, float height, string name = null, Action<RectTransform> callback = null)
    {
        var rect = previousRect;
        rect.x = x;
        rect.y -= offsetY;
        rect.width = rect.height = height;
        if (GUI.Button(rect, name))
        {
            Undo.RecordObjects(targets, "Inspector");
            foreach (RectTransform item in targets)
            {
                callback(item);
            }
        }

        return rect;
    }

    void OnSceneGUI()
    {
        inspector_OnSceneGUI?.Invoke();
    }

    void OnDisable()
    {
        inspector_OnDisable?.Invoke();

        if (inspectorInstance)
        {
            UnityEngine.Object.DestroyImmediate(inspectorInstance);
            inspectorInstance = null;
        }
    }
}

#endif