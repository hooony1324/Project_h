using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using static Define;

public class ItemHolder : BaseObject
{
    private Item item;
    private DropData dropData;
    private SpriteRenderer spriteRenderer;

    private Action endCallback;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        spriteRenderer = Util.FindChild(gameObject, "Sprite").GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = SortingLayers.DROP_ITEM;

        return true;
    }

    public void Setup(DropData dropData, Action endCallback = null)
    {
        this.dropData = dropData;
        this.item = Managers.Data.GetItemData(dropData.itemID);
        this.endCallback = endCallback;

        spriteRenderer.sprite = item.ItemHolderSprite;

        // parabola motion
        _ = DoParabolaMotion();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") == false)
            return;

        item.Acquire();
        Managers.Object.Despawn<ItemHolder>(this);
    }


    async UniTask DoParabolaMotion()
    {
        float speed = 5f;
        float heightArc = 2f;
        Vector2 startPos = Position;
        Vector2 destPos = startPos.RandomPointInAnnulus(0, 3);
        float duration = Vector2.Distance(startPos, destPos) / speed;

        var tween = transform.DOPath(
            new Vector3[] {
                startPos,
                new Vector3((startPos.x + destPos.x) * 0.5f, Mathf.Max(startPos.y, destPos.y) + heightArc, 0f),
                destPos
            },
            duration,
            PathType.CatmullRom)
            .SetEase(Ease.OutExpo);

        await tween.AsyncWaitForCompletion();
        endCallback?.Invoke();
    }
}