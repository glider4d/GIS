using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет аргументы события запроса представления.
    /// </summary>
    /// <typeparam name="T">Тип модели представления.</typeparam>
    public sealed class ViewRequestedEventArgs<T> : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ViewRequestedEventArgs{T}"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ViewRequestedEventArgs(T viewModel)
        {
            this.ViewModel = viewModel;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает результат представления.
        /// </summary>
        public bool Result
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает модель представления.
        /// </summary>
        public T ViewModel
        {
            get;
        }

        #endregion
    }
}