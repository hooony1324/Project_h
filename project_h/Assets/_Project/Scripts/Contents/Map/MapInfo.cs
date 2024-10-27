using System.Collections;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;

public class MapInfo : InitOnce
{
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