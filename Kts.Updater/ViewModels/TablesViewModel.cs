using Kts.Updater.Services;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы таблиц.
    /// </summary>
    internal sealed class TablesViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TablesViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public TablesViewModel(SqlDataService dataService) : base(dataService)
        {
            // Загружаем данные таблиц.
            this.SqlObjects.AddRange(SqlObjectViewModel.GetTables(dataService));
        }

        #endregion
    }
}