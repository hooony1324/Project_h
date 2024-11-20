using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnterNextDungeonInteraction : NpcInteraction
{
    [SerializeField] private readonly List<DungeonData> _dungeonDatas = new List<DungeonData>();
    public override void HandleNpcInteraction()
    {
        Managers.Dungeon.TryEnterNextDungeon();
    }

    private void EnterDungeon(bool result)
    {
        if (!result)
            return;
        
        Managers.Dungeon.TryEnterNextDungeon();
    }
}