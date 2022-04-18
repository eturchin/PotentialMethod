using System;

namespace Source
{
    public static class ArrayExtension
    {
        public static void Show(this float[,] array, string text)
        {
            Console.WriteLine(text);
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                    Console.Write(array[i, j] == float.NaN ? "inf " : $"{array[i, j]} ");
                Console.WriteLine();
            }
        }
        public static void Show(this bool[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                    Console.Write($"{array[i, j]} ");
                Console.WriteLine();
            }
        }
        public static void Show(this float[] array, string text)
        {
            Console.WriteLine($"\n{text}: {string.Join(", ", array)}.");
        }
    }
}