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

            Console.WriteLine("\nDefault Array:");
            price.Show();

            int[] a = {100, 200, 150};
            a.Show("\nSupplies");

            int[] b = {40, 60, 100, 50, 200};
            b.Show("\nRequirement");

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

            Console.WriteLine("\nDelivery:");
            supplies.Show();
            a.Show("\nSupplies");
            Console.WriteLine($"\nSum: {sum}");

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

                        //for (var l = 0; l < supplies.GetLength(0); l++)
                        //{
                        //    if (supplies[l, j] == 0) continue;
                        //    u[l] = price[l, j] - v[j];
                        //}
                    }
                    else if (u[i] == int.MaxValue && v[j] != int.MaxValue)
                    {
                        u[i] = price[i, j] - v[j];

                        //for (var l = 0; l < supplies.GetLength(1); l++)
                        //{
                        //    if (supplies[i, l] == 0) continue;
                        //    v[l] = price[i, l] - u[i];
                        //}
                    }
                }
            }

            v.Show("\nV");
            u.Show("\nU");


            Console.ReadKey();
        }
    }
}