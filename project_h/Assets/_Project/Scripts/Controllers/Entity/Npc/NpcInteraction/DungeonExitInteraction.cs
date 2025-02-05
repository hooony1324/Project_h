using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DungeonExitInteraction : NpcInteraction
{
    public override void Setup(Npc npc)
    {

    }

    public override void HandleNpcInteraction()
    {
        UI_MessagePopup messagePopup = Managers.UI.ShowPopupUI<UI_MessagePopup>();
        messagePopup.SetupMessageConfrim(StringTable.GetMessage("AskExitDungeon"), OnClickConfirm);
    }

    private void OnClickConfirm(bool result)
    {
        if (result)
        {
            // Save Index => 0으로

            Managers.Scene.LoadScene(EScene.GameScene);
        }
    }
}