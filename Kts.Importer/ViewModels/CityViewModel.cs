using Kts.Importer.Data;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления населенного пункта.
    /// </summary>
    internal sealed class CityViewModel : Utilities.CityViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CityViewModel"/>.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        public CityViewModel(TerritorialEntityModel city) : base(city)
        {
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает список улиц.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список улиц.</returns>
        public List<StreetViewModel> GetStreets(IDataService dataService)
        {
            var streets = new List<StreetViewModel>();

            foreach (var street in dataService.GetStreets(this.TerritorialEntity))
                streets.Add(new StreetViewModel(street));

            return streets;
        }

        #endregion
    }
}