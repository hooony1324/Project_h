using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_NpcInteraction : UI_Base
{
    public Action OnInteraction;

    enum GameObjects
    {
        InteractionArea    
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));

        GetGameObject((int)GameObjects.InteractionArea).BindEvent(OnClickInteractionArea);
        GetGameObject((int)GameObjects.InteractionArea).GetComponent<Image>().enabled = false;

        return true;
    }

    void OnClickInteractionArea()
    {
        OnInteraction?.Invoke();
    }
}