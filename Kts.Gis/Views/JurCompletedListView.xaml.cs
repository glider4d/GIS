using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление сопоставленных объектов с программой "Расчеты с юридическими лицами".
    /// </summary>
    internal sealed partial class JurCompletedListView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly JurKvpCompletedListViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ObjectListView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public JurCompletedListView(JurKvpCompletedListViewModel viewModel)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;

            this.DataContext = this.viewModel;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Закрыть".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Control.MouseDoubleClick"/> ячейки сетки данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.viewModel.ShowSelectedObject();
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
            this.Left = owner.Left + 25;
            this.Top = owner.Top + SystemParameters.MaximizedPrimaryScreenHeight - this.ActualHeight - 100;
        }

        #endregion
    }
}