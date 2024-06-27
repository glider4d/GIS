using Kts.Importer.Data;
using Kts.Importer.Views;
using Kts.Messaging;
using Kts.Utilities;
using System.Windows;
using System.Windows.Threading;

namespace Kts.Importer
{
    /// <summary>
    /// Представляет основной класс приложения.
    /// </summary>
    internal sealed partial class App : Application
    {
        #region Закрытые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private IMessageService messageService;

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Application.DispatcherUnhandledException"/> основного класса приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.messageService.ShowMessage("Непредвиденная ошибка", "Импортер данных", MessageType.Error);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Application.Startup"/> основного класса приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.messageService = new WpfMessageService(this);

            var dataService = new SqlDataService(new SqlConnectionString("172.16.3.85", "sa", "gjghj,eqgjl,thb", "Gis", 0, "Основной"));

            var mainView = new MainView(dataService, this.messageService);

            mainView.Show();
        }

        #endregion
    }
}