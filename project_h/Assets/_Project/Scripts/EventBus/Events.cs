using UnityEngine;

public interface IEvent {}

public struct MonsterDeadEvent : IEvent 
{
    // 몬스터 종류
    
}


// 첫 BaseMap에 입장했을 때
// - 진행 중 던전 정보 해당 UI에 뿌리기 > UI Popup
public struct GameStartEvent : IEvent
{

}

public struct HeroSelectEvent : IEvent
{
    public int heroDataID;
    public Vector3 selectPosition;
}

public struct TEST_HeroSpawnEvent : IEvent
{

}