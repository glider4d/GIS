using Kts.Gis.ViewModels;
using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление выбора котельной.
    /// </summary>
    internal partial class SelectBoilerView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SelectBoilerView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public SelectBoilerView(SelectBoilerViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="SelectBoilerViewModel.CloseRequested"/> модели представления.
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