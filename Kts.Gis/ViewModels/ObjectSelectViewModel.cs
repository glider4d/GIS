using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Gis.Substrates;
using Kts.History;
using Kts.Messaging;
using Kts.Settings;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks; 

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора объекта.
    /// </summary>
    internal sealed partial class ObjectSelectViewModel : ServicedViewModel, ILayerHolder
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что загружены ли данные карты.
        /// </summary>
        private bool isDataLoaded;

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        private ISelectableObjectViewModel selectedObject;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private readonly AccessService accessService;

        /// <summary>
        /// Группа всех объектов.
        /// </summary>
        private readonly GroupViewModel allObjectsGroup;

        /// <summary>
        /// Сервис истории изменений.
        /// </summary>
        private readonly HistoryService historyService;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Группа планируемых объектов.
        /// </summary>
        private readonly GroupViewModel planningObjectsGroup;

        /// <summary>
        /// Сервис подложек.
        /// </summary>
        private readonly SubstrateService substrateService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие запроса выполнения долгодлительной задачи.
        /// </summary>
        public event EventHandler<LongTimeTaskRequestedEventArgs> LongTimeTaskRequested;

        /// <summary>
        /// Событие загрузки карты.
        /// </summary>
        public event EventHandler MapLoaded;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ObjectSelectViewModel"/>.
        /// </summary>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        /// <param name="substrateService">Сервис подложек.</param>
        public ObjectSelectViewModel(AccessService accessService, IDataService dataService, IMapBindingService mapBindingService, IMessageService messageService, ISettingService settingService, SubstrateService substrateService) : base(dataService, messageService)
        {
            this.accessService = accessService;
            this.mapBindingService = mapBindingService;
            this.SettingService = settingService;
            this.substrateService = substrateService;

            this.historyService = new HistoryService();

            this.CloseCommand = new RelayCommand(this.ExecuteClose);
            this.SelectCommand = new RelayCommand(this.ExecuteSelect, this.CanExecuteSelect);

            this.allObjectsGroup = new GroupViewModel("Существующие", true, true, this, this.accessService, dataService, this.historyService, mapBindingService, messageService);
            this.planningObjectsGroup = new GroupViewModel("Планируемые", true, true, this, this.accessService, dataService, this.historyService, mapBindingService, messageService);

            this.NodesGroup = new GroupViewModel("Узлы", true, true, this, this.accessService, dataService, this.historyService, mapBindingService, messageService);

            this.MapViewModel = new MapViewModel(dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Figure || x.ObjectKind == ObjectKind.Line).ToList(), this.allObjectsGroup, null, null, null, null, null, null, null, null, null, this, accessService, this.DataService, this.historyService, this.mapBindingService, messageService, this.SettingService);

            this.ParameterGridViewModel = new ParameterGridViewModel(settingService);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду закрытия представления.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что загружены ли данные.
        /// </summary>
        public bool IsDataLoaded
        {
            get
            {
                return this.isDataLoaded;
            }
            set
            {
                this.isDataLoaded = value;

                this.MapViewModel.IsReady = value;

                this.NotifyPropertyChanged(nameof(this.IsDataLoaded));
            }
        }

        /// <summary>
        /// Возвращает модель представления карты.
        /// </summary>
        public MapViewModel MapViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления сетки параметров.
        /// </summary>
        public ParameterGridViewModel ParameterGridViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает результат выбора.
        /// </summary>
        public IObjectViewModel Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду выбора.
        /// </summary>
        public RelayCommand SelectCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис настроек.
        /// </summary>
        public ISettingService SettingService
        {
            get;
        }

        #endregion
        
        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить выбор объекта.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteSelect()
        {
            return this.SelectedObject != null;
        }

        /// <summary>
        /// Выполняет закрытие представления.
        /// </summary>
        private void ExecuteClose()
        {
            // Уведомляем о том, что нужно закрыть представление.
            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет выбор объекта.
        /// </summary>
        private void ExecuteSelect()
        {
            this.Result = this.SelectedObject as IObjectViewModel;

            if (this.CloseCommand.CanExecute(null))
                this.CloseCommand.Execute(null);
        }

        /// <summary>
        /// Загружает фигуры.
        /// </summary>
        /// <param name="figures">Фигуры.</param>
        private void LoadFigures(List<FigureModel> figures)
        {
            var allFigures = new Dictionary<ObjectType, List<IObjectViewModel>>();
            var notPlacedFigures = new Dictionary<ObjectType, List<IObjectViewModel>>();
            var planningFigures = new Dictionary<ObjectType, List<IObjectViewModel>>();

            FigureViewModel figureViewModel = null;

            foreach (var figure in figures)
            {
                switch (figure.FigureType)
                {
                    case FigureType.Ellipse:
                        figureViewModel = new EllipseViewModel(figure, this, this.accessService, this.DataService, this.historyService, this.mapBindingService, this.MessageService);

                        break;

                    case FigureType.Polygon:
                        figureViewModel = new PolygonViewModel(figure, this, this.accessService, this.DataService, this.historyService, this.mapBindingService, this.MessageService);

                        break;

                    case FigureType.Rectangle:
                        figureViewModel = new RectangleViewModel(figure, this, this.accessService, this.DataService, this.historyService, this.mapBindingService, this.MessageService);

                        break;

                    case FigureType.None:
                        // Пусть в том случае, когда объект является неразмещенным, ону будет представляться прямоугольником.
                        figureViewModel = new RectangleViewModel(figure, this, this.accessService, this.DataService, this.historyService, this.mapBindingService, this.MessageService);

                        break;
                }

                // Добавляем объект в соответствующий ему слой.
                if (figure.FigureType != FigureType.None)
                {
                    figureViewModel.IsInitialized = true;
                    figureViewModel.IsPlaced = true;

                    if (!figure.IsPlanning)
                    {
                        if (!allFigures.ContainsKey(figure.Type))
                            allFigures.Add(figure.Type, new List<IObjectViewModel>());

                        allFigures[figure.Type].Add(figureViewModel);
                    }
                    else
                    {
                        if (!planningFigures.ContainsKey(figure.Type))
                            planningFigures.Add(figure.Type, new List<IObjectViewModel>());

                        planningFigures[figure.Type].Add(figureViewModel);
                    }
                }
                else
                {
                    if (!notPlacedFigures.ContainsKey(figure.Type))
                        notPlacedFigures.Add(figure.Type, new List<IObjectViewModel>());

                    notPlacedFigures[figure.Type].Add(figureViewModel);
                }

                figureViewModel.IsModified = false;
            }

            // Добавляем их в слои.
            foreach (var entry in allFigures)
                this.allObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            //this.allObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
            foreach (var entry in planningFigures)
                this.planningObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            //this.planningObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
        }

        /// <summary>
        /// Загружает линии.
        /// </summary>
        /// <param name="lines">Линии.</param>
        private void LoadLines(List<LineModel> lines)
        {
            var allLines = new Dictionary<ObjectType, List<IObjectViewModel>>();
            var planningLines = new Dictionary<ObjectType, List<IObjectViewModel>>();

            LineViewModel lineViewModel = null;

            foreach (var line in lines)
            {
                lineViewModel = new LineViewModel(line, this, this.MapViewModel, this.accessService, this.DataService, this.historyService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true,
                    IsPlaced = true,
                };

                if (!line.IsPlanning)
                {
                    if (!allLines.ContainsKey(line.Type))
                        allLines.Add(line.Type, new List<IObjectViewModel>());

                    allLines[line.Type].Add(lineViewModel);
                }
                else
                {
                    if (!planningLines.ContainsKey(line.Type))
                        planningLines.Add(line.Type, new List<IObjectViewModel>());

                    planningLines[line.Type].Add(lineViewModel);
                }

                lineViewModel.IsModified = false;
            }

            // Добавляем их в слои.
            foreach (var entry in allLines)
                this.allObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            //this.allObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
            foreach (var entry in planningLines)
                this.planningObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            //this.planningObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);

            // Восстанавливаем их группировку.
            foreach (var entry in allLines)
                foreach (LineViewModel line in entry.Value)
                    line.RestoreGrouping();
            foreach (var entry in planningLines)
                foreach (LineViewModel line in entry.Value)
                    line.RestoreGrouping();
        }

        /// <summary>
        /// Загружает узлы.
        /// </summary>
        /// <param name="nodes">Узлы.</param>
        private void LoadNodes(List<NodeModel> nodes)
        {
            var allNodes = new Dictionary<ObjectType, List<IObjectViewModel>>();

            NodeViewModel nodeViewModel = null;

            foreach (var node in nodes)
            {
                nodeViewModel = new NodeViewModel(node, this, this.accessService, this.DataService, this.historyService, this.mapBindingService, this.MessageService)
                {
                    IsModified = false
                };

                // Восстанавливаем объект, к которому присоединен узел.
                nodeViewModel.RestoreConnectedObject();

                // Восстанавливаем соединения с узлом.
                nodeViewModel.RestoreConnections(true);

                if (nodeViewModel.ConnectionCount > 0)
                {
                    nodeViewModel.NotifyViewConnectionsChanged();

                    nodeViewModel.IsInitialized = true;

                    if (!allNodes.ContainsKey(node.Type))
                        allNodes.Add(node.Type, new List<IObjectViewModel>());

                    allNodes[node.Type].Add(nodeViewModel);

                    nodeViewModel.IsPlaced = true;
                }
            }

            // Добавляем их в слои.
            foreach (var entry in allNodes)
            {
                this.NodesGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
                //this.NodesGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
            }
                
        }

        /// <summary>
        /// Загружает подложку карты.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Значение, указывающее на то, что загружена ли подложка.</returns>
        private bool LoadSubstrate(CityViewModel city)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                this.MapViewModel.SubstrateTiles = null;
                this.MapViewModel.SubstrateDimension = System.Windows.Size.Empty;
                this.MapViewModel.SubstrateSize = new System.Windows.Size(0, 0);
            }));

            // Проверяем актуальность подложки.
            var substrate = city.GetSubstrate(this.DataService);
            if (substrate != null)
            {
                // Если в источнике данных есть данные подложки текущего населенного пункта...
                if (this.substrateService.IsSubstrateNewer(substrate))
                {
                    // ...и эти данные новее кешированной версии, то удаляем кешированные файлы-изображения.
                    city.DeleteCachedImages(this.substrateService);

                    // Удаляем предыдущие данные и добавляем новые.
                    city.RemoveSubstrate(this.substrateService);
                    city.UpdateSubstrate(substrate, this.substrateService);

                    // Если нужно, кешируем изображения.
                    if (substrate.HasImageSource)
                        if (!this.substrateService.CacheImages(substrate, this.DataService.SubstrateFolderName))
                        {

                            System.Windows.MessageBox.Show(this.DataService.SubstrateFolderName);
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => this.MessageService.ShowMessage("Не удалось выполнить кеширование подложки", "Загрузка подложки", MessageType.Error)));

                            return false;
                        }
                }
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => this.MessageService.ShowMessage("Для загружаемого населенного пункта не задана подложка", "Загрузка подложки", MessageType.Error)));

                return false;
            }

            var substrateSize = city.GetSubstrateSize(this.substrateService);

            if (city.HasImageSource(this.substrateService))
            {
                var cachedImageFileNames = city.GetCachedImageFileNames(this.substrateService);
                var substrateDimension = city.GetSubstrateDimension(this.substrateService);
                var thumbnailPath = city.GetThumbnailPath(this.DataService);

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this.MapViewModel.SubstrateTiles = cachedImageFileNames;
                    this.MapViewModel.SubstrateDimension = substrateDimension;
                    this.MapViewModel.ThumbnailPath = thumbnailPath;

                    this.MapViewModel.SubstrateSize = substrateSize;
                }));
            }
            else
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this.MapViewModel.ThumbnailPath = null;

                    this.MapViewModel.SubstrateSize = substrateSize;
                }));

            this.MapViewModel.SubstrateSize = substrateSize;

            return true;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Загружает данные заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>true - если загрузка выполнена успешно, иначе - false.</returns>
        public bool Load(CityViewModel city)
        {
            // Указываем на то, что данные еще не загружены.
            this.IsDataLoaded = false;

            // Получаем актуальную схему.
            this.CurrentSchema = this.DataService.Schemas.First(x => x.IsActual);
            if (this.CurrentSchema == null)
            {
                this.MessageService.ShowMessage("Не удалось получить актуальную схему", "Загрузка данных", MessageType.Error);

                return false;
            }

            this.CurrentCity = city;

            double scale = 0;

            var figures = new List<FigureModel>();
            var lines = new List<LineModel>();
            var nodes = new List<NodeModel>();

            DataSet badges = new DataSet();

            // Пробуем загрузить все данные.
            var waitViewModel = new WaitViewModel("Загрузка данных", "Пожалуйста подождите, идет загрузка данных...", async () =>
            {
                return await Task.Factory.StartNew(() =>
                {
                    // Подготавливаем схему к работе.
                    this.DataService.PrepareSchema(this.CurrentSchema.Id, city.Id);

                    // Обновляем справочники.
                    this.DataService.UpdateTables(city.Id, null, this.CurrentSchema, LoadLevel.Always);
                    this.DataService.UpdateTables(city.Id, null, this.CurrentSchema, LoadLevel.AfterChange);

                    // Теперь каждый раз необходимо перезагружать данные о параметрах, отвечающих за составление названий объектов.
                    this.DataService.UpdateTypeCaptionParams(city.Id);

                    // Пробуем загрузить подложку.
                    if (!this.LoadSubstrate(city))
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => this.MessageService.ShowMessage("Не удалось установить подложку. Дальнейшая загрузка невозможна.", "Загрузка", MessageType.Error)));

                        return false;
                    }

                    // Загружаем масштаб линий из источника данных.
                    scale = city.GetScale(this.DataService);

                    // Обновляем настройки вида карты.
                    this.mapBindingService.MapSettingService.Load(city.Id);

                    // Загружаем данные из источника данных.
                    figures = city.GetFigures(this.CurrentSchema, this.DataService);
                    lines = city.GetLines(this.CurrentSchema, this.DataService);
                    nodes = city.GetNodes(this.CurrentSchema, this.DataService);

                    // Получаем все значки данного населенного пункта, чтобы потом передать их линиям.
                    badges.Tables.Add(city.GetBadgesRaw(this.CurrentSchema, this.DataService));

                    return true;
                });
            });
            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
            if (!waitViewModel.Result)
                return false;

            // Задаем масштаб линий.
            this.MapViewModel.Scale = scale;

            // Добавляем объекты на карту.
            this.LoadFigures(figures);
            this.LoadLines(lines);
            this.LoadNodes(nodes);

            // Добавляем значки линий.
            var lineTypes = this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Line);
            foreach (var layer in this.allObjectsGroup.Layers.Where(x => lineTypes.Contains(x.Type)))
                foreach (LineViewModel line in layer.Objects)
                    line.LoadBadges(badges);
            foreach (var layer in this.planningObjectsGroup.Layers.Where(x => lineTypes.Contains(x.Type)))
                foreach (LineViewModel line in layer.Objects)
                    line.LoadBadges(badges);

            this.IsDataLoaded = true;

            this.MapLoaded?.Invoke(this, EventArgs.Empty);

            return true;
        }

        #endregion
    }

    // Реализация ILayerHolder.
    internal sealed partial class ObjectSelectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает команду копирования объекта.
        /// </summary>
        public RelayCommand CopyCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает текущий населенный пункт.
        /// </summary>
        public CityViewModel CurrentCity
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает текущую схему.
        /// </summary>
        public SchemaModel CurrentSchema
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду удаления объекта.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает редактируемый объект.
        /// </summary>
        public IEditableObjectViewModel EditingObject
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду полного удаления объекта.
        /// </summary>
        public RelayCommand FullDeleteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду управления слоем кастомного объекта.
        /// </summary>
        public RelayCommand ManageCustomObjectLayerCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает группу узлов.
        /// </summary>
        public GroupViewModel NodesGroup
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранную группу.
        /// </summary>
        public ISelectableObjectViewModel SelectedGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный слой.
        /// </summary>
        public ISelectableObjectViewModel SelectedLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный неразмещенный объект.
        /// </summary>
        public ISelectableObjectViewModel SelectedNotPlacedObject
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        public ISelectableObjectViewModel SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                if (this.SelectedObject != value)
                {
                    if (this.SelectedObject != null)
                    {
                        this.SelectedObject.IsSelected = false;

                        this.ParameterGridViewModel.UnloadData();
                    }

                    this.selectedObject = value;

                    this.NotifyPropertyChanged(nameof(this.SelectedObject));

                    var paramObj = value as IParameterizedObjectViewModel;

                    if (paramObj != null)
                        this.ParameterGridViewModel.LoadObjectMainData(paramObj);

                    this.SelectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Создает объект и добавляет его на карту и соответствующий ему слой.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public void AddObject(IObjectViewModel obj)
        {
            var mapObject = obj as IMapObjectViewModel;

            mapObject.RegisterBinding();

            obj.IsInitialized = true;

            mapObject.IsPlaced = true;

            if (obj as NodeViewModel != null)
                this.NodesGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
            else
                if (!obj.IsPlanning)
                    this.allObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
                else
                    this.planningObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
        }

        /// <summary>
        /// Очищает группу выбранных объектов.
        /// </summary>
        public void ClearSelectedGroup()
        {
            // Ничего не делаем.
        }
        
        /// <summary>
        /// Возвращает или задает слой объектов.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="layerType">Тип слоя объектов.</param>
        /// <returns>Слой объектов.</returns>
        public LayerViewModel GetLayer(ObjectType type, LayerType layerType)
        {
            if (layerType == LayerType.Planning)
                return this.planningObjectsGroup.Layers.First(x => x.Type == type);

            return this.allObjectsGroup.Layers.First(x => x.Type == type);
        }

        /// <summary>
        /// Возвращает слои по виду объектов.
        /// </summary>
        /// <param name="kind">Вид объектов.</param>
        /// <returns>Слои.</returns>
        public List<LayerViewModel> GetLayers(ObjectKind kind)
        {
            var result = new List<LayerViewModel>();

            result.AddRange(this.allObjectsGroup.Layers.Where(x => x.Type.ObjectKind == kind));
            result.AddRange(this.planningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == kind));

            return result;
        }

        /// <summary>
        /// Возвращает объект по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект.</returns>
        public IObjectViewModel GetObject(Guid id)
        {
            foreach (var layer in this.allObjectsGroup.Layers)
            {
                var result = layer.Objects.FirstOrDefault(x => x.Id == id);

                if (result != null)
                    return result;
            }

            foreach (var layer in this.planningObjectsGroup.Layers)
            {
                var result = layer.Objects.FirstOrDefault(x => x.Id == id);

                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Возвращает выбранные объекты верхнего уровня.
        /// </summary>
        /// <returns>Список объектов.</returns>
        public List<IObjectViewModel> GetSelectedObjects()
        {
            return new List<IObjectViewModel>();
        }

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, подлежащий удалению.</param>
        public void MarkToDelete(IDeletableObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Помечает объект на раннее обновление в источнике данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно обновить раньше остальных.</param>
        public void MarkToUpdate(IEditableObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает объект с карты и с соответствующего ему слоя.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public void RemoveObject(IObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает выбранные объекты.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public void SetSelectedObjects(List<IObjectViewModel> objects)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает пометку с объекта, подлежащего к удалению из источника данных.
        /// </summary>
        /// <param name="obj">Объект, подлежащий удалению.</param>
        public void UnmarkToDelete(IDeletableObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает отметку с объекта, подлежащего к раннему обновлению в источнике данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно обновить раньше остальных.</param>
        public void UnmarkToUpdate(IEditableObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        #endregion
    }
}