using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет изменялку вершины многоугольника.
    /// </summary>
    internal sealed partial class PolygonVertexThumb : FigureResizeThumb
    {
        #region Закрытые поля

        /// <summary>
        /// Пункт меню дублирования вершины многоугольника.
        /// </summary>
        private MenuItem menuItemCopy;

        /// <summary>
        /// Пункт меню удаления вершины многоугольника.
        /// </summary>
        private MenuItem menuItemDelete;

        /// <summary>
        /// Предыдущая структура вершин многоугольника.
        /// </summary>
        private string prevPoints;

        /// <summary>
        /// Предыдущее положение многоугольника.
        /// </summary>
        private Point prevPosition;

        /// <summary>
        /// Предыдущий размер многоугольника.
        /// </summary>
        private Size prevSize;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Многоугольник.
        /// </summary>
        private readonly InteractivePolygon polygon;

        /// <summary>
        /// Толщина изменялки вершины многоугольника.
        /// </summary>
        private readonly double thickness;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Текущий подписчик на события контекстного меню.
        /// </summary>
        private static PolygonVertexThumb subscriber;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PolygonVertexThumb"/>.
        /// </summary>
        /// <param name="polygon">Многоугольник.</param>
        /// <param name="position">Положение изменялки вершины многоугольника на холсте.</param>
        /// <param name="cursor">Курсор изменялки вершины многоугольника.</param>
        /// <param name="index">Индекс вершины.</param>
        /// <param name="size">Размер изменялки вершины многоугольника.</param>
        /// <param name="thickness">Толщина изменялки вершины многоугольника.</param>
        public PolygonVertexThumb(InteractivePolygon polygon, Point position, Cursor cursor, int index, double size, double thickness)
        {
            this.polygon = polygon;
            this.Index = index;
            this.thickness = thickness;

            // Задаем центральную точку трансформации.
            this.RenderTransformOrigin = this.polygon.TransformOrigin;

            // Задаем размеры изменялки прямо в коде, так как из стиля размеры подхватываются только при загрузке элемента, а размер нам нужен уже при задании начального положения изменялки на холсте.
            this.Height = size;
            this.Width = size;

            this.Position = position;
            this.Cursor = cursor;

            this.DragStarted += this.PolygonVertexThumb_DragStarted;
            this.DragDelta += this.PolygonVertexThumb_DragDelta;
            this.DragCompleted += this.PolygonVertexThumb_DragCompleted;

            this.PreviewMouseRightButtonUp += this.PolygonVertexThumb_PreviewMouseRightButtonUp;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает индекс вершины.
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ContextMenu.Closed"/> контекстного меню.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            this.ReleaseContextMenu();
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню дублирования вершины многоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            this.polygon.InsertVertex(this.Index);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню удаления вершины многоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.polygon.DeleteVertex(this.Index);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> изменялки вершины многоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PolygonVertexThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.polygon.SetPointsChanged(this.prevPoints, this.prevPosition, this.prevSize);

            this.polygon.ShowBackground();

            this.polygon.ShowAllThumbs();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> изменялки вершины многоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PolygonVertexThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Position = new Point(this.Position.X + e.HorizontalChange, this.Position.Y + e.VerticalChange);

            this.polygon.ChangePoint(this.Index, this.Position);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> изменялки вершины многоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PolygonVertexThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.prevPoints = this.polygon.Points;
            this.prevPosition = this.polygon.LeftTopCorner;
            this.prevSize = this.polygon.Size;

            this.polygon.CollapseAllThumbsExceptThis(this);

            this.polygon.HideBackground();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> изменялки вершины многоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PolygonVertexThumb_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Если уже есть подписчик на контекстное меню, то высвобождаем его, иначе будет пидьиэс.
            if (subscriber != null)
                subscriber.ReleaseContextMenu();

            subscriber = this;

            this.ContextMenu = Application.Current.Resources["VertexContextMenu"] as ContextMenu;

            this.ContextMenu.Closed += this.ContextMenu_Closed;

            // Работаем с пунктом меню дублирования вершины многоугольника.
            this.menuItemCopy = LogicalTreeHelper.FindLogicalNode(this.ContextMenu, "menuItemCopy") as MenuItem;
            this.menuItemCopy.Click += this.MenuItemCopy_Click;

            // Работаем с пунктом меню удаления вершины многоугольника.
            this.menuItemDelete = LogicalTreeHelper.FindLogicalNode(this.ContextMenu, "menuItemDelete") as MenuItem;
            this.menuItemDelete.IsEnabled = this.polygon.CanDeleteVertex(this.Index);
            this.menuItemDelete.Click += this.MenuItemDelete_Click;

            this.ContextMenu.IsOpen = true;

            e.Handled = true;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высвобождает контекстное меню.
        /// </summary>
        private void ReleaseContextMenu()
        {
            if (this.ContextMenu != null)
            {
                this.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.menuItemCopy.Click -= this.MenuItemCopy_Click;

                this.menuItemDelete.Click -= this.MenuItemDelete_Click;

                this.ContextMenu = null;
            }
        }

        #endregion
    }

    // Реализация Control.
    internal sealed partial class PolygonVertexThumb
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            (this.Template.FindName("Ellipse", this) as Ellipse).StrokeThickness = this.thickness;
        }

        #endregion
    }
}