using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RuntimeNavMeshBaker : MonoBehaviour
{
    private NavMeshSurface _surface2D;

    public IEnumerator Start()
    {
        _surface2D = GetComponent<NavMeshSurface>();

        if (_surface2D.useGeometry == NavMeshCollectGeometry.PhysicsColliders)
        {
            yield return new WaitForFixedUpdate();
        }
        _surface2D.BuildNavMesh();
        yield return null;
    }


}