using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonEnterInteraction : NpcInteraction
{
    [SerializeField] private List<DungeonData> _dungeonDatas = new List<DungeonData>();
    public override void HandleNpcInteraction()
    {
        Managers.Dungeon.SetupDungeons(_dungeonDatas);

        // 입장하시겠습니까??
        
        Managers.UI.ShowPopupUI<UI_MessagePopup>().SetupMessageConfrim(StringTable.GetMessage("AskEnterDungeon"),
        callback: EnterDungeon);        
    }

    private void EnterDungeon(bool result)
    {
        if (!result)
            return;
        
        Managers.Dungeon.TryEnterNextDungeon();
    }
}