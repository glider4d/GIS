using Kts.Gis.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление добавления документа.
    /// </summary>
    internal sealed partial class AddDocumentView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly AddDocumentViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddDocumentView"/>.
        /// </summary>
        public AddDocumentView(AddDocumentViewModel viewModel)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;

            this.DataContext = viewModel;

            viewModel.BrowseFile += this.ViewModel_BrowseFile;
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
        /// Обрабатывает событие <see cref="AddDocumentViewModel.BrowseFile"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_BrowseFile(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Filter = "Все файлы|*",
                RestoreDirectory = true,
                Title = "Добавление документа"
            };

            var result = dlg.ShowDialog();

            if (result.HasValue && result.Value == true)
                this.viewModel.Path = dlg.FileName;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="AddDocumentViewModel.CloseRequested"/> модели представления.
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