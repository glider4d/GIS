using Kts.Gis.ViewModels;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление выбора параметров.
    /// </summary>
    internal sealed partial class ParameterSelectionView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterSelectionView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ParameterSelectionView(ParameterSelectionViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ParameterSelectionViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}