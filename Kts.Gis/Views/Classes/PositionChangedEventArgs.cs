using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события изменения положения.
    /// </summary>
    internal sealed class PositionChangedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PositionChangedEventArgs"/>.
        /// </summary>
        /// <param name="delta">Дельта изменения положения.</param>
        public PositionChangedEventArgs(Point delta)
        {
            this.Delta = delta;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает дельту изменения положения.
        /// </summary>
        public Point Delta
        {
            get;
        }

        #endregion
    }
}