using System;
using System.Collections.Generic;

public interface ISaveable
{
    public void Save();
    public void Load();
}

[Serializable]
public class GameSaveData
{
    public DungeonPlayInfo DungeonPlayInfo;
    
    //public int ItemDbIdGenerator = 1; // Instance된 순서(동일 아이템 구분 용도)
}