using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление поиска.
    /// </summary>
    internal sealed partial class SearchView : UserControl
    {
        #region Закрытые поля

        /// <summary>
        /// Модель представления поиска.
        /// </summary>
        private SearchViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchView"/>.
        /// </summary>
        public SearchView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Control.MouseDoubleClick"/> ячейки сетки данных.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.viewModel.ShowSelectedSearchEntry();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.LongTimeTaskRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_LongTimeTaskRequested(object sender, WpfUtilities.LongTimeTaskRequestedEventArgs e)
        {
            var owner = Window.GetWindow(this);

            var view = new WaitView(e.WaitViewModel, owner.Icon)
            {
                Owner = owner
            };

            view.ShowDialog();
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает контекст данных.
        /// </summary>
        /// <param name="viewModel">Модель представления поиска.</param>
        public void SetDataContext(SearchViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = this.viewModel;

            // Добавляем привязки к названиям столбцов со значениями параметров.
            var binding = new Binding();
            binding.Path = new PropertyPath("ColumnHeaders[0]");
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column1, DataGridColumn.HeaderProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnHeaders[1]");
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column2, DataGridColumn.HeaderProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnHeaders[2]");
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column3, DataGridColumn.HeaderProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnHeaders[3]");
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column4, DataGridColumn.HeaderProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnHeaders[4]");
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column5, DataGridColumn.HeaderProperty, binding);
            // Добавляем привязки видимости столбцов со значениями параметров.
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnVisibilities[0]");
            binding.Converter = new BoolToVisibilityConverter();
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column1, DataGridColumn.VisibilityProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnVisibilities[1]");
            binding.Converter = new BoolToVisibilityConverter();
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column2, DataGridColumn.VisibilityProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnVisibilities[2]");
            binding.Converter = new BoolToVisibilityConverter();
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column3, DataGridColumn.VisibilityProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnVisibilities[3]");
            binding.Converter = new BoolToVisibilityConverter();
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column4, DataGridColumn.VisibilityProperty, binding);
            binding = new Binding();
            binding.Path = new PropertyPath("ColumnVisibilities[4]");
            binding.Converter = new BoolToVisibilityConverter();
            binding.Source = this.DataContext;
            BindingOperations.SetBinding(this.column5, DataGridColumn.VisibilityProperty, binding);

            viewModel.LongTimeTaskRequested += this.ViewModel_LongTimeTaskRequested;
        }

        #endregion
    }
}