using Kts.Gis.Services;
using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления автоматической легенды.
    /// </summary>
    [Serializable]
    internal sealed class AutoLegendViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Длина-пример.
        /// </summary>
        private double exampleLength;

        /// <summary>
        /// Значение, указывающее на то, что изменен ли масштаб линий.
        /// </summary>
        private bool isScaleChanged;

        /// <summary>
        /// Текст масштаба.
        /// </summary>
        private string scaleText;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AutoLegendViewModel"/>.
        /// </summary>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public AutoLegendViewModel(MainViewModel mainViewModel, IMapBindingService mapBindingService)
        {
            this.mainViewModel = mainViewModel;
            this.mapBindingService = mapBindingService;
            
            this.mainViewModel.MapViewModel.ScaleChanged += this.MapViewModel_ScaleChanged;

            this.mapBindingService.ScaleChanged += this.MapBindingService_ScaleChanged;
        }
        
        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает длину-пример.
        /// </summary>
        public double ExampleLength
        {
            get
            {
                return this.exampleLength;
            }
            set
            {
                this.exampleLength = value;

                this.NotifyPropertyChanged(nameof(this.ExampleLength));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что изменен ли масштаб линий.
        /// </summary>
        public bool IsScaleChanged
        {
            get
            {
                return this.isScaleChanged;
            }
            private set
            {
                this.isScaleChanged = value;

                if (value)
                    this.ScaleText = "Масштаб изменен (" + this.mainViewModel.MapViewModel.Scale + " м. на п.)";

                this.NotifyPropertyChanged(nameof(this.IsScaleChanged));
            }
        }

        /// <summary>
        /// Возвращает слои.
        /// </summary>
        public IEnumerable<LayerViewModel> Layers
        {
            get
            {
                return this.mainViewModel.AllObjectsGroup.Layers;
            }
        }

        /// <summary>
        /// Возвращает слои.
        /// </summary>
        public IEnumerable<LayerViewModel> PlanningLayers
        {
            get
            {
                return this.mainViewModel.PlanningObjectsGroup.Layers;
            }
        }

        /// <summary>
        /// Возвращает или задает текст масштаба.
        /// </summary>
        public string ScaleText
        {
            get
            {
                return this.scaleText;
            }
            private set
            {
                this.scaleText = value;

                this.NotifyPropertyChanged(nameof(this.ScaleText));
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="IMapBindingService.ScaleChanged"/> сервиса привязки представлений карты с моделями представлений.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapBindingService_ScaleChanged(object sender, EventArgs e)
        {
            this.ExampleLength = Math.Round(200 / this.mapBindingService.Scale * this.mainViewModel.MapViewModel.Scale, 2);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MapViewModel.ScaleChanged"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_ScaleChanged(object sender, EventArgs e)
        {
            this.IsScaleChanged = (sender as MapViewModel).Scale != 1;
        }

        #endregion
    }
}