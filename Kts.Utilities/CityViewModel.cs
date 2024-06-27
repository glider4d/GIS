using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет модель представления населенного пункта.
    /// </summary>
    [Serializable]
    public abstract class CityViewModel : TerritorialEntityViewModel
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