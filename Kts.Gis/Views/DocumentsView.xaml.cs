using Kts.Gis.ViewModels;
using Kts.Utilities;
using System.Windows;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление отображения документов объекта.
    /// </summary>
    internal sealed partial class DocumentsView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly DocumentsViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentsView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public DocumentsView(DocumentsViewModel viewModel)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;

            this.DataContext = viewModel;

            this.viewModel.DocumentAddRequested += this.ViewModel_DocumentAddRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Control.MouseDoubleClick"/> ячейки сетки данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.viewModel.OpenDocumentCommand.CanExecute(null))
                this.viewModel.OpenDocumentCommand.Execute(null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="DocumentsViewModel.DocumentAddRequested"/> модели представлений.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_DocumentAddRequested(object sender, ViewRequestedEventArgs<AddDocumentViewModel> e)
        {
            var view = new AddDocumentView(e.ViewModel)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            e.Result = result.HasValue && result.Value;
        }

        #endregion
    }
}