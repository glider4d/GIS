using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора региона.
    /// </summary>
    public partial class RegionSelectionViewModel : TerritorialEntitySelectionViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что можно ли выбрать котельную.
        /// </summary>
        private bool canSelectBoiler;
        
        /// <summary>
        /// Выбранная котельная.
        /// </summary>
        private BoilerViewModel selectedBoiler;

        /// <summary>
        /// Выбранная схема.
        /// </summary>
        private SchemaModel selectedSchema;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Идентификаторы разрешенных к выбору регионов.
        /// </summary>
        private readonly List<int> permittedRegionIds;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RegionSelectionViewModel"/>.
        /// </summary>
        /// <param name="permittedRegionIds">Идентификаторы разрешенных к выбору регионов.</param>
        /// <param name="dataService">Сервис данных.</param>
        public RegionSelectionViewModel(List<int> permittedRegionIds, IDataService dataService) : base(SelectionDepth.City)
        {
            this.permittedRegionIds = permittedRegionIds;
            this.dataService = dataService;

            this.NormalSchemas.AddRange(this.dataService.Schemas.Where(s => !s.IsIS));
            this.Schemas.AddRange(this.dataService.Schemas);

            this.SelectedSchema = this.Schemas.First(x => x.IsActual);

            this.SelectionChanged += this.RegionSelectionViewModel_SelectionChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает список, состоящий из искусственной котельной, представляющей все котельные населенного пункта.
        /// </summary>
        public List<BoilerViewModel> AllBoilers
        {
            get;
        } = new List<BoilerViewModel>()
        {
            new BoilerViewModel(Guid.Empty, "Все")
        };

        /// <summary>
        /// Возвращает список, состоящий из искусственнего населенного пункта, представляющий все населенные пункты региона.
        /// </summary>
        public List<CityViewModel> AllCities
        {
            get;
        } = new List<CityViewModel>()
        {
            new CityViewModel(new TerritorialEntityModel(-1, "Все"))
        };

        /// <summary>
        /// Возвращает котельные текущего населенного пункта.
        /// </summary>
        public AdvancedObservableCollection<BoilerViewModel> Boilers
        {
            get;
        } = new AdvancedObservableCollection<BoilerViewModel>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли выбрать котельную.
        /// </summary>
        public bool CanSelectBoiler
        {
            get
            {
                return this.canSelectBoiler;
            }
            private set
            {
                this.canSelectBoiler = value;

                this.NotifyPropertyChanged(nameof(this.CanSelectBoiler));
            }
        }

        /// <summary>
        /// Возвращает фактические схемы.
        /// </summary>
        public AdvancedObservableCollection<SchemaModel> NormalSchemas
        {
            get;
        } = new AdvancedObservableCollection<SchemaModel>();

        /// <summary>
        /// Возвращает схемы.
        /// </summary>
        public AdvancedObservableCollection<SchemaModel> Schemas
        {
            get;
        } = new AdvancedObservableCollection<SchemaModel>();

        /// <summary>
        /// Возвращает или задает выбранную котельную.
        /// </summary>
        public BoilerViewModel SelectedBoiler
        {
            get
            {
                return this.selectedBoiler;
            }
            set
            {
                this.selectedBoiler = value;

                this.NotifyPropertyChanged(nameof(this.SelectedBoiler));
            }
        }

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

                this.NotifyPropertyChanged(nameof(this.SelectedSchema));
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="TerritorialEntitySelectionViewModel.SelectionChanged"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RegionSelectionViewModel_SelectionChanged(object sender, EventArgs e)
        {
            this.LoadBoilers(this.SelectedSchema, this.SelectedCity);
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Загружает котельные выбранной схемы.
        /// </summary>
        /// <param name="schema"></param>
        private void LoadBoilers(SchemaModel schema, Utilities.CityViewModel city)
        {
            this.Boilers.Clear();

            if (city != this.AllCities[0])
            {
                // Загружаем котельные.
                var boilers = new List<BoilerViewModel>();
                foreach (var boiler in this.dataService.TerritorialEntityAccessService.GetBoilers(city.Id, schema))
                    boilers.Add(new BoilerViewModel(boiler.Item1, boiler.Item2));
                this.Boilers.AddRange(boilers);

                this.CanSelectBoiler = true;
            }
            else
                this.CanSelectBoiler = false;

            this.SelectedBoiler = this.AllBoilers[0];
        }

        #endregion
    }

    // Реализация TerritorialEntitySelectionViewModel.
    public partial class RegionSelectionViewModel
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Возвращает коллекцию населенных пунктов.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Коллекция населенных пунктов.</returns>
        protected override IEnumerable<Utilities.CityViewModel> GetCities(Utilities.RegionViewModel region)
        {
            var result = new List<CityViewModel>();

            foreach (var city in (region as RegionViewModel).GetCities(this.dataService))
                result.Add(city);
                
            return result;
        }

        /// <summary>
        /// Возвращает коллекцию регионов.
        /// </summary>
        /// <returns>Коллекция регионов.</returns>
        protected override IEnumerable<Utilities.RegionViewModel> GetRegions()
        {
            var result = new List<RegionViewModel>();

            foreach (var region in RegionViewModel.GetRegions(this.dataService))
                if (this.permittedRegionIds.Contains(region.Id))
                    result.Add(region);

            return result;
        }

        /// <summary>
        /// Возвращает коллекцию улиц.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Коллекция улиц.</returns>
        protected override IEnumerable<StreetViewModel> GetStreets(Utilities.CityViewModel city)
        {
            return null;
        }

        /// <summary>
        /// Выполняет действия, связанные с загрузкой населенных пунктов.
        /// </summary>
        protected override void OnCitiesLoaded()
        {
            this.SelectedCity = this.AllCities[0];
        }

        #endregion
    }
}