using System;
using Random = UnityEngine.Random;

public static class EnumsExtensions
{
    public static T GetRandomEnum<T>() where T : Enum
    {
        var values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }
}