using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace balancing
{
    class Program
    {
        public static int caunter = 0;
        public static int rightElements = 0;
        

        static void Main(string[] args)
        {
            SortedList elemPosition = new SortedList();
            double[] koeff = new double[100];
            string tempstring;
            char plus = '+', equal = '=', star = '*', parenthesisL = '(', parenthesisR = ')';
            Fourth:
            double temp = 0, tempEq = 0, koef = 1, parenthesisKoef = 1;
            bool k;
            int j, u = 0;
            Console.Clear();
            Console.WriteLine("Введите уравнение.\n\nВсе формулы должны быть введены в нормальном виде (например CaSO4*0.5H2O) и не должны содержать больше 1 пары круглых и 1 пары квадратных скобок, а так же одного символа «*». Вещества разделяются знаками «+» или «=». Допустимо любое число пробелов.");
            string eq = Console.ReadLine();
            eq = eq.Replace(" ", string.Empty);
            for (int i = 0; i < eq.Length; i++)
            {
                if (eq[i].ToString() == equal.ToString())
                    tempEq++;
            }
            if (tempEq > 1)
            {
                Console.WriteLine("Введено два знака равно. Проверьте уравнение и нажмите Enter для повторого ввода.\n");
                ConsoleKeyInfo clr = Console.ReadKey();
                goto Fourth;
            }

            GausMethod Solution = new GausMethod(rows(eq), colomns(eq));
            Solution.RowCount = 0;
            Solution.ColumCount = 0;

            for (int i = 0; i < eq.Length; i++)
            {
                temp = 0;
                Third:
                bool y = Double.TryParse(eq[i].ToString(), out koeff[i]);
                //KOEFF___________________________________________________________________________________________________________
                if (y && i + 1 < eq.Length && caunter < 0)
                {
                    for (j = i + 1; j < eq.Length; j++)
                    {
                        y = Double.TryParse(eq[j].ToString(), out koeff[j]);
                        if (y == false)
                            goto First;
                    }

                    First:
                    koef = koeff[i];
                    for (int g = i + 1; g < j; g++)
                    {
                        koef = Double.Parse(koef.ToString() + koeff[g].ToString());
                    }
                    caunter++;
                }

                //ELEMENTS________________________________________________________________________________________________

                else if (System.Char.IsUpper(eq[i]) == true)
                {
                    if (i + 1 < eq.Length)
                    {
                        bool l = Double.TryParse(eq[i + 1].ToString(), out koeff[i]);

                        //TWO LETTERS__________________________________________________________________________________________________
                        if (System.Char.IsLower(eq[i + 1]) == true)
                        {

                            tempstring = eq[i].ToString() + eq[i + 1].ToString();
                            if (i + 2 < eq.Length && Double.TryParse(eq[i + 2].ToString(), out koeff[i]) == true)
                            {
                                for (j = i + 3; j < eq.Length; j++)
                                {
                                    k = Double.TryParse(eq[j].ToString(), out koeff[j - 2]);
                                    if (k == false)
                                        goto Second;
                                }

                                Second:
                                temp += koeff[i];
                                for (int g = i + 3; g < j; g++)
                                {
                                    temp = Double.Parse(temp.ToString() + koeff[g - 2].ToString());
                                }
                                addElements(tempstring, ref Solution.RowCount, Solution.ColumCount, rightElements, parenthesisKoef * koef, temp, ref elemPosition, Solution.Matrix, caunter);

                            }
                            else
                            {
                                addElements(tempstring, ref Solution.RowCount, Solution.ColumCount, rightElements, parenthesisKoef * koef, 1, ref elemPosition, Solution.Matrix, caunter);
                            }
                        }
                        //ONE LETTERS AND KOEFF_____________________________________________________________________________________
                        else if (l)
                        {
                            for (j = i + 2; j < eq.Length; j++)
                            {
                                l = Double.TryParse(eq[j].ToString(), out koeff[j - 1]);
                                if (l == false)
                                    goto Second;
                            }

                            Second:
                            temp += koeff[i];
                            for (int g = i + 2; g < j; g++)
                            {
                                temp += Double.Parse(temp.ToString() + koeff[g - 1].ToString());
                            }

                            addElements(eq[i].ToString(), ref Solution.RowCount, Solution.ColumCount, rightElements, parenthesisKoef * koef, temp, ref elemPosition, Solution.Matrix, caunter);

                        }
                        else
                        {
                            addElements(eq[i].ToString(), ref Solution.RowCount, Solution.ColumCount, rightElements, koef, parenthesisKoef, ref elemPosition, Solution.Matrix, caunter);
                        }

                    }
                    //ONE LETTER_________________________________________________________________________________________________
                    else
                    {
                        for (int o = i - 1; o > 0; o--)
                        {
                            if (eq[o].ToString() == eq[i].ToString())
                            {
                                Console.WriteLine("I was here!");
                                koef += Solution.Matrix[Int32.Parse(elemPosition.GetKey(elemPosition.IndexOfValue(eq[o])).ToString())][Solution.ColumCount];
                            }
                            else if (eq[o] == equal || eq[o] == plus)
                                goto Add;
                        }
                        Add:
                        addElements(eq[i].ToString(), ref Solution.RowCount, Solution.ColumCount, rightElements, koef, parenthesisKoef, ref elemPosition, Solution.Matrix, caunter);
                    }
                }

                //SIGNS_________________________________________________________________________________________________
                else if (eq[i].ToString() == plus.ToString() || eq[i].ToString() == star.ToString())
                {
                    ++i;
                    ++Solution.ColumCount;
                    if (caunter > 0)
                        caunter--;
                    goto Third;
                }
                else if (eq[i].ToString() == equal.ToString())
                {
                    ++rightElements;
                    ++i;
                    ++Solution.ColumCount;
                    if (caunter > 0)
                        caunter--;
                    goto Third;
                }
                else if (eq[i] == parenthesisL)
                {
                    caunter++;
                    int p = i;
                    for (; p < eq.Length; p++)
                    {
                        if (eq[p] == parenthesisR)
                            goto Here;
                    }

                    Here:
                    y = Double.TryParse(eq[p + 1].ToString(), out parenthesisKoef);
                    for (u = p + 2; u < eq.Length; u++)
                    {
                        y = Double.TryParse(eq[u].ToString(), out koeff[u]);
                        if (y == false)
                        {
                            goto First;
                        }
                    }
                    First:
                    for (int g = p + 2; g < u; g++)
                    {
                        parenthesisKoef = Double.Parse(parenthesisKoef.ToString() + koeff[g].ToString());

                    }
                    ++i;
                    goto Third;
                }
                else if (eq[i] == parenthesisR)
                {
                    ++i;
                    caunter--;
                    parenthesisKoef = 1;
                    if (i + 1 < eq.Length)
                    {
                        for (j = i + 1; j < eq.Length; j++)
                        {
                            y = Double.TryParse(eq[j].ToString(), out koeff[j]);
                            ++i;
                            if (y == false)
                                goto Third;
                        }
                    }
                }

            }
            printMatrix(Solution.Matrix, rows(eq), colomns(eq));


            for (int h = 0; h < Solution.RowCount; h++)
                Solution.RightPart[h] = Solution.Matrix[h][Solution.ColumCount];

            Solution.SolveMatrix();

            int loop = 0, lastKoeff = 1;
            for (int h = 0; h < Solution.ColumCount; h++)
            {

                if (Solution.Answer[h] < 1)
                {
                    if (Solution.Answer[h] % 10 == 6 || Solution.Answer[h] % 10 == 7)
                    {
                        multiply(Solution.Answer, 3);
                        lastKoeff *= 3;
                    }
                    else
                    {
                        multiply(Solution.Answer, 2);
                        lastKoeff *= 2;
                    }
                }
            }

            for (int b = 0; b < Solution.ColumCount; b++)
            {
                if (!(Solution.Answer[b] == (int)Solution.Answer[b]))
                    Console.Write(Math.Abs(Solution.Answer[b]).ToString("F01"));
                else
                    Console.Write(Math.Abs(Solution.Answer[b]));

                for (; loop < eq.Length; loop++)
                {
                    if (eq[loop] == equal || eq[loop] == plus)
                    {
                        Console.Write(" {0} ", eq[loop]);
                        loop++;
                        break;
                    }

                    Console.Write(eq[loop]);

                }
            }


            Console.Write(lastKoeff);
            for (; loop < eq.Length; loop++)
                Console.Write(eq[loop]);

            Console.WriteLine();
        }

        public static double[][] buildMatrix(int row, int colonm)
        {
            double[][] matrix = new double[row][];
            for (int i = 0; i < row; i++)
                matrix[i] = new double[colonm];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < colonm; j++)
                    matrix[i][j] = 0;
            }
            return matrix;
        }

        public static int rows(string eq)
        {
            ArrayList arrayList = new ArrayList();
            int rows = 0;
            for (int indexer = 0; indexer < eq.Length; indexer++)
            {
                if (System.Char.IsUpper(eq[indexer]) == true)
                {
                    if (indexer + 1 < eq.Length && System.Char.IsLower(eq[indexer + 1]) == true)
                    {
                        if (!(arrayList.Contains((eq[indexer]).ToString() + (eq[indexer + 1]).ToString())))
                        {
                            arrayList.Add((eq[indexer]).ToString() + (eq[indexer + 1]).ToString());
                            indexer++;
                            rows++;
                        }
                    }
                    else if (!(arrayList.Contains(eq[indexer])))
                    {
                        arrayList.Add(eq[indexer]);
                        rows++;
                    }
                }
            }
            return rows;
        }
        public static int colomns(string eq)
        {
            char equal = '=', plus = '+';
            int colonms = 2;
            for (int indexer = 0; indexer < eq.Length; indexer++)
            {
                if (((eq[indexer]).ToString()) == equal.ToString() || ((eq[indexer]).ToString()) == plus.ToString())
                {
                    colonms++;
                }
            }
            return colonms;

        }

        public static void addElements(string element, ref int rows, int colomns, int rightElements, double koef, double index, ref SortedList sortedList, double[][] matrix, int caunter)
        {
            if (sortedList.ContainsValue(element) == false)
            {
                sortedList.Add(rows, element);
                if (caunter > 0)
                {
                    if (rightElements > 0)
                        matrix[rows][colomns] = -koef * index;
                    else
                        matrix[rows][colomns] = koef * index;
                }
                else if (rightElements > 0)
                    matrix[rows][colomns] = -index;
                else
                    matrix[rows][colomns] = index;
                rows++;
            }
            else
            {
                int index1 = sortedList.IndexOfValue(element);
                var item = sortedList.GetKey(index1);
                int row = Int32.Parse(item.ToString());
                if (caunter > 0)
                {
                    if (rightElements > 0)
                        matrix[row][colomns] = -koef * index;
                    else
                        matrix[row][colomns] = koef * index;

                }
                else if (rightElements > 0)
                {
                    matrix[row][colomns] = -index;
                }
                else
                    matrix[row][colomns] = index;

            }
        }


        public static void printMatrix(double[][] matrix, int row, int colomn)
        {
            for (int i = 0; i < row; i++)
            {
                for (int jeppa = 0; jeppa < colomn; jeppa++)
                    Console.Write("{0} \t", matrix[i][jeppa]);
                Console.WriteLine();
            }
        }

        public static void multiply(double[] mas, int number)
        {
            for (int u = 0; u < mas.Length; u++)
                mas[u] *= number;

        }

    }
    class GausMethod
    {
        public int RowCount;
        public int ColumCount;
        public double[][] Matrix { get; set; }
        public double[] RightPart { get; set; }
        public double[] Answer { get; set; }

        public GausMethod(int Row, int Colum)
        {
            RightPart = new double[Row];
            Answer = new double[Row];
            Matrix = new double[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new double[Colum];
            RowCount = Row;
            ColumCount = Colum;

            //  обнулим массив
            for (int i = 0; i < Row; i++)
            {
                Answer[i] = 0;
                RightPart[i] = 0;
                for (int j = 0; j < Colum; j++)
                    Matrix[i][j] = 0;
            }
        }

        private void SortRows(int SortIndex)
        {

            double MaxElement = Matrix[SortIndex][SortIndex];
            int MaxElementIndex = SortIndex;
            for (int i = SortIndex + 1; i < RowCount; i++)
            {
                if (Matrix[i][SortIndex] > MaxElement)
                {
                    MaxElement = Matrix[i][SortIndex];
                    MaxElementIndex = i;
                }
            }

            // теперь найден максимальный элемент ставим его на верхнее место
            if (MaxElementIndex > SortIndex)//если это не первый элемент
            {
                double Temp;

                Temp = RightPart[MaxElementIndex];
                RightPart[MaxElementIndex] = RightPart[SortIndex];
                RightPart[SortIndex] = Temp;

                for (int i = 0; i < ColumCount; i++)
                {
                    Temp = Matrix[MaxElementIndex][i];
                    Matrix[MaxElementIndex][i] = Matrix[SortIndex][i];
                    Matrix[SortIndex][i] = Temp;
                }
            }
        }

        public int SolveMatrix()
        {
            if (RowCount != ColumCount)
                return 1; //нет решения

            for (int i = 0; i < RowCount - 1; i++)
            {
                SortRows(i);
                for (int j = i + 1; j < RowCount; j++)
                {
                    if (Matrix[i][i] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        double MultElement = Matrix[j][i] / Matrix[i][i];
                        for (int k = i; k < ColumCount; k++)
                            Matrix[j][k] -= Matrix[i][k] * MultElement;
                        RightPart[j] -= RightPart[i] * MultElement;
                    }
                    //для нулевого главного элемента просто пропускаем данный шаг
                }
            }

            //ищем решение
            for (int i = (int)(RowCount - 1); i >= 0; i--)
            {
                Answer[i] = RightPart[i];

                for (int j = (int)(RowCount - 1); j > i; j--)
                    Answer[i] -= Matrix[i][j] * Answer[j];

                if (Matrix[i][i] == 0)
                    if (RightPart[i] == 0)
                        return 2; //множество решений
                    else
                        return 1; //нет решения

                Answer[i] /= Matrix[i][i];

            }
            return 1;
        }



        public override String ToString()
        {
            String S = "";
            for (int i = 0; i < RowCount; i++)
            {
                S += "\r\n";
                for (int j = 0; j < ColumCount; j++)
                {
                    S += Matrix[i][j].ToString("F04") + "\t";
                }

                S += "\t" + Answer[i].ToString("F08");
                S += "\t" + RightPart[i].ToString("F04");
            }
            return S;
        }
    }
}