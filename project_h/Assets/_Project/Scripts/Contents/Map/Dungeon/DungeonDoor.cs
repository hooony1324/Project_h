using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public class DungeonDoor : BaseObject
{
    public Action OnTriggered = () => {};
    private TilemapCollider2D _collider;

    public Vector3 TeleportPosition { get; private set; }

    public DungeonRoom Owner { get; private set; }

    public DungeonRoom TargetRoom { get; private set; }

    private DungeonDoor _targetDoor;

    private bool _isOpened = false;
    private SpriteRenderer _spriteRenderer;

    public override bool Init()
    {
        if (base.Init() == false)
            return true;

        _collider = GetComponent<TilemapCollider2D>();
        _collider.isTrigger = true;
        _spriteRenderer = Util.FindChild<SpriteRenderer>(gameObject, "Sprite").GetComponent<SpriteRenderer>();

        return false;
    }
    
    public void SetInfo(DungeonRoom owner, DungeonRoom targetRoom, Vector3 teleportPosition, DungeonDoor targetDoor)
    {
        TargetRoom = targetRoom;
        TeleportPosition = teleportPosition;
        _targetDoor = targetDoor;
        Owner = owner;

        SetupDoor();
    }

    public void Open()
    {
        _isOpened = true;

        // 열림 처리(애니메이션, Sprite, 소리 등)
    }

    public void Close()
    {
        _isOpened = false;

        // 닫힘 처리(애니메이션, Sprite, 소리 등)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        // 만약 열쇠개념 추가하면
        // room.CheckOpenCondition
        // room.InvokeOpenEvent

        if (!_isOpened)
            return;


        MoveToTargetRoom();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Owner.IsWaveCleared)
            Open();
    }

    void MoveToTargetRoom()
    {
        // Teleport to TargetRoom
        _targetDoor.Close();    // 왔다갔다 방지
        Managers.Hero.TeleportHero(TeleportPosition);
        
        TargetRoom.HandleHeroVisited();
    }

    void SetupDoor()
    {
        switch (TargetRoom.RoomType)
        {
            case EDungeonRoomType.BossMonster:
                HandleBossRoom();
                break;
            default:
                HandleNormal();
                break;
        }
    }

    void HandleNormal()
    {
        _spriteRenderer.color = Color.white;
    }

    void HandleBossRoom()
    {
        _spriteRenderer.color = Color.red;
    }

}