using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет точку.
    /// </summary>
    [Serializable]
    public sealed class Point
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Point"/>.
        /// </summary>
        /// <param name="x">Координата по X.</param>
        /// <param name="y">Координата по Y.</param>
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает координату по X.
        /// </summary>
        public double X
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает координату по Y.
        /// </summary>
        public double Y
        {
            get;
            set;
        }

        #endregion

        #region Открытые статические методы

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

        #endregion
    }
}