using UnityEngine;

// public enum EHeroMoveState
// {
//     None,
//     TargetMonster,
//     CollectEnv,
//     ReturnToCamp,
//     ForceMove,
//     ForcePath,
// }

public class HeroStateMachine : MonoStateMachine<Entity>
{
    // 기본 상태
    // DefaultState
    // - Idle
    // - 무언가를 Tracing

    // CombatState

    // DeatState


    // 나중에 응용
    // HeroMoveState에 있는 것들...
    // CCState
    // SkillState

    protected override void AddStates()
    {
        AddState<EntityDefaultState>();
        AddState<DeadState>();
    }

    protected override void MakeTransitions()
    {
        MakeAnyTransition<DeadState>(state => Owner.IsDead);
        MakeTransition<DeadState, EntityDefaultState>(state => !Owner.IsDead);
    }
}