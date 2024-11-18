using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DungeonExitInteraction : NpcInteraction
{
    public override void HandleNpcInteraction()
    {
        UI_MessagePopup messagePopup = Managers.UI.ShowPopupUI<UI_MessagePopup>();
        messagePopup.SetMessageCheck(StringTable.GetMessage("AskExitDungeon"), OnClickConfirm);
    }

    private void OnClickConfirm(bool result)
    {
        if (result)
        {
            Managers.Scene.LoadScene(EScene.GameScene);
        }
    }
}