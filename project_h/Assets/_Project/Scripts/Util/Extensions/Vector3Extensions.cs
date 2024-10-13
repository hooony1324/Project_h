using UnityEngine;

public static class Vector3Extensions
{
    public static bool InRangeOf(this Vector3 current, Vector3 target, float range) 
    {
        return (current - target).sqrMagnitude <= range * range;
    }
}