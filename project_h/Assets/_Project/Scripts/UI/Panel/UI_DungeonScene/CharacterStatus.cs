using UnityEngine;


public class CharacterStatus : UI_Base 
{
    enum GameObjects
    {
        HpPanel,
        InventoryButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
            
        BindGameObjects(typeof(GameObjects));

        GetGameObject((int)GameObjects.InventoryButton).BindEvent(OnClickEquipment);

        return true;
    }

    public void Setup(Entity mainHero)
    {
        GetGameObject((int)GameObjects.HpPanel).GetComponent<HpPanel>().Setup(mainHero);
    }

    void OnClickEquipment()
    {
        var popup = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
    }
}