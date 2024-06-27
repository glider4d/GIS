using Kts.Gis.Models;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления ошибки.
    /// </summary>
    [Serializable]
    internal sealed class ErrorViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ErrorViewModel"/>.
        /// </summary>
        /// <param name="obj">Объект, который содержит ошибку.</param>
        /// <param name="param">Параметр, в котором найдена ошибка.</param>
        public ErrorViewModel(IObjectViewModel obj, ParameterModel param)
        {
            this.Object = obj;
            this.Parameter = param;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает объект, который содержит ошибку.
        /// </summary>
        public IObjectViewModel Object
        {
            get;
        }

        /// <summary>
        /// Возвращает параметр, в котором найдена ошибка.
        /// </summary>
        public ParameterModel Parameter
        {
            get;
        }

        #endregion
    }
}