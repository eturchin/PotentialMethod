using System;

namespace Source
{
    public static class ArrayExtension
    {
        public static void Show(this int[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                    Console.Write(array[i, j] == int.MaxValue ? "inf " : $"{array[i, j]} ");
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
        public static void Show(this int[] array, string text)
        {
            Console.WriteLine($"\n{text}: {string.Join(", ", array)}.");
        }
    }
}