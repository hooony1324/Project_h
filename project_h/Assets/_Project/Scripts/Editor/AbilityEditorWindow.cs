using System;
using UnityEngine;
using UnityEditor.VersionControl;
using Sirenix.Reflection.Editor;
using System.Text;


#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class AbilityEditorWindow : OdinMenuEditorWindow
{
    private string[] abilitySystemGuide = 
    {
        "[Category]\n - 개념을 구분하는 용도\n - ex) 캐릭터 : 적/아군 구분, 스킬 : 버프/디버프/CC 구분",
        "[Stat]\n - 체력, 마나/기력 등의 비용, 속도, 스킬 레벨업을 위한 스킬 포인트 등",
        "[Skill]\n - 스킬, 스킬의 작동에 의해 Effect가 발생",
        "[Effect]\n - 단일 데미지, 독 데미지, 스택형 데미지, 수면, 속박, 걸려있는 CC해제 등등"
    };

    [MenuItem("AbilitySystem/AbilityEditorWindow")]
    private static void Open()
    {
        var window = GetWindow<AbilityEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(950, 800);
    }

    private Type[] Abilities = { typeof(Category), typeof(Stat), typeof(Skill), typeof(Effect)};
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.IconSize = 28.0f;
        tree.Config.DrawSearchToolbar = true;        

        foreach (var ability in Abilities)
        {
            tree.AddAllAssetsAtPath(ability.Name, $"Resources/{ability.Name}", ability);
        }

        tree.EnumerateTree().AddIcons<IdentifiedObject>(x => x.Icon);

        return tree;
    }

    protected override void OnImGUI()
    {
        base.OnImGUI();

        if (this.MenuTree.Selection.Count == 0)
            return;

        // 메뉴 아이템 클릭 시
        foreach (var ability in Abilities)
        {
            if (MenuTree.Selection[0].Name == ability.Name)
            {
                // Guide
                {
                    Rect rect = new Rect(MenuWidth + 2, 2, position.width - MenuWidth - 4, 400);
                    EditorGUI.DrawRect(rect, SirenixGUIStyles.BoxBackgroundColor);
                    
                    GUIStyle guideStyle = new GUIStyle(GUI.skin.label);
                    guideStyle.wordWrap = true;
                    guideStyle.fontSize = 15;
                    guideStyle.richText = true;

                    string guide1 = $"{ability.Name} Data를 전부 Remove하면 Ability Editor Window에서 볼 수 없습니다.\n최소한 1개의 Data를 남겨두거나 이미 없다면 <b><color=green>Resources/{ability.Name}</color></b>폴더 에서\n<b><color=green>[ 우클릭 > Create > AbilitySystem > {ability.Name} ]</color></b>를 선택하여 Data를 생성해주세요.";
                    StringBuilder sb = new StringBuilder();
                    sb.Append(guide1);
                    sb.AppendLine();
                    sb.AppendLine();
                    foreach(var guide in abilitySystemGuide)
                    {
                        sb.AppendLine();
                        sb.Append(guide);
                        sb.AppendLine();
                    }

                    EditorGUI.LabelField(rect, sb.ToString(), guideStyle);
                }

                string path = $"Assets/Resources/{ability.Name}";
                EditorGUILayout.LabelField("Selected Ability Path: ", path);
                
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.fontSize = 15;                
                if (GUILayout.Button($"Create New [{ability.Name}] Data", buttonStyle, GUILayout.Height(50)))
                {
                    CreateAbility(ability);
                }

                if (GUILayout.Button($"Refresh Datas Id", buttonStyle, GUILayout.Height(50)))
                {
                    RefreshDatasId(path);
                }
            }
        }


        // Ability 클릭 시
        if (this.MenuTree.Selection.SelectedValue is IdentifiedObject selectedAbility)
        {
            string assetPath = AssetDatabase.GetAssetPath(selectedAbility);
            EditorGUILayout.LabelField($"Selected {selectedAbility.CodeName} Path: ", assetPath);
            
            if (Selection.activeObject != selectedAbility)
            {
                Selection.activeObject = selectedAbility;
            }

            GUILayout.Space(25);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 15;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button($"Remove [{MenuTree.Selection[0].Name}]", buttonStyle, GUILayout.Height(25)))
            {
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();                
            }

            GUILayout.Space(5);
        }
        
    }

    private void CreateAbility(Type dataType)
    {
        var guid = Guid.NewGuid();
        var newData = CreateInstance(dataType) as IdentifiedObject;
        
        string[] guids = AssetDatabase.FindAssets("t:IdentifiedObject", new[] {$"Assets/Resources/{dataType.Name}"});

        dataType.BaseType.GetField("codeName", BindingFlags.NonPublic | BindingFlags.Instance)
        .SetValue(newData, guid.ToString());

        dataType.BaseType.GetField("id", BindingFlags.NonPublic | BindingFlags.Instance)
        .SetValue(newData, guids.Length);

        AssetDatabase.CreateAsset(newData, $"Assets/Resources/{dataType.Name}/{dataType.Name.ToUpper()}_{guid}.asset");
        AssetDatabase.SaveAssets();
    }

    private void RefreshDatasId(string path)
    {
        string[] guids = AssetDatabase.FindAssets($"t:IdentifiedObject", new[] { path });

        int idCounter = 0;
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            IdentifiedObject asset = AssetDatabase.LoadAssetAtPath<IdentifiedObject>(assetPath);

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
            }
        }
    }

}
#endif