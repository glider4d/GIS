using System;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события щелчка при зажатой клавише шифта.
    /// </summary>
    internal sealed class ShiftClickedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ShiftClickedEventArgs"/>.
        /// </summary>
        /// <param name="shape">Фигура, на которую был произведен щелчок.</param>
        public ShiftClickedEventArgs(IInteractiveShape shape)
        {
            this.Shape = shape;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает фигуру, на которую был произведен щелчок.
        /// </summary>
        public IInteractiveShape Shape
        {
            get;
        }

        #endregion
    }
}