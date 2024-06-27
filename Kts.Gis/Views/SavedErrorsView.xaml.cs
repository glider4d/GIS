using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление ошибок сохраненных значений параметров объектов.
    /// </summary>
    internal sealed partial class SavedErrorsView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly SavedErrorsViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SavedErrorsView"/>.
        /// </summary>
        /// <param name="savedErrorsViewModel">Модель представления сохраненных значений параметров объектов.</param>
        public SavedErrorsView(SavedErrorsViewModel savedErrorsViewModel)
        {
            this.InitializeComponent();

            this.viewModel = savedErrorsViewModel;

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