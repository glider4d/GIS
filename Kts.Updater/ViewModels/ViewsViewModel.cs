using Kts.Updater.Services;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы представлений.
    /// </summary>
    internal sealed class ViewsViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ViewsViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public ViewsViewModel(SqlDataService dataService) : base(dataService)
        {
            // Загружаем данные представлений.
            this.SqlObjects.AddRange(SqlObjectViewModel.GetViews(dataService));
        }

        #endregion
    }
}