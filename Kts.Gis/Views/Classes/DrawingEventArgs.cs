using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события рисования на карте.
    /// </summary>
    internal sealed class DrawingEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DrawingEventArgs"/>.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        public DrawingEventArgs(Point mousePosition) : this(mousePosition, mousePosition)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DrawingEventArgs"/>.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        public DrawingEventArgs(Point mousePosition, Point mousePrevPosition)
        {
            this.MousePosition = mousePosition;
            this.MousePrevPosition = mousePrevPosition;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает положение мыши.
        /// </summary>
        public Point MousePosition
        {
            get;
        }

        /// <summary>
        /// Возвращает предыдущее положение мыши.
        /// </summary>
        public Point MousePrevPosition
        {
            get;
        }

        #endregion
    }
}