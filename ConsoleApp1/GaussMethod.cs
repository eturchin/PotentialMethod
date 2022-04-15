namespace Source
{
    internal class Maths
    {
        public static double[] Gauss(double[,] matrix)
        {
            var n = matrix.GetLength(0); //Размерность начальной матрицы (строки)
            var matrixClone = new double[n, n + 1]; //Матрица-дублер
            for (var i = 0; i < n; i++)
            for (var j = 0; j < n + 1; j++)
                matrixClone[i, j] = matrix[i, j];

            //Прямой ход (Зануление нижнего левого угла)
            for (var k = 0; k < n; k++) //k-номер строки
            {
                for (var i = 0; i < n + 1; i++) //i-номер столбца
                    matrixClone[k, i] =
                        matrixClone[k, i] /
                        matrix[k, k]; //Деление k-строки на первый член !=0 для преобразования его в единицу
                for (var i = k + 1; i < n; i++) //i-номер следующей строки после k
                {
                    var d = matrixClone[i, k] / matrixClone[k, k]; //Коэффициент
                    for (var j = 0; j < n + 1; j++) //j-номер столбца следующей строки после k
                        matrixClone[i, j] =
                            matrixClone[i, j] -
                            matrixClone[k, j] *
                            d; //Зануление элементов матрицы ниже первого члена, преобразованного в единицу
                }

                for (var i = 0; i < n; i++) //Обновление, внесение изменений в начальную матрицу
                for (var j = 0; j < n + 1; j++)
                    matrix[i, j] = matrixClone[i, j];
            }

            //Обратный ход (Зануление верхнего правого угла)
            for (var k = n - 1; k > -1; k--) //k-номер строки
            {
                for (var i = n; i > -1; i--) //i-номер столбца
                    matrixClone[k, i] = matrixClone[k, i] / matrix[k, k];
                for (var i = k - 1; i > -1; i--) //i-номер следующей строки после k
                {
                    var d = matrixClone[i, k] / matrixClone[k, k];
                    for (var j = n; j > -1; j--) //j-номер столбца следующей строки после k
                        matrixClone[i, j] = matrixClone[i, j] - matrixClone[k, j] * d;
                }
            }

            //Отделяем от общей матрицы ответы
            var answer = new double[n];
            for (var i = 0; i < n; i++)
                answer[i] = matrixClone[i, n];

            return answer;
        }
    }
}