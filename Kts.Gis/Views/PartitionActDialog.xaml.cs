using Kts.Gis.ViewModels;
using Kts.Messaging;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет диалог формирования акта раздела границ.
    /// </summary>
    internal sealed partial class PartitionActDialog : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Карта.
        /// </summary>
        private readonly Map map;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly PartitionActViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PartitionActDialog"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="map">Карта.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public PartitionActDialog(PartitionActViewModel viewModel, Map map, IMessageService messageService)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;
            this.map = map;
            this.messageService = messageService;

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
            viewModel.TakeMapScreenshot += this.ViewModel_TakeMapScreenshot;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="PartitionActViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, System.EventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PartitionActViewModel.TakeMapScreenshot"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_TakeMapScreenshot(object sender, System.EventArgs e)
        {
            if (!this.map.IsPrintAreaVisible)
            {
                this.messageService.ShowMessage("Не выбрана область печати", "Захват области", MessageType.Error);

                return;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> окна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            var owner = this.Owner;

            // Добавим небольшой отступ, чтобы окно не было прям на краю экрана.
            this.Left = owner.Left + 50;
            this.Top = owner.Top + SystemParameters.MaximizedPrimaryScreenHeight / 2 - this.ActualHeight / 2;
        }

        #endregion
    }
}