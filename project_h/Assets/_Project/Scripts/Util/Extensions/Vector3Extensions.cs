using UnityEngine;

public static class Vector3Extensions
{
    public static bool InRangeOf(this Vector3 current, Vector3 target, float range)
    {
        return (current - target).sqrMagnitude <= range * range;
    }

    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Vector3 RandomPointInAnnulus(this Vector3 origin, float minRadius, float maxRadius)
    {
        float angle = Random.value * Mathf.PI * 2f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        float minRadiusSquared = minRadius * minRadius;
        float maxRadiusSquared = maxRadius * maxRadius;
        float distance = Mathf.Sqrt(Random.value * (maxRadiusSquared - minRadiusSquared) + minRadiusSquared);

        Vector3 position = new Vector3(direction.x, 0, direction.y) * distance;
        return origin + position;
    }
}