using UnityEngine;

public class EntityData : ScriptableObject
{
    [SerializeField]
    private Sprite sprite;
    
    [SerializeField]
    private float scale = 1;

    [SerializeField]
    private string animatorControllerName;

    [SerializeField]
    private StatOverride[] statsForOverride;

    public Sprite Sprite => sprite;
    public float Scale => scale;
    public string AnimatorControllerName => animatorControllerName;
}