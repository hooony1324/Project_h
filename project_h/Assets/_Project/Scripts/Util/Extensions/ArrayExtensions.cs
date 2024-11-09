using UnityEngine;

public static class ArrayExtensions
{
    public static void ClearWith<T>(this T[,] array, T value)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = value;
            }
        }
    }
}