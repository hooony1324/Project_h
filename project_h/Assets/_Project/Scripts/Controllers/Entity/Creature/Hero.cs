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

    private Transform HeroCampDest
    {
        get
        {
            HeroCamp heroCamp = Managers.Object.HeroCamp;
            return Movement.IsForcedMoving ? heroCamp.Destination : heroCamp.Pivot;
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Hero;

        Managers.Game.OnJoystickStateChanged -= HandleJoyStickState;
        Managers.Game.OnJoystickStateChanged += HandleJoyStickState;

        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        Animator.runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>(_animControllerName);
    }

    private void HandleJoyStickState(EJoystickState joystickState)
    {
        switch (joystickState)
        {
            case EJoystickState.PointerDown:
                Movement.IsForcedMoving = true;
                Target = HeroCampDest;
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:
                Movement.IsForcedMoving = false;
                Target = HeroCampDest;
                break;
        }
    }
}