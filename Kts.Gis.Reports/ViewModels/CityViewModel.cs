using Kts.Utilities;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления населенного пункта.
    /// </summary>
    public sealed class CityViewModel : Utilities.CityViewModel
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
    }
}