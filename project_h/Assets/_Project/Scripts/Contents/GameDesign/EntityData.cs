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
        
        // 기존 override 값들을 Dictionary에 저장
        var existingOverrides = statOverrides?
            .Where(x => x.IsUseOverride)
            .ToDictionary(x => x.Stat.ID);
        
        // 새로운 StatOverride 배열 생성
        statOverrides = stats.Select(x => {
            // 기존에 override 되어있던 stat인 경우 해당 값을 유지
            if (existingOverrides != null && existingOverrides.TryGetValue(x.ID, out var existingOverride))
            {
                return existingOverride;
            }
            return new StatOverride(x);
        }).ToArray();
    }

    public virtual string GetAssetPrefix() => string.Empty;

#endif
}