using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления истории изменений значения параметра.
    /// </summary>
    internal sealed class ParameterHistoryViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Дата с.
        /// </summary>
        private DateTime fromDate;

        /// <summary>
        /// Список параметров.
        /// </summary>
        private List<ParameterModel> parameters;

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        private IObjectViewModel selectedObject;

        /// <summary>
        /// Выбранный параметр.
        /// </summary>
        private ParameterModel selectedParameter;
        
        /// <summary>
        /// Дата по.
        /// </summary>
        private DateTime toDate;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterHistoryViewModel"/>.
        /// </summary>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="dataService">Сервис данных.</param>
        public ParameterHistoryViewModel(ILayerHolder layerHolder, IDataService dataService)
        {
            this.layerHolder = layerHolder;
            this.dataService = dataService;

            var currentDate = DateTime.Now;

            this.fromDate = new DateTime(currentDate.Year, 1, 1);
            this.toDate = currentDate;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает дату с.
        /// </summary>
        public DateTime FromDate
        {
            get
            {
                return this.fromDate;
            }
            set
            {
                this.fromDate = value;

                this.TryToUpdateData();
            }
        }

        /// <summary>
        /// Возвращает историю изменений.
        /// </summary>
        public AdvancedObservableCollection<ParameterHistoryEntryViewModel> History
        {
            get;
        } = new AdvancedObservableCollection<ParameterHistoryEntryViewModel>();

        /// <summary>
        /// Возвращает или задает список параметров.
        /// </summary>
        public List<ParameterModel> Parameters
        {
            get
            {
                return this.parameters;
            }
            private set
            {
                this.parameters = value;

                this.NotifyPropertyChanged(nameof(this.Parameters));
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        public IObjectViewModel SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;

                this.InitParameters();

                if (this.Parameters.Count > 0)
                    this.SelectedParameter = this.Parameters[0];
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный параметр.
        /// </summary>
        public ParameterModel SelectedParameter
        {
            get
            {
                return this.selectedParameter;
            }
            set
            {
                this.selectedParameter = value;

                this.NotifyPropertyChanged(nameof(this.SelectedParameter));

                this.TryToUpdateData();
            }
        }

        /// <summary>
        /// Возвращает или задает дату по.
        /// </summary>
        public DateTime ToDate
        {
            get
            {
                return this.toDate;
            }
            set
            {
                this.toDate = value;

                this.TryToUpdateData();
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Инициализирует параметры.
        /// </summary>
        private void InitParameters()
        {
            var parameters = new List<ParameterModel>();

            if (this.SelectedObject != null)
                foreach (var param in this.SelectedObject.Type.Parameters)
                    if (!param.IsCommon && !this.SelectedObject.Type.IsParameterReadonly(param) && param.IsVisible && !param.Format.IsCalculate && !param.Format.IsGroup)
                        parameters.Add(param);

            this.Parameters = parameters;
        }

        /// <summary>
        /// Пробует обновить историю изменений значения параметра.
        /// </summary>
        private void TryToUpdateData()
        {
            if (this.SelectedParameter == null)
                return;
            if ( !dataService.MapAccessService.testConnection("",true))
            //if (BaseSqlDataAccessService.localModeFlag)
                return;
            
            var temp = new List<ParameterHistoryEntryViewModel>();
            
#warning При выделении всей сети котельной возникала ошибка из-за того, что параметр, отвечающий за тип объекта, мог содержать неправильные данные. В будущем надо убрать такое обновление таблиц и повесить их напрямую в обращение к данным.
            if (this.SelectedParameter.LoadLevel == LoadLevel.Always)
                this.SelectedParameter.Table = this.dataService.ParameterAccessService.UpdateTable(this.SelectedParameter.Table, this.layerHolder.CurrentCity.Id, this.SelectedObject.Type, this.layerHolder.CurrentSchema);

            foreach (var entry in this.dataService.ParameterAccessService.GetParameterHistory(this.SelectedParameter.Id, this.FromDate, this.ToDate, this.SelectedObject.Id, this.layerHolder.CurrentSchema))
                temp.Add(new ParameterHistoryEntryViewModel(entry, this.SelectedParameter));

            this.History.Clear();

            this.History.AddRange(temp);
        }

        #endregion
    }
}