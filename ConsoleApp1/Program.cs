using System;

namespace Source
{
    internal class Program
    {
        private static void Main()
        {
            int[,] price =
            {
                {4, 1, 2, 3, 0},
                {3, 6, int.MaxValue, 4, 0},
                {int.MaxValue, 2, 3, 5, 0}
            };

            Console.WriteLine("\nМатрица стоимостей:");
            price.Show();

            int[] a = {100, 200, 150};
            a.Show("Запасы");

            int[] b = {40, 60, 100, 50, 200};
            b.Show("Заявки");

            var bufArr = new int[price.Length];
            var k = 0;

            for (var i = 0; i < price.GetLength(0); i++)
            for (var j = 0; j < price.GetLength(1) - 1; j++)
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
                for (var j = 0; j < price.GetLength(1) - 1; j++)
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

            for (var i = 0; i < price.GetLength(0); i++)
            {
                if (a[i] == 0) continue;
                supplies[i, price.GetLength(1) - 1] = a[i];
                a[i] = 0;
            }

            Console.WriteLine("\nПоставки:");
            supplies.Show();
            a.Show("Запасы");
            Console.WriteLine($"\nСумма: {sum}");

            var u = new int[a.Length];

            for (var i = 0; i < u.Length; i++)
            {
                u[i] = int.MaxValue;
            }
            u[0] = 0;

            var v = new int[b.Length];

            for (var i = 0; i < b.Length; i++)
            {
                v[i] = int.MaxValue;
            }

            for (var s = 0; s < supplies.GetLength(0); s++)
            {
                for (var i = 0; i < supplies.GetLength(0); i++)
                for (var j = 0; j < supplies.GetLength(1); j++)
                {
                    if (supplies[i, j] == 0) continue;
                    if (v[j] == int.MaxValue && u[i] != int.MaxValue)
                    {
                        v[j] = price[i, j] - u[i];
                    }
                    else if (u[i] == int.MaxValue && v[j] != int.MaxValue)
                    {
                        u[i] = price[i, j] - v[j];
                    }
                }
            }

            v.Show("V");
            u.Show("U");

            var c = true;

            for (var i = 0; i < price.GetLength(0); i++)
            for (var j = 0; j < price.GetLength(1); j++)
            {
                if (supplies[i, j] == 0) continue;
                if (u[i] + v[j] > price[i, j])
                {
                    c = false;
                }
            }

            if (c == true)
            {
                Console.WriteLine($"\nРешение является оптимальным, где f(x) = {sum}");
            }
            else
            {
                Console.WriteLine($"\nРешение не является оптимальным, где f(x) = {sum}");
            }

            Console.ReadKey();
        }
    }
}