using Kts.Updater.Services;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы справочников.
    /// </summary>
    internal sealed class CatalogsViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CatalogsViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public CatalogsViewModel(SqlDataService dataService) : base(dataService)
        {
            // Загружаем данные представлений.
            this.SqlObjects.AddRange(SqlObjectViewModel.GetCatalogs(dataService));
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Подготавливает данные к созданию.
        /// </summary>
        public void PrepareData()
        {
            // Получаем выбранные объекты и для каждого из них загружаем команды.
            var objects = this.GetSelectedObjects();

            for (int i = 0; i < objects.Count; i++)
                this.SqlDataService.GetCatalogData(objects[i]);
        }

        #endregion
    }
}