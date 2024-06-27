using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет модель представления улицы.
    /// </summary>
    [Serializable]
    public abstract class StreetViewModel : TerritorialEntityViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StreetViewModel"/>.
        /// </summary>
        /// <param name="street">Улица.</param>
        public StreetViewModel(TerritorialEntityModel street) : base(street)
        {
        }

        #endregion
    }
}