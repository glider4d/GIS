using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет область редактирования группы объектов.
    /// </summary>
    internal sealed partial class GroupArea : Control, IMapObject
    {
        #region Закрытые константы

        /// <summary>
        /// Значок перемещения.
        /// </summary>
        private const string moveBadge = "F1 M 30.25,58L 18,58L 18,45.75L 22,41.75L 22,50.75L 30,42.75L 33.25,46L 25.25,54L 34.25,54L 30.25,58 Z M 58,45.75L 58,58L 45.75,58L 41.75,54L 50.75,54L 42.75,46L 46,42.75L 54,50.75L 54,41.75L 58,45.75 Z M 45.75,18L 58,18L 58,30.25L 54,34.25L 54,25.25L 46,33.25L 42.75,30L 50.75,22L 41.75,22L 45.75,18 Z M 18,30.25L 18,18L 30.25,18L 34.25,22L 25.25,22L 33.25,30L 30,33.25L 22,25.25L 22,34.25L 18,30.25 Z";

        /// <summary>
        /// Значок изменения размера.
        /// </summary>
        private const string resizeBadge = "F1 M 54.2499,34L 42,34L 42,21.7501L 45.9999,17.7501L 45.9999,26.7501L 53.9999,18.7501L 57.2499,22.0001L 49.2499,30.0001L 58.2499,30.0001L 54.2499,34 Z M 34,21.7501L 34,34L 21.75,34L 17.75,30.0001L 26.75,30.0001L 18.75,22.0001L 22,18.7501L 30,26.7501L 30,17.7501L 34,21.7501 Z M 21.75,42L 34,42L 34,54.25L 30,58.25L 30,49.25L 22,57.25L 18.75,54L 26.75,46L 17.75,46L 21.75,42 Z M 42,54.25L 42,42L 54.2499,42L 58.2499,46L 49.2499,46.0001L 57.2499,54L 53.9999,57.25L 45.9999,49.25L 45.9999,58.25L 42,54.25 Z";

        /// <summary>
        /// Значок вращения.
        /// </summary>
        private const string rotateBadge = "F1 M 50.672,20.5864L 55.4219,25.3364L 55.422,38.0031L 42.7553,38.0031L 38.0053,33.2531L 46.8578,33.2522C 44.6831,30.8224 41.5227,29.2932 38.0052,29.2932C 31.4459,29.2932 26.1285,34.6106 26.1285,41.1699C 26.1285,44.4495 27.4579,47.4187 29.6071,49.5679L 25.6881,53.4869C 22.5359,50.3347 20.5862,45.9799 20.5862,41.1698C 20.5862,31.5494 28.385,23.7507 38.0053,23.7507C 42.9966,23.7507 47.4975,25.8499 50.6734,29.2137L 50.672,20.5864 Z";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значок.
        /// </summary>
        private Path badge;

        /// <summary>
        /// Холст, на котором находится точка области.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// Текущий тип точки области.
        /// </summary>
        private GroupAreaPointType currentPoint = GroupAreaPointType.Move;

        /// <summary>
        /// Панель для отрисовки очертаний объектов.
        /// </summary>
        private DrawingPanel drawingPanel;

        /// <summary>
        /// Начальное положение мыши.
        /// </summary>
        private Point initialPosition;

        /// <summary>
        /// Значение, указывающее на то, что было ли выполнено перемещение.
        /// </summary>
        private bool isMoved;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли перемещение области.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли перемещение точки области.
        /// </summary>
        private bool isPointMoving;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        private Point mousePrevPosition;

        /// <summary>
        /// Точка области, используемая для задания центра трансформации области.
        /// </summary>
        private Ellipse originPoint;

        /// <summary>
        /// Сетка, содержащая <see cref="originPoint"/>.
        /// </summary>
        private Grid originPointContainer;

        #endregion

        #region Открытые статические поля

        /// <summary>
        /// Положение точки области.
        /// </summary>
        public static DependencyProperty OriginPointPositionProperty = DependencyProperty.Register("OriginPointPosition", typeof(Point), typeof(GroupArea), new PropertyMetadata(new Point(), new PropertyChangedCallback(OriginPointPositionPropertyChanged)));

        /// <summary>
        /// Положение области.
        /// </summary>
        public static DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(GroupArea), new PropertyMetadata(new Point(), new PropertyChangedCallback(PositionPropertyChanged)));

        /// <summary>
        /// Размер области.
        /// </summary>
        public static DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Size), typeof(GroupArea), new PropertyMetadata(Size.Empty, new PropertyChangedCallback(SizePropertyChanged)));

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие изменения угла поворота области.
        /// </summary>
        public event EventHandler<AngleChangedEventArgs> AngleChanged;

        /// <summary>
        /// Событие изменения положения области.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        /// <summary>
        /// Событие изменения масштаба.
        /// </summary>
        public event EventHandler<ScaleChangedEventArgs> ScaleChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupArea"/>.
        /// </summary>
        public GroupArea()
        {
            this.MouseLeave += this.GroupArea_MouseLeave;
            this.PreviewMouseLeftButtonDown += this.GroupArea_PreviewMouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += this.GroupArea_PreviewMouseLeftButtonUp;
            this.PreviewMouseMove += this.GroupArea_PreviewMouseMove;

            this.IsVisibleChanged += this.GroupArea_IsVisibleChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает положение точки области.
        /// </summary>
        public Point OriginPointPosition
        {
            get
            {
                return (Point)this.GetValue(OriginPointPositionProperty);
            }
            set
            {
                this.SetValue(OriginPointPositionProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает положение области.
        /// </summary>
        public Point Position
        {
            get
            {
                return (Point)this.GetValue(PositionProperty);
            }
            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Возвращает или задает размер области.
        /// </summary>
        public Size Size
        {
            get
            {
                return (Size)this.GetValue(SizeProperty);
            }
            set
            {
                this.SetValue(SizeProperty, value);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_MouseLeave(object sender, MouseEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.DirectlyOver is Ellipse)
                // В данном случае мышка нажата по точке области.
                return;

            this.CaptureMouse();

            this.mousePrevPosition = e.GetPosition(this.Canvas);

            this.initialPosition = this.mousePrevPosition;

            this.isMoving = true;

            // Блокируем "всплытие" события.
            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isMoving)
                return;

            var p = e.GetPosition(this.Canvas);

            switch (this.currentPoint)
            {
                case GroupAreaPointType.Move:
                    var deltaX = p.X - this.mousePrevPosition.X;
                    var deltaY = p.Y - this.mousePrevPosition.Y;

                    // Сдвигаем область.
                    var point = this.Position;
                    this.Position = new Point(point.X + deltaX, point.Y + deltaY);

                    break;

                case GroupAreaPointType.Resize:
                    var oi = Math.Sqrt(Math.Pow(this.initialPosition.X - this.OriginPointPosition.X, 2) + Math.Pow(this.initialPosition.Y - this.OriginPointPosition.Y, 2));
                    var op = Math.Sqrt(Math.Pow(p.X - this.OriginPointPosition.X, 2) + Math.Pow(p.Y - this.OriginPointPosition.Y, 2));

                    var scale = op / oi;

                    this.RenderTransform = new ScaleTransform(scale, scale);

                    break;

                case GroupAreaPointType.Rotate:
                    // Вращаем область относительно центра ее трансформации.
                    var a = new Vector(this.initialPosition.X - this.OriginPointPosition.X, this.initialPosition.Y - this.OriginPointPosition.Y);
                    var b = new Vector(p.X - this.OriginPointPosition.X, p.Y - this.OriginPointPosition.Y);
                    
                    this.RenderTransform = new RotateTransform(Vector.AngleBetween(a, b));

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа со следующим типом точки области редактирования группы объектов: " + this.currentPoint.ToString());
            }

            this.mousePrevPosition = p;

            this.isMoved = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.IsVisibleChanged"/> области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void GroupArea_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                this.SetOriginPointPosition(this.OriginPointPosition);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ScaleChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Map_ScaleChanged(object sender, EventArgs e)
        {
            if (this.originPointContainer == null || this.badge == null)
                return;

            this.RecalculateSize();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> точки области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void originPoint_MouseLeave(object sender, MouseEventArgs e)
        {
            this.OnPointLeaveOrLeftButtonUp();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> точки области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void originPoint_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                this.HandlePointChange();
            else
            {
                this.originPoint.CaptureMouse();

                this.mousePrevPosition = e.GetPosition(this.canvas);

                this.initialPosition = new Point(this.Position.X, this.Position.Y);

                this.isPointMoving = true;
            }

            // Блокируем "всплытие" события.
            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> точки области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void originPoint_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnPointLeaveOrLeftButtonUp();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> точки области.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void originPoint_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isPointMoving)
                return;

            var p = e.GetPosition(this.canvas);

            var deltaX = p.X - this.mousePrevPosition.X;
            var deltaY = p.Y - this.mousePrevPosition.Y;

            // Сдвигаем точку области.
            var point = this.OriginPointPosition;
            this.OriginPointPosition = new Point(point.X + deltaX, point.Y + deltaY);

            this.mousePrevPosition = p;
        }

        #endregion

        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="OriginPointPositionProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void OriginPointPositionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var groupArea = source as GroupArea;
            var point = (Point)e.NewValue;

            groupArea.SetOriginPointPosition(point);
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="PositionProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void PositionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var groupArea = source as GroupArea;
            var point = (Point)e.NewValue;
            
            System.Windows.Controls.Canvas.SetLeft(groupArea, point.X);
            System.Windows.Controls.Canvas.SetTop(groupArea, point.Y);

            // Также сдвигаем точку области, чтобы она визуально не перемещалась относительно перемещенных объектов.
            groupArea.SetOriginPointPosition(new Point(groupArea.OriginPointPosition.X, groupArea.OriginPointPosition.Y));

            groupArea.UpdateLayout();
        }

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="SizeProperty"/>.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void SizePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var groupArea = source as GroupArea;
            var size = (Size)e.NewValue;

            groupArea.Width = size.Width;
            groupArea.Height = size.Height;

            groupArea.UpdateLayout();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет действия, связанные с уходом мыши с области или отжатии левой кнопки мыши.
        /// </summary>
        private void OnLeaveOrLeftButtonUp()
        {
            if (!this.isMoving)
                return;

            if (!this.isMoved)
                return;

            this.isMoving = false;

            this.isMoved = false;

            this.ReleaseMouseCapture();

            switch (this.currentPoint)
            {
                case GroupAreaPointType.Move:
                    var originPoint = this.OriginPointPosition;

                    var eventArgs = new PositionChangedEventArgs(new Point(this.Position.X, this.Position.Y));

                    this.PositionChanged.Invoke(this, eventArgs);

                    this.OriginPointPosition = new Point(originPoint.X + eventArgs.Delta.X, originPoint.Y + eventArgs.Delta.Y);

                    break;

                case GroupAreaPointType.Resize:
                    this.ScaleChanged.Invoke(this, new ScaleChangedEventArgs((this.RenderTransform as ScaleTransform).ScaleX, this.OriginPointPosition));

                    this.RenderTransform = null;

                    break;

                case GroupAreaPointType.Rotate:
                    this.AngleChanged.Invoke(this, new AngleChangedEventArgs((this.RenderTransform as RotateTransform).Angle, this.OriginPointPosition));

                    this.RenderTransform = null;

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа со следующим типом точки области редактирования группы объектов: " + this.currentPoint.ToString());
            }
        }

        /// <summary>
        /// Выполняет действия, связанные с уходом мыши с точки области или отжатии левой кнопки мыши.
        /// </summary>
        private void OnPointLeaveOrLeftButtonUp()
        {
            if (!this.isPointMoving)
                return;

            this.isPointMoving = false;

            this.originPoint.ReleaseMouseCapture();
        }

        /// <summary>
        /// Пересчитывает размер точки области.
        /// </summary>
        private void RecalculateSize()
        {
            var scale = this.Canvas.Map.Scale;

            var sizeDelta = this.originPointContainer.Width;

            this.originPointContainer.Width = 50 / scale;
            this.originPointContainer.Height = this.originPointContainer.Width;

            sizeDelta = (this.originPointContainer.Width - sizeDelta) / 2;

            this.badge.Width = this.originPointContainer.Width - this.originPointContainer.Width * 0.45;
            this.badge.Height = this.badge.Width;

            this.SetOriginPointPosition(new Point(this.OriginPointPosition.X - sizeDelta, this.OriginPointPosition.Y - sizeDelta));
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Отменяет все проделанные изменения.
        /// </summary>
        public void CancelChanges()
        {
            if (!this.isMoving)
                return;

            if (!this.isMoved)
                return;

            this.isMoving = false;

            this.isMoved = false;

            this.ReleaseMouseCapture();

            this.RenderTransform = null;
        }

        /// <summary>
        /// Убирает очертания объектов с карты.
        /// </summary>
        public void ClearOutlines()
        {
            if (this.drawingPanel == null)
                return;    

            this.drawingPanel.DeleteVisuals();
        }

        /// <summary>
        /// Создает очертания объектов.
        /// </summary>
        /// <param name="geometries">Геометрии объектов.</param>
        /// <param name="pens">Кисти обводки объектов.</param>
        public void CreateOutlines(List<Geometry> geometries, List<Pen> pens)
        {
            var layer = new DrawingVisual();

            using (var dc = layer.RenderOpen())
                for (int i = 0; i < geometries.Count; i++)
                    dc.DrawGeometry(null, pens[i], geometries[i]);

            this.drawingPanel.AddVisual(layer);
        }

        /// <summary>
        /// Управляет сменой точки области.
        /// </summary>
        public void HandlePointChange()
        {
            // Прерываем текущие действия с областью.
            this.OnLeaveOrLeftButtonUp();

            // Если был произведен двойной щелчок, то меняем тип точки области.
            switch (this.currentPoint)
            {
                case GroupAreaPointType.Move:
                    this.badge.Data = Geometry.Parse(rotateBadge);

                    this.currentPoint = GroupAreaPointType.Rotate;

                    break;

                case GroupAreaPointType.Resize:
                    this.badge.Data = Geometry.Parse(moveBadge);

                    this.currentPoint = GroupAreaPointType.Move;

                    break;

                case GroupAreaPointType.Rotate:
                    this.badge.Data = Geometry.Parse(resizeBadge);

                    this.currentPoint = GroupAreaPointType.Resize;

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа со следующим типом точки области редактирования группы объектов: " + this.currentPoint.ToString());
            }
        }

        /// <summary>
        /// Задает новое положение точки области.
        /// </summary>
        /// <param name="newPosition">Новое положение.</param>
        public void SetOriginPointPosition(Point newPosition)
        {
            System.Windows.Controls.Canvas.SetLeft(this.originPointContainer, newPosition.X - this.originPointContainer.Width / 2);
            System.Windows.Controls.Canvas.SetTop(this.originPointContainer, newPosition.Y - this.originPointContainer.Height / 2);

            this.originPointContainer.UpdateLayout();

            if (this.Width > 0 && this.Height > 0)
                // Задаем точку трансформации.
                this.RenderTransformOrigin = new Point(newPosition.X / this.Width, newPosition.Y / this.Height);
        }

        #endregion
    }

    // Реализация Control.
    internal sealed partial class GroupArea
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.drawingPanel = this.GetTemplateChild("drawingPanel") as DrawingPanel;
            this.canvas = this.GetTemplateChild("canvas") as Canvas;
            this.originPointContainer = this.GetTemplateChild("originPointContainer") as Grid;
            this.originPoint = this.GetTemplateChild("originPoint") as Ellipse;
            this.badge = this.GetTemplateChild("badge") as Path;

            this.originPoint.MouseLeave += this.originPoint_MouseLeave;
            this.originPoint.PreviewMouseLeftButtonDown += this.originPoint_PreviewMouseLeftButtonDown;
            this.originPoint.PreviewMouseLeftButtonUp += this.originPoint_PreviewMouseLeftButtonUp;
            this.originPoint.PreviewMouseMove += this.originPoint_PreviewMouseMove;

            this.badge.Data = Geometry.Parse(moveBadge);

            this.RecalculateSize();
        }

        #endregion
    }

    // Реализация IMapObject.
    internal sealed partial class GroupArea
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает холст, на котором расположен объект.
        /// </summary>
        public IndentableCanvas Canvas
        {
            get;
            private set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет объект на холст.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <returns>true, если объект был добавлен, иначе - false.</returns>
        public bool AddToCanvas(IndentableCanvas canvas)
        {
            if (this.Canvas != null)
                return false;

            this.Canvas = canvas;

            this.Canvas.Children.Add(this);

            this.Canvas.Map.ScaleChanged += this.Map_ScaleChanged;

            return true;
        }

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public bool RemoveFromCanvas()
        {
            if (this.Canvas == null)
                return false;

            this.Canvas.Children.Remove(this);

            this.Canvas.Map.ScaleChanged -= this.Map_ScaleChanged;

            this.Canvas = null;

            return true;
        }

        #endregion
    }
}