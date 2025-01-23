using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_DungeonScene : UI_Scene
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
