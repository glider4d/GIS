using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргументы события надобности котельной.
    /// </summary>
    internal sealed class NeedBoilerEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NeedBoilerEventArgs"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public NeedBoilerEventArgs(SelectBoilerViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        #endregion

        #region Открытые события

        /// <summary>
        /// Возвращает модель представления выбора котельной.
        /// </summary>
        public SelectBoilerViewModel ViewModel
        {
            get;
        }

        #endregion
    }
}