using System;
using System.Windows;

namespace Kts.Messaging
{
    /// <summary>
    /// Представляет представление ввода значения.
    /// </summary>
    public sealed partial class ValueInputView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValueInputView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="icon">Иконка окна.</param>
        public ValueInputView(ValueInputViewModel viewModel)
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
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.textBox.SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ValueInputViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}