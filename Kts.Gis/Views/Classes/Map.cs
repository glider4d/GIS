using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет карту, состояющую из нескольких областей и слоев.
    /// </summary>
    internal sealed partial class Map : Control
    {
        #region Закрытые константы

        /// <summary>
        /// Название дочернего элемента-границы.
        /// </summary>
        private const string borderName = "border";

        /// <summary>
        /// Название дочернего элемента-сетки-контейнера.
        /// </summary>
        private const string containerGridName = "containerGrid";

        /// <summary>
        /// Название дочернего элемента-сетки.
        /// </summary>
        private const string gridName = "grid";

        /// <summary>
        /// Название дочернего элемента-сетки, используемого для хранения изображений подложки.
        /// </summary>
        private const string imageGridName = "imageGrid";

        /// <summary>
        /// Отступ легенды.
        /// </summary>
        private const int legendIndent = 400;

        /// <summary>
        /// Название дочернего элемента-скроллвьювера.
        /// </summary>
        private const string scrollViewerName = "scrollViewer";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Дополнительные слои.
        /// </summary>
        //[NonSerialized]
        private Dictionary<Guid, DrawingVisual> additionalLayers = new Dictionary<Guid, DrawingVisual>();

        /// <summary>
        /// Панель для дополнительных слоев, находящаяся сзади объектов.
        /// </summary>
        //[NonSerialized]
        private DrawingPanel additionalLayersPanelBack;

        /// <summary>
        /// Панель для дополнительных слоев, находящаяся спереди объектов.
        /// </summary>
        //[NonSerialized]
        private DrawingPanel additionalLayersPanelFront;

        /// <summary>
        /// Панель для анимирования объектов.
        /// </summary>
        //[NonSerialized]
        private DrawingPanel animationPanel;

        /// <summary>
        /// Размер блоков подложки.
        /// </summary>
        private List<Size> blockSizes = new List<Size>();

        /// <summary>
        /// Граница.
        /// </summary>
        //[NonSerialized]
        private Border border;

        /// <summary>
        /// Холсты. Ключом является уровень по Z от 0 и выше, а значением - словарь холстов, где ключом выступает некоторый объект (представляющий идентификатор слоя), а значением - список холстов.
        /// </summary>
        //[NonSerialized]
        private Dictionary<int, Dictionary<object, List<IndentableCanvas>>> canvases = new Dictionary<int, Dictionary<object, List<IndentableCanvas>>>();

        /// <summary>
        /// Значение, указывающее на то, что можно ли обновлять подложку карты.
        /// </summary>
        private bool canUpdateSubstrate = true;

        /// <summary>
        /// Сетка-контейнер.
        /// </summary>
        private Grid containerGrid;

        /// <summary>
        /// Холст, предназначенный для хранения вспомогательных объектов.
        /// </summary>
        private IndentableCanvas defaultCanvas;

        /// <summary>
        /// Размерность карты (количество областей, на которое она делится).
        /// </summary>
        private int dimension = 2;

        /// <summary>
        /// Сетка.
        /// </summary>
        //[NonSerialized]
        private Grid grid;

        /// <summary>
        /// Область редактирования группы объектов.
        /// </summary>
        //[NonSerialized]
        private GroupArea groupArea;

        /// <summary>
        /// Сетка, используемая для хранения изображений подложки.
        /// </summary>
        //[NonSerialized]
        private Grid imageGrid;

        /// <summary>
        /// Значение, указывающее на то, что задана ли размерность.
        /// </summary>
        private bool isDimensionSetted;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли рисование на карте.
        /// </summary>
        private bool isDrawing;

        /// <summary>
        /// Значение, указывающее на то, что было ли выполнено перемещение вида карты.
        /// </summary>
        private bool isMoved;

        /// <summary>
        /// Значение, указывающее на то, что перемещается ли вид карты.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Значение, указывающее на то, что происходит ли вставка объекта.
        /// </summary>
        private bool isPasting;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли задание области печати.
        /// </summary>
        private bool isPrintAreaSetting;

        /// <summary>
        /// Значение, указывающее на то, что выбираются ли объекты на карте.
        /// </summary>
        private bool isSelecting;

        /// <summary>
        /// Миникарта.
        /// </summary>
        //[NonSerialized]
        private Minimap minimap;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        //[NonSerialized]
        private Point mousePrevPosition;

        /// <summary>
        /// Родительская сетка.
        /// </summary>
        private Grid parentGrid;

        /// <summary>
        /// Вставляемый объект.
        /// </summary>
        private IMapObject pastingObject;

        /// <summary>
        /// Предыдущий верхний холст.
        /// </summary>
        //[NonSerialized]
        private IndentableCanvas prevTopCanvas;

        /// <summary>
        /// Установщик области печати.
        /// </summary>
        //[NonSerialized]
        private PrintAreaSetter printAreaSetter;

        /// <summary>
        /// Скроллвьювер.
        /// </summary>
        //[NonSerialized]
        private ScrollViewer scrollViewer;

        /// <summary>
        /// Выбранные фигуры.
        /// </summary>
        private List<IInteractiveShape> selectedShapes;

        /// <summary>
        /// Прямоугольная выделялка.
        /// </summary>
        private SelectionRectangle selectionRectangle;

        #endregion

        #region Открытые статические поля

        /// <summary>
        /// Автоматическая легенда карты.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty AutoLegendProperty = DependencyProperty.Register("AutoLegend", typeof(AutoLegend), typeof(Map), new PropertyMetadata(null, new PropertyChangedCallback(AutoLegendPropertyChanged)));

        /// <summary>
        /// Значение, указывающее на то, что включен ли вызов контекстного меню.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty EnableContextMenusProperty = DependencyProperty.Register("EnableContextMenus", typeof(bool), typeof(Map), new PropertyMetadata(true));

        /// <summary>
        /// Положение точки области редактирования группы объектов.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty GroupAreaOriginPointProperty = DependencyProperty.Register("GroupAreaOriginPoint", typeof(Point), typeof(Map), new PropertyMetadata(new Point()));

        /// <summary>
        /// Положение области редактирования группы объектов.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty GroupAreaPositionProperty = DependencyProperty.Register("GroupAreaPosition", typeof(Point), typeof(Map), new PropertyMetadata(new Point()));

        /// <summary>
        /// Размер области редактирования группы объектов.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty GroupAreaSizeProperty = DependencyProperty.Register("GroupAreaSize", typeof(Size), typeof(Map), new PropertyMetadata(Size.Empty));

        /// <summary>
        /// Значение, указывающее на то, что загружены ли данные карты.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty IsDataLoadedProperty = DependencyProperty.Register("IsDataLoaded", typeof(bool), typeof(Map), new PropertyMetadata(false, new PropertyChangedCallback(IsDataLoadedPropertyChanged)));

        /// <summary>
        /// Видимость легенды карты.
        /// </summary>
        //[NonSerialized]
        public static DependencyProperty IsLegendVisibleProperty = DependencyProperty.Register("IsLegendVisible", typeof(bool), typeof(Map), new PropertyMetadata(false));

        /// <summary>
        /// Видимость области печати.
        /// </summary>
        public static DependencyProperty IsPrintAreaVisibleProperty = DependencyProperty.Register("IsPrintAreaVisible", typeof(bool), typeof(Map), new PropertyMetadata(false, new PropertyChangedCallback(IsPrintAreaVisiblePropertyChanged)));

        /// <summary>
        /// Видимость подложки карты.
        /// </summary>
        public static DependencyProperty IsSubstrateVisibleProperty = DependencyProperty.Register("IsSubstrateVisible", typeof(bool), typeof(Map), new PropertyMetadata(false, new PropertyChangedCallback(IsSubstrateVisiblePropertyChanged)));

        /// <summary>
        /// Идентификаторы слоев.
        /// </summary>
        public static DependencyProperty LayerIdsProperty = DependencyProperty.Register("LayerIds", typeof(List<ObjectType>), typeof(Map), new PropertyMetadata(new List<ObjectType>(), new PropertyChangedCallback(LayerIdsPropertyChanged)));

        /// <summary>
        /// Действие над картой.
        /// </summary>
        public static DependencyProperty MapActionProperty = DependencyProperty.Register("MapAction", typeof(MapAction), typeof(Map), new PropertyMetadata(new PropertyChangedCallback(MapActionPropertyChanged)));

        /// <summary>
        /// Выбранный принтер.
        /// </summary>
        public static DependencyProperty SelectedPrinterProperty = DependencyProperty.Register("SelectedPrinter", typeof(PrintQueue), typeof(Map), new PropertyMetadata(null));

        /// <summary>
        /// Масштаб карты.
        /// </summary>
        public static DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(Map), new PropertyMetadata(1.0, new PropertyChangedCallback(ScalePropertyChanged)));

        /// <summary>
        /// Размерность подложки карты.
        /// </summary>
        public static DependencyProperty SubstrateDimensionProperty = DependencyProperty.Register("SubstrateDimension", typeof(Size), typeof(Map), new PropertyMetadata(Size.Empty));

        /// <summary>
        /// Прозрачность подложки карты.
        /// </summary>
        public static DependencyProperty SubstrateOpacityProperty = DependencyProperty.Register("SubstrateOpacity", typeof(double), typeof(Map), new PropertyMetadata(1.0, new PropertyChangedCallback(SubstrateOpacityPropertyChanged)));

        /// <summary>
        /// Размер подложки карты.
        /// </summary>
        public static DependencyProperty SubstrateSizeProperty = DependencyProperty.Register("SubstrateSize", typeof(Size), typeof(Map), new PropertyMetadata(Size.Empty, new PropertyChangedCallback(SubstrateSizePropertyChanged)));

        /// <summary>
        /// Названия файлов-изображений подложки карты. Каждый столбец представляет уменьшенную копию изображения предыдущего.
        /// </summary>
        public static DependencyProperty SubstrateTilesProperty = DependencyProperty.Register("SubstrateTiles", typeof(string[][]), typeof(Map), new PropertyMetadata(null));

        /// <summary>
        /// Путь к миниатюре подложки карты.
        /// </summary>
        public static DependencyProperty ThumbnailPathProperty = DependencyProperty.Register("ThumbnailPath", typeof(string), typeof(Map), new PropertyMetadata(null, new PropertyChangedCallback(ThumbnailPathPropertyChanged)));

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие щелчка при зажатой клавише ктрла.
        /// </summary>
        public event EventHandler<ShiftClickedEventArgs> CtrlClicked;

        /// <summary>
        /// Событие завершения рисования.
        /// </summary>
        public event EventHandler<DrawingCompletedEventArgs> DrawingCompleted;

        /// <summary>
        /// Событие рисования.
        /// </summary>
        public event EventHandler<DrawingEventArgs> DrawingDelta;

        /// <summary>
        /// Событие начала рисования.
        /// </summary>
        public event EventHandler<DrawingEventArgs> DrawingStarted;

        /// <summary>
        /// Событие изменения угла поворота области редактирования группы объектов.
        /// </summary>
        public event EventHandler<AngleChangedEventArgs> GroupAreaAngleChanged;

        /// <summary>
        /// Событие отмены изменений области редактирования группы объектов.
        /// </summary>
        public event EventHandler GroupAreaChangeCanceled;

        /// <summary>
        /// Событие изменения положения области редактирования группы объектов.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs> GroupAreaPositionChanged;

        /// <summary>
        /// Событие изменения масштаба области редактирования группы объектов.
        /// </summary>
        public event EventHandler<ScaleChangedEventArgs> GroupAreaScaleChanged;

        /// <summary>
        /// Событие отмены вставки объекта.
        /// </summary>
        public event EventHandler PasteCanceled;

        /// <summary>
        /// Событие завершения вставки объекта.
        /// </summary>
        public event EventHandler PasteCompleted;

        /// <summary>
        /// Событие изменения масштаба.
        /// </summary>
        public event EventHandler ScaleChanged;

        /// <summary>
        /// Событие изменения положения карты.
        /// </summary>
        public event EventHandler ScrollChanged;

        /// <summary>
        /// Событие окончания изменения положения карты.
        /// </summary>
        public event EventHandler ScrollEnded;

        /// <summary>
        /// Событие начала изменения положения карты.
        /// </summary>
        public event EventHandler ScrollStarted;

        /// <summary>
        /// Событие изменения выбора фигур.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Событие щелчка при зажатой клавише шифта.
        /// </summary>
        public event EventHandler<ShiftClickedEventArgs> ShiftClicked;

        /// <summary>
        /// Событие вставки текста.
        /// </summary>
        public event EventHandler<LabelPastedEventArgs> TextPasted;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Map"/>.
        /// </summary>
        public Map()
        {
            this.ScaleFactor = 0.25;
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли рисовать на карте.
        /// </summary>
        private bool CanDraw
        {
            get
            {
                return this.IsDataLoaded;
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает автоматическую легенду карты.
        /// </summary>
        public AutoLegend AutoLegend
        {
            get
            {
                return (AutoLegend)this.GetValue(AutoLegendProperty);
            }
            set
            {
                this.SetValue(AutoLegendProperty, value);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли редактировать объекты карты.
        /// </summary>
        public bool CanEdit
        {
            get
            {
                return this.MapAction == MapAction.Edit;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выбирать объекты карты.
        /// </summary>
        public bool CanSelect
        {
            get
            {
                return this.MapAction == MapAction.Select;
            }
        }

        /// <summary>
        /// Возвращает или задает размерность карты (количество областей, на которое она делится).
        /// </summary>
        public int Dimension
        {
            get
            {
                return this.dimension;
            }
            set
            {
                if (!this.isDimensionSetted)
                {
                    this.isDimensionSetted = true;

                    this.dimension = value;

                    this.SetRowsAndColumns();
                    this.SetCanvases();
                }
                else
                    throw new NotImplementedException("Не реализовано изменение размерности карты");
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что включен ли вызов контекстного меню.
        /// </summary>
        public bool EnableContextMenus
        {
            get
            {
                return (bool)this.GetValue(EnableContextMenusProperty);
            }
            set
            {
                this.SetValue(EnableContextMenusProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает положение точки области редактирования группы объектов.
        /// </summary>
        public Point GroupAreaOriginPoint
        {
            get
            {
                return (Point)this.GetValue(GroupAreaOriginPointProperty);
            }
            set
            {
                this.SetValue(GroupAreaOriginPointProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает положение области редактирования группы объектов.
        /// </summary>
        public Point GroupAreaPosition
        {
            get
            {
                return (Point)this.GetValue(GroupAreaPositionProperty);
            }
            set
            {
                this.SetValue(GroupAreaPositionProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает размер области редактирования группы объектов.
        /// </summary>
        public Size GroupAreaSize
        {
            get
            {
                return (Size)this.GetValue(GroupAreaSizeProperty);
            }
            set
            {
                this.SetValue(GroupAreaSizeProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что загружены ли данные карты.
        /// </summary>
        public bool IsDataLoaded
        {
            get
            {
                return (bool)this.GetValue(IsDataLoadedProperty);
            }
            set
            {
                this.SetValue(IsDataLoadedProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что заданы ли идентификаторы слоев.
        /// </summary>
        public bool IsLayerIdsSetted
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает видимость легенды карты.
        /// </summary>
        public bool IsLegendVisible
        {
            get
            {
                return (bool)this.GetValue(IsLegendVisibleProperty);
            }
            set
            {
                this.SetValue(IsLegendVisibleProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает видимость области печати.
        /// </summary>
        public bool IsPrintAreaVisible
        {
            get
            {
                return (bool)this.GetValue(IsPrintAreaVisibleProperty);
            }
            set
            {
                this.SetValue(IsPrintAreaVisibleProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает видимость подложки карты.
        /// </summary>
        public bool IsSubstrateVisible
        {
            get
            {
                return (bool)this.GetValue(IsSubstrateVisibleProperty);
            }
            set
            {
                this.SetValue(IsSubstrateVisibleProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает идентификаторы слоев.
        /// </summary>
        public List<ObjectType> LayerIds
        {
            get
            {
                return (List<ObjectType>)this.GetValue(LayerIdsProperty);
            }
            set
            {
                this.SetValue(LayerIdsProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает действие над картой.
        /// </summary>
        public MapAction MapAction
        {
            get
            {
                return (MapAction)this.GetValue(MapActionProperty);
            }
            set
            {
                this.SetValue(MapActionProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает горизонтальный отступ карты.
        /// </summary>
        public double MapHorizontalOffset
        {
            get
            {
                return this.scrollViewer.HorizontalOffset;
            }
            private set
            {
                this.scrollViewer.ScrollToHorizontalOffset(value);
            }
        }

        /// <summary>
        /// Возвращает размер карты в пикселях.
        /// </summary>
        public Size MapSize
        {
            get
            {
                return new Size(this.grid.Width, this.grid.Height);
            }
        }

        /// <summary>
        /// Возвращает или задает вертикальный отступ карты.
        /// </summary>
        public double MapVerticalOffset
        {
            get
            {
                return this.scrollViewer.VerticalOffset;
            }
            private set
            {
                this.scrollViewer.ScrollToVerticalOffset(value);
            }
        }

        /// <summary>
        /// Возвращает размер вмещаемого вида карты.
        /// </summary>
        public Size MapViewSize
        {
            get
            {
                return new Size(this.scrollViewer.ViewportWidth, this.scrollViewer.ViewportHeight);
            }
        }

        /// <summary>
        /// Возвращает положение мыши на карте.
        /// </summary>
        public Point MousePosition
        {
            get
            {
                return Mouse.GetPosition(this.grid);
            }
        }

        /// <summary>
        /// Возвращает область печати.
        /// </summary>
        public PrintArea PrintArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает фактор масштабирования карты.
        /// </summary>
        public double ScaleFactor
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает масштаб карты.
        /// </summary>
        public double Scale
        {
            get
            {
                return (double)this.GetValue(ScaleProperty);
            }
            set
            {
                this.SetValue(ScaleProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный принтер.
        /// </summary>
        public PrintQueue SelectedPrinter
        {
            get
            {
                return (PrintQueue)this.GetValue(SelectedPrinterProperty);
            }
            set
            {
                this.SetValue(SelectedPrinterProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает выбранные фигуры.
        /// </summary>
        public List<IInteractiveShape> SelectedShapes
        {
            get
            {
                return this.selectedShapes;
            }
            private set
            {
                this.selectedShapes = value;

                this.SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Возвращает или задает размерность подложки карты.
        /// </summary>
        public Size SubstrateDimension
        {
            get
            {
                return (Size)this.GetValue(SubstrateDimensionProperty);
            }
            set
            {
                this.SetValue(SubstrateDimensionProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает прозрачность подложки карты.
        /// </summary>
        public double SubstrateOpacity
        {
            get
            {
                return (double)this.GetValue(SubstrateOpacityProperty);
            }
            set
            {
                this.SetValue(SubstrateOpacityProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает размер подложки карты.
        /// </summary>
        public Size SubstrateSize
        {
            get
            {
                return (Size)this.GetValue(SubstrateSizeProperty);
            }
            set
            {
                this.SetValue(SubstrateSizeProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает названия файлов-изображений подложки карты.
        /// </summary>
        public string[][] SubstrateTiles
        {
            get
            {
                return (string[][])this.GetValue(SubstrateTilesProperty);
            }
            set
            {
                this.SetValue(SubstrateTilesProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает путь к миниатюре подложки карты.
        /// </summary>
        public string ThumbnailPath
        {
            get
            {
                return (string)this.GetValue(ThumbnailPathProperty);
            }
            set
            {
                this.SetValue(ThumbnailPathProperty, value);
            }
        }

        /// <summary>
        /// Возвращает размер видимой части карты.
        /// </summary>
        public Size VisibleSize
        {
            get
            {
                return new Size(this.scrollViewer.ViewportWidth / this.Scale, this.scrollViewer.ViewportHeight / this.Scale);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.isSelecting)
            {
                this.OnSelectingCompleted(e.GetPosition(this.grid));

                return;
            }

            if (this.isMoving)
            {
                this.OnMovingCompleted(e.GetPosition(this.grid));

                return;
            }

            if (this.isPrintAreaSetting)
            {
                this.OnPrintAreaSettingCompleted(e.GetPosition(this.grid));

                return;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeftButtonDown"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.MapAction == MapAction.Select)
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    this.OnSelectWithShift(e.GetPosition(this.grid));
                else
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                        this.OnSelectWithCtrl(e.GetPosition(this.grid));
                    else
                        this.OnSelectingStarted(e.GetPosition(this.grid));
            else
                if (this.MapAction == MapAction.SetPrintArea)
                    this.OnPrintAreaSettingStarted(e.GetPosition(this.grid));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.isPasting)
            {
                this.isPasting = false;

                (this.pastingObject as IInteractiveShape).EndMoveTo();

                this.PasteCompleted?.Invoke(this, EventArgs.Empty);

                return;
            }

            if (this.MapAction == MapAction.Draw)
                this.OnDrawingStarted(e.GetPosition(this.grid));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.isSelecting)
            {
                this.OnSelectingCompleted(e.GetPosition(this.grid));

                return;
            }

            if (this.isMoving)
            {
                this.OnMovingCompleted(e.GetPosition(this.grid));

                return;
            }

            if (this.isDrawing)
            {
                this.OnDrawingCompleted(e.GetPosition(this.grid));

                return;
            }

            if (this.isPrintAreaSetting)
            {
                this.OnPrintAreaSettingCompleted(e.GetPosition(this.grid));

                return;
            }

            if (this.MapAction == MapAction.Text)
                this.TextPasted?.Invoke(this, new LabelPastedEventArgs(e.GetPosition(this.grid)));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this.grid);

            if (this.isMoving)
            {
                // Тут мы отдельно обрабатываем перемещение вида холста, так как оно могло быть начато нажатием правой кнопки мыши.
                this.OnMovingDelta(mousePosition);

                return;
            }

            if (this.isPasting)
            {
                (this.pastingObject as IInteractiveShape).MoveTo(this.MousePosition);

                return;
            }

            switch (this.MapAction)
            {
                case MapAction.Draw:
                    this.OnDrawingDelta(mousePosition);

                    break;

                case MapAction.SetPrintArea:
                    this.OnPrintAreaSettingDelta(mousePosition);

                    break;

                case MapAction.Select:
                    this.OnSelectingDelta(mousePosition);

                    break;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonDown"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnMovingStarted(e.GetPosition(this.grid));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> сетки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Grid_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnMovingCompleted(e.GetPosition(this.grid));

            e.Handled = this.isMoved;

            this.isMoved = false;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="GroupArea.AngleChanged"/> области редактирования группы объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_AngleChanged(object sender, AngleChangedEventArgs e)
        {
            this.GroupAreaAngleChanged.Invoke(this, e);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="GroupArea.PositionChanged"/> области редактирования группы объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            this.GroupAreaPositionChanged.Invoke(this, e);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="GroupArea.ScaleChanged"/> области редактирования группы объектов.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_ScaleChanged(object sender, ScaleChangedEventArgs e)
        {
            this.GroupAreaScaleChanged.Invoke(this, e);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewKeyDown"/> скроллвьювера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ScrollViewer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (this.PrintArea != null)
            {
                if (e.Key == Key.OemPlus || e.Key == Key.Add)
                    this.PrintArea.IncreasePageCount();

                if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                    this.PrintArea.DecreasePageCount();

                if (e.Key == Key.D9)
                    this.PrintArea.RotateLeft();

                if (e.Key == Key.D0)
                    this.PrintArea.RotateRight();
            }

            if (e.SystemKey == Key.LeftAlt)
                if (this.groupArea.IsVisible)
                {
                    this.groupArea.HandlePointChange();

                    e.Handled = true;
                }

            if (e.Key == Key.Escape)
                if (this.isDrawing)
                    this.OnDrawingCompleted(new Point(), true);
                else
                    if (this.isPasting)
                    {
                        this.isPasting = false;

                        this.PasteCanceled?.Invoke(this, EventArgs.Empty);
                    }
                    else
                        if (this.groupArea.IsVisible)
                        {
                            this.groupArea.CancelChanges();

                            this.GroupAreaChangeCanceled?.Invoke(this, EventArgs.Empty);
                        }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                if (e.Key == Key.Add || e.Key == Key.OemPlus)
                    this.Zoom(true);
                else
                    if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                        this.Zoom(false);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> скроллвьювера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.scrollViewer.IsFocused)
                // Если не установлен фокус, то ставим его.
                this.scrollViewer.Focus();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseWheel"/> скроллвьювера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Отменяем скроллинг, чтобы выполнять только зуминг.
            e.Handled = true;

            this.Zoom(e.Delta > 0);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ScrollViewer.ScrollChanged"/> скроллвьювера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!this.IsDataLoaded)
            {
                this.border.BorderThickness = new Thickness(0);

                return;
            }

            // В зависимости от возможности скроллинга показываем или скрываем границы.
            int horizontalBorder = 1;
            int verticalBorder = 1;
            if (this.scrollViewer.ScrollableWidth > 0)
                verticalBorder = 0;
            if (this.scrollViewer.ScrollableHeight > 0)
                horizontalBorder = 0;
            this.border.BorderThickness = new Thickness(verticalBorder, horizontalBorder, verticalBorder, horizontalBorder);

            // Обновляем положение внутри миникарты.
            this.minimap.UpdateLocation();

            // Обновляем подложку карты.
            this.UpdateSubstrate();

            this.ScrollChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="SmartImage.SourceChanged"/> части изображения подложки карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartImage_SourceChanged(object sender, RoutedEventArgs e)
        {
            var smartImage = sender as SmartImage;

            smartImage.SourceChanged -= this.SmartImage_SourceChanged;

            if (smartImage.Source == null)
                return;

            var animation = new DoubleAnimation
            {
                From = 0,
                Duration = TimeSpan.FromMilliseconds(500),
                To = 1
            };

            var story = new Storyboard();

            Storyboard.SetTarget(animation, smartImage);
            Storyboard.SetTargetProperty(animation, new PropertyPath(OpacityProperty));

            story.Children.Add(animation);

            smartImage.BeginStoryboard(story);
        }

        #endregion

        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="AutoLegendProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void AutoLegendPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as Map).SetAutoLegend((AutoLegend)e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="IsDataLoadedProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void IsDataLoadedPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as Map).SetIsDataLoaded((bool)e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="IsPrintAreaVisibleProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void IsPrintAreaVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as Map).SetIsPrintAreaVisible((bool)e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="IsSubstrateVisibleProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void IsSubstrateVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as Map).SetIsSubstrateVisible((bool)e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="LayerIdsProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void LayerIdsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source as Map).IsLayerIdsSetted)
                (source as Map).SetCanvases();
            else
                throw new NotImplementedException("Не реализовано изменение идентификаторов слоев");
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="MapActionProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void MapActionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as Map).SetMapAction((MapAction)e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="ScaleProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void ScalePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var map = source as Map;

            map.ScaleChanged?.Invoke(map, EventArgs.Empty);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="SubstrateOpacityProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void SubstrateOpacityPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as Map).SetSubstrateOpacity((double)e.NewValue);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="SubstrateSizeProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void SubstrateSizePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (((Size)e.NewValue).IsEmpty)
                return;

            (source as Map).SetSubstrate();
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="ThumbnailPathProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void ThumbnailPathPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValue)))
                return;

            (source as Map).SetThumbnail();
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли быть произведен зум аут.
        /// </summary>
        /// <param name="scaleFactor">Фактор масштабирования.</param>
        /// <returns>true, если может, иначе - false.</returns>
        private bool CanZoomOut(double scaleFactor)
        {
            var result = this.SubstrateSize.Width * (this.Scale - scaleFactor) >= this.scrollViewer.ViewportWidth || this.SubstrateSize.Height * (this.Scale - scaleFactor) >= this.scrollViewer.ViewportHeight;

            return result ? result : (this.SubstrateSize.Width * this.Scale >= this.scrollViewer.ViewportWidth || this.SubstrateSize.Height * this.Scale >= this.scrollViewer.ViewportHeight);
        }

        /// <summary>
        /// Возвращает холст, который находится в пределах указанной точки.
        /// </summary>
        /// <param name="canvases">Список холстов.</param>
        /// <param name="point">Точка.</param>
        /// <returns>Холст.</returns>
        private IndentableCanvas GetCanvas(List<IndentableCanvas> canvases, Point point)
        {
            var column = (int)(point.X / (this.grid.Width / this.Dimension));
            var row = (int)(point.Y / (this.grid.Height / this.Dimension));

            foreach (var canvas in canvases)
                if (Grid.GetColumn(canvas) == column && Grid.GetRow(canvas) == row)
                    return canvas;


            //return canvases[0];
            throw new Exception("Выход за пределы списка холстов");
        }

        /// <summary>
        /// Загружает часть подложки в изображение.
        /// </summary>
        /// <param name="smartImage">Изображение.</param>
        /// <param name="fileName">Название файла.</param>
        private void LoadImage(SmartImage smartImage, string fileName)
        {
            // Загружаем изображение.
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.UriSource = new Uri(fileName);
            bitmapImage.EndInit();

            if (bitmapImage.CanFreeze)
                bitmapImage.Freeze();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                smartImage.Opacity = 0;
                smartImage.SourceChanged += this.SmartImage_SourceChanged;
                smartImage.Source = bitmapImage;
            }));
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением рисования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <param name="isForced">Значение, указывающее на то, что рисование завершается принудительно.</param>
        private void OnDrawingCompleted(Point mousePosition, bool isForced = false)
        {
            if (!this.CanDraw)
                return;

            if (!this.isDrawing)
                return;

            var eventArgs = new DrawingCompletedEventArgs(mousePosition, isForced);

            this.DrawingCompleted?.Invoke(this, eventArgs);

            if (isForced || !eventArgs.IsCanceled)
                this.isDrawing = false;
        }

        /// <summary>
        /// Выполняет действия, связанные с рисованием.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnDrawingDelta(Point mousePosition)
        {
            if (!this.CanDraw)
                return;

            if (!this.isDrawing)
                return;

            this.DrawingDelta?.Invoke(this, new DrawingEventArgs(mousePosition, this.mousePrevPosition));
        }

        /// <summary>
        /// Выполняет действия, связанные с началом рисования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnDrawingStarted(Point mousePosition)
        {
            if (!this.CanDraw)
                return;

            if (this.isDrawing)
                return;

            this.mousePrevPosition = mousePosition;

            this.isDrawing = true;

            this.DrawingStarted?.Invoke(this, new DrawingEventArgs(mousePosition));
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnMovingCompleted(Point mousePosition)
        {
            this.isMoving = false;

            this.SetMapCursor(this.MapAction);

            this.ScrollEnded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет действия, связанные с перемещением.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnMovingDelta(Point mousePosition)
        {
            if (!this.isMoving)
                return;

            // Приходится уменьшать дельты, так как если не делать этого, то при перемещении вида карты при большом зум ауте, все подергивается. Вообще хер его знает, что это за магия такая.
            double deltaX = (this.mousePrevPosition.X - mousePosition.X) / (3 / this.Scale);
            double deltaY = (this.mousePrevPosition.Y - mousePosition.Y) / (3 / this.Scale);

            if (Math.Abs(this.mousePrevPosition.X - mousePosition.X) > 1 || Math.Abs(this.mousePrevPosition.Y - mousePosition.Y) > 1)
                // Это необходимо для того, чтобы отсеивать ложные клики правой кнопкой мыши по объектам.
                this.isMoved = true;

            // При изменении скролла мы должны временно заблокировать возможность обновления подложки карты, чтобы не выполнять одни и те же действия два раза.
            this.canUpdateSubstrate = false;
            this.MapHorizontalOffset = this.scrollViewer.HorizontalOffset + deltaX;
            this.canUpdateSubstrate = true;
            this.MapVerticalOffset = this.scrollViewer.VerticalOffset + deltaY;
        }

        /// <summary>
        /// Выполняет действия, связанные с началом перемещения.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnMovingStarted(Point mousePosition)
        {
            this.mousePrevPosition = mousePosition;

            this.isMoving = true;

            this.SetMapCursor(MapAction.Move);

            this.ScrollStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением задания области печати.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnPrintAreaSettingCompleted(Point mousePosition)
        {
            if (!this.isPrintAreaSetting)
                return;

            this.isPrintAreaSetting = false;

            if (this.PrintArea != null)
                this.printAreaSetter.SetArea(this.PrintArea);
            
            // Убираем установщик области печати.
            this.printAreaSetter.RemoveFromCanvas();
            this.printAreaSetter = null;
        }

        /// <summary>
        /// Выполняет действия, связанные с заданием области печати.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnPrintAreaSettingDelta(Point mousePosition)
        {
            if (!this.isPrintAreaSetting)
                return;

            // Перерисовываем установщик области печати.
            this.printAreaSetter.Draw(this.mousePrevPosition, mousePosition);
        }

        /// <summary>
        /// Выполняет действия, связанные с началом задания области печати.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnPrintAreaSettingStarted(Point mousePosition)
        {
            this.mousePrevPosition = mousePosition;

            this.isPrintAreaSetting = true;

            // Добавляем установщик области печати.
            this.printAreaSetter = new PrintAreaSetter();
            this.printAreaSetter.LeftTopCorner = mousePosition;
            this.printAreaSetter.AddToCanvas(this.defaultCanvas);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением выбора.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnSelectingCompleted(Point mousePosition)
        {
            if (!this.isSelecting)
                return;

            this.isSelecting = false;

            // Получаем список выбранных фигур.
            this.SelectedShapes = this.selectionRectangle.GetHittedShapes();

            // Убираем прямоугольную выделялку.
            this.selectionRectangle.RemoveFromCanvas();
            this.selectionRectangle = null;
        }

        /// <summary>
        /// Выполняет действия, связанные с выбором.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnSelectingDelta(Point mousePosition)
        {
            if (!this.isSelecting)
                return;

            // Перерисовываем прямоугольную выделялку.
            this.selectionRectangle.Draw(this.mousePrevPosition, mousePosition);
        }

        /// <summary>
        /// Выполняет действия, связанные с началом выбора.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnSelectingStarted(Point mousePosition)
        {
            this.mousePrevPosition = mousePosition;

            this.isSelecting = true;

            // Добавляем прямоугольную выделялку.
            this.selectionRectangle = new SelectionRectangle();
            this.selectionRectangle.LeftTopCorner = mousePosition;
            this.selectionRectangle.AddToCanvas(this.defaultCanvas);
        }

        /// <summary>
        /// Выполняет действия, связанные с выбором объектов при зажатой клавишей ктрла.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnSelectWithCtrl(Point mousePosition)
        {
            // Пока временно можно воспользоваться прямоугольной выделялкой, для получения выбранного объекта.
            this.selectionRectangle = new SelectionRectangle()
            {
                LeftTopCorner = mousePosition,
                // Скрываем ее.
                Visibility = Visibility.Collapsed
            };

            this.selectionRectangle.AddToCanvas(this.defaultCanvas);

            var shapes = this.selectionRectangle.GetHittedShapes();
            if (shapes != null && shapes.Count > 0)
                this.CtrlClicked?.Invoke(this, new ShiftClickedEventArgs(shapes[0]));

            // Убираем прямоугольную выделялку.
            this.selectionRectangle.RemoveFromCanvas();
            this.selectionRectangle = null;
        }

        /// <summary>
        /// Выполняет действия, связанные с выбором объектов при зажатой клавишей шифта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void OnSelectWithShift(Point mousePosition)
        {
            // Пока временно можно воспользоваться прямоугольной выделялкой, для получения выбранного объекта.
            this.selectionRectangle = new SelectionRectangle()
            {
                LeftTopCorner = mousePosition,
                // Скрываем ее.
                Visibility = Visibility.Collapsed
            };

            this.selectionRectangle.AddToCanvas(this.defaultCanvas);

            var shapes = this.selectionRectangle.GetHittedShapes();
            if (shapes != null && shapes.Count > 0)
                this.ShiftClicked?.Invoke(this, new ShiftClickedEventArgs(shapes[0]));

            // Убираем прямоугольную выделялку.
            this.selectionRectangle.RemoveFromCanvas();
            this.selectionRectangle = null;
        }

        /// <summary>
        /// Сохраняет <see cref="RenderTargetBitmap"/> в GIF-файл.
        /// </summary>
        /// <param name="rtb">Сохраняемый <see cref="RenderTargetBitmap"/>.</param>
        /// <param name="path">Путь к сохраняемому файлу.</param>
        private void SaveRTBAsGIF(RenderTargetBitmap rtb, string path)
        {
            var enc = new GifBitmapEncoder();

            enc.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = File.Create(path))
                enc.Save(fs);
        }

        /// <summary>
        /// Сохраняет <see cref="RenderTargetBitmap"/> в JPG-файл.
        /// </summary>
        /// <param name="rtb">Сохраняемый <see cref="RenderTargetBitmap"/>.</param>
        /// <param name="path">Путь к сохраняемому файлу.</param>
        private void SaveRTBAsJPG(RenderTargetBitmap rtb, string path)
        {
            var enc = new JpegBitmapEncoder();

            enc.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = File.Create(path))
                enc.Save(fs);
        }

        /// <summary>
        /// Сохраняет <see cref="RenderTargetBitmap"/> в PNG-файл.
        /// </summary>
        /// <param name="rtb">Сохраняемый <see cref="RenderTargetBitmap"/>.</param>
        /// <param name="path">Путь к сохраняемому файлу.</param>
        private void SaveRTBAsPNG(RenderTargetBitmap rtb, string path)
        {
            var enc = new PngBitmapEncoder();

            enc.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = File.Create(path))
                enc.Save(fs);
        }

        /// <summary>
        /// Задает холсты.
        /// </summary>
        private void SetCanvases()
        {
            if (this.LayerIds.Count == 0 || this.grid == null)
                // Выходим из метода, если либо не заданы идентификаторы слоев, либо не задана сетка.
                return;

            this.IsLayerIdsSetted = true;

            // Создаем холсты.
            IndentableCanvas canvas;
            this.canvases.Add(0, new Dictionary<object, List<IndentableCanvas>>());
            this.canvases.Add(2, new Dictionary<object, List<IndentableCanvas>>());
            this.canvases.Add(4, new Dictionary<object, List<IndentableCanvas>>());
            this.canvases.Add(6, new Dictionary<object, List<IndentableCanvas>>());
            this.canvases.Add(8, new Dictionary<object, List<IndentableCanvas>>());
            foreach (var key in this.LayerIds)
            {
                this.canvases[0].Add(key, new List<IndentableCanvas>());
                this.canvases[2].Add(key, new List<IndentableCanvas>());
                this.canvases[4].Add(key, new List<IndentableCanvas>());
                this.canvases[6].Add(key, new List<IndentableCanvas>());
                this.canvases[8].Add(key, new List<IndentableCanvas>());

                for (int i = 0; i < this.Dimension; i++)
                    for (int j = 0; j < this.Dimension; j++)
                        for (int k = 0; k < 5; k++)
                        {
                            canvas = new IndentableCanvas(this, k * 2);
                            
                            Grid.SetColumn(canvas, j);
                            Grid.SetRow(canvas, i);
                            Panel.SetZIndex(canvas, canvas.ZOrder);

                            this.canvases[k * 2][key].Add(canvas);
                        }
            }

            // Добавляем созданные холсты.
            foreach (var entry in this.canvases.Values)
                foreach (var canvases in entry.Values)
                    foreach (var item in canvases)
                        this.grid.Children.Add(item);

            // Добавляем холст, предназначенный для хранения вспомогательных объектов.
            this.defaultCanvas = new IndentableCanvas(this, 10);
            Grid.SetColumnSpan(this.defaultCanvas, this.Dimension);
            Grid.SetRowSpan(this.defaultCanvas, this.Dimension);
            Panel.SetZIndex(this.defaultCanvas, this.defaultCanvas.ZOrder);
            this.grid.Children.Add(this.defaultCanvas);

            // Добавляем панель для анимирования объектов.
            this.animationPanel = new DrawingPanel()
            {
                IsHitTestVisible = false
            };
            // Помещаем ее на 3 уровень, так как на 4 уровне находятся значки, а нам не нужно их перекрывать.
            Panel.SetZIndex(this.animationPanel, 3);
            this.grid.Children.Add(this.animationPanel);

            // Добавляем панели для дополнительных слоев.
            this.additionalLayersPanelBack = new DrawingPanel()
            {
                IsHitTestVisible = false
            };
            Panel.SetZIndex(this.additionalLayersPanelBack, 1);
            this.grid.Children.Add(this.additionalLayersPanelBack);
            this.additionalLayersPanelFront = new DrawingPanel()
            {
                IsHitTestVisible = false
            };
            Panel.SetZIndex(this.additionalLayersPanelFront, 3);
            this.grid.Children.Add(this.additionalLayersPanelFront);

            // Добавляем область редактирования группы объектов.
            this.groupArea = new GroupArea()
            {
                Visibility = Visibility.Hidden
            };
            var binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("GroupAreaOriginPoint"),
                Mode = BindingMode.TwoWay
            };
            this.groupArea.SetBinding(GroupArea.OriginPointPositionProperty, binding);
            binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("GroupAreaPosition"),
                Mode = BindingMode.TwoWay
            };
            this.groupArea.SetBinding(GroupArea.PositionProperty, binding);
            binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("GroupAreaSize"),
                Mode = BindingMode.TwoWay
            };
            this.groupArea.SetBinding(GroupArea.SizeProperty, binding);
            this.groupArea.AddToCanvas(this.defaultCanvas);
            this.groupArea.PositionChanged += this.GroupArea_PositionChanged;
            this.groupArea.AngleChanged += this.GroupArea_AngleChanged;
            this.groupArea.ScaleChanged += this.GroupArea_ScaleChanged;
            
        }
        
        /// <summary>
        /// Задает отступы холстам.
        /// </summary>
        private void SetIndents()
        {
            double leftIndent = -this.grid.Width / this.Dimension;
            double topIndent = -this.grid.Height / this.Dimension;

            foreach (var entry in this.canvases.Values)
                foreach (var canvases in entry.Values)
                    foreach (var item in canvases)
                        item.Indent = new Thickness(Grid.GetColumn(item) * leftIndent, Grid.GetRow(item) * topIndent, 0, 0);
        }

        /// <summary>
        /// Задает курсор карты по типу действия над картой.
        /// </summary>
        /// <param name="mapAction">Действие над картой.</param>
        private void SetMapCursor(MapAction mapAction)
        {
            // Задаем курсор карты в зависимости от типа действия.
            switch (mapAction)
            {
                case MapAction.Draw:
                    this.Cursor = Cursors.Pen;

                    break;

                case MapAction.Edit:
                case MapAction.EditGroup:
                    this.Cursor = Cursors.Hand;

                    break;

                case MapAction.Move:
                    this.Cursor = Cursors.SizeAll;

                    break;

                case MapAction.Select:
                case MapAction.SetPrintArea:
                    this.Cursor = Cursors.Arrow;

                    break;

                case MapAction.Text:
                    this.Cursor = Cursors.IBeam;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание курсора для следующего типа действия над картой: " + mapAction.ToString());
            }
        }

        /// <summary>
        /// Задает строки и столбцы.
        /// </summary>
        private void SetRowsAndColumns()
        {
            if (this.grid == null)
                return;

            // Создаем строки и столбцы.
            this.grid.RowDefinitions.Clear();
            this.grid.ColumnDefinitions.Clear();
            for (int i = 0; i < this.Dimension; i++)
            {
                this.grid.RowDefinitions.Add(new RowDefinition());
                this.grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        /// <summary>
        /// Выгружает часть подложки с изображения.
        /// </summary>
        /// <param name="smartImage">Изображение.</param>
        private void UnloadImage(SmartImage smartImage)
        {
            smartImage.Source = null;

            smartImage.Opacity = 0;

            smartImage.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Обновляет подложку карты.
        /// </summary>
        private void UpdateSubstrate()
        {
            if (!this.canUpdateSubstrate)
                return;

            if (!this.IsSubstrateVisible)
                return;

            // Получаем прямоугольник, описывающий отображаемую часть подложки карты.
            var coeff = 1 / this.Scale;
            var left = this.scrollViewer.HorizontalOffset * coeff;
            var top = this.scrollViewer.VerticalOffset * coeff;
            var width = this.scrollViewer.ViewportWidth * coeff;
            var height = this.scrollViewer.ViewportHeight * coeff;
            var visRect = new Rect(new Point(left, top), new Size(width, height));

            // Сохраняем в данной переменной значение, указывающее на то, что найден ли уровень тайлов, которых следует отобразить.
            var isFound = false;
            // А в этой - индекс слоя блоков, которые нужно отобразить.
            var index = -1;

            for (int i = 0; i < this.blockSizes.Count; i++)
            {
                var blockSize = this.blockSizes[i];
                
                if (isFound || height / blockSize.Height > 3)
                {
                    if (!isFound && i == this.blockSizes.Count - 1)
                    {
                        isFound = true;

                        index = i;

                        continue;
                    }

                    foreach (SmartImage image in (this.imageGrid.Children[i] as Grid).Children)
                        this.UnloadImage(image);

                    (this.imageGrid.Children[i] as Grid).Visibility = Visibility.Hidden;
                }
                else
                {
                    isFound = true;

                    index = i;
                }
            }

            if (index > -1)
            {
                (this.imageGrid.Children[index] as Grid).Visibility = Visibility.Visible;

                // Выполняем проверку видимости всех частей подложки карты.
                Rect rect;
                foreach (var child in (this.imageGrid.Children[index] as Grid).Children)
                {
                    var smartImage = child as SmartImage;

                    left = Grid.GetColumn(smartImage) * this.blockSizes[index].Width;
                    top = Grid.GetRow(smartImage) * this.blockSizes[index].Height;

                    rect = new Rect(new Point(left, top), new Size(smartImage.Width, smartImage.Height));

                    if (visRect.IntersectsWith(rect))
                    {
                        if (smartImage.Visibility == Visibility.Hidden)
                        {
                            smartImage.Visibility = Visibility.Visible;

                            // Получаем название файла.
                            var column = Grid.GetColumn(smartImage);
                            var row = Grid.GetRow(smartImage);
                            var fileName = this.SubstrateTiles[index][column + Convert.ToInt32((this.imageGrid.Children[index] as Grid).ColumnDefinitions.Count) * row];

                            Task.Factory.StartNew(() => this.LoadImage(smartImage, fileName));
                        }
                    }
                    else
                        this.UnloadImage(smartImage);
                }
            }
        }

        /// <summary>
        /// Выполняет зумирование карты.
        /// </summary>
        /// <param name="zoomIn">true, если зум внутрь, иначе - false.</param>
        private void Zoom(bool zoomIn)
        {
            var a = Mouse.GetPosition(this.grid);
            var b = Mouse.GetPosition(this.scrollViewer);

            var scale = Math.Round(this.Scale, 2);

            if (zoomIn)
                if (scale - this.ScaleFactor > this.ScaleFactor)
                    scale += this.ScaleFactor;
                else
                    scale += 0.05;
            else
                // Ограничиваем зум аут. Пусть оно будет не меньше фактора масштабирования.
                if (scale - this.ScaleFactor > this.ScaleFactor && this.CanZoomOut(this.ScaleFactor))
                scale -= this.ScaleFactor;
            else
                    if (this.CanZoomOut(0.025))
                scale -= 0.025;

            scale = Math.Round(scale, 2);
            if (scale <= 0)
            {
                return;
            }

            this.Scale = scale;

            var newPosition = (this.parentGrid.LayoutTransform as ScaleTransform).Transform(a);

            // При изменении зума мы должны временно заблокировать возможность обновления подложки карты, чтобы не выполнять одни и те же действия два раза.
            this.canUpdateSubstrate = false;
            this.MapHorizontalOffset = newPosition.X - b.X;
            this.canUpdateSubstrate = true;
            this.MapVerticalOffset = newPosition.Y - b.Y;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет дополнительный слой с заданными фигурами и кистями.
        /// </summary>
        /// <param name="geometries">Словарь с кистями и геометриями фигур.</param>
        /// <param name="front">true, если слой должен находиться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public Guid AddLayer(Dictionary<Pen, List<Geometry>> geometries, bool front)
        {
            var id = Guid.NewGuid();

            var layer = new DrawingVisual();

            using (var dc = layer.RenderOpen())
                foreach (var entry in geometries)
                    foreach (var geometry in entry.Value)
                        dc.DrawGeometry(null, entry.Key, geometry);

            this.additionalLayers.Add(id, layer);

            if (front)
                this.additionalLayersPanelFront.AddVisual(layer);
            else
                this.additionalLayersPanelBack.AddVisual(layer);

            return id;
        }

        /// <summary>
        /// Добавляет дополнительный слой с заданными фигурами.
        /// </summary>
        /// <param name="geometries">Геометрии.</param>
        /// <param name="pen">Кисть геометрий.</param>
        /// <param name="front">true, если слой должен находиться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public Guid AddLayer(List<Geometry> geometries, Pen pen, bool front)
        {
            var id = Guid.NewGuid();

            var layer = new DrawingVisual();

            using (var dc = layer.RenderOpen())
                for (int i = 0; i < geometries.Count; i++)
                    dc.DrawGeometry(null, pen, geometries[i]);

            this.additionalLayers.Add(id, layer);

            if (front)
                this.additionalLayersPanelFront.AddVisual(layer);
            else
                this.additionalLayersPanelBack.AddVisual(layer);

            return id;
        }

        /// <summary>
        /// Отключает анимацию объектов.
        /// </summary>
        public void AnimateOff()
        {
            this.animationPanel.DeleteVisuals();
        }

        /// <summary>
        /// Включает анимацию заданного объекта.
        /// </summary>
        /// <param name="geometry">Геометрия объекта.</param>
        /// <param name="pen">Кисть обводки объекта.</param>
        public void AnimateOn(Geometry geometry, Pen pen)
        {
            var layer = new DrawingVisual();

            using (var dc = layer.RenderOpen())
                dc.DrawGeometry(null, pen, geometry);

            this.animationPanel.AddVisual(layer);
        }

        /// <summary>
        /// Включает анимацию заданных объектов.
        /// </summary>
        /// <param name="geometries">Геометрии объектов.</param>
        /// <param name="pens">Кисти обводки объектов.</param>
        public void AnimateOn(List<Geometry> geometries, List<Pen> pens)
        {
            var layer = new DrawingVisual();

            using (var dc = layer.RenderOpen())
                for (int i = 0; i < geometries.Count; i++)
                    dc.DrawGeometry(null, pens[i], geometries[i]);

            this.animationPanel.AddVisual(layer);
        }

        /// <summary>
        /// Отводит холст на задний план.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        public void BringToBack(IndentableCanvas canvas)
        {
            if (this.prevTopCanvas != canvas)
                Panel.SetZIndex(canvas, canvas.ZOrder);
        }

        /// <summary>
        /// Выводит холст на передний план.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        public void BringToFront(IndentableCanvas canvas)
        {
            if (this.prevTopCanvas != null)
                Panel.SetZIndex(this.prevTopCanvas, this.prevTopCanvas.ZOrder);

            Panel.SetZIndex(canvas, canvas.ZOrder + 1);

            this.prevTopCanvas = canvas;
        }

        /// <summary>
        /// Убирает очертания объектов с карты.
        /// </summary>
        public void ClearOutlines()
        {
            this?.groupArea?.ClearOutlines();
        }

        /// <summary>
        /// Создает очертания объектов.
        /// </summary>
        /// <param name="geometries">Геометрии объектов.</param>
        /// <param name="pens">Кисти обводки объектов.</param>
        public void CreateOutlines(List<Geometry> geometries, List<Pen> pens)
        {
            this.groupArea.CreateOutlines(geometries, pens);
        }

        /// <summary>
        /// Возвращает холст, который находится в пределах указанной точки по уровню по Z и идентификатору слоя.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <param name="zOrder">Уровень по Z от 0 и выше (значение меняется на 1).</param>
        /// <param name="id">Идентификатор слоя.</param>
        /// <returns>Холст.</returns>
        public IndentableCanvas GetCanvas(Point point, int zOrder, object id)
        {
            return this.GetCanvas(this.canvases[zOrder * 2][id], point);
        }

        /// <summary>
        /// Вовзращает положение центра отображаемой части карты.
        /// </summary>
        /// <returns>Положение центра.</returns>
        public Point GetCurrentCenter()
        {
            var x = this.MapHorizontalOffset + this.scrollViewer.ViewportWidth / 2;
            var y = this.MapVerticalOffset + this.scrollViewer.ViewportHeight / 2;

            return new Point(x / this.Scale, y / this.Scale);
        }

        /// <summary>
        /// Возвращает холст по умолчанию.
        /// </summary>
        /// <returns>Холст.</returns>
        public IndentableCanvas GetDefaultCanvas()
        {
            return this.defaultCanvas;
        }

        /// <summary>
        /// Возвращает идентификаторы слоев.
        /// </summary>
        /// <returns>Список идентификаторов слоев.</returns>
        public List<object> GetLayerIds()
        {
            if (this.canvases.Count == 0)
                return new List<object>();

            return this.canvases[0].Keys.ToList();
        }

        /// <summary>
        /// Скрывает холсты заданного уровня по Z.
        /// </summary>
        /// <param name="zOrder">Уровень по Z от 0 и выше (значение меняется на 1).</param>
        public void HideCanvases(int zOrder)
        {
            foreach (var entry in this.canvases[zOrder * 2])
                foreach (var canvas in entry.Value)
                    canvas.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что виден ли слой.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        /// <returns>Значение, указывающее на то, что виден ли слой.</returns>
        public bool IsLayerVisible(object id)
        {
            if (this.canvases[0][id][0].Visibility == Visibility.Visible)
                return true;

            return false;
        }

        /// <summary>
        /// Печатает выбранную область карты.
        /// </summary>
        public void Print()
        {
            var dlg = new PrintDialog()
            {
                PrintQueue = this.SelectedPrinter
            };

            // Скрываем область печати.
            this.PrintArea.Visibility = Visibility.Hidden;

            // Убираем трансформацию масштабирования.
            var prevTransform = this.parentGrid.LayoutTransform;
            this.parentGrid.LayoutTransform = null;

            Exception exception = null;

            try
            {
                var mapPaginator = new MapPaginator(this, this.parentGrid, this.PrintArea);

                dlg.PrintDocument(mapPaginator, "Печать схемы");
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Восстанавливаем трансформацию.
            this.parentGrid.LayoutTransform = prevTransform;

            // Отображаем область печати.
            this.PrintArea.Visibility = Visibility.Visible;

            if (exception != null)
                throw exception;
        }

        /// <summary>
        /// Печатает выбранную область карты в PNG-файл.
        /// </summary>
        /// <param name="output">Папка для сохранения.</param>
        /// <param name="dpi">DPI.</param>
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
        public void PrintAsPNG(string output, int dpi)
        {
            // Скрываем область печати.
            this.PrintArea.Visibility = Visibility.Hidden;

            // Убираем трансформацию масштабирования.
            var prevTransform = this.parentGrid.LayoutTransform;
            this.parentGrid.LayoutTransform = null;

            Exception exception = null;

            var tempFile = "";

            try
            {
                var mapPaginator = new MapPaginator(this, this.parentGrid, this.PrintArea, true);

                tempFile = Path.GetTempFileName();

                using (var fileStream = File.Open(tempFile, FileMode.Open))
                {
                    var package = Package.Open(fileStream, FileMode.OpenOrCreate);
                    var doc = new XpsDocument(package);
                    var writer = XpsDocument.CreateXpsDocumentWriter(doc);

                    writer.Write(mapPaginator);

                    doc.Close();
                    package.Close();
                }
            }
            catch (Exception e)
            {
                exception = e;

                File.Delete(tempFile);
            }

            // Восстанавливаем трансформацию.
            this.parentGrid.LayoutTransform = prevTransform;

            // Отображаем область печати.
            this.PrintArea.Visibility = Visibility.Visible;

            if (exception != null)
                throw exception;

            // Пользуемся программкой для сохранения изображения.
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    Arguments = "\"" + tempFile + "\" \"" + output + "\" " + dpi.ToString(),
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Kts.XpsToImg.exe"),
                    UseShellExecute = false
                }
            };
            process.Start();
            process.WaitForExit();

            File.Delete(tempFile);
        }

        /// <summary>
        /// Печатает выбранную область карты в XPS-файл.
        /// </summary>
        /// <param name="path">Путь к XPS-файлу.</param>
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
        public void PrintAsXPS(string path)
        {
            // Скрываем область печати.
            this.PrintArea.Visibility = Visibility.Hidden;

            // Убираем трансформацию масштабирования.
            var prevTransform = this.parentGrid.LayoutTransform;
            this.parentGrid.LayoutTransform = null;

            Exception exception = null;

            try
            {
                var mapPaginator = new MapPaginator(this, this.parentGrid, this.PrintArea, true);

                using (var fileStream = File.Create(path))
                {
                    var package = Package.Open(fileStream, FileMode.OpenOrCreate);
                    var doc = new XpsDocument(package);
                    var writer = XpsDocument.CreateXpsDocumentWriter(doc);

                    writer.Write(mapPaginator);

                    doc.Close();
                    package.Close();
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Восстанавливаем трансформацию.
            this.parentGrid.LayoutTransform = prevTransform;

            // Отображаем область печати.
            this.PrintArea.Visibility = Visibility.Visible;

            if (exception != null)
                throw exception;
        }

        /// <summary>
        /// Печатает выбранную область карты через XPS-файл.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes")]
        public void PrintXPS()
        {
            // Скрываем область печати.
            this.PrintArea.Visibility = Visibility.Hidden;

            // Убираем трансформацию масштабирования.
            var prevTransform = this.parentGrid.LayoutTransform;
            this.parentGrid.LayoutTransform = null;

            Exception exception = null;

            var tempFile = "";

            try
            {
                var mapPaginator = new MapPaginator(this, this.parentGrid, this.PrintArea, true);

                tempFile = Path.GetTempFileName();

                using (var fileStream = File.Open(tempFile, FileMode.Open))
                {
                    var package = Package.Open(fileStream, FileMode.OpenOrCreate);
                    var doc = new XpsDocument(package);
                    var writer = XpsDocument.CreateXpsDocumentWriter(doc);

                    writer.Write(mapPaginator);
                    
                    doc.Close();
                    package.Close();
                }

                this.SelectedPrinter.AddJob("Печать схемы", tempFile, false);
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                File.Delete(tempFile);
            }

            // Восстанавливаем трансформацию.
            this.parentGrid.LayoutTransform = prevTransform;

            // Отображаем область печати.
            this.PrintArea.Visibility = Visibility.Visible;

            if (exception != null)
                throw exception;
        }

        /// <summary>
        /// Удаляет дополнительный слой с заданным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого слоя.</param>
        public void RemoveLayer(Guid id)
        {
            var layer = this.additionalLayers[id];

            if (this.additionalLayersPanelFront.HasVisual(layer))
                this.additionalLayersPanelFront.DeleteVisual(layer);
            else
                this.additionalLayersPanelBack.DeleteVisual(layer);

            this.additionalLayers.Remove(id);
        }

        /// <summary>
        /// Сохраняет заданный кусок карты в файл-изображение.
        /// </summary>
        /// <param name="path">Путь к сохраняемому файлу.</param>
        /// <param name="leftTopCorner">Левый верхний угол куска карты.</param>
        /// <param name="size">Размер куска карты.</param>
        public void SaveAsImage(string path, Point leftTopCorner, Size size)
        {
            int targetWidth = Convert.ToInt32(size.Width);
            int targetHeight = Convert.ToInt32(size.Height);

            var dpi = 96d;
            var rtb = new RenderTargetBitmap(targetWidth, targetHeight, dpi, dpi, PixelFormats.Default);
            var dv = new DrawingVisual();

            using (var dc = dv.RenderOpen())
            {
                var vb = new VisualBrush(this.parentGrid)
                {
                    Viewbox = new Rect(leftTopCorner.X / this.parentGrid.ActualWidth, leftTopCorner.Y / this.parentGrid.ActualHeight, size.Width / this.parentGrid.ActualWidth, size.Height / this.parentGrid.ActualHeight)
                };

                dc.DrawRectangle(vb, null, new Rect(new Point(0, 0), new Size(targetWidth, targetHeight)));
            }

            rtb.Render(dv);

            // Вызываем функцию сохранения, соответствующую расширению файла.
            switch (Path.GetExtension(path))
            {
                case ".gif":
                    this.SaveRTBAsGIF(rtb, path);

                    break;

                case ".jpeg":
                case ".jpg":
                    this.SaveRTBAsJPG(rtb, path);

                    break;

                case ".png":
                    this.SaveRTBAsPNG(rtb, path);

                    break;
            }

            // Принудительно вызываем сборку мусора.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Скроллит содержимое карты по указанным величинам отступа.
        /// </summary>
        /// <param name="horizontalOffset">Горизонтальный отступ.</param>
        /// <param name="verticalOffset">Вертикальный отступ.</param>
        public void ScrollTo(double horizontalOffset, double verticalOffset)
        {
            this.canUpdateSubstrate = false;

            this.MapHorizontalOffset = horizontalOffset;

            this.canUpdateSubstrate = true;

            this.MapVerticalOffset = verticalOffset;
        }

        /// <summary>
        /// Скроллит содержимое карты к заданной точке, центрируя ее.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void ScrollToAndCenter(Point point)
        {
            // При изменении скролла мы должны временно заблокировать возможность обновления подложки карты, чтобы не выполнять одни и те же действия два раза.
            this.canUpdateSubstrate = false;
            this.MapHorizontalOffset = point.X * this.Scale - this.scrollViewer.ViewportWidth / 2;
            this.canUpdateSubstrate = true;
            this.MapVerticalOffset = point.Y * this.Scale - this.scrollViewer.ViewportHeight / 2;
        }

        /// <summary>
        /// Скроллит содержимое карты к ее центру.
        /// </summary>
        public void ScrollToCenter()
        {
            this.canUpdateSubstrate = false;

            this.MapHorizontalOffset = this.grid.Width / 2 - this.scrollViewer.ViewportWidth / 2;

            this.canUpdateSubstrate = true;

            this.MapVerticalOffset = this.grid.Height / 2 - this.scrollViewer.ViewportHeight / 2;
        }

        /// <summary>
        /// Задает автоматическую легенду карты.
        /// </summary>
        /// <param name="legend">Автоматическая легенда.</param>
        public void SetAutoLegend(AutoLegend legend)
        {
            // Добавляем привязку к видимости легенды.
            var multiBinding = new MultiBinding()
            {
                Converter = new BooleansToVisibilityConverter(),
                NotifyOnSourceUpdated = true
            };
            multiBinding.Bindings.Add(new Binding(nameof(this.IsDataLoaded))
            {
                Source = this
            });
            multiBinding.Bindings.Add(new Binding(nameof(this.IsLegendVisible))
            {
                Source = this
            });
            legend.SetBinding(VisibilityProperty, multiBinding);
        }

        /// <summary>
        /// Задает значение, указывающее на то, что загружены ли данные карты.
        /// </summary>
        /// <param name="value">Значение, указывающее на то, что загружены ли данные карты.</param>
        public void SetIsDataLoaded(bool value)
        {
            foreach (var entry in this.canvases.Values)
                foreach (var canvases in entry.Values)
                    foreach (var item in canvases)
                        item.IsReady = value;
            if (this.defaultCanvas != null)
                this.defaultCanvas.IsReady = value;

            if (!value)
                // Убираем границы карты.
                this.border.BorderThickness = new Thickness(0);
        }

        /// <summary>
        /// Задает видимость области печати.
        /// </summary>
        /// <param name="value">Значение видимости.</param>
        public void SetIsPrintAreaVisible(bool value)
        {
            if (value)
            {
                this.SetIsPrintAreaVisible(false);

                this.PrintArea = new PrintArea(this)
                {
                    Position = new Point((this.MapHorizontalOffset + this.scrollViewer.ViewportWidth / 2) / this.Scale, (this.MapVerticalOffset + this.scrollViewer.ViewportHeight / 2) / this.Scale),
                    Printer = this.SelectedPrinter
                };

                this.PrintArea.Fit(new Size(this.scrollViewer.ViewportWidth / this.Scale, this.scrollViewer.ViewportHeight / this.Scale));

                this.PrintArea.Center();

                this.PrintArea.AddToCanvas(this.defaultCanvas);
            }
            else
                if (this.PrintArea != null)
                {
                    this.PrintArea.RemoveFromCanvas();

                    this.PrintArea = null;
                }
        }

        /// <summary>
        /// Задает видимость подложки карты.
        /// </summary>
        /// <param name="value">Значение видимости.</param>
        public void SetIsSubstrateVisible(bool value)
        {
            if (value)
                this.imageGrid.Visibility = Visibility.Visible;
            else
                this.imageGrid.Visibility = Visibility.Hidden;

            this.UpdateSubstrate();
        }

        /// <summary>
        /// Задает прозрачность слоя.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        /// <param name="value">Значение прозрачности.</param>
        public void SetLayerOpacity(object id, double value)
        {
            foreach (var entry in this.canvases.Values)
                foreach (var canvas in entry[id])
                    canvas.Opacity = value;
        }

        /// <summary>
        /// Задает видимость слоя.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        /// <param name="isVisible">Значение, указывающее на то, что виден ли слой.</param>
        public void SetLayerVisibility(object id, bool isVisible)
        {
            foreach (var entry in this.canvases.Values)
                foreach (var canvas in entry[id])
                    canvas.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Задает тип действия над картой.
        /// </summary>
        /// <param name="mapAction">Действие над картой.</param>
        public void SetMapAction(MapAction mapAction)
        {
            // Задаем курсор карты.
            this.SetMapCursor(mapAction);

            if (mapAction != MapAction.Draw && this.isDrawing)
                // Если тип действия над картой не рисование, но в это время оно выполняется, то принудительно завершаем его.
                this.OnDrawingCompleted(new Point(), true);

            if (this.isPasting)
            {
                this.isPasting = false;

                this.PasteCanceled?.Invoke(this, EventArgs.Empty);
            }

            // Управляем видимостью области редактирования объектов.
            if (this.groupArea != null)
                if (mapAction == MapAction.EditGroup)
                    this.groupArea.Visibility = Visibility.Visible;
                else
                    this.groupArea.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Задает подложку карты.
        /// </summary>
        public void SetSubstrate()
        {
            if (this.grid == null)
                return;

            // Убираем все контейнеры, содержащие части подложки.
            foreach (Grid child in this.imageGrid.Children)
            {
                foreach (SmartImage image in child.Children)
                    image.Source = null;

                child.Children.Clear();
            }
            this.imageGrid.Children.Clear();

            this.imageGrid.Width = this.SubstrateSize.Width;
            this.imageGrid.Height = this.SubstrateSize.Height;

            this.blockSizes = new List<Size>();

            if (this.SubstrateTiles != null && this.SubstrateTiles.Length > 0)
                for (int i = 0; i < this.SubstrateTiles.Length; i++)
                {
                    // Создаем новый контейнер изображений и задаем его столбцы и строки.
                    this.imageGrid.Children.Add(new Grid()
                    {
                        IsHitTestVisible = false
                    });
                    var grid = this.imageGrid.Children[i] as Grid;
                    // Определяем текущее количество тайлов.
                    var columnCount = this.SubstrateDimension.Width;
                    var rowCount = this.SubstrateDimension.Height;
                    for (int j = 0; j < i; j++)
                    {
                        columnCount = Math.Ceiling(columnCount / 4);
                        rowCount = Math.Ceiling(rowCount / 4);
                    }
                    ColumnDefinition columnDefinition;
                    for (int j = 0; j < columnCount; j++)
                    {
                        columnDefinition = new ColumnDefinition()
                        {
                            Width = new GridLength(0, GridUnitType.Auto)
                        };

                        grid.ColumnDefinitions.Add(columnDefinition);
                    }
                    RowDefinition rowDefinition;
                    for (int j = 0; j < rowCount; j++)
                    {
                        rowDefinition = new RowDefinition()
                        {
                            Height = new GridLength(0, GridUnitType.Auto)
                        };

                        grid.RowDefinitions.Add(rowDefinition);
                    }

                    double maxHeight = 0;
                    double maxWidth = 0;

                    // Подготавливаем элементы, отображающие изображения.
                    SmartImage smartImage;
                    for (int j = 0; j < rowCount; j++)
                        for (int k = 0; k < columnCount; k++)
                        {
                            smartImage = new SmartImage()
                            {
                                SnapsToDevicePixels = true,
                                Opacity = 0,
                                Visibility = Visibility.Hidden
                            };
                            
                            Grid.SetColumn(smartImage, k);
                            Grid.SetRow(smartImage, j);
                            
                            // Получаем размер соответствующего изображения, чтобы заранее задать размер.
                            var bitmapDecoder = BitmapDecoder.Create(new Uri(this.SubstrateTiles[i][k + Convert.ToInt32(columnCount) * j]), BitmapCreateOptions.None, BitmapCacheOption.None);
                            var frame = bitmapDecoder.Frames[0];
                            smartImage.Height = frame.PixelHeight;
                            smartImage.Width = frame.PixelWidth;

                            if (i > 0)
                            {
                                smartImage.Height *= i * 2;
                                smartImage.Width *= i * 2;
                            }

                            // Определяем максимальный размер блока подложки.
                            if (smartImage.Height > maxHeight)
                                maxHeight = smartImage.Height;
                            if (smartImage.Width > maxWidth)
                                maxWidth = smartImage.Width;

                            grid.Children.Add(smartImage);
                        }

                    this.blockSizes.Add(new Size(maxWidth, maxHeight));
                }

            this.grid.Width = this.imageGrid.Width;
            this.grid.Height = this.imageGrid.Height;

            // Подгоняем размер миникарты.
            this.minimap.FitToMaxSize();

            this.SetIndents();

            this.ScrollToCenter();

            this.UpdateSubstrate();
        }

        /// <summary>
        /// Задает прозрачность подложки карты.
        /// </summary>
        /// <param name="value">Значение прозрачности.</param>
        public void SetSubstrateOpacity(double value)
        {
            this.imageGrid.Opacity = value;
        }

        /// <summary>
        /// Задает миниатюру подложки карты.
        /// </summary>
        public void SetThumbnail()
        {
            this.minimap.LoadImage(this.ThumbnailPath);
        }

        /// <summary>
        /// Начинает вставку объекта.
        /// </summary>
        /// <param name="mapObject">Объект карты.</param>
        public void StartPaste(IMapObject mapObject)
        {
            this.pastingObject = mapObject;

            (this.pastingObject as IInteractiveShape).StartMoveTo();

            this.isPasting = true;
        }

        #endregion
    }

    // Реализация Control.
    internal sealed partial class Map
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.containerGrid = this.GetTemplateChild(containerGridName) as Grid;
            this.border = this.GetTemplateChild(borderName) as Border;
            this.imageGrid = this.GetTemplateChild(imageGridName) as Grid;
            this.grid = this.GetTemplateChild(gridName) as Grid;
            this.scrollViewer = this.GetTemplateChild(scrollViewerName) as ScrollViewer;

            this.parentGrid = this.imageGrid.Parent as Grid;

            this.SetRowsAndColumns();
            this.SetCanvases();

            if (this.AutoLegend != null)
                // Добавляем легенду на карту.
                this.containerGrid.Children.Add(this.AutoLegend);

            // Добавляем миникарту.
            this.minimap = new Minimap(this);
            this.containerGrid.Children.Add(this.minimap);

            this.grid.MouseLeave += this.Grid_MouseLeave;
            this.grid.MouseLeftButtonDown += this.Grid_MouseLeftButtonDown;

            this.grid.PreviewMouseLeftButtonDown += this.Grid_PreviewMouseLeftButtonDown;
            this.grid.PreviewMouseLeftButtonUp += this.Grid_PreviewMouseLeftButtonUp;
            this.grid.PreviewMouseMove += this.Grid_PreviewMouseMove;
            this.grid.PreviewMouseRightButtonDown += this.Grid_PreviewMouseRightButtonDown;
            this.grid.PreviewMouseRightButtonUp += this.Grid_PreviewMouseRightButtonUp;

            this.scrollViewer.PreviewKeyDown += this.ScrollViewer_PreviewKeyDown;
            this.scrollViewer.PreviewMouseMove += this.ScrollViewer_PreviewMouseMove;
            this.scrollViewer.PreviewMouseWheel += this.ScrollViewer_PreviewMouseWheel;
            this.scrollViewer.ScrollChanged += this.ScrollViewer_ScrollChanged;
        }

        #endregion
    }
}