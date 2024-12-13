using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DungeonPlayInfo : ISaveable
{
    // 0아니면 던전 진행 중 종료
    public int ProgressedDungeonId = 0; 


    // 진행 중 던전X, 클리어 했던 이전 던전에서의 플레이어 상태
    public string HeroDataName;

    public void Save()
    {
        ProgressedDungeonId = Managers.Dungeon.CurrentDungeonId;
        HeroDataName = Managers.Game.HeroDataName;
    }

    public void Load()
    {
        if (ProgressedDungeonId != 0)
        {
            DungeonData currentDungeonData = Managers.Data.GetDungeonData(ProgressedDungeonId);
            Managers.Dungeon.SetFirstDungeon(currentDungeonData);            
        }
        
        if (HeroDataName != null)
        {
            Managers.Game.SetHeroData(HeroDataName);
        }
    }
}