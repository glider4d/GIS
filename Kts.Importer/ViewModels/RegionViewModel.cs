using Kts.Importer.Data;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления региона.
    /// </summary>
    internal sealed class RegionViewModel : Utilities.RegionViewModel
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
        /// Возвращает список населенных пунктов.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список населенных пунктов.</returns>
        public List<CityViewModel> GetCities(IDataService dataService)
        {
            var cities = new List<CityViewModel>();

            foreach (var city in dataService.GetCities(this.TerritorialEntity))
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

            foreach (var region in dataService.GetRegions())
                regions.Add(new RegionViewModel(region));

            return regions;
        }

        #endregion
    }
}