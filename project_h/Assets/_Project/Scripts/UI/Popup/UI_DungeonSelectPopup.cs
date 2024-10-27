using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using System.Linq;

public class UI_DungeonSelectPopup : UI_Popup
{
    enum GameObjects
    {
        Content_DungeonStages,
    }

    enum TmpTexts
    {
        DungeonNameText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindGameObjects(typeof(GameObjects));
        BindTMPTexts(typeof(TmpTexts));
        
        return true;
    }

    private List<DungeonData> _dungeonDatas = new List<DungeonData>();
    public void SetInfo(List<DungeonData> dungeonDatas)
    {
        GameObject content = GetGameObject((int)GameObjects.Content_DungeonStages);

        foreach (Transform child in content.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        _dungeonDatas.Clear();
        _dungeonDatas = dungeonDatas.Select(data => (DungeonData)data.Clone()).ToList();

        foreach (DungeonData dungeonData in dungeonDatas)
        {
            UI_DungeonStageSlot slot = Managers.UI.MakeSubItem<UI_DungeonStageSlot>(content.transform);
            slot.SetInfo(dungeonData);
        }
    }

}