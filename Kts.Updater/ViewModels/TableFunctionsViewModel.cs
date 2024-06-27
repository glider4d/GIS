using Kts.Updater.Services;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы табличных функций.
    /// </summary>
    internal sealed class TableFunctionsViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TableFunctionsViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public TableFunctionsViewModel(SqlDataService dataService) : base(dataService)
        {
            // Загружаем данные табличных функций.
            this.SqlObjects.AddRange(SqlObjectViewModel.GetTableFunctions(dataService));
        }

        #endregion
    }
}