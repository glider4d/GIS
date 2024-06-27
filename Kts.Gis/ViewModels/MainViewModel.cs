using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Reports;
using Kts.Gis.Reports.ViewModels;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.Gis.Substrates;
using Kts.History;

using Kts.Messaging;
using Kts.Settings;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет главную модель представления.
    /// </summary>
    [Serializable]
    internal sealed partial class MainViewModel : ServicedViewModel, ILayerHolder
    {
        #region Закрытые поля

        /// <summary>
        /// Копируемый объект.
        /// </summary>
        private ICopyableObjectViewModel copyingObject;

        /// <summary>
        /// Рисуемый объект.
        /// </summary>
        private IMapObjectViewModel drawingObject;

        /// <summary>
        /// Редактируемый объект.
        /// </summary>
        private IEditableObjectViewModel editingObject;

        /// <summary>
        /// Редактируемые объекты.
        /// </summary>
        private List<IMapObjectViewModel> editingObjects;

        /// <summary>
        /// Положение точки области редактирования группы объектов.
        /// </summary>
        private System.Windows.Point groupAreaOriginPoint;

        /// <summary>
        /// Положение области редактирования группы объектов.
        /// </summary>
        private System.Windows.Point groupAreaPosition;

        /// <summary>
        /// Размер области редактирования группы объектов.
        /// </summary>
        private System.Windows.Size groupAreaSize;

        /// <summary>
        /// Значение, указывающее на то, что начато ли групповое редактирование объектов.
        /// </summary>
        private bool groupEditStarted;

        /// <summary>
        /// Значение, указывающее на то, что имеется ли выбранный объект.
        /// </summary>
        private bool hasSelectedObject;

        /// <summary>
        /// Значение, указывающее на то, что загружена ли схема только выборочных котельных.
        /// </summary>
        private bool isCustomBoilersLoaded;

        /// <summary>
        /// Значение, указывающее на то, что загружены ли данные карты.
        /// </summary>
        private bool isDataLoaded;

        /// <summary>
        /// Название загруженного населенного пункта.
        /// </summary>
        private string loadedCityName;

        /// <summary>
        /// Время загрузки данных.
        /// </summary>
        private double loadTime;

        /// <summary>
        /// Вставляемый объект.
        /// </summary>
        private ICopyableObjectViewModel pastingObject;

        /// <summary>
        /// Инструмент, выбранный до блокировки панели инструментов.
        /// </summary>
        private Tool prevTool = Tool.Selector;

        /// <summary>
        /// Выбранная группа.
        /// </summary>
        private ISelectableObjectViewModel selectedGroup;

        /// <summary>
        /// Выбранный слой.
        /// </summary>
        private ISelectableObjectViewModel selectedLayer;

        /// <summary>
        /// Выбранный неразмещенный объект.
        /// </summary>
        private ISelectableObjectViewModel selectedNotPlacedObject;

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        private ISelectableObjectViewModel selectedObject;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Граничные линии.
        /// </summary>
        private readonly List<LineViewModel> boundaryLines = new List<LineViewModel>();

        /// <summary>
        /// Объекты, используемые для отображения очертаний на карте.
        /// </summary>
        private readonly List<IMapObjectViewModel> outlineObjects = new List<IMapObjectViewModel>();

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        [NonSerialized]
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Сервис отчетов.
        /// </summary>
        [NonSerialized]
        private readonly ReportService reportService;

        /// <summary>
        /// Группа выбранных объектов.
        /// </summary>
        private readonly GroupViewModel selectedObjectsGroup;

        /// <summary>
        /// Сервис подложек.
        /// </summary>
        [NonSerialized]
        private readonly SubstrateService substrateService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса отображения представления узлов поворота.
        /// </summary>
        public event EventHandler BendNodesRequested;

        /// <summary>
        /// Событие запроса изменения отображения длины.
        /// </summary>
        public event EventHandler<ChangeLengthViewRequestedEventArgs> ChangeLengthViewRequested;

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие запроса отображения представления документов.
        /// </summary>
        public event EventHandler<ViewRequestedEventArgs<DocumentsViewModel>> DocumentViewRequested;

        /// <summary>
        /// Событие уведомления о нахождении ошибок.
        /// </summary>
        public event EventHandler ErrorFound;

        /// <summary>
        /// Событие запроса экспорта данных.
        /// </summary>
        public event EventHandler<ImportExportRequestedEventArgs> ExportRequested;

        /// <summary>
        /// Событие запроса поиска объекта.
        /// </summary>
        public event EventHandler FindRequested;

        /// <summary>
        /// Событие запроса отображения представления неподключенных узлов.
        /// </summary>
        public event EventHandler FreeNodesRequested;

        /// <summary>
        /// Событие запроса скрытия панели инструментов группового редактирования.
        /// </summary>
        public event EventHandler GroupAreaToolBarCloseRequested;

        /// <summary>
        /// Событие запроса отображения панели инструментов группового редактирования.
        /// </summary>
        public event EventHandler GroupAreaToolBarRequested;

        /// <summary>
        /// Событие запроса импорта данных.
        /// </summary>
        public event EventHandler<ImportExportRequestedEventArgs> ImportRequested;

        /// <summary>
        /// Событие запроса отображения представления списка сопоставленных объектов.
        /// </summary>
        public event EventHandler JurCompletedListRequested;

        /// <summary>
        /// Событие запроса отображения списка несопоставленных объектов с базовой программой КТС.
        /// </summary>
        public event EventHandler<ViewRequestedEventArgs<KtsLeftoversViewModel>> KtsLeftViewRequested;

        /// <summary>
        /// Событие запроса настройки слоев.
        /// </summary>
        public event EventHandler LayersSettingsRequested;

        /// <summary>
        /// Событие запроса выполнения долгодлительной задачи.
        /// </summary>
        public event EventHandler<LongTimeTaskRequestedEventArgs> LongTimeTaskRequested;

        /// <summary>
        /// Событие управления слоем кастомного объекта.
        /// </summary>
        public event EventHandler<ManageCustomObjectLayerEventArgs> ManageCustomObjectLayer;

        /// <summary>
        /// Событие закрытия карты.
        /// </summary>
        public event EventHandler MapClosed;

        /// <summary>
        /// Событие загрузки карты.
        /// </summary>
        public event EventHandler MapLoaded;

        /// <summary>
        /// Событие запроса отображения представления списка объектов.
        /// </summary>
        public event EventHandler ObjectListRequested;

        /// <summary>
        /// Событие запроса вставки объекта на карту.
        /// </summary>
        public event EventHandler<PasteRequestedEventArgs> PasteRequested;

        /// <summary>
        /// Событие запроса скрытия панели инструментов области печати.
        /// </summary>
        public event EventHandler PrintToolBarCloseRequested;

        /// <summary>
        /// Событие запроса отображения панели инструментов области печати.
        /// </summary>
        public event EventHandler PrintToolBarRequested;

        /// <summary>
        /// Событие запроса сохранения изображения.
        /// </summary>
        public event EventHandler SaveAsImageRequested;

        /// <summary>
        /// Событие уведомления о нахождении ошибок в сохраненных значениях параметров.
        /// </summary>
        public event EventHandler SavedErrorFound;

        /// <summary>
        /// Событие запроса схемы населенного пункта.
        /// </summary>
        public event EventHandler<SchemaRequestedEventArgs> SchemaRequested;

        /// <summary>
        /// Событие запроса отображения локальной карты.
        /// </summary>
        public event EventHandler ShowLocalMapRequested;

        /// <summary>
        /// Событие запроса ввода значения.
        /// </summary>
        public event EventHandler<ValueInputRequestedEventArgs> ValueInputRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="loginName">Название логина.</param>
        /// <param name="roleName">Название пользовательской роли.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="reportService">Сервис отчетов.</param>
        /// <param name="settingService">Сервис настроек.</param>
        /// <param name="substrateService">Сервис подложек.</param>
        public MainViewModel(string loginName, string roleName, AccessService accessService, IDataService dataService, IMapBindingService mapBindingService, IMessageService messageService, ReportService reportService, ISettingService settingService, SubstrateService substrateService) : base(dataService, messageService)
        {
            this.AccessService = accessService;
            this.mapBindingService = mapBindingService;
            this.reportService = reportService;
            this.SettingService = settingService;
            this.substrateService = substrateService;

            this.AttachNodesCommand = new RelayCommand(this.ExecuteAttachNodes, this.CanExecuteAttachNodes);
            this.CalculateHydraulicsCommand = new RelayCommand(this.ExecuteCalculateHydraulics);
            this.CheckErrorsCommand = new RelayCommand(this.ExecuteCheckErrors, this.CanExecuteCheckErrors);
            this.CheckFuelCommand = new RelayCommand(this.ExecuteCheckFuel, this.CanExecuteCheckFuel);
            this.CheckJurCommand = new RelayCommand(this.ExecuteCheckJur, this.CanExecuteCheckJur);
            this.CheckJurCompletedCommand = new RelayCommand(this.ExecuteCheckJurCompleted, this.CanExecuteCheckJurCompleted);
            this.CheckJurLeftCommand = new RelayCommand(this.ExecuteCheckJurLeft, this.CanExecuteCheckJurLeft);
            this.CheckKvpCommand = new RelayCommand(this.ExecuteCheckKvp, this.CanExecuteCheckKvp);
            this.CheckKvpCompletedCommand = new RelayCommand(this.ExecuteCheckKvpCompleted, this.CanExecuteCheckKvpCompleted);
            this.CheckKvpLeftCommand = new RelayCommand(this.ExecuteCheckKvpLeft, this.CanExecuteCheckKvpLeft);
            this.CheckSavedErrorsCommand = new RelayCommand(this.ExecuteCheckSavedErrors, this.CanExecuteCheckSavedErrors);
            this.ClearImageCacheCommand = new RelayCommand(this.ExecuteClearImageCache, this.CanExecuteClearImageCache);
            this.ClearSettingsCommand = new RelayCommand(this.ExecuteClearSettings);
            this.CloseCommand = new RelayCommand(this.ExecuteClose);
            this.CopyCommand = new RelayCommand(this.ExecuteCopy, this.CanExecuteCopy);
            this.DeleteCommand = new RelayCommand(this.ExecuteDelete, this.CanExecuteDelete);
            this.ExportCommand = new RelayCommand(this.ExecuteExport, this.CanExecuteExport);
            this.FindBendNodesCommand = new RelayCommand(this.ExecuteFindBendNodes, this.CanExecuteFindBendNodes);
            this.FindFreeNodesCommand = new RelayCommand(this.ExecuteFindFreeNodes, this.CanExecuteFindFreeNodes);
            this.FindCommand = new RelayCommand(this.ExecuteFind, this.CanExecuteFind);
            this.FullDeleteCommand = new RelayCommand(this.ExecuteFullDelete);
            this.GenerateDiffObjectsCommand = new RelayCommand(this.ExecuteGenerateDiffObjects);
            this.GenerateDiffObjects2Command = new RelayCommand(this.ExecuteGenerateDiffObjects2);
            this.GenerateIntegrationStatsCommand = new RelayCommand(this.ExecuteGenerateIntegrationStats);
            this.GenerateIntegrationStats2Command = new RelayCommand(this.ExecuteGenerateIntegrationStats2);
            this.GroupLinesCommand = new RelayCommand(this.ExecuteGroupLines);
            this.HydraulicsToExcelCommand = new RelayCommand(this.ExecuteHydraulicsToExcel);
            this.ImportCommand = new RelayCommand(this.ExecuteImport, this.CanExecuteImport);
            this.LayersSettingsCommand = new RelayCommand(this.ExecuteLayersSettings);
            this.LoadCommand = new RelayCommand(this.ExecuteLoad);
            this.ManageCustomObjectLayerCommand = new RelayCommand(this.ExecuteManageCustomObjectLayer);
            this.PasteCommand = new RelayCommand(this.ExecutePaste, this.CanExecutePaste);
            this.ReattachNodesCommand = new RelayCommand(this.ExecuteReattachNodes, this.CanExecuteReattachNodes);



            this.SaveAsImageCommand = new RelayCommand(this.ExecuteSaveAsImage, this.CanExecuteSaveAsImage);
            this.SaveCommand = new RelayCommand(this.ExecuteSave, this.CanExecuteSave);
            this.SelectBrokenLines = new RelayCommand(this.ExecuteSelectBrokenLines, this.CanExecuteSelectBrokenLines);
            this.TestCommand = new RelayCommand(this.ExecuteTest, this.CanExecuteTest);
            this.UngroupLinesCommand = new RelayCommand(this.ExecuteUngroupLines);

            this.HistoryService = new HistoryService();

            // Отслеживаем изменения истории изменений.
            this.HistoryService.HistoryChanged += this.HistoryService_HistoryChanged;

            this.AllObjectsGroup = new GroupViewModel("Существующие", true, true, this, accessService, dataService, this.HistoryService, mapBindingService, messageService);
            this.NotPlacedObjectsGroup = new GroupViewModel("Неразмещенные", false, false, this, accessService, dataService, this.HistoryService, mapBindingService, messageService);
            this.PlanningObjectsGroup = new GroupViewModel("Планируемые", true, true, this, accessService, dataService, this.HistoryService, mapBindingService, messageService);
            this.selectedObjectsGroup = new GroupViewModel("Выбранные", true, false, this, accessService, dataService, this.HistoryService, mapBindingService, messageService);
            this.DisabledObjectsGroup = new GroupViewModel("Отключенные", true, true, this, accessService, dataService, this.HistoryService, mapBindingService, messageService);

            this.NodesGroup = new GroupViewModel("Узлы", true, true, this, accessService, dataService, this.HistoryService, mapBindingService, messageService);

            this.Groups.Add(this.AllObjectsGroup);
            this.Groups.Add(this.PlanningObjectsGroup);
            this.Groups.Add(this.DisabledObjectsGroup);
            this.Groups.Add(this.selectedObjectsGroup);

            this.CitySelectionViewModel = new CitySelectionViewModel(accessService, dataService, settingService);
            this.CitySelectionViewModel.Init();

            this.ToolBarViewModel = new ToolBarViewModel(dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Figure).ToList(), LineTypeViewModel.GetLineTypes(dataService), this, accessService, this.GroupLinesCommand, this.UngroupLinesCommand, messageService);

            this.ToolBarViewModel.DecreaseFontRequested += this.ToolBarViewModel_DecreaseFontRequested;
            this.ToolBarViewModel.IncreaseFontRequested += this.ToolBarViewModel_IncreaseFontRequested;
            this.ToolBarViewModel.PropertyChanged += this.ToolBarViewModel_PropertyChanged;
            this.ToolBarViewModel.ResetLabelsRequested += this.ToolBarViewModel_ResetLabelsRequested;
            this.ToolBarViewModel.StartGroupEdit += this.ToolBarViewModel_StartGroupEdit;
            this.ToolBarViewModel.StopGroupEdit += this.ToolBarViewModel_StopGroupEdit;

            this.Filters.Add(new FilterViewModel(this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure).ToList(), "Фильтр фигур"));
            this.Filters.Add(new FilterViewModel(this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line).ToList(), "Фильтр линий"));

            this.Hydraulics = new HydraulicsViewModel(mapBindingService);
            this.HydraulicsError = new HydraulicsErrorViewModel(mapBindingService);
            this.YearDiff = new YearDiffViewModel(mapBindingService, settingService);
            this.RP = new RPViewModel(mapBindingService, settingService);
            this.UO = new UOViewModel(mapBindingService, settingService);
            this.disableObjectViewModel = new DisableObjectViewModel(mapBindingService, settingService);


            this.IJS = new IJSViewModel(mapBindingService, settingService, "#FF0000FF");
            this.IJSF = new IJSViewModel(mapBindingService, settingService, "#FF00FF00");


            this.MapViewModel = new MapViewModel(dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Figure || x.ObjectKind == ObjectKind.Line).ToList(), this.AllObjectsGroup, this.DisabledObjectsGroup, this.Hydraulics, this.HydraulicsError, this.YearDiff, this.RP, this.UO, this.disableObjectViewModel, this.IJS, this.IJSF, this, accessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService, this.SettingService);
            this.MapViewModel.PropertyChanged += this.MapViewModel_PropertyChanged;
            this.MapViewModel.NeedReopen += this.MapViewModel_NeedReopen;
            this.MapViewModel.NeedSilentSave += this.MapViewModel_NeedSilentSave;

            this.Reports = reportService.GetReportTree();

            this.ParameterHistoryViewModel = new ParameterHistoryViewModel(this, dataService);

            this.ServerName = dataService.ServerName;
            this.LoginName = loginName;
            this.RoleName = roleName;

            this.mapBindingService.ScaleChanged += this.MapBindingService_ScaleChanged;

            this.BoilerInfo = new BoilerInfoViewModel(dataService);
            this.BuildingInfo = new BuildingInfoViewModel(dataService);
            this.StorageInfo = new StorageInfoViewModel(dataService);

            this.BendNodesViewModel = new BendNodesViewModel(this, this.MessageService);

            this.PropertyChanged += this.MainViewModel_PropertyChanged;

            this.ParameterGridViewModel = new ParameterGridViewModel(settingService);

            this.FastSearch = new FastSearchViewModel(this);

            this.CustomLayersViewModel = new CustomLayersViewModel(this, accessService, dataService, this.HistoryService, messageService);

            this.CustomLayersViewModel.VisibilityChanged += this.CustomLayersViewModel_VisibilityChanged;

            this.ObjectListViewModel = new ObjectListViewModel(this, dataService);
            this.JurKvpCompletedListViewModel = new JurKvpCompletedListViewModel(this, dataService);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает сервис доступа к функциям приложения.
        /// </summary>
        public AccessService AccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает группу всех объектов.
        /// </summary>
        public GroupViewModel AllObjectsGroup
        {
            get;
        }

        /// <summary>
        /// Возвращает команду прикрепления узлов к границам фигур.
        /// </summary>
        public RelayCommand AttachNodesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления узлов поворота.
        /// </summary>
        public BendNodesViewModel BendNodesViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает информацию о котельной.
        /// </summary>
        public BoilerInfoViewModel BoilerInfo
        {
            get;
        }


        public BuildingInfoViewModel BuildingInfo
        {
            get;
        }

        /// <summary>
        /// Возвращает команду расчета гидравлики.
        /// </summary>
        public RelayCommand CalculateHydraulicsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнять отладку приложения.
        /// </summary>
        public bool CanDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Возвращает команду выполнения проверки наличия ошибок.
        /// </summary>
        public RelayCommand CheckErrorsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду проверки интеграции с программой "Учет потребления топлива".
        /// </summary>
        public RelayCommand CheckFuelCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду проверки интеграции с программой "Расчеты с юридическими лицами".
        /// </summary>
        public RelayCommand CheckJurCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения сопоставленных объектов с программой "Расчеты с юридическими лицами".
        /// </summary>
        public RelayCommand CheckJurCompletedCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения несопоставленных объектов с программой "Расчеты с юридическими лицами".
        /// </summary>
        public RelayCommand CheckJurLeftCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду проверки интеграции с программой "Квартплата".
        /// </summary>
        public RelayCommand CheckKvpCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения сопоставленных объектов с программой "Квартплата".
        /// </summary>
        public RelayCommand CheckKvpCompletedCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения несопоставленных объектов с программой "Квартплата".
        /// </summary>
        public RelayCommand CheckKvpLeftCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду выполнения проверки наличия ошибок в сохраненных значениях параметров объектов.
        /// </summary>
        public RelayCommand CheckSavedErrorsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления выбора населенного пункта.
        /// </summary>
        public CitySelectionViewModel CitySelectionViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает команду очистки кэша изображений.
        /// </summary>
        public RelayCommand ClearImageCacheCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду удаления сохраненных настроек.
        /// </summary>
        public RelayCommand ClearSettingsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду закрытия представления.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления кастомных слоев.
        /// </summary>
        public CustomLayersViewModel CustomLayersViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает список пользовательских объектов.
        /// </summary>
        public AdvancedObservableCollection<ICustomLayerObject> CustomObjects
        {
            get;
        } = new AdvancedObservableCollection<ICustomLayerObject>();

        /// <summary>
        /// Возвращает список объектов, подлежащих безвозвратному удалению.
        /// </summary>
        public List<IDeletableObjectViewModel> DeletingObjects
        {
            get;
        } = new List<IDeletableObjectViewModel>();

        /// <summary>
        /// Возвращает группу отключенных объектов.
        /// </summary>
        public GroupViewModel DisabledObjectsGroup
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления ошибок.
        /// </summary>
        public ErrorsViewModel ErrorsViewModel
        {
            get;
        } = new ErrorsViewModel();

        /// <summary>
        /// Возвращает команду экспорта данных.
        /// </summary>
        public RelayCommand ExportCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления быстрого поиска.
        /// </summary>
        public FastSearchViewModel FastSearch
        {
            get
            {
                return m_fastSearch;
            }
            set
            {
                m_fastSearch = value;
            }
        }
        [NonSerialized]
        FastSearchViewModel m_fastSearch;

        /// <summary>
        /// Возвращает команду нахождения узлов поворота.
        /// </summary>
        public RelayCommand FindBendNodesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду нахождения узлов поворота.
        /// </summary>
        public RelayCommand FindFreeNodesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду поиска объекта.
        /// </summary>
        public RelayCommand FindCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает фильтры слоев.
        /// </summary>
        public List<FilterViewModel> Filters
        {
            get;
        } = new List<FilterViewModel>();

        /// <summary>
        /// Возвращает модель представления неподключенных узлов.
        /// </summary>
        public FreeNodesViewModel FreeNodesViewModel
        {
            get;
        } = new FreeNodesViewModel();

        /// <summary>
        /// Возвращает команду вывода отчета о несопоставленных объектах.
        /// </summary>
        public RelayCommand GenerateDiffObjectsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вывода отчета о несопоставленных объектах.
        /// </summary>
        public RelayCommand GenerateDiffObjects2Command
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вывода отчета о проценте сопоставления.
        /// </summary>
        public RelayCommand GenerateIntegrationStatsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вывода отчета о проценте сопоставления (только по факту).
        /// </summary>
        public RelayCommand GenerateIntegrationStats2Command
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает положение точки области редактирования группы объектов.
        /// </summary>
        public System.Windows.Point GroupAreaOriginPoint
        {
            get
            {
                return this.groupAreaOriginPoint;
            }
            set
            {
                this.groupAreaOriginPoint = value;

                this.NotifyPropertyChanged(nameof(this.GroupAreaOriginPoint));
            }
        }

        /// <summary>
        /// Возвращает или задает положение области редактирования группы объектов.
        /// </summary>
        public System.Windows.Point GroupAreaPosition
        {
            get
            {
                return this.groupAreaPosition;
            }
            set
            {
                this.groupAreaPosition = value;

                this.NotifyPropertyChanged(nameof(this.GroupAreaPosition));
            }
        }

        /// <summary>
        /// Возвращает или задает размер области редактирования группы объектов.
        /// </summary>
        public System.Windows.Size GroupAreaSize
        {
            get
            {
                return this.groupAreaSize;
            }
            set
            {
                this.groupAreaSize = value;

                this.NotifyPropertyChanged(nameof(this.GroupAreaSize));
            }
        }

        /// <summary>
        /// Возвращает или задает панель инструментов группового редактирования.
        /// </summary>
        public GroupAreaToolBarViewModel GroupAreaToolBar
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает группы слоев.
        /// </summary>
        public List<GroupViewModel> Groups
        {
            get;
        } = new List<GroupViewModel>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли выбранный объект.
        /// </summary>
        public bool HasSelectedObject
        {
            get
            {
                return this.hasSelectedObject;
            }
            private set
            {
                this.hasSelectedObject = value;

                this.NotifyPropertyChanged(nameof(this.HasSelectedObject));
            }
        }

        /// <summary>
        /// Возвращает сервис истории изменений.
        /// </summary>
        public HistoryService HistoryService
        {
            get;
        }

        /// <summary>
        /// Возвращает слой гидравлики.
        /// </summary>
        public HydraulicsViewModel Hydraulics
        {
            get;
        }

        /// <summary>
        /// Возвращает слой ошибок гидравлики.
        /// </summary>
        public HydraulicsErrorViewModel HydraulicsError
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вывода отчета о гидравлическом расчете.
        /// </summary>
        public RelayCommand HydraulicsToExcelCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду импорта данных.
        /// </summary>
        public RelayCommand ImportCommand
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

                // Обновляем состояния команд.
                this.AttachNodesCommand.RaiseCanExecuteChanged();
                this.CheckErrorsCommand.RaiseCanExecuteChanged();
                this.CheckFuelCommand.RaiseCanExecuteChanged();
                this.CheckJurCommand.RaiseCanExecuteChanged();
                this.CheckJurCompletedCommand.RaiseCanExecuteChanged();
                this.CheckJurLeftCommand.RaiseCanExecuteChanged();
                this.CheckKvpCommand.RaiseCanExecuteChanged();
                this.CheckKvpCompletedCommand.RaiseCanExecuteChanged();
                this.CheckKvpLeftCommand.RaiseCanExecuteChanged();
                this.CheckSavedErrorsCommand.RaiseCanExecuteChanged();
                this.ClearImageCacheCommand.RaiseCanExecuteChanged();
                this.ExportCommand.RaiseCanExecuteChanged();
                this.FindBendNodesCommand.RaiseCanExecuteChanged();
                this.FindFreeNodesCommand.RaiseCanExecuteChanged();
                this.FindCommand.RaiseCanExecuteChanged();
                this.ImportCommand.RaiseCanExecuteChanged();
                this.ReattachNodesCommand.RaiseCanExecuteChanged();
                this.SaveAsImageCommand.RaiseCanExecuteChanged();
                this.SaveCommand.RaiseCanExecuteChanged();
                this.SelectBrokenLines.RaiseCanExecuteChanged();

                this.FastSearch.IsEnabled = value;
            }
        }

        /// <summary>
        /// Возвращает модель представления списка сопоставленных объектов.
        /// </summary>
        public JurKvpCompletedListViewModel JurKvpCompletedListViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает команду настройки слоев.
        /// </summary>
        public RelayCommand LayersSettingsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает информацию о рисуемой линии.
        /// </summary>
        public LineInfoViewModel LineInfo
        {
            get;
        } = new LineInfoViewModel();

        /// <summary>
        /// Возвращает команду загрузки данных.
        /// </summary>
        public RelayCommand LoadCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список идентификаторов загруженных котельных.
        /// </summary>
        public List<Guid> LoadedBoilers
        {
            get;
            private set;
        } = new List<Guid>();

        /// <summary>
        /// Возвращает или задает название загруженного населенного пункта.
        /// </summary>
        public string LoadedCityName
        {
            get
            {
                return this.loadedCityName;
            }
            private set
            {
                this.loadedCityName = value;

                this.NotifyPropertyChanged(nameof(this.LoadedCityName));
            }
        }

        /// <summary>
        /// Возвращает или задает время загрузки данных.
        /// </summary>
        public double LoadTime
        {
            get
            {
                return this.loadTime;
            }
            private set
            {
                this.loadTime = value;

                this.NotifyPropertyChanged(nameof(this.LoadTime));
            }
        }

        /// <summary>
        /// Возвращает название логина.
        /// </summary>
        public string LoginName
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления карты.
        /// </summary>
        public MapViewModel MapViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает группу неразмещенных объектов.
        /// </summary>
        public GroupViewModel NotPlacedObjectsGroup
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления списка объектов.
        /// </summary>
        public ObjectListViewModel ObjectListViewModel
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
        /// Возвращает модель представления истории изменений значения параметра.
        /// </summary>
        public ParameterHistoryViewModel ParameterHistoryViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вставки объекта.
        /// </summary>
        public RelayCommand PasteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает группу планируемых объектов.
        /// </summary>
        public GroupViewModel PlanningObjectsGroup
        {
            get;
        }

        /// <summary>
        /// Возвращает команду прикрепления узлов к фигурам.
        /// </summary>
        public RelayCommand ReattachNodesCommand
        {
            get;
        }



        /// <summary>
        /// Возвращает дерево отчетов.
        /// </summary>
        public List<ITreeItemViewModel> Reports
        {
            get;
        }

        /// <summary>
        /// Возвращает название пользовательской роли.
        /// </summary>
        public string RoleName
        {
            get;
        }

        /// <summary>
        /// Возвращает слой ремонтной программы.
        /// </summary>
        public RPViewModel RP
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сохранения изображения.
        /// </summary>
        public RelayCommand SaveAsImageCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сохранения данных.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления ошибок в значениях сохраненных параметров объектов.
        /// </summary>
        public SavedErrorsViewModel SavedErrorsViewModel
        {
            get;
        } = new SavedErrorsViewModel();

        /// <summary>
        /// Возвращает команду выбора линий со сломанными узлами.
        /// </summary>
        public RelayCommand SelectBrokenLines
        {
            get;
        }

        /// <summary>
        /// Возвращает название сервера.
        /// </summary>
        public string ServerName
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

        /// <summary>
        /// Возвращает информацию о складе.
        /// </summary>
        public StorageInfoViewModel StorageInfo
        {
            get;
        }

        /// <summary>
        /// Возвращает тестовую команду.
        /// </summary>
        public RelayCommand TestCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления панели инструментов.
        /// </summary>
        public ToolBarViewModel ToolBarViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает слой несопоставленных объектов.
        /// </summary>
        public UOViewModel UO
        {
            get;
        }

        public IJSViewModel IJS
        {
            get;
        }

        public IJSViewModel IJSF
        {
            get;
        }

        public DisableObjectViewModel disableObjectViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает список объектов, которые нужно обновить раньше остальных объектов.
        /// </summary>
        public List<IEditableObjectViewModel> UpdatingObjects
        {
            get;
        } = new List<IEditableObjectViewModel>();

        /// <summary>
        /// Возвращает слой разделения линий по годам.
        /// </summary>
        public YearDiffViewModel YearDiff
        {
            get;
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Возвращает или задает копируемый объект.
        /// </summary>
        private ICopyableObjectViewModel CopyingObject
        {
            get
            {
                return this.copyingObject;
            }
            set
            {
                this.copyingObject = value;

                this.PasteCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает команду группировки линий.
        /// </summary>
        private RelayCommand GroupLinesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает вставляемый объект.
        /// </summary>
        private ICopyableObjectViewModel PastingObject
        {
            get
            {
                return this.pastingObject;
            }
            set
            {
                this.pastingObject = value;

                this.PasteCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает команду разгруппировки линий.
        /// </summary>
        private RelayCommand UngroupLinesCommand
        {
            get;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="CustomLayersViewModel.VisibilityChanged"/> модели представления кастомных слоев.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void CustomLayersViewModel_VisibilityChanged(object sender, CustomLayerVisibilityChangedEventArgs e)
        {
            foreach (var obj in this.CustomObjects)
                if (obj.CustomLayer == e.Layer)
                    if (e.IsVisible)
                        obj.IsPlaced = true;
                    else
                        obj.IsPlaced = false;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="HistoryService.HistoryChanged"/> истории изменений.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void HistoryService_HistoryChanged(object sender, EventArgs e)
        {
            this.SaveCommand.RaiseCanExecuteChanged();

            // Скрываем дополнительные слои, если они видны.
            if (this.MapViewModel.IsHydraulicsVisible)
                this.MapViewModel.ShowHideHydraulicsCommand.Execute(null);
            if (this.MapViewModel.IsYearDiffVisible)
                this.MapViewModel.ShowHideYearDiffCommand.Execute(null);
            if (this.MapViewModel.IsRPVisible)
                this.MapViewModel.ShowHideRPCommand.Execute(null);
            if (this.MapViewModel.IsUOVisible)
                this.MapViewModel.ShowHideUOCommand.Execute(null);
            if (this.MapViewModel.IsIJSVisibleT)
                this.MapViewModel.ShowHideIJSTCommand.Execute(null);
            if (this.MapViewModel.IsIJSVisibleF)
                this.MapViewModel.ShowHideIJSFCommand.Execute(null);
        }
        PolylineViewModel polyLine;


        List<PolylineViewModel> m_polyLineList;
        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.SelectedLayer):
                    var layer = this.SelectedLayer as IParameterizedObjectViewModel;

                    this.ParameterGridViewModel.CancelLoading();

                    if (layer != null)
                    {
                        // Добавляем отслеживание за изменением свойств слоя объектов, чтобы выгрузить значения его параметров, когда с него будет снят выбор.
                        (layer as BaseViewModel).PropertyChanged += this.SelectedLayer_PropertyChanged;

                        await this.ParameterGridViewModel.LoadLayerDataAsync(layer as LayerViewModel);
                    }
                    else
                        this.ParameterGridViewModel.UnloadData();

                    break;

                case nameof(this.SelectedNotPlacedObject):
                    var notPlacedObject = this.SelectedNotPlacedObject as IParameterizedObjectViewModel;

                    this.ParameterGridViewModel.CancelLoading();
                    //if (!BaseSqlDataAccessService.localModeFlag)
                    
                    
                    if (DataService.MapAccessService.testConnection("", true))
                    {

                        if (notPlacedObject != null)
                        {
                            // Добавляем отслеживание за изменением свойств неразмещенного объекта, чтобы выгрузить значения его параметров, когда с него будет снят выбор.
                            (notPlacedObject as BaseViewModel).PropertyChanged += this.SelectedObject_PropertyChanged;

                            await this.ParameterGridViewModel.LoadObjectDataAsync(notPlacedObject);
                        }
                        else
                            this.ParameterGridViewModel.UnloadData();
                    }
                    break;

                case nameof(this.SelectedObject):
                    var obj = this.SelectedObject as IParameterizedObjectViewModel;

                    this.ParameterGridViewModel.CancelLoading();


                    
                    

                        if (obj != null)
                        {
                            // Добавляем отслеживание за изменением свойств объекта, чтобы выгрузить значения его параметров, когда с него будет снят выбор.
                            (obj as BaseViewModel).PropertyChanged += this.SelectedObject_PropertyChanged;

                            await this.ParameterGridViewModel.LoadObjectDataAsync(obj);
                        }
                        else
                            //if (!BaseSqlDataAccessService.localModeFlag)
                            if ( DataService.MapAccessService.testConnection("",true))
                                this.ParameterGridViewModel.UnloadData();
                    
                    this.ParameterHistoryViewModel.SelectedObject = obj != null ? obj as IObjectViewModel : null;

                    // Если выбранный объект - это котельная, то отображаем всплывающее окно.
                    var figure = this.SelectedObject as FigureViewModel;
                    // Также нужно проверить, можно ли отобразить всплывающее окно.


                    /*
                     * mstsc
                    if (figure != null && !figure.IsBoiler && !figure.IsStorage && !figure.isTrashStorage)
                    {
                        await this.BuildingInfo.SetBuildingAsync(figure, this.CurrentSchema);
                    } else
                        this.BuildingInfo.UnsetBuilding();
                    */
                    if (figure != null && figure.IsBoiler && this.MapViewModel.IsBoilerPopupVisible)
                        await this.BoilerInfo.SetBoilerAsync(figure, this.CurrentSchema);
                    else
                        this.BoilerInfo.UnsetBoiler();
                    // Или если это склад.
                    if (figure != null && figure.IsStorage && this.MapViewModel.IsStoragePopupVisible)
                        await this.StorageInfo.SetStorageAsync(figure, this.CurrentSchema);
                    else
                        this.StorageInfo.UnsetStorage();

                    if (figure != null && figure.isTrashStorage)
                    {
                        //this.DataService.FigureAccessService.GetKvpObjects
                        List<string> buildingTrashConnect = 
                            this.DataService.FigureAccessService.getTrashList(figure.CityId, figure.Id.ToString());
                        int vs = ((ShapeMapBindingService)mapBindingService).test();






                        var allInterObjectOnLayers = this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure).
                            Concat(this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure)).
                            Concat(this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure));

                        List<FigureViewModel> tmpFigureList = new List<FigureViewModel>();

                        foreach ( var lay in allInterObjectOnLayers)
                            foreach (FigureViewModel ob in lay.Objects)
                            {
                                if (ob != null)
                                {
                                    

                                    if (ob.ChangedParameterValues.ParameterValues.Count > 0)
                                    {
                                        foreach (var itm in ob.ChangedParameterValues.ParameterValues)
                                        {
                                            if (itm.Key.Id == 403)
                                            {

                                                string currentGUIDstring = ob.Id.ToString();

                                                int indexFromPrevList = 
                                                    buildingTrashConnect.FindIndex((x) => x == currentGUIDstring);
                                                bool isPresent = itm.Value.ToString().Equals(figure.Id.ToString());
                                                if (indexFromPrevList >= 0)
                                                {
                                                    if ( !isPresent )
                                                    {
                                                        buildingTrashConnect.RemoveAt(indexFromPrevList);
                                                    }
                                                }
                                                else
                                                {
                                                    if (isPresent)
                                                    {
                                                        buildingTrashConnect.Add(currentGUIDstring);
                                                    }
                                                }
                                            
                                            }
                                        }
//                                      
                                    }

                                    if (tmpFigureList.Count < buildingTrashConnect.Count)
                                    {
                                        int indexFromPrevListTmp =
                                            buildingTrashConnect.FindIndex((x) => x == ob.Id.ToString());
                                        if (indexFromPrevListTmp >= 0)
                                        {
                                            tmpFigureList.Add(ob);
                                        }
                                    }

                                }
                            }


                        foreach (var itm in tmpFigureList)
                        {

                        }
                        
                        double xPosition = figure.Position.X;
                        double yPosition = figure.Position.Y;
                        Views.InteractiveFigure interactiveTrashFigure;
                        if (((ShapeMapBindingService)mapBindingService).GetMapObject(figure) is Views.InteractiveFigure)
                        {
                            interactiveTrashFigure =
                                ((ShapeMapBindingService)mapBindingService).GetMapObject(figure) as Views.InteractiveFigure;
                     

                            ObjectType ot = this.DataService.ObjectTypes[0];
                      
                            m_polyLineList = new List<PolylineViewModel>();

                            foreach (FigureViewModel itm in tmpFigureList)
                            {
                                if (((ShapeMapBindingService)mapBindingService).GetMapObject(itm) is Views.InteractiveFigure)
                                {
                                    double xPositionItm = 0;
                                    double yPositionItm = 0;




                                    LineInfoViewModel lineInfo = new LineInfoViewModel();



                                    PointCollection pointCollection = new PointCollection();
                                

                                    var types = new List<ObjectType>();

                                    types.Add(this.ToolBarViewModel.LineTypes[0].Type);


                                    

                                    Views.InteractiveFigure interactiveFigure =
                                        ((ShapeMapBindingService)mapBindingService).GetMapObject(itm) as Views.InteractiveFigure;
                                    xPosition = interactiveFigure.LeftTopCorner.X;
                                    yPosition = interactiveFigure.LeftTopCorner.Y;

                                    //System.Windows.Point pnt = interactiveFigure.GetNearestPoint(itm.Position);
                                    System.Windows.Point pnt = interactiveFigure.GetNearestPoint(figure.Position);
                                    xPositionItm = pnt.X;
                                    yPositionItm = pnt.Y;



                                    var  tmpPoly = new PolylineViewModel(types[0], true, pointCollection, this.mapBindingService.GetBrush(types[0].Color), lineInfo, this.MapViewModel.Scale, this.DataService, this.MessageService, this.mapBindingService, 4);
                                    
                                    m_polyLineList.Add(tmpPoly);

                                    System.Windows.Point pntTrash = interactiveTrashFigure.GetNearestPoint(pnt);
                                    pointCollection.Add(new System.Windows.Point(xPositionItm, yPositionItm));
                                    pointCollection.Add(new System.Windows.Point(pntTrash.X, pntTrash.Y));
                                    //polyLine.IsPlaced = true;
                                    m_polyLineList.Last().IsPlaced = true;

                                }
                            }
                        }
                        

                    }
                    else
                    {
                        if (polyLine != null)
                        {
                            if (polyLine.IsPlaced)
                                polyLine.IsPlaced = false;
                        }

                        if (m_polyLineList != null)
                        {
                            foreach (var line in m_polyLineList)
                            {
                                line.IsPlaced = false;
                            }
                            m_polyLineList.Clear();
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="HistoryService.HistoryChanged"/> истории изменений.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapBindingService_ScaleChanged(object sender, EventArgs e)
        {
            this.RefreshToolBar();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MapViewModel.NeedReopen"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_NeedReopen(object sender, EventArgs e)
        {
            this.CitySelectionViewModel.SelectedCity = this.CitySelectionViewModel.LoadedCity;

            this.Load(this.CitySelectionViewModel.LoadedCity, false);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MapViewModel.NeedSilentSave"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_NeedSilentSave(object sender, BoolResultEventArgs e)
        {
            e.Result = this.Save1(true);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MapViewModel.AutoHideNodes))
                this.RefreshToolBar();

            if (e.PropertyName == nameof(MapViewModel.IsPrintAreaVisible))
                if (this.MapViewModel.IsPrintAreaVisible)
                    this.PrintToolBarRequested?.Invoke(this, EventArgs.Empty);
                else
                    this.PrintToolBarCloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> выбранной группы слоев неразмещенных объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SelectedLayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISelectableObjectViewModel.IsSelected))
            {
                if ((sender as ISelectableObjectViewModel).IsSelected)
                    return;

                var obj = sender as IParameterizedObjectViewModel;

                (sender as BaseViewModel).PropertyChanged -= this.SelectedLayer_PropertyChanged;

                this.ParameterGridViewModel.CancelLoading();

                if (obj != null)
                {
                    obj.UnloadParameterValues();
                    obj.UnloadCommonParameterValues();
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> выбранного объекта.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SelectedObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISelectableObjectViewModel.IsSelected))
            {
                if ((sender as ISelectableObjectViewModel).IsSelected)
                    return;

                var obj = sender as IParameterizedObjectViewModel;

                (sender as BaseViewModel).PropertyChanged -= this.SelectedObject_PropertyChanged;

                this.ParameterGridViewModel.CancelLoading();

                
                if (obj != null && /*!BaseSqlDataAccessService.localModeFlag*/false)
                {
                    obj.UnloadParameterValues();
                    obj.UnloadCalcParameterValues();
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ToolBarViewModel.DecreaseFontRequested"/> модели представления панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBarViewModel_DecreaseFontRequested(object sender, EventArgs e)
        {
            if ((this.selectedObjectsGroup.ObjectCount == 0 && this.SelectedLayer == null) || (this.SelectedLayer != null && (this.SelectedLayer as LayerViewModel).ObjectCount == 0))
            {
                this.MessageService.ShowMessage("Необходимо выбрать хотя бы один объект, размер надписи которого следует уменьшить", "Уменьшение размера шрифта надписей", MessageType.Information);

                return;
            }

            if (this.selectedObjectsGroup.ObjectCount != 0)
            {
                foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                    foreach (FigureViewModel figure in layer.Objects)
                        figure.DecreaseLabelSize();

                foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                    foreach (LineViewModel line in layer.Objects)
                        line.DecreaseLabelSize();
            }
            else
            {
                var selectedLayer = this.SelectedLayer as LayerViewModel;

                if (selectedLayer.Type.ObjectKind == ObjectKind.Figure)
                    foreach (FigureViewModel figure in selectedLayer.Objects)
                        figure.DecreaseLabelSize();
                else
                    if (selectedLayer.Type.ObjectKind == ObjectKind.Line)
                    foreach (LineViewModel line in selectedLayer.Objects)
                        line.DecreaseLabelSize();
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ToolBarViewModel.IncreaseFontRequested"/> модели представления панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBarViewModel_IncreaseFontRequested(object sender, EventArgs e)
        {
            if ((this.selectedObjectsGroup.ObjectCount == 0 && this.SelectedLayer == null) || (this.SelectedLayer != null && (this.SelectedLayer as LayerViewModel).ObjectCount == 0))
            {
                this.MessageService.ShowMessage("Необходимо выбрать хотя бы один объект, размер надписи которого следует увеличить", "Увеличение размера шрифта надписей", MessageType.Information);

                return;
            }

            if (this.selectedObjectsGroup.ObjectCount != 0)
            {
                foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                    foreach (FigureViewModel figure in layer.Objects)
                        figure.IncreaseLabelSize();

                foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                    foreach (LineViewModel line in layer.Objects)
                        line.IncreaseLabelSize();
            }
            else
            {
                var selectedLayer = this.SelectedLayer as LayerViewModel;

                if (selectedLayer.Type.ObjectKind == ObjectKind.Figure)
                    foreach (FigureViewModel figure in selectedLayer.Objects)
                        figure.IncreaseLabelSize();
                else
                    if (selectedLayer.Type.ObjectKind == ObjectKind.Line)
                    foreach (LineViewModel line in selectedLayer.Objects)
                        line.IncreaseLabelSize();
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> модели представления панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ToolBarViewModel.SelectedTool))
            {
                if (this.ToolBarViewModel.SelectedTool != Tool.Selector)
                {
                    if (this.SelectedGroup != null)
                        this.SelectedGroup.IsSelected = false;

                    if (this.SelectedLayer != null)
                        this.SelectedLayer.IsSelected = false;

                    if (this.SelectedObject != null)
                        this.SelectedObject.IsSelected = false;

                    this.ClearSelectedGroup();

                    if (this.MapViewModel.IsHydraulicsVisible)
                        // Скрываем слой гидравлики, так как изменение инструмента может привести к визуальному изменению схемы.
                        this.MapViewModel.ShowHideHydraulicsCommand.Execute(null);

                    if (this.MapViewModel.IsYearDiffVisible)
                        this.MapViewModel.ShowHideYearDiffCommand.Execute(null);
                    if (this.MapViewModel.IsRPVisible)
                        this.MapViewModel.ShowHideRPCommand.Execute(null);
                    if (this.MapViewModel.IsUOVisible)
                        this.MapViewModel.ShowHideUOCommand.Execute(null);
                    if (this.MapViewModel.IsIJSVisibleT)
                        this.MapViewModel.ShowHideIJSTCommand.Execute(null);
                    if (this.MapViewModel.IsIJSVisibleF)
                        this.MapViewModel.ShowHideIJSFCommand.Execute(null);
                    if (this.MapViewModel.IsDisabledVisible)
                        this.MapViewModel.ShowHideDisableObjCommand.Execute(null);

                }

                if (this.ToolBarViewModel.SelectedTool != Tool.Editor)
                    if (this.EditingObject != null)
                        this.EditingObject.IsEditing = false;

                if (this.ToolBarViewModel.SelectedTool == Tool.GroupArea)
                    this.GroupAreaToolBarRequested?.Invoke(this, EventArgs.Empty);
                else
                    this.GroupAreaToolBarCloseRequested?.Invoke(this, EventArgs.Empty);

                this.CopyCommand.RaiseCanExecuteChanged();
                this.PasteCommand.RaiseCanExecuteChanged();

                this.MapViewModel.IsReadOnly = this.ToolBarViewModel.SelectedTool == Tool.Selector;

                if (this.ToolBarViewModel.SelectedTool == Tool.PrintArea)
                {
                    if (!this.MapViewModel.IsPrintAreaVisible)
                        this.MapViewModel.IsPrintAreaVisible = true;

                    if (!this.MapViewModel.IsPrintAreaVisible)
                        this.ToolBarViewModel.SelectedTool = Tool.Selector;
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ToolBarViewModel.ResetLabelsRequested"/> модели представления панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBarViewModel_ResetLabelsRequested(object sender, EventArgs e)
        {
            if ((this.selectedObjectsGroup.ObjectCount == 0 && this.SelectedLayer == null) || (this.SelectedLayer != null && (this.SelectedLayer as LayerViewModel).ObjectCount == 0))
            {
                this.MessageService.ShowMessage("Необходимо выбрать хотя бы один объект, настройки надписи которого следует сбросить", "Сброс настроек надписей", MessageType.Information);

                return;
            }

            if (this.selectedObjectsGroup.ObjectCount != 0)
            {
                foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                    foreach (FigureViewModel figure in layer.Objects)
                        figure.ResetLabel();

                foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                    foreach (LineViewModel line in layer.Objects)
                        line.ResetLabels();
            }
            else
            {
                var selectedLayer = this.SelectedLayer as LayerViewModel;

                if (selectedLayer.Type.ObjectKind == ObjectKind.Figure)
                    foreach (FigureViewModel figure in selectedLayer.Objects)
                        figure.ResetLabel();
                else
                    if (selectedLayer.Type.ObjectKind == ObjectKind.Line)
                    foreach (LineViewModel line in selectedLayer.Objects)
                        line.ResetLabels();
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ToolBarViewModel.StopGroupEdit"/> модели представления панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBarViewModel_StartGroupEdit(object sender, StartGroupEditEventArgs e)
        {
            // Составляем список редактируемых объектов и объектов, используемых для отображения очертаний на карте.
            this.editingObjects = new List<IMapObjectViewModel>();
            var nodes = new List<IMapObjectViewModel>();
            var lineOutlines = new List<IMapObjectViewModel>();
            foreach (var layer in this.selectedObjectsGroup.Layers)
                foreach (IMapObjectViewModel obj in layer.Objects)
                {
                    if (obj is FigureViewModel)
                        this.editingObjects.Add(obj);

                    if (obj is LineViewModel)
                    {
                        var line = obj as LineViewModel;

                        if (line.LeftNode != null && !nodes.Contains(line.LeftNode))
                            nodes.Add(line.LeftNode);

                        if (line.RightNode != null && !nodes.Contains(line.RightNode))
                            nodes.Add(line.RightNode);

                        lineOutlines.Add(line);
                    }
                }

            if (this.editingObjects.Count == 0 && nodes.Count == 0)
            {
                e.CanStart = false;

                return;
            }

            var figureCount = this.editingObjects.Count;
            var lineCount = lineOutlines.Count;

            this.outlineObjects.AddRange(this.editingObjects);
            this.outlineObjects.AddRange(lineOutlines);

            // Находим граничные точки во всех объектах.
            var minX = double.MaxValue;
            var maxX = double.MinValue;
            var minY = double.MaxValue;
            var maxY = double.MinValue;
            System.Windows.Point position;
            foreach (FigureViewModel obj in this.editingObjects)
            {
                // В качестве граничной берем центральную точку.
                position = obj.Position;
                position = new System.Windows.Point(position.X + obj.Size.Width / 2, position.Y + obj.Size.Height / 2);

                if (position.X < minX)
                    minX = position.X;

                if (position.X > maxX)
                    maxX = position.X;

                if (position.Y < minY)
                    minY = position.Y;

                if (position.Y > maxY)
                    maxY = position.Y;
            }
            foreach (NodeViewModel obj in nodes)
            {
                position = obj.LeftTopCorner;

                if (position.X < minX)
                    minX = position.X;

                if (position.X > maxX)
                    maxX = position.X;

                if (position.Y < minY)
                    minY = position.Y;

                if (position.Y > maxY)
                    maxY = position.Y;
            }

            // Вычисляем середину ограничивающего прямоугольника.
            this.GroupAreaOriginPoint = new System.Windows.Point(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);

            // Задаем положение и размер области редактирования группы объектов.
            this.GroupAreaPosition = new System.Windows.Point(0, 0);
            this.GroupAreaSize = new System.Windows.Size(this.MapViewModel.SubstrateSize.Width, this.MapViewModel.SubstrateSize.Height);

            this.editingObjects.AddRange(lineOutlines);
            this.editingObjects.AddRange(nodes);

            this.groupEditStarted = true;

            // Создаем очертания объектов на карте.
            this.mapBindingService.CreateOutlines(this.outlineObjects);

            this.GroupAreaToolBar = new GroupAreaToolBarViewModel(lineOutlines.Select(x => x as LineViewModel).ToList(), figureCount, lineCount, this.HistoryService);

            e.CanStart = this.groupEditStarted;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ToolBarViewModel.StartGroupEdit"/> модели представления панели инструментов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ToolBarViewModel_StopGroupEdit(object sender, EventArgs e)
        {
            this.groupEditStarted = false;

            this.editingObjects = null;

            this.outlineObjects.Clear();

            this.mapBindingService.ClearOutlines();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Добавляет узел и присоединяет к нему заданную линию.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="line">Линия.</param>
        /// <param name="side">Сторона соединения линии с узлом.</param>
        private void AddNode(NodeViewModel node, LineViewModel line, NodeConnectionSide side)
        {
            if (!node.IsPlaced)
            {
                this.AddObject(node);

                node.AddConnection(new NodeConnection(line, side));

                // Если узел существует в источнике данных, то убираем с него отметку на обновление.
                if (node.IsSaved)
                    this.UnmarkToUpdate(node);
            }
            else
                node.AddConnection(new NodeConnection(line, side));
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить прикрепление узлов к границам фигур.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteAttachNodes()
        {
            return this.IsDataLoaded;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить проверку наличия ошибок.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить проверку наличия ошибок.</returns>
        private bool CanExecuteCheckErrors()
        {
            return this.IsDataLoaded && this.AccessService.CanCheckErrors(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить проверку интеграции с программой "Расчет потребления топлива".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckFuel()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить проверку интеграции с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckJur()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли отобразить объекты, сопоставленные с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckJurCompleted()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли отобразить объекты, несопоставленные с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckJurLeft()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить проверку интеграции с программой "Квартплата".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckKvp()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли отобразить объекты, сопоставленные с программой "Квартплата".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckKvpCompleted()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли отобразить объекты, несопоставленные с программой "Квартплата".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckKvpLeft()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanCheckBaseProgram(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить проверку наличия ошибок в сохраненных значениях параметров объектов.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCheckSavedErrors()
        {
            return this.IsDataLoaded && this.AccessService.CanCheckErrors(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить очистку кэша подложек.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteClearImageCache()
        {
            return !this.IsDataLoaded;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить копирование.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить копирование.</returns>
        private bool CanExecuteCopy()
        {
            return this.EditingObject != null && (this.EditingObject is ICopyableObjectViewModel) && this.ToolBarViewModel.SelectedTool == Tool.Editor;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить удаление редактируемого объекта.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить удаление редактируемого объекта.</returns>
        private bool CanExecuteDelete()
        {
            if (this.EditingObject != null)
            {
                // Нельзя удалять сохраненные в источнике данных объекты.
                if ((this.EditingObject as ISavableObjectViewModel).IsSaved)
                    return false;

                var type = this.EditingObject.GetType();

                if (type == typeof(NodeViewModel))
                    // Нельзя удалять узлы.
                    return false;
            }
            else
                if (this.SelectedObject != null)
            {
                // Нельзя удалять сохраненные в источнике данных объекты.
                if ((this.SelectedObject as ISavableObjectViewModel).IsSaved)
                    return false;

                var type = this.SelectedObject.GetType();

                if (type == typeof(BadgeViewModel) || type == typeof(NonVisualObjectViewModel))
                    return true;
            }

            return this.EditingObject != null;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить экспорт данных.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteExport()
        {
#warning Временно заблокирован экспорт данных в связи с устареванием хранимой процедуры
            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить поиск объекта.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteFind()
        {
            return this.IsDataLoaded;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить поиск узлов поворота.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteFindBendNodes()
        {
            // Такое мы можем проделать только в том случае, если схема является фактической схемой, а также если пользователь обладает нужными правами.
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanFindBendNodes(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить поиск неподключенных узлов.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteFindFreeNodes()
        {
            return this.IsDataLoaded && this.CurrentSchema != null && this.AccessService.CanFindFreeNodes(this.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить импорт данных.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteImport()
        {
#warning Временно заблокирован импорт данных в связи с устареванием хранимой процедуры
            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить вставку.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить вставку.</returns>
        private bool CanExecutePaste()
        {
            return this.CopyingObject != null && this.ToolBarViewModel.SelectedTool == Tool.Editor && this.PastingObject == null;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить прикрепление узлов к фигурам.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteReattachNodes()
        {
            return this.IsDataLoaded && this.AccessService.CanReattachNodes(this.CurrentSchema.IsActual);
        }

 

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сохранение данных.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить сохранение данных.</returns>
        private bool CanExecuteSave()
        {
            // Сохранение можно выполнить только в том случае, если данные загружены и история изменений содержит неотмененную запись о действии, которая затрагивает данные.
            return this.IsDataLoaded && this.HistoryService.Contains(Target.Data);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сохранение изображения.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить сохранение изображения.</returns>
        private bool CanExecuteSaveAsImage()
        {
            return this.IsDataLoaded;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить выбор линий со сломанными узлами.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteSelectBrokenLines()
        {
            return this.IsDataLoaded;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить тестовое действие.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteTest()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Выявляет наличие ошибок в параметрах объекта и возвращает true, если ошибки найдены, иначе - false.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Возвращает значение, указывающее на то, что найдены ли ошибки.</returns>
        private bool CheckForError(IObjectViewModel obj)
        {
            var errors = (obj as IParameterizedObjectViewModel).GetErrors();

            foreach (var param in errors)
                this.ErrorsViewModel.Items.Add(new ErrorViewModel(obj, param));

            return errors.Count > 0;
        }

        /// <summary>
        /// Выявляет наличие ошибок в параметрах объектов и возвращает true, если ошибки найдены, иначе - false.
        /// </summary>
        /// <returns>Возвращает значение, указывающее на то, что найдены ли ошибки.</returns>
        private bool CheckForErrors()
        {
            bool result = false;

            this.ErrorsViewModel.Items.Clear();
            // Сперва проверяем фигуры.
            foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                foreach (var obj in layer.Objects)
                    if (this.CheckForError(obj))
                        result = true;
            foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                foreach (var obj in layer.Objects)
                    if (this.CheckForError(obj))
                        result = true;
            foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                foreach (var obj in layer.Objects)
                    if (this.CheckForError(obj))
                        result = true;

            // Затем линии.
            foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach (var obj in layer.Objects)
                    if (this.CheckForError(obj))
                        result = true;
            foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach (var obj in layer.Objects)
                    if (this.CheckForError(obj))
                        result = true;
            foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach (var obj in layer.Objects)
                    if (this.CheckForError(obj))
                        result = true;

            return result;
        }

        /// <summary>
        /// Выполняет прикрепление узлов к границам фигур.
        /// </summary>
        private void ExecuteAttachNodes()
        {
            foreach (var layer in this.NodesGroup.Layers)
                foreach (NodeViewModel node in layer.Objects)
                    if (node.ConnectedObject != null)
                        node.LeftTopCorner = new System.Windows.Point(node.LeftTopCorner.X + 0.001, node.LeftTopCorner.Y + 0.001);
        }

        /// <summary>
        /// Выполняет расчет гидравлики.
        /// </summary>
        private void ExecuteCalculateHydraulics()
        {
            // Сперва проверяем, выбрана ли котельная.
            var figure = this.SelectedObject as FigureViewModel;
            if (figure == null)
            {
                this.MessageService.ShowMessage("Сперва необходимо выбрать котельную", "Расчет гидравлики", MessageType.Information);

                return;
            }
            if (!figure.IsBoiler)
            {
                this.MessageService.ShowMessage("Сперва необходимо выбрать котельную", "Расчет гидравлики", MessageType.Information);

                return;
            }

            // Она должна быть уже сохраненной в базе данных.
            if (!figure.IsSaved)
            {
                this.MessageService.ShowMessage("Нельзя произвести расчет гидравлики для новой котельной", "Расчет гидравлики", MessageType.Information);

                return;
            }

            var waitViewModel = new WaitViewModel("Расчет гидравлики", "Пожалуйста подождите, идет расчет гидравлики...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.DataService.ReportAccessService.CalculateHydraulics(figure.Id, this.CurrentSchema);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            this.MessageService.ShowMessage("Расчет гидравлики успешно выполнен", "Расчет гидравлики", MessageType.Information);
        }

        /// <summary>
        /// Выполняет проверку наличия ошибок.
        /// </summary>
        private void ExecuteCheckErrors()
        {
            // Закрываем представление ошибок.
            this.ErrorsViewModel.Close();

            if (this.CheckForErrors())
            {
                if (this.ErrorFound != null)
                {
                    this.ErrorsViewModel.Title = "Проверка наличия ошибок";
                    this.ErrorsViewModel.Content = "Найдены незаполненные значения обязательных параметров. Пожалуйста, заполните значения заданных параметров:";

                    this.ErrorFound(this, EventArgs.Empty);
                }
            }
            else
                this.MessageService.ShowMessage("Ошибки не найдены", "Проверка наличия ошибок", MessageType.Information);
        }

        /// <summary>
        /// Выполняет проверку интеграции с программой "Учет потребления топлива".
        /// </summary>
        private void ExecuteCheckFuel()
        {
            // Закрываем представление списка объектов.
            this.ObjectListViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить проверку интеграции с программой \"Учет потребления топлива\", так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var objects = new List<Tuple<Guid, Guid, string, Guid>>();
            var boilers = new List<Tuple<Guid, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет проверка интеграции с программой \"Учет потребления топлива\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    objects.AddRange(this.DataService.FigureAccessService.GetObjectsWithoutFuelIntegration(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    boilers.AddRange(this.CurrentCity.GetBoilers(this.CurrentSchema, this.DataService));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (objects.Count > 0)
            {
                this.ObjectListViewModel.CurrentMode = ObjectListViewModel.Mode.Other;
                this.ObjectListViewModel.CanCompare = false;
                this.ObjectListViewModel.Tip = "У следующих объектов выявлена проблема с интеграцией с программой \"Учет потребления топлива\":";
                this.ObjectListViewModel.Title = "Проверка интеграции";

                this.ObjectListViewModel.ClearObjects();
                this.ObjectListViewModel.ClearCompareObjects();
                this.ObjectListViewModel.Boilers.Clear();

                this.ObjectListViewModel.SetObjects(objects);
                this.ObjectListViewModel.Boilers.AddRange(boilers);

                if (this.ObjectListViewModel.Boilers.Count > 0)
                    this.ObjectListViewModel.SelectedBoiler = this.ObjectListViewModel.Boilers.First().Item1;
                else
                    this.ObjectListViewModel.SelectedBoiler = Guid.Empty;

                this.ObjectListRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Проблем с интеграцией с программой \"Учет потребления топлива\" не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет проверку интеграции с программой "Расчеты с юридическими лицами".
        /// </summary>
        private void ExecuteCheckJur()
        {
            // Закрываем представление списка объектов.
            this.ObjectListViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить проверку интеграции с программой \"Расчеты с юридическими лицами\", так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var objects = new List<Tuple<Guid, Guid, string, Guid>>();
            var jur = new List<Tuple<string, string>>();
            var boilers = new List<Tuple<Guid, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет проверка интеграции с программой \"Расчеты с юридическими лицами\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    objects.AddRange(this.DataService.FigureAccessService.GetObjectsWithoutJurIntegration(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    jur.AddRange(this.DataService.FigureAccessService.GetJurObjects(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    boilers.AddRange(this.CurrentCity.GetBoilers(this.CurrentSchema, this.DataService));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (objects.Count > 0 || jur.Count > 0)
            {
                this.ObjectListViewModel.CurrentMode = ObjectListViewModel.Mode.Jur;
                this.ObjectListViewModel.CanCompare = true;
                this.ObjectListViewModel.Tip = "На левой стороне отображены объекты, у которых выявлена проблема с интеграцией с программой \"Расчеты с юридическими лицами\". При двойном щелчке по объекту он будет отображен на карте. Справа - объекты из программы \"Расчеты с юридическими лицами\". Вам необходимо выбрать слева и справа идентичные объекты и нажать на кнопку \"=\".";
                this.ObjectListViewModel.Title = "Проверка интеграции";

                this.ObjectListViewModel.ClearObjects();
                this.ObjectListViewModel.ClearCompareObjects();
                this.ObjectListViewModel.Boilers.Clear();

                this.ObjectListViewModel.SetObjects(objects);
                this.ObjectListViewModel.SetCompareObjects(jur);
                this.ObjectListViewModel.Boilers.AddRange(boilers);

                if (this.ObjectListViewModel.Boilers.Count > 0)
                    this.ObjectListViewModel.SelectedBoiler = this.ObjectListViewModel.Boilers.First().Item1;
                else
                    this.ObjectListViewModel.SelectedBoiler = Guid.Empty;

                this.ObjectListRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Проблем с интеграцией с программой \"Расчеты с юридическими лицами\" не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет отображение сопоставленных объектов.
        /// </summary>
        private void ExecuteCheckJurCompleted()
        {
            // Закрываем представление списка сопоставленных объектов.
            this.JurKvpCompletedListViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить отображение списка сопоставленных объектов, так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var objects = new List<Tuple<Guid, Guid, string, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет получение списка объектов, сопоставленных с программой \"Расчеты с юридическими лицами\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    objects.AddRange(this.DataService.FigureAccessService.GetObjectsWithJurIntegration(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (objects.Count > 0)
            {
                this.JurKvpCompletedListViewModel.CurrentMode = JurKvpCompletedListViewModel.Mode.Jur;

                this.JurKvpCompletedListViewModel.Objects.Clear();

                this.JurKvpCompletedListViewModel.Objects.AddRange(objects);

                this.JurCompletedListRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Объектов, сопоставленных с программой \"Расчеты с юридическими лицами\", не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет отображение несопоставленных объектов.
        /// </summary>
        private void ExecuteCheckJurLeft()
        {
            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить отображение списка несопоставленных объектов, так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var hidden = new List<Tuple<long, string>>();
            var visible = new List<Tuple<long, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет получение списка объектов, несопоставленных с программой \"Расчеты с юридическими лицами\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    hidden.AddRange(this.DataService.KtsAccessService.GetJurHidden(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    visible.AddRange(this.DataService.KtsAccessService.GetJurVisible(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (hidden.Count > 0 || visible.Count > 0)
                this.KtsLeftViewRequested?.Invoke(this, new ViewRequestedEventArgs<KtsLeftoversViewModel>(new KtsLeftoversViewModel(hidden, visible, this.CurrentCity.Id, 2, this.DataService)));
            else
                this.MessageService.ShowMessage("Объектов, несопоставленных с программой \"Расчеты с юридическими лицами\", не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет проверку интеграции с программой "Квартплата".
        /// </summary>
        private void ExecuteCheckKvp()
        {
            // Закрываем представление списка объектов.
            this.ObjectListViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить проверку интеграции с программой \"Квартплата\", так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var objects = new List<Tuple<Guid, Guid, string, Guid>>();
            var kvp = new List<Tuple<string, string>>();
            var boilers = new List<Tuple<Guid, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет проверка интеграции с программой \"Квартплата\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    objects.AddRange(this.DataService.FigureAccessService.GetObjectsWithoutKvpIntegration(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    kvp.AddRange(this.DataService.FigureAccessService.GetKvpObjects(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    boilers.AddRange(this.CurrentCity.GetBoilers(this.CurrentSchema, this.DataService));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (objects.Count > 0 || kvp.Count > 0)
            {
                this.ObjectListViewModel.CurrentMode = ObjectListViewModel.Mode.Kvp;
                this.ObjectListViewModel.CanCompare = true;
                this.ObjectListViewModel.Tip = "На левой стороне отображены объекты, у которых выявлена проблема с интеграцией с программой \"Квартплата\". При двойном щелчке по объекту он будет отображен на карте. Справа - объекты из программы \"Квартплата\". Вам необходимо выбрать слева и справа идентичные объекты и нажать на кнопку \"=\".";
                this.ObjectListViewModel.Title = "Проверка интеграции";

                this.ObjectListViewModel.ClearObjects();
                this.ObjectListViewModel.ClearCompareObjects();
                this.ObjectListViewModel.Boilers.Clear();

                this.ObjectListViewModel.SetObjects(objects);
                this.ObjectListViewModel.SetCompareObjects(kvp);
                this.ObjectListViewModel.Boilers.AddRange(boilers);

                if (this.ObjectListViewModel.Boilers.Count > 0)
                    this.ObjectListViewModel.SelectedBoiler = this.ObjectListViewModel.Boilers.First().Item1;
                else
                    this.ObjectListViewModel.SelectedBoiler = Guid.Empty;

                this.ObjectListRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Проблем с интеграцией с программой \"Квартплата\" не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет отображение сопоставленных объектов.
        /// </summary>
        private void ExecuteCheckKvpCompleted()
        {
            // Закрываем представление списка сопоставленных объектов.
            this.JurKvpCompletedListViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить отображение списка сопоставленных объектов, так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var objects = new List<Tuple<Guid, Guid, string, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет получение списка объектов, сопоставленных с программой \"Квартплата\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    objects.AddRange(this.DataService.FigureAccessService.GetObjectsWithKvpIntegration(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (objects.Count > 0)
            {
                this.JurKvpCompletedListViewModel.CurrentMode = JurKvpCompletedListViewModel.Mode.Kvp;

                this.JurKvpCompletedListViewModel.Objects.Clear();

                this.JurKvpCompletedListViewModel.Objects.AddRange(objects);

                this.JurCompletedListRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Объектов, сопоставленных с программой \"Квартплата\", не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет отображение несопоставленных объектов.
        /// </summary>
        private void ExecuteCheckKvpLeft()
        {
            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить отображение списка несопоставленных объектов, так как имеются несохраненные данные", "Проверка интеграции", MessageType.Information);

                return;
            }

            var hidden = new List<Tuple<long, string>>();
            var visible = new List<Tuple<long, string>>();

            var waitViewModel = new WaitViewModel("Проверка интеграции", "Пожалуйста подождите, идет получение списка объектов, несопоставленных с программой \"Квартплата\"...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    hidden.AddRange(this.DataService.KtsAccessService.GetKvpHidden(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                    visible.AddRange(this.DataService.KtsAccessService.GetKvpVisible(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (hidden.Count > 0 || visible.Count > 0)
                this.KtsLeftViewRequested?.Invoke(this, new ViewRequestedEventArgs<KtsLeftoversViewModel>(new KtsLeftoversViewModel(hidden, visible, this.CurrentCity.Id, 1, this.DataService)));
            else
                this.MessageService.ShowMessage("Объектов, несопоставленных с программой \"Квартплата\", не найдено", "Проверка интеграции", MessageType.Information);
        }

        /// <summary>
        /// Выполняет проверку наличия ошибок в сохраненных значениях параметров объектов.
        /// </summary>
        private void ExecuteCheckSavedErrors()
        {
            this.SavedErrorsViewModel.Close();

            var ids = new List<Guid>();

            var waitViewModel = new WaitViewModel("Проверка наличия ошибок", "Пожалуйста подождите, идет проверка наличия ошибок...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    ids.AddRange(this.DataService.ParameterAccessService.GetObjectsWithErrors(this.CitySelectionViewModel.LoadedCity.Id, this.CurrentSchema));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (ids.Count > 0)
            {
                var items = new List<IObjectViewModel>();

                IObjectViewModel obj;

                foreach (var id in ids)
                {
                    obj = this.GetObject(id);

                    if (obj != null)
                        items.Add(obj);
                }

                this.SavedErrorsViewModel.Items.Clear();
                this.SavedErrorsViewModel.Items.AddRange(items);

                if (this.SavedErrorsViewModel.Items.Count > 0)
                    this.SavedErrorFound(this, EventArgs.Empty);
                else
                    this.MessageService.ShowMessage("Ошибки не найдены", "Проверка наличия ошибок", MessageType.Information);
            }
            else
                this.MessageService.ShowMessage("Ошибки не найдены", "Проверка наличия ошибок", MessageType.Information);
        }

        /// <summary>
        /// Выполняет очистку кэша подложек.
        /// </summary>
        private void ExecuteClearImageCache()
        {
            var result = this.MessageService.ShowYesNoMessage("Вы уверены что хотите очистить кэш подложек?", "Очистка кэша подложек");

            if (result)
            {
                var waitViewModel = new WaitViewModel("Очистка кэша подложек", "Пожалуйста подождите, идет очистка кэша подложек...", async () =>
                {
                    return await Task.Factory.StartNew(() =>
                    {
                        return this.substrateService.FullReset();
                    });
                });

                this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

                if (waitViewModel.Result)
                    this.MessageService.ShowMessage("Очистка кэша подложек прошла успешно", "Очистка кэша подложек", MessageType.Information);
                else
                    this.MessageService.ShowMessage("Не удалось очистить кэш подложек", "Очистка кэша подложек", MessageType.Error);
            }
        }

        /// <summary>
        /// Выполняет удаление сохраненных настроек.
        /// </summary>
        private void ExecuteClearSettings()
        {
            var result = this.MessageService.ShowYesNoMessage("Вы уверены что хотите удалить сохраненные настройки?", "Удаление сохраненных настроек");

            if (result)
                if (this.SettingService.FullReset())
                    this.MessageService.ShowMessage("Удаление сохраненных настроек прошло успешно", "Удаление сохраненных настроек", MessageType.Information);
                else
                    this.MessageService.ShowMessage("Не удалось удалить сохраненные настройки", "Удаление сохраненных настроек", MessageType.Error);
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
        /// Выполняет копирование объекта.
        /// </summary>
        private void ExecuteCopy()
        {
            this.CopyingObject = this.EditingObject as ICopyableObjectViewModel;
        }

        /// <summary>
        /// Выполняет удаление редактируемого объекта.
        /// </summary>
        private void ExecuteDelete()
        {
            if (this.MessageService.ShowYesNoMessage("Вы действительно хотите удалить выбранный объект?", "Удаление"))
            {
                var obj = this.EditingObject as IObjectViewModel;

                if (obj != null)
                {
                    if (this.EditingObject is LineViewModel)
                    {
                        var line = this.EditingObject as LineViewModel;

                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new AddRemoveLineAction(line, this, false);
                        this.HistoryService.Add(new HistoryEntry(action, Target.Data, "удаление линии"));
                        action.Do();
                    }

                    if (this.EditingObject is FigureViewModel)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new AddRemoveFigureAction((FigureViewModel)obj, this, false);
                        this.HistoryService.Add(new HistoryEntry(action, Target.Data, "удаление фигуры"));
                        action.Do();
                    }

                    if (this.EditingObject is BadgeViewModel)
                    {
                        var child = this.EditingObject as IChildObjectViewModel;

                        this.EditingObject.IsEditing = false;

                        if (child != null)
                            (child.Parent as IContainerObjectViewModel).DeleteChild(child as IObjectViewModel);
                    }
                }
                else
                {
                    var child = this.SelectedObject as IChildObjectViewModel;

                    if (child != null)
                        (child.Parent as IContainerObjectViewModel).DeleteChild(child as IObjectViewModel);
                    else
                        if (this.EditingObject is LabelViewModel)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new AddRemoveLabelAction((LabelViewModel)this.EditingObject, this, false);
                        this.HistoryService.Add(new HistoryEntry(action, Target.Data, "удаление надписи"));
                        action.Do();
                    }
                    else
                            if (this.EditingObject is ICustomLayerObject)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new AddRemoveCustomObjectAction((ICustomLayerObject)this.EditingObject, this, false);
                        this.HistoryService.Add(new HistoryEntry(action, Target.Data, "удаление пользовательского объекта"));
                        action.Do();
                    }
                }
            }
        }

        /// <summary>
        /// Выполняет экспорт данных.
        /// </summary>
        private void ExecuteExport()
        {
            if (this.ExportRequested != null)
            {
                var eventArgs = new ImportExportRequestedEventArgs();

                this.ExportRequested(this, eventArgs);

                if (!string.IsNullOrEmpty(eventArgs.FileName))
                {
                    var waitViewModel = new WaitViewModel("Экспорт данных", "Пожалуйста подождите, идет экспорт данных...", async () =>
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            var dataSet = new DataSet();

                            dataSet.Tables.Add(this.DataService.GetLogs());

                            using (var stream = File.Open(eventArgs.FileName, FileMode.OpenOrCreate))
                            {
                                var formatter = new BinaryFormatter();

                                formatter.Serialize(stream, dataSet);
                            }
                        });
                    });

                    this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

                    this.MessageService.ShowMessage("Эскпорт успешно завершен", "Экспорт данных", MessageType.Information);
                }
            }
        }

        /// <summary>
        /// Выполняет поиск объекта.
        /// </summary>
        private void ExecuteFind()
        {
            this.FindRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет поиск узлов поворота.
        /// </summary>
        private void ExecuteFindBendNodes()
        {
            // Закрываем представление узлов поворота.
            this.BendNodesViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить поиск узлов поворота, так как имеются несохраненные данные", "Поиск узлов поворота", MessageType.Information);

                return;
            }

            var ids = new List<Guid>();

            var waitViewModel = new WaitViewModel("Поиск узлов поворота", "Пожалуйста подождите, идет поиск узлов поворота...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    ids.AddRange(this.DataService.NodeAccessService.GetBendNodes(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (ids.Count > 0)
            {
                this.BendNodesViewModel.Nodes.Clear();

                // Находим все узлы поворота по их идентификаторам.
                var bendNodes = new List<NodeViewModel>();
                foreach (var layer in this.NodesGroup.Layers)
                    foreach (NodeViewModel node in layer.Objects)
                        if (ids.Contains(node.Id) && node.IsPlaced)
                            bendNodes.Add(node);

                this.BendNodesViewModel.Nodes.AddRange(bendNodes);

                this.BendNodesRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Узлы поворота не найдены", "Поиск узлов поворота", MessageType.Information);
        }

        /// <summary>
        /// Выполняет поиск неподключенных узлов.
        /// </summary>
        private void ExecuteFindFreeNodes()
        {
            // Закрываем представление неподключенных узлов.
            this.FreeNodesViewModel.Close();

            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Нельзя выполнить поиск неподключенных узлов, так как имеются несохраненные данные", "Поиск неподключенных узлов", MessageType.Information);

                return;
            }

            var ids = new List<Guid>();

            var waitViewModel = new WaitViewModel("Поиск неподключенных узлов", "Пожалуйста подождите, идет поиск неподключенных узлов...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    ids.AddRange(this.DataService.NodeAccessService.GetFreeNodes(this.CurrentSchema, this.CitySelectionViewModel.LoadedCity.Id));
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (ids.Count > 0)
            {
                this.FreeNodesViewModel.Nodes.Clear();

                // Находим все неподключенные узлы по их идентификаторам.
                var freeNodes = new List<NodeViewModel>();
                foreach (var layer in this.NodesGroup.Layers)
                    foreach (NodeViewModel node in layer.Objects)
                        if (ids.Contains(node.Id) && node.IsPlaced)
                            freeNodes.Add(node);

                this.FreeNodesViewModel.Nodes.AddRange(freeNodes);

                this.FreeNodesRequested?.Invoke(this, EventArgs.Empty);
            }
            else
                this.MessageService.ShowMessage("Неподключенные узлы не найдены", "Поиск неподключенных узлов", MessageType.Information);
        }

        /// <summary>
        /// Выполняет полное удаление редактируемого объекта.
        /// </summary>
        private void ExecuteFullDelete()
        {
            if (this.SaveCommand.CanExecute(null))
            {
                this.MessageService.ShowMessage("Невозможно удалить выбранный объект, так как имеются несохраненные данные", "Безвозвратное удаление", MessageType.Information);

                return;
            }

            if (this.MessageService.ShowYesNoMessage("Вы действительно хотите безвозвратно удалить выбранный объект?", "Безвозвратное удаление"))
            {
                var obj = this.EditingObject as IObjectViewModel;

                if (obj != null)
                {
                    if (this.EditingObject is LineViewModel)
                    {
                        var line = this.EditingObject as LineViewModel;

                        var leftNode = line.LeftNode;
                        var rightNode = line.RightNode;

                        // Сперва просто удаляем линию.
                        var action = new AddRemoveLineAction(line, this, false);
                        action.Do();

                        if (leftNode != null)
                            if (leftNode.ConnectionCount == 0)
                                leftNode.FullDelete();
                        if (rightNode != null)
                            if (rightNode.ConnectionCount == 0)
                                rightNode.FullDelete();

                        line.FullDelete();
                    }

                    if (this.EditingObject is FigureViewModel)
                    {
                        var figure = this.EditingObject as FigureViewModel;

                        // Сперва просто удаляем фигуру.
                        var action = new AddRemoveFigureAction(figure, this, false);
                        action.Do();

                        figure.FullDelete();
                    }

                    if (this.EditingObject is BadgeViewModel)
                    {
                        var child = this.EditingObject as IChildObjectViewModel;

                        this.EditingObject.IsEditing = false;

                        (child.Parent as IContainerObjectViewModel).FullDeleteChild(child as IObjectViewModel);
                    }
                }
                else
                {
                    var child = this.SelectedObject as IChildObjectViewModel;

                    if (child != null)
                        (child.Parent as IContainerObjectViewModel).FullDeleteChild(child as IObjectViewModel);
                    else
                        if (this.EditingObject is ICustomLayerObject)
                    {
                        var custom = this.EditingObject as ICustomLayerObject;

                        var action = new AddRemoveCustomObjectAction((ICustomLayerObject)this.EditingObject, this, false);
                        action.Do();

                        (custom as IDeletableObjectViewModel).FullDelete();
                    }
                    else
                    {
                        var label = (LabelViewModel)this.EditingObject;

                        // Сперва просто удаляем надпись.
                        var action = new AddRemoveLabelAction(label, this, false);
                        action.Do();

                        label.FullDelete();
                    }
                }
            }
        }

        /// <summary>
        /// Выполняет экспорт отчета о несопоставленных объектах.
        /// </summary>
        private void ExecuteGenerateDiffObjects()
        {
            var waitViewModel = new WaitViewModel("Отчет о несопоставленных объектах", "Пожалуйста подождите, идет формирование отчета о несопоставленных объектах...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.reportService.GenerateDiffObjects(true, this.DataService);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
        }

        /// <summary>
        /// Выполняет экспорт отчета о несопоставленных объектах (только по факту).
        /// </summary>
        private void ExecuteGenerateDiffObjects2()
        {
            var waitViewModel = new WaitViewModel("Отчет о несопоставленных объектах (только по факту)", "Пожалуйста подождите, идет формирование отчета о несопоставленных объектах (только по факту)...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.reportService.GenerateDiffObjects(false, this.DataService);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
        }

        /// <summary>
        /// Выполняет экспорт отчета по проценту сопоставления.
        /// </summary>
        private void ExecuteGenerateIntegrationStats()
        {
            var waitViewModel = new WaitViewModel("Отчет о сопоставлении", "Пожалуйста подождите, идет формирование отчета о сопоставлении...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.reportService.GenerateIntegrationStats(true, this.DataService);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
        }

        /// <summary>
        /// Выполняет экспорт отчета по проценту сопоставления (только по факту).
        /// </summary>
        private void ExecuteGenerateIntegrationStats2()
        {
            var waitViewModel = new WaitViewModel("Отчет о сопоставлении (только по факту)", "Пожалуйста подождите, идет формирование отчета о сопоставлении (только по факту)...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.reportService.GenerateIntegrationStats(false, this.DataService);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
        }

        /// <summary>
        /// Выполняет группировку линий.
        /// </summary>
        private void ExecuteGroupLines()
        {
            // Сперва проверяем, можно ли вообще выполнить группировку линий.
            var isOk = false;
            var layers = this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line && x.ObjectCount > 0).ToList();
            if (layers.Count > 1)
                // Самый первый слой линий должен содержать не больше одной линии, так как все линии более низшего уровня будут сгруппированы с ней.
                if (layers[0].ObjectCount == 1)
                    isOk = true;
                else
                    this.MessageService.ShowMessage("Для группировки необходимо выбрать не больше одной линии типа \"" + layers[0].Type.SingularName + "\"", "Группировка линий", MessageType.Information);
            else
                this.MessageService.ShowMessage("Для группировки необходимо выбрать по крайней мере две линии разного типа", "Группировка линий", MessageType.Information);

            if (isOk)
            {
                // Получаем главную линию.
                var mainLine = (LineViewModel)layers[0].Objects[0];

                // Составляем список линий.
                var lines = new List<LineViewModel>();
                for (int i = 1; i < layers.Count; i++)
                    foreach (LineViewModel line in layers[i].Objects)
                        lines.Add(line);

                // Запоминаем действие в истории изменений и выполняем его.
                var action = new GroupLinesAction(lines, mainLine);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "группировку линий"));
                action.Do();
            }
        }

        /// <summary>
        /// Выполняет экспорт отчета по гидравлике.
        /// </summary>
        private void ExecuteHydraulicsToExcel()
        {
            // Сперва проверяем, выбрана ли котельная.
            var figure = this.SelectedObject as FigureViewModel;
            if (figure == null)
            {
                this.MessageService.ShowMessage("Сперва необходимо выбрать котельную", "Формирование отчета по гидравлике", MessageType.Information);

                return;
            }
            if (!figure.IsBoiler)
            {
                this.MessageService.ShowMessage("Сперва необходимо выбрать котельную", "Формирование отчета по гидравлике", MessageType.Information);

                return;
            }

            // Она должна быть уже сохраненной в базе данных.
            if (!figure.IsSaved)
            {
                this.MessageService.ShowMessage("Нельзя произвести расчет гидравлики для новой котельной", "Расчет гидравлики", MessageType.Information);

                return;
            }

            var waitViewModel = new WaitViewModel("Отчет о гидравлике", "Пожалуйста подождите, идет формирование отчета о гидравлике...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.reportService.GenerateHydraulics(figure.Id, this.CurrentSchema, this.DataService);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
        }

        /// <summary>
        /// Выполняет импорт данных.
        /// </summary>
        private void ExecuteImport()
        {
            if (this.ImportRequested != null)
            {
                var eventArgs = new ImportExportRequestedEventArgs();

                this.ImportRequested(this, eventArgs);

                if (!string.IsNullOrEmpty(eventArgs.FileName))
                {
                    DataSet dataSet = null;

                    using (var stream = File.Open(eventArgs.FileName, FileMode.OpenOrCreate))
                        if (stream.Length > 0)
                        {
                            var formatter = new BinaryFormatter();

                            dataSet = formatter.Deserialize(stream) as DataSet;
                        }

                    if (dataSet != null)
                        if (dataSet.Tables.Count > 0)
                            this.DataService.UpdateLogs(dataSet.Tables[0]);

                    this.MessageService.ShowMessage("Импорт успешно завершен", "Импорт данных", MessageType.Information);
                }
            }
        }

        /// <summary>
        /// Выполняет настройку слоев.
        /// </summary>
        private void ExecuteLayersSettings()
        {
            this.LayersSettingsRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет загрузку данных выбранного населенного пункта.
        /// </summary>
        private void ExecuteLoad()
        {
            this.Load((CityViewModel)this.CitySelectionViewModel.SelectedCity, true);
        }

        /// <summary>
        /// Выполняет управление слоем кастомного объекта.
        /// </summary>
        private void ExecuteManageCustomObjectLayer()
        {
            if (this.CustomLayersViewModel.Layers.Count > 0)
                this.ManageCustomObjectLayer?.Invoke(this, new ManageCustomObjectLayerEventArgs(this.EditingObject as ICustomLayerObject, this.CustomLayersViewModel.Layers.ToList()));
            else
                this.MessageService.ShowMessage("Пользовательские слои не найдены!", "Слои", MessageType.Error);
        }

        /// <summary>
        /// Выполняет вставку объекта.
        /// </summary>
        private void ExecutePaste()
        {
            this.PastingObject = this.CopyingObject.Copy();

            var obj = this.PastingObject as IObjectViewModel;

            var mapObject = obj as IMapObjectViewModel;

            mapObject.RegisterBinding();

            obj.IsInitialized = true;

            mapObject.IsPlaced = true;

            this.PasteRequested?.Invoke(this, new PasteRequestedEventArgs(mapObject));
        }

        /// <summary>
        /// Выполняет прикрепление узлов к фигурам.
        /// </summary>
        public void ExecuteReattachNodes()
        {
            foreach (var layer in this.NodesGroup.Layers)
                foreach (NodeViewModel node in layer.Objects)
                    if (node.ConnectionCount == 1 && node.ConnectedObject == null)
                        node.ConnectToNearestFigure(5);
        }

        /// <summary>
        /// Выполняет сохранение данных.
        /// </summary>
        private void ExecuteSave()
        {
            this.Save1(false);
        }

        /// <summary>
        /// Выполняет сохранение изображения.
        /// </summary>
        private void ExecuteSaveAsImage()
        {
            // Уведомляем о том, что нужно сохранить изображение.
            this.SaveAsImageRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет выбор линий со сломанными узлами.
        /// </summary>
        public void ExecuteSelectBrokenLines()
        {
            var list = new List<IObjectViewModel>();

            foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach (var line in layer.Objects.Cast<LineViewModel>())
                {

               


                    if (line.LeftNode == null || line.RightNode == null)
                        list.Add(line);
                }
            foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach (var line in layer.Objects.Cast<LineViewModel>())
                {
                


                    if (line.LeftNode == null || line.RightNode == null)
                        list.Add(line);
                }



            foreach ( var layer in this.NotPlacedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach ( var line in layer.Objects.Cast<LineViewModel>())
                {
              

                    if (line.LeftNode == null || line.RightNode == null)
                        list.Add(line);
                }



            foreach ( var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                foreach( var line in layer.Objects.Cast<LineViewModel>())
                {
             


                    if (line.LeftNode == null || line.RightNode == null)
                        list.Add(line);
                }

            

            this.SetSelectedObjects(list);
        }

        /// <summary>
        /// Выполняет тестовое действие.
        /// </summary>
        private void ExecuteTest()
        {
#if DEBUG
            // Составляем список сдвигаемых объектов.
            var movingObjects = new List<IMapObjectViewModel>();
            var nodes = new List<IMapObjectViewModel>();
            var delta = new System.Windows.Point(100, 0);
            foreach (var layer in this.selectedObjectsGroup.Layers)
                foreach (IMapObjectViewModel obj in layer.Objects)
                {
                    if (obj is FigureViewModel)
                        movingObjects.Add(obj);

                    if (obj is LineViewModel)
                    {
                        var line = obj as LineViewModel;

                        if (line.LeftNode != null && !nodes.Contains(line.LeftNode))
                            nodes.Add(line.LeftNode);

                        if (line.RightNode != null && !nodes.Contains(line.RightNode))
                            nodes.Add(line.RightNode);
                    }
                }
            movingObjects.AddRange(nodes);

            foreach (var obj in movingObjects)
                obj.Shift(delta);
#else
            // Ничего не делаем.
#endif
        }

        /// <summary>
        /// Выполняет разгруппировку линий.
        /// </summary>
        private void ExecuteUngroupLines()
        {
            // Сперва проверяем, можно ли вообще выполнить разгруппировку линий.
            var isOk = false;
            var layers = this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line && x.ObjectCount > 0).ToList();
            if (layers.Count > 0)
                isOk = true;
            else
                this.MessageService.ShowMessage("Для разгруппировки необходимо выбрать хотя бы одну линию", "Разгруппировка линий", MessageType.Information);

            if (isOk)
            {
                // Составляем список линий и их связей.
                var lines = new List<LineViewModel>();
                var bonds = new Dictionary<LineViewModel, LineViewModel>();
                for (int i = 0; i < layers.Count; i++)
                    foreach (LineViewModel line in layers[i].Objects)
                    {
                        lines.Add(line);

                        bonds.Add(line, line);
                    }

                // Запоминаем действие в истории изменений и выполняем его.
                var action = new GroupLinesAction(lines, bonds);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "разгруппировку линий"));
                action.Do();
            }
        }

        /// <summary>
        /// Загружает данные заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>true - если загрузка выполнена успешно, иначе - false.</returns>
        private bool Load(CityViewModel city, SchemaModel schema = null)
        {

            this.LoadTime = 0;

            // Указываем на то, что данные еще не загружены.
            this.IsDataLoaded = false;

            // Убираем область печати.
            this.MapViewModel.IsPrintAreaVisible = false;

            // Меняем инструмент.
            this.ToolBarViewModel.SelectedTool = Tool.Selector;

            // Очищаем историю.
            this.HistoryService.Clear();

            this.DeselectAll();

            // Очищаем слои.
            this.AllObjectsGroup.ClearLayerData();
            this.NotPlacedObjectsGroup.ClearLayerData();
            this.PlanningObjectsGroup.ClearLayerData();
            this.NodesGroup.ClearLayerData();
            this.DisabledObjectsGroup.ClearLayerData();

            this.ClearSelectedGroup();

            this.UpdatingObjects.Clear();

            // Очищаем список объектов, подлежащих удалению.
            this.DeletingObjects.Clear();

            // Убираем пользовательские объекты.
            this.ClearCustomObjects();

            // Убираем линии со слоя гидравлики и слоя разделения по годам.
            if (this.MapViewModel.IsHydraulicsVisible)
                this.MapViewModel.ShowHideHydraulicsCommand.Execute(null);
            this.Hydraulics.UnsetLines();
            this.HydraulicsError.UnsetLines();
            if (this.MapViewModel.IsYearDiffVisible)
                this.MapViewModel.ShowHideYearDiffCommand.Execute(null);
            this.YearDiff.UnsetLines();
            if (this.MapViewModel.IsRPVisible)
                this.MapViewModel.ShowHideRPCommand.Execute(null);
            this.RP.UnsetLines();
            if (this.MapViewModel.IsUOVisible)
                this.MapViewModel.ShowHideUOCommand.Execute(null);
            this.UO.UnsetFigures();

            if (this.MapViewModel.IsIJSVisibleT)
                this.MapViewModel.ShowHideIJSTCommand.Execute(null);
            if (this.MapViewModel.IsIJSVisibleF)
                this.MapViewModel.ShowHideIJSFCommand.Execute(null);

            this.IJS.UnsetFigures();
            this.IJSF.UnsetFigures();

            // Убиваем кастомные слои.
            this.CustomLayersViewModel.Terminate();

            // Убираем привязки.
            this.mapBindingService.UnregisterBindings();

            // Уведомляем о том, что карта закрыта.
            this.MapClosed?.Invoke(this, EventArgs.Empty);

            // Убираем подложку.
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                this.MapViewModel.SubstrateTiles = null;
                this.MapViewModel.SubstrateDimension = System.Windows.Size.Empty;
                this.MapViewModel.SubstrateSize = new System.Windows.Size(0, 0);
            }));

            // Работаем со схемой.
            if (schema == null)
            {
                // Запрашиваем схему.
                var eventArgs = new SchemaRequestedEventArgs(city);
                this.SchemaRequested.Invoke(this, eventArgs);
                this.CurrentSchema = eventArgs.Schema;
                this.LoadedBoilers = eventArgs.BoilerIds;
            }
            else
            {
                this.CurrentSchema = schema;
                this.LoadedBoilers = new List<Guid>();
            }
            this.isCustomBoilersLoaded = this.LoadedBoilers.Count > 0;
            if (this.CurrentSchema == null)
                return false;

            this.ToolBarViewModel.IsDrawPlanning = true;
            this.ToolBarViewModel.UpdateState();

            double scale = 0;

            var figures = new List<FigureModel>();
            var lines = new List<LineModel>();
            var nodes = new List<NodeModel>();
            var labels = new List<LabelModel>();
            var tables = new List<LengthPerDiameterTableModel>();
            var headers = new List<ApprovedHeaderModel>();

            // Списки идентификаторов линий, которые относятся к слою гидравлики и слою разделения по годам.
            var hydraulicsLines = new List<Guid>();
            var hydraulicsErrorLines = new List<Guid>();
            var thisYearLines = new List<Guid>();
            var lastYearLines = new List<Guid>();
            var rp = new List<Guid>();
            var uo = new List<Guid>();
            var disableObjects = new List<Guid>();
            var ijs_t = new List<Guid>();
            var ijs_f = new List<Guid>();

            DataSet badges = new DataSet();

            var startTime = DateTime.Now;

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
                    this.DataService.UpdateTables(city.Id);

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
                    if (this.isCustomBoilersLoaded)
                    {
                        figures = city.GetFigures(this.DataService, this.LoadedBoilers, this.CurrentSchema);
                        BinaryFormatter bin = new BinaryFormatter();
                        MemoryStream memStream = new MemoryStream();
                        bin.Serialize(memStream, figures);

                        memStream.Close();
                        
                        //bin.Serialize()
                        lines = city.GetLines(this.DataService, this.LoadedBoilers, this.CurrentSchema);
                        nodes = city.GetNodes(this.DataService, this.LoadedBoilers, this.CurrentSchema);
                    }
                    else
                    {
                        figures = city.GetFigures(this.CurrentSchema, this.DataService);

                        BinaryFormatter bin = new BinaryFormatter();
                        MemoryStream memStream = new MemoryStream();
                        bin.Serialize(memStream, figures);

                        memStream.Close();

                        lines = city.GetLines(this.CurrentSchema, this.DataService);
                        nodes = city.GetNodes(this.CurrentSchema, this.DataService);
                    }
                    labels = city.GetLabels(this.CurrentSchema, this.DataService);
                    tables = city.GetTables(this.CurrentSchema, this.DataService);
                    headers = city.GetApprovedHeader(this.CurrentSchema, this.DataService);
                    hydraulicsLines = this.DataService.LineAccessService.GetHydraulicsLines(city.Id, this.CurrentSchema);
                    hydraulicsErrorLines = this.DataService.LineAccessService.GetHydraulicsErrorLines(city.Id, this.CurrentSchema);
                    thisYearLines = this.DataService.LineAccessService.GetLinesByYear(city.Id, this.CurrentSchema, DateTime.Now.Year);
                    lastYearLines = this.DataService.LineAccessService.GetLinesByYear(city.Id, this.CurrentSchema, DateTime.Now.Year - 1);
                    rp = this.DataService.LineAccessService.GetRP(city.Id, this.CurrentSchema);
                    uo = this.DataService.FigureAccessService.GetUO(city.Id, this.CurrentSchema);

                    ijs_t = this.DataService.FigureAccessService.GetIjs(city.Id, this.CurrentSchema, 0);
                    ijs_f = this.DataService.FigureAccessService.GetIjs(city.Id, this.CurrentSchema, 1);

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

            // Инициализируем кастомные слои. Это нужно сделать обязательно до начала размещения надписей.
            this.CustomLayersViewModel.Init(this.CurrentSchema, city);

            // Добавляем объекты на карту.
            this.LoadFigures(figures);
            this.LoadLines(lines);
            this.LoadNodes(nodes);
            this.LoadLabels(labels);
            this.LoadTables(tables);
            this.LoadHeaders(headers);

            this.Hydraulics.SetLines(hydraulicsLines, this);
            this.HydraulicsError.SetLines(hydraulicsErrorLines, this);
            this.YearDiff.SetLines(thisYearLines, lastYearLines, this);
            this.RP.SetLines(rp, this);
            this.UO.SetFigures(uo, this);
            /*
            foreach ( var item in DisabledObjectsGroup.Layers)
            {
                foreach ( var obj in item.Objects)
                {
                    
                    disableObjects.Add(obj.Id);
                }
            }
            this.disableObjectViewModel.SetFigures(disableObjects, this);

            */
            this.IJS.SetFigures(ijs_t, this);
            this.IJSF.SetFigures(ijs_f, this);

            // Добавляем значки линий.
            var lineTypes = this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Line);
            foreach (var layer in this.AllObjectsGroup.Layers.Where(x => lineTypes.Contains(x.Type)))
                foreach (LineViewModel line in layer.Objects)//3198
                    line.LoadBadges(badges);
            foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => lineTypes.Contains(x.Type)))
                foreach (LineViewModel line in layer.Objects)
                    line.LoadBadges(badges);
            foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => lineTypes.Contains(x.Type)))
                foreach (LineViewModel line in layer.Objects)
                    line.LoadBadges(badges);

            this.CitySelectionViewModel.LoadedCity = city;

            if (this.isCustomBoilersLoaded)
                this.LoadedCityName = city.Name + " - Неполная схема - " + this.CurrentSchema.Name;
            else
                this.LoadedCityName = city.Name + " - " + this.CurrentSchema.Name;
            if (BaseSqlDataAccessService.loadAllObjectFlag)
            {

                foreach (var layer in this.AllObjectsGroup.Layers)
                {
                    foreach (var obj in layer.Objects)
                    {
                        var itemObject = obj as IParameterizedObjectViewModel;
                        if (itemObject != null)
                            this.ParameterGridViewModel.LoadObjectData(itemObject);
                    }
                }

                foreach (var layer in this.PlanningObjectsGroup.Layers)
                {
                    foreach (var obj in layer.Objects)
                    {
                        var itemObject = obj as IParameterizedObjectViewModel;
                        if (itemObject != null)
                            this.ParameterGridViewModel.LoadObjectData(itemObject);
                    }
                }

                foreach (var layer in this.DisabledObjectsGroup.Layers)
                {
                    foreach (var obj in layer.Objects)
                    {
                        var itemObject = obj as IParameterizedObjectViewModel;
                        if (itemObject != null)
                            this.ParameterGridViewModel.LoadObjectData(itemObject);
                    }
                }

                foreach (var item in CustomObjects)
                {
                    //var notPlacedObject = this.SelectedNotPlacedObject as IParameterizedObjectViewModel
                    //await this.ParameterGridViewModel.LoadObjectDataAsync(notPlacedObject);

                    var itemObject = item as IParameterizedObjectViewModel;
                    if (itemObject != null)
                        this.ParameterGridViewModel.LoadObjectData(itemObject);
                }
            }
            this.IsDataLoaded = true;

            this.ShowLocalMapRequested?.Invoke(this, EventArgs.Empty);

            this.MapLoaded?.Invoke(this, EventArgs.Empty);

            this.LoadTime = Math.Round((DateTime.Now - startTime).TotalMilliseconds / 1000, 2);

            return true;

        }

        /// <summary>
        /// Выполняет загрузку заданного населенного пункта с учетом или без учета необходимости сохранения.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <param name="checkSave">true, если нужно проверить перед открытием необходимость сохранения, иначе - false.</param>
        private void Load(CityViewModel city, bool checkSave)
        {
            // Закрываем представления ошибок.
            this.ErrorsViewModel.Close();
            this.SavedErrorsViewModel.Close();

            // Закрываем представление узлов поворота и неподключенных узлов.
            this.BendNodesViewModel.Close();
            this.FreeNodesViewModel.Close();

            this.ObjectListViewModel.Close();
            this.JurKvpCompletedListViewModel.Close();

            FormGenerator.CloseAllForms();

            if (checkSave)
                // Сперва необходимо проверить наличие необходимости сохранения изменений.
                if (this.SaveCommand.CanExecute(null))
                {
                    var result = this.MessageService.ShowYesNoCancelMessage("Сохранить все внесенные изменения?", "Загрузка данных");

                    if (!result.HasValue)
                        return;
                    else
                        if (result.Value)
                    {
                        this.SaveCommand.Execute(null);

                        // Еще раз выполняем проверку необходимости сохранения, так как при сохранении могли возникнуть ошибки.
                        if (this.SaveCommand.CanExecute(null))
                            // И если возникли ошибки, то необходимо отменить дальнейшую загрузку данных.
                            return;
                    }
                }

            this.Load(city);
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
            var disabledFigured = new Dictionary<ObjectType, List<IObjectViewModel>>();

            FigureViewModel figureViewModel = null;

            foreach (var figure in figures)
            {
                switch (figure.FigureType)
                {
                    case FigureType.Ellipse:
                        figureViewModel = new EllipseViewModel(figure, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                        break;

                    case FigureType.Polygon:
                        figureViewModel = new PolygonViewModel(figure, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                        break;

                    case FigureType.Rectangle:
                        figureViewModel = new RectangleViewModel(figure, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                        break;

                    case FigureType.None:
                        // Пусть в том случае, когда объект является неразмещенным, ону будет представляться прямоугольником.
                        figureViewModel = new RectangleViewModel(figure, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                        break;
                }

                // Добавляем объект в соответствующий ему слой.
                if (figure.FigureType != FigureType.None)
                {
                    figureViewModel.IsInitialized = true;
                    figureViewModel.IsPlaced = true;

                    if (!figure.IsActive)
                    {
                        if (!disabledFigured.ContainsKey(figure.Type))
                            disabledFigured.Add(figure.Type, new List<IObjectViewModel>());

                        disabledFigured[figure.Type].Add(figureViewModel);
                    }
                    else
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
            {
                foreach (var layerItem in AllObjectsGroup.Layers)
                {
                    if (layerItem.Type == entry.Key)
                    {
                    }

                    if (layerItem.Type.Equals(entry.Key))
                    {
                    }
                }

                //this.AllObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
                this.AllObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
                //--exec gis..update_substrate @city_id = 535, @width = 3508, @height = 4961, @column_count = 7, @row_count = 10
            }
            if (this.CurrentSchema.IsActual && this.ToolBarViewModel.CanDrawFigures)
                foreach (var entry in notPlacedFigures)
                    this.NotPlacedObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
                    //this.NotPlacedObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
            foreach (var entry in planningFigures)
                this.PlanningObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
                //this.PlanningObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
            foreach (var entry in disabledFigured)
            {
                this.DisabledObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
                //this.DisabledObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
            }
        }

        /// <summary>
        /// Загружает заголовки.
        /// </summary>
        /// <param name="headers">Заголовки.</param>
        private void LoadHeaders(List<ApprovedHeaderModel> headers)
        {
            foreach (var header in headers)
            {
                var action = new AddRemoveCustomObjectAction(new ApprovedHeaderViewModel(header, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService), this, true);

                action.Do();
            }
        }

        /// <summary>
        /// Загружает надписи.
        /// </summary>
        /// <param name="labels">Надписи.</param>
        private void LoadLabels(List<LabelModel> labels)
        {
            foreach (var label in labels)
            {
                var action = new AddRemoveLabelAction(new LabelViewModel(label, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService), this, true);

                action.Do();
            }
        }

        /// <summary>
        /// Загружает линии.
        /// </summary>
        /// <param name="lines">Линии.</param>
        private void LoadLines(List<LineModel> lines)
        {
            var allLines = new Dictionary<ObjectType, List<IObjectViewModel>>();
            var planningLines = new Dictionary<ObjectType, List<IObjectViewModel>>();
            var disabledLines = new Dictionary<ObjectType, List<IObjectViewModel>>();

            LineViewModel lineViewModel = null;

            foreach (var line in lines)
            {
                lineViewModel = new LineViewModel(line, this, this.MapViewModel, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true,
                    IsPlaced = true,
                };

                if (!line.IsActive)
                {
                    if (!disabledLines.ContainsKey(line.Type))
                        disabledLines.Add(line.Type, new List<IObjectViewModel>());

                    disabledLines[line.Type].Add(lineViewModel);
                }
                else
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
                //this.AllObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
                this.AllObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            foreach (var entry in planningLines)
                //this.PlanningObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
                this.PlanningObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            foreach (var entry in disabledLines)
                //this.DisabledObjectsGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
                this.DisabledObjectsGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);

            // Восстанавливаем их группировку.
            foreach (var entry in allLines)
                foreach (LineViewModel line in entry.Value)
                    line.RestoreGrouping();
            foreach (var entry in planningLines)
                foreach (LineViewModel line in entry.Value)
                    line.RestoreGrouping();
            foreach (var entry in disabledLines)
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
                nodeViewModel = new NodeViewModel(node, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsModified = false
                };

                // Восстанавливаем объект, к которому присоединен узел.
                nodeViewModel.RestoreConnectedObject();

                // Восстанавливаем соединения с узлом.
#warning Ранее только режим открытия отдельной котельной игнорил ошибки в коннекшенах
                //nodeViewModel.RestoreConnections(this.isCustomBoilersLoaded);
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
                this.NodesGroup.Layers.First(x => x.Type.Equals(entry.Key)).AddRange(entry.Value);
            //this.NodesGroup.Layers.First(x => x.Type == entry.Key).AddRange(entry.Value);
        }

        /// <summary>
        /// Загружает подложку карты.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Значение, указывающее на то, что загружена ли подложка.</returns>
        private bool LoadSubstrate(CityViewModel city)
        {
            
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
                    {
                        if (!this.substrateService.CacheImages(substrate, this.DataService.SubstrateFolderName))
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => this.MessageService.ShowMessage("Не удалось выполнить кеширование подложки", "Загрузка подложки", MessageType.Error)));

                            return false;
                        }
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

        /// <summary>
        /// Загружает таблицы.
        /// </summary>
        /// <param name="tables">Таблицы.</param>
        private void LoadTables(List<LengthPerDiameterTableModel> tables)
        {
            foreach (var table in tables)
            {
                var action = new AddRemoveCustomObjectAction(new LengthPerDiameterTableViewModel(table, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService), this, true);

                action.Do();
            }
        }

        /// <summary>
        /// Управляет блокировкой панели инструментов.
        /// </summary>
        private void RefreshToolBar()
        {
            if (this.mapBindingService.Scale < 1 && this.MapViewModel.AutoHideNodes || this.isCustomBoilersLoaded)
            {
                if (this.ToolBarViewModel.IsToolBarEnabled)
                {
                    if (this.EditingObject != null)
                        this.EditingObject.IsEditing = false;

                    this.prevTool = this.ToolBarViewModel.SelectedTool;

                    this.ToolBarViewModel.SelectedTool = Tool.Selector;

                    this.ToolBarViewModel.IsToolBarEnabled = false;
                }
            }
            else
                if (!this.ToolBarViewModel.IsToolBarEnabled)
            {
                this.ToolBarViewModel.IsToolBarEnabled = true;

                this.ToolBarViewModel.SelectedTool = this.prevTool;
            }
        }

        /// <summary>
        /// Удаляет узел.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="line">Линия, которая присоединена к узлу.</param>
        private void RemoveNode(NodeViewModel node, LineViewModel line)
        {
            if (node == null)
                return;

            if (node.ConnectionCount > 1)
                // Если узел имеет другие соединения, то мы убираем соединение только с заданной линией.
                node.RemoveConnection(line);
            else
            {
                // Иначе, также удаляем и сам узел.
                node.RemoveConnection(line);
                this.RemoveObject(node);

                // Если узел уже существует в источнике данных, то помечаем его на обновление.
                if (node.IsSaved)
                    this.MarkToUpdate(node);
            }
        }



        /// <summary>
        /// Выполняет сохранение изменений.
        /// </summary>
        /// <param name="isSilent">Значение, указывающее на то, что является ли сохранение скрытным.</param>
        /// <returns>true, если сохранение удалось выполнить, иначе - false.</returns>
        private bool Save1(bool isSilent)
        {
            this.DeselectAll();

            // Закрываем представления ошибок.
            this.ErrorsViewModel.Close();
            this.SavedErrorsViewModel.Close();

            // Закрываем представление узлов поворота и неподключенных узлов.
            this.BendNodesViewModel.Close();
            this.FreeNodesViewModel.Close();

            this.ObjectListViewModel.Close();
            this.JurKvpCompletedListViewModel.Close();

            FormGenerator.CloseAllForms();

            if (!isSilent)
                if (this.CheckForErrors())
                    if (this.ErrorFound != null)
                    {
                        this.ErrorsViewModel.Title = "Сохранение";
                        this.ErrorsViewModel.Content = "Найдены незаполненные значения обязательных параметров. Пожалуйста, заполните значения заданных параметров:";

                        this.ErrorFound(this, EventArgs.Empty);
                    }


            if (DataService.MapAccessService.testConnection("Не ждать, сохранить локально"))
            {
                this.CitySelectionViewModel.LoadedCity.UpdateScale(this.MapViewModel.Scale, this.DataService);
            }
            else
            {// connection faild
                BaseSqlDataAccessService.localModeFlag = true;// режим сохранения на диск
                //System.Windows.MessageBox.Show("connection faild");
            }


            // Результат сохранения.
            var result = false;

            var waitViewModel = new WaitViewModel("Сохранение данных", "Пожалуйста подождите, идет сохранение данных...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {

                    this.DataService.BeginSaveTransaction();

                    // Сперва обновляем объекты, которые нужно обновить раньше остальных.
                    foreach (IObjectViewModel obj in this.UpdatingObjects)
                    {
                        /*
                        BinaryFormatter bin = new BinaryFormatter();
                        MemoryStream memStream = new MemoryStream();
                        bin.Serialize(memStream, obj);

                        memStream.Close();*/
                        obj.BeginSave();
                        
                    }

                
                    // Потом сохраняем фигуры.
                    foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                        foreach (var obj in layer.Objects)
                        {
                            obj.BeginSave();
                       

                        }
                    foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                        foreach (var obj in layer.Objects)
                            obj.BeginSave();
                    foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                        foreach (var obj in layer.Objects)
                            obj.BeginSave();

                    // Затем сохраняем линии.
                    foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                        foreach (var obj in layer.Objects)
                            obj.BeginSave();
                    foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                        foreach (var obj in layer.Objects)
                            obj.BeginSave();
                    foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                        foreach (var obj in layer.Objects)
                            obj.BeginSave();

                    // Узлы.
                    foreach (var layer in this.NodesGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Node))
                        foreach (var obj in layer.Objects)
                            obj.BeginSave();

                    // Сохраняем изменения в кастомных слоях до кастомных объектов, это важно.
                    this.CustomLayersViewModel.BeginApplyChanges();

                    // И кастомные объекты.
                    foreach (var obj in this.CustomObjects)
                        obj.BeginSave(this.DataService);

                    // Затем удаляем объекты, подлежащие удалению.
                    foreach (var obj in this.DeletingObjects)
                        obj.MarkFullDelete();

                    result = this.DataService.EndSaveTransaction();



                    if (result)
                    {
                        foreach (IObjectViewModel obj in this.UpdatingObjects)
                            obj.EndSave();

                        // Завершаем сохранения.
                        foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var layer in this.NodesGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Node))
                            foreach (var obj in layer.Objects)
                                obj.EndSave();
                        foreach (var obj in this.CustomObjects)
                            obj.EndSave();

                        this.UpdatingObjects.Clear();

                        // Очищаем список объектов, подлежащих к удалению.
                        this.DeletingObjects.Clear();

                        this.CustomLayersViewModel.EndApplyChanges();
                        //if (!BaseSqlDataAccessService.localModeFlag)
                        if (DataService.MapAccessService.testConnection("",true))
                        {

                            

                            // Подготавливаем схему к работе.
                            this.DataService.PrepareSchema(this.CurrentSchema.Id, this.CitySelectionViewModel.LoadedCity.Id);

                            // Обновляем справочники.
                            this.DataService.UpdateTables(this.CitySelectionViewModel.LoadedCity.Id, null, this.CurrentSchema, LoadLevel.AfterChange);
                        }
                    }
                    else
                    {
                        foreach (IObjectViewModel obj in this.UpdatingObjects)
                            obj.RevertSave();

                        // Отменяем сохранения.
                        foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var layer in this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var layer in this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var layer in this.NodesGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Node))
                            foreach (var obj in layer.Objects)
                                obj.RevertSave();
                        foreach (var obj in this.CustomObjects)
                            obj.RevertSave();

                        this.CustomLayersViewModel.RevertApplyChanges();
                    }
                    
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));

            if (result)
            {
                // Очищаем историю.
                this.HistoryService.Clear();

                if (!isSilent)
                    this.MessageService.ShowMessage("Сохранение завершено", "Сохранение", MessageType.Information);

                return true;
            }
            else
            {
                if (!isSilent)
                    this.MessageService.ShowMessage("Не удалось выполнить сохранение. Пожалуйста свяжитесь с разработчиками приложения для устранения ошибки.", "Сохранение", MessageType.Error);

                return false;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет граничную линию.
        /// </summary>
        /// <param name="line">Линия.</param>
        public void AddBoundaryLine(LineViewModel line)
        {
            if (this.boundaryLines.Contains(line))
                return;

            this.boundaryLines.Add(line);
        }

        /// <summary>
        /// Добавляет заданный пользовательский объект.
        /// </summary>
        /// <param name="customLayerObject">Добавляемый объект.</param>
        public void AddCustomLayerObject(ICustomLayerObject customLayerObject)
        {
            this.CustomObjects.Add(customLayerObject);
        }

        /// <summary>
        /// Добавляет заданную надпись.
        /// </summary>
        /// <param name="label">Надпись.</param>
        public void AddLabel(LabelViewModel label)
        {
            this.CustomObjects.Add(label);
        }

        /// <summary>
        /// Добавляет или убирает из группы выбранных объектов заданный объект.
        /// </summary>
        /// <param name="obj">Добавляемый/убираемый объект.</param>
        public void AddOrRemoveSelectedObject(IObjectViewModel obj)
        {
            this.selectedObjectsGroup.IsSelected = false;

            // Проверяем, выбран ли уже добавляемый объект.
            var layer = this.selectedObjectsGroup.Layers.First(x => x.Type == obj.Type);
            if (layer.Contains(obj))
                // Если содержит, то убираем его.
                layer.Remove(obj);
            else
                // Иначе, добавляем.
                layer.Add(obj);

            this.selectedObjectsGroup.IsSelected = true;
        }

        /// <summary>
        /// Добавляет/удаляет линию.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="isAdding">Значение, указывающее на то, что добавляется ли линия.</param>
        public void AddRemoveLine(LineViewModel line, bool isAdding)
        {
            // Убираем со всего выбор.
            this.DeselectAll();

            // Очищаем группу выбранных объектов.
            this.ClearSelectedGroup();

            if (isAdding)
                this.AddObject(line);
            else
                this.RemoveObject(line);
        }

        /// <summary>
        /// Добавляет/удаляет узлы.
        /// </summary>
        /// <param name="line">Линия, которая присоединена к узлам.</param>
        /// <param name="leftNode">Узел, к которому линия присоединена левой стороной.</param>
        /// <param name="rightNode">Узел, к которому линия присоединена правой стороной.</param>
        /// <param name="isAdding">Значение, указывающее на то, что добавляются ли узлы.</param>
        public void AddRemoveNodes(LineViewModel line, NodeViewModel leftNode, NodeViewModel rightNode, bool isAdding)
        {
            if (isAdding)
            {
                this.AddNode(leftNode, line, NodeConnectionSide.Left);
                this.AddNode(rightNode, line, NodeConnectionSide.Right);
            }
            else
            {
                this.RemoveNode(leftNode, line);
                this.RemoveNode(rightNode, line);
            }
        }

        /// <summary>
        /// Добавляет в группу выбранных объектов заданный объект.
        /// </summary>
        /// <param name="obj">Добавляемый объект.</param>
        public void AddSelectedObject(IObjectViewModel obj)
        {
            this.selectedObjectsGroup.IsSelected = false;

            // Проверяем, выбран ли уже добавляемый объект.
            var layer = this.selectedObjectsGroup.Layers.First(x => x.Type == obj.Type);
            if (!layer.Contains(obj))
                layer.Add(obj);

            this.selectedObjectsGroup.IsSelected = true;
        }

        /// <summary>
        /// Добавляет в группу выбранных объектов заданные объекты.
        /// </summary>
        /// <param name="objects">Добавляемые объекты.</param>
        public void AddSelectedObjects(List<IObjectViewModel> objects)
        {
            this.selectedObjectsGroup.IsSelected = false;

            foreach (var obj in objects)
            {
                // Проверяем, выбран ли уже добавляемый объект.
                var layer = this.selectedObjectsGroup.Layers.First(x => x.Type == obj.Type);
                if (!layer.Contains(obj))
                    layer.Add(obj);
            }

            this.selectedObjectsGroup.IsSelected = true;
        }

        /// <summary>
        /// Отменяет рисование объекта.
        /// </summary>
        public void CancelDraw()
        {
            if (this.drawingObject == null)
                // Когда мы заканчиваем рисовать неразмещенный объект, мы меняем выбранный инструмент, что приводит к повторному вызову данного метода, причем рисуемый объект пустой. Поэтому нужно выйти из метода.
                return;

            // Если отмененный объект - полилайн, то убираем все его параллельные полилайны.
            var polyline = this.drawingObject as PolylineViewModel;
            if (polyline != null)
                foreach (var item in polyline.ParallelPolylines)
                {
                    item.IsPlaced = false;

                    item.UnregisterBinding();
                }

            this.drawingObject.IsPlaced = false;

            this.drawingObject.UnregisterBinding();

            this.drawingObject = null;
        }

        /// <summary>
        /// Отменяет вставку объекта.
        /// </summary>
        public void CancelPaste()
        {
            var obj = this.PastingObject as IObjectViewModel;

            var mapObject = obj as IMapObjectViewModel;

            mapObject.IsPlaced = false;

            obj.IsInitialized = false;

            mapObject.UnregisterBinding();

            this.PastingObject = null;
        }

        /// <summary>
        /// Очищает список пользовательских объектов.
        /// </summary>
        public void ClearCustomObjects()
        {
            foreach (var obj in this.CustomObjects)
            {
                obj.IsEditing = false;
                obj.IsPlaced = false;
            }

            this.CustomObjects.Clear();
        }

        /// <summary>
        /// Завершает рисование объекта.
        /// </summary>
        public void CompleteDraw()
        {
            var temp = this.drawingObject;

            this.drawingObject = null;

            var obj = temp as IObjectViewModel;

            if (obj != null)
            {
                // Если нарисована фигура, то указываем, что больше не рисуем ее.
                var figure = obj as FigureViewModel;
                if (figure != null)
                    figure.IsDrawing = false;

                obj.IsInitialized = true;

                if (!obj.IsActive)
                    this.DisabledObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
                else
                    if (!obj.IsPlanning)
                    this.AllObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
                else
                    this.PlanningObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);

                this.HistoryService.Add(new HistoryEntry(new AddRemoveFigureAction((FigureViewModel)obj, this, true), Target.Data, "добавление фигуры"));

                if (this.SelectedNotPlacedObject != null)
                {
                    var layer = this.NotPlacedObjectsGroup.Layers.First(x => x.Type == obj.Type);

                    layer.Remove(layer.GetObjectById(obj.Id));
                }
            }
            else
            {
                var polyline = temp as PolylineViewModel;

                if (polyline != null)
                {
                    // Получаем общую толщину полилайна.
                    var thickness = polyline.TotalThickness;
                    
                    var lines = new List<LineViewModel>();
                    
                    // Находим минимальное количество точек в линиях пучка.
                    var minCount = polyline.Points.Count;
                    foreach (var item in polyline.ParallelPolylines)
                        if (item.Points.Count < minCount)
                            minCount = item.Points.Count;

                    // Определяем отступ для главной линии.
                    var offset = this.mapBindingService.MapSettingService.LineThickness;
                    switch (polyline.ParallelPolylines.Count)
                    {
                        case 2:
                            offset = this.mapBindingService.MapSettingService.LineThickness * 7;

                            break;

                        case 3:
                            offset = this.mapBindingService.MapSettingService.LineThickness * 10;

                            break;
                    }

                    lines.Add(this.ReplacePolyline(polyline, minCount, offset));

                    // Определяем отступ для боковых линий.
                    switch (polyline.ParallelPolylines.Count)
                    {
                        case 1:
                            offset = -this.mapBindingService.MapSettingService.LineThickness * 5;

                            break;

                        case 2:
                            offset = this.mapBindingService.MapSettingService.LineThickness * 7;

                            break;

                        case 3:
                            offset = this.mapBindingService.MapSettingService.LineThickness * 10;

                            break;

                        default:
                            offset = this.mapBindingService.MapSettingService.LineThickness;

                            break;
                    }

                    foreach (var item in polyline.ParallelPolylines)
                        lines.Add(this.ReplacePolyline(item, minCount, offset, lines[0].Length));

                    // Группируем полученные линии.
                    for (int i = 1; i < lines.Count; i++)
                        lines[i].GroupWith(lines[0]);

                    // Добавляем в историю изменений добавление линий и узлов.
                    this.HistoryService.Add(new HistoryEntry(new AddRemoveLineAction(lines, this, true), Target.Data, "добавление линии(й)"));

                    // Проверяем попадания левых узлов на объекты.
                    foreach (var item in lines)
                        item.LeftNode.ConnectToNearest(polyline.ParallelPolylines.Count > 0 ? thickness : 0.5);

                    // Проверяем попадания правых узлов на объекты.
                    foreach (var item in lines)
                        item.RightNode.ConnectToNearest(polyline.ParallelPolylines.Count > 0 ? thickness : 0.5);
                }
                else
                {
                    var ruler = temp as RulerViewModel;

                    if (ruler != null)
                    {
                        // Запрашиваем новое значение масштаба линейки.
                        if (this.ValueInputRequested != null)
                        {
                            var eventArgs = new ValueInputRequestedEventArgs("Введите длину в метрах:", "Линейка", typeof(double), Math.Round(ruler.Length * this.MapViewModel.Scale, 2), this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Line).ToList());

                            this.ValueInputRequested(this, eventArgs);

                            if (eventArgs.Result != null)
                            {
                                var newScale = Math.Round((double)eventArgs.Result / ruler.Length, 2);

                                // Запоминаем действие в истории изменений и выполняем его.
                                var action = new ChangeScaleAction(this.MapViewModel.Scale, newScale, eventArgs.Option, this, eventArgs.LineType);
                                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение масштаба линий"));
                                action.Do();
                            }
                        }
                        else
                            throw new NullReferenceException(nameof(this.ValueInputRequested));

                        ruler.IsPlaced = false;
                    }
                    else
                    {
                        var newRuler = temp as NewRulerViewModel;

                        if (newRuler != null)
                            newRuler.IsPlaced = false;
                    }
                }
            }
        }

        /// <summary>
        /// Завершает вставку объекта.
        /// </summary>
        public void CompletePaste()
        {
            var obj = this.PastingObject as IObjectViewModel;

            if (!obj.IsActive)
                this.DisabledObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
            else
                if (!obj.IsPlanning)
                this.AllObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
            else
                this.PlanningObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);

            // Если вставленный объект - линия, то нужно добавить ее узлы.
            if (obj is LineViewModel)
            {
                var line = obj as LineViewModel;

                // Сперва добавляем левый узел.
                var nodeModel = new NodeModel(ObjectModel.DefaultId, line.CityId, line.Type, new Point(line.LeftPoint.X, line.LeftPoint.Y), null, null, false);
                var nodeViewModel = new NodeViewModel(nodeModel, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };
                this.NodesGroup.Layers.First(x => x.Type.ObjectKind == ObjectKind.Node).Add(nodeViewModel);
                // Присоединяем к нему вставленную линию.
                nodeViewModel.AddConnection(new NodeConnection(line, NodeConnectionSide.Left));
                // Добавляем узел на карту.
                nodeViewModel.IsPlaced = true;

                // Затем правый.
                nodeModel = new NodeModel(ObjectModel.DefaultId, line.CityId, line.Type, new Point(line.RightPoint.X, line.RightPoint.Y), null, null, false);
                nodeViewModel = new NodeViewModel(nodeModel, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };
                this.NodesGroup.Layers.First(x => x.Type.ObjectKind == ObjectKind.Node).Add(nodeViewModel);
                // Присоединяем к нему вставленную линию.
                nodeViewModel.AddConnection(new NodeConnection(line, NodeConnectionSide.Right));
                // Добавляем узел на карту.
                nodeViewModel.IsPlaced = true;
            }

            this.HistoryService.Add(new HistoryEntry(new PasteObjectAction(this.PastingObject, this), Target.Data, "вставку объекта"));

            (this.PastingObject as IEditableObjectViewModel).IsEditing = true;

            this.PastingObject = null;
        }

        /// <summary>
        /// Убирает выбор с выбранного и редактирование с редактируемого объекта.
        /// </summary>
        public void DeselectAll()
        {
            if (this.SelectedGroup != null)
                this.SelectedGroup.IsSelected = false;

            if (this.SelectedLayer != null)
                this.SelectedLayer.IsSelected = false;

            if (this.SelectedNotPlacedObject != null)
                this.SelectedNotPlacedObject.IsSelected = false;

            if (this.SelectedObject != null)
                this.SelectedObject.IsSelected = false;

            if (this.EditingObject != null)
                this.EditingObject.IsEditing = false;
        }

        /// <summary>
        /// Выполняет поиск пути от заданной линии к граничным линиям.
        /// </summary>
        /// <param name="line">Линия.</param>
        public void FindWayToOtherLines(LineViewModel line)
        {
            if (this.boundaryLines.Count > 0)
                foreach (var boundaryLine in this.boundaryLines)
                {
                    var ds = new Dictionary<LineViewModel, int>();

                    ds.Add(line, 0);

                    var d = 0;

                    var marked = false;
                    var isFound = false;

                    // Выполняем поиск граничной линии.
                    while (true)
                    {
                        marked = false;

                        var cds = ds.Where(x => x.Value == d).ToList();

                        foreach (var l in cds)
                        {
                            // Проверяем левый узел.
                            foreach (var x in l.Key.LeftNode.ConnectedLines)
                                if (!ds.ContainsKey(x))
                                {
                                    ds.Add(x, d + 1);

                                    if (x == boundaryLine)
                                        isFound = true;

                                    marked = true;
                                }

                            // Проверяем правый узел.
                            foreach (var x in l.Key.RightNode.ConnectedLines)
                                if (!ds.ContainsKey(x))
                                {
                                    ds.Add(x, d + 1);

                                    if (x == boundaryLine)
                                        isFound = true;

                                    marked = true;
                                }
                        }

                        d++;

                        if (isFound || !marked)
                            break;
                    }

                    // Составляем обратный путь.
                    var path = new List<IObjectViewModel>();
                    if (isFound)
                    {
                        d = ds[boundaryLine];

                        var rds = new Dictionary<LineViewModel, int>();

                        rds.Add(boundaryLine, d);

                        while (d > 0)
                        {
                            var cds = rds.Where(x => x.Value == d).ToList();

                            foreach (var l in cds)
                            {
                                // Проверяем левый узел.
                                foreach (var x in l.Key.LeftNode.ConnectedLines)
                                    if (ds.ContainsKey(x) && ds[x] == d - 1)
                                    {
                                        if (!path.Contains(x))
                                            path.Add(x);

                                        rds.Add(x, d - 1);
                                    }

                                // Проверяем правый узел.
                                foreach (var x in l.Key.RightNode.ConnectedLines)
                                    if (ds.ContainsKey(x) && ds[x] == d - 1)
                                    {
                                        if (!path.Contains(x))
                                            path.Add(x);

                                        rds.Add(x, d - 1);
                                    }
                            }

                            d--;
                        }
                    }

                    // Добавляем этот путь в список выбранных объектов.
                    this.AddSelectedObjects(path);
                }

            this.AddBoundaryLine(line);
        }

        /// <summary>
        /// Возвращает объект по его идентификатору и типу.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <returns>Объект.</returns>
        public IObjectViewModel GetObject(Guid id, ObjectType type)
        {
            if (type.ObjectKind == ObjectKind.Node)
                return this.NodesGroup.Layers.First(x => x.Type == type).Objects.FirstOrDefault(x => x.Id == id);

            IObjectViewModel result;

            result = this.AllObjectsGroup.Layers.First(x => x.Type == type).Objects.FirstOrDefault(x => x.Id == id);

            if (result != null)
                return result;

            result = this.PlanningObjectsGroup.Layers.First(x => x.Type == type).Objects.FirstOrDefault(x => x.Id == id);

            if (result != null)
                return result;

            result = this.DisabledObjectsGroup.Layers.First(x => x.Type == type).Objects.FirstOrDefault(x => x.Id == id);

            if (result != null)
                return result;

            return result;
        }

        /// <summary>
        /// Производит вставку заголовка утверждения/согласования.
        /// </summary>
        /// <param name="type">Тип заголовка.</param>
        /// <returns>Вставленный заголовок.</returns>
        public ApprovedHeaderViewModel InsertApprovedHeader(ApprovedHeaderType type)
        {
            var position = this.mapBindingService.GetCurrentCenter();

            var model = new ApprovedHeaderModel(Guid.Empty, this.CurrentCity.Id, Guid.Empty, new Point(position.X, position.Y), 50, "", "", DateTime.Now.Year, type);

            var viewModel = new ApprovedHeaderViewModel(model, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService);

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new AddRemoveCustomObjectAction(viewModel, this, true);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "добавление заголовка"));
            action.Do();

            return viewModel;
        }

        /// <summary>
        /// Производит вставку надписи.
        /// </summary>
        /// <param name="position">Положение вставки надписи.</param>
        /// <returns>Вставленная надпись.</returns>
        public LabelViewModel InsertLabel(Point position)
        {
            var label = new LabelViewModel(new LabelModel("Надпись", position, 50, 0, this.CitySelectionViewModel.LoadedCity.Id, Guid.Empty), this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService);

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new AddRemoveLabelAction(label, this, true);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "добавление надписи"));
            action.Do();

            return label;
        }

        /// <summary>
        /// Производит вставку таблицы с данными о протяженностях труб, разбитых по диаметрам.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <returns>Вставленная таблица.</returns>
        public LengthPerDiameterTableViewModel InsertLengthPerDiameterTable(Guid boilerId)
        {
            var position = this.mapBindingService.GetCurrentCenter();

            var model = new LengthPerDiameterTableModel(Guid.Empty, this.CurrentCity.Id, boilerId, Guid.Empty, new Point(position.X, position.Y), 50);

            var viewModel = new LengthPerDiameterTableViewModel(model, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService);

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new AddRemoveCustomObjectAction(viewModel, this, true);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "добавление свода участков по диаметрам"));
            action.Do();

            return viewModel;
        }

        /// <summary>
        /// Загружает данные заданной схемы.
        /// </summary>
        /// <param name="schemaId">Идентификатор схемы.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>true - если загрузка выполнена успешно, иначе - false.</returns>
        public bool LoadSchema(int schemaId, int cityId)
        {
            // Закрываем представления ошибок.
            this.ErrorsViewModel.Close();
            this.SavedErrorsViewModel.Close();

            // Закрываем представление узлов поворота и неподключенных узлов.
            this.BendNodesViewModel.Close();
            this.FreeNodesViewModel.Close();

            this.ObjectListViewModel.Close();
            this.JurKvpCompletedListViewModel.Close();

            FormGenerator.CloseAllForms();

            // Сперва необходимо проверить наличие необходимости сохранения изменений.
            if (this.SaveCommand.CanExecute(null))
            {
                var result = this.MessageService.ShowYesNoCancelMessage("Сохранить все внесенные изменения?", "Загрузка данных");

                if (!result.HasValue)
                    return false;
                else
                    if (result.Value)
                {
                    this.SaveCommand.Execute(null);

                    // Еще раз выполняем проверку необходимости сохранения, так как при сохранении могли возникнуть ошибки.
                    if (this.SaveCommand.CanExecute(null))
                        // И если возникли ошибки, то необходимо отменить дальнейшую загрузку данных.
                        return false;
                }
            }

            // Получаем информацию о схеме.
            var schema = this.DataService.Schemas.First(x => x.Id == schemaId);

            this.CitySelectionViewModel.LoadCity(new TerritorialEntityModel(cityId, ""));

            return this.Load((CityViewModel)this.CitySelectionViewModel.SelectedCity, schema);
        }

        /// <summary>
        /// Перемещает все редактируемые объекты.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void MoveEditingObjects(System.Windows.Point delta)
        {
            if (!this.groupEditStarted)
                return;

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new MoveObjectsAction(this.editingObjects, new System.Windows.Point(delta.X, delta.Y), this);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "перемещение объектов"));
            action.Do();
        }

        /// <summary>
        /// Подготавливает схему к работе.
        /// </summary>
        public void PrepareSchema()
        {
            var waitViewModel = new WaitViewModel("Обновление данных", "Пожалуйста подождите, необходимо выполнить обновление данных...", async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    this.DataService.PrepareSchema(this.CurrentSchema.Id, this.CitySelectionViewModel.LoadedCity.Id);
                });
            });

            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(waitViewModel));
        }

        /// <summary>
        /// Перерисовывает все очертания на карте.
        /// </summary>
        public void RedrawOutlines()
        {
            this.mapBindingService.ClearOutlines();

            this.GroupAreaPosition = new System.Windows.Point(0, 0);

            this.mapBindingService.CreateOutlines(this.outlineObjects);
        }

        /// <summary>
        /// Удаляет заданный пользовательский объект.
        /// </summary>
        /// <param name="customLayerObject">Удаляемый объект.</param>
        public void RemoveCustomLayerObject(ICustomLayerObject customLayerObject)
        {
            this.CustomObjects.Remove(customLayerObject);
        }

        /// <summary>
        /// Удаляет заданную надпись.
        /// </summary>
        /// <param name="label">Надпись.</param>
        public void RemoveLabel(LabelViewModel label)
        {
            this.CustomObjects.Remove(label);
        }

        /// <summary>
        /// Запрашивает изменение отображения длины.
        /// </summary>
        /// <param name="viewModel">Модель представления изменения отображения длины.</param>
        public void RequestChangeLengthView(ChangeLengthViewModel viewModel)
        {
            this.ChangeLengthViewRequested?.Invoke(this, new ChangeLengthViewRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Вращает все редактируемые объекты.
        /// </summary>
        /// <param name="angle">Угол поворота.</param>
        /// <param name="originPoint">Точка, относительно которой нужно повернуть объекты.</param>
        public void RotateEditingObjects(double angle, System.Windows.Point originPoint)
        {
            if (!this.groupEditStarted)
                return;

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new RotateObjectsAction(this.editingObjects, angle, new System.Windows.Point(originPoint.X, originPoint.Y), this);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "поворот объектов"));
            action.Do();
        }

        /// <summary>
        /// Масштабирует все редактируемые объекты.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        /// <param name="originPoint">Точка, относительно которой нужно смасштабировать объекты.</param>
        public void ScaleEditingObjects(double scale, System.Windows.Point originPoint)
        {
            if (!this.groupEditStarted)
                return;

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new ScaleObjectsAction(this.editingObjects, scale, new System.Windows.Point(originPoint.X, originPoint.Y), this);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "масштабирование объектов"));
            action.Do();
        }

        /// <summary>
        /// Отображает документы заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="name">Название.</param>
        public void ShowDocuments(IObjectModel obj, string name)
        {
            this.DocumentViewRequested?.Invoke(this, new ViewRequestedEventArgs<DocumentsViewModel>(new DocumentsViewModel(obj, name, this.CurrentSchema, this.AccessService, this.DataService, this.MessageService)));
        }

        /// <summary>
        /// Начинает рисование объекта.
        /// </summary>
        /// <param name="position">Начальное положение объекта.</param>
        /// <returns>Рисуемый объект.</returns>
        public IMapObjectViewModel StartDraw(System.Windows.Point position)
        {
            // Для фигур необходимо заранее определить некоторые параметры.
            var type = this.ToolBarViewModel.SelectedFigureType;
            var isPlanning = true;
            var hasChildren = false;
            var name = "";
            var id = ObjectModel.DefaultId;
            var childrenTypes = new List<ObjectType>();
            if (this.SelectedNotPlacedObject != null)
            {
                var obj = this.SelectedNotPlacedObject as IObjectViewModel;

                type = obj.Type;
                isPlanning = obj.IsPlanning;
                hasChildren = (obj as IContainerObjectViewModel).HasChildren;
                name = (obj as INamedObjectViewModel).RawName;
                id = obj.Id;
                childrenTypes.AddRange((obj as IContainerObjectViewModel).ChildrenTypes);
            }
            else
                // Задаем режим рисования.
                this.ToolBarViewModel.IsDrawPlanning = true;

            switch (this.ToolBarViewModel.SelectedTool)
            {
                case Tool.Ellipse:
                    var ellipse = new FigureModel(id, this.CitySelectionViewModel.LoadedCity.Id, type, isPlanning, hasChildren, name, FigureType.Ellipse, new Size(0, 0), new Point(position.X, position.Y), 0, "", null, null, null, true, childrenTypes);

                    this.drawingObject = new EllipseViewModel(ellipse, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                    break;

                case Tool.Line:
                    // Получаем идентификаторы выбранных типов линий.
                    var types = new List<ObjectType>();

                    foreach (var lineType in this.ToolBarViewModel.LineTypes)
                        if (lineType.IsSelected)
                            types.Add(lineType.Type);

                    var pointCollection = new PointCollection()
                    {
                        position, position
                    };

                    isPlanning = this.ToolBarViewModel.IsDrawPlanning;

                    this.drawingObject = new PolylineViewModel(types[0], isPlanning, pointCollection, this.mapBindingService.GetBrush(types[0].Color), this.LineInfo, this.MapViewModel.Scale, this.DataService, this.MessageService, this.mapBindingService)
                    {
                        IsPlaced = true
                    };

                    for (int i = 1; i < types.Count; i++)
                        (this.drawingObject as PolylineViewModel).ParallelPolylines.Add(new PolylineViewModel(types[i], isPlanning, pointCollection.Clone(), this.mapBindingService.GetBrush(types[i].Color), this.LineInfo, this.MapViewModel.Scale, this.DataService, this.MessageService, this.mapBindingService)
                        {
                            IsPlaced = true
                        });

                    break;

                case Tool.NewRuler:
                    this.drawingObject = new NewRulerViewModel(position, this.MapViewModel.Scale, this.LineInfo, this.mapBindingService);

                    break;

                case Tool.Polygon:
                    var polygon = new FigureModel(id, this.CitySelectionViewModel.LoadedCity.Id, type, isPlanning, hasChildren, name, FigureType.Polygon, new Size(0, 0), new Point(position.X, position.Y), 0, "", null, null, null, true, childrenTypes);

                    this.drawingObject = new PolygonViewModel(polygon, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                    break;

                case Tool.Rectangle:
                    var rectangle = new FigureModel(id, this.CitySelectionViewModel.LoadedCity.Id, type, isPlanning, hasChildren, name, FigureType.Rectangle, new Size(0, 0), new Point(position.X, position.Y), 0, "", null, null, null, true, childrenTypes);

                    this.drawingObject = new RectangleViewModel(rectangle, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

                    break;

                case Tool.Ruler:
                    this.drawingObject = new RulerViewModel(position, this.mapBindingService);

                    break;
            }

            // Если рисуется фигура, то указываем, что рисуем ее.
            var viewModel = this.drawingObject as FigureViewModel;
            if (viewModel != null)
                viewModel.IsDrawing = true;

            this.drawingObject.IsPlaced = true;

            return this.drawingObject;
        }

        #endregion
    }

    // Реализация ILayerHolder.
    internal sealed partial class MainViewModel
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
        /// Возвращает текущий населенный пункт.
        /// </summary>
        public CityViewModel CurrentCity
        {
            get
            {
                return this.CitySelectionViewModel.LoadedCity;
            }
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
            get
            {
                return this.editingObject;
            }
            set
            {
                if (this.EditingObject != value)
                {
                    if (value != null)
                    {
                        // Меняем инструмент на изменялку.
                        this.ToolBarViewModel.SelectedTool = Tool.Editor;

                        if (this.EditingObject != null)
                            this.EditingObject.IsEditing = false;
                    }

                    this.editingObject = value;

                    this.NotifyPropertyChanged(nameof(this.EditingObject));

                    this.CopyCommand.RaiseCanExecuteChanged();
                    this.DeleteCommand.RaiseCanExecuteChanged();
                }
            }
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
            get
            {
                return this.selectedGroup;
            }
            set
            {
                if (value != null && this.SelectedGroup != null && this.SelectedGroup != value)
                    // Убираем выбор с ранее выбранной группы.
                    this.SelectedGroup.IsSelected = false;

                this.selectedGroup = value;

                this.NotifyPropertyChanged(nameof(this.SelectedGroup));

                if (value != null)
                {
                    //vvs15
                    //this.ToolBarViewModel.SelectedTool = Tool.Selector;

                    if (this.SelectedObject != null)
                        this.SelectedObject.IsSelected = false;

                    if (this.SelectedNotPlacedObject != null)
                        this.SelectedNotPlacedObject.IsSelected = false;
                }
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный слой.
        /// </summary>
        public ISelectableObjectViewModel SelectedLayer
        {
            get
            {
                return this.selectedLayer;
            }
            set
            {
                this.selectedLayer = value;

                if (value != null)
                {
                    this.ToolBarViewModel.SelectedTool = Tool.Selector;

                    if (this.SelectedObject != null)
                        this.SelectedObject.IsSelected = false;

                    if (this.SelectedNotPlacedObject != null)
                        this.SelectedNotPlacedObject.IsSelected = false;
                }

                this.NotifyPropertyChanged(nameof(this.SelectedLayer));
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный неразмещенный объект.
        /// </summary>
        public ISelectableObjectViewModel SelectedNotPlacedObject
        {
            get
            {
                return this.selectedNotPlacedObject;
            }
            set
            {
                if (value != null && value as FigureViewModel == null)
                    return;

                if (this.SelectedNotPlacedObject != value)
                {
                    if (value != null)
                        this.DeselectAll();

                    this.selectedNotPlacedObject = value;

                    // Ограничиваем или убираем ограничение с рисования.
                    if (value != null)
                    {
                        this.ToolBarViewModel.IsDrawPlanning = (value as IObjectViewModel).IsPlanning;

                        this.ToolBarViewModel.SelectedFigureType = (value as FigureViewModel).Type;

                        this.ToolBarViewModel.SelectedTool = Tool.Rectangle;

                        this.ToolBarViewModel.ForceDrawFigures = true;

                        this.ToolBarViewModel.UpdateState();
                    }
                    else
                    {
                        this.ToolBarViewModel.IsDrawPlanning = true;

                        this.ToolBarViewModel.ForceDrawFigures = false;

                        this.ToolBarViewModel.UpdateState();

                        this.ToolBarViewModel.SelectedTool = Tool.Selector;
                    }

                    this.NotifyPropertyChanged(nameof(this.SelectedNotPlacedObject));
                }
            }
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
                    if (value != null)
                    {
                        var obj = value as IObjectViewModel;

                        var layer = this.selectedObjectsGroup.Layers.FirstOrDefault(x => x.Type == obj.Type);

                        if (layer != null && !layer.Contains(obj))
                        {
                            // Если имеется выбранный объект и он отсутствует в группе "Выбранные", то мы очищаем эту группу и добавляем в нее наш объект.
                            this.ClearSelectedGroup();
                            layer.Add(obj);
                        }

                        this.ToolBarViewModel.SelectedTool = Tool.Selector;

                        this.DeselectAll();
                    }

                    this.selectedObject = value;

                    if (value is LineViewModel)
                        this.AddBoundaryLine(value as LineViewModel);
                    

                    this.NotifyPropertyChanged(nameof(this.SelectedObject));

                    this.HasSelectedObject = value != null;
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
                //this.NodesGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
                this.NodesGroup.Layers.First(x => x.Type.Equals(obj.Type)).Add(obj);
            else
                if (!obj.IsActive)
                    this.DisabledObjectsGroup.Layers.First(x => x.Type.Equals(obj.Type)).Add(obj);
                    //this.DisabledObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
                else
                    if (!obj.IsPlanning)
                        this.AllObjectsGroup.Layers.First(x => x.Type.Equals(obj.Type)).Add(obj);
                        //this.AllObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
                    else
                        this.PlanningObjectsGroup.Layers.First(x => x.Type.Equals(obj.Type)).Add(obj);
                        //this.PlanningObjectsGroup.Layers.First(x => x.Type == obj.Type).Add(obj);
        }

        /// <summary>
        /// Очищает группу выбранных объектов.
        /// </summary>
        public void ClearSelectedGroup()
        {
            if (this.selectedObjectsGroup != null)
            {
                this.selectedObjectsGroup.IsSelected = false;

                this.selectedObjectsGroup.ClearLayerData();

                // Также очищаем список граничных линий.
                this.boundaryLines.Clear();
            }
        }

        /// <summary>
        /// Возвращает или задает слой объектов.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="layerType">Тип слоя объектов.</param>
        /// <returns>Слой объектов.</returns>
        public LayerViewModel GetLayer(ObjectType type, LayerType layerType)
        {
            switch (layerType)
            {
                case LayerType.Disabled:
                    return this.DisabledObjectsGroup.Layers.First(x => x.Type.Equals(type));
                //return this.DisabledObjectsGroup.Layers.First(x => x.Type == type);

                case LayerType.Planning:
                    return this.PlanningObjectsGroup.Layers.First(x => x.Type.Equals(type));
                //return this.PlanningObjectsGroup.Layers.First(x => x.Type == type);

                case LayerType.Standart:
                    return this.AllObjectsGroup.Layers.First(x => x.Type.Equals(type));
                    //return this.AllObjectsGroup.Layers.First(x => x.Type == type);
            }

            throw new NotImplementedException("Не реализована обработка следующего типа слоя объектов: " + layerType.ToString());
        }

        /// <summary>
        /// Возвращает слои по виду объектов.
        /// </summary>
        /// <param name="kind">Вид объектов.</param>
        /// <returns>Слои.</returns>
        public List<LayerViewModel> GetLayers(ObjectKind kind)
        {
            var result = new List<LayerViewModel>();

            result.AddRange(this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind.Equals(kind)));
            result.AddRange(this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind.Equals(kind)));
            result.AddRange(this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind.Equals(kind)));

            /*
            result.AddRange(this.AllObjectsGroup.Layers.Where(x => x.Type.ObjectKind == kind));
            result.AddRange(this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == kind));
            result.AddRange(this.DisabledObjectsGroup.Layers.Where(x => x.Type.ObjectKind == kind));
            */

            return result;
        }

        /// <summary>
        /// Возвращает объект по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект.</returns>
        public IObjectViewModel GetObject(Guid id)
        {
            foreach (var layer in this.AllObjectsGroup.Layers)
            {
                var result = layer.Objects.FirstOrDefault(x => x.Id == id);

                if (result != null)
                    return result;
            }

            foreach (var layer in this.PlanningObjectsGroup.Layers)
            {
                var result = layer.Objects.FirstOrDefault(x => x.Id == id);

                if (result != null)
                    return result;
            }

            foreach (var layer in this.DisabledObjectsGroup.Layers)
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
            var result = new List<IObjectViewModel>();

            foreach (var layer in this.selectedObjectsGroup.Layers)
                result.AddRange(layer.Objects);

            return result;
        }

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, подлежащий удалению.</param>
        public void MarkToDelete(IDeletableObjectViewModel obj)
        {
            if (this.DeletingObjects.Contains(obj))
                return;
            
            this.DeletingObjects.Add(obj);
        }

        /// <summary>
        /// Помечает объект на раннее обновление в источнике данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно обновить раньше остальных.</param>
        public void MarkToUpdate(IEditableObjectViewModel obj)
        {
            if (this.UpdatingObjects.Contains(obj))
                return;

            this.UpdatingObjects.Add(obj);
        }

        /// <summary>
        /// Выполняет пересчет длин несохраненных в источнике данных линий.
        /// </summary>
        public void RecalculateLength()
        {
            switch (this.MapViewModel.ScaleMode)
            {
                case LengthUpdateOption.All:
                    foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                        if (this.MapViewModel.ScaleLineType == null || layer.Type == this.MapViewModel.ScaleLineType)
                            foreach (var obj in layer.Objects)
                                (obj as LineViewModel).UpdateLength();

                    break;

                case LengthUpdateOption.OnlyNonSaved:
                    foreach (var layer in this.PlanningObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                        if (this.MapViewModel.ScaleLineType == null || layer.Type == this.MapViewModel.ScaleLineType)
                            foreach (LineViewModel line in layer.Objects)
                                if (!line.IsLengthFixed)
                                    line.UpdateLength();

                    break;

                case LengthUpdateOption.OnlySelected:
                    foreach (var layer in this.selectedObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Line))
                        if (this.MapViewModel.ScaleLineType == null || layer.Type == this.MapViewModel.ScaleLineType)
                            foreach (LineViewModel line in layer.Objects)
                                if (line.IsPlanning)
                                    line.UpdateLength();

                    break;
            }
        }

        /// <summary>
        /// Убирает объект с карты и с соответствующего ему слоя.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public void RemoveObject(IObjectViewModel obj)
        {
            // Убираем выбор со всего.
            this.DeselectAll();
            this.ClearSelectedGroup();

            var mapObject = obj as IMapObjectViewModel;

            mapObject.IsPlaced = false;

            obj.IsInitialized = false;

            mapObject.UnregisterBinding();

            if (obj as NodeViewModel != null)
                this.NodesGroup.Layers.First(x => x.Type == obj.Type).Remove(obj);
            else
                if (!obj.IsActive)
                    this.DisabledObjectsGroup.Layers.First(x => x.Type.Equals(obj.Type)).Remove(obj);
                else
                    if (!obj.IsPlanning)
                        this.AllObjectsGroup.Layers.First(x => x.Type.Equals(obj.Type)).Remove(obj);
            //this.AllObjectsGroup.Layers.First(x => x.Type == obj.Type).Remove(obj);
            else
                        this.PlanningObjectsGroup.Layers.First(x => x.Type.Equals( obj.Type)).Remove(obj);
            //this.PlanningObjectsGroup.Layers.First(x => x.Type == obj.Type).Remove(obj);
        }

        /// <summary>
        /// Заменяет полилайн на линию.
        /// </summary>
        /// <param name="polyline">Полилайн.</param>
        /// <param name="pointCount">Количество рассматриваемых точек.</param>
        /// <param name="forcedLength">Форсированно заданная длина линии полилайна.</param>
        /// <param name="offset">Отступ надписей линии.</param>
        /// <returns>Полученная линия.</returns>
        public LineViewModel ReplacePolyline(PolylineViewModel polyline, int pointCount, double offset, double? forcedLength = null)
        {
            LineViewModel result;

            var leftPoint = polyline.Points[0];
            var rightPoint = polyline.Points[pointCount - 1];

            var nodeType = this.DataService.ObjectTypes.First(x => x.ObjectKind == ObjectKind.Node);

            // Создаем левый узел и добавляем его на слой.
            var nodeModel = new NodeModel(ObjectModel.DefaultId, this.CitySelectionViewModel.LoadedCity.Id, nodeType, new Point(leftPoint.X, leftPoint.Y), null, null, false);
            var node = new NodeViewModel(nodeModel, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
            {
                IsInitialized = true
            };
            this.NodesGroup.Layers.First(x => x.Type.ObjectKind == ObjectKind.Node).Add(node);

            var layer = this.AllObjectsGroup.Layers.First(x => x.Type == polyline.Id);
            var planningLayer = this.PlanningObjectsGroup.Layers.First(x => x.Type == polyline.Id);

            // Создаем линию.
            var lineModel = new LineModel(ObjectModel.DefaultId, ObjectModel.DefaultId, this.CitySelectionViewModel.LoadedCity.Id, (ObjectType)polyline.Id, polyline.IsPlanning, false, new Point(leftPoint.X, leftPoint.Y), new Point(rightPoint.X, rightPoint.Y), 0, polyline.GetBendPoints(pointCount), new Dictionary<int, double>(), new Dictionary<int, Point>(), new Dictionary<int, int>(), "", 0, offset, true, null, true);
            result = new LineViewModel(lineModel, this, this.MapViewModel, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
            {
                IsInitialized = true,
                IsPlaced = true
            };

            if (polyline.IsPlanning)
                planningLayer.Add(result);
            else
                layer.Add(result);

            // Присоединяем линию к левому узлу.
            node.AddConnection(new NodeConnection(result, NodeConnectionSide.Left));

            // Добавляем узел на карту.
            node.IsPlaced = true;

            // Создаем правый узел и добавляем его на слой.
            nodeModel = new NodeModel(ObjectModel.DefaultId, this.CitySelectionViewModel.LoadedCity.Id, nodeType, new Point(rightPoint.X, rightPoint.Y), null, null, false);
            node = new NodeViewModel(nodeModel, this, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
            {
                IsInitialized = true
            };
            this.NodesGroup.Layers.First(x => x.Type.ObjectKind == ObjectKind.Node).Add(node);

            // Присоединяем к нему линию.
            node.AddConnection(new NodeConnection(result, NodeConnectionSide.Right));

            // Добавляем узел на карту.
            node.IsPlaced = true;

            // Обновляем длину линии.
            if (forcedLength.HasValue)
                result.UpdateLength(forcedLength.Value);
            else
                result.UpdateLength();

            // Убираем полилайн с карты.
            polyline.IsPlaced = false;
            polyline.UnregisterBinding();

            return result;
        }

        /// <summary>
        /// Задает выбранные объекты.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public void SetSelectedObjects(List<IObjectViewModel> objects)
        {
            this.DeselectAll();

            this.ClearSelectedGroup();

            // Будем вначале формировать слои из объектов, и только потом добавлять их. Это увеличит быстродействие.
            var layers = new Dictionary<ObjectType, List<IObjectViewModel>>();
            
            // Инициализируем слои.
            foreach (var layer in this.selectedObjectsGroup.Layers)
                layers.Add(layer.Type, new List<IObjectViewModel>());
                    
            // Заполняем их.
            foreach (var obj in objects)
                layers[obj.Type].Add(obj);

            // По слоям добавляем объекты в группу выбранных объектов.
            foreach (var layer in this.selectedObjectsGroup.Layers)
                layer.AddRange(layers[layer.Type]);
                    
            // Если был выбран только один слой объектов, то выбираем его, иначе - всю группу.
            int count = 0;
            LayerViewModel firstLayer = null;
            foreach (var layer in this.selectedObjectsGroup.Layers)
                if (layer.ObjectCount > 0)
                {
                    count++;

                    if (count == 1)
                        firstLayer = layer;
                }
            if (count == 1)
                // Но если в этом слое только один объект, то лучше выбрать его.
                if (firstLayer.ObjectCount == 1)
                    (firstLayer.Objects.First() as ISelectableObjectViewModel).IsSelected = true;
                else
                    firstLayer.IsSelected = true;
            else
                this.selectedObjectsGroup.IsSelected = true;
        }

        /// <summary>
        /// Убирает пометку с объекта, подлежащего к удалению из источника данных.
        /// </summary>
        /// <param name="obj">Объект, подлежащий удалению.</param>
        public void UnmarkToDelete(IDeletableObjectViewModel obj)
        {
            this.DeletingObjects.Remove(obj);
        }

        /// <summary>
        /// Убирает отметку с объекта, подлежащего к раннему обновлению в источнике данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно обновить раньше остальных.</param>
        public void UnmarkToUpdate(IEditableObjectViewModel obj)
        {
            this.UpdatingObjects.Remove(obj);
        }

        #endregion
    }
}