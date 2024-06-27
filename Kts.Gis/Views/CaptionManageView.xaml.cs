using Kts.Gis.ViewModels;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление настройки надписей.
    /// </summary>
    internal sealed partial class CaptionManageView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly CaptionManageViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CaptionManageView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public CaptionManageView(CaptionManageViewModel viewModel)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;

            this.DataContext = viewModel;

            this.viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="CaptionManageViewModel.CloseRequested"/> модели представления.
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