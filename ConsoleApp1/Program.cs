using System;

namespace Source
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int[,] price =
            {
                {4, 1, 2, 3},
                {3, 6, int.MaxValue, 4},
                {int.MaxValue, 2, 3, 5}
            };

            Console.WriteLine("\nDefault Array:");
            price.Show();

            int[] a = {100, 200, 150};
            a.Show("\nSupplies");

            int[] b = {40, 60, 100, 50};
            b.Show("\nRequirement");

            var bufArr = new int[price.Length];
            var k = 0;

            for (var i = 0; i < price.GetLength(0); i++)
            for (var j = 0; j < price.GetLength(1); j++)
            {
                bufArr[k] = price[i, j];
                k++;
            }

            Array.Sort(bufArr);

            var supplies = new int[price.GetLength(0), price.GetLength(1)];
            var sum = 0;

            for (k = 0; k < bufArr.Length; k++)
            {
                for (var i = 0; i < price.GetLength(0); i++)
                for (var j = 0; j < price.GetLength(1); j++)
                    if (bufArr[k] == price[i, j])
                        if (a[i] != 0 && b[j] != 0)
                        {
                            if (a[i] >= b[j])
                            {
                                supplies[i, j] = b[j];
                                a[i] -= b[j];
                                b[j] = 0;
                            }
                            else if (a[i] < b[j])
                            {
                                supplies[i, j] = a[i];
                                b[j] -= a[i];
                                a[i] = 0;
                            }

                            sum += supplies[i, j] * price[i, j];
                        }

            }

            Console.WriteLine("\nDelivery:");
            supplies.Show();
            a.Show("\nSupplies");
            Console.WriteLine($"\nSum: {sum}");

            int[] u = new int[a.Length];
            u[1] = 0;
            int[] v = new int[b.Length + 1];

            Console.ReadKey();
        }
    }
}