using UnityEngine;

public interface IEvent {}

public struct MonsterDeadEvent : IEvent 
{
    // 몬스터 종류
    
}

public struct HeroSelectEvent : IEvent
{
    public int heroDataID;
    public Vector3 selectPosition;
}

public struct HeroDeadEvent : IEvent
{

}

public struct HeroRevialEvent : IEvent
{

}



#if UNITY_EDITOR
public struct TEST_HeroSpawnEvent : IEvent
{

}
#endif