using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonEnterInteraction : NpcInteraction
{
    [SerializeField] private DungeonData _firstDungeonData;

    public override void Setup(Npc npc) {}

    public override void HandleNpcInteraction()
    {
        Managers.Dungeon.SetFirstDungeon(_firstDungeonData);

        // 입장하시겠습니까??
        
        Managers.UI.ShowPopupUI<UI_MessagePopup>().SetupMessageConfrim(StringTable.GetMessage("AskEnterDungeon"),
        callback: EnterDungeon);        
    }

    private void EnterDungeon(bool result)
    {
        if (!result)
            return;
        
        Managers.Dungeon.EnterFirstDungeon();
    }
}