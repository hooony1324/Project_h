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

        Managers.Map.SetNavMesh();
        Managers.Map.SetMap("BaseMap");
        Managers.Map.LoadMap();

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();

        Vector3 startPosition = Managers.Map.CurrentMap.StartPosition;
        Hero hero = Managers.Object.Spawn<Hero>(startPosition);
        Managers.Hero.SetMainHero(hero);
        hero.SetData(Managers.Data.GetHeroData("HERO_WARRIOR"));

        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        Managers.Game.Cam.transform.position = hero.Position;
        Managers.Game.Cam.Target = hero;
        Managers.Game.PlayerController.SetControlTarget(hero);

        //Monster monster = Managers.Object.Spawn<Monster>(new Vector3(10, -10, 0), nameof(Monster));
        //monster.SetData(Managers.Data.GetMonsterData("MONSTER_SLIME_BOSS"));

        return true;
    }

    public override void Clear()
    {
        
    }
}