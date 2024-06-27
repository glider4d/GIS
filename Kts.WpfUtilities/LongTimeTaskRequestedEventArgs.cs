using System;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет аргумент события запроса выполнения долгодлительной задачи.
    /// </summary>
    public sealed class LongTimeTaskRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземепляр класса <see cref="LongTimeTaskRequestedEventArgs"/>.
        /// </summary>
        /// <param name="waitViewModel">Модель представления ожидания окончания выполнения какой-либо задачи.</param>
        public LongTimeTaskRequestedEventArgs(WaitViewModel waitViewModel)
        {
            this.WaitViewModel = waitViewModel;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает модель представления ожидания окончания выполнения какой-либо задачи.
        /// </summary>
        public WaitViewModel WaitViewModel
        {
            get;
        }

        #endregion
    }
}