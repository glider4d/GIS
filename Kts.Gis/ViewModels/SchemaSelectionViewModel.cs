using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Settings;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора схемы.
    /// </summary>
    internal sealed class SchemaSelectionViewModel : BaseViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Значение, обозначающее выбор всех котельных.
        /// </summary>
        private const string allBoilersText = "Все";

        /// <summary>
        /// Значение, обозначающее выбор ни одной котельной.
        /// </summary>
        private const string noBoilersText = "Котельные не выбраны";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбраны ли все котельные.
        /// </summary>
        private bool allBoilers;

        /// <summary>
        /// Значение, указывающее на то, что имеется ли выбранная котельная.
        /// </summary>
        private bool hasSelectedBoiler;

        /// <summary>
        /// Выбранная схема.
        /// </summary>
        private SchemaModel selectedSchema;

        /// <summary>
        /// Статус выбора котельных.
        /// </summary>
        private string selectionStatus = allBoilersText;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Населенный пункт.
        /// </summary>
        private readonly CityViewModel city;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие "ОК".
        /// </summary>
        public event EventHandler OK;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SchemaSelectionViewModel"/>.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public SchemaSelectionViewModel(CityViewModel city, IDataService dataService, ISettingService settingService)
        {
            this.city = city;
            this.dataService = dataService;
            this.settingService = settingService;
            
            this.OKCommand = new RelayCommand(this.ExecuteOK, this.CanExecuteOK);

            this.Schemas.AddRange(dataService.Schemas);
            
            var lastSchemas = this.settingService.Settings["LastSchemas"] as Dictionary<int, int>;
            int? lastSchemaId = null;
            if (lastSchemas.Any(x => x.Key == city.Id))
                lastSchemaId = lastSchemas.First(x => x.Key == city.Id).Value;
            if (lastSchemaId.HasValue)
                this.SelectedSchema = this.Schemas.Any(x => x.Id == lastSchemaId) ? this.Schemas.First(x => x.Id == lastSchemaId) : this.Schemas.First();
            else
                this.SelectedSchema = this.Schemas.First();
        }

        #endregion

        #region Закрытые свойства
        
        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли выбранная котельная.
        /// </summary>
        private bool HasSelectedBoiler
        {
            get
            {
                return this.hasSelectedBoiler;
            }
            set
            {
                this.hasSelectedBoiler = value;

                this.OKCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Открытые свойства

        bool m_localLoading = false;
        public bool localLoading
        {
            get
            {
                return m_localLoading;
            }
            set
            {
                m_localLoading = value;
                this.NotifyPropertyChanged(nameof(this.localLoading));
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что выбраны ли все котельные.
        /// </summary>
        public bool AllBoilers
        {
            get
            {
                return this.allBoilers;
            }
            set
            {
                this.allBoilers = value;

                // Обходим все котельные и выбираем их.
                foreach (var boiler in this.Boilers)
                    boiler.IsSelected = value;

                this.NotifyPropertyChanged(nameof(this.AllBoilers));
            }
        }

        /// <summary>
        /// Возвращает коллекцию котельных выбранного населенного пункта.
        /// </summary>
        public AdvancedObservableCollection<SelectableBoilerViewModel> Boilers
        {
            get;
        } = new AdvancedObservableCollection<SelectableBoilerViewModel>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что все ли котельные выбраны.
        /// </summary>
        public bool IsAllBoilersSelected
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду "ОК".
        /// </summary>
        public RelayCommand OKCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает схемы.
        /// </summary>
        public AdvancedObservableCollection<SchemaModel> Schemas
        {
            get;
        } = new AdvancedObservableCollection<SchemaModel>();

        /// <summary>
        /// Возвращает список выбранных котельных.
        /// </summary>
        public List<SelectableBoilerViewModel> SelectedBoilers
        {
            get;
        } = new List<SelectableBoilerViewModel>();
        
        /// <summary>
        /// Возвращает или задает выбранную схему.
        /// </summary>
        public SchemaModel SelectedSchema
        {
            get
            {
                return this.selectedSchema;
            }
            set
            {
                this.selectedSchema = value;

                if (value != null)
                {
                    // Запоминаем выбор схемы.
                    var lastSchemas = this.settingService.Settings["LastSchemas"] as Dictionary<int, int>;
                    if (lastSchemas.Any(x => x.Key == this.city.Id))
                        lastSchemas[this.city.Id] = value.Id;
                    else
                        lastSchemas.Add(this.city.Id, value.Id);

                    // Если список котельных уже был загружен, то отписываемся от их событий.
                    foreach (var boiler in this.Boilers)
                        boiler.PropertyChanged -= this.Boiler_PropertyChanged;

                    // Загружаем список котельных и выбираем первую из них.
                    this.Boilers.Clear();
                    var boilers = new List<SelectableBoilerViewModel>();
                    foreach (var boiler in this.city.GetBoilers(value, this.dataService))
                        boilers.Add(new SelectableBoilerViewModel(boiler.Item1, boiler.Item2));
                    this.Boilers.AddRange(boilers);

                    this.AllBoilers = true;

                    // Подписываемся на события загруженных котельных.
                    foreach (var boiler in this.Boilers)
                        boiler.PropertyChanged += this.Boiler_PropertyChanged;

                    this.UpdateStatus();
                }

                this.NotifyPropertyChanged(nameof(this.SelectedSchema));
            }
        }

        /// <summary>
        /// Возвращает или задает статус выбора котельных.
        /// </summary>
        public string SelectionStatus
        {
            get
            {
                return this.selectionStatus;
            }
            private set
            {
                this.selectionStatus = value;

                this.NotifyPropertyChanged(nameof(this.SelectionStatus));
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="BaseViewModel.PropertyChanged"/> модели представления котельной.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Boiler_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectableBoilerViewModel.IsSelected))
                this.UpdateStatus();
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить "ОК".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteOK()
        {
            return this.HasSelectedBoiler;
        }

        /// <summary>
        /// Выполняет "ОК".
        /// </summary>
        private void ExecuteOK()
        {
            BaseSqlDataAccessService.loadAllObjectFlag = localLoading;
            this.OK?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обновляет статус выбора котельных.
        /// </summary>
        private void UpdateStatus()
        {
            var count = 0;
            var status = "";

            this.SelectedBoilers.Clear();

            foreach (var boiler in this.Boilers)
                if (boiler.IsSelected)
                {
                    status += boiler.Name + ", ";

                    count++;

                    this.SelectedBoilers.Add(boiler);
                }
                
            if (count == this.Boilers.Count)
                this.SelectionStatus = allBoilersText;
            else
                if (count == 0)
                    this.SelectionStatus = noBoilersText;
                else
                {
                    // Удаляем последнюю запятую.
                    status = status.Remove(status.Length - 2, 2);

                    this.SelectionStatus = status;
                }

            if (count == this.Boilers.Count)
            {
                this.IsAllBoilersSelected = true;

                this.allBoilers = true;

                this.NotifyPropertyChanged(nameof(this.AllBoilers));
            }
            else
            {
                this.IsAllBoilersSelected = false;

                this.allBoilers = false;

                this.NotifyPropertyChanged(nameof(this.AllBoilers));
            }

            if (count == 0 && !this.IsAllBoilersSelected)
                this.HasSelectedBoiler = false;
            else
                this.HasSelectedBoiler = true;
        }

        #endregion
    }
}