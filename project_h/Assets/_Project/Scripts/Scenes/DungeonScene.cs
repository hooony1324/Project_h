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
        hero.SetData(Managers.Data.GetHeroData("HERO_WARRIOR"));
        hero.gameObject.SetActive(false);
        
        return true;
    }

    private async void Start()
    {
        await Managers.Dungeon.GenerateDungeon();

        hero.gameObject.SetActive(true);

        Managers.Game.Cam.transform.position = hero.Position;
        Managers.Game.Cam.Target = hero;
        Managers.Game.PlayerController.SetControlTarget(hero);
        
        // Managers.Game.OnBroadcastEvent -= HandleOnBroadcastEvent;
        // Managers.Game.OnBroadcastEvent += HandleOnBroadcastEvent;

        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_DungeonScene>();

        Managers.Dungeon.CurrentDungeon.ForceClearAllRooms();
    }


    public override void Clear()
    {
        
    }
}