using System.Collections.Generic;

public static class Utils
{
    public static void Shuffle<T>(IList<T> array)
    {
        int n = array.Count;

        while (n > 1)
        {
            int k = UnityEngine.Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
