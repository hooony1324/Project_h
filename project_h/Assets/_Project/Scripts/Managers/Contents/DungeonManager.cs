using UnityEngine;

public class DungeonManager
{
    public Dungeon CurrentDungeon { get; private set; }

    public void SetDungeon(Dungeon dungeon)
    {
        CurrentDungeon = dungeon;
    }

    public void Clear()
    {
        CurrentDungeon = null;
    }

    public async Awaitable GenerateDungeon()
    {
        await CurrentDungeon?.GenerateDungeon();
    }
}