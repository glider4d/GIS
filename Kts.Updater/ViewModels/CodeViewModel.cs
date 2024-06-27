using Kts.Updater.Models;
using Kts.Updater.Services;
using Kts.WpfUtilities;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления страницы с кодом.
    /// </summary>
    internal sealed class CodeViewModel : BasePageViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CodeViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public CodeViewModel(SqlDataService dataService) : base(dataService)
        {
            this.AddCodeCommand = new RelayCommand(this.ExecuteAddCommand);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления кода.
        /// </summary>
        public RelayCommand AddCodeCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет добавление кода.
        /// </summary>
        private void ExecuteAddCommand()
        {
            this.SqlObjects.Add(new CodeObjectViewModel(new SqlObjectModel("Без названия")));
        }

        #endregion
    }
}