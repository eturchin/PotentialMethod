using System;
using System.Collections.Generic;

namespace Source
{
    public static class ArrayExtension
    {
        public static void Show(this List<List<float>> array, string text)
        {
            Console.WriteLine(text);
            foreach (var t in array)
            {
                foreach (var t1 in t)
                    Console.Write(float.IsNaN(t1) ? "\t0 " : $"\t{t1} ");

                Console.WriteLine();
            }
        }

        public static void Show(this List<float> array, string text)
        {
            Console.WriteLine($"\n{text}: {string.Join(", ", array)}.");
        }
    }
}