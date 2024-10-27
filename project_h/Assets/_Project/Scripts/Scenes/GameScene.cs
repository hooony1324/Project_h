using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{

    private Hero _hero;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.GameScene;
        Managers.Scene.SetCurrentScene(this);

        Managers.Map.Init();
        Managers.Map.SetMap("BaseMap");
        Managers.Map.LoadMap();
            
        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        Vector3 startPosition = Managers.Map.Info.StartPosition;
        _hero = Managers.Object.Spawn<Hero>(startPosition);
        _hero.SetData(null);

        // Managers.Game.OnBroadcastEvent -= HandleOnBroadcastEvent;
        // Managers.Game.OnBroadcastEvent += HandleOnBroadcastEvent;

        Managers.Game.Cam.transform.position = _hero.Position;
        Managers.Game.Cam.Target = _hero;
        Managers.Game.PlayerController.SetControlTarget(_hero);

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();
          
        
        return true;
    }

    public override void Clear()
    {
        
    }
}