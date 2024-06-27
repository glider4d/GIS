using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет панель инструментов области группового редактирования.
    /// </summary>
    internal sealed partial class GroupAreaToolBarView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupAreaToolBarView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public GroupAreaToolBarView(GroupAreaToolBarViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;

            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;

            if (overflowGrid != null)
                overflowGrid.Visibility = Visibility.Collapsed;

            if (mainPanelBorder != null)
                mainPanelBorder.Margin = new Thickness();
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
            this.Left = owner.Left + SystemParameters.MaximizedPrimaryScreenWidth - this.ActualWidth - 50;
            this.Top = owner.Top + SystemParameters.MaximizedPrimaryScreenHeight / 2 - this.ActualHeight / 2;
        }

        #endregion
    }
}