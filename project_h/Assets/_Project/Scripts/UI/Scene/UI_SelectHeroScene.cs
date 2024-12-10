using UnityEngine;

public class UI_SelectHeroScene : UI_Scene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        GetComponent<Canvas>().worldCamera = Camera.main;

        return true;
    }

    
}
