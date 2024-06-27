using Kts.Gis.Reports.ViewModels;
using Kts.WpfUtilities;
using System.Windows;

namespace Kts.Gis.Reports.Views
{
    /// <summary>
    /// Представляет представление формирования отчета с информацией о количестве введенных объектов.
    /// </summary>
    public sealed partial class AddedObjectInfoView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddedObjectInfoView"/>.
        /// </summary>
        /// <param name="addedObjectInfoViewModel">Модель представления формирования отчета с информацией о количестве введенных объектов.</param>
        public AddedObjectInfoView(AddedObjectInfoViewModel addedObjectInfoViewModel)
        {
            this.InitializeComponent();

            this.DataContext = addedObjectInfoViewModel;

            addedObjectInfoViewModel.LongTimeTaskRequested += this.AddedObjectInfoViewModel_LongTimeTaskRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="AddedObjectInfoViewModel.LongTimeTaskRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddedObjectInfoViewModel_LongTimeTaskRequested(object sender, LongTimeTaskRequestedEventArgs e)
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