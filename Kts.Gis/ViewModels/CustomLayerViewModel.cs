using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления кастомного слоя.
    /// </summary>
    [Serializable]
    internal sealed class CustomLayerViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что изменен ли идентификатор кастомного слоя.
        /// </summary>
        private bool isIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что был ли изменен кастомный слой.
        /// </summary>
        private bool isModified;

        /// <summary>
        /// Значение, указывающее на то, что начато ли сохранение.
        /// </summary>
        private bool isSaveStarted;

        /// <summary>
        /// Значение, указывающее на то, что виден ли кастомный слой.
        /// </summary>
        private bool isVisible;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Кастомный слой.
        /// </summary>
        private readonly CustomLayerModel customLayer;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomLayerViewModel"/>.
        /// </summary>
        public CustomLayerViewModel() : this(new CustomLayerModel(Guid.Empty, null, -1, "-"), null)
        {
            this.IsEmpty = true;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomLayerViewModel"/>.
        /// </summary>
        /// <param name="customLayer">Модель кастомного слоя.</param>
        /// <param name="dataService">Сервис данных.</param>
        public CustomLayerViewModel(CustomLayerModel customLayer, IDataService dataService)
        {
            this.customLayer = customLayer;
            this.dataService = dataService;

            if (this.customLayer.Id == Guid.Empty)
                this.isModified = true;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор кастомного слоя.
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.customLayer.Id;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли кастомный слой пустышкой.
        /// </summary>
        public bool IsEmpty
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли кастомный слой.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.customLayer.Id != Guid.Empty;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли кастомный слой.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (value != this.IsVisible)
                {
                    this.isVisible = value;

                    this.NotifyPropertyChanged(nameof(this.IsVisible));
                }
            }
        }

        /// <summary>
        /// Возвращает или задает название кастомного слоя.
        /// </summary>
        public string Name
        {
            get
            {
                return this.customLayer.Name;
            }
            set
            {
                this.customLayer.Name = value;

                this.isModified = true;

                this.NotifyPropertyChanged(nameof(this.Name));
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Начинает сохранение кастомного слоя.
        /// </summary>
        public void BeginSave()
        {
            this.isSaveStarted = true;

            this.isIdChanged = false;

            if (this.isModified)
                if (this.IsSaved)
                    dataService.CustomLayerAccessService.UpdateLayer(this.customLayer);
                else
                {
                    this.customLayer.Id = dataService.CustomLayerAccessService.AddLayer(this.customLayer);

                    this.isIdChanged = true;
                }
        }

        /// <summary>
        /// Удаляет кастомный слой из источника данных.
        /// </summary>
        public void DeleteFromSource()
        {
            this.dataService.CustomLayerAccessService.DeleteLayer(this.customLayer.Schema, this.customLayer.CityId, this.Id);
        }

        /// <summary>
        /// Завершает сохранение кастомного слоя.
        /// </summary>
        public void EndSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            this.isModified = false;
        }

        /// <summary>
        /// Отменяет сохранение кастомного слоя.
        /// </summary>
        public void RevertSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            if (this.isIdChanged)
                this.customLayer.Id = Guid.Empty;
        }

        #endregion
    }
}