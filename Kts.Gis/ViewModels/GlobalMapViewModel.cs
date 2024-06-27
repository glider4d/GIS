using Kts.Gis.Data;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления глобальной карты.
    /// </summary>
    internal sealed class GlobalMapViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Сигнализитор токена отмены загрузки данных выбранного региона.
        /// </summary>
        private CancellationTokenSource cts;

        /// <summary>
        /// Значение, указывающее на то, что видна ли информация выбранного визуального региона.
        /// </summary>
        private bool isInfoVisible;

        /// <summary>
        /// Значение, указывающее на то, что идет ли загрузка данных.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Тип (по планируемости) объектов.
        /// </summary>
        private bool selectedObjectType;

        /// <summary>
        /// Выбранный визуальный регион.
        /// </summary>
        private VisualRegionViewModel selectedVisualRegion;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GlobalMapViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public GlobalMapViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            this.PropertyChanged += this.GlobalMapViewModel_PropertyChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видна ли информация выбранного визуального региона.
        /// </summary>
        public bool IsInfoVisible
        {
            get
            {
                return this.isInfoVisible;
            }
            set
            {
                this.isInfoVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsInfoVisible));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что идет ли загрузка данных.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            private set
            {
                this.isLoading = value;

                this.NotifyPropertyChanged(nameof(this.IsLoading));
            }
        }

        /// <summary>
        /// Возвращает типы (по планируемости) объектов.
        /// </summary>
        public List<Tuple<bool, string>> ObjectTypes
        {
            get;
        } = new List<Tuple<bool, string>>()
        {
            new Tuple<bool, string>(false, "Фактические объекты"),
            new Tuple<bool, string>(true, "Планируемые объекты")
        };

        /// <summary>
        /// Возвращает или задает тип (по планируемости) объектов.
        /// </summary>
        public bool SelectedObjectType
        {
            get
            {
                return this.selectedObjectType;
            }
            set
            {
                this.selectedObjectType = value;

                if (this.SelectedVisualRegion != null && this.SelectedVisualRegion.Info != null)
                    this.SelectedVisualRegion.Info.IsPlanning = value;
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный визуальный регион.
        /// </summary>
        public VisualRegionViewModel SelectedVisualRegion
        {
            get
            {
                return this.selectedVisualRegion;
            }
            set
            {
                if (this.SelectedVisualRegion != value)
                {
                    if (this.SelectedVisualRegion != null)
                        this.SelectedVisualRegion.IsSelected = false;

                    this.selectedVisualRegion = value;

                    if (value != null)
                        value.IsSelected = true;

                    this.NotifyPropertyChanged(nameof(this.SelectedVisualRegion));

                    this.IsInfoVisible = value != null;
                }
            }
        }

        /// <summary>
        /// Возвращает визуальные регионы.
        /// </summary>
        public List<VisualRegionViewModel> VisualRegions
        {
            get;
        } = new List<VisualRegionViewModel>();

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void GlobalMapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedVisualRegion))
            {
                if (this.cts != null)
                    this.cts.Cancel();

                var region = this.SelectedVisualRegion;

                if (region != null)
                {
                    var cts = new CancellationTokenSource();

                    this.cts = cts;

                    this.IsLoading = true;

                    try
                    {
                        await region.LoadInfoAsync(cts.Token);

                        if (!cts.IsCancellationRequested)
                            this.IsLoading = false;
                    }
                    catch
                    {
                        if (!cts.IsCancellationRequested)
                            this.IsLoading = false;
                    }

                    if (region.Info != null)
                        region.Info.IsPlanning = this.SelectedObjectType;
                }
                else
                    this.IsLoading = false;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Загружает глобальную карту.
        /// </summary>
        public void Load()
        {
            foreach (var visualRegion in this.dataService.GlobalMapAccessService.GetVisualRegions())
                this.VisualRegions.Add(new VisualRegionViewModel(visualRegion, this.dataService));
        }

        #endregion
    }
}