using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IdentifiedObject), true)]
public class IdentifiedObjectEditor : Editor
{
    private SerializedProperty idProperty;
    private SerializedProperty iconProperty;
    private SerializedProperty displayNameProperty;
    private SerializedProperty codeNameProperty;

    // Title의 Foldout Expand 상태를 저장하는 변수
    private readonly Dictionary<string, bool> isFoldoutExpandedesByTitle = new();

    private GUIStyle textAreaStyle;
    protected virtual void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        iconProperty = serializedObject.FindProperty("icon");
        idProperty = serializedObject.FindProperty("id");
        displayNameProperty = serializedObject.FindProperty("displayName");
        codeNameProperty = serializedObject.FindProperty("codeName");
    }

    private void StyleSetup()
    {
        if (textAreaStyle == null)
        {
            // Style의 기본 양식은 textArea
            textAreaStyle = new(EditorStyles.textArea);
            // 문자열이 TextBox 밖으로 못 빠져나가게 함
            textAreaStyle.wordWrap = true;
        }
    }

    protected bool DrawFoldoutTitle(string text)
        => CustomEditorUtility.DrawFoldoutTitle(isFoldoutExpandedesByTitle, text);

    public override void OnInspectorGUI()
    {
        StyleSetup();

        // 객체의 Serialize 변수들의 값을 업데이트함.
        serializedObject.Update();


        EditorGUILayout.BeginHorizontal("HelpBox");
        {
            // [Sprite]
            iconProperty.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none, iconProperty.objectReferenceValue,
            typeof(Sprite), false, GUILayout.Width(65));

            EditorGUILayout.BeginVertical();
            {
                // [ID]
                EditorGUILayout.BeginHorizontal();
                {
                    // 사용자가 편집 못하도록
                    GUI.enabled = false;
                    // 변수의 선행 명칭(Prefix) 지정
                    EditorGUILayout.PrefixLabel("ID");
                    // id 변수를 그리되 Prefix는 그리지않음(=GUIContent.none); 
                    EditorGUILayout.PropertyField(idProperty, GUIContent.none);
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();

                // [CodeName] 
                // 변수 수정 검사 > 엔터 > 리소스 이름 변경 가능
                EditorGUI.BeginChangeCheck();
                var prevCodeName = codeNameProperty.stringValue;
                EditorGUILayout.DelayedTextField(codeNameProperty);

                if (EditorGUI.EndChangeCheck())
                {
                    var assetPath = AssetDatabase.GetAssetPath(target);
                    var newName = $"{target.GetType().Name.ToUpper()}_{codeNameProperty.stringValue.ToUpper()}";

                    // 이 작업 없으면 바뀐 값 적용 안됨
                    serializedObject.ApplyModifiedProperties();

                    // ProjectView에서 보이는 값을 수정, 같은 이름 가진 객체 있는 경우 실패
                    var message = AssetDatabase.RenameAsset(assetPath, newName);
                    // 성공 시, 외부이름과 내부이름을 일치시켜줘야함
                    // 다를 시 유니티에서 경고 발생 + 문제 일어날 가능성 다분함
                    if (string.IsNullOrEmpty(message))
                    {
                        target.name = newName;
                        // Refresh [id]s
                        string parentPath = Path.GetDirectoryName(assetPath);
                        string[] guids = AssetDatabase.FindAssets($"t:IdentifiedObject", new[] { parentPath });

                        int idCounter = 0;
                        foreach (string guid in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(guid);
                            IdentifiedObject asset = AssetDatabase.LoadAssetAtPath<IdentifiedObject>(path);

                            if (asset != null)
                            {
                                SerializedObject serializedObject = new SerializedObject(asset);
                                SerializedProperty idProperty = serializedObject.FindProperty("id");

                                if (idProperty != null && idProperty.propertyType == SerializedPropertyType.Integer)
                                {
                                    idProperty.intValue = idCounter;
                                    serializedObject.ApplyModifiedProperties();
                                    idCounter++;
                                }

                                EditorUtility.SetDirty(asset);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                        }
                    }
                    else
                        codeNameProperty.stringValue = prevCodeName;


                }

                // [DisplayName]
                EditorGUILayout.PropertyField(displayNameProperty);

            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();


        // Serialize 변수들의 값 변화를 적용함(=디스크에 저장함)
        // 이 작업을 해주지 않으면 바뀐 값이 적용되지 않아서 이전 값으로 돌아감
        serializedObject.ApplyModifiedProperties();
    }
}