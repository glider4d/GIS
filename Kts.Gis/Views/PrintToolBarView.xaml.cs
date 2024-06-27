using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет панель инструментов области печати.
    /// </summary>
    internal sealed partial class PrintToolBarView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Карта.
        /// </summary>
        private readonly Map map;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintToolBarView"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        public PrintToolBarView(Map map)
        {
            this.InitializeComponent();

            this.map = map;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Добавить листы".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonAddPages_Click(object sender, RoutedEventArgs e)
        {
            this.map.PrintArea.IncreasePageCount();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Убрать листы".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonRemovePages_Click(object sender, RoutedEventArgs e)
        {
            this.map.PrintArea.DecreasePageCount();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Повернуть налево".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonRotateLeft_Click(object sender, RoutedEventArgs e)
        {
            this.map.PrintArea.RotateLeft();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Повернуть направо".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonRotateRight_Click(object sender, RoutedEventArgs e)
        {
            this.map.PrintArea.RotateRight();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> текстового поля ввода угла поворота области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => (sender as TextBox).SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.TextBoxBase.TextChanged"/> текстового поля ввода угла поворота области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int angle;

            if (int.TryParse((sender as TextBox).Text, out angle))
                this.map.PrintArea.Rotate(angle);
        }

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
        /// Обрабатывает событие <see cref="UIElement.PreviewKeyDown"/> окна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemPlus || e.Key == Key.Add)
                this.map.PrintArea.IncreasePageCount();

            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                this.map.PrintArea.DecreasePageCount();

            if (e.Key == Key.D9)
                this.map.PrintArea.RotateLeft();

            if (e.Key == Key.D0)
                this.map.PrintArea.RotateRight();
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