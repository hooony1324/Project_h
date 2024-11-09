using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonDoor : InitOnce
{
    public Action OnTriggered = () => {};
    private TilemapCollider2D _collider;

    public override bool Init()
    {
        if (base.Init() == false)
            return true;

        _collider = GetComponent<TilemapCollider2D>();
        //_collider.isTrigger = false;

        return false;
    }

    public void Open()
    {
        _collider.isTrigger = true;
    }

    public void Close()
    {
        _collider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Door Enter...");
        OnTriggered.Invoke();
    }
}