using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    Hero hero;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        SceneType = EScene.GameScene;
        Managers.Scene.SetCurrentScene(this);

        Managers.Dungeon.Clear();
        Managers.Map.SetMap("BaseMap");
        Managers.Map.LoadMap();

        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        // Start에서 Animator Dynamic하게 설정하면 캐릭터 안보임
        Vector3 startPosition = Managers.Map.CurrentMap.StartPosition;
        hero = Managers.Object.Spawn<Hero>(startPosition);

        Managers.Hero.SetMainHero(hero);
        hero.SetData(Managers.Hero.CurrentHeroData);

        hero.gameObject.SetActive(false);

        return true;
    }

    private IEnumerator Start()
    {
        Managers.Map.CurrentMap.NavMeshSurface2D.BuildNavMesh();

        yield return null;

        Managers.UI.Joystick = Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_GameScene>();

        hero.gameObject.SetActive(true);

        Managers.Game.Cam.transform.position = hero.Position;
        Managers.Game.Cam.Target = hero;
        Managers.Game.PlayerController.SetControlTarget(hero);
    }

    public override void Clear()
    {
        Managers.SaveLoad.SaveGame();
        Managers.UI.Joystick = null;

    }

    // void OnApplicationQuit()
    // {
    //     Managers.Dungeon.SetFirstDungeon(null);
    //     Managers.SaveLoad.SaveGame();
    // }
}