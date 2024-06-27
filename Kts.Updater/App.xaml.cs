using Kts.Messaging;
using Kts.Updater.Views;
using System.Windows;

namespace Kts.Updater
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
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            this.messageService.ShowMessage("Непредвиденная ошибка", "Обновлятор", MessageType.Error);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Application.Startup"/> основного класса приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            this.messageService = new WpfMessageService(this);
            
            var mainView = new ServerView(this.messageService);

            mainView.Show();
        }

        #endregion
    }
}