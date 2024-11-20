using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager
{
    public Dungeon CurrentDungeon { get; private set; }

    private List<DungeonData> _dungeonDatas = new List<DungeonData>();
    int _nextIdx = 0;

    public void SetupDungeons(List<DungeonData> dungeonDatas)
    {
        _dungeonDatas.Clear();
        _dungeonDatas = dungeonDatas.ToList();
    }

    public bool TryEnterNextDungeon()
    {
        if (_dungeonDatas.Count == 0)
            return false;

        // Dungeon마지막에서 TryEnter
        if (_nextIdx >= _dungeonDatas.Count - 1)
        {
            Debug.Log("마지막 던전");
            return false;
        }
        
        Managers.Map.SetMap(_dungeonDatas[_nextIdx].PrefabName);
        Managers.Scene.LoadScene(EScene.DungeonScene);

        _nextIdx++;

        return true;
    }

    public void SetDungeon(Dungeon dungeon)
    {
        CurrentDungeon = dungeon;
    }

    public void Clear()
    {
        _dungeonDatas.Clear();
        CurrentDungeon = null;
        _nextIdx = 0;
    }

    public async Awaitable GenerateDungeon()
    {
        await CurrentDungeon?.GenerateDungeon();
    }
}