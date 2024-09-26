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