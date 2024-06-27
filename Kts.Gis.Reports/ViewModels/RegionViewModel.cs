using Kts.Gis.Data;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления региона.
    /// </summary>
    public sealed class RegionViewModel : Utilities.RegionViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RegionViewModel"/>.
        /// </summary>
        /// <param name="region">Регион.</param>
        public RegionViewModel(TerritorialEntityModel region) : base(region)
        {
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает список населенных пунктов региона.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список населенных пунктов.</returns>
        public List<CityViewModel> GetCities(IDataService dataService)
        {
            var cities = new List<CityViewModel>();
            
            foreach (var city in dataService.TerritorialEntityAccessService.GetCities(this.TerritorialEntity))
                cities.Add(new CityViewModel(city));

            return cities;
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает список регионов.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список регионов.</returns>
        public static List<RegionViewModel> GetRegions(IDataService dataService)
        {
            var regions = new List<RegionViewModel>();

            foreach (var region in dataService.TerritorialEntityAccessService.GetRegions())
                regions.Add(new RegionViewModel(region));

            return regions;
        }

        #endregion
    }
}