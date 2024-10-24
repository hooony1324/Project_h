using System.Collections;
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
        
        Managers.Map.Init();
        //Managers.Map.LoadMap("BaseMap");
        
        Vector3Int startPos = new Vector3Int(8, 10, 0);

        Hero hero = Managers.Object.Spawn<Hero>(startPos);
        hero.SetData(null);

        Managers.Game.Cam.transform.position = hero.Position;
        Managers.Game.Cam.Target = hero;
        Managers.Game.PlayerController.SetControlTarget(hero);
        
        // Managers.Game.OnBroadcastEvent -= HandleOnBroadcastEvent;
        // Managers.Game.OnBroadcastEvent += HandleOnBroadcastEvent;

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();
          
        
        return true;
    }


    public override void Clear()
    {
        
    }
}