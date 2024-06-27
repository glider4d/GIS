using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса выбора параметров.
    /// </summary>
    internal sealed class ParameterSelectRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterSelectRequestedEventArgs"/>.
        /// </summary>
        /// <param name="parameters">Параметры.</param>
        public ParameterSelectRequestedEventArgs(List<ParameterModel> parameters)
        {
            this.AllParameters = parameters;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает все параметры.
        /// </summary>
        public List<ParameterModel> AllParameters
        {
            get;
        }

        /// <summary>
        /// Возвращает список выбранных параметров.
        /// </summary>
        public List<ParameterModel> SelectedParameters
        {
            get;
        } = new List<ParameterModel>();

        #endregion
    }
}