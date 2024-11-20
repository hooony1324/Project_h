using System.Linq;
using UnityEngine;

public class EntityData : ScriptableObject
{
    [SerializeField]
    public string entityId;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float scale = 1;

    [SerializeField]
    private string animatorControllerName;

    [SerializeField]
    private StatOverride[] statOverrides;

    public Sprite Sprite => sprite;
    public float Scale => scale;
    public string AnimatorControllerName => animatorControllerName;
    public StatOverride[] StatOverrides => statOverrides;

#if UNITY_EDITOR
    public void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat").OrderBy(x => x.ID);
        statOverrides = stats.Select(x => new StatOverride(x)).ToArray();
    }

    public virtual string GetAssetPrefix() => string.Empty;

#endif
}