using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.GameScene;
        Managers.Scene.SetCurrentScene(this);
        
        Managers.Map.LoadMap("BaseMap");
        
        Vector3Int startPos = new Vector3Int(8, 10, 0);

        HeroCamp heroCamp = Managers.Object.Spawn<HeroCamp>(startPos);
        heroCamp.SetCellPos(startPos, true);

        Hero hero = Managers.Object.Spawn<Hero>(startPos);
        Managers.Map.MoveTo(hero, startPos, true);

        Managers.Game.Cam.transform.position = heroCamp.Position;
        Managers.Game.Cam.Target = heroCamp;
        
        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
        Managers.Game.OnJoystickStateChanged += HandleOnJoystickStateChanged;
        // Managers.Game.OnBroadcastEvent -= HandleOnBroadcastEvent;
        // Managers.Game.OnBroadcastEvent += HandleOnBroadcastEvent;

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();
          
        
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

    public override void Clear()
    {
        
    }
}