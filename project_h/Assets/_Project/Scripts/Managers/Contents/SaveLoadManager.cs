using System;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;



#if UNITY_EDITOR
using UnityEditor;
#endif 

[Serializable]
public class GameSaveData
{
    public int ProgressedDungeonId;   // 0아니면 던전 진행 중 종료
    public int HeroDataID = -1;
    public int DefaultAttackID;
    public int DodgeID;
    public List<StatSaveData> StatSaveDatas = new List<StatSaveData>();
    public List<int> ItemSaveDatas = new List<int>();
    public List<int> PassiveSkills = new();
}

public class SaveLoadManager
{
    private GameSaveData _gameSaveData = new GameSaveData();
    public GameSaveData SaveData => _gameSaveData;
    public static string Path => Application.persistentDataPath + "/SaveData.json";

    public void Init()
    {
        if (File.Exists(Path) == false)
            InitGame();
        else
            LoadGame();
    }

    private void InitGame()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        // 히어로 정보
        Managers.Hero.SavePlayDatas();

        // 플레이 중인 던전
        SaveData.ProgressedDungeonId = Managers.Dungeon.CurrentDungeonId;

        // 플레이 중인 히어로
        SaveData.HeroDataID = Managers.Hero.HeroDataID;

        // 스탯
        SaveData.StatSaveDatas.Clear();
        SaveData.StatSaveDatas = Managers.Hero.StatSaveDatas.ToList(); 

        // 아이템
        SaveData.ItemSaveDatas.Clear();
        SaveData.ItemSaveDatas = Managers.Inventory.AllItems.ToList();

        // 스킬
        SaveData.DefaultAttackID = Managers.Hero.DefaultAttackID;
        SaveData.DodgeID = Managers.Hero.DodgeID;

        SaveData.PassiveSkills.Clear();
        SaveData.PassiveSkills = Managers.Hero.PassiveSkills.ToList();

        string jsonStr = JsonUtility.ToJson(_gameSaveData);
        File.WriteAllText(Path, jsonStr);
    }

    public void LoadGame()
    {
        string fileStr = File.ReadAllText(Path);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(fileStr);

        if (saveData == null)
        {
            Debug.LogError($"Fail To Load Data : {Path}");
            return;
        }

        _gameSaveData = saveData;
        
        // 플레이 중인 던전
        DungeonData currentDungeonData = Managers.Data.GetDungeonData(SaveData.ProgressedDungeonId);
        Managers.Dungeon.SetFirstDungeon(currentDungeonData);   

        // 플레이 중인 히어로
        Managers.Hero.LoadHeroData(SaveData.HeroDataID);

        // 스탯
        Managers.Hero.LoadStatSaveData(SaveData.StatSaveDatas);

        // 아이템
        Managers.Inventory.LoadItemSaveData(SaveData.ItemSaveDatas);

        // 스킬
        Managers.Hero.LoadSkills();
    }

    public void LoadPlayData()
    {
        // 로드한 데이터를 적용
        // - 스탯, 스킬, 아이템
        Managers.Hero.LoadPlayDatas();
        Managers.Inventory.LoadItems();
    }


    public void RemovePlayData()
    {
        // TODO: 로그라이크?
        if (File.Exists(Path))
        {
            File.Delete(Path);
            Debug.Log("Savedata removed!");
        }
        else
        {
            Debug.Log("Savedata not found!");
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/RemoveSavedata")]
    public static void RemoveSaveData()
    {
        if (File.Exists(Path))
        {
            File.Delete(Path);
            Debug.Log("Savedata removed!");
        }
        else
        {
            Debug.Log("Savedata not found!");
        }
    }

#endif

}

[Serializable]
public class StatSaveData
{
    public int ID;
    public float DefaultValue;
    public float MaxValue;
}
