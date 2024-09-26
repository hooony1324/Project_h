using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Skill")]
public class Skill : IdentifiedObject
{
    private const int kInfinity = 0;

    public delegate void LevelChangedHandler(Skill skill, int currentLevel, int prevLevel);
    public delegate void StateChangedHandler(Skill skill, State<Skill> newState, State<Skill> prevState, int layer);
    public delegate void AppliedHander(Skill skill, int currentApplyCount);
    public delegate void UsedHandler(Skill skill);

    // Skill이 사용(Use)된 직후 실행되는 Event
    public delegate void ActivatedHandler(Skill skill);
    // Skill이 종료된 직후 실행되는 Event
    public delegate void DeactivatedHandler(Skill skill);

    public delegate void CanceledHandler(Skill skill);
    public delegate void TargetSelectionCompletedHandler(Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result);
    public delegate void CurrentApplyCountChangedHandler(Skill skill, int currentApplyCount, int prevApplyCount);

    [SerializeField]
    private SkillType type;
    [SerializeField]
    private SkillUseType useType;

    [SerializeField]
    private SkillExecutionType executionType;
    [SerializeField]
    private SkillApplyType applyType;

    [SerializeField]
    private NeedSelectionResultType needSelectionResultType;
    [SerializeField]
    private TargetSelectionTimingOption targetSelectionTimingOption;
    [SerializeField]
    private TargetSearchTimingOption targetSearchTimingOption;

    [SerializeReference]
    private EntityCondition[] acquisitionConditions;
    [SerializeReference]
    private Cost[] acquisitionCosts;

    // Skill을 사용하기 위한 조건들
    [SerializeReference]
    private SkillCondition[] useConditions;

    [SerializeField]
    private bool isAllowLevelExceedDatas;
    [SerializeField]
    private int maxLevel;
    [SerializeField, Min(1)]
    private int defaultLevel = 1;
    [SerializeField]
    private SkillData[] skillDatas;

    private SkillData currentData;

    private int level;

    private int currentApplyCount;
    private float currentCastTime;
    private float currentCooldown;
    private float currentDuration;
    private float currentChargePower;
    private float currentChargeDuration;

    private readonly Dictionary<SkillCustomActionType, CustomAction[]> customActionsByType = new();

    public Entity Owner { get; private set; }
    public StateMachine<Skill> StateMachine { get; private set; }

    public SkillType Type => type;
    public SkillUseType UseType => useType;

    public SkillExecutionType ExecutionType => executionType;
    public SkillApplyType ApplyType => applyType;

    public IReadOnlyList<EntityCondition> AcquisitionConditions => acquisitionConditions;
    public IReadOnlyList<Cost> AcquisitionCosts => acquisitionCosts;

    public IReadOnlyList<EntityCondition> LevelUpConditions => currentData.levelUpConditions;
    public IReadOnlyList<Cost> LevelUpCosts => currentData.levelUpCosts;

    public IReadOnlyList<SkillCondition> UseConditions => useConditions;

    public IReadOnlyList<Effect> Effects { get; private set; } = Array.Empty<Effect>();
    
}