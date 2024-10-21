using UnityEngine;

[System.Serializable]
public class DungeonEntranceInteraction : NpcInteraction
{
    public override void HandleNpcInteraction()
    {
        Managers.UI.ShowPopupUI<UI_DungeonSelectPopup>();
    }
}