using UnityEngine;
using UnityEngine.UI;


public class UI_EquipmentPopupSlot : UI_Base
{
    Image itemImage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        itemImage = GetComponent<Image>();
        gameObject.SetActive(false);
        return true;
    }

    public void Setup(Item item)
    {
        if (item.IsEquipment == false)
            return;
        itemImage.sprite = item.ItemHolderSprite;
        gameObject.SetActive(true);
    }

}