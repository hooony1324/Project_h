using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonScene : BaseScene
{
    [SerializeField] private SceneField _sceneTL;

    Hero hero;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.DungeonScene;
        Managers.Scene.SetCurrentScene(this);
        
        Managers.Map.LoadMap();


        GameObject playerController = new GameObject { name = "@PlayerController"};
        playerController.AddComponent<PlayerController>();

        Vector3 startPosition = Managers.Map.CurrentMap.StartPosition;
        hero = Managers.Object.Spawn<Hero>(startPosition);

        Managers.Hero.SetMainHero(hero);
        hero.SetData(Managers.Hero.CurrentHeroData);

        hero.gameObject.SetActive(false);
        
        Managers.SaveLoad.LoadPlayData();
        Managers.SaveLoad.SaveGame();
        

        return true;
    }

    private async void Start()
    {
        await Managers.Dungeon.GenerateDungeon();

        Managers.UI.Joystick = Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_DungeonScene>().Setup(hero);

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
}