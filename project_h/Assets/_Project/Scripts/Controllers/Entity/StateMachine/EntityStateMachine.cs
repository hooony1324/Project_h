using UnityEngine;


public class EntityStateMachine : MonoStateMachine<Entity>
{
    protected override void AddStates()
    {
        AddState<EntityDefaultState>();
        AddState<DeadState>();
        AddState<RollingState>();
        AddState<InSkillPrecedingActionState>();
        AddState<InSkillActionState>();
    }

    protected override void MakeTransitions()
    {
        // Default State
        MakeTransition<EntityDefaultState, RollingState>(state => Owner.Movement?.IsRolling ?? false);
        // MakeTransition<EntityDefaultState, CastingSkillState>(EntityStateCommand.ToCastingSkillState);
        // MakeTransition<EntityDefaultState, ChargingSkillState>(EntityStateCommand.ToChargingSkillState);
        MakeTransition<EntityDefaultState, InSkillPrecedingActionState>(EntityStateCommand.ToInSkillPrecedingActionState);
        MakeTransition<EntityDefaultState, InSkillActionState>(EntityStateCommand.ToInSkillActionState);

        // Rolling State
        MakeTransition<RollingState, EntityDefaultState>(state => !Owner.Movement.IsRolling);

        // PrecedingAction State
        MakeTransition<InSkillPrecedingActionState, InSkillActionState>(EntityStateCommand.ToInSkillActionState);
        MakeTransition<InSkillPrecedingActionState, EntityDefaultState>(state => !IsSkillInState<InPrecedingActionState>(state));

        //Action State
        MakeTransition<InSkillActionState, EntityDefaultState>(state => (state as InSkillActionState).IsStateEnded);

        // Any Transitions
        MakeAnyTransition<EntityDefaultState>(EntityStateCommand.ToDefaultState);
        MakeAnyTransition<DeadState>(state => Owner.IsDead);

        MakeTransition<DeadState, EntityDefaultState>(state => !Owner.IsDead);
    }
    private bool IsSkillInState<T>(State<Entity> state) where T : State<Skill>
        => (state as EntitySkillState).RunningSkill.IsInState<T>();
}