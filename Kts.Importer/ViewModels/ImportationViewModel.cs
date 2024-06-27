using Kts.Excel;
using Kts.Gis.Models;
using Kts.Importer.Data;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления импортирования.
    /// </summary>
    internal sealed class ImportationViewModel : BaseViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Шаблон для заголовка.
        /// </summary>
        private const string titleTemplate = "Импортирование: {0} из {1} - Импортер данных";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Текущий импортируемый объект.
        /// </summary>
        private ImportingObjectViewModel currentObject;

        /// <summary>
        /// Список импортируемых объектов.
        /// </summary>
        private List<ImportingObjectViewModel> objects = new List<ImportingObjectViewModel>();

        /// <summary>
        /// Объекты-родители.
        /// </summary>
        private List<Tuple<string, Guid>> parents;

        /// <summary>
        /// Указатель на текущую позицию в списке импортируемых объектов.
        /// </summary>
        private int pointer;

        /// <summary>
        /// Идентификатор выбранной котельной.
        /// </summary>
        private Guid? selectedBoilerId;

        /// <summary>
        /// Идентификатор выбранного объекта-родителя.
        /// </summary>
        private Guid? selectedParentId;

        /// <summary>
        /// Заголовок.
        /// </summary>
        private string title;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImportationViewModel"/>.
        /// </summary>
        /// <param name="sourcePath">Путь к файлу-источнику данных.</param>
        /// <param name="type">Тип импортируемых объектов.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ImportationViewModel(string sourcePath, ObjectType type, IDataService dataService, IMessageService messageService)
        {
            this.Type = type;
            this.dataService = dataService;
            this.messageService = messageService;

            this.AddToSourceCommand = new RelayCommand(this.ExecuteAddToSource, this.CanExecuteAddToSource);
            this.GoBackCommand = new RelayCommand(this.ExecuteGoBack, this.CanExecuteGoBack);
            this.GoForwardCommand = new RelayCommand(this.ExecuteGoForward, this.CanExecuteGoForward);
            this.SetValuesCommand = new RelayCommand(this.ExecuteSetValues);

            this.LoadData(sourcePath);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления импортируемых объектов в источник данных.
        /// </summary>
        public RelayCommand AddToSourceCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список котельных.
        /// </summary>
        public AdvancedObservableCollection<Tuple<Guid, string>> Boilers
        {
            get;
        } = new AdvancedObservableCollection<Tuple<Guid, string>>();

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли задать котельную.
        /// </summary>
        public bool CanSetBoiler
        {
            get
            {
                return this.Type.TypeId == 1;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли задать населенный пункт.
        /// </summary>
        public bool CanSetCity
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли задать родителя.
        /// </summary>
        public bool CanSetParent
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли задать улицу.
        /// </summary>
        public bool CanSetStreet
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает текущий импортируемый объект.
        /// </summary>
        public ImportingObjectViewModel CurrentObject
        {
            get
            {
                return this.currentObject;
            }
            private set
            {
                this.currentObject = value;

                this.NotifyPropertyChanged(nameof(this.CurrentObject));

                // Меняем заголовок.
                this.Title = string.Format(titleTemplate, this.pointer + 1, this.objects.Count);
            }
        }

        /// <summary>
        /// Возвращает команду выбора предыдущего импортируемого объекта.
        /// </summary>
        public RelayCommand GoBackCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду выбора следующего импортируемого объекта.
        /// </summary>
        public RelayCommand GoForwardCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает объекты-родители.
        /// </summary>
        public List<Tuple<string, Guid>> Parents
        {
            get
            {
                return this.parents;
            }
            private set
            {
                this.parents = value;

                this.NotifyPropertyChanged(nameof(this.Parents));
            }
        }

        /// <summary>
        /// Возвращает или задает идентификатор выбранной котельной.
        /// </summary>
        public Guid? SelectedBoilerId
        {
            get
            {
                return this.selectedBoilerId;
            }
            set
            {
                this.selectedBoilerId = value;

                this.NotifyPropertyChanged(nameof(this.SelectedBoilerId));
            }
        }

        /// <summary>
        /// Возвращает или задает идентификатор выбранного объекта-родителя.
        /// </summary>
        public Guid? SelectedParentId
        {
            get
            {
                return this.selectedParentId;
            }
            set
            {
                this.selectedParentId = value;

                this.NotifyPropertyChanged(nameof(this.SelectedParentId));
            }
        }

        /// <summary>
        /// Возвращает команду задания выбираемых значений импортируемого объекта.
        /// </summary>
        public RelayCommand SetValuesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает модель представления выбора территориальной единицы.
        /// </summary>
        public TerritorialEntitySelectionViewModel TerritorialEntitySelectionViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает заголовок.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            private set
            {
                this.title = value;

                this.NotifyPropertyChanged(nameof(this.Title));
            }
        }

        /// <summary>
        /// Возвращает тип импортируемого объекта.
        /// </summary>
        public ObjectType Type
        {
            get;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Utilities.TerritorialEntitySelectionViewModel.SelectionChanged"/> модели представления выбора территориальной единицы.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TerritorialEntitySelectionViewModel_SelectionChanged(object sender, EventArgs e)
        {
            // Если необходимо, обновляем список объектов-родителей.
            if (this.CanSetParent)
            {
                this.Parents = this.dataService.GetParents(this.TerritorialEntitySelectionViewModel.SelectedCity.Id, this.dataService.GetDefaultSchema(this.TerritorialEntitySelectionViewModel.SelectedCity.Id));

                var first = this.Parents.FirstOrDefault();

                if (first != null)
                    this.SelectedParentId = first.Item2;
                else
                    this.SelectedParentId = null;
            }

            // Если необходимо, обновляем список котельных.
            if (this.CanSetBoiler)
            {
                this.Boilers.Clear();

                // Добавляем пустую котельную.
                this.Boilers.Add(new Tuple<Guid, string>(Guid.Empty, "-"));

                this.Boilers.AddRange(this.dataService.GetBoilers(this.TerritorialEntitySelectionViewModel.SelectedCity.Id, this.dataService.GetDefaultSchema(this.TerritorialEntitySelectionViewModel.SelectedCity.Id)));

                var first = this.Boilers.FirstOrDefault();

                if (first != null)
                    this.SelectedBoilerId = first.Item1;
                else
                    this.SelectedBoilerId = null;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить добавление импортируемых объектов в источник данных.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить добавление импортируемых объектов в источник данных.</returns>
        private bool CanExecuteAddToSource()
        {
            foreach (var obj in this.objects)
                if (!obj.CanBeImported)
                    return false;
                    
            return true;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить выбор предыдущего импортируемого объекта.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить выбор предыдущего импортируемого объекта.</returns>
        private bool CanExecuteGoBack()
        {
            return this.pointer > 0;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить выбор следующего импортируемого объекта.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить выбор следующего импортируемого объекта.</returns>
        private bool CanExecuteGoForward()
        {
            return this.pointer < this.objects.Count - 1;
        }

        /// <summary>
        /// Выполняет добавление импортируемых объектов в источник данных.
        /// </summary>
        private void ExecuteAddToSource()
        {
            foreach (var obj in this.objects)
                obj.SaveToDataSource(this.dataService);

            this.messageService.ShowMessage("Импортирование завершено", "Импортирование данных", MessageType.Information);
        }

        /// <summary>
        /// Выполняет выбор предыдущего импортируемого объекта.
        /// </summary>
        private void ExecuteGoBack()
        {
            this.pointer--;

            this.CurrentObject = this.objects[pointer];

            this.UpdateGoBackForward();
        }

        /// <summary>
        /// Выполняет выбор следующего импортируемого объекта.
        /// </summary>
        private void ExecuteGoForward()
        {
            this.pointer++;

            this.CurrentObject = this.objects[pointer];

            this.UpdateGoBackForward();
        }

        /// <summary>
        /// Выполняет задание выбираемых значений импортируемого объекта.
        /// </summary>
        private void ExecuteSetValues()
        {
            if (this.CanSetCity)
            {
                this.CurrentObject.Region = this.TerritorialEntitySelectionViewModel.SelectedRegion as RegionViewModel;
                this.CurrentObject.City = this.TerritorialEntitySelectionViewModel.SelectedCity as CityViewModel;
            }

            if (this.CanSetStreet)
                this.CurrentObject.Street = this.TerritorialEntitySelectionViewModel.SelectedStreet as StreetViewModel;

            if (this.CanSetParent)
                this.CurrentObject.ParentId = this.SelectedParentId;

            if (this.CanSetBoiler)
                this.CurrentObject.Id = this.SelectedBoilerId;

            // Переходим к следующему объекту, если это возможно.
            if (this.GoForwardCommand.CanExecute(null))
                this.GoForwardCommand.Execute(null);

            this.AddToSourceCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Загружает данные из заданного файла.
        /// </summary>
        /// <param name="fileName">Файл.</param>
        private void LoadData(string fileName)
        {
            var doc = ExcelDocument.Open(fileName, true, false);

            // Получаем первый рабочий лист.
            var worksheet = doc.GetWorksheet(1);

            // Получаем информацию о шаблоне. Она находится в первых трех строках рабочего листа.
            var templateData = worksheet.GetRange(1, 1, 1, 3).Values;
            var startColumn = Convert.ToInt32(templateData[1, 1]);
            var startRow = Convert.ToInt32(templateData[2, 1]);
            var endRow = Convert.ToInt32(templateData[3, 1]);

            int streetParamId = -1;

            // Заполняем данные параметров.
            // Словарь данных параметров. Ключом является индекс строки, а значением - тюпл, состоящий из метки параметра и его идентификатора.
            var parameters = new Dictionary<int, Tuple<string, int>>();
            // Получаем два столбца значений: в первом находятся метки, а во втором - идентификаторы параметров.
            var range = worksheet.GetRange(2, startRow, 3, endRow).Values;
            for (int i = 1; i <= range.Length / 2; i++)
            {
                // Пытаемся получить метку параметра.
                string paramLabel = range[i, 1] != null ? range[i, 1].ToString() : "";

                // Пытаемся получить идентификатор параметра.
                int paramId = -1;
                if (range[i, 2] != null)
                    int.TryParse(range[i, 2].ToString(), out paramId);

                parameters.Add(i, new Tuple<string, int>(paramLabel, paramId));

                if (paramLabel != "")
                    switch (paramLabel)
                    {
                        case "city":
                            this.CanSetCity = true;

                            break;

                        case "parent":
                            this.CanSetParent = true;

                            break;

                        case "street":
                            this.CanSetStreet = true;

                            // Запоминаем идентификатор параметра, представляющего улицу.
                            streetParamId = paramId;

                            break;
                    }
            }

            if (this.CanSetStreet)
                this.TerritorialEntitySelectionViewModel = new TerritorialEntitySelectionViewModel(SelectionDepth.Street, this.dataService);
            else
                if (this.CanSetCity)
                    this.TerritorialEntitySelectionViewModel = new TerritorialEntitySelectionViewModel(SelectionDepth.City, this.dataService);
                else
                    throw new NotSupportedException("Не поддерживается работа с текущей структурой данных");

            this.TerritorialEntitySelectionViewModel.SelectionChanged += this.TerritorialEntitySelectionViewModel_SelectionChanged;

            this.TerritorialEntitySelectionViewModel.Init();

            int curColumn = startColumn;

            string regionName = "";
            string cityName = "";
            string streetName = "";
            string parentName = "";

            while (!string.IsNullOrEmpty(worksheet.GetCell(curColumn, startRow).Value))
            {
                var data = worksheet.GetRange(curColumn, startRow, curColumn, endRow).Values;

                var pars = new Dictionary<ParameterModel, object>();

                foreach (var entry in parameters)
                    switch (entry.Value.Item1)
                    {
                        case "city":
                            cityName = data[entry.Key, 1].ToString();

                            break;

                        case "parent":
                            parentName = data[entry.Key, 1].ToString();

                            break;

                        case "region":
                            regionName = data[entry.Key, 1].ToString();

                            break;

                        case "street":
                            streetName = Convert.ToString(data[entry.Key, 1]);

                            break;

                        default:
                            var paramData = this.Type.Parameters.First(x => x.Id == entry.Value.Item2);

                            var value = data[entry.Key, 1];

                            if (string.IsNullOrEmpty(Convert.ToString(value)) && !paramData.Format.IsGroup)
                                pars.Add(paramData, null);
                            else
                                if (paramData.Format.ClrType == typeof(DateTime))
                                    pars.Add(paramData, DateTime.FromOADate(Convert.ToDouble(value)));
                                else
                                    pars.Add(paramData, paramData.Format.Format(value));

                            break;
                    }

                if (this.CanSetCity)
                    if (this.CanSetStreet)
                        this.objects.Add(new ImportingObjectViewModel(this.Type, new ParameterValueSetModel(pars), regionName, cityName, streetName, streetParamId));
                    else
                        if (this.CanSetParent)
                            this.objects.Add(new ImportingObjectViewModel(this.Type, new ParameterValueSetModel(pars), regionName, cityName, parentName));
                        else
                            this.objects.Add(new ImportingObjectViewModel(this.Type, new ParameterValueSetModel(pars), regionName, cityName));
                else
                    throw new NotSupportedException("Не поддерживается работа с текущей структурой данных");

                curColumn++;
            }

            doc.Close();

            // Добавляем всем объектам значения парамертов по умолчанию.
            foreach (var obj in this.objects)
            {
                obj.ParameterValueSetViewModel.ParameterValueSet.ParameterValues.Add(this.Type.Parameters.First(x => x.Alias == Alias.TypeId), this.Type.TypeId);
                obj.ParameterValueSetViewModel.ParameterValueSet.ParameterValues.Add(this.Type.Parameters.First(x => x.Alias == Alias.IsActive), 1);
                obj.ParameterValueSetViewModel.ParameterValueSet.ParameterValues.Add(this.Type.Parameters.First(x => x.Alias == Alias.IsPlanning), 0);
            }

            this.CurrentObject = this.objects[0];

            this.UpdateGoBackForward();
        }

        /// <summary>
        /// Обновляет возможности выбора предыдущего и следующего импортируемых объектов.
        /// </summary>
        private void UpdateGoBackForward()
        {
            this.GoBackCommand.RaiseCanExecuteChanged();
            this.GoForwardCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}