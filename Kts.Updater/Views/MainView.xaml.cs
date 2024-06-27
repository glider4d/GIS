using Kts.Updater.ViewModels;
using System;
using System.Windows;

namespace Kts.Updater.Views
{
    /// <summary>
    /// Представляет главное представление.
    /// </summary>
    internal sealed partial class MainView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainView"/>.
        /// </summary>
        /// <param name="viewModel">Главная модель представления.</param>
        public MainView(MainViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
            viewModel.Packaging += this.ViewModel_Packaging;

            this.Title = viewModel.Target + " - Обновлятор";
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
        /// Обрабатывает событие <see cref="MainViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.Packaging"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_Packaging(object sender, PackagingEventArgs e)
        {
            var view = new PackageView(e.Package)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
                e.IsCreated = true;
        }

        #endregion
    }
}