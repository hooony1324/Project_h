using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "AbilitySystem/Stat")]
public class Stat : IdentifiedObject
{
    public delegate void ValueChangedHandler(Stat stat, float currentValue, float prevValue);
    public event ValueChangedHandler onValueChanged;
    public event ValueChangedHandler onValueMax;
    public event ValueChangedHandler onValueMin;    
    public event ValueChangedHandler onMaxValueChanged;

    // LoadStats할 때 필터 용도
    [SerializeField, Tooltip("코인, 열쇠, ... Ability(X)")]
    private bool isConsumable;

    // 1 => 100%, 0 => 0%
    [Tooltip("확률형 Stat이면 적용\n ex) 쿨 다운 감소율, 기술 자원 소모율, 스킬 지속시간 증가율...")]
    [SerializeField]    
    private bool isPercentType;

    [Tooltip("최저값 설정하여 값을 줄이는 용도\nex)CoolTime, Duration줄이는 용도로 사용 가능")]
    [SerializeField]    
    private bool isReduceType;

    [SerializeField]
    private float maxValue;

    [SerializeField]
    private float minValue;

    [SerializeField]
    private float defaultValue;

    // 기본 stat 외의 bonus stat을 저장하는 dictionary,
    // key 값은 bonus stat을 준 대상 (ex. 장비가 bonus Stat을 주었다면 그 장비가 key값이 됨)
    // value Dictionary의 key 값은 SubKey
    // mainKey가 bonus stat을 여러번 줄 때 각 bonus 값을 구분하기 위한 용도
    // subKey가 필요없을 경우 string.Empty를 subKey로 bonus를 저장함
    private Dictionary<object, Dictionary<object, float>> bonusValuesByKey = new();

    public bool IsConsumable => isConsumable;
    public bool IsPercentType => isPercentType;
    public bool IsReduceType => isReduceType;
    public bool IsClampable => IsPercentType || IsReduceType;
    
    public float MaxValue
    {
        get => maxValue;
        set
        {
            if (Mathf.Approximately(maxValue, value))
                return;
                
            maxValue = value;
            onMaxValueChanged?.Invoke(this, value, maxValue);
        }
    }

    public float MinValue
    {
        get => minValue;
        set => minValue = value;
    }
    public float DefaultValue
    {
        get => defaultValue;
        set
        {
            float prevValue = Value;
            defaultValue = Mathf.Clamp(value, MinValue, MaxValue);
            // value가 변했을 시 event로 알림
            TryInvokeValueChangedEvent(Value, prevValue);
        }
    }
    public float BonusValue 
    { 
        get; 
        private set; 
    }

    // Default + Bonus, 현재 총 수치
    public float Value => Mathf.Clamp(defaultValue + BonusValue, MinValue, MaxValue);
    public bool IsMax => Mathf.Approximately(Value, maxValue);
    public bool IsMin => Mathf.Approximately(Value, minValue);
    public float IncreaseRate => Value / defaultValue;
    private void TryInvokeValueChangedEvent(float currentValue, float prevValue)
    {
        if (!Mathf.Approximately(currentValue, prevValue))
        {
            onValueChanged?.Invoke(this, currentValue, prevValue);
            if (Mathf.Approximately(currentValue, MaxValue))
                onValueMax?.Invoke(this, MaxValue, prevValue);
            else if (Mathf.Approximately(currentValue, MinValue))
                onValueMin?.Invoke(this, MinValue, prevValue);
        }
    }

    #region BonusValue
    public void SetBonusValue(object key, object subKey, float value)
    {
        if (!bonusValuesByKey.ContainsKey(key))
            bonusValuesByKey[key] = new Dictionary<object, float>();
        else
            BonusValue -= bonusValuesByKey[key][subKey];

        float prevValue = Value;
        bonusValuesByKey[key][subKey] = value;
        BonusValue += value;
        
        TryInvokeValueChangedEvent(Value, prevValue);
    }

    public void SetBonusValue(object key, float value)
        => SetBonusValue(key, string.Empty, value);

    public float GetBonusValue(object key)
        => bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey) ?
        bonusValuesBySubkey.Sum(x => x.Value) : 0f;

    public float GetBonusValue(object key, object subKey)
    {
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            if (bonusValuesBySubkey.TryGetValue(subKey, out var value))
                return value;
        }
        return 0f;
    }

    public bool RemoveBonusValue(object key)
    {
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            float prevValue = Value;
            BonusValue -= bonusValuesBySubkey.Values.Sum();
            bonusValuesByKey.Remove(key);

            TryInvokeValueChangedEvent(Value, prevValue);
            return true;
        }
        return false;
    }

    public bool RemoveBonusValue(object key, object subKey)
    {
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            if (bonusValuesBySubkey.Remove(subKey, out var value))
            {
                var prevValue = Value;
                BonusValue -= value;
                TryInvokeValueChangedEvent(Value, prevValue);
                return true;
            }
        }
        return false;
    }

    public bool ContainsBonusValue(object key)
        => bonusValuesByKey.ContainsKey(key);

    public bool ContainsBonusValue(object key, object subKey)
        => bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubKey) ? bonusValuesBySubKey.ContainsKey(subKey) : false;

    
    public override string Description
    {
        get
        {
            string description = base.Description;

            // var stringsByKeyword = new Dictionary<string, string>()
            // {
            //     { "duration", Duration.ToString("0.##") },
            //     { "applyCount", ApplyCount.ToString() },
            //     { "applyCycle", ApplyCycle.ToString("0.##") },
            //     { "castTime", CastTime.ToString("0.##") },
            //     { "chargeDuration", ChargeDuration.ToString("0.##") },
            //     { "chargeTime", ChargeTime.ToString("0.##") },
            //     { "needChargeTimeToUse", NeedChargeTimeToUse.ToString("0.##") }
            // };

            var stringsByKeyword = new Dictionary<string, string>()
            {

            };

            TextReplacer.Replace(description, stringsByKeyword);
            
            // description = TextReplacer.Replace(description, stringsByKeyword);
            // description = TargetSearcher.BuildDescription(description);

            // if (PrecedingAction != null)
            //     description = PrecedingAction.BuildDescription(description);

            // description = Action.BuildDescription(description);

            // for (int i = 0; i < Effects.Count; i++)
            //     description = Effects[i].BuildDescription(description, i);

            return "";
        }
    }
    #endregion
}
