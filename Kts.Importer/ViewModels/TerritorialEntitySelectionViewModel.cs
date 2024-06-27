using Kts.Importer.Data;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора территориальной единицы.
    /// </summary>
    internal sealed partial class TerritorialEntitySelectionViewModel : Utilities.TerritorialEntitySelectionViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TerritorialEntitySelectionViewModel"/>.
        /// </summary>
        /// <param name="depth">Глубина выбора территориальной единицы.</param>
        /// <param name="dataService">Сервис данных.</param>
        public TerritorialEntitySelectionViewModel(SelectionDepth depth, IDataService dataService) : base(depth)
        {
            this.dataService = dataService;
        }

        #endregion
    }

    // Реализация TerritorialEntitySelectionViewModel.
    internal sealed partial class TerritorialEntitySelectionViewModel
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Возвращает коллекцию населенных пунктов.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Коллекция населенных пунктов.</returns>
        protected override IEnumerable<Utilities.CityViewModel> GetCities(Utilities.RegionViewModel region)
        {
            return (region as RegionViewModel).GetCities(this.dataService);
        }

        /// <summary>
        /// Возвращает коллекцию регионов.
        /// </summary>
        /// <returns>Коллекция регионов.</returns>
        protected override IEnumerable<Utilities.RegionViewModel> GetRegions()
        {
            return RegionViewModel.GetRegions(this.dataService);
        }

        /// <summary>
        /// Возвращает коллекцию улиц.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Коллекция улиц.</returns>
        protected override IEnumerable<Utilities.StreetViewModel> GetStreets(Utilities.CityViewModel city)
        {
            return (city as CityViewModel).GetStreets(this.dataService);
        }

        #endregion
    }
}