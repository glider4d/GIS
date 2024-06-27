using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления поиска.
    /// </summary>
    internal sealed class SearchViewModel : ServicedViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Количество столбцов со значениями параметров.
        /// </summary>
        private const int columnCount = 5;

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что находятся ли в результате поиска объекты с одного населенного пункта.
        /// </summary>
        private bool oneCity;

        /// <summary>
        /// Операторы.
        /// </summary>
        private Dictionary<Operator, string> operators;

        /// <summary>
        /// Искомая дата.
        /// </summary>
        private DateTime searchDateValue = DateTime.Now;

        /// <summary>
        /// Искомое численное значение.
        /// </summary>
        private object searchNumValue;

        /// <summary>
        /// Искомое текстовое значение.
        /// </summary>
        private string searchTextValue;

        /// <summary>
        /// Выбранный оператор.
        /// </summary>
        private Operator selectedOperator;

        /// <summary>
        /// Выбранный параметр.
        /// </summary>
        private SearchingParameterViewModel selectedParameter;

        /// <summary>
        /// Выбранное условие поиска.
        /// </summary>
        private SearchTermViewModel selectedSearchTerm;

        /// <summary>
        /// Выбранный тип объекта.
        /// </summary>
        private ObjectType selectedType;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса выполнения долгодлительной задачи.
        /// </summary>
        public event EventHandler<LongTimeTaskRequestedEventArgs> LongTimeTaskRequested;

        /// <summary>
        /// Событие запроса отображения объекта на карте.
        /// </summary>
        public event EventHandler<ShowOnMapRequestedEventArgs> ShowOnMapRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchViewModel"/>.
        /// </summary>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public SearchViewModel(MainViewModel mainViewModel, AccessService accessService, IDataService dataService, IMessageService messageService) : base(dataService, messageService)
        {
            this.MainViewModel = mainViewModel;

            this.AddTermCommand = new RelayCommand(this.ExecuteAddTerm);
            this.ClearCommand = new RelayCommand(this.ExecuteClear);
            this.ClearTermsCommand = new RelayCommand(this.ExecuteClearTerms, this.CanExecuteClearTerms);
            this.DeleteTermCommand = new RelayCommand(this.ExecuteDeleteTerm, this.CanExecuteDeleteTerm);
            this.FindCommand = new RelayCommand(this.ExecuteFind, this.CanExecuteFind);
            this.SelectAllCommand = new RelayCommand(this.ExecuteSelectAll, this.CanExecuteSelectAll);

            // Заполняем типы объектов.
            var list = new List<GroupedTypeViewModel>();
            var paramTypes = dataService.ObjectTypes.Where(x => x.HasParameters);
            foreach (var type in paramTypes.Where(x => x.ObjectKind == ObjectKind.Figure))
                list.Add(new GroupedTypeViewModel("Здания", type));
            foreach (var type in paramTypes.Where(x => x.ObjectKind == ObjectKind.Line))
                list.Add(new GroupedTypeViewModel("Трубы", type));
            foreach (var type in paramTypes.Where(x => x.ObjectKind == ObjectKind.NonVisualObject))
                list.Add(new GroupedTypeViewModel("Внутренние объекты зданий", type));
            foreach (var type in paramTypes.Where(x => x.ObjectKind == ObjectKind.Badge))
                list.Add(new GroupedTypeViewModel("Внутренние объекты труб", type));

            this.Types = CollectionViewSource.GetDefaultView(list);
            this.Types.GroupDescriptions.Add(new PropertyGroupDescription(nameof(GroupedTypeViewModel.GroupName)));

            // Выбираем первый тип объекта.
            this.SelectedType = list[0].ObjectType;

            // Инициализируем работу с территориальными единицами.
            this.RegionSelectionViewModel = new RegionSelectionViewModel(accessService.PermittedRegions, dataService);
            this.RegionSelectionViewModel.Init();

            this.SearchTerms.CollectionChanged += this.SearchTerms_CollectionChanged;

            for (int i = 0; i < columnCount; i++)
                this.ColumnVisibilities.Add(false);
        }

        #endregion
        
        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления условия поиска.
        /// </summary>
        public RelayCommand AddTermCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду, очищающую список результатов.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду очистки условий поиска.
        /// </summary>
        public RelayCommand ClearTermsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает заголовки столбцов.
        /// </summary>
        public AdvancedObservableCollection<string> ColumnHeaders
        {
            get;
        } = new AdvancedObservableCollection<string>();

        /// <summary>
        /// Возвращает видимости столбцов.
        /// </summary>
        public AdvancedObservableCollection<bool> ColumnVisibilities
        {
            get;
        } = new AdvancedObservableCollection<bool>();

        /// <summary>
        /// Возвращает команду удаления условия поиска.
        /// </summary>
        public RelayCommand DeleteTermCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду, выполняющую поиск.
        /// </summary>
        public RelayCommand FindCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает главную модель представления.
        /// </summary>
        public MainViewModel MainViewModel
        {
            get;
        }

        /// <summary>
        /// Операторы.
        /// </summary>
        public Dictionary<Operator, string> Operators
        {
            get
            {
                return this.operators;
            }
            private set
            {
                this.operators = value;

                this.NotifyPropertyChanged(nameof(this.Operators));
            }
        }

        /// <summary>
        /// Возвращает параметры.
        /// </summary>
        public AdvancedObservableCollection<SearchingParameterViewModel> Parameters
        {
            get;
        } = new AdvancedObservableCollection<SearchingParameterViewModel>();

        /// <summary>
        /// Возвращает модель представления выбора региона.
        /// </summary>
        public RegionSelectionViewModel RegionSelectionViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает результат поиска.
        /// </summary>
        public AdvancedObservableCollection<SearchEntryViewModel> Result
        {
            get;
        } = new AdvancedObservableCollection<SearchEntryViewModel>();

        /// <summary>
        /// Возвращает или задает искомую дату.
        /// </summary>
        public DateTime SearchDateValue
        {
            get
            {
                return this.searchDateValue;
            }
            set
            {
                this.searchDateValue = value;

                this.NotifyPropertyChanged(nameof(this.SearchDateValue));
            }
        }

        /// <summary>
        /// Возвращает или задает искомое численное значение.
        /// </summary>
        public object SearchNumValue
        {
            get
            {
                return this.searchNumValue;
            }
            set
            {
                this.searchNumValue = value;

                this.NotifyPropertyChanged(nameof(this.SearchNumValue));
            }
        }

        /// <summary>
        /// Возвращает условия поиска.
        /// </summary>
        public AdvancedObservableCollection<SearchTermViewModel> SearchTerms
        {
            get;
        } = new AdvancedObservableCollection<SearchTermViewModel>();

        /// <summary>
        /// Возвращает или задает искомое текстовое значение.
        /// </summary>
        public string SearchTextValue
        {
            get
            {
                return this.searchTextValue;
            }
            set
            {
                this.searchTextValue = value;

                this.NotifyPropertyChanged(nameof(this.SearchTextValue));
            }
        }

        /// <summary>
        /// Возвращает команду, выбирающую все объекты.
        /// </summary>
        public RelayCommand SelectAllCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный оператор.
        /// </summary>
        public Operator SelectedOperator
        {
            get
            {
                return this.selectedOperator;
            }
            set
            {
                this.selectedOperator = value;

                this.NotifyPropertyChanged(nameof(this.SelectedOperator));
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный параметр.
        /// </summary>
        public SearchingParameterViewModel SelectedParameter
        {
            get
            {
                return this.selectedParameter;
            }
            set
            {
                if (this.SelectedParameter != value)
                {
                    this.selectedParameter = value;

                    this.NotifyPropertyChanged(nameof(this.SelectedParameter));

                    var temp = new Dictionary<Operator, string>();

                    if (this.SelectedParameter.HasPredefinedValues)
                    {
                        temp.Add(Operator.Equal, "РАВНО");
                        temp.Add(Operator.NotEqual, "НЕ РАВНО");

                        this.SearchNumValue = this.selectedParameter.PredefinedValues[0].Key;
                    }
                    else
                    {
                        temp.Add(Operator.Equal, "РАВНО");
                        temp.Add(Operator.NotEqual, "НЕ РАВНО");

                        if (this.SelectedParameter.IsNumeric || this.SelectedParameter.IsDate)
                        {
                            temp.Add(Operator.Less, "МЕНЬШЕ");
                            temp.Add(Operator.More, "БОЛЬШЕ");
                        }
                        else
                        {
                            temp.Add(Operator.Contains, "СОДЕРЖИТ");
                            temp.Add(Operator.NotContains, "НЕ СОДЕРЖИТ");
                        }
                    }
                    
                    temp.Add(Operator.Empty, "БЕЗ ЗНАЧЕНИЯ");

                    this.Operators = temp;

                    this.SelectedOperator = Operator.Equal;
                }
            }
        }

        /// <summary>
        /// Возвращает или задает выбранную запись результата поиска.
        /// </summary>
        public SearchEntryViewModel SelectedSearchEntry
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранное условие поиска.
        /// </summary>
        public SearchTermViewModel SelectedSearchTerm
        {
            get
            {
                return this.selectedSearchTerm;
            }
            set
            {
                this.selectedSearchTerm = value;

                this.DeleteTermCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный тип объекта.
        /// </summary>
        public ObjectType SelectedType
        {
            get
            {
                return this.selectedType;
            }
            set
            {
                if (this.SelectedType != value)
                {
                    this.selectedType = value;

                    this.NotifyPropertyChanged(nameof(this.SelectedType));

                    this.LoadParameters(value);

                    // Очищаем условия поиска.
                    this.SearchTerms.Clear();
                }
            }
        }

        /// <summary>
        /// Возвращает типы объектов.
        /// </summary>
        public ICollectionView Types
        {
            get;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> параметра, используемого при поиске.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SearchingParameter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchingParameterViewModel.IsSelected))
            {
                var param = sender as SearchingParameterViewModel;

                if (param.IsSelected)
                    this.SelectedParameter = param;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Collections.ObjectModel.ObservableCollection{T}.CollectionChanged"/> условий поиска.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SearchTerms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ClearTermsCommand.RaiseCanExecuteChanged();
            this.FindCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить очистку условий поиска.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteClearTerms()
        {
            return this.SearchTerms.Count > 0;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить выбранное условие поиска.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDeleteTerm()
        {
            return this.SelectedSearchTerm != null;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить поиск.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteFind()
        {
            return this.SearchTerms.Count > 0;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выбрать все объекты.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выбрать все объекты.</returns>
        private bool CanExecuteSelectAll()
        {
            return this.oneCity && this.Result.Count > 0;
        }

        /// <summary>
        /// Очищает результаты поиска.
        /// </summary>
        private void ClearResult()
        {
            this.Result.Clear();

            this.SelectAllCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Выполняет добавление условия поиска.
        /// </summary>
        private void ExecuteAddTerm()
        {
            if (this.SearchTerms.Count >= columnCount)
            {
                this.MessageService.ShowMessage("На данный момент нельзя добавить больше " + columnCount.ToString() + " условий поиска", "Добавление условия поиска", MessageType.Information);

                return;
            }

            string innerValue;
            string value;

            if (this.SelectedParameter.HasPredefinedValues)
            {
                innerValue = Convert.ToString(this.SearchNumValue);
                value = this.SelectedParameter.PredefinedValues.First(x => x.Key == this.SearchNumValue).Value;
            }
            else
            {
                innerValue = this.SearchTextValue != null ? this.SearchTextValue : "";
                if (this.SelectedParameter.IsNumeric)
                    innerValue = innerValue.Replace(",", ".");
                value = innerValue;
            }

            if (this.SelectedParameter.IsDate)
            {
                innerValue = this.SearchDateValue.ToString("yyyy-MM-dd");
                value = innerValue;
            }

            // Проверяем, существует ли уже условие поиска по тому же параметру.
            var searchTerm = new SearchTermViewModel(this.SelectedParameter, this.SelectedOperator, this.Operators[this.SelectedOperator], this.SelectedOperator != Operator.Empty ? value : null, this.SelectedOperator != Operator.Empty ? innerValue : null);
            var prevSearchTerm = this.SearchTerms.FirstOrDefault(x => x.Parameter.Id == searchTerm.Parameter.Id);
            if (prevSearchTerm != null)
            {
                this.SearchTerms.Remove(prevSearchTerm);

                this.SearchTerms.Add(searchTerm);
            }
            else
                this.SearchTerms.Add(searchTerm);
        }

        /// <summary>
        /// Выполняет очистку результатов поиска.
        /// </summary>
        private void ExecuteClear()
        {
            this.ClearResult();
        }

        /// <summary>
        /// Выполняет очистку условий поиска.
        /// </summary>
        private void ExecuteClearTerms()
        {
            this.SearchTerms.Clear();
        }

        /// <summary>
        /// Выполняет удаление выбранного условия поиска.
        /// </summary>
        private void ExecuteDeleteTerm()
        {
            var term = this.SelectedSearchTerm;

            this.SearchTerms.Remove(term);
        }

        /// <summary>
        /// Выполняет поиск.
        /// </summary>
        private void ExecuteFind()
        {
            this.oneCity = false;

            this.ClearResult();

            var list = new List<SearchEntryViewModel>();
            
            // Составляем список условий поиска.
            var searchTerms = new List<SearchTermModel>();
            foreach (var searchTerm in this.SearchTerms)
                searchTerms.Add(new SearchTermModel(this.SelectedType.Parameters.First(x => x.Id == searchTerm.Parameter.Id), searchTerm.Term, searchTerm.InnerValue));

            var waitViewModel = new WaitViewModel("Поиск объектов", "Пожалуйста подождите, идет поиск объектов...", async () =>
            {
                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        var rs = this.RegionSelectionViewModel;

                        foreach (var entry in this.DataService.SearchService.FindObjects(this.SelectedType, rs.SelectedRegion.Id, rs.CanSelectCity ? rs.SelectedCity.Id : -1, rs.SelectedSchema, searchTerms))
                            list.Add(new SearchEntryViewModel(entry));
                    });
                }
                catch
                {
                    this.MessageService.ShowMessage("Во время поиска возникла ошибка", "Поиск объектов", MessageType.Error);
                }
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            // Подготавливаем столбцы.
            this.ColumnHeaders.Clear();
            if (list.Count > 0)
                this.ColumnHeaders.AddRange(list[0].GetParamNames());
            for (int i = 0; i < columnCount; i++)
                if (i < this.ColumnHeaders.Count)
                    this.ColumnVisibilities[i] = true;
                else
                    this.ColumnVisibilities[i] = false;

            this.Result.AddRange(list);

            if (this.Result.Count == 0)
                this.MessageService.ShowMessage("Ничего не найдено", "Поиск", MessageType.Information);

            if (this.RegionSelectionViewModel.HasSelectedCity)
                this.oneCity = true;

            this.SelectAllCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Выполняет выбор всех объектов.
        /// </summary>
        private void ExecuteSelectAll()
        {
            this.ShowOnMapRequested?.Invoke(this, new ShowOnMapRequestedEventArgs(this.Result.ToList()));
        }

        /// <summary>
        /// Загружает параметры заданного типа объекта.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        private void LoadParameters(ObjectType type)
        {
            this.Parameters.Clear();

            SearchingParameterViewModel searchingParameter;

            foreach (var param in this.DataService.ObjectTypes.First(x => x == type).Parameters)
            {
                if (!param.IsVisible || param.IsCommon || !param.IsSearchable)
                    continue;

                searchingParameter = new SearchingParameterViewModel(param, type);
                
                searchingParameter.PropertyChanged += this.SearchingParameter_PropertyChanged;

                if (!type.ParameterBonds.ContainsKey(param))
                    // Если у параметра нет родителя, то добавляем его в саму коллекцию.
                    this.Parameters.Add(searchingParameter);
                else
                    // Иначе, обходим все элементы коллекции в поиске родителя.
                    foreach (var item in this.Parameters)
                        if (item.Id == type.ParameterBonds[param].Id)
                        {
                            item.Children.Add(searchingParameter);

                            break;
                        }
            }

            if (this.Parameters.Count > 0)
                // Если параметры имеются, то выбираем первый из них.
                this.Parameters[0].IsSelected = true;
            else
                // Иначе, убираем выбранный параметр.
                this.SelectedParameter = null;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Отображает на карте выбранную запись результата поиска.
        /// </summary>
        public void ShowSelectedSearchEntry()
        {
            this.ShowOnMapRequested?.Invoke(this, new ShowOnMapRequestedEventArgs(new List<SearchEntryViewModel>()
            {
                this.SelectedSearchEntry
            }));
        }

        #endregion
    }
}