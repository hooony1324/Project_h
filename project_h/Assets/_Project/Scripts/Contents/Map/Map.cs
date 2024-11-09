using UnityEngine;

public class Map : InitOnce
{
    // List<DungeonRoom> _dungeonRooms;
    // DungeonRoom _startRoom;
    // _startRoom.HeroSpawnPosition

    // Init과정
    // 1. LinkRooms : Link Doors and Transport other Room
    //  - 현재는 프리팹에 미리 위치시킴, 추후에 Procedural되면 위치를 랜덤 자동 배치
    // 2. 

    public Vector3 StartPosition => _startPoint.position;
    private Transform _startPoint;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _startPoint = Util.FindChild(gameObject, "StartPoint").transform;

        return true;
    }


}