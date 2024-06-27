using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление ошибок.
    /// </summary>
    internal sealed partial class ErrorsView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly ErrorsViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ErrorsView"/>.
        /// </summary>
        /// <param name="errorsViewModel">Модель представления ошибок.</param>
        public ErrorsView(ErrorsViewModel errorsViewModel)
        {
            this.InitializeComponent();

            this.viewModel = errorsViewModel;

            this.DataContext = this.viewModel;
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
            this.viewModel.ShowSelectedItem();
        }
        
        #endregion
    }
}