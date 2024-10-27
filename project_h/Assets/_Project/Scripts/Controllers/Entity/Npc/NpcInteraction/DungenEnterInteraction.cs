using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DungeonEnterInteraction : NpcInteraction
{
    [SerializeField] private List<DungeonData> _dungeonDatas = new List<DungeonData>();
    public override void HandleNpcInteraction()
    {
        UI_DungeonSelectPopup dungeonSelectPopup = Managers.UI.ShowPopupUI<UI_DungeonSelectPopup>();
        dungeonSelectPopup.SetInfo(_dungeonDatas);
    }
}