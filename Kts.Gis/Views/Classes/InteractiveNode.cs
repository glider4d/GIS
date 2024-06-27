using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивный узел.
    /// </summary>
    [Serializable]
    internal sealed partial class InteractiveNode : InteractiveShape
    {
        #region Закрытые поля

        /// <summary>
        /// Коэффициент привязки по X.
        /// </summary>
        private double coefficientX;

        /// <summary>
        /// Коэффициент привязки по Y.
        /// </summary>
        private double coefficientY;

        /// <summary>
        /// Фигура, к которой прикреплен узел.
        /// </summary>
        private InteractiveFigure connectedFigure;

        /// <summary>
        /// Значение, указывающее на то, что подключен ли узел к другому узлу.
        /// </summary>
        private bool isConnectedToNode;

        /// <summary>
        /// Положение левого верхнего угла.
        /// </summary>
        private Point leftTopCorner;

        /// <summary>
        /// Предыдущее положение левого верхнего угла.
        /// </summary>
        private Point prevLeftTopCorner;

        /// <summary>
        /// Кисть границы узла.
        /// </summary>
        private SolidColorBrush strokeBrush;

        /// <summary>
        /// Треугольник.
        /// </summary>
        private Polygon triangle;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть заполнения.
        /// </summary>
        private static SolidColorBrush fillBrush = new SolidColorBrush(Colors.White);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveNode"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="size">Размер.</param>
        /// <param name="leftTopCorner">Положение левого верхнего угла.</param>
        /// <param name="toolTip">Содержимое подсказки, при наведении курсора на узел.</param>
        /// <param name="thickness">Толщина обводки узла.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveNode(NodeViewModel viewModel, Size size, Point leftTopCorner, string toolTip, double thickness, IMapBindingService mapBindingService) : base(viewModel, new Ellipse(), mapBindingService)
        {
            this.Shape.Width = size.Width;
            this.Shape.Height = size.Height;

            this.LeftTopCorner = leftTopCorner;

            this.IsConnectedToNode = false;
            
            this.Shape.StrokeThickness = thickness;

            if (!string.IsNullOrEmpty(toolTip))
                this.Shape.ToolTip = toolTip;
        }

        #endregion

        #region Статические конструкторы

        /// <summary>
        /// Инициализирует статические члены класса <see cref="InteractiveNode"/>.
        /// </summary>
        static InteractiveNode()
        {
            if (fillBrush.CanFreeze)
                fillBrush.Freeze();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает фигуру, к которой прикреплен узел.
        /// </summary>
        public InteractiveFigure ConnectedFigure
        {
            get
            {
                return this.connectedFigure;
            }
            set
            {
                if (this.connectedFigure != null)
                    this.connectedFigure.PropertyChanged -= this.ConnectedFigure_PropertyChanged;

                this.connectedFigure = value;

                if (value != null)
                {
                    this.connectedFigure.PropertyChanged += this.ConnectedFigure_PropertyChanged;

                    this.RecalculateCoefficients();
                    
                    this.Shape.Fill = this.StrokeBrush;
                }
                else
                    this.Shape.Fill = fillBrush;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что подключен ли узел к другому узлу.
        /// </summary>
        public bool IsConnectedToNode
        {
            get
            {
                return this.isConnectedToNode;
            }
            set
            {
                this.isConnectedToNode = value;

                if (value || this.ConnectedFigure != null)
                    this.Shape.Fill = this.StrokeBrush;
                else
                    this.Shape.Fill = fillBrush;
            }
        }

        /// <summary>
        /// Возвращает или задает положение левого верхнего угла узла.
        /// </summary>
        public Point LeftTopCorner
        {
            get
            {
                return this.leftTopCorner;
            }
            set
            {
                var oldValue = this.LeftTopCorner;

                var newValue = value;

                if (this.ConnectedFigure == null || this.ConnectedFigure != null && this.ConnectedFigure.IsPointIn(value))
                {
                    if (this.ConnectedFigure != null && !(this.ViewModel as NodeViewModel).IgnoreStick)
                        newValue = this.ConnectedFigure.GetNearestPoint(value);

                    this.leftTopCorner = newValue;

                    System.Windows.Controls.Canvas.SetLeft(this.Shape, newValue.X - this.Shape.Width / 2);
                    System.Windows.Controls.Canvas.SetTop(this.Shape, newValue.Y - this.Shape.Height / 2);

                    if (this.IsInitialized)
                        this.Shape.UpdateLayout();

                    if (oldValue != newValue)
                        this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.LeftTopCorner), newValue);

                    if (this.ConnectedFigure != null)
                        this.RecalculateCoefficients();
                }
                else
                    if (oldValue != newValue)
                        this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.LeftTopCorner), oldValue);

                if (this.triangle != null)
                    this.RearrangeTriangle();
            }
        }

        /// <summary>
        /// Возвращает или задает кисть границы узла.
        /// </summary>
        public SolidColorBrush StrokeBrush
        {
            get
            {
                return this.strokeBrush;
            }
            set
            {
                if (this.StrokeBrush != value)
                {
                    this.strokeBrush = value;

                    this.Shape.Stroke = this.strokeBrush;
                }
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> фигуры, к которой прикреплен узел.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ConnectedFigure_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(InteractiveFigure.Angle):
                case nameof(InteractiveFigure.LeftTopCorner):
                case nameof(InteractiveFigure.Size):
                    this.LeftTopCorner = this.ConnectedFigure.GetRotatedPointPosition(new Point(this.ConnectedFigure.LeftTopCorner.X + this.ConnectedFigure.Size.Width / this.coefficientX, this.ConnectedFigure.LeftTopCorner.Y + this.ConnectedFigure.Size.Height / this.coefficientY), false);

                    break;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ContextMenu.Closed"/> контекстного меню.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (this.Shape.ContextMenu != null)
            {
                this.Shape.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.Shape.ContextMenu = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> треугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Triangle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.HideTriangle();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Перестраиваем положение треугольника.
        /// </summary>
        private void RearrangeTriangle()
        {
            var center = this.CenterPoint;

            this.triangle.Points.Clear();

            this.triangle.Points.Add(new Point(center.X - this.MapBindingService.MapSettingService.NodeTriangleXOffset, center.Y - this.MapBindingService.MapSettingService.NodeTriangleYOffset));
            this.triangle.Points.Add(new Point(center.X + this.MapBindingService.MapSettingService.NodeTriangleXOffset, center.Y - this.MapBindingService.MapSettingService.NodeTriangleYOffset));
            this.triangle.Points.Add(new Point(center.X, center.Y - this.Shape.ActualWidth / 2));
        }

        /// <summary>
        /// Пересчитывает коэффициенты прикрепления узла к фигуре.
        /// </summary>
        private void RecalculateCoefficients()
        {
            var point = this.ConnectedFigure.GetRotatedPointPosition(this.LeftTopCorner, true);

            this.coefficientX = this.ConnectedFigure.Size.Width / (point.X - this.ConnectedFigure.LeftTopCorner.X);
            this.coefficientY = this.ConnectedFigure.Size.Height / (point.Y - this.ConnectedFigure.LeftTopCorner.Y);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения узла.
        /// </summary>
        /// <param name="prevLeftTopCorner">Предыдущее положение левого верхнего угла узла.</param>
        private void SetMoveCompleted(Point prevLeftTopCorner)
        {
            if (Math.Abs(prevLeftTopCorner.X - this.LeftTopCorner.X) >= 1 || Math.Abs(prevLeftTopCorner.Y - this.LeftTopCorner.Y) >= 1)
                (this.ViewModel as NodeViewModel).OnLeftTopCornerChanged(prevLeftTopCorner);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Скрывает треугольник над узлом.
        /// </summary>
        public void HideTriangle()
        {
            if (this.triangle == null)
                return;

            this.triangle.PreviewMouseLeftButtonDown += this.Triangle_PreviewMouseLeftButtonDown;

            (this.triangle.Tag as Canvas).Children.Remove(this.triangle);

            this.triangle = null;
        }

        /// <summary>
        /// Отображает треугольник над узлом.
        /// </summary>
        public void ShowTriangle()
        {
            if (this.triangle != null)
                return;

            this.triangle = new Polygon()
            {
                Fill = this.Shape.Stroke,
                Stroke = this.Shape.Stroke,
                StrokeThickness = 1
            };

            this.triangle.PreviewMouseLeftButtonDown += this.Triangle_PreviewMouseLeftButtonDown;

            this.RearrangeTriangle();

            this.Canvas.Children.Add(this.triangle);

            // Добавляем в тег треугольника ссылку на холст, чтобы мы могли потом убрать его из этого холста.
            this.triangle.Tag = this.Canvas;
        }

        #endregion
    }

    // Реализация InteractiveShape.
    internal sealed partial class InteractiveNode
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что может ли фигура быть выделенной.
        /// </summary>
        public override bool CanBeHighlighted
        {
            get
            {
                return false;
            }
            set
            {
                // Ничего не делаем.
            }
        }

        /// <summary>
        /// Возвращает центральную точку фигуры.
        /// </summary>
        public override Point CenterPoint
        {
            get
            {
                return this.LeftTopCorner;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что видна ли фигура.
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return this.Canvas.Map.IsLayerVisible((this.ViewModel as NodeViewModel).ConnectedLinesType);
            }
        }

        /// <summary>
        /// Возвращает важную точку фигуры.
        /// </summary>
        public override Point MajorPoint
        {
            get
            {
                return this.LeftTopCorner;
            }
        }

        #endregion

        #region Защищенные переопределенные свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выбрать фигуру.
        /// </summary>
        protected override bool CanBeSelected
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void HideUI()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnContextMenuRequested(Point mousePosition)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню редактирования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnEditContextMenuRequested(Point mousePosition)
        {
            this.Shape.ContextMenu = Application.Current.Resources["NodeEditContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.Shape.ContextMenu.DataContext = null;
            this.Shape.ContextMenu.DataContext = this.ViewModel;

            this.Shape.ContextMenu.Closed += this.ContextMenu_Closed;

            this.Shape.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с двойным нажатием мыши по фигуре.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnMouseDoubleClick(Point mousePosition)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с перемещением фигуры.
        /// </summary>
        /// <param name="deltaX">Изменение положения фигуры по X.</param>
        /// <param name="deltaY">Изменение положения фигуры по Y.</param>
        protected override void OnMoving(double deltaX, double deltaY)
        {
            var point = this.LeftTopCorner;

            this.LeftTopCorner = new Point(point.X + deltaX, point.Y + deltaY);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения фигуры.
        /// </summary>
        protected override void OnMovingCompleted()
        {
            this.SetMoveCompleted(this.prevLeftTopCorner);
        }

        /// <summary>
        /// Выполняет действия, связанные с началом перемещения фигуры.
        /// </summary>
        protected override void OnMovingStarted()
        {
            this.prevLeftTopCorner = this.LeftTopCorner;
        }

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void ShowUI()
        {
            // Ничего не делаем.
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Завершает перемещение фигуры. Используется для закрепления результата работы метода <see cref="MoveTo(Point)"/>.
        /// </summary>
        public override void EndMoveTo()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Возвращает геометрию фигуры.
        /// </summary>
        /// <returns>Геометрия.</returns>
        public override Geometry GetGeometry()
        {
            var result = new EllipseGeometry(this.CenterPoint, this.Shape.Width / 2, this.Shape.Height / 2);

            if (result.CanFreeze)
                result.Freeze();

            return result;
        }

        /// <summary>
        /// Возвращает кисть обводки фигуры.
        /// </summary>
        /// <returns>Кисть обводки.</returns>
        public override Pen GetStrokePen()
        {
            return null;
        }

        /// <summary>
        /// Перемещает фигуру в заданную точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        public override void MoveTo(Point point)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Начинает перемещение фигуры.
        /// </summary>
        public override void StartMoveTo()
        {
            // Ничего не делаем.
        }

        #endregion
    }
}