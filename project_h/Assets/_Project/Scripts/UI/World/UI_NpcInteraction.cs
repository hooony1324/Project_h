using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_NpcInteraction : UI_Base
{
    public Action OnClickInteraction = () => {};
    public Action OnCollisionInteraction = () => {};



    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        GetComponent<Image>().color = Color.clear;
        GetComponent<Canvas>().sortingOrder = SortingLayers.NPC_INTERACTION;

        return true;
    }

    public void SetInfo(Action interactionAction, bool isClickInteraction)
    {
        if (isClickInteraction)
        {
            OnClickInteraction -= interactionAction;
            OnClickInteraction += interactionAction;

            gameObject.BindEvent(OnClickInteractionArea);
        }
        else
        {
            OnCollisionInteraction -= interactionAction;
            OnCollisionInteraction += interactionAction;

            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            RectTransform rect = GetComponent<RectTransform>();
            collider.size = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y);
        }
    }

    void OnClickInteractionArea()
    {
        OnClickInteraction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
            OnCollisionInteraction.Invoke();
    }
}