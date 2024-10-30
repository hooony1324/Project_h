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

        Managers.Map.SetNavMesh();
        Managers.Map.SetMap("BaseMap");
        Managers.Map.LoadMap();

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();

        return true;
    }

    private void Start()
    {
        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        Vector3 startPosition = Managers.Map.Info.StartPosition;
        _hero = Managers.Object.Spawn<Hero>(startPosition);
        _hero.SetData(Managers.Data.GetHeroData("HERO_WARRIOR"));

        Managers.Game.Cam.transform.position = _hero.Position;
        Managers.Game.Cam.Target = _hero;

        Monster monster = Managers.Object.Spawn<Monster>(new Vector3(10, -10, 0), nameof(Monster));
        monster.SetData(Managers.Data.GetMonsterData("MONSTER_SLIME_BOSS"));

        Managers.Game.PlayerController.SetControlTarget(_hero);
    }

    public override void Clear()
    {
        
    }
}