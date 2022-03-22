﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Задание 3 №17
 * Дан массив из n точек на прямой. Найти такую точку из данного массива, сумма расстояний от которой до остальных его точек минимальна, и саму эту сумму
 */
namespace ConsoleApp3
{
    internal class Program
    {
        //Вывод результата в консоли
        static void Main(string[] args)
        {
            //string answ = Task(InputPoints(InputCount()));
            Console.WriteLine("Ответ: {0}", Task(InputPoints(InputCount())));
            Console.ReadLine();
        }

        //Класс точка с методами и свойствами
        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string ToStr()
            {
                return string.Format("({0}, {1})", X, Y);
            }
        }

        //Функция InputPoints заносит в массив количество count точек и возвращает массив точек
        static Point[] InputPoints(int count)
        {
            Point[] points = new Point[count];

            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine("Ввод точки номер {0}:", i);
                Console.Write("Введите X: ");
                int x = CorrectIntInput();
                Console.Write("Введите Y: ");
                int y = CorrectIntInput();
                points[i - 1] = new Point
                {
                    X = x,
                    Y = y
                };
            }

            return points;
        }

        //Функция CorrectIntInput возвращает ввод корректного числа
        static int CorrectIntInput()
        {
            bool isCorr;
            int res;

            do
            {
                isCorr = int.TryParse(Console.ReadLine(), out res);
                if (!isCorr)
                {
                    Console.Write("Ошибка. Повторите ввод: ");
                }
            } while (!isCorr);

            return res;
        }

        //Функция InputCount возвращает количество введенных точек
        static int InputCount()
        {
            Console.Write("Введите количество точек: ");
            int count = CorrectIntInput();

            while (count < 2)
            {
                Console.Write("Ошибка. Введите количество точек (2 или больше): ");
                count = CorrectIntInput();
            }

            return count;
        }

        //Функция DistanceBetween возвращает расстояние между точками point1, point2
        static double DistanceBetween(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        //Функция K — угловой коэффициент возвращает некоторое число точек point1, point2
        static double K(Point point1, Point point2)
        {
            if (point1.X == point2.X || point1.Y == point2.Y)
            {
                return 0;
            }
            else
            {
                return (point1.Y - point2.Y) / (point1.X - point2.X);
            }           
        }

        //Функция B — свободный коэффициент возвращает некоторое число точки point и углового коэффициента k
        static double B(Point point, double k)
        {
            return point.Y - k * point.X;
        }

        //Функция OnLine возвращает true, если k — угловой коэффициент и b — свободный коэффициент у последующих точек point1, point2 равны
        static bool OnLine(Point point1, Point point2, double k, double b)
        {
            double kNext = K(point1, point2);
            double bNext = B(point2, kNext);
            
            return kNext == k && bNext == b;
        }

        /* 
         * Линейная функция. Если k и b у точек равны, то они лежат на одной прямой
         * y = k * x + b
         * k = (y1 - y2) / (x1 - x2)
         * b = y2 - k * x2
         */
        //Функция Task возврщает строку с ответом и находит массива точек points наименьшею длину между ними на прямой
        static string Task(Point[] points)
        {
            double sumMin = double.MaxValue;
            Point p1 = new Point();

            //проверка для всех точек, кроме последней
            for (int i = 0; i < points.Length - 1; i++)
            {
                double sumCur = 0;
                double k = K(points[i], points[i + 1]);
                double b = B(points[i + 1], k);

                for (int j = i + 1; j < points.Length; j++)
                {
                    if (OnLine(points[i], points[j], k, b))
                    {
                        sumCur += DistanceBetween(points[i], points[j]);
                    }
                }

                for (int j = 0; j < i; j++)
                {
                    if (OnLine(points[i], points[j], k, b))
                    {
                        sumCur += DistanceBetween(points[i], points[j]);
                    }
                }

                if (sumMin > sumCur && sumCur > 0)
                {
                    sumMin = sumCur;
                    p1 = points[i];
                }
            }

            //проверка для последней точки
            double sumCurLast = 0;
            double kLast = K(points[points.Length - 2], points[points.Length - 1]);
            double bLast = B(points[points.Length - 1], kLast);

            for (int i = 0; i < points.Length - 1; i++)
            {
                if (OnLine(points[i], points[points.Length - 1], kLast, bLast))
                {
                    sumCurLast += DistanceBetween(points[points.Length - 1], points[i]);
                }
            }

            if (sumMin > sumCurLast && sumCurLast > 0)
            {
                sumMin = sumCurLast;
                p1 = points[points.Length - 1];
            }

            if (sumMin == double.MaxValue)
            {
                return "Таких нет";
            }
            else
            {
                return $"Точка = {p1.ToStr()}, Сумма: {sumMin:f3}";
            }
        }
    }
}