using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR

#endif

[System.Serializable]
public struct SkillData
{
    [DelayedProperty, OnValueChanged("OnLevelChanged")]
    [GUIColor("RGB(0, 1, 0)")]
    [MinValue(1)]
    public int level;

    // Skill Level Up조건
    [Title("Level Up")]
    [SerializeReference]
    public EntityCondition[] levelUpConditions;

    // Skill Level UP비용
    [SerializeReference]
    public Cost[] levelUpCosts;

    // Skill Action 발동 전 동작 수행
    // ex. 상대에게 달려감, 구르기 후 스킬, Jump등등
    [Title("Preceding Action")]
    [SerializeReference]
    public SkillPrecedingAction precedingAction;

    // Skill Action
    // 발동 할 스킬, 여러 형태의 스킬을 모듈식으로 관리
    public SkillAction action;

    [Title("Setting")]
    public SkillRunningFinishOption runningFinishOption;
    // runningFinishOption이 FinishWhenDurationEnded이고 duration이 0이면 무한 지속
    [Min(0)]
    public float duration;
    // applyCount가 0이면 무한 적용
    [Min(0)]
    public int applyCount;
    // 첫 한번은 효과가 바로 적용될 것이기 때문에, 한번 적용된 후부터 ApplyCycle에 따라 적용됨
    // 예를 들어서, ApplyCycle이 1초라면, 바로 한번 적용된 후 1초마다 적용되게 됨. 
    [Min(0f)]
    public float applyCycle;
    public StatScaleFloat cooldown;

    [Title("Target Searcher")]
    public TargetSearcher targetSearcher;

    // Skill 사용을 위한 비용
    [Title("Cost")]
    [SerializeReference]
    public Cost[] costs;

    [Title("Cast")]
    public bool isUseCast;
    [ShowIf("isUseCast")]
    public StatScaleFloat castTime;

    [Title("Charge")]
    public bool isUseCharge;
    [ShowIf("isUseCharge")]
    public SkillChargeFinishActionOption chargeFinishActionOption;
    // Charge의 지속 시간
    [Min(0f), ShowIf("isUseCharge")]
    public float chargeDuration;
    // Full Charge까지 걸리는 시간
    [Min(0f), ShowIf("isUseCharge")]
    public float chargeTime;
    // Skill을 사용하기 위해 필요한 최소 충전 시간
    [Min(0f), ShowIf("isUseCharge")]
    public float needChargeTimeToUse;
    // Charge의 시작 Power
    [Range(0f, 1f), ShowIf("isUseCharge")]
    public float startChargePower;

    // Skill 적용 효과
    // Dot딜, 회복, 등등
    [Title("Effect")]
    public EffectSelector[] effectSelectors;

    [Title("Animation")]
    public InSkillActionFinishOption inSkillActionFinishOption;
    public AnimatorParameter castAnimatorParamter;
    public AnimatorParameter chargeAnimatorParameter;
    public AnimatorParameter precedingActionAnimatorParameter;
    public AnimatorParameter actionAnimatorParameter;
    
    [SerializeReference, TabGroup("Cast")]
    public CustomAction[] customActionsOnCast;
    [SerializeReference, TabGroup("Charge")]
    public CustomAction[] customActionsOnCharge;
    [SerializeReference, TabGroup("Preceding")]
    public CustomAction[] customActionsOnPrecedingAction;
    [SerializeReference, TabGroup("Action")]
    public CustomAction[] customActionsOnAction;

/*
[FoldoutGroup("SomeOtherGroup")]
[ResponsiveButtonGroup("SomeOtherGroup/SomeBtnGroup")] public void Baz1() { }
[ResponsiveButtonGroup("SomeOtherGroup/SomeBtnGroup")] public void Baz2() { }
[ResponsiveButtonGroup("SomeOtherGroup/SomeBtnGroup")] public void Baz3() { }
*/
#if UNITY_EDITOR
    private void OnLevelChanged()
    {
        var skill = Selection.activeObject as Skill;

        if (!skill.IsValidData(this))
        {
            level = -1;
            return;
        }

        skill.SortSkillsByLevel();
    }
#endif 
}