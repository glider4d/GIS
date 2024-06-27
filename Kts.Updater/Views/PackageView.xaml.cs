using Kts.Updater.ViewModels;
using System.Windows;

namespace Kts.Updater.Views
{
    /// <summary>
    /// Представляет представление пакета обновления.
    /// </summary>
    internal sealed partial class PackageView : Window
    {
        #region Конструкторы
    
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PackageView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public PackageView(PackageViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
            viewModel.ErrorFound += this.ViewModel_ErrorFound;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Отмена".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PackageViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, System.EventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PackageViewModel.ErrorFound"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_ErrorFound(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}