using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseLayerBehavior : StateMachineBehaviour
{
    private readonly static int kSpeedHash = Animator.StringToHash("speed");
    private readonly static int kIsRollingHash = Animator.StringToHash("isRolling");
    private readonly static int kIsDeadHash = Animator.StringToHash("isDead");

    private Entity entity;
    private NavMeshAgent agent;
    private EntityMovement movement;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entity != null)
            return;
        
        entity = animator.GetComponent<Entity>();
        agent = animator.GetComponent<NavMeshAgent>();
        movement = animator.GetComponent<EntityMovement>();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        if (entity)
        {
            animator.SetFloat(kSpeedHash, entity.IsMoving ? 1f : 0f);
            animator.SetBool(kIsDeadHash, entity.IsDead);
        }

        if (movement)
            animator.SetBool(kIsRollingHash, movement.IsRolling);

        

        // animator.SetBool(kIsStunningHash, entity.IsInState<StunningState>());
        // animator.SetBool(kIsSleepingHash, entity.IsInState<SleepingState>());
    }

}
