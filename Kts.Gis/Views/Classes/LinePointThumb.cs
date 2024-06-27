using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет изменялку точки изгиба линии.
    /// </summary>
    internal sealed partial class LinePointThumb : FigureResizeThumb
    {
        #region Закрытые поля

        /// <summary>
        /// Пункт меню дублирования точки изгиба линии.
        /// </summary>
        private MenuItem menuItemCopy;

        /// <summary>
        /// Пункт меню удаления точки изгиба линии.
        /// </summary>
        private MenuItem menuItemDelete;

        /// <summary>
        /// Пункт меню превращения узла изгиба в узел соединения.
        /// </summary>
        private MenuItem menuItemTransform;

        /// <summary>
        /// Предыдущая структура точек изгиба линии.
        /// </summary>
        private string prevPoints;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Линия.
        /// </summary>
        private readonly InteractiveLine line;

        /// <summary>
        /// Толщина обводки изменялки.
        /// </summary>
        private readonly double thickness;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Текущий подписчик на события контекстного меню.
        /// </summary>
        private static LinePointThumb subscriber;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LinePointThumb"/>.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="position">Положение изменялки точки изгиба линии на холсте.</param>
        /// <param name="cursor">Курсор изменялки точки изгиба линии.</param>
        /// <param name="index">Индекс точки изгиба.</param>
        /// <param name="strokeBrush">Кисть границы изменялки.</param>
        /// <param name="size">Размер изменялки.</param>
        /// <param name="thickness">Толщина обводки изменялки.</param>
        public LinePointThumb(InteractiveLine line, Point position, Cursor cursor, int index, Brush strokeBrush, double size, double thickness)
        {
            this.line = line;
            this.Index = index;
            this.thickness = thickness;

            // Задаем размеры изменялки прямо в коде, так как из стиля размеры подхватываются только при загрузке элемента, а размер нам нужен уже при задании начального положения изменялки на холсте.
            this.Height = size;
            this.Width = size;

            this.Position = position;
            this.Cursor = cursor;

            this.BorderBrush = strokeBrush;

            this.DragStarted += this.LinePointThumb_DragStarted;
            this.DragDelta += this.LinePointThumb_DragDelta;
            this.DragCompleted += this.LinePointThumb_DragCompleted;

            this.PreviewMouseRightButtonUp += this.LinePointThumb_PreviewMouseRightButtonUp;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает индекс точки изгиба.
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
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню дублирования точки изгиба линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            this.line.InsertVertex(this.Index);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню удаления точки изгиба линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.line.DeleteVertex(this.Index);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MenuItem.Click"/> пункта меню превращения узла изгиба в узел соединения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MenuItemTransform_Click(object sender, RoutedEventArgs e)
        {
            this.line.TransformVertex(this.Index);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> изменялки точки изгиба линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LinePointThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.line.SetPointsChanged(this.prevPoints);

            this.line.ShowAllThumbs();

            this.line.ShowLabels();

            this.line.ShowBadges();

            this.line.CanBeHighlighted = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> изменялки точки изгиба линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LinePointThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Position = new Point(this.Position.X + e.HorizontalChange, this.Position.Y + e.VerticalChange);

            this.line.ChangePoint(this.Index, this.Position);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> изменялки точки изгиба линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LinePointThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.prevPoints = this.line.Points;

            this.line.CanBeHighlighted = false;

            this.line.CollapseAllThumbsExceptThis(this);

            this.line.HideLabels();

            this.line.HideBadges();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> изменялки точки изгиба линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LinePointThumb_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Если уже есть подписчик на контекстное меню, то высвобождаем его, иначе будет пидьиэс.
            if (subscriber != null)
                subscriber.ReleaseContextMenu();

            subscriber = this;

            this.ContextMenu = Application.Current.Resources["LinePointContextMenu"] as ContextMenu;

            this.ContextMenu.Closed += this.ContextMenu_Closed;

            // Работаем с пунктом меню дублирования точки изгиба линии.
            this.menuItemCopy = LogicalTreeHelper.FindLogicalNode(this.ContextMenu, "menuItemCopy") as MenuItem;
            this.menuItemCopy.Click += this.MenuItemCopy_Click;

            // Работаем с пунктом меню удаления точки изгиба линии.
            this.menuItemDelete = LogicalTreeHelper.FindLogicalNode(this.ContextMenu, "menuItemDelete") as MenuItem;
            this.menuItemDelete.IsEnabled = this.line.CanDeletePoint(this.Index);
            this.menuItemDelete.Click += this.MenuItemDelete_Click;

            this.menuItemTransform = LogicalTreeHelper.FindLogicalNode(this.ContextMenu, "menuItemTransform") as MenuItem;
            this.menuItemTransform.Click += this.MenuItemTransform_Click;

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

                // Отписываемся от нажатий на пункты меню.
                this.menuItemCopy.Click -= this.MenuItemCopy_Click;
                this.menuItemDelete.Click -= this.MenuItemDelete_Click;
                this.menuItemTransform.Click -= this.MenuItemTransform_Click;

                this.ContextMenu = null;
            }
        }

        #endregion
    }

    // Реализация Control.
    internal sealed partial class LinePointThumb
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            (this.Template.FindName("Rectangle", this) as Rectangle).StrokeThickness = this.thickness;
        }

        #endregion
    }
}