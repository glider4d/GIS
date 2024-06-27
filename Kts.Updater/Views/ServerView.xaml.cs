using Kts.Messaging;
using Kts.Updater.Services;
using Kts.Updater.ViewModels;
using System;
using System.Windows;

namespace Kts.Updater.Views
{
    /// <summary>
    /// Представляет представление выбора сервера.
    /// </summary>
    internal sealed partial class ServerView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly SqlDataService dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly ServerViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ServerView"/>.
        /// </summary>
        /// <param name="messageService">Сервис сообщений.</param>
        public ServerView(IMessageService messageService)
        {
            this.InitializeComponent();

            this.messageService = messageService;

            this.dataService = new SqlDataService();

            this.viewModel = new ServerViewModel(this.dataService, messageService, new SettingService());

            this.viewModel.DataSelected += this.ViewModel_DataSelected;

            this.DataContext = this.viewModel;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ServerViewModel.DataSelected"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_DataSelected(object sender, EventArgs e)
        {
            this.viewModel.DataSelected -= this.ViewModel_DataSelected;

            var mainView = new MainView(new MainViewModel(this.viewModel.ConnectionString, this.dataService, this.messageService));

            mainView.Show();

            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            this.Close();
        }

        #endregion
    }
}