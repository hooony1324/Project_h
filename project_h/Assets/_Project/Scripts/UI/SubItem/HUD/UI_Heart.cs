using UnityEngine;


public class UI_Heart : UI_Base
{
    enum GameObjects
    {
        OnIcon,
        OffIcon,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));

        return true;
    }

    public void On()
    {
        GetGameObject((int)GameObjects.OnIcon).SetActive(true);
        GetGameObject((int)GameObjects.OffIcon).SetActive(false);
    }

    public void Off()
    {
        GetGameObject((int)GameObjects.OnIcon).SetActive(false);
        GetGameObject((int)GameObjects.OffIcon).SetActive(true);
    }
}