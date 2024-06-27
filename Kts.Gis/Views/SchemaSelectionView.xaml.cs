using Kts.Gis.Data;
using Kts.Gis.ViewModels;
using Kts.Messaging;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление выбора схемы.
    /// </summary>
    internal sealed partial class SchemaSelectionView : Window
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

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SchemaSelectionView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public SchemaSelectionView(SchemaSelectionViewModel viewModel, IDataService dataService, IMessageService messageService)
        {
            this.InitializeComponent();

            this.dataService = dataService;
            this.messageService = messageService;

            this.DataContext = viewModel;
            
            viewModel.OK += this.ViewModel_OK;
        }

        #endregion

        #region Обработчики событий
        
        /// <summary>
        /// Обрабатывает событие <see cref="SchemaSelectionViewModel.OK"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_OK(object sender, System.EventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}