using Kts.Gis.Models;
using System.Collections.Generic;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления набора значений параметров.
    /// </summary>
    internal sealed class ParameterValueSetViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Отношения моделей и моделей представлений параметров.
        /// </summary>
        private Dictionary<ParameterModel, ParameterViewModel> paramRelations = new Dictionary<ParameterModel, ParameterViewModel>();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterValueSetViewModel"/>.
        /// </summary>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        public ParameterValueSetViewModel(ParameterValueSetModel parameterValueSet)
        {
            this.ParameterValueSet = parameterValueSet;

#warning Импортер временно сломан из-за изменения в структуре параметров
            //foreach (var paramValue in this.ParameterValueSet.ParameterValues)
            //{
            //    ParameterViewModel paramViewModel;

            //    if (paramValue.Key.Parent != null)
            //    {
            //        var parentParam = this.paramRelations[paramValue.Key.Parent];

            //        paramViewModel = new ParameterViewModel(paramValue.Key, this, paramValue.Value, parentParam.DepthLevel + 1);

            //        this.paramRelations[paramValue.Key.Parent].Children.Add(paramViewModel);
            //    }
            //    else
            //    {
            //        paramViewModel = new ParameterViewModel(paramValue.Key, this, paramValue.Value, 0);

            //        this.Parameters.Add(paramViewModel);
            //    }

            //    this.paramRelations.Add(paramValue.Key, paramViewModel);
            //}
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает набор значений параметров.
        /// </summary>
        public ParameterValueSetModel ParameterValueSet
        {
            get;
        }

        /// <summary>
        /// Возвращает параметры.
        /// </summary>
        public List<ParameterViewModel> Parameters
        {
            get;
        } = new List<ParameterViewModel>();

        #endregion
    }
}