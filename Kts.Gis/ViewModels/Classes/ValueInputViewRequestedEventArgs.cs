using Kts.Messaging;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргументы события запроса представления ввода значения.
    /// </summary>
    internal sealed class ValueInputViewRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValueInputViewRequestedEventArgs"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ValueInputViewRequestedEventArgs(ValueInputViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что есть ли результат ввода значения.
        /// </summary>
        public bool HasResult
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает модель представления.
        /// </summary>
        public ValueInputViewModel ViewModel
        {
            get;
        }

        #endregion
    }
}