using System.Collections;
using UnityEngine;
using static Define;

public class PlayerController : InitOnce
{
    private Entity controlTarget;
    private Pivot pivot;
    private Vector3 moveDir;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        pivot = Managers.Resource.Instantiate(nameof(Pivot), this.transform).GetComponent<Pivot>();
        pivot.SetRadius(3f);    

        Managers.Game.OnJoystickStateChanged += HandleJoystickState;

        return true;
    }

    public void SetControlTarget(Entity entity)
    {
        controlTarget = entity;
        controlTarget.Movement.Stop();

        Managers.Game.OnTabTriggered -= Roll; 
        Managers.Game.OnTabTriggered += Roll;
    }

    private void HandleJoystickState(EJoystickState state)
    {
        switch (state)
        {
            case EJoystickState.PointerDown:
                // Use안되고 PointerDown                => ReservedSkill
                // Use후 쫒아가는 중 PointerDown        => 
                // Use > InSkillActionState 에서        =>
                controlTarget.Movement.IsForcedMoving = true;
                controlTarget.SkillSystem.CancelAll(isForce:true);
                controlTarget.SkillSystem.CancelReservedSkill();
                controlTarget.Target = null;
                controlTarget.Movement.TraceTarget = null;
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:
                controlTarget.Movement.IsForcedMoving = false;
                controlTarget.SkillSystem.CancelAll(isForce:true);
                controlTarget.SkillSystem.CancelReservedSkill();
                controlTarget.Target = null;
                controlTarget.Movement.TraceTarget = null;
                break;
        }
    }

    private void OnEnable()
    {
        moveDir = Vector3.zero; 
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
    }

    private void OnDisable()
    {
        moveDir = Vector3.zero; 
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }

    private void Update()
    {
        if (controlTarget.Movement.enabled == false)
            return;

        controlTarget.Movement.Move(moveDir);

        if (controlTarget.Movement.IsForcedMoving)
            return;

        if (controlTarget.Target == null)
            return;

        if (controlTarget.SkillSystem.DefaultAttack.IsUseable)
        {
            controlTarget.SkillSystem.DefaultAttack.Use();
        }
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        moveDir = dir;
        if (dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(-dir.x, +dir.y) * 180 / Mathf.PI;

            if (angle > 15f && angle <= 75f)
                moveDir = MoveDir.TOP_LEFT;
            else if (angle > 75f && angle <= 105f)
                moveDir = MoveDir.LEFT;
            else if (angle > 105f && angle <= 160f)
                moveDir = MoveDir.BOTTOM_LEFT;
            else if (angle > 160f || angle <= -160f)
                moveDir = MoveDir.BOTTOM;
            else if (angle < -15f && angle >= -75f)
                moveDir = MoveDir.TOP_RIGHT;
            else if (angle < -75f && angle >= -105f)
                moveDir = MoveDir.RIGHT;
            else if (angle < -105f && angle >= -160f)
                moveDir = MoveDir.BOTTOM_RIGHT;
            else
                moveDir = MoveDir.TOP;

            pivot.SetAngle(angle);
        }
    }

    private void Roll(float tabTime)
    {
        if (moveDir != Vector3.zero)            
            controlTarget.Roll(tabTime);
    }
}
