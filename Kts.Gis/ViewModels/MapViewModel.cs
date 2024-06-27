using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Settings;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Windows;
using Kts.Gis.ViewModels;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления карты.
    /// </summary>
    internal sealed partial class MapViewModel : Utilities.BaseViewModel, ISetterIgnorer
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что должны ли надписи автоматически скрываться.
        /// </summary>
        private bool autoHideLabels;

        /// <summary>
        /// Значение, указывающее на то, что должны ли узлы автоматически скрываться.
        /// </summary>
        private bool autoHideNodes;

        /// <summary>
        /// Масштаб значков.
        /// </summary>
        private double badgeScale;

        /// <summary>
        /// Значение, указывающее на то, что имеется ли подложка карты.
        /// </summary>
        private bool hasSubstrate;

        /// <summary>
        /// Значение, указывающее на то, что видно ли всплывающее окно с информацией о котельной.
        /// </summary>
        private bool isBoilerPopupVisible = true;

        /// <summary>
        /// Значение, указывающее на то, что виден ли слой гидравлики.
        /// </summary>
        private bool isHydraulicsVisible;

        /// <summary>
        /// Значение, указывающее на то, что виден ли слой ошибок гидравлики.
        /// </summary>
        private bool isHydraulicsErrorVisible;

        /// <summary>
        /// Значение, указывающее на то, что видны ли надписи объектов.
        /// </summary>
        private bool isLabelsVisible = true;

        /// <summary>
        /// Значение, указывающее на то, что видны ли слои карты.
        /// </summary>
        private bool isLayersVisible = true;
        private bool isDisableLayersVisible = true;

        /// <summary>
        /// Значение, указывающее на то, что видна ли легенда карты.
        /// </summary>
        private bool isLegendVisible;

        /// <summary>
        /// Значение, указывающее на то, что видна ли область печати карты.
        /// </summary>
        private bool isPrintAreaVisible;

        /// <summary>
        /// Значение, указывающее на то, что находится ли карта в режиме только для чтения.
        /// </summary>
        private bool isReadOnly;

        /// <summary>
        /// Значение, указывающее на то, что готова ли карта к работе.
        /// </summary>
        private bool isReady;

        /// <summary>
        /// Значение, указывающее на то, что видно ли всплывающее окно с информацией о топливном складе.
        /// </summary>
        private bool isStoragePopupVisible = true;

        /// <summary>
        /// Значение, указывающее на то, что видна ли подложка карты.
        /// </summary>
        private bool isSubstrateVisible;

        /// <summary>
        /// Значение, указывающее на то, что виден ли слой разделения по годам.
        /// </summary>
        private bool isYearDiffVisible;

        /// <summary>
        /// Выбранный принтер.
        /// </summary>
        private PrintQueue selectedPrinter;

        /// <summary>
        /// Масштаб линий.
        /// </summary>
        private double scale;

        /// <summary>
        /// Размерность подложки.
        /// </summary>
        private Size substrateDimension;

        /// <summary>
        /// Прозрачность подложки.
        /// </summary>
        private double substrateOpacity;

        /// <summary>
        /// Размер подложки.
        /// </summary>
        private Size substrateSize;

        /// <summary>
        /// Названия файлов-изображений подложки.
        /// </summary>
        private string[][] substrateTiles;

        /// <summary>
        /// Путь к миниатюре подложки карты.
        /// </summary>
        private string thumbnailPath;

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
        private readonly GroupViewModel disableObjectsGroup;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис истории изменений.
        /// </summary>
        private readonly HistoryService historyService;

        /// <summary>
        /// Слой гидравлики.
        /// </summary>
        private readonly HydraulicsViewModel hydraulics;

        /// <summary>
        /// Слой ошибок гидравлики.
        /// </summary>
        private readonly HydraulicsErrorViewModel hydraulicsError;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Слой ремонтной программы.
        /// </summary>
        private readonly RPViewModel rpViewModel;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        /// <summary>
        /// Слой несопоставленных объектов.
        /// </summary>
        private readonly UOViewModel uoViewModel;
        private readonly DisableObjectViewModel disableViewModel;
        private readonly IJSViewModel ijsViewModel;
        private readonly IJSViewModel ijsFViewModel;

        /// <summary>
        /// Слой разделения линий по годам.
        /// </summary>
        private readonly YearDiffViewModel yearDiff;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса добавления объекта, представляемого значком на карте.
        /// </summary>
        public event EventHandler<AddBadgeRequestedEventArgs> AddBadgeRequested;

        /// <summary>
        /// Событие запроса отображения представления настройки надписей.
        /// </summary>
        public event EventHandler<Utilities.ViewRequestedEventArgs<CaptionManageViewModel>> CaptionManageViewRequested;

        /// <summary>
        /// Событие запроса отображения представления настроек вида карты.
        /// </summary>
        public event EventHandler<MapSettingsViewRequestedEventArgs> MapSettingsViewRequested;

        /// <summary>
        /// Событие необходимости переоткрытия схемы.
        /// </summary>
        public event EventHandler NeedReopen;

        /// <summary>
        /// Событие необходимости тихого сохранения изменений схемы.
        /// </summary>
        public event EventHandler<BoolResultEventArgs> NeedSilentSave;

        /// <summary>
        /// Событие запроса выбора принтера.
        /// </summary>
        public event EventHandler<PrinterSelectionRequestedEventArgs> PrinterSelectionRequested;

        /// <summary>
        /// Событие изменения масштаба линий.
        /// </summary>
        public event EventHandler ScaleChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MapViewModel"/>.
        /// </summary>
        /// <param name="types">Типы объектов карты.</param>
        /// <param name="allObjectsGroup">Группа всех объектов.</param>
        /// <param name="hydraulics">Слой гидравлики.</param>
        /// <param name="yearDiff">Слой разделения по годам.</param>
        /// <param name="rpViewModel">Слой ремонтной программы.</param>
        /// <param name="uoViewModel">Слой несопоставленных объектов.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public MapViewModel(List<ObjectType> types, GroupViewModel allObjectsGroup, GroupViewModel disableObjectsGroup, HydraulicsViewModel hydraulics, HydraulicsErrorViewModel hydraulicsError, YearDiffViewModel yearDiff, RPViewModel rpViewModel, UOViewModel uoViewModel, DisableObjectViewModel disableViewModel, IJSViewModel ijsViewModel, IJSViewModel ijsFViewModel, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService, ISettingService settingService)
        {
            this.Types = types;
            this.allObjectsGroup = allObjectsGroup;
            this.disableObjectsGroup = disableObjectsGroup;
            this.hydraulics = hydraulics;
            this.hydraulicsError = hydraulicsError;
            this.yearDiff = yearDiff;
            this.rpViewModel = rpViewModel;
            this.uoViewModel = uoViewModel;
            this.disableViewModel = disableViewModel;

            this.ijsViewModel = ijsViewModel;
            this.ijsFViewModel = ijsFViewModel;

            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.dataService = dataService;
            this.historyService = historyService;
            this.mapBindingService = mapBindingService;
            this.messageService = messageService;
            this.settingService = settingService;

            this.ChangeMapCaptionsCommand = new RelayCommand(this.ExecuteChangeMapCaptions, this.CanExecuteChangeMapCaptions);
            this.ChangeMapSettingsCommand = new RelayCommand(this.ExecuteChangeMapSettings, this.CanExecuteChangeMapSettings);
            this.ShowHideBoilerPopupCommand = new RelayCommand(this.ExecuteShowHideBoilerPopup);
            this.ShowHideHydraulicsCommand = new RelayCommand(this.ExecuteShowHideHydraulics, this.CanExecuteShowHideHydraulics);
            this.ShowHideHydraulicsErrorCommand = new RelayCommand(this.ExecuteShowHideHydraulicsError, this.CanExecuteShowHideHydraulicsError);
            this.ShowHideLabelsCommand = new RelayCommand(this.ExecuteShowHideLabels, this.CanExecuteShowHideLabels);
            this.ShowHideLayersCommand = new RelayCommand(this.ExecuteShowHideLayers, this.CanExecuteShowHideLayers);
            this.ShowDisableLayersCommand = new RelayCommand(this.ShowHideDisableLayers, this.CanExecuteShowHideLabels);
            this.ShowHideLegendCommand = new RelayCommand(this.ExecuteShowHideLegend, this.CanExecuteShowHideLegend);
            this.ShowHidePrintAreaCommand = new RelayCommand(this.ExecuteShowHidePrintArea, this.CanExecuteShowHidePrintArea);
            this.ShowHideRPCommand = new RelayCommand(this.ExecuteShowHideRP, this.CanExecuteShowHideRP);
            this.ShowHideStoragePopupCommand = new RelayCommand(this.ExecuteShowHideStoragePopup);
            this.ShowHideSubstrateCommand = new RelayCommand(this.ExecuteShowHideSubstrate, this.CanExecuteShowHideSubstrate);
            this.ShowHideUOCommand = new RelayCommand(this.ExecuteShowHideUO, this.CanExecuteShowHideUO);
            this.ShowHideDisableObjCommand = new RelayCommand(this.ExecuteShowHideDisabled, this.CanExecuteShowHideDisabled);
            this.ShowHideIJSTCommand = new RelayCommand(this.ExecuteShowHideIJST, this.CanExecuteShowHideIJST);

            this.ShowHideIJSFCommand = new RelayCommand(this.ExecuteShowHideIJSF, this.CanExecuteShowHideIJSF);

            this.ShowHideYearDiffCommand = new RelayCommand(this.ExecuteShowHideYearDiff, this.CanExecuteShowHideYearDiff);

            this.isSubstrateVisible = Convert.ToBoolean(this.settingService.Settings["IsSubstrateVisible"]);
            this.isLegendVisible = Convert.ToBoolean(this.settingService.Settings["IsLegendVisible"]);
            this.substrateOpacity = Convert.ToDouble(this.settingService.Settings["SubstrateOpacity"]);
            this.badgeScale = Convert.ToDouble(this.settingService.Settings["BadgeScale"]);
            this.autoHideLabels = Convert.ToBoolean(this.settingService.Settings["AutoHideLabels"]);
            this.autoHideNodes = Convert.ToBoolean(this.settingService.Settings["AutoHideNodes"]);
            this.isBoilerPopupVisible = Convert.ToBoolean(this.settingService.Settings["IsBoilerPopupVisible"]);
            this.isStoragePopupVisible = Convert.ToBoolean(this.settingService.Settings["IsStoragePopupVisible"]);

            this.SetValue(nameof(this.BadgeScale), this.badgeScale);
            this.SetValue(nameof(this.AutoHideLabels), this.autoHideLabels);
            this.SetValue(nameof(this.AutoHideNodes), this.autoHideNodes);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что должны ли надписи автоматически скрываться.
        /// </summary>
        public bool AutoHideLabels
        {
            get
            {
                return this.autoHideLabels;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.AutoHideLabels), this.AutoHideLabels, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение автоматического скрытия надписей"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что должны ли узлы автоматически скрываться.
        /// </summary>
        public bool AutoHideNodes
        {
            get
            {
                return this.autoHideNodes;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.AutoHideNodes), this.AutoHideNodes, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение автоматического скрытия узлов"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает масштаб значков.
        /// </summary>
        public double BadgeScale
        {
            get
            {
                return this.badgeScale;
            }
            set
            {
                // При изменении масштаба значков, необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение масштаба меняется очень часто.
                var exists = false;
                var entry = this.historyService.GetCurrentEntry();
                if (entry != null)
                {
                    var action = entry.Action as SetPropertyAction;

                    if (action != null && action.Object == this && action.PropertyName == nameof(this.BadgeScale))
                    {
                        action.NewValue = value;

                        action.Do();

                        exists = true;
                    }
                }
                if (!exists)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.BadgeScale), this.BadgeScale, value);
                    this.historyService.Add(new HistoryEntry(action, Target.View, "изменение масштаба значков"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает команду смены настроек надписей.
        /// </summary>
        public RelayCommand ChangeMapCaptionsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду смены настроек вида карты.
        /// </summary>
        public RelayCommand ChangeMapSettingsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли подложка карты.
        /// </summary>
        public bool HasSubstrate
        {
            get
            {
                return this.hasSubstrate;
            }
            private set
            {
                this.hasSubstrate = value;

                this.NotifyPropertyChanged(nameof(this.HasSubstrate));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видно ли всплывающее окно с информацией о котельной.
        /// </summary>
        public bool IsBoilerPopupVisible
        {
            get
            {
                return this.isBoilerPopupVisible;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsBoilerPopupVisible), this.IsBoilerPopupVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости всплывающего окна с информацией о котельной"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли слой гидравлики.
        /// </summary>
        public bool IsHydraulicsVisible
        {
            get
            {
                return this.isHydraulicsVisible;
            }
            set
            {
                this.isHydraulicsVisible = value;

                this.hydraulics.IsVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsHydraulicsVisible));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли слой ошибок гидравлики.
        /// </summary>
        public bool IsHydraulicsErrorVisible
        {
            get
            {
                return this.isHydraulicsErrorVisible;
            }
            set
            {
                this.isHydraulicsErrorVisible = value;

                this.hydraulicsError.IsVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsHydraulicsErrorVisible));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видны ли надписи объектов.
        /// </summary>
        public bool IsLabelsVisible
        {
            get
            {
                return this.isLabelsVisible;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsLabelsVisible), this.IsLabelsVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости надписей объектов"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видны ли слои карты.
        /// </summary>
        public bool IsLayersVisible
        {
            get
            {
                return this.isLayersVisible;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsLayersVisible), this.IsLayersVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости слоев карты"));
                action.Do();
            }
        }

        public bool IsDisableLayersVisible
        {
            get
            {
                return this.isDisableLayersVisible;
            }
            set
            {
                var action = new SetPropertyAction(this, nameof(this.IsDisableLayersVisible), this.IsDisableLayersVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости неактивных слоев карты"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видна ли легенда карты.
        /// </summary>
        public bool IsLegendVisible
        {
            get
            {
                return this.isLegendVisible;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsLegendVisible), this.IsLegendVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости легенды карты"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видна ли область печати карты.
        /// </summary>
        public bool IsPrintAreaVisible
        {
            get
            {
                return this.isPrintAreaVisible;
            }
            set
            {
                if (!value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.IsPrintAreaVisible), this.IsPrintAreaVisible, value);
                    this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости области печати карты"));
                    action.Do();
                }
                else
                    if (this.PrinterSelectionRequested != null)
                {
                    var eventArgs = new PrinterSelectionRequestedEventArgs();

                    this.PrinterSelectionRequested(this, eventArgs);

                    this.SelectedPrinter = eventArgs.SelectedPrinter;

                    if (eventArgs.SelectedPrinter != null)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new SetPropertyAction(this, nameof(this.IsPrintAreaVisible), this.IsPrintAreaVisible, value);
                        this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости области печати карты"));
                        action.Do();
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что находится ли карта в режиме только для чтения.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;

                this.ShowHideHydraulicsCommand.RaiseCanExecuteChanged();
                this.ShowHideYearDiffCommand.RaiseCanExecuteChanged();
                this.ShowHideRPCommand.RaiseCanExecuteChanged();
                this.ShowHideUOCommand.RaiseCanExecuteChanged();
                this.ShowHideDisableObjCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что готова ли карта к работе.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return this.isReady;
            }
            set
            {
                this.isReady = value;

                this.NotifyPropertyChanged(nameof(this.IsReady));

                this.ChangeMapCaptionsCommand.RaiseCanExecuteChanged();
                this.ChangeMapSettingsCommand.RaiseCanExecuteChanged();
                this.ShowHideHydraulicsCommand.RaiseCanExecuteChanged();
                this.ShowHideLabelsCommand.RaiseCanExecuteChanged();
                this.ShowHideLayersCommand.RaiseCanExecuteChanged();
                this.ShowHideLegendCommand.RaiseCanExecuteChanged();
                this.ShowHidePrintAreaCommand.RaiseCanExecuteChanged();
                this.ShowHideYearDiffCommand.RaiseCanExecuteChanged();
                this.ShowHideRPCommand.RaiseCanExecuteChanged();
                this.ShowHideUOCommand.RaiseCanExecuteChanged();
                this.ShowHideDisableObjCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Значение, указывающее на то, что виден ли слой ремонтной программы.
        /// </summary>
        private bool isRPVisible;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли слой ремонтной программы.
        /// </summary>
        public bool IsRPVisible
        {
            get
            {
                return this.isRPVisible;
            }
            set
            {
                this.isRPVisible = value;

                this.rpViewModel.IsVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsRPVisible));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видно ли всплывающее окно с информацией о топливном складе.
        /// </summary>
        public bool IsStoragePopupVisible
        {
            get
            {
                return this.isStoragePopupVisible;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsStoragePopupVisible), this.IsStoragePopupVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости всплывающего окна с информацией о топливном складе"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видна ли подложка карты.
        /// </summary>
        public bool IsSubstrateVisible
        {
            get
            {
                return this.isSubstrateVisible;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsSubstrateVisible), this.IsSubstrateVisible, value);
                this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости подложки карты"));
                action.Do();
            }
        }

        /// <summary>
        /// Значение, указывающее на то, что виден ли слой несопоставленных объектов.
        /// </summary>
        private bool isUOVisible;
        private bool isIJSVisibleT;
        private bool isIJSVisibleF;
        private bool isDisabledVisible = true;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли слой несопоставленных объектов.
        /// </summary>
        public bool IsUOVisible
        {
            get
            {
                return this.isUOVisible;
            }
            set
            {
                this.isUOVisible = value;

                this.uoViewModel.IsVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsUOVisible));
            }
        }

        public bool IsDisabledVisible
        {
            get
            {
                return this.isDisabledVisible;
            }
            set
            {
                if (isDisabledVisible == value) return;
                this.isDisabledVisible = value;
                //this.disableViewModel.IsVisible = value;


                //var lineTypes = this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Line);
                //foreach (var layer in this.AllObjectsGroup.Layers.Where(x => lineTypes.Contains(x.Type)))
                //foreach (LineViewModel line in layer.Objects)//3198
                //ssk11
                foreach (var item in this.disableObjectsGroup.Layers)
                {
                    foreach (var obj in item.Objects)
                    {

                        if (obj is FigureViewModel)
                        {
                            this.mapBindingService.SetMapObjectViewValue(obj as FigureViewModel, nameof(this.IsDisabledVisible), value);
                        }
                        //disableObjects.Add(obj.Id);
                        //this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsHighlighted), value);
                    }
                }
                //mstsc2
                /*
                foreach (var layer in this.disableObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure || x.Type.ObjectKind == ObjectKind.Line))
                {

                    layer.SetValue(nameof(LayerViewModel.IsVisible), this.isDisableLayersVisible);
                    layer.
                }*/
                //this.mapBindingService.SetMapObjectViewValue()
                //this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsHighlighted), value);


                this.NotifyPropertyChanged(nameof(this.IsDisabledVisible));
            }
        }


        public bool IsIJSVisibleT
        {
            get
            {
                return this.isIJSVisibleT;
            }
            set
            {
                this.isIJSVisibleT = value;
                this.ijsViewModel.IsVisible = value;
                this.NotifyPropertyChanged(nameof(this.IsIJSVisibleT));
            }
        }

        public bool IsIJSVisibleF
        {
            get
            {
                return this.isIJSVisibleF;
            }
            set
            {
                this.isIJSVisibleF = value;
                this.ijsFViewModel.IsVisible = value;
                this.NotifyPropertyChanged(nameof(this.IsIJSVisibleF));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли слой разделения по годам.
        /// </summary>
        public bool IsYearDiffVisible
        {
            get
            {
                return this.isYearDiffVisible;
            }
            set
            {
                this.isYearDiffVisible = value;

                this.yearDiff.IsVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsYearDiffVisible));
            }
        }

        /// <summary>
        /// Возвращает или задает масштаб линий.
        /// </summary>
        public double Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;

                this.ScaleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Возвращает или задает тип линий, которые подвергаются обновлению длины.
        /// </summary>
        public ObjectType ScaleLineType
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает опцию обновления длин линий.
        /// </summary>
        public LengthUpdateOption ScaleMode
        {
            get;
            set;
        } = LengthUpdateOption.OnlyNonSaved;

        /// <summary>
        /// Возвращает или задает выбранный принтер.
        /// </summary>
        public PrintQueue SelectedPrinter
        {
            get
            {
                return this.selectedPrinter;
            }
            private set
            {
                this.selectedPrinter = value;

                this.NotifyPropertyChanged(nameof(this.SelectedPrinter));
            }
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия всплывающего окна с информацией о котельной.
        /// </summary>
        public RelayCommand ShowHideBoilerPopupCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия слоя гидравлики.
        /// </summary>
        public RelayCommand ShowHideHydraulicsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия слоя ошибок гидравлики.
        /// </summary>
        public RelayCommand ShowHideHydraulicsErrorCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия надписей объектов.
        /// </summary>
        public RelayCommand ShowHideLabelsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия слоев карты.
        /// </summary>
        public RelayCommand ShowHideLayersCommand
        {
            get;
        }

        public RelayCommand ShowDisableLayersCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия легенды карты.
        /// </summary>
        public RelayCommand ShowHideLegendCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия области печати.
        /// </summary>
        public RelayCommand ShowHidePrintAreaCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия слоя ремонтной программы.
        /// </summary>
        public RelayCommand ShowHideRPCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия всплывающего окна с информацией о топливном складе.
        /// </summary>
        public RelayCommand ShowHideStoragePopupCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия подложки карты.
        /// </summary>
        public RelayCommand ShowHideSubstrateCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия слоя несопоставленных объектов.
        /// </summary>
        public RelayCommand ShowHideUOCommand
        {
            get;
        }

        public RelayCommand ShowHideIJSTCommand
        {
            get;
        }

        public RelayCommand ShowHideIJSFCommand
        {
            get;
        }

        public RelayCommand ShowHideDisableObjCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения/скрытия слоя разделения по годам.
        /// </summary>
        public RelayCommand ShowHideYearDiffCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает размерность подложки.
        /// </summary>
        public Size SubstrateDimension
        {
            get
            {
                return this.substrateDimension;
            }
            set
            {
                this.substrateDimension = value;

                this.NotifyPropertyChanged(nameof(this.SubstrateDimension));
            }
        }

        /// <summary>
        /// Возвращает или задает прозрачность подложки.
        /// </summary>
        public double SubstrateOpacity
        {
            get
            {
                return this.substrateOpacity;
            }
            set
            {
                // При изменении прозрачности, необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение прозрачности меняется очень часто.
                var exists = false;
                var entry = this.historyService.GetCurrentEntry();
                if (entry != null)
                {
                    var action = entry.Action as SetPropertyAction;

                    if (action != null && action.Object == this && action.PropertyName == nameof(this.SubstrateOpacity))
                    {
                        action.NewValue = value;

                        action.Do();

                        exists = true;
                    }
                }
                if (!exists)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.SubstrateOpacity), this.SubstrateOpacity, value);
                    this.historyService.Add(new HistoryEntry(action, Target.View, "изменение прозрачности подложки"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает размер подложки.
        /// </summary>
        public Size SubstrateSize
        {
            get
            {
                return this.substrateSize;
            }
            set
            {
                this.substrateSize = value;

                this.NotifyPropertyChanged(nameof(this.SubstrateSize));
            }
        }

        /// <summary>
        /// Возвращает или задает названия файлов-изображений подложки.
        /// </summary>
        public string[][] SubstrateTiles
        {
            get
            {
                return this.substrateTiles;
            }
            set
            {
                this.substrateTiles = value;

                this.NotifyPropertyChanged(nameof(this.SubstrateTiles));

                this.HasSubstrate = !(this.SubstrateTiles == null || this.SubstrateTiles.Length == 0);

                this.ShowHideSubstrateCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает путь к миниатюре подложки карты.
        /// </summary>
        public string ThumbnailPath
        {
            get
            {
                return this.thumbnailPath;
            }
            set
            {
                this.thumbnailPath = value;

                this.NotifyPropertyChanged(nameof(this.ThumbnailPath));
            }
        }

        /// <summary>
        /// Возвращает список типов объектов карты.
        /// </summary>
        public List<ObjectType> Types
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить смену настроек надписей карты.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteChangeMapCaptions()
        {
            return this.IsReady && this.accessService.CanChangeMapCaptions(this.layerHolder.CurrentSchema.IsActual) && !this.historyService.Contains(Target.Data);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить смену настроек вида карты.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteChangeMapSettings()
        {
            return this.IsReady && this.accessService.CanChangeMapSettings(this.layerHolder.CurrentSchema.IsActual) && !this.historyService.Contains(Target.Data);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости слоя гидравлики.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShowHideHydraulics()
        {
            return this.IsReady && this.IsReadOnly;
        }
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости слоя ошибок гидравлики.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShowHideHydraulicsError()
        {
            return this.IsReady && this.IsReadOnly;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости надписей объектов.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить задание видимости надписей объектов.</returns>
        private bool CanExecuteShowHideLabels()
        {
            return this.IsReady;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости слоев карты.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить задание видимости слоев карты.</returns>
        private bool CanExecuteShowHideLayers()
        {
            return this.IsReady;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости легенды карты.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить задание видимости легенды карты.</returns>
        private bool CanExecuteShowHideLegend()
        {
            return this.IsReady;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости области печати.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить задание видимости области печати.</returns>
        private bool CanExecuteShowHidePrintArea()
        {
            return this.IsReady;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости слоя ремонтной программы.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShowHideRP()
        {
            return this.IsReady && this.IsReadOnly;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости подложки карты.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить задание видимости подложки карты.</returns>
        private bool CanExecuteShowHideSubstrate()
        {
            return this.HasSubstrate;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости слоя несопоставленных объектов.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShowHideUO()
        {
            return this.IsReady && this.IsReadOnly;
        }

        private bool CanExecuteShowHideDisabled()
        {
            return this.IsReady && this.IsReadOnly;
        }

        public bool CanExecuteShowHideIJST()
        {
            return this.IsReady && this.IsReadOnly;
        }

        public bool CanExecuteShowHideIJSF()
        {
            return this.IsReady && this.IsReadOnly;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить задание видимости слоя разделения по годам.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShowHideYearDiff()
        {
            return this.IsReady && this.IsReadOnly;
        }

        /// <summary>
        /// Выполняет смену настроек надписей карты.
        /// </summary>
        private void ExecuteChangeMapCaptions()
        {
            
            var eventArgs = new Utilities.ViewRequestedEventArgs<CaptionManageViewModel>(new CaptionManageViewModel(this.layerHolder.CurrentCity.Id, this.dataService));

            this.CaptionManageViewRequested?.Invoke(this, eventArgs);

            if (eventArgs.Result)
                this.messageService.ShowMessage("Для вступления изменений в силу необходимо заново открыть схему", "Настройка надписей населенного пункта", MessageType.Information);
        }

        /// <summary>
        /// Выполняет смену настроек вида карты.
        /// </summary>
        private void ExecuteChangeMapSettings()
        {
            var settings = this.mapBindingService.MapSettingService;

            var model = new MapSettingsModel(settings.FigureLabelDefaultSize,
                                             settings.FigurePlanningOffset,
                                             settings.FigureThickness,
                                             settings.IndependentLabelDefaultSize,
                                             settings.LineDisabledOffset,
                                             settings.LineLabelDefaultSize,
                                             settings.LinePlanningOffset,
                                             settings.LineThickness);

            var eventArgs = new MapSettingsViewRequestedEventArgs(new MapSettingsViewModel(model, this.mapBindingService));

            this.MapSettingsViewRequested?.Invoke(this, eventArgs);

            if (eventArgs.Result)
            {
                // Сперва попробуем сохранить измененные значения схемы, если они есть.
                var e = new BoolResultEventArgs();
                this.NeedSilentSave?.Invoke(this, e);

                if (e.Result)
                {
                    // Сохраняем заданные настройки вида карты.
                    this.mapBindingService.MapSettingService.Load(model);
                    this.mapBindingService.MapSettingService.SaveCurrent();

                    this.messageService.ShowMessage("Изменения сохранены. Для дальнейшей работы необходимо заново открыть схему", "Настройка населенного пункта", MessageType.Information);
                }
                else
                    this.messageService.ShowMessage("Не удалось сохранить изменения. Для дальнейшей работы необходимо заново открыть схему", "Настройка населенного пункта", MessageType.Error);
            }
            else
                this.messageService.ShowMessage("Изменения были отменены. Для дальнейшей работы необходимо заново открыть схему", "Настройка населенного пункта", MessageType.Information);

            // Требуем переоткрытие схемы.
            this.NeedReopen?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет отображение/скрытие всплывающего окна с информацией о котельной.
        /// </summary>
        private void ExecuteShowHideBoilerPopup()
        {
            this.IsBoilerPopupVisible = !this.IsBoilerPopupVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоя гидравлики.
        /// </summary>
        private void ExecuteShowHideHydraulics()
        {
            this.IsHydraulicsVisible = !this.IsHydraulicsVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоя ошибок гидравлики.
        /// </summary>
        private void ExecuteShowHideHydraulicsError()
        {
            this.IsHydraulicsErrorVisible = !this.IsHydraulicsErrorVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие надписей объектов.
        /// </summary>
        private void ExecuteShowHideLabels()
        {
            this.IsLabelsVisible = !this.IsLabelsVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоев карты.
        /// </summary>
        private void ExecuteShowHideLayers()
        {
            this.IsLayersVisible = !this.IsLayersVisible;
        }

        public void ShowHideDisableLayers()
        {
            this.IsDisableLayersVisible = !this.IsDisableLayersVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие легенды карты.
        /// </summary>
        private void ExecuteShowHideLegend()
        {
            this.IsLegendVisible = !this.IsLegendVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоя ремонтной программы.
        /// </summary>
        private void ExecuteShowHidePrintArea()
        {
            this.IsPrintAreaVisible = !this.IsPrintAreaVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоя гидравлики.
        /// </summary>
        private void ExecuteShowHideRP()
        {
            this.IsRPVisible = !this.IsRPVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие всплывающего окна с информацией о топливном складе.
        /// </summary>
        private void ExecuteShowHideStoragePopup()
        {
            this.IsStoragePopupVisible = !this.IsStoragePopupVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие подложки карты.
        /// </summary>
        private void ExecuteShowHideSubstrate()
        {
            this.IsSubstrateVisible = !this.IsSubstrateVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоя несопоставленных объектов.
        /// </summary>
        private void ExecuteShowHideUO()
        {
            this.IsUOVisible = !this.IsUOVisible;
        }

        private void ExecuteShowHideIJST()
        {
            this.IsIJSVisibleT = !this.IsIJSVisibleT;
        }

        private void ExecuteShowHideIJSF()
        {
            this.IsIJSVisibleF = !this.IsIJSVisibleF;
        }

        public void ExecuteShowHideDisabled()
        {
            this.IsDisabledVisible = !this.IsDisabledVisible;
        }

        /// <summary>
        /// Выполняет отображение/скрытие слоя разделения по годам.
        /// </summary>
        private void ExecuteShowHideYearDiff()
        {
            this.IsYearDiffVisible = !this.IsYearDiffVisible;
        }

        #endregion

        #region Открытые методы
        
        /// <summary>
        /// Запрашивает добавление объекта, представляемого значком на карте.
        /// </summary>
        /// <param name="line">Линия, к которой нужно добавить объект, представляемый значком на карте.</param>
        /// <returns>Расстояние объекта, представляемого значком на карте, от конца линии.</returns>
        public double? RequestAddBadge(LineViewModel line)
        {
            if (this.AddBadgeRequested != null)
            {
                var eventArgs = new AddBadgeRequestedEventArgs(line);

                this.AddBadgeRequested(this, eventArgs);

                return eventArgs.Result;
            }

            return null;
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class MapViewModel
    {
        #region Открытые методы

        /// <summary>
        /// Задает значение заданного свойства в обход его сеттера.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        /// <param name="value">Значение.</param>
        public void SetValue(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(this.AutoHideLabels):
                    this.autoHideLabels = (bool)value;

                    this.mapBindingService.SetAutoHideLabels(this.AutoHideLabels);

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["AutoHideLabels"] = value;

                    break;

                case nameof(this.AutoHideNodes):
                    this.autoHideNodes = (bool)value;

                    this.mapBindingService.SetAutoHideNodes(this.AutoHideNodes);

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["AutoHideNodes"] = value;

                    break;

                case nameof(this.BadgeScale):
                    this.badgeScale = (double)value;

                    this.mapBindingService.SetBadgeScale(this.BadgeScale);

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["BadgeScale"] = value;

                    break;

                case nameof(this.IsBoilerPopupVisible):
                    this.isBoilerPopupVisible = (bool)value;

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["IsBoilerPopupVisible"] = value;

                    break;

                case nameof(this.IsLabelsVisible):
                    this.isLabelsVisible = (bool)value;

                    if (this.IsLabelsVisible)
                        this.mapBindingService.ForceShowLabels();
                    else
                        this.mapBindingService.ForceHideLabels();

                    break;

                case nameof(this.IsLayersVisible):
                    this.isLayersVisible = (bool)value;

                    foreach (var layer in this.allObjectsGroup.Layers.Where(x => x.Type.ObjectKind == ObjectKind.Figure || x.Type.ObjectKind == ObjectKind.Line))
                        layer.SetValue(nameof(LayerViewModel.IsVisible), this.IsLayersVisible);

                    break;
                case nameof(this.IsDisableLayersVisible):
                    this.isDisableLayersVisible = (bool)value;
                    foreach(var layer in this.disableObjectsGroup.Layers.Where(x=> x.Type.ObjectKind == ObjectKind.Figure || x.Type.ObjectKind == ObjectKind.Line))
                        layer.SetValue(nameof(LayerViewModel.IsVisible), this.isDisableLayersVisible);
                    //foreach(var layer in this.Disable)
                    break;
                case nameof(this.IsLegendVisible):
                    this.isLegendVisible = (bool)value;

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["IsLegendVisible"] = value;

                    break;

                case nameof(this.IsStoragePopupVisible):
                    this.isStoragePopupVisible = (bool)value;

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["IsStoragePopupVisible"] = value;

                    break;

                case nameof(this.IsSubstrateVisible):
                    this.isSubstrateVisible = (bool)value;

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["IsSubstrateVisible"] = value;

                    break;

                case nameof(this.IsPrintAreaVisible):
                    this.isPrintAreaVisible = (bool)value;

                    break;

                case nameof(this.SubstrateOpacity):
                    this.substrateOpacity = (double)value;

                    // Запоминаем новое значение в настройках приложения.
                    this.settingService.Settings["SubstrateOpacity"] = value;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.NotifyPropertyChanged(propertyName);
        }

        #endregion
    }
}