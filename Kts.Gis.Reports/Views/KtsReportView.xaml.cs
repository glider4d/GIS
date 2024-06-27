using Kts.Gis.Reports.ViewModels;
using Kts.WpfUtilities;
using System.Windows;

namespace Kts.Gis.Reports.Views
{
    /// <summary>
    /// Представление диалога составления отчета КТС.
    /// </summary>
    public sealed partial class KtsReportView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KtsReportView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public KtsReportView(KtsReportViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            viewModel.LongTimeTaskRequested += this.ViewModel_LongTimeTaskRequested;
        }


        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="KtsKvpReportViewModel.LongTimeTaskRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_LongTimeTaskRequested(object sender, WpfUtilities.LongTimeTaskRequestedEventArgs e)
        {
            var view = new WaitView(e.WaitViewModel, this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();

            this.Close();
        }

        #endregion
    }
}