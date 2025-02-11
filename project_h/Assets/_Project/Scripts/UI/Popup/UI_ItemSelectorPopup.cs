using System.Linq;
using UnityEngine;
using static Define;

public struct ItemSelectorInfo
{
    public int selectSkillID;
    public int resultSkillID;
    public EDefaultSkillSlot skillSlot;
}

public class UI_ItemSelectorPopup : UI_Popup
{
    enum GameObjects
    {
        ItemSelectorSlots,
    }

    enum TMPTexts
    {
        HeaderText,
    }
    

    private UI_ItemSelectorSlot[] itemSelectorSlots;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));
        BindTMPTexts(typeof(TMPTexts));

        itemSelectorSlots = new UI_ItemSelectorSlot[3];
        GameObject itemSelectorSlotObj = GetGameObject((int)GameObjects.ItemSelectorSlots);
        int index = 0;
        foreach (Transform child in itemSelectorSlotObj.transform)
        {
            itemSelectorSlots[index++] = child.GetComponent<UI_ItemSelectorSlot>();
        }

        return true;
    }

    public void Setup(ItemSelectorInfo[] infos)
    {
        int index = 0;
        foreach (UI_ItemSelectorSlot slot in itemSelectorSlots)
        {
            slot.gameObject.SetActive(false);
            
            if (index < infos.Count())
            {
                var info = infos[index++];
                slot.Setup(info);
                slot.gameObject.SetActive(true);
            }
        }
    }

    void OnEnable()
    {
        Managers.Game.PauseGame();
    }

    void OnDisable()
    {
        Managers.Game.ResumeGame();
    }
}