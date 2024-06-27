using Kts.Gis.ViewModels;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление остатков сопоставления с базовыми программами КТС.
    /// </summary>
    internal sealed partial class KtsLeftoversView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KtsLeftoversView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public KtsLeftoversView(KtsLeftoversViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;
        }

        #endregion

        #region Обработчики событий
        
        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> окна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            var owner = this.Owner;

            // Добавим небольшой отступ, чтобы окно не было прям на краю экрана.
            this.Left = owner.Left + 25;
            this.Top = owner.Top + SystemParameters.MaximizedPrimaryScreenHeight - this.ActualHeight - 100;
        }

        #endregion
    }
}