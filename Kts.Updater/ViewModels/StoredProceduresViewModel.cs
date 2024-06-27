using Kts.Updater.Services;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы хранимых процедур.
    /// </summary>
    internal sealed class StoredProceduresViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StoredProceduresViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public StoredProceduresViewModel(SqlDataService dataService) : base(dataService)
        {
            // Загружаем данные хранимых процедур.
            this.SqlObjects.AddRange(SqlObjectViewModel.GetStoredProcedures(dataService));
        }

        #endregion
    }
}