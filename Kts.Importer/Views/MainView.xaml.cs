using Kts.Importer.Data;
using Kts.Importer.ViewModels;
using Kts.Messaging;
using Microsoft.Win32;
using System.Windows;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет главное представление.
    /// </summary>
    internal sealed partial class MainView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly MainViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainView"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public MainView(IDataService dataService, IMessageService messageService)
        {
            this.InitializeComponent();

            this.dataService = dataService;
            this.messageService = messageService;

            this.viewModel = new MainViewModel(dataService);

            this.DataContext = this.viewModel;

            this.viewModel.CanStartImportChanged += this.ViewModel_CanStartImportChanged;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки выбора файла-источника данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Отображаем диалог выбора файла.
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Книга Excel (*.xlsx)|*.xlsx",
                FilterIndex = 3,
                RestoreDirectory = true,
                Title = "Выбор файла-источника данных",
                Multiselect = false
            };
            bool? result = dlg.ShowDialog();

            if (result == true)
                this.viewModel.SourcePath = dlg.FileName;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.CanStartImportChanged"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CanStartImportChanged(object sender, System.EventArgs e)
        {
            var detail = new ImportationView(this.viewModel.SourcePath, this.viewModel.SelectedType, this.dataService, this.messageService)
            {
                Owner = this
            };

            detail.ShowDialog();
        }

        #endregion
    }
}