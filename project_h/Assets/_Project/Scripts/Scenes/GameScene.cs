using UnityEngine;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.GameScene;
        Managers.Scene.SetCurrentScene(this);
        
        //test
        HeroCamp heroCamp = Managers.Resource.Instantiate(nameof(HeroCamp)).GetComponent<HeroCamp>();
        heroCamp.transform.position = Vector3.zero;
        Managers.Game.Cam.transform.position = heroCamp.Position;
        Managers.Game.Cam.Target = heroCamp;
        
        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
        Managers.Game.OnJoystickStateChanged += HandleOnJoystickStateChanged;
        // Managers.Game.OnBroadcastEvent -= HandleOnBroadcastEvent;
        // Managers.Game.OnBroadcastEvent += HandleOnBroadcastEvent;

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();

        {
            GameObject obj = Managers.Resource.Instantiate(nameof(Hero));
            obj.GetComponent<Animator>().runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>("Hero_Bow");
        }
        {
            GameObject obj = Managers.Resource.Instantiate(nameof(Hero));
            obj.GetComponent<Animator>().runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>("Hero_Warrior");
        }
        

        return true;
    }

    private void HandleOnJoystickStateChanged(EJoystickState joystickState)
    {
        switch (joystickState)
        {
            case EJoystickState.PointerDown:
                break;
            case EJoystickState.Drag:
                break;
            case EJoystickState.PointerUp:

                break;
            default:
                break;
        }
    }
    
    // private void HandleOnBroadcastEvent(EBroadcastEventType eventType, ECurrencyType currencyType, int value)
    // {
    //     switch (eventType)
    //     {
    //         case EBroadcastEventType.HeroDead:
    //             if (IsDefeated())
    //             {
    //                 OnDefeated();
    //             }

    //             break;
    //     }
    // }

    public override void Clear()
    {
        
    }
}