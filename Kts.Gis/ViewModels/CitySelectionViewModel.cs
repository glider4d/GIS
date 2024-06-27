using Kts.Gis.Data;
using Kts.Gis.Services;
using Kts.Settings;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора населенного пункта.
    /// </summary>
    [Serializable]
    internal sealed partial class CitySelectionViewModel : TerritorialEntitySelectionViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что учтен ли идентификатор населенного пункта из настроек.
        /// </summary>
        private bool isCityIdConsidered;

        /// <summary>
        /// Загруженный населенный пункт.
        /// </summary>
        private CityViewModel loadedCity;

        /// <summary>
        /// Идентификатор населенного пункта, который необходимо загрузить.
        /// </summary>
        private int targetCity = int.MinValue;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private AccessService accessService;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CitySelectionViewModel"/>.
        /// </summary>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public CitySelectionViewModel(AccessService accessService, IDataService dataService, ISettingService settingService) : base(SelectionDepth.City)
        {
            this.accessService = accessService;
            this.dataService = dataService;
            this.settingService = settingService;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает загруженный населенный пункт.
        /// </summary>
        public CityViewModel LoadedCity
        {
            get
            {
                return this.loadedCity;
            }
            set
            {
                this.loadedCity = value;

                this.NotifyPropertyChanged(nameof(this.LoadedCity));
            }
        }

        #endregion

        #region Открытые методы
        
        /// <summary>
        /// Загружает заданный населенный пункт.
        /// </summary>
        /// <param name="territorialEntity">Территориальная единица, представляющая населенный пункт.</param>
        public void LoadCity(TerritorialEntityModel territorialEntity)
        {
            var cityData = this.dataService.TerritorialEntityAccessService.GetCityData(territorialEntity);
            
            this.targetCity = territorialEntity.Id;

            this.SelectedRegion = this.Regions.First(x => x.Id == cityData.Item1.Id);
        }

        #endregion
    }

    // Реализация TerritorialEntitySelectionViewModel.
    internal sealed partial class CitySelectionViewModel
    {
        #region Защищенные переопределенные свойства

        /// <summary>
        /// Выполняет действия, связанные с загрузкой населенных пунктов.
        /// </summary>
        protected override void OnCitiesLoaded()
        {
            if (!this.isCityIdConsidered)
            {
                var lastUsedCityId = Convert.ToInt32(this.settingService.Settings["LastUsedCityId"]);
                if (lastUsedCityId == -1)
                {
                    this.SelectedCity = this.Cities[0];
                }
                else
                    if (this.Cities.Any(x => x.Id == lastUsedCityId))
                {
                    this.SelectedCity = this.Cities.First(x => x.Id == lastUsedCityId);
                }
                else
                {
                    this.SelectedCity = this.Cities[0];
                }
                this.isCityIdConsidered = true;
            }
            else
            {
                if (this.targetCity != int.MinValue)
                {
                    this.SelectedCity = this.Cities.First(x => x.Id == this.targetCity);

                    this.targetCity = int.MinValue;
                }
                else
                {
                    this.SelectedCity = this.Cities[0];
                }
            }
            this.settingService.Settings["LastUsedRegionId"] = this.SelectedRegion.Id;
        }

        /// <summary>
        /// Выполняет действия, связанные с загрузкой регионов.
        /// </summary>
        protected override void OnRegionsLoaded()
        {
            if (!this.isCityIdConsidered)
            {
                var lastUsedRegionId = Convert.ToInt32(this.settingService.Settings["LastUsedRegionId"]);
                if (lastUsedRegionId == -1)
                {
                    this.SelectedRegion = this.Regions[0];
                }
                else
                {
                    if (this.Regions.Any(x => x.Id == lastUsedRegionId))
                    {
                        this.SelectedRegion = this.Regions.First(x => x.Id == lastUsedRegionId);
                    }
                    else
                    {
                        this.SelectedRegion = this.Regions[0];
                    }
                }
            }
            else
            {
                this.SelectedRegion = this.Regions[0];
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Возвращает коллекцию населенных пунктов.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Коллекция населенных пунктов.</returns>
        protected override IEnumerable<Utilities.CityViewModel> GetCities(Utilities.RegionViewModel region)
        {
            return (region as RegionViewModel).GetCities(this.accessService.CanViewCityId, this.dataService);
        }

        /// <summary>
        /// Возвращает коллекцию регионов.
        /// </summary>
        /// <returns>Коллекция регионов.</returns>
        protected override IEnumerable<Utilities.RegionViewModel> GetRegions()
        {
            var result = new List<RegionViewModel>();

            foreach (var region in RegionViewModel.GetRegions(this.dataService))
                if (this.accessService.IsRegionPermitted(region.Id))
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
        /// Выполняет действия, связанные с изменением выбора.
        /// </summary>
        protected override void OnSelectionChanged()
        {
            this.settingService.Settings["LastUsedCityId"] = this.SelectedCity != null ? this.SelectedCity.Id : -1;
        }

        #endregion
    }
}