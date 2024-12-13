using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class SaveLoadManager
{
    private GameSaveData _gameSaveData;
    public static string Path => Application.persistentDataPath + "/SaveData.json";
    public void Init()
    {
        //InitGame();

        if (File.Exists(Path) == false)
            InitGame();
        else
            LoadGame();

    }


    // public int GenerateEquipmentInstanceID()
    // {
    //     int equipmentInstanceID = _gameSaveData.EquipmentInstanceIDGeneratorNumber;
    //     _gameSaveData.EquipmentInstanceIDGeneratorNumber++;
    //     return equipmentInstanceID;
    // }

    private void InitGame()
    {
        _gameSaveData = new GameSaveData();

        _gameSaveData.DungeonPlayInfo = new DungeonPlayInfo();
        // 초반 진행 데이터 세팅
        // EX)
        // - 첫 퀘스트 세팅
        // - 기본 아이템
        // - 기본 스킬

        SaveGame();
    }

    public void SaveGame()
    {
        _gameSaveData.DungeonPlayInfo.Save();


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


        // Load Dungeon Play Data
        _gameSaveData.DungeonPlayInfo.Load();
        
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