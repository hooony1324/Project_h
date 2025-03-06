using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;


[System.Serializable]
public class DungeonGoldAcquireAction : ItemAcquireAction
{
    public int goldStatID;
    public override void AqcuireAction(Item owner) 
    {
        this.owner = owner;
        
        Vector2Int range = Managers.Dungeon.CurrentDungeonData != null ? Managers.Dungeon.CurrentDungeonData.GoldDropRange : new Vector2Int(1, 3);
        int goldValue = Random.Range(range.x, range.y);

        Stat stat = Managers.Hero.MainHero.StatsComponent.GoldsStat;
        stat.DefaultValue += goldValue;
    }
}