using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.DungeonScene;
        Managers.Scene.SetCurrentScene(this);
        
        Managers.Map.Init();
        Managers.Map.LoadMap();

        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        Vector3 startPos = Managers.Map.Info.StartPosition;
        Hero hero = Managers.Object.Spawn<Hero>(startPos);
        hero.SetData(null);

        Managers.Game.Cam.transform.position = hero.Position;
        Managers.Game.Cam.Target = hero;
        Managers.Game.PlayerController.SetControlTarget(hero);
        
        // Managers.Game.OnBroadcastEvent -= HandleOnBroadcastEvent;
        // Managers.Game.OnBroadcastEvent += HandleOnBroadcastEvent;

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_DungeonScene>();
          
        
        return true;
    }


    public override void Clear()
    {
        
    }
}