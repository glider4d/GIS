using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события изменения угла.
    /// </summary>
    internal sealed class AngleChangedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AngleChangedEventArgs"/>.
        /// </summary>
        /// <param name="angle">Угол, на который был выполнен поворот.</param>
        /// <param name="originPoint">Точка, относительно который был выполнен поворот.</param>
        public AngleChangedEventArgs(double angle, Point originPoint)
        {
            this.Angle = angle;
            this.OriginPoint = originPoint;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает угол, на который был выполнен поворот.
        /// </summary>
        public double Angle
        {
            get;
        }

        /// <summary>
        /// Возвращает точку, относительно который был выполнен поворот.
        /// </summary>
        public Point OriginPoint
        {
            get;
        }

        #endregion
    }
}