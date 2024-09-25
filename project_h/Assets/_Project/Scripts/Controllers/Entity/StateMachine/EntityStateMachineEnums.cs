public enum EntityStateMessage { UsingSkill }

public enum EntityStateCommand
{
    ToDefaultState,
    ToCastingSkillState,
    ToChargingSkillState,
    ToInSkillPrecedingActionState,
    ToInSkillActionState,
    ToStunningState,
    ToSleepingState
}
