using Kts.Gis.ViewModels;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление выбора принтера.
    /// </summary>
    internal sealed partial class PrinterSelectionView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrinterSelectionView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public PrinterSelectionView(PrinterSelectionViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "ОК".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}