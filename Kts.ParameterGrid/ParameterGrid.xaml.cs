using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;


namespace Kts.ParameterGrid
{
    /// <summary>
    /// Представляет сетку параметров.
    /// </summary>
    public sealed partial class ParameterGrid : UserControl, INotifyPropertyChanged
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что можно ли очистить фильтр.
        /// </summary>
        private bool canClearFilter;

        /// <summary>
        /// Фильтр.
        /// </summary>
        private string filter = "";

        /// <summary>
        /// Значение, указывающее на то, что открыт ли какой-нибудь комбобокс.
        /// </summary>
        private bool isComboBoxOpen;

        /// <summary>
        /// Выбранная сетка параметров.
        /// </summary>
        private DataGrid selectedDataGrid;

        /// <summary>
        /// Выбранная строка.
        /// </summary>
        private DataGridRow selectedRow;

        #endregion

        #region Открытые статические поля

        /// <summary>
        /// Ширина столбца заголовков параметров.
        /// </summary>
        public static DependencyProperty HeaderColumnWidthProperty = DependencyProperty.Register("HeaderColumnWidth", typeof(double), typeof(ParameterGrid));

        /// <summary>
        /// Значение, указывающее на то, что стоит ли скрывать параметры с пустыми значениями.
        /// </summary>
        public static DependencyProperty HideEmptyProperty = DependencyProperty.Register("HideEmpty", typeof(bool), typeof(ParameterGrid), new PropertyMetadata(false, new PropertyChangedCallback(HideEmptyPropertyChanged)));

        /// <summary>
        /// Параметры.
        /// </summary>
        public static DependencyProperty ParametersProperty = DependencyProperty.Register("Parameters", typeof(IEnumerable<IParameter>), typeof(ParameterGrid), new PropertyMetadata(null, new PropertyChangedCallback(ParametersPropertyChanged)));

        /// <summary>
        /// Заголовок сетки параметров.
        /// </summary>
        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ParameterGrid));

        /// <summary>
        /// Ширина столбца значений параметров.
        /// </summary>
        public static DependencyProperty ValueColumnWidthProperty = DependencyProperty.Register("ValueColumnWidth", typeof(double), typeof(ParameterGrid));

        #endregion
        
        #region Открытые события

        /// <summary>
        /// Событие завершения редактирования значения параметра.
        /// </summary>
        public event EventHandler<EditCompletedEventArgs> EditCompleted;

        /// <summary>
        /// Событие изменения свойства сетки параметров.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterGrid"/>.
        /// </summary>
        public ParameterGrid()
        {
            this.InitializeComponent();

            // Инициализируем года.
            this.Years = new List<int>();
            for (int i = 1980; i <= DateTime.Now.Year; i++)
                this.Years.Add(i);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли очистить фильтр.
        /// </summary>
        public bool CanClearFilter
        {
            get
            {
                return this.canClearFilter;
            }
            private set
            {
                this.canClearFilter = value;

                this.NotifyPropertyChanged(nameof(this.CanClearFilter));
            }
        }

        /// <summary>
        /// Возвращает или задает фильтр.
        /// </summary>
        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;

                if (this.Parameters != null)
                    // Обходим все отображаемые параметры и их дочерние параметры, чтобы изменить значение отфильтрованности.
                    this.FilterAll(this.Parameters, value);

                this.CanClearFilter = !string.IsNullOrEmpty(value);

                this.NotifyPropertyChanged(nameof(this.Filter));
            }
        }

        /// <summary>
        /// Возвращает или задает ширину столбцов заголовков параметров.
        /// </summary>
        public double HeaderColumnWidth
        {
            get
            {
                return (double)this.GetValue(HeaderColumnWidthProperty);
            }
            set
            {
                this.SetValue(HeaderColumnWidthProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что стоит ли скрывать параметры с пустыми значениями.
        /// </summary>
        public bool HideEmpty
        {
            get
            {
                return (bool)this.GetValue(HideEmptyProperty);
            }
            set
            {
                this.SetValue(HideEmptyProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает параметры.
        /// </summary>
        public IEnumerable<IParameter> Parameters
        {
            get
            {

                IEnumerable<IParameter> t = (IEnumerable<IParameter>)this.GetValue(ParametersProperty);
                

                return (IEnumerable<IParameter>)this.GetValue(ParametersProperty);
                
            }
            set
            {
                this.SetValue(ParametersProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает заголовок сетки параметров.
        /// </summary>
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }
            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Возвращает список из "Да" и "Нет".
        /// </summary>
        public List<Tuple<string, bool>> TrueFalseList
        {
            get;
        } = new List<Tuple<string, bool>>()
        {
            new Tuple<string, bool>("Да", true),
            new Tuple<string, bool>("Нет", false)
        };

        /// <summary>
        /// Возвращает или задает ширину столбцов значений параметров.
        /// </summary>
        public double ValueColumnWidth
        {
            get
            {
                return (double)this.GetValue(ValueColumnWidthProperty);
            }
            set
            {
                this.SetValue(ValueColumnWidthProperty, value);
            }
        }

        /// <summary>
        /// Возвращает список годов.
        /// </summary>
        public List<int> Years
        {
            get;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ButtonBase.Click"/> кнопки очистки фильтра.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Filter = "";
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ComboBox.DropDownClosed"/> комбобокса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var dataGrid = this.FindVisualParent<DataGrid>(sender as ComboBox);

            dataGrid.CommitEdit();

            this.isComboBoxOpen = false;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ComboBox.DropDownOpened"/> комбобокса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            this.isComboBoxOpen = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> комбобокса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).DropDownOpened += this.ComboBox_DropDownOpened;
            (sender as ComboBox).DropDownClosed += this.ComboBox_DropDownClosed;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Unloaded"/> комбобокса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ComboBox_Unloaded(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).DropDownOpened -= this.ComboBox_DropDownOpened;
            (sender as ComboBox).DropDownClosed -= this.ComboBox_DropDownClosed;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="DataGrid.CellEditEnding"/> сетки параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var param = (e.EditingElement as ContentPresenter).Content as IParameter;

            if (param.ValueEditor == ValueEditor.None)
                return;

            this.EditCompleted?.Invoke(this, new EditCompletedEventArgs(param));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseWheel"/> сетки параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this.isComboBoxOpen)
                return;

            var dataGrid = sender as DataGrid;

            if (dataGrid != null)
            {
                var scrollViewer = dataGrid.Template.FindName("DG_ScrollViewer", dataGrid) as ScrollViewer;

                if (scrollViewer != null)
                    if (e.Delta > 0)
                        ScrollBar.LineUpCommand.Execute(null, scrollViewer);
                    else
                        ScrollBar.LineDownCommand.Execute(null, scrollViewer);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="DataGrid.SelectedCellsChanged"/> сетки параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count > 0)
            {
                var dataGrid = sender as DataGrid;

                if (this.selectedDataGrid != null && this.selectedDataGrid != dataGrid)
                    this.selectedDataGrid.UnselectAllCells();

                this.selectedDataGrid = dataGrid;
                this.selectedRow = (DataGridRow)this.selectedDataGrid.ItemContainerGenerator.ContainerFromItem(e.AddedCells[0].Item);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> ячейки сетки параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var cell = sender as DataGridCell;

            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                if (!cell.IsFocused)
                    cell.Focus();

                var dataGrid = this.FindVisualParent<DataGrid>(cell);

                if (dataGrid != null)
                    if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
                    {
                        if (!cell.IsSelected)
                            cell.IsSelected = true;
                    }
                    else
                    {
                        var row = this.FindVisualParent<DataGridRow>(cell);

                        if (row != null && !row.IsSelected)
                            row.IsSelected = true;
                    }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> редактируемого текстового поля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void EditingTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => (sender as TextBox).SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Expander.Collapsed"/> расширителя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (this.selectedRow != null)
                this.selectedRow.DetailsVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Expander.Expanded"/> расширителя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (this.selectedRow != null)
                this.selectedRow.DetailsVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeftButtonDown"/> панели, содержащей текстовый блок.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            // Получаем соседний расширитель.
            var expander = ((sender as Grid).Parent as Grid).Children[0] as Expander;

            expander.IsExpanded = !expander.IsExpanded;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> контекстного меню копирования.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText((sender as MenuItem).Tag.ToString());
        }

        #endregion

        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="HideEmptyProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void HideEmptyPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var paramGrid = source as ParameterGrid;

            paramGrid.Filter = paramGrid.Filter;
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="ParametersProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void ParametersPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var paramGrid = source as ParameterGrid;

            paramGrid.Filter = paramGrid.Filter;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Задает значение отфильтрованности заданным параметрам по заданному фильтру.
        /// </summary>
        /// <param name="parameters">Параметры.</param>
        /// <param name="filter">Фильтр.</param>
        /// <returns>true, если не нашлось неотфильтрованного параметра, иначе - false.</returns>
        private bool FilterAll(IEnumerable<IParameter> parameters, string filter)
        {
            var result = true;
            
            foreach (var param in parameters)
            {
                if (param.Children.Count > 0)
                {
                    param.IsFilteredOut = this.FilterAll(param.Children, filter);

                    if (!param.IsFilteredOut)
                        result = false;
                }
                else
                    if (param.Header.ToLower().Contains(filter.ToLower()))
                        if (this.HideEmpty)
                        {
                            param.IsFilteredOut = string.IsNullOrEmpty(Convert.ToString(param.DisplayedValue));

                            if (!param.IsFilteredOut)
                                result = false;
                        }
                        else
                        {
                            param.IsFilteredOut = false;

                            result = false;
                        }
                    else
                        param.IsFilteredOut = true;
            }

            return result;
        }

        /// <summary>
        /// Находит визуального родителя.
        /// </summary>
        /// <typeparam name="T">Тип родителя.</typeparam>
        /// <param name="element">Элемент, для которого ищется родитель.</param>
        /// <returns>Визуальный родитель.</returns>
        private T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            var parent = element;

            while (parent != null)
            {
                var correctlyTyped = parent as T;

                if (correctlyTyped != null)
                    return correctlyTyped;

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        /// <summary>
        /// Уведомляет об изменении свойства сетки параметров.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}