using System;


namespace Kts.Utilities
{
    /// <summary>
    /// Представляет модель представления региона.
    /// </summary>
    [Serializable]
    public abstract class RegionViewModel : TerritorialEntityViewModel
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
    }
}