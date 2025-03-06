using UnityEngine;

public class UI_EquipmentPopup : UI_Popup
{
    enum GameObjects
    {
        Content,
    }

    GameObject content;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Contents
        BindGameObjects(typeof(GameObjects));
        content = GetGameObject((int)GameObjects.Content);
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        return true;
    }

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in Managers.Inventory.EquippedItems)
        {
            var slot = Managers.Resource.Instantiate(nameof(UI_EquipmentPopupSlot), content.transform);
            slot.GetComponent<UI_EquipmentPopupSlot>().Setup(item);
        }
    }
}