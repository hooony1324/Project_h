using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLayerBehavior : StateMachineBehaviour
{
    private readonly static int kSpeedHash = Animator.StringToHash("speed");

    private readonly static int kIsDeadHash = Animator.StringToHash("isDead");

    private Entity entity;
    private EntityMovement movement;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entity != null)
            return;
        
        entity = animator.GetComponent<Entity>();
        movement = animator.GetComponent<EntityMovement>();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        if (entity == null)
            return;

        if (movement)
            animator.SetFloat(kSpeedHash, movement.IsMoving);

        animator.SetBool(kIsDeadHash, entity.IsDead);


    }

}
