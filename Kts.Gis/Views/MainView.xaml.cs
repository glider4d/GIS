using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Reports;
using Kts.Gis.Reports.ViewModels;
using Kts.Gis.Reports.Views;
using Kts.Gis.Services;
using Kts.Gis.Substrates;
using Kts.Gis.ViewModels;
using Kts.Messaging;
using Kts.Settings;

using Kts.WpfUtilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет главное представление.
    /// </summary>
    internal sealed partial class MainView : Window, IDisposable
    {
        #region Закрытые поля

        /// <summary>
        /// Представление узлов поворота.
        /// </summary>
        private BendNodesView bendNodesView;

        /// <summary>
        /// Рисуемый объект.
        /// </summary>
        private IMapObjectViewModel drawingObject;

        /// <summary>
        /// Представление ошибок.
        /// </summary>
        private ErrorsView errorsView;

        /// <summary>
        /// Окно генератора форм.
        /// </summary>
        private Window formGeneratorWindow;
        
        /// <summary>
        /// Представление неподключенных узлов.
        /// </summary>
        private FreeNodesView freeNodesView;

        /// <summary>
        /// Представление панели инструментов группового редактирования.
        /// </summary>
        private GroupAreaToolBarView groupAreaToolBarView;

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Представление списка сопоставленных объектов.
        /// </summary>
        private JurCompletedListView jurCompletedListView;

        /// <summary>
        /// Последний выбранный узел поворота.
        /// </summary>
        private InteractiveNode lastBendNode;

        /// <summary>
        /// Последний выбранный неподключенный узел.
        /// </summary>
        private InteractiveNode lastFreeNode;

        /// <summary>
        /// Значение, указывающее на то, что была ли скрыта информация о рисуемой линии при начале перемещения карты.
        /// </summary>
        private bool lineInfoHidden;

        /// <summary>
        /// Представление списка объектов.
        /// </summary>
        private ObjectListView objectListView;

        /// <summary>
        /// Представление панели инструментов области печати.
        /// </summary>
        private PrintToolBarView printToolBarView;

        /// <summary>
        /// Представление ошибок в сохраненных значениях параметров объектов.
        /// </summary>
        private SavedErrorsView savedErrorsView;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private readonly AccessService accessService;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly ShapeMapBindingService mapBindingService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        /// <summary>
        /// Сервис подложек.
        /// </summary>
        private readonly SubstrateService substrateService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainView"/>.
        /// </summary>
        /// <param name="loginName">Название логина.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        /// <param name="substrateService">Сервис подложек.</param>
        public MainView(string loginName, AccessService accessService, IDataService dataService, IMessageService messageService, ISettingService settingService, SubstrateService substrateService)
        {
            this.InitializeComponent();

            Application.Current.MainWindow = this;

            this.accessService = accessService;
            this.dataService = dataService;
            this.messageService = messageService;
            this.settingService = settingService;
            this.substrateService = substrateService;

            // Составляем словари с данными геометрий значков.
            var geometries = new Dictionary<ObjectType, Geometry>();
            var hotPoints = new Dictionary<ObjectType, Point>();
            var originPoints = new Dictionary<ObjectType, Point>();
            foreach (var entry in dataService.BadgeGeometries)
            {
                geometries.Add(entry.Type, (PathGeometry)XamlReader.Parse(entry.Geometry));
                hotPoints.Add(entry.Type, new Point(entry.HotPoint.X, entry.HotPoint.Y));
                originPoints.Add(entry.Type, new Point(entry.OriginPoint.X, entry.OriginPoint.Y));
            }

            // Замораживаем геометрии для повышения производительности.
            foreach (var geometry in geometries.Values)
                if (geometry.CanFreeze)
                    geometry.Freeze();

#warning Нужно будет переделать так, чтобы настройки вида карты подхватывались из источника данных
            this.mapBindingService = new ShapeMapBindingService(this.map, geometries, hotPoints, originPoints, this.accessService, new SqlMapSettingService(dataService));

            var reportService = new ReportService();

            reportService.AddedObjectInfoViewShowRequested += this.ReportService_AddedObjectInfoViewShowRequested;
            reportService.KtsViewShowRequested += this.ReportService_KtsViewShowRequested;
            reportService.TechSpecViewShowRequested += this.ReportService_TechSpecViewShowRequested;

            this.mainViewModel = new MainViewModel(loginName, accessService.RoleName, accessService, dataService, this.mapBindingService, messageService, reportService, settingService, substrateService);

            this.DataContext = this.mainViewModel;

            this.mainViewModel.MapViewModel.AddBadgeRequested += this.MapViewModel_AddBadgeRequested;
            this.mainViewModel.BendNodesRequested += this.MainViewModel_BendNodesRequested;
            this.mainViewModel.BendNodesViewModel.CloseRequested += this.BendNodesViewModel_CloseRequested;
            this.mainViewModel.BendNodesViewModel.OpenNodeRequested += this.BendNodesViewModel_OpenNodeRequested;
            this.mainViewModel.ChangeLengthViewRequested += this.MainViewModel_ChangeLengthViewRequested;
            this.mainViewModel.CloseRequested += this.MainViewModel_CloseRequested;
            this.mainViewModel.ExportRequested += this.MainViewModel_ExportRequested;
            this.mainViewModel.FreeNodesRequested += this.MainViewModel_FreeNodesRequested;
            this.mainViewModel.FreeNodesViewModel.CloseRequested += this.FreeNodesViewModel_CloseRequested;
            this.mainViewModel.FreeNodesViewModel.OpenNodeRequested += this.FreeNodesViewModel_OpenNodeRequested;
            this.mainViewModel.ImportRequested += this.MainViewModel_ImportRequested;
            this.mainViewModel.LongTimeTaskRequested += this.MainViewModel_LongTimeTaskRequested;
            this.mainViewModel.MapClosed += this.MainViewModel_MapClosed;
            this.mainViewModel.MapLoaded += this.MainViewModel_MapLoaded;
            this.mainViewModel.PasteRequested += this.MainViewModel_PasteRequested;
            this.mainViewModel.SaveAsImageRequested += this.MainViewModel_SaveAsImageRequested;
            this.mainViewModel.ErrorFound += this.MainViewModel_ErrorFound;
            this.mainViewModel.SavedErrorFound += this.MainViewModel_SavedErrorFound;
            this.mainViewModel.ShowLocalMapRequested += this.MainViewModel_ShowLocalMapRequested;
            this.mainViewModel.ValueInputRequested += this.MainViewModel_ValueInputRequested;
            this.mainViewModel.ErrorsViewModel.CloseRequested += this.ErrorsViewModel_CloseRequested;
            this.mainViewModel.ErrorsViewModel.ObjectOpenRequested += this.ErrorsViewModel_ObjectOpenRequested;
            this.mainViewModel.SavedErrorsViewModel.CloseRequested += this.SavedErrorsViewModel_CloseRequested;
            this.mainViewModel.SavedErrorsViewModel.ObjectOpenRequested += this.SavedErrorsViewModel_ObjectOpenRequested;
            this.mainViewModel.MapViewModel.PrinterSelectionRequested += this.MapViewModel_PrinterSelectionRequested;
            this.mainViewModel.SchemaRequested += this.MainViewModel_SchemaRequested;
            this.mainViewModel.FindRequested += this.MainViewModel_FindRequested;
            this.mainViewModel.ObjectListViewModel.CloseRequested += this.ObjectListViewModel_CloseRequested;
            this.mainViewModel.ObjectListViewModel.OpenObjectRequested += this.ObjectListViewModel_OpenObjectRequested;
            this.mainViewModel.ObjectListRequested += this.MainViewModel_ObjectListRequested;
            this.mainViewModel.PrintToolBarCloseRequested += this.MainViewModel_PrintToolBarCloseRequested;
            this.mainViewModel.PrintToolBarRequested += this.MainViewModel_PrintToolBarRequested;
            this.mainViewModel.GroupAreaToolBarCloseRequested += this.MainViewModel_GroupAreaToolBarCloseRequested;
            this.mainViewModel.GroupAreaToolBarRequested += this.MainViewModel_GroupAreaToolBarRequested;
            this.mainViewModel.LayersSettingsRequested += this.MainViewModel_LayersSettingsRequested;
            this.mainViewModel.MapViewModel.MapSettingsViewRequested += this.MapViewModel_MapSettingsViewRequested;
            this.mainViewModel.MapViewModel.CaptionManageViewRequested += this.MapViewModel_CaptionManageViewRequested;

            this.mainViewModel.CustomLayersViewModel.ViewRequested += this.CustomLayersViewModel_ViewRequested;
            this.mainViewModel.CustomLayersViewModel.BoilerNeeded += this.CustomLayersViewModel_BoilerNeeded;

            this.mainViewModel.ManageCustomObjectLayer += this.MainViewModel_ManageCustomObjectLayer;

            this.mainViewModel.DocumentViewRequested += this.MainViewModel_DocumentViewRequested;

            this.mainViewModel.JurKvpCompletedListViewModel.CloseRequested += this.JurCompletedListViewModel_CloseRequested;
            this.mainViewModel.JurKvpCompletedListViewModel.OpenObjectRequested += this.JurCompletedListViewModel_OpenObjectRequested;
            this.mainViewModel.JurCompletedListRequested += this.MainViewModel_JurCompletedListRequested;
            
            this.mainViewModel.KtsLeftViewRequested += this.MainViewModel_KtsLeftViewRequested;

            // Создаем автоматическую легенду карты.
            var autoLegendViewModel = new AutoLegendViewModel(this.mainViewModel, this.mapBindingService);
            this.map.AutoLegend = new AutoLegend(autoLegendViewModel);

            this.map.CtrlClicked += this.map_CtrlClicked;
            this.map.DrawingCompleted += this.map_DrawingCompleted;
            this.map.DrawingDelta += this.map_DrawingDelta;
            this.map.DrawingStarted += this.map_DrawingStarted;
            this.map.GroupAreaAngleChanged += this.map_GroupAreaAngleChanged;
            this.map.GroupAreaChangeCanceled += this.map_GroupAreaChangeCanceled;
            this.map.GroupAreaPositionChanged += this.map_GroupAreaPositionChanged;
            this.map.GroupAreaScaleChanged += this.map_GroupAreaScaleChanged;
            this.map.PasteCanceled += this.map_PasteCanceled;
            this.map.PasteCompleted += this.map_PasteCompleted;
            this.map.ScrollChanged += this.map_ScrollChanged;
            this.map.ScrollEnded += this.map_ScrollEnded;
            this.map.ScrollStarted += this.map_ScrollStarted;
            this.map.SelectionChanged += this.map_SelectionChanged;
            this.map.ShiftClicked += this.map_ShiftClicked;
            this.map.TextPasted += this.map_TextPasted;

            // Задаем контекст данных для раздела поиска.
            var searchViewModel = new SearchViewModel(this.mainViewModel, accessService, dataService, messageService);
            this.searchView.SetDataContext(searchViewModel);

            // Подписываемся на событие запроса отображения объекта на карте.
            searchViewModel.ShowOnMapRequested += this.SearchViewModel_ShowOnMapRequested;

            // Следим за необходимостью выбора параметров при вставке их значений.
            ObjectViewModel.ClipboardManager.ParameterSelectRequested += this.ClipboardManager_ParameterSelectRequested;

            FormGenerator.CloseAllFormsRequested += this.FormGenerator_CloseAllFormsRequested;
            FormGenerator.PartitionActDialog += this.FormGenerator_PartitionActDialog;
        }

        #endregion

        #region Деструкторы

        /// <summary>
        /// Финализирует экземпляр класса <see cref="MainView"/>.
        /// </summary>
        ~MainView()
        {
            this.Dispose(false);
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> представления узлов поворота.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BendNodesView_Closed(object sender, EventArgs e)
        {
            this.bendNodesView.Closed -= this.BendNodesView_Closed;

            if (this.lastBendNode != null)
            {
                this.lastBendNode.HideTriangle();

                this.lastBendNode = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="BendNodesViewModel.CloseRequested"/> модели представления узлов поворота.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BendNodesViewModel_CloseRequested(object sender, EventArgs e)
        {
            if (this.bendNodesView != null)
            {
                this.bendNodesView.Close();

                this.bendNodesView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="BendNodesViewModel.OpenNodeRequested"/> модели представления узлов поворота.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BendNodesViewModel_OpenNodeRequested(object sender, EventArgs e)
        {
            // Если уже был выбран узел поворота, то убираем с него треугольник.
            if (this.lastBendNode != null)
                this.lastBendNode.HideTriangle();
                
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            // Обновляем карту.
            this.map.UpdateLayout();

            var node = this.mapBindingService.GetMapObjectView(this.mainViewModel.BendNodesViewModel.SelectedNode) as InteractiveNode;

            if (node != null)
            {
                // Перемещаем вид карты к узлу.
                this.map.ScrollToAndCenter(node.CenterPoint);

                this.lastBendNode = node;

                this.lastBendNode.ShowTriangle();
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="CustomLayersViewModel.BoilerNeeded"/> модели представления кастомных слоев.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void CustomLayersViewModel_BoilerNeeded(object sender, NeedBoilerEventArgs e)
        {
            var view = new SelectBoilerView(e.ViewModel)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="CustomLayersViewModel.ViewRequested"/> модели представления кастомных слоев.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void CustomLayersViewModel_ViewRequested(object sender, EventArgs e)
        {
            var view = new CustomLayersView(this.mainViewModel.CustomLayersViewModel)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FormGenerator.CloseAllFormsRequested"/> генератора форм.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FormGenerator_CloseAllFormsRequested(object sender, EventArgs e)
        {
            if (this.formGeneratorWindow != null)
            {
                this.formGeneratorWindow.Close();

                this.formGeneratorWindow = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FormGenerator.PartitionActDialog"/> генератора форм.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FormGenerator_PartitionActDialog(object sender, Utilities.ViewRequestedEventArgs<Reports.Models.PartitionActModel> e)
        {
            if (this.formGeneratorWindow != null)
                return;

            this.formGeneratorWindow = new PartitionActDialog(new PartitionActViewModel(this.mainViewModel, this.messageService), this.map, this.messageService)
            {
                Owner = this
            };

            this.formGeneratorWindow.Closed += this.FormGeneratorWindow_Closed;

            this.formGeneratorWindow.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> формы генератора форм.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FormGeneratorWindow_Closed(object sender, EventArgs e)
        {
            this.formGeneratorWindow.Closed -= this.FormGeneratorWindow_Closed;

            this.formGeneratorWindow = null;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FreeNodesViewModel.OpenNodeRequested"/> модели представления неподключенных узлов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FreeNodesViewModel_CloseRequested(object sender, EventArgs e)
        {
            if (this.freeNodesView != null)
            {
                this.freeNodesView.Close();

                this.freeNodesView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FreeNodesViewModel.CloseRequested"/> модели представления неподключенных узлов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FreeNodesViewModel_OpenNodeRequested(object sender, EventArgs e)
        {
            // Если уже был выбран неподключенный узел, то убираем с него треугольник.
            if (this.lastFreeNode != null)
                this.lastFreeNode.HideTriangle();

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            // Обновляем карту.
            this.map.UpdateLayout();

            var node = this.mapBindingService.GetMapObjectView(this.mainViewModel.FreeNodesViewModel.SelectedNode) as InteractiveNode;

            if (node != null)
            {
                // Перемещаем вид карты к узлу.
                this.map.ScrollToAndCenter(node.CenterPoint);

                this.lastFreeNode = node;

                this.lastFreeNode.ShowTriangle();
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ClipboardManager.ParameterSelectRequested"/> менеджера буфера обмена.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ClipboardManager_ParameterSelectRequested(object sender, ParameterSelectRequestedEventArgs e)
        {
            var viewModel = new ParameterSelectionViewModel(e.AllParameters);

            var view = new ParameterSelectionView(viewModel)
            {
                Owner = this
            };

            view.ShowDialog();

            e.SelectedParameters.AddRange(viewModel.SelectedParameters);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ContextMenu.Opened"/> контекстного меню.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            // Обновляем возможности выполнения всех команд контекстного меню.
            foreach (var item in (sender as ContextMenu).Items)
            {
                var menuItem = item as MenuItem;

                if (menuItem != null)
                    if (menuItem.Command != null)
                        (menuItem.Command as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ErrorsViewModel.CloseRequested"/> модели представления ошибок.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ErrorsViewModel_CloseRequested(object sender, EventArgs e)
        {
            if (this.errorsView != null)
            {
                this.errorsView.Close();

                this.errorsView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ErrorsViewModel.ObjectOpenRequested"/> модели представления ошибок.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ErrorsViewModel_ObjectOpenRequested(object sender, EventArgs e)
        {
            var obj = this.mainViewModel.ErrorsViewModel.SelectedItem.Object;

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            // Обновляем карту.
            this.map.UpdateLayout();

            // Перемещаем вид карты к объекту.
            this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView((IMapObjectViewModel)obj) as IInteractiveShape).CenterPoint);

            var selectable = obj as ISelectableObjectViewModel;

            if (selectable != null)
                selectable.IsSelected = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> представления неподключенных узлов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FreeNodesView_Closed(object sender, EventArgs e)
        {
            this.freeNodesView.Closed -= this.FreeNodesView_Closed;

            if (this.lastFreeNode != null)
            {
                this.lastFreeNode.HideTriangle();

                this.lastFreeNode = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewKeyDown"/> сетки затемнения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void frozenGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseDown"/> сетки затемнения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void frozenGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> представления списка сопоставленных объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void JurCompletedListView_Closed(object sender, EventArgs e)
        {
            this.jurCompletedListView.Closed -= this.JurCompletedListView_Closed;

            if (((sender as Window).DataContext as JurKvpCompletedListViewModel).HasChanges)
            {
                this.mainViewModel.PrepareSchema();

                ((sender as Window).DataContext as JurKvpCompletedListViewModel).HasChanges = false;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="JurKvpCompletedListViewModel.CloseRequested"/> модели представления списка сопоставленных объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void JurCompletedListViewModel_CloseRequested(object sender, EventArgs e)
        {
            if (this.jurCompletedListView != null)
            {
                this.jurCompletedListView.Close();

                this.jurCompletedListView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="JurKvpCompletedListViewModel.OpenObjectRequested"/> модели представления списка сопоставленных объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void JurCompletedListViewModel_OpenObjectRequested(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            var obj = this.mainViewModel.JurKvpCompletedListViewModel.SelectedObject;

            IInteractiveShape shape = null;

            IObjectViewModel realObj;

            realObj = this.mainViewModel.GetObject(obj.Item1);
            var mapObject = realObj as IMapObjectViewModel;
            if (mapObject != null)
                shape = this.mapBindingService.GetMapObjectView(mapObject) as IInteractiveShape;

            if (shape == null && obj.Item2 != Guid.Empty)
            {
                realObj = this.mainViewModel.GetObject(obj.Item2);

                shape = this.mapBindingService.GetMapObjectView(realObj as IMapObjectViewModel) as IInteractiveShape;
            }

            if (shape != null)
            {
                // Обновляем карту.
                this.map.UpdateLayout();

                // Перемещаем вид карты к объекту.
                this.map.ScrollToAndCenter(shape.CenterPoint);

                var selectable = realObj as ISelectableObjectViewModel;

                if (selectable != null)
                    selectable.IsSelected = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> списка надписей.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ListBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var view = this.mapBindingService.GetMapObjectView(this.listBoxCustomObjects.SelectedItem as ICustomLayerObject);

            Point point = new Point();

            if (view is SmartIndependentLabel)
                point = (view as SmartIndependentLabel).CenterPoint;
            else
                if (view is LengthPerDiameterTable)
                    point = (view as LengthPerDiameterTable).CenterPoint;
            
            this.map.ScrollToAndCenter(point);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> списка результатов быстрого поиска.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ListBoxSearchItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var id = (this.listBoxSearchItems.SelectedItem as Tuple<Guid, string>).Item1;

            var obj = this.mainViewModel.GetObject(id);

            if (obj != null)
            {
                var mapObj = this.mapBindingService.GetMapObjectView(obj as IMapObjectViewModel);

                if (mapObj != null)
                    this.map.ScrollToAndCenter((mapObj as IInteractiveShape).CenterPoint);

                var selectable = obj as ISelectableObjectViewModel;

                if (selectable != null)
                    selectable.IsSelected = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.BendNodesRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_BendNodesRequested(object sender, EventArgs e)
        {
            if (this.bendNodesView != null)
                return;

            this.bendNodesView = new BendNodesView(this.mainViewModel.BendNodesViewModel)
            {
                Owner = this
            };

            this.bendNodesView.Closed += this.BendNodesView_Closed;

            this.bendNodesView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ChangeLengthViewRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ChangeLengthViewRequested(object sender, ChangeLengthViewRequestedEventArgs e)
        {
            var view = new ChangeLengthView(e.ViewModel)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.CloseRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.DocumentViewRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_DocumentViewRequested(object sender, Utilities.ViewRequestedEventArgs<DocumentsViewModel> e)
        {
            var view = new DocumentsView(e.ViewModel)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ErrorFound"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ErrorFound(object sender, EventArgs e)
        {
            if (this.errorsView != null)
                return;

            this.errorsView = new ErrorsView(this.mainViewModel.ErrorsViewModel)
            {
                Owner = this
            };

            this.errorsView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ExportRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ExportRequested(object sender, ImportExportRequestedEventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                Filter = "XLF (*.xlf)|*.xlf",
                OverwritePrompt = true,
                RestoreDirectory = true,
                Title = "Экспорт данных"
            };

            var result = dlg.ShowDialog();

            if (result.HasValue && result.Value == true)
                e.FileName = dlg.FileName;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.FindRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_FindRequested(object sender, EventArgs e)
        {
            var view = new FindObjectView(this.messageService)
            {
                Owner = this
            };

            view.OpenObjectRequested += this.View_OpenObjectRequested;

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.FreeNodesRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_FreeNodesRequested(object sender, EventArgs e)
        {
            if (this.freeNodesView != null)
                return;

            this.freeNodesView = new FreeNodesView(this.mainViewModel.FreeNodesViewModel)
            {
                Owner = this
            };

            this.freeNodesView.Closed += this.FreeNodesView_Closed;

            this.freeNodesView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.GroupAreaToolBarCloseRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_GroupAreaToolBarCloseRequested(object sender, EventArgs e)
        {
            if (this.groupAreaToolBarView != null)
            {
                this.groupAreaToolBarView.Close();

                this.groupAreaToolBarView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.GroupAreaToolBarRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_GroupAreaToolBarRequested(object sender, EventArgs e)
        {
            if (this.groupAreaToolBarView != null)
                return;

            this.groupAreaToolBarView = new GroupAreaToolBarView(this.mainViewModel.GroupAreaToolBar)
            {
                Owner = this
            };

            this.groupAreaToolBarView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ImportRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ImportRequested(object sender, ImportExportRequestedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Filter = "XLF (*.xlf)|*.xlf",
                RestoreDirectory = true,
                Title = "Импорт данных"
            };

            var result = dlg.ShowDialog();

            if (result.HasValue && result.Value == true)
                e.FileName = dlg.FileName;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.JurCompletedListRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_JurCompletedListRequested(object sender, EventArgs e)
        {
            if (this.jurCompletedListView != null)
                return;

            this.jurCompletedListView = new JurCompletedListView(this.mainViewModel.JurKvpCompletedListViewModel)
            {
                Owner = this
            };

            this.jurCompletedListView.Closed += this.JurCompletedListView_Closed;

            this.jurCompletedListView.Show();
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.KtsLeftViewRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_KtsLeftViewRequested(object sender, Utilities.ViewRequestedEventArgs<KtsLeftoversViewModel> e)
        {
            var view = new KtsLeftoversView(e.ViewModel)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.LayersSettingsRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_LayersSettingsRequested(object sender, EventArgs e)
        {
            var view = new LayersSettingsView(this.settingService)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.LongTimeTaskRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_LongTimeTaskRequested(object sender, LongTimeTaskRequestedEventArgs e)
        {
            var view = new WaitView(e.WaitViewModel, this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ManageCustomObjectLayer"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ManageCustomObjectLayer(object sender, ManageCustomObjectLayerEventArgs e)
        {
            var view = new ManageCustomObjectLayerView(new ManageCustomObjectLayerViewModel(e.CustomLayerObject, e.Layers))
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.MapClosed"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_MapClosed(object sender, EventArgs e)
        {
            if (this.mainViewModel.CitySelectionViewModel.LoadedCity != null)
                // Запоминаем масштаб и отступы загруженной карты.
                (this.settingService.Settings["MapPositions"] as Dictionary<int, Tuple<double, Size>>)[this.mainViewModel.CitySelectionViewModel.LoadedCity.Id] = new Tuple<double, Size>(this.map.Scale, new Size(this.map.MapHorizontalOffset, this.map.MapVerticalOffset));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.MapLoaded"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_MapLoaded(object sender, EventArgs e)
        {
            var dictionary = this.settingService.Settings["MapPositions"] as Dictionary<int, Tuple<double, Size>>;

            var cityId = this.mainViewModel.CitySelectionViewModel.LoadedCity.Id;

            if (this.mainViewModel.LoadedBoilers.Count == 0)
                if (dictionary.ContainsKey(cityId))
                {
                    var tuple = dictionary[cityId];

                    this.map.Scale = tuple.Item1;

                    // Скроллим карту к сохраненной позиции.
                    this.map.ScrollTo(tuple.Item2.Width, tuple.Item2.Height);
                }
                else
                {
                    this.map.Scale = 1;

                    // Иначе, скроллим к центру.
                    this.map.ScrollToCenter();
                }
            else
            {
                this.map.Scale = 1;

                this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView(this.mainViewModel.GetObject(this.mainViewModel.LoadedBoilers[0]) as IMapObjectViewModel) as IInteractiveShape).CenterPoint);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ObjectListRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ObjectListRequested(object sender, EventArgs e)
        {
            if (this.objectListView != null)
                return;

            this.objectListView = new ObjectListView(this.mainViewModel.ObjectListViewModel)
            {
                Owner = this
            };

            this.objectListView.Closed += this.ObjectListView_Closed;

            this.objectListView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.PasteRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_PasteRequested(object sender, PasteRequestedEventArgs e)
        {
            this.map.StartPaste(this.mapBindingService.GetMapObjectView(e.MapObject));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.PrintToolBarCloseRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_PrintToolBarCloseRequested(object sender, EventArgs e)
        {
            if (this.printToolBarView != null)
            {
                this.printToolBarView.Close();

                this.printToolBarView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.PrintToolBarRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_PrintToolBarRequested(object sender, EventArgs e)
        {
            if (this.printToolBarView != null)
                return;

            this.printToolBarView = new PrintToolBarView(this.map)
            {
                Owner = this
            };

            this.printToolBarView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.SaveAsImageRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_SaveAsImageRequested(object sender, EventArgs e)
        {
            var view = new ImageExportView(new ImageExportViewModel(new Utilities.Size(this.map.MapSize.Width, this.map.MapSize.Height), this.mainViewModel.CitySelectionViewModel.LoadedCity.Name, this.mainViewModel.MapViewModel, this.messageService), this.map)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.SavedErrorFound"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_SavedErrorFound(object sender, EventArgs e)
        {
            if (this.savedErrorsView != null)
                return;

            this.savedErrorsView = new SavedErrorsView(this.mainViewModel.SavedErrorsViewModel)
            {
                Owner = this
            };

            this.savedErrorsView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.SchemaRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_SchemaRequested(object sender, SchemaRequestedEventArgs e)
        {
            var viewModel = new SchemaSelectionViewModel(e.City, this.dataService, this.settingService);

            var view = new SchemaSelectionView(viewModel, this.dataService, this.messageService)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
            {
                e.Schema = viewModel.SelectedSchema;

                if (!viewModel.IsAllBoilersSelected)
                    foreach (var boiler in viewModel.SelectedBoilers)
                        e.BoilerIds.Add(boiler.Id);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ShowLocalMapRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ShowLocalMapRequested(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.ValueInputRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_ValueInputRequested(object sender, ValueInputRequestedEventArgs e)
        {
            // Заполняем дополнительные опции.
            var additionalOptions = new List<OptionModel>();
            additionalOptions.Add(new OptionModel("Все", true));
            foreach (var type in e.LineTypes)
                additionalOptions.Add(new OptionModel(type.Name, false));

            var options = new List<OptionModel>()
            {
                new OptionModel("Только для несохраненных линий", true),
                new OptionModel("Для всех линий", false)
            };

            var viewModel = new ValueInputViewModel(e.Content, e.Caption, e.ValueType, e.InitialValue, options, additionalOptions, "Тип линий:", "ВНИМАНИЕ: Пересчет длин будет выполнен только для планируемых линий", this.messageService);

            // Отображаем представление ввода значения.
            var view = new ValueInputView(viewModel)
            {
                Icon = this.Icon,
                Owner = this
            };
            if (view.ShowDialog().Value)
            {
                e.Result = viewModel.Result;

                for (int i = 0; i < additionalOptions.Count; i++)
                    if (additionalOptions[i].IsSelected)
                        if (i != 0)
                            e.LineType = e.LineTypes[i - 1];
                        else
                            e.LineType = null;

                for (int i = 0; i < options.Count; i++)
                    if (options[i].IsSelected)
                    {
                        switch (i)
                        {
                            case 0:
                                e.Option = LengthUpdateOption.OnlyNonSaved;

                                break;

                            case 1:
                                e.Option = LengthUpdateOption.All;

                                break;
                        }

                        break;
                    }
            }
            else
                e.Result = null;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.AddBadgeRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_AddBadgeRequested(object sender, AddBadgeRequestedEventArgs e)
        {
            var viewModel = new AddBadgeViewModel(e.Line, this.messageService);

            var view = new AddBadgeView(viewModel, this.mapBindingService)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
                e.Result = viewModel.Result;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MapViewModel.CaptionManageViewRequested"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_CaptionManageViewRequested(object sender, Utilities.ViewRequestedEventArgs<CaptionManageViewModel> e)
        {
            var view = new CaptionManageView(e.ViewModel)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
                e.Result = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MapViewModel.MapSettingsViewRequested"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_MapSettingsViewRequested(object sender, MapSettingsViewRequestedEventArgs e)
        {
            var view = new MapSettingsView(e.ViewModel)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
                e.Result = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MapViewModel.PrinterSelectionRequested"/> модели представления карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MapViewModel_PrinterSelectionRequested(object sender, PrinterSelectionRequestedEventArgs e)
        {
            var viewModel = new PrinterSelectionViewModel();

            var view = new PrinterSelectionView(viewModel)
            {
                Owner = this
            };

            var result = view.ShowDialog();

            if (result.HasValue && result.Value)
                e.SelectedPrinter = viewModel.SelectedPrinter;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.CtrlClicked"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_CtrlClicked(object sender, ShiftClickedEventArgs e)
        {
            this.mainViewModel.AddOrRemoveSelectedObject((IObjectViewModel)this.mapBindingService.GetMapObjectViewModel(e.Shape));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.DrawingCompleted"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_DrawingCompleted(object sender, DrawingCompletedEventArgs e)
        {
            if (!e.IsForced)
            {
                e.IsCanceled = !(this.mapBindingService.GetMapObjectView(this.drawingObject) as IDrawableObject).IsDrawCompleted(e.MousePosition);

                if (!e.IsCanceled)
                {
                    this.mainViewModel.CompleteDraw();

                    if (this.drawingObject is PolylineViewModel || this.drawingObject is NewRulerViewModel)
                        this.mainViewModel.LineInfo.IsVisible = false;
                }
            }
            else
            {
                this.mainViewModel.CancelDraw();

                if (this.drawingObject is PolylineViewModel || this.drawingObject is NewRulerViewModel)
                    this.mainViewModel.LineInfo.IsVisible = false;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.DrawingDelta"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_DrawingDelta(object sender, DrawingEventArgs e)
        {
            (this.mapBindingService.GetMapObjectView(this.drawingObject) as IDrawableObject).Draw(e.MousePrevPosition, e.MousePosition);

            if (this.drawingObject is PolylineViewModel || this.drawingObject is NewRulerViewModel)
            {
                var position = Mouse.GetPosition(this.gridLocalMap);
                
                this.mainViewModel.LineInfo.Left = position.X + SystemParameters.CursorWidth;
                this.mainViewModel.LineInfo.Top = position.Y + SystemParameters.CursorHeight;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.DrawingStarted"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_DrawingStarted(object sender, DrawingEventArgs e)
        {
            this.drawingObject = this.mainViewModel.StartDraw(e.MousePosition);

            if (this.drawingObject is PolylineViewModel || this.drawingObject is NewRulerViewModel)
            {
                var position = Mouse.GetPosition(this.gridLocalMap);

                this.mainViewModel.LineInfo.Left = position.X + SystemParameters.CursorWidth;
                this.mainViewModel.LineInfo.Top = position.Y + SystemParameters.CursorHeight;

                this.mainViewModel.LineInfo.IsVisible = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.GroupAreaAngleChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_GroupAreaAngleChanged(object sender, AngleChangedEventArgs e)
        {
            this.LockUI();

            var view = new FreezeView(new FreezeViewModel("Поворот объектов", "Пожалуйста подождите, идет поворот объектов...", () => this.mainViewModel.RotateEditingObjects(e.Angle, e.OriginPoint)))
            {
                Icon = this.Icon,
                Owner = this
            };

            view.ShowDialog();

            this.UnlockUI();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.GroupAreaChangeCanceled"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_GroupAreaChangeCanceled(object sender, EventArgs e)
        {
            this.mainViewModel.RedrawOutlines();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.GroupAreaPositionChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_GroupAreaPositionChanged(object sender, PositionChangedEventArgs e)
        {
            this.LockUI();

            var view = new FreezeView(new FreezeViewModel("Перемещение объектов", "Пожалуйста подождите, идет перемещение объектов...", () => this.mainViewModel.MoveEditingObjects(e.Delta)))
            {
                Icon = this.Icon,
                Owner = this
            };

            view.ShowDialog();

            this.UnlockUI();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.GroupAreaScaleChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_GroupAreaScaleChanged(object sender, ScaleChangedEventArgs e)
        {
            this.LockUI();

            var view = new FreezeView(new FreezeViewModel("Масштабирование объектов", "Пожалуйста подождите, идет масштабирование объектов...", () => this.mainViewModel.ScaleEditingObjects(e.Scale, e.OriginPoint)))
            {
                Icon = this.Icon,
                Owner = this
            };

            view.ShowDialog();

            this.UnlockUI();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.PasteCompleted"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_PasteCompleted(object sender, EventArgs e)
        {
            this.mainViewModel.CompletePaste();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.PasteCanceled"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_PasteCanceled(object sender, EventArgs e)
        {
            this.mainViewModel.CancelPaste();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ScrollChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_ScrollChanged(object sender, EventArgs e)
        {
            if (this.mainViewModel.BoilerInfo.IsBoilerInfoVisible)
                this.mainViewModel.BoilerInfo.UnsetBoiler();
            if (this.mainViewModel.BuildingInfo.IsBuildingInfoVisible)
                this.mainViewModel.BuildingInfo.UnsetBuilding();
            if (this.mainViewModel.StorageInfo.IsStorageInfoVisible)
                this.mainViewModel.StorageInfo.UnsetStorage();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ScrollEnded"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_ScrollEnded(object sender, EventArgs e)
        {
            if (this.lineInfoHidden)
            {
                this.mainViewModel.LineInfo.IsVisible = true;

                this.lineInfoHidden = false;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ScrollStarted"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_ScrollStarted(object sender, EventArgs e)
        {
            if (this.mainViewModel.LineInfo.IsVisible)
            {
                this.mainViewModel.LineInfo.IsVisible = false;

                this.lineInfoHidden = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.SelectionChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_SelectionChanged(object sender, EventArgs e)
        {
            var list = new List<IObjectViewModel>();

            foreach (var shape in this.map.SelectedShapes)
                list.Add((IObjectViewModel)this.mapBindingService.GetMapObjectViewModel(shape));

            this.mainViewModel.SetSelectedObjects(list);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ShiftClicked"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_ShiftClicked(object sender, ShiftClickedEventArgs e)
        {
            var viewModel = this.mapBindingService.GetMapObjectViewModel(e.Shape);

            this.mainViewModel.AddSelectedObject((IObjectViewModel)viewModel);

            if (viewModel is LineViewModel)
                this.mainViewModel.FindWayToOtherLines(viewModel as LineViewModel);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.TextPasted"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_TextPasted(object sender, LabelPastedEventArgs e)
        {
            this.mainViewModel.InsertLabel(new Utilities.Point(e.Position.X, e.Position.Y));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Сменить пароль...".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var view = new ChangePasswordView(new ChangePasswordViewModel(this.dataService, this.messageService))
            {
                Owner = this,
                ShowInTaskbar = false
            };

            view.ShowDialog();
        }

        private void MenuItemChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            var view = new ChangeTheme(this.settingService); 
            view.ShowDialog();
        }

        private void MenuItemBoilerMeter_Click(object sender, RoutedEventArgs e)
        {
            var view = new MetersView(dataService, messageService, settingService);
            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Сменить пользователя...".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemChangeUser_Click(object sender, RoutedEventArgs e)
        {
            var authorizationView = new AuthorizationView(this.messageService, this.settingService, this.substrateService);

            this.Close();

            authorizationView.Show();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Экспорт в PNG-изображение...".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemExportToPng_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = "Выберите папку, куда нужно будет сохранить изображение/изображения:",
                ShowNewFolderButton = true
            };

            var dr = dlg.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                var viewModel = new SelectDpiViewModel();

                var view = new SelectDpiView(viewModel)
                {
                    Owner = this
                };

                var result = view.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    this.messageService.ShowMessage("Нажмите на кнопку ОК, чтобы продолжить экспорт." + Environment.NewLine + Environment.NewLine + "ВНИМАНИЕ: Ничего не нажимайте во время экспорта, дождитесь появления окна о его завершении.", "Экспорт", MessageType.Information);

                    this.map.PrintAsPNG(dlg.SelectedPath, viewModel.SelectedDpi);

                    this.messageService.ShowMessage("Экспорт завершен", "Экспорт", MessageType.Information);
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Руководство пользователя".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemManual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("Docs\\Руководство пользователя.pdf");
            }
            catch
            {
                this.messageService.ShowMessage("Не удалось отобразить руководство пользователя", "Руководство пользователя", MessageType.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Печать".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemPrint_Click(object sender, RoutedEventArgs e)
        {
            this.messageService.ShowMessage("Нажмите на кнопку ОК, чтобы продолжить печать." + Environment.NewLine + Environment.NewLine + "ВНИМАНИЕ: Ничего не нажимайте во время печати, дождитесь появления окна о ее завершении.", "Печать", MessageType.Information);

            try
            {
                this.map.Print();
            }
            catch (Exception exception)
            {
                this.messageService.ShowMessage("Возникла ошибка при печати, сообщите о ней разработчикам приложения: " + exception.Message, "Печать", MessageType.Error);

                return;
            }

            this.messageService.ShowMessage("Печать завершена", "Печать", MessageType.Information);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Печать в XPS-файл".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemPrintAsXPS_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                Filter = "XPS-файл (*.xps)|*.xps",
                OverwritePrompt = true,
                RestoreDirectory = true,
                Title = "Печать в XPS-файл"
            };

            var result = dlg.ShowDialog();

            if (result.HasValue && result.Value == true)
            {
                this.messageService.ShowMessage("Нажмите на кнопку ОК, чтобы продолжить печать." + Environment.NewLine + Environment.NewLine + "ВНИМАНИЕ: Ничего не нажимайте во время печати, дождитесь появления окна о ее завершении.", "Печать в XPS-файл", MessageType.Information);

                try
                {
                    this.map.PrintAsXPS(dlg.FileName);
                }
                catch (Exception exception)
                {
                    this.messageService.ShowMessage("Возникла ошибка при печати в XPS-файл, сообщите о ней разработчикам приложения: " + exception.Message, "Печать в XPS-файл", MessageType.Error);

                    return;
                }

                this.messageService.ShowMessage("Печать завершена", "Печать в XPS-файл", MessageType.Information);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Печать через XPS-файл".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemPrintXPS_Click(object sender, RoutedEventArgs e)
        {
            this.messageService.ShowMessage("Нажмите на кнопку ОК, чтобы продолжить печать." + Environment.NewLine + Environment.NewLine + "ВНИМАНИЕ: Ничего не нажимайте во время печати, дождитесь появления окна о ее завершении.", "Печать через XPS-файл", MessageType.Information);

            try
            {
                this.map.PrintXPS();
            }
            catch (Exception exception)
            {
                this.messageService.ShowMessage("Возникла ошибка при печати через XPS-файл, сообщите о ней разработчикам приложения: " + exception.Message, "Печать через XPS-файл", MessageType.Error);

                return;
            }

            this.messageService.ShowMessage("Печать завершена", "Печать через XPS-файл", MessageType.Information);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Об уровне отрисовки...".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemTier_Click(object sender, RoutedEventArgs e)
        {
            this.messageService.ShowMessage("Текущий уровень отрисовки: " + (RenderCapability.Tier >> 16), "Об уровне отрисовки", MessageType.Information);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "Об обновлении...".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            var view = new UpdateInfoView()
            {
                Owner = this,
                ShowInTaskbar = false
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню "О версии приложения...".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemVersion_Click(object sender, RoutedEventArgs e)
        {
            this.messageService.ShowMessage("Текущая версия приложения: " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(), "О версии приложения", MessageType.Information);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> представления списка объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ObjectListView_Closed(object sender, EventArgs e)
        {
            this.objectListView.Closed -= this.ObjectListView_Closed;

            if (((sender as Window).DataContext as ObjectListViewModel).HasChanges)
            {
                this.mainViewModel.PrepareSchema();

                ((sender as Window).DataContext as ObjectListViewModel).HasChanges = false;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ObjectListViewModel.CloseRequested"/> модели представления списка объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ObjectListViewModel_CloseRequested(object sender, EventArgs e)
        {
            if (this.objectListView != null)
            {
                this.objectListView.Close();

                this.objectListView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ObjectListViewModel.OpenObjectRequested"/> модели представления списка объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ObjectListViewModel_OpenObjectRequested(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            var obj = this.mainViewModel.ObjectListViewModel.SelectedObject;

            IInteractiveShape shape = null;

            IObjectViewModel realObj;

            realObj = this.mainViewModel.GetObject(obj.Item1);
            var mapObject = realObj as IMapObjectViewModel;
            if (mapObject != null)
                shape = this.mapBindingService.GetMapObjectView(mapObject) as IInteractiveShape;

            if (shape == null && obj.Item2 != Guid.Empty)
            {
                realObj = this.mainViewModel.GetObject(obj.Item2);

                shape = this.mapBindingService.GetMapObjectView(realObj as IMapObjectViewModel) as IInteractiveShape;
            }

            if (shape != null)
            {
                // Обновляем карту.
                this.map.UpdateLayout();

                // Перемещаем вид карты к объекту.
                this.map.ScrollToAndCenter(shape.CenterPoint);

                var selectable = realObj as ISelectableObjectViewModel;

                if (selectable != null)
                    selectable.IsSelected = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref=System.Windows.Controls.Primitives.Popup.Opened"/> всплывающего окна с информацией о котельной.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Popup_Opened(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemDiameters.IsSelected = true));
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="ReportService.AddedObjectInfoViewShowRequested"/> сервиса отчетов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReportService_AddedObjectInfoViewShowRequested(object sender, EventArgs e)
        {
            var view = new AddedObjectInfoView(new AddedObjectInfoViewModel(this.dataService))
            {
                Icon = this.Icon,
                Owner = this
            };

            view.ShowDialog();
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="ReportService.KtsViewShowRequested"/> сервиса отчетов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReportService_KtsViewShowRequested(object sender, EventArgs e)
        {
            var view = new KtsReportView(new KtsReportViewModel(this.accessService.PermittedRegions, this.dataService))
            {
                Icon = this.Icon,
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ReportService.TechSpecViewShowRequested"/> сервиса отчетов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReportService_TechSpecViewShowRequested(object sender, EventArgs e)
        {
            var view = new TechSpecView(new TechSpecViewModel(this.accessService.PermittedRegions, this.dataService))
            {
                Icon = this.Icon,
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="SavedErrorsViewModel.CloseRequested"/> модели представления ошибок в сохраненных значениях параметров объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SavedErrorsViewModel_CloseRequested(object sender, EventArgs e)
        {
            if (this.savedErrorsView != null)
            {
                this.savedErrorsView.Close();

                this.savedErrorsView = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="SavedErrorsViewModel.ObjectOpenRequested"/> модели представления ошибок в сохраненных значениях параметров объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SavedErrorsViewModel_ObjectOpenRequested(object sender, EventArgs e)
        {
            var obj = this.mainViewModel.SavedErrorsViewModel.SelectedItem;

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            // Обновляем карту.
            this.map.UpdateLayout();

            // Перемещаем вид карты к объекту.
            this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView((IMapObjectViewModel)obj) as IInteractiveShape).CenterPoint);

            var selectable = obj as ISelectableObjectViewModel;

            if (selectable != null)
                selectable.IsSelected = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="SearchViewModel.ShowOnMapRequested"/> модели представления поиска.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SearchViewModel_ShowOnMapRequested(object sender, ShowOnMapRequestedEventArgs e)
        {
            if (e.SearchEntries.Count == 1)
            {
                var searchEntry = e.SearchEntries[0];

                if (this.mainViewModel.CurrentSchema == null || this.mainViewModel.CurrentSchema.Id != searchEntry.SchemaId || this.mainViewModel.CurrentCity.Id != searchEntry.CityId)
                    if (!this.mainViewModel.LoadSchema(searchEntry.SchemaId, searchEntry.CityId))
                        return;

                IObjectViewModel obj;

                if (searchEntry.Type.ObjectKind == ObjectKind.Figure || searchEntry.Type.ObjectKind == ObjectKind.Line)
                    obj = this.mainViewModel.GetObject(searchEntry.Id, searchEntry.Type);
                else
                    obj = this.mainViewModel.GetObject(searchEntry.ParentId.Value);

                Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

                if (obj != null)
                {
                    // Обновляем карту.
                    this.map.UpdateLayout();

                    // Перемещаем вид карты к объекту.
                    this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView((IMapObjectViewModel)obj) as IInteractiveShape).CenterPoint);

                    var selectable = obj as ISelectableObjectViewModel;

                    if (selectable != null)
                        selectable.IsSelected = true;
                }
                else
                    this.messageService.ShowMessage("Объект не обнаружен", "Поиск объекта", MessageType.Error);
            }
            else
            {

                if (this.mainViewModel.CurrentSchema == null || this.mainViewModel.CurrentSchema.Id != e.SearchEntries[0].SchemaId || this.mainViewModel.CurrentCity.Id != e.SearchEntries[0].CityId)
                    if (!this.mainViewModel.LoadSchema(e.SearchEntries[0].SchemaId, e.SearchEntries[0].CityId))
                        return;

                var objects = new List<IObjectViewModel>();

                IObjectViewModel obj;

                foreach (var searchEntry in e.SearchEntries)
                {
                    if (searchEntry.Type.ObjectKind == ObjectKind.Figure || searchEntry.Type.ObjectKind == ObjectKind.Line)
                        obj = this.mainViewModel.GetObject(searchEntry.Id, searchEntry.Type);
                    else
                        obj = this.mainViewModel.GetObject(searchEntry.ParentId.Value);

                    if (obj != null)
                        objects.Add(obj);
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

                this.mainViewModel.SetSelectedObjects(objects);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> элемента управления с вкладками.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Сразу же применяем шаблон карты, чтобы она смогла загрузить все свои элементы.
            this.map.ApplyTemplate();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.IsVisibleChanged"/> вкладки общих параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void tabItemCommonParameters_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.tabItemCommonParameters.IsVisible)
                this.tabItemParameters.IsSelected = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.IsVisibleChanged"/> вкладки вычисляемых параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void tabItemComputationalParameters_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.tabItemComputationalParameters.IsVisible)
                this.tabItemParameters.IsSelected = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.IsVisibleChanged"/> вкладки локальной карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void tabItemLocalMap_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.tabItemLocalMap.IsVisible)
                this.tabItemMap.IsSelected = true;
            else
                this.tabItemLocalMap.IsSelected = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.IsVisibleChanged"/> вкладки истории изменений значения параметра.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void tabItemParameterHistory_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.tabItemParameterHistory.IsVisible)
                this.tabItemParameters.IsSelected = true;
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> поля быстрого поиска.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.textBoxFastSearch.SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> дерева объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TreeView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Если выделенный в дереве объектов элемент является моделью представления объекта, то перемещаем вид карты к нему.
            var treeView = sender as TreeView;
            var obj = treeView.SelectedItem as IObjectViewModel;
            if (obj != null && obj.GetType() != typeof(NonVisualObjectViewModel))
                this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView((IMapObjectViewModel)obj) as IInteractiveShape).CenterPoint);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonDown"/> дерева объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = this.GetTreeViewItem(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                // Убираем выделение с ранее выбранного объекта.
                var treeView = sender as TreeView;
                var selectObj = treeView.SelectedItem as ISelectableObjectViewModel;
                if (selectObj != null)
                    selectObj.IsSelected = false;

                treeViewItem.IsSelected = true;

                // Если выделенный в дереве объектов элемент является моделью представления объекта, то перемещаем вид карты к нему.
                var obj = treeView.SelectedItem as IObjectViewModel;
                if (obj != null && obj.GetType() != typeof(NonVisualObjectViewModel))
                    this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView((IMapObjectViewModel)obj) as IInteractiveShape).CenterPoint);
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.ContextMenuOpening"/> элемента дерева объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void TreeViewItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var element = sender as FrameworkElement;

            var contextMenu = element.ContextMenu;

            contextMenu.DataContext = null;
            contextMenu.DataContext = element.DataContext;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FindObjectView.OpenObjectRequested"/> представления поиска объекта.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void View_OpenObjectRequested(object sender, OpenObjectRequestedEventArgs e)
        {
            var obj = this.mainViewModel.GetObject(e.ObjectId);

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.tabItemLocalMap.IsSelected = true));

            if (obj != null)
            {
                // Обновляем карту.
                this.map.UpdateLayout();

                // Перемещаем вид карты к объекту.
                this.map.ScrollToAndCenter((this.mapBindingService.GetMapObjectView((IMapObjectViewModel)obj) as IInteractiveShape).CenterPoint);

                var selectable = obj as ISelectableObjectViewModel;

                if (selectable != null)
                    selectable.IsSelected = true;

                e.IsFound = true;
            }
            else
                e.IsFound = false;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closing"/> главного представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Closing(object sender, CancelEventArgs e)
        {
            if (this.mainViewModel.SaveCommand.CanExecute(null))
            {
                var result = this.messageService.ShowYesNoCancelMessage("Сохранить все внесенные изменения?", "Выход");

                if (!result.HasValue)
                    e.Cancel = true;
                else
                    if (result.Value)
                    {
                        this.mainViewModel.SaveCommand.Execute(null);

                        // Еще раз выполняем проверку необходимости сохранения, так как при сохранении могли возникнуть ошибки.
                        if (this.mainViewModel.SaveCommand.CanExecute(null))
                            // И если возникли ошибки, то необходимо отменить закрытие представления.
                            e.Cancel = true;
                    }
            }

            if (!e.Cancel)
                if (this.mainViewModel.CitySelectionViewModel.LoadedCity != null)
                    // Запоминаем масштаб и отступы загруженной карты.
                    (this.settingService.Settings["MapPositions"] as Dictionary<int, Tuple<double, Size>>)[this.mainViewModel.CitySelectionViewModel.LoadedCity.Id] = new Tuple<double, Size>(this.map.Scale, new Size(this.map.MapHorizontalOffset, this.map.MapVerticalOffset));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Deactivated"/> главного представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Deactivated(object sender, EventArgs e)
        {
            this.mainViewModel.BoilerInfo.UnsetBoiler();
            this.mainViewModel.StorageInfo.UnsetStorage();
            this.mainViewModel.BuildingInfo.UnsetBuilding();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> главного представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new GlobalMapViewModel(dataService);

            this.globalMapView.ViewModel = viewModel;

            var view = new WaitView(new WaitViewModel("Загрузка глобальной карты", "Пожалуйста подождите, идет загрузка глобальной карты...", async () => await Task.Factory.StartNew(() => viewModel.Load())), this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();


        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    this.mapBindingService.Dispose();

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Возвращает элемент дерева.
        /// </summary>
        /// <param name="source">Элемент, представляющий элемент дерева.</param>
        /// <returns>Элемент дерева.</returns>
        private TreeViewItem GetTreeViewItem(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        /// <summary>
        /// Блокирует UI представления, чтобы нельзя было ничего нажать.
        /// </summary>
        private void LockUI()
        {
            this.frozenGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Разблокировывает UI представления.
        /// </summary>
        private void UnlockUI()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.frozenGrid.Visibility = Visibility.Hidden), DispatcherPriority.Background, null);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет сохранение несохраненных изменений, если они имеются.
        /// </summary>
        public void LastBreath()
        {
            if (this.mainViewModel.SaveCommand.CanExecute(null))
            {
                var result = this.messageService.ShowYesNoMessage("Сохранить все внесенные изменения?", "Аварийное завершение работы");

                if (result)
                    this.mainViewModel.SaveCommand.Execute(null);
            }
        }

        #endregion
    }

    // Реализация IDisposable.
    internal sealed partial class MainView
    {
        #region Открытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}