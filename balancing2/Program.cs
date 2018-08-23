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
            int[] koeff = new int[100];
            string tempstring;
            char plus = '+', equal = '=', star = '*', parenthesisL = '(', parenthesisR = ')';
            Fourth:
            int j, temp, u = 0, tempEq = 0, koef = 1, parenthesisKoef = 1;
            bool k;
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
                Third:
                bool y = Int32.TryParse(eq[i].ToString(), out koeff[i]);
                //KOEFF___________________________________________________________________________________________________________
                if (y && i + 1 < eq.Length && caunter < 0)
                {
                    for (j = i + 1; j < eq.Length; j++)
                    {
                        y = Int32.TryParse(eq[j].ToString(), out koeff[j]);
                        if (y == false)
                            goto First;
                    }

                    First:
                    koef = koeff[i];
                    for (int g = i + 1; g < j; g++)
                    {
                        koef = Int32.Parse(koef.ToString() + koeff[g].ToString());
                    }
                    caunter++;
                }

                //ELEMENTS________________________________________________________________________________________________

                else if (System.Char.IsUpper(eq[i]) == true)
                {
                    if (i + 1 < eq.Length)
                    {
                        bool l = Int32.TryParse(eq[i + 1].ToString(), out koeff[i]);

                        //TWO LETTERS__________________________________________________________________________________________________
                        if (System.Char.IsLower(eq[i + 1]) == true)
                        {
                            tempstring = eq[i].ToString() + eq[i + 1].ToString();
                            if (i + 2 < eq.Length && Int32.TryParse(eq[i + 2].ToString(), out koeff[i]) == true)
                            {
                                for (j = i + 3; j < eq.Length; j++)
                                {
                                    k = Int32.TryParse(eq[j].ToString(), out koeff[j - 2]);
                                    if (k == false)
                                        goto Second;
                                }

                                Second:
                                temp = koeff[i];
                                for (int g = i + 3; g < j; g++)
                                {
                                    temp = Int32.Parse(temp.ToString() + koeff[g - 2].ToString());
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
                                l = Int32.TryParse(eq[j].ToString(), out koeff[j - 1]);
                                if (l == false)
                                    goto Second;
                            }

                            Second:
                            temp = koeff[i];
                            for (int g = i + 2; g < j; g++)
                            {
                                temp = Int32.Parse(temp.ToString() + koeff[g - 1].ToString());
                            }

                            addElements(eq[i].ToString(), ref Solution.RowCount, Solution.ColumCount, rightElements, parenthesisKoef * koef, temp, ref elemPosition, Solution.Matrix, caunter);

                        }
                        else
                            addElements(eq[i].ToString(), ref Solution.RowCount, Solution.ColumCount, rightElements, koef, parenthesisKoef, ref elemPosition, Solution.Matrix, caunter);
                    }
                    //ONE LETTER_________________________________________________________________________________________________
                    else
                        addElements(eq[i].ToString(), ref Solution.RowCount, Solution.ColumCount, rightElements, koef, parenthesisKoef, ref elemPosition, Solution.Matrix, caunter);
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
                    y = Int32.TryParse(eq[p + 1].ToString(), out parenthesisKoef);
                    for (u = p + 2; u < eq.Length; u++)
                    {
                        y = Int32.TryParse(eq[u].ToString(), out koeff[u]);
                        if (y == false)
                        {
                            goto First;
                        }
                    }
                    First:
                    for (int g = p + 2; g < u; g++)
                    {
                        parenthesisKoef = Int32.Parse(parenthesisKoef.ToString() + koeff[g].ToString());

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
                            y = Int32.TryParse(eq[j].ToString(), out koeff[j]);
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
            
            //Multiply:
            //for (int h = 0; h < Solution.ColumCount; h++)
            //{
            //    if (Math.Round(Solution.Answer[h], 0) != Solution.Answer[h])
            //    {

            //        if (Solution.Answer[h] % 10 == 6 || Solution.Answer[h] % 10 == 7)
            //        {
            //            multiply(Solution.Answer, 3);
            //            goto Multiply;
            //        }
            //        //else
            //        //{
            //        //    multiply(Solution.Answer, 2);
            //        //    goto Multiply;
            //        //}
            //    }
            //}

            for (int b = 0; b < Solution.ColumCount; b++)
            {
                Console.Write("{0} \t",Math.Round(Solution.Answer[b], 0));
                //if (Math.Round(Solution.Answer[b]) != Solution.Answer[b])
                //{
                //    string khan = Solution.Answer[b].ToString();
                //    khan = khan.Replace(".", "");
                //    Solution.Answer[b] = Int32.Parse(khan);
                //}
                //Console.Write("\t{0}", Solution.Answer[b]);
            }
            //for (; b < eq.Length; b++)
            //{
            //    Console.Write(eq[b]);
            //    if (eq[b] == equal || eq[b] == plus)
            //        break;
            //}
        }

        public static double[][] buildMatrix(uint row, uint colonm)
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

        public static uint rows(string eq)
        {
            ArrayList arrayList = new ArrayList();
            uint rows = 0;
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
        public static uint colomns(string eq)
        {
            char equal = '=', plus = '+';
            uint colonms = 1;
            for (int indexer = 0; indexer < eq.Length; indexer++)
            {
                if (((eq[indexer]).ToString()) == equal.ToString() || ((eq[indexer]).ToString()) == plus.ToString())
                {
                    colonms++;
                }
            }
            return colonms;

        }

    public static void addElements(string element, ref uint rows, uint colomns, int rightElements, int koef, int index, ref SortedList sortedList, double[][] matrix, int caunter)
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


        public static void printMatrix(double[][] matrix, uint row, uint colomn)
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

}
class GausMethod
{
    public uint RowCount;
    public uint ColumCount;
    public double[][] Matrix { get; set; }
    public double[] RightPart { get; set; }
    public double[] Answer { get; set; }

    public GausMethod(uint Row, uint Colum)
    {
        RightPart = new double[Row];
        Answer = new double[Row];
        Matrix = new double[Row][];
        for (int i = 0; i < Row; i++)
            Matrix[i] = new double[Colum];
        RowCount = Row;
        ColumCount = Colum;

        //обнулим массив
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

        //теперь найден максимальный элемент ставим его на верхнее место
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
        return 0;
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