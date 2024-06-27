using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события вставки надписи на карту.
    /// </summary>
    internal sealed class LabelPastedEventArgs : EventArgs
    {
        #region Конструкторы
        
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelPastedEventArgs"/>.
        /// </summary>
        /// <param name="position">Положение вставки надписи.</param>
        public LabelPastedEventArgs(Point position)
        {
            this.Position = position;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает положение вставки надписи.
        /// </summary>
        public Point Position
        {
            get;
        }

        #endregion
    }
}