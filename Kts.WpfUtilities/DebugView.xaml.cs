using System;
using System.Linq;
using System.Windows;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет представление отладки.
    /// </summary>
    public sealed partial class DebugView : Window
    {
        #region Закрытые константы

        /// <summary>
        /// Отступ расположения представления.
        /// </summary>
        private const int offset = 10;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DebugView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public DebugView(DebugViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.ItemAdded += this.ViewModel_ItemAdded;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="DebugViewModel.ItemAdded"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_ItemAdded(object sender, EventArgs e)
        {
            this.dataGrid.ScrollIntoView((sender as DebugViewModel).Items.Last());
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            this.Left = workArea.Right - this.Width - offset;
            this.Top = workArea.Bottom - this.Height - offset;
        }

        #endregion
    }
}