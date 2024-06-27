using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события завершения рисования на карте.
    /// </summary>
    internal sealed class DrawingCompletedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DrawingCompletedEventArgs"/>.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <param name="isForced">Значение, указывающее на то, что завершается ли рисование принудительно.</param>
        public DrawingCompletedEventArgs(Point mousePosition, bool isForced)
        {
            this.MousePosition = mousePosition;
            this.IsForced = isForced;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что отменено ли завершение рисования.
        /// </summary>
        public bool IsCanceled
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершается ли рисование принудительно.
        /// </summary>
        public bool IsForced
        {
            get;
        }

        /// <summary>
        /// Возвращает положение мыши.
        /// </summary>
        public Point MousePosition
        {
            get;
        }

        #endregion
    }
}