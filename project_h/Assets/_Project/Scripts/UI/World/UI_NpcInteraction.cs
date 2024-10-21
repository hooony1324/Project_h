using System;
using UnityEngine;
using UnityEngine.UI;
using static Define;

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
        GetGameObject((int)GameObjects.InteractionArea).GetComponent<Image>().color = Color.clear;

        GetComponent<Canvas>().sortingOrder = SortingLayers.NPC_INTERACTION;

        return true;
    }

    void OnClickInteractionArea()
    {
        OnInteraction?.Invoke();
    }
}