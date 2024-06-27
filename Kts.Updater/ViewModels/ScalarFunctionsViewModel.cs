using Kts.Updater.Services;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы скалярных функций.
    /// </summary>
    internal sealed class ScalarFunctionsViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ScalarFunctionsViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public ScalarFunctionsViewModel(SqlDataService dataService) : base(dataService)
        {
            // Загружаем данные скалярных функций.
            this.SqlObjects.AddRange(SqlObjectViewModel.GetScalarFunctions(dataService));
        }

        #endregion
    }
}