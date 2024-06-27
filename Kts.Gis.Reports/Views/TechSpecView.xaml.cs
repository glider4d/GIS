using Kts.Gis.Reports.ViewModels;
using Kts.WpfUtilities;
using System.Windows;

namespace Kts.Gis.Reports.Views
{
    /// <summary>
    /// Представляет представление формирования отчета о технических характеристиках.
    /// </summary>
    public sealed partial class TechSpecView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TechSpecView"/>.
        /// </summary>
        /// <param name="techSpecViewModel">Модель представления формирования отчета о технических характеристиках.</param>
        public TechSpecView(TechSpecViewModel techSpecViewModel)
        {
            this.InitializeComponent();

            this.DataContext = techSpecViewModel;

            techSpecViewModel.LongTimeTaskRequested += this.TechSpecViewModel_LongTimeTaskRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="TechSpecViewModel.LongTimeTaskRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TechSpecViewModel_LongTimeTaskRequested(object sender, LongTimeTaskRequestedEventArgs e)
        {
            var view = new WaitView(e.WaitViewModel, this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        #endregion
    }
}