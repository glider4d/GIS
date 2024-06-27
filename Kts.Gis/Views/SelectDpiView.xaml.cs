using Kts.Gis.ViewModels;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление выбора DPI.
    /// </summary>
    internal sealed partial class SelectDpiView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SelectDpiView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public SelectDpiView(SelectDpiViewModel viewModel)
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