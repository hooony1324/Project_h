using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor;


#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

[CreateAssetMenu]
public class IdentifiedObject : ScriptableObject, ICloneable
{
    [PropertyTooltip("캐릭터 : 적/아군 구분, 스킬: 스턴/힐/버프 구분, 등등 개념을 구분하는 용도")]
    [SerializeField]
    private Category[] categories;

    [BoxGroup("Information")]
    [HideLabel, PreviewField(60, ObjectFieldAlignment.Left)]
    [HorizontalGroup("Information/Row1", 60), VerticalGroup("Information/Row1/left")]
    [SerializeField]
    private Sprite icon;

    [ReadOnly]
    [VerticalGroup("Information/Row1/right")]
    [SerializeField]
    private int id = 0;

    [VerticalGroup("Information/Row1/right")]
    [PropertyTooltip("HP입력 -> Resources/Stat/STAT_HP로 저장됨")]
    [OnValueChanged("OnCodeNameChanged"), DelayedProperty]
    [SerializeField]
    private string codeName;

    [VerticalGroup("Information/Row1/right")]
    [SerializeField]
    private string displayName;

    [VerticalGroup("Information/Row2")]
    [Title("Description", bold:false), HideLabel]
    [SerializeField, MultiLineProperty(4)]
    private string description;

    public Sprite Icon => icon;
    public int ID => id;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public virtual string Description => description;

    public virtual object Clone() => Instantiate(this);
    
    public bool HasCategory(Category category)
        => categories.Any(x => x.ID == category.ID);

    public bool HasCategory(string category)
        => categories.Any(x => x == category);

    private void OnCodeNameChanged()
    {
        var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var prevName = Selection.activeObject.name;
        var newName = $"{this.GetType().Name.ToUpper()}_{CodeName.ToUpper()}";

        var message = AssetDatabase.RenameAsset(assetPath, newName);
        if (string.IsNullOrEmpty(message))
            Selection.activeObject.name = newName;
        else
            Selection.activeObject.name = prevName;

        AssetDatabase.SaveAssets();
    }

}