using UnityEngine;

public class HpPanel : UI_Base 
{
    Entity _owner;
    UI_Heart[] _hearts;
    int _heartsIndex;

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
        SetupHp(_owner.StatsComponent.HPStat.MaxValue);

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
        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i > currentValue - 1)
                _hearts[i].Off();
            else
                _hearts[i].On();
        }
    }

    void OnHPMaxValueChanged(Stat stat, float currentValue, float prevValue)
    {
        // 최대 체력이 늘거나 줄어든다
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        SetupHp(currentValue);
    }

    void SetupHp(float hpMax)
    {
        int maxCount = Mathf.FloorToInt(hpMax);
        _hearts = new UI_Heart[maxCount];

        for (int i = 0; i < maxCount; i++)
        {
            _hearts[i] = Managers.Resource.Instantiate(nameof(UI_Heart), transform).GetComponent<UI_Heart>();
            
            if (i < _owner.StatsComponent.HPStat.Value)
                _hearts[i].On();
            else
                _hearts[i].Off();
        }
    }
}