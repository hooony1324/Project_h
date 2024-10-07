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
        if (controlTarget != null)
        {
            controlTarget.SkillSystem.onSkillTargetSelectionCompleted -= ReserveSkill;
        }

        controlTarget = entity;
        controlTarget.Movement.Stop();

        controlTarget.SkillSystem.onSkillTargetSelectionCompleted += ReserveSkill;
    }

    private void HandleJoystickState(EJoystickState state)
    {
        switch (state)
        {
            case EJoystickState.PointerUp:
                controlTarget.SkillSystem.DefaultAttack.CancelSelectTarget();
                controlTarget.SkillSystem.DefaultAttack.Use();
                break;
        }
    }

    private void OnEnable()
    {
        moveDir = Vector3.zero; 
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        StartCoroutine("UpdateMovement");
    }

    private void OnDisable()
    {
        moveDir = Vector3.zero; 
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        StopCoroutine("UpdateMovement");
    }

    private IEnumerator UpdateMovement()
    {
        while (true)
        {
            controlTarget.Movement.Move(moveDir);
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
    }

    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (result.resultMessage != SearchResultMessage.OutOfRange ||
            !skill.IsInState<SearchingTargetState>())
            return;
        
        controlTarget.SkillSystem.ReserveSkill(skill);

        var selectionResult = skill.TargetSelectionResult;
        if (selectionResult.selectedTarget)
            controlTarget.Movement.TraceTarget = selectionResult.selectedTarget.transform;
    }
}
