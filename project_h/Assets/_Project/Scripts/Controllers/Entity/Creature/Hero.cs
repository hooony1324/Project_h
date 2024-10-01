using System.Collections;
using NavMeshPlus.Extensions;
using UnityEngine;
using static Define;

public enum EHeroMoveState
{
    None,
    TargetMonster,
    CollectEnv,
    ReturnToCamp,
    ForceMove,
    ForcePath,
}

public class Hero : Entity
{
    // Experimental Data -> TODO: Data Sheet
    string _animControllerName = "Hero_Warrior";

    private Pivot pivot;

    // Joystick Input
    private Vector3 moveDir;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Hero;


        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        Animator.runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>(_animControllerName);
        
        // Only Main Character
        ControlType = EEntityControlType.Player;
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        pivot = Managers.Resource.Instantiate(nameof(Pivot), this.transform).GetComponent<Pivot>();
        pivot.SetRadius(3f);
        StartCoroutine("UpdateMovement");

        Managers.Game.OnJoystickStateChanged -= HandleJoystickStateChanged;
        Managers.Game.OnJoystickStateChanged += HandleJoystickStateChanged;
    }

    private void HandleJoystickStateChanged(EJoystickState joystickState)
    {
        switch (joystickState)
        {
            case EJoystickState.PointerDown:
                Movement.IsForcedMoving = true;
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:
                Movement.IsForcedMoving = false;
                break;
        }
    }

    private IEnumerator UpdateMovement()
    {
        while (true)
        {
            Movement.Move(moveDir);
            yield return null;
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
        else
        {
            Debug.Log(moveDir);
        }

    }
}