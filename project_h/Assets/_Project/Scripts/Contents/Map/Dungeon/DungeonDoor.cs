using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonDoor : InitOnce
{
    public Action OnTriggered = () => {};
    private TilemapCollider2D _collider;

    public Vector3 TeleportPosition { get; private set; }

    public DungeonRoom Owner { get; private set; }

    public DungeonRoom TargetRoom { get; private set; }

    private DungeonDoor _targetDoor;

    private bool _isOpened = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return true;

        _collider = GetComponent<TilemapCollider2D>();
        _collider.isTrigger = true;

        return false;
    }
    
    public void SetInfo(DungeonRoom owner, DungeonRoom targetRoom, Vector3 teleportPosition, DungeonDoor targetDoor)
    {
        TargetRoom = targetRoom;
        TeleportPosition = teleportPosition;
        _targetDoor = targetDoor;
        Owner = owner;
    }

    public void Open()
    {
        _isOpened = true;
    }

    public void Close()
    {
        _isOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        if (!_isOpened)
            return;

        // Teleport to TargetRoom
        _targetDoor.Close();// 왔다갔다 방지
        Managers.Hero.TeleportHero(TeleportPosition);
        
        //OnTriggered.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Owner.IsCleared)
            Open();
    }

}