using System.Linq;
using UnityEngine;

public class EntityData : ScriptableObject
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string entityName;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float scale = 1;

    [SerializeField]
    private string animatorControllerName;

    [SerializeField]
    protected StatOverride[] statOverrides;

    [SerializeField]
    private string[] defaultSkills;

    [SerializeField]
    private string dodgeSkill;

    public int ID => id;
    public string EntityName => entityName;
    public Sprite Sprite => sprite;
    public float Scale => scale;
    public string AnimatorControllerName => animatorControllerName;
    public StatOverride[] StatOverrides => statOverrides;
    public string[] DefaultSkills => defaultSkills;
    public string DodgeSkill => dodgeSkill;

#if UNITY_EDITOR
    public virtual void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat")
            .Where(x => !x.IsConsumable)
            .OrderBy(x => x.ID);
        statOverrides = stats.Select(x => new StatOverride(x)).ToArray();
    }

    public virtual string GetAssetPrefix() => string.Empty;

#endif
}