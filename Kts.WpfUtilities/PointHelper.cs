using System;
using System.Windows;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет помощника по работе с точками.
    /// </summary>
    public static class PointHelper
    {
        #region Открытые статические методы

        /// <summary>
        /// Возвращает расстояние от точки C до отрезка AB.
        /// </summary>
        /// <param name="a">Точка начала отрезка AB.</param>
        /// <param name="b">Точка конца отрезка AB.</param>
        /// <param name="c">Точка, от которой находим расстояние.</param>
        /// <returns>Расстояние от точки C до отрезка AB.</returns>
        public static double GetCDistance(Point a, Point b, Point c)
        {
            return GetDistance(c, GetNearestPoint(a, b, c));
        }

        /// <summary>
        /// Возвращает расстояние между двумя точками.
        /// </summary>
        /// <param name="a">Первая точка.</param>
        /// <param name="b">Вторая точка.</param>
        /// <returns>Расстояние между двумя точками.</returns>
        public static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        /// <summary>
        /// Возвращает середину заданного отрезка.
        /// </summary>
        /// <param name="a">Первая точка отрезка.</param>
        /// <param name="b">Вторая точка отрезка.</param>
        /// <returns>Середина заданного отрезка.</returns>
        public static Point GetMidPoint(Point a, Point b)
        {
            return new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        /// <summary>
        /// Находит точку проекции, опущенной с точки C на отрезок AB.
        /// </summary>
        /// <param name="a">Точка начала отрезка AB.</param>
        /// <param name="b">Точка конца отрезка AB.</param>
        /// <param name="c">Точка, от которой опускается проекция.</param>
        /// <returns>Точка проекции.</returns>
        public static Point GetNearestPoint(Point a, Point b, Point c)
        {
            var v = b - a;
            var w = c - a;

            var z = v.X * w.X + v.Y * w.Y;

            if (z <= 0)
                return a;

            var x = v.X * v.X + v.Y * v.Y;

            if (x <= z)
                return b;

            var rel = z / x;

            return new Point(a.X + v.X * rel, a.Y + v.Y * rel);
        }

        #endregion
    }
}