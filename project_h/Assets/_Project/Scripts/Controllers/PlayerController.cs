using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;
using static Define;

public class PlayerController : InitOnce
{
    private Hero controlTarget;
    private Pivot pivot;
    private Vector3 moveDir;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        pivot = Managers.Resource.Instantiate(nameof(Pivot), this.transform).GetComponent<Pivot>();
        pivot.SetRadius(4f);    

        Managers.Game.OnJoystickStateChanged += HandleJoystickState;

        return true;
    }

    public void SetControlTarget(Entity entity)
    {
        Hero hero = entity as Hero;
        if (!hero)
            return;

        if (controlTarget)
            controlTarget.IsMainHero = false;

        controlTarget = hero;
        controlTarget.IsMainHero = true;
        controlTarget.Movement.Stop();

        // Joystick에 등록된 해당 Hero의 스킬 다시 세팅
        Managers.UI.Joystick.SetupActionButtons(controlTarget);
    }

    private void HandleJoystickState(EJoystickState state)
    {
        switch (state)
        {
            case EJoystickState.PointerDown:
                // Use안되고 PointerDown                => ReservedSkill
                // Use후 쫒아가는 중 PointerDown        => 
                // Use > InSkillActionState 에서        =>
                controlTarget.Movement.TraceTarget = null;
                //controlTarget.SkillSystem.CancelAll(isForce:true);
                controlTarget.SkillSystem.CancelReservedSkill();
                controlTarget.Movement.IsForcedMoving = true;
                controlTarget.Target = null;
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:
                controlTarget.Movement.TraceTarget = null;
                //controlTarget.SkillSystem.CancelAll(isForce:true);
                controlTarget.SkillSystem.CancelReservedSkill();
                controlTarget.Movement.IsForcedMoving = false;
                controlTarget.Target = null;
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
        if (controlTarget == null)
            return;

        transform.position = controlTarget.Position;

        UpdateTargetMovement();
    }

    void UpdateTargetMovement()
    {
        if (controlTarget.Movement.enabled == false)
            return;

        controlTarget.Movement.Move(moveDir);
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        moveDir = dir;
        if (dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI;
            pivot.SetAngle(angle);
        }
    }
}
