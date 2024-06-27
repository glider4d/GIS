using System;

namespace Kts.ParameterGrid
{
    /// <summary>
    /// Представляет аргумент события завершения редактирования значения параметра.
    /// </summary>
    public sealed class EditCompletedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EditCompletedEventArgs"/>.
        /// </summary>
        /// <param name="parameter">Параметр, значение которого было отредактировано.</param>
        public EditCompletedEventArgs(IParameter parameter)
        {
            this.Parameter = parameter;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает параметр, значение которого было отредактировано.
        /// </summary>
        public IParameter Parameter
        {
            get;
        }

        #endregion
    }
}