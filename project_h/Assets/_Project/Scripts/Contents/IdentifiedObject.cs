using System;
using UnityEngine;

[CreateAssetMenu]
public class IdentifiedObject : ScriptableObject, ICloneable
{
    [SerializeField]
    private int id = -1;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string displayName;
    [SerializeField]
    private string codeName;


    public int ID => id;
    public Sprite Icon => icon;
    public string DisplayName => displayName;
    public string CodeName => codeName;

    public virtual object Clone() => Instantiate(this);

}