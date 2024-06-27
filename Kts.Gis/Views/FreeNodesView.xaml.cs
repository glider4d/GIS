using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представления неподключенных узлов.
    /// </summary>
    internal sealed partial class FreeNodesView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly FreeNodesViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FreeNodesView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public FreeNodesView(FreeNodesViewModel viewModel)
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
        /// Обрабатывает событие <see cref="System.Windows.Controls.Control.MouseDoubleClick"/> ячейки сетки данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.viewModel.ShowSelectedNode();
        }

        #endregion
    }
}