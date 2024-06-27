using Kts.Utilities;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления улицы.
    /// </summary>
    internal sealed class StreetViewModel : Utilities.StreetViewModel
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