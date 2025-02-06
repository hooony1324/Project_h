using UnityEngine;


// 최대 체력 5 => 하트 2개 반
// 최대 체력 10 = > 하트 5개
public class HpPanel : UI_Base 
{
    Entity _owner;
    UI_Heart[] _hearts;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        return true;   
    }    

    public void Setup(Entity entity)
    {
        _owner = entity;
        Debug.Assert(_owner != null, "HpPanel::Setup() - entity는 null이 될 수 없습니다");

        // 하트 만들기
        SetupHearts(_owner.StatsComponent.HPStat);

        _owner.StatsComponent.HPStat.onValueChanged += OnHPValueChanged;
        _owner.StatsComponent.HPStat.onMaxValueChanged += OnHPMaxValueChanged;
    }

    void OnDisable()
    {
        if (_owner != null)
        {
            _owner.StatsComponent.HPStat.onValueChanged -= OnHPValueChanged;
            _owner.StatsComponent.HPStat.onMaxValueChanged -= OnHPMaxValueChanged;      
        }
            
    }

    void OnHPValueChanged(Stat stat, float currentValue, float prevValue)
    {
        int currentHP = Mathf.FloorToInt(stat.Value);
        int maxHP = Mathf.FloorToInt(stat.MaxValue);
        UpdateHerats(currentHP, maxHP);
    }

    void OnHPMaxValueChanged(Stat stat, float currentValue, float prevValue)
    {
        // 최대 체력이 늘거나 줄어든다
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        SetupHearts(stat);
    }

    void SetupHearts(Stat hpStat)
    {
        int maxHP = Mathf.FloorToInt(hpStat.MaxValue);
        int currentHP = Mathf.FloorToInt(hpStat.Value);
        int heartCount = Mathf.CeilToInt(maxHP / 2f);

        _hearts = new UI_Heart[heartCount];
        for (int i = 0; i < heartCount; i++)
        {
            _hearts[i] = Managers.Resource.Instantiate(nameof(UI_Heart), transform).GetComponent<UI_Heart>();
        }

        UpdateHerats(currentHP, maxHP);
    }

    void UpdateHerats(int currentHP, int maxHP)
    {
        // ex)
        // currentHP : 3
        // maxHP : 5
        // => 1.5개 / 2.5개


        // hearts.Length : 3
        // currentHP : 3
        // maxHP : 5
        for (int i = 0; i < _hearts.Length; i++)
        {
            int emptyOrFill = currentHP - (i * 2);   // empty or fill : 2(이상), 1, 0이하,
            int halfOrFull = maxHP - (i * 2);      // half or full : 1 -> half, 2-> full

            // emptyOrFill
            // 0이하 => empty
            // 1 => halfFill
            // 2이상 => full

            // halfOrFull
            // 2이상 => full
            // 1 => half

            if (emptyOrFill <= 0 && halfOrFull >= 2)
                _hearts[i].SetValue(UI_Heart.EHeartState.Empty);
            else if (emptyOrFill <= 0 && halfOrFull == 1)
                _hearts[i].SetValue(UI_Heart.EHeartState.HalfEmpty);
            else if (emptyOrFill == 1 && halfOrFull == 1)
                _hearts[i].SetValue(UI_Heart.EHeartState.Half);
            else if (emptyOrFill == 1 && halfOrFull >= 2)
                _hearts[i].SetValue(UI_Heart.EHeartState.HalfFull);
            else
                _hearts[i].SetValue(UI_Heart.EHeartState.Full);
        }
    }
}