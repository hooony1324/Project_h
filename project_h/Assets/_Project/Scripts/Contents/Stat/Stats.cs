using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Stats : MonoBehaviour
{
    [Space]
    [SerializeField]
    private StatOverride[] statOverrides;
    private Stat[] stats;

    [SerializeField] private Stat hpStat;
    [SerializeField] private Stat moveSpeedStat;
    [SerializeField] private Stat levelStat;
    public Stat HPStat { get; private set; }
    public Stat MoveSpeedStat { get; private set; }
    public Stat LevelStat { get; private set; }

    public Entity Owner { get; private set; }

    public virtual void Setup(Entity interactionObject)
    {
        Owner = interactionObject;
        stats = statOverrides.Select(x => x.CreateStat()).ToArray();

        HPStat = hpStat ? GetStat(hpStat) : null;
        MoveSpeedStat = moveSpeedStat ? GetStat(moveSpeedStat) : null;
        LevelStat = levelStat ? GetStat(levelStat) : null;
    }

    private void OnDestroy()
    {
        foreach (var stat in stats)
            Destroy(stat);
        stats = null;
    }

    public Stat GetStat(Stat stat)
    {
        Debug.Assert(stat != null, $"Stats::GetStat - stat은 null이 될 수 없습니다.");
        return stats.FirstOrDefault(x => x.ID == stat.ID);
    }

    public Stat GetStat(string codeName)
    {
        Debug.Assert(codeName != null, "Stats::GetStat - stat의 codeName은 null이 될 수 없습니다.");
        return stats.FirstOrDefault(x => x.CodeName == codeName);
    }

    public bool TryGetStat(Stat stat, out Stat outStat)
    {
        Debug.Assert(stat != null, $"Stats::TryGetStat - stat은 null이 될 수 없습니다.");

        outStat = stats.FirstOrDefault(x => x.ID == stat.ID);
        return outStat != null;
    }

    public float GetValue(Stat stat)
        => GetStat(stat).Value;

    public bool HasStat(Stat stat)
    {
        Debug.Assert(stat != null, $"Stats::HasStat - stat은 null이 될 수 없습니다.");
        return stats.Any(x => x.ID == stat.ID);
    }


    public void SetDefaultValue(Stat stat, float value)
        => GetStat(stat).DefaultValue = value;

    public float GetDefaultValue(Stat stat)
        => GetStat(stat).DefaultValue;

    public void IncreaseDefaultValue(Stat stat, float value)
        => GetStat(stat).DefaultValue += value;

    public void SetBonusValue(Stat stat, object key, float value)
        => GetStat(stat).SetBonusValue(key, value);
    public void SetBonusValue(Stat stat, object key, object subKey, float value)
        => GetStat(stat).SetBonusValue(key, subKey, value);

    public float GetBonusValue(Stat stat)
        => GetStat(stat).BonusValue;
    public float GetBonusValue(Stat stat, object key)
        => GetStat(stat).GetBonusValue(key);
    public float GetBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).GetBonusValue(key, subKey);
    
    public void RemoveBonusValue(Stat stat, object key)
        => GetStat(stat).RemoveBonusValue(key);
    public void RemoveBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).RemoveBonusValue(key, subKey);

    public bool ContainsBonusValue(Stat stat, object key)
        => GetStat(stat).ContainsBonusValue(key);
    public bool ContainsBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).ContainsBonusValue(key, subKey);

#if UNITY_EDITOR
    [ContextMenu("LoadStats")]
    private void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat").OrderBy(x => x.ID);
        statOverrides = stats.Select(x => new StatOverride(x)).ToArray();
    }
#endif

#if DEVELOPEMENT_BUILD || UNITY_EDITOR
    private void OnGUI()
    {
        if (!Owner.IsPlayer)
            return;

        // 좌측 상단에 넓은 Box를 그려줌
        GUI.Box(new Rect(52f, 52f, 300f, 300f), string.Empty);

        // 박스 윗 부분에 Player Stat Text를 뜨워줌
        GUI.Label(new Rect(54f, 52f, 150f, 80f), "Player Stat");

        var textRect = new Rect(54f, 72f, 250f, 80f);
        // Stat 증가를 위한 + Button의 기준 위치
        var plusButtonRect = new Rect(textRect.x + textRect.width, textRect.y, 20f, 20f);
        // Stat 감소를 위한 - Button의 기준 위치
        var minusButtonRect = plusButtonRect;
        minusButtonRect.x += 22f;

        foreach (var stat in stats)
        {
            // % Type이면 곱하기 100을 해서 0~100으로 출력
            // 0.##;-0.## format은 소숫점 2번째짜리까지 출력하되
            // 양수면 그대로 출력, 음수면 -를 붙여서 출력하라는 것
            string defaultValueAsString = stat.IsPercentType ?
                $"{stat.DefaultValue * 100f:0.##;-0.##}%" :
                stat.DefaultValue.ToString("0.##;-0.##");

            string bonusValueAsString = stat.IsPercentType ?
                $"{stat.BonusValue * 100f:0.##;-0.##}%" :
                stat.BonusValue.ToString("0.##;-0.##");

            GUI.Label(textRect, $"{stat.DisplayName}: {defaultValueAsString} ({bonusValueAsString})");
            // + Button을 누르면 Stat 증가
            if (GUI.Button(plusButtonRect, "+"))
            {
                if (stat.IsPercentType)
                    stat.DefaultValue += 0.01f;
                else
                    stat.DefaultValue += 1f;
            }

            // - Button을 누르면 Stat 감소
            if (GUI.Button(minusButtonRect, "-"))
            {
                if (stat.IsPercentType)
                    stat.DefaultValue -= 0.01f;
                else
                    stat.DefaultValue -= 1f;
            }

            // 다음 Stat 정보 출력을 위해 y축으로 한칸 내림
            textRect.y += 22f;
            plusButtonRect.y = minusButtonRect.y = textRect.y;
        }
    }
#endif
}