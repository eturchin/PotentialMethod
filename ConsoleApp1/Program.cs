using System;

namespace Source
{
    internal class Program
    {
        private static void Main()
        {
            float[,] price =
            {
                { 4, 1, 2, 3, 0 },
                { 3, 6, 1000, 4, 0 },
                { 1000, 2, 3, 5, 0 }
            };


            float[] a = { 100, 200, 150 };
            a.Show("Запасы");

            float[] b = { 40, 60, 100, 50, 200 };
            b.Show("Заявки");

            price.Show("\nМатрица стоимостей:");


            var transportProblem = new TransportProblem(a, b, price);

            var supplies = transportProblem.NordWest();

            supplies.Show("\nSupplies:");
            float sum = 0;
            for (var i = 0; i < supplies.GetLength(0); i++)
            for (var j = 0; j < supplies.GetLength(1) - 1; j++)
                if (!float.IsNaN(supplies[i, j]) && !float.IsNaN(price[i, j]))
                    sum += supplies[i, j] * price[i, j];
            Console.WriteLine($"\nSum = {sum}");

            var result = transportProblem.PotentialMethod(supplies);
            sum = 0;
            for (var i = 0; i < result.GetLength(0); i++)
            for (var j = 0; j < result.GetLength(1) - 1; j++)
                if (!float.IsNaN(result[i, j]) && !float.IsNaN(price[i, j]))
                    sum += result[i, j] * price[i, j];

            result.Show("\nResult:");
            Console.WriteLine($"\nSum = {sum}");
        }
    }
}