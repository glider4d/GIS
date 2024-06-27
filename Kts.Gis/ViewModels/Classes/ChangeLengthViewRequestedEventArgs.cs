using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса изменения отображения длины.
    /// </summary>
    internal sealed class ChangeLengthViewRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLengthViewRequestedEventArgs"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления изменения отображения длины.</param>
        public ChangeLengthViewRequestedEventArgs(ChangeLengthViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает модель представления изменения отображения длины.
        /// </summary>
        public ChangeLengthViewModel ViewModel
        {
            get;
        }

        #endregion
    }
}