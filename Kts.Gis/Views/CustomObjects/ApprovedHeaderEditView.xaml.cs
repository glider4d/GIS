using Kts.Gis.ViewModels;
using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление редактирования заголовка утверждения/согласования.
    /// </summary>
    internal sealed partial class ApprovedHeaderEditView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApprovedHeaderEditView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ApprovedHeaderEditView(ApprovedHeaderEditViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> поля ввода должности.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.textBox.SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="LabelEditViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, System.EventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}