using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<Entity>
{
    private EntityMovement movement;
    protected override void Setup()
    {
        movement = Entity.GetComponent<EntityMovement>();
    }

    public override void Enter()
    {
        if (movement)
        {
            movement.enabled = false;
            movement.AgentEnabled = false;
        }
        
    }

    public override void Exit()
    {
        if (movement)
        {
            movement.enabled = true;
            movement.AgentEnabled = true;
        }
    }
}
