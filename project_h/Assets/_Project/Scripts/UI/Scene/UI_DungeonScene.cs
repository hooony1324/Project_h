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
    }

    

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
            
        BindGameObjects(typeof(GameObjects));

        return true;
    }

    public void Setup(Entity mainHero)
    {
        GetGameObject((int)GameObjects.HpPanel).GetComponent<HpPanel>().Setup(mainHero);
    }

}
