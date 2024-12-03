using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;

public class Map : InitOnce
{
    public NavMeshSurface NavMeshSurface2D => _navMeshSurface;
    private NavMeshSurface _navMeshSurface;

    public Vector3 StartPosition => _startPoint.position;
    private Transform _startPoint;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _startPoint = Util.FindChild(gameObject, "StartPoint").transform;

        GameObject navMesh = Managers.Resource.Instantiate("NavMesh");
        if (navMesh == null)
        {
            navMesh = GameObject.Find("NavMesh");
        }
        navMesh.name = "@NavMesh";
        _navMeshSurface = navMesh.GetComponent<NavMeshSurface>();

        return true;
    }
}