using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static void Shuffle<T>(this T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // UnityEngine.Random
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // UnityEngine.Random
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
