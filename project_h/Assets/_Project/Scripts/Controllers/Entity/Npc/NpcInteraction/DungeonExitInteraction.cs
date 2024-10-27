using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DungeonExitInteraction : NpcInteraction
{
    public override void HandleNpcInteraction()
    {
        UI_MessagePopup messagePopup = Managers.UI.ShowPopupUI<UI_MessagePopup>();
        messagePopup.SetInfo("던전을 나가시겠습니까?", OnClickConfirm, showConfirmButton: true, showCancelButton: true);
    }

    private void OnClickConfirm(bool result)
    {
        if (result)
            Managers.Scene.LoadScene(EScene.GameScene);
    }
}