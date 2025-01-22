using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class SelectHeroScene : BaseScene
{


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.SelectHeroScene;
        Managers.Scene.SetCurrentScene(this);


        Managers.UI.ShowSceneUI<UI_SelectHeroScene>();

        return true;
    }

    void Start()
    {
        int currentDungeonId = Managers.Dungeon.CurrentDungeonId;
        Debug.Log($"Current Progressed Dungeon Id:{currentDungeonId}");

        if (currentDungeonId != 0)
        {
            var messagePopup = Managers.UI.ShowPopupUI<UI_MessagePopup>();
            string message = StringTable.GetMessage("AskContinueDungeon");
            messagePopup.SetupMessageConfrim(message, ContinueDungeon);
        }
    }

    void ContinueDungeon(bool result)
    {
        if (result)
        {
            Managers.Dungeon.EnterFirstDungeon();
        }
        else
        {
            Managers.SaveLoad.RemovePlayData();
        }
    }

    public override void Clear()
    {

    }
}
