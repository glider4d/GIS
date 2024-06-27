using Kts.Gis.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление изменения отображения длины.
    /// </summary>
    internal sealed partial class ChangeLengthView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLengthView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ChangeLengthView(ChangeLengthViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> поля ввода значения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => (sender as TextBox).SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ChangeLengthViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}