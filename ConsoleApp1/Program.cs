using System;
using System.Collections.Generic;
using System.Threading;

namespace Source
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.Write("1 - Индивидуальное задание\n" +
                              "2 - Решение транспортной задачи\n" +
                              "3 - О программе\n" +
                              "0 - Выход\n" +
                              "\nВвод: ");
                int key;
                while (!int.TryParse(Console.ReadLine(), out key)) Console.Write("Ошибка! Введите целое число:");

                switch (key)
                {
                    case 1:
                        var price = new List<List<float>>
                        {
                            new() { 4, 1, 2, 3 },
                            new() { 3, 6, 100, 4 },
                            new() { 100, 2, 3, 5 }
                        };

                        var a = new List<float> { 100, 200, 150 };
                        a.Show("Запасы");

                        var b = new List<float> { 40, 60, 100, 50 };
                        b.Show("Заявки");

                        price.Show("\nМатрица стоимостей:");


                        var transportProblem = new TransportTask(a, b, price);

                        price.Show("\nМатрица стоимостей:");

                        var supplies = transportProblem.NorthWestern();

                        supplies.Show("\nОппорный план:");

                        float sum = 0;

                        for (var i = 0; i < supplies.Count; i++)
                        for (var j = 0; j < supplies[i].Count; j++)
                            if (!float.IsNaN(supplies[i][j]) && !float.IsNaN(price[i][j]))
                                sum += supplies[i][j] * price[i][j];

                        Console.WriteLine($"\nСумма = {sum}");

                        var result = transportProblem.PotentialMethod(supplies);
                        sum = 0;

                        for (var i = 0; i < result.Count; i++)
                        for (var j = 0; j < result[i].Count; j++)
                            if (!float.IsNaN(result[i][j]) && !float.IsNaN(price[i][j]))
                                sum += result[i][j] * price[i][j];

                        result.Show("\nРезультат:");
                        Console.WriteLine($"\nСумма = {sum}");
                        break;
                    case 2:
                        Console.WriteLine("Введите размер матрицы: ");
                        Console.Write("\nКоличество строк n = ");

                        int n;

                        while (!int.TryParse(Console.ReadLine(), out n)) Console.Write("Ошибка! Введите целое число:");

                        Console.Write("\nКоличество столбцов m = ");

                        int m;

                        while (!int.TryParse(Console.ReadLine(), out m)) Console.Write("Ошибка! Введите целое число:");

                        Console.WriteLine("\nЗаполните матрицу:\n");

                        var price1 = new List<List<float>>();

                        for (var i = 0; i < n; i++)
                        {
                            var buf = new List<float>();

                            for (var j = 0; j < m; j++)
                            {
                                int k;

                                Console.Write($"Стоимость из пункта {i + 1} в пункт {j + 1}: ");
                                while (!int.TryParse(Console.ReadLine(), out k))
                                    Console.Write("Ошибка! Введите целое число:");
                                buf.Add(k);
                            }

                            price1.Add(buf);
                        }

                        var a1 = new List<float>();

                        for (var i = 0; i < n; i++)
                        {
                            int k;

                            Console.Write($"Количество запасов в пункте {i + 1}: ");

                            while (!int.TryParse(Console.ReadLine(), out k))
                                Console.Write("Ошибка! Введите целое число:");

                            a1.Add(k);
                        }

                        a1.Show("Запасы");

                        var b1 = new List<float>();

                        for (var j = 0; j < m; j++)
                        {
                            int k;

                            Console.Write($"Количество заявок в пункте {j + 1}: ");

                            while (!int.TryParse(Console.ReadLine(), out k))
                                Console.Write("Ошибка! Введите целое число:");

                            b1.Add(k);
                        }

                        b1.Show("\nЗаявки");

                        price1.Show("\nМатрица стоимостей:");


                        var transportProblem1 = new TransportTask(a1, b1, price1);

                        price1.Show("\nМатрица стоимостей:");

                        var supplies1 = transportProblem1.NorthWestern();

                        supplies1.Show("\nОппорный план:");

                        float sum1 = 0;

                        for (var i = 0; i < supplies1.Count; i++)
                        for (var j = 0; j < supplies1[i].Count; j++)
                            if (!float.IsNaN(supplies1[i][j]) && !float.IsNaN(price1[i][j]))
                                sum1 += supplies1[i][j] * price1[i][j];

                        Console.WriteLine($"\nСумма = {sum1}");

                        var result1 = transportProblem1.PotentialMethod(supplies1);
                        sum1 = 0;

                        for (var i = 0; i < result1.Count; i++)
                        for (var j = 0; j < result1[i].Count; j++)
                            if (!float.IsNaN(result1[i][j]) && !float.IsNaN(price1[i][j]))
                                sum1 += result1[i][j] * price1[i][j];

                        result1.Show("\nРезультат:");
                        Console.WriteLine($"\nСумма = {sum1}");
                        break;
                    case 3:
                        Console.WriteLine("\nРазработчик - Турчин Егор Викторович" +
                                          "\nГруппа - А01ИСТ2" +
                                          "\nПрограмма решения и оптимизации транспортной задачи методом потенциалов" +
                                          "\nПримичание: Если перевозка из пункта i в пункт j невозможна, то требуется ввести число больше всех остальных");
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Выберите один из пунктов меню.\n");
                        Console.ReadKey();
                        break;
                }

                Console.WriteLine("\nНажмите любую кнопку для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}