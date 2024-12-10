using System;
using UnityEngine;

public class SaveLoadManager
{
    public string Path => Application.persistentDataPath + "/SaveData.json";
    public void Init()
    {
        InitGame();

        // if (File.Exists(Path) == false)
        //     InitGame();
        // else
        //     LoadGame();
        

    }


    // public int GenerateEquipmentInstanceID()
    // {
    //     int equipmentInstanceID = _gameSaveData.EquipmentInstanceIDGeneratorNumber;
    //     _gameSaveData.EquipmentInstanceIDGeneratorNumber++;
    //     return equipmentInstanceID;
    // }



    
    private void InitGame()
    {

        
    }

    private void LoadGame()
    {

    }

}