using System.Collections;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class Map : InitOnce
{
    public Vector3 StartPosition => _startPoint.position;

    [SerializeField] NavMeshSurface _navMeshSurface;
    [SerializeField] Transform _startPoint;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    // nav 굽기
    // navBound비활성화
    // startPoint -> 갈 수 있는지 Check
    // MonsterSpawner와 연동하여 해당 맵이 정리될 때 스폰된 몬스터도 정리

}