using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события изменения масштаба.
    /// </summary>
    internal sealed class ScaleChangedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ScaleChangedEventArgs"/>.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        /// <param name="originPoint">Точка, относительно который было выполнено масштабирование.</param>
        public ScaleChangedEventArgs(double scale, Point originPoint)
        {
            this.Scale = scale;
            this.OriginPoint = originPoint;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает точку, относительно который было выполнено масштабирование.
        /// </summary>
        public Point OriginPoint
        {
            get;
        }

        /// <summary>
        /// Возвращает масштаб.
        /// </summary>
        public double Scale
        {
            get;
        }

        #endregion
    }
}