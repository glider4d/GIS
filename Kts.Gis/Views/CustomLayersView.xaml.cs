using Kts.Gis.ViewModels;
using Kts.Messaging;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление кастомных слоев.
    /// </summary>
    internal sealed partial class CustomLayersView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomLayersView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public CustomLayersView(CustomLayersViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.ValueInputRequested += this.ViewModel_ValueInputRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "ОК".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="CustomLayersViewModel.ValueInputRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_ValueInputRequested(object sender, ValueInputViewRequestedEventArgs e)
        {
            var view = new ValueInputView(e.ViewModel)
            {
                Icon = this.Icon,
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
                e.HasResult = true;
        }

        #endregion
    }
}