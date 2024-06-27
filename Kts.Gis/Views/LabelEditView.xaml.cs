using Kts.Gis.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление редактирования надписи.
    /// </summary>
    internal sealed partial class LabelEditView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelEditView"/>.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="sizes">Допустимые размеры надписи.</param>
        public LabelEditView(LabelViewModel label, List<int> sizes)
        {
            this.InitializeComponent();

            var viewModel = new LabelEditViewModel(label, sizes);

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
            this.Dispatcher.BeginInvoke(new Action(() => (sender as TextBox).SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="LabelEditViewModel.CloseRequested"/> модели представления.
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