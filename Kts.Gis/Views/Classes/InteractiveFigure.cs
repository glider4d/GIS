using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивную фигуру.
    /// </summary>
    [Serializable]
    internal abstract partial class InteractiveFigure : InteractiveShape, IDrawableObject, ILabeledObject, INotifyPropertyChanged
    {
        #region Закрытые поля

        /// <summary>
        /// Угол поворота фигуры.
        /// </summary>
        private double angle;

        /// <summary>
        /// Значение, указывающее на то, что скрыт ли фон фигуры.
        /// </summary>
        private bool isBackgroundHidden;

        /// <summary>
        /// Положение левого верхнего угла фигуры.
        /// </summary>
        private Point leftTopCorner;

        /// <summary>
        /// Начальная кисть фона фигуры.
        /// </summary>
        private Brush originalBrush;

        /// <summary>
        /// Предыдущее положение левого верхнего угла фигуры.
        /// </summary>
        private Point prevLeftTopCorner;

        /// <summary>
        /// Размер фигуры.
        /// </summary>
        private Size size;

        /// <summary>
        /// Ручка обводки границы фигуры.
        /// </summary>
        private Pen strokePen;

        #endregion

        #region Закрытые неизменяемые поля
        
        /// <summary>
        /// Фигура.
        /// </summary>
        private readonly Shape figure;

        /// <summary>
        /// Модель представления фигуры.
        /// </summary>
        private readonly FigureViewModel figureViewModel;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть границы фигуры.
        /// </summary>
        private static SolidColorBrush strokeBrush = new SolidColorBrush(Colors.Black);

        /// <summary>
        /// Прозрачная кисть.
        /// </summary>
        private static SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveFigure"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="figure">Фигура.</param>
        /// <param name="fillBrush">Кисть фона.</param>
        /// <param name="strokePen">Ручка обводки границы.</param>
        /// <param name="thickness">Толщина границы фигуры.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveFigure(FigureViewModel viewModel, Shape figure, Brush fillBrush, Pen strokePen, double thickness, IMapBindingService mapBindingService) : base(viewModel, figure, mapBindingService)
        {
            this.figureViewModel = viewModel;
            this.figure = figure;

            this.ChangeColor(fillBrush, strokePen);

            this.figure.Stroke = strokeBrush;
            this.figure.StrokeThickness = thickness;

            this.ChangeBorders(viewModel.IsPlanning);

            this.Label = new SmartFigureLabel(this, viewModel.LabelAngle, viewModel.LabelPosition, viewModel.LabelSize, mapBindingService.MapSettingService);
        }

        #endregion

        #region Статические конструкторы

        /// <summary>
        /// Инициализирует статические члены класса <see cref="InteractiveFigure"/>.
        /// </summary>
        static InteractiveFigure()
        {
            if (strokeBrush.CanFreeze)
                strokeBrush.Freeze();

            if (transparentBrush.CanFreeze)
                transparentBrush.Freeze();
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает или задает изменялки размеров фигуры.
        /// </summary>
        protected FigureResizeThumb[] ResizeThumbs
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает вращалки фигуры.
        /// </summary>
        protected FigureRotateThumb[] RotateThumbs
        {
            get;
            set;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает координаты левой нижней вершины ограничивающего прямоугольника.
        /// </summary>
        public Point LeftBottom
        {
            get
            {
                return this.GetRotatedPointPosition(new Point(this.LeftTopCorner.X, this.LeftTopCorner.Y + this.Size.Height), false);
            }
        }

        /// <summary>
        /// Возвращает координаты левой верхней вершины ограничивающего прямоугольника.
        /// </summary>
        public Point LeftTop
        {
            get
            {
                return this.GetRotatedPointPosition(this.LeftTopCorner, false);
            }
        }

        /// <summary>
        /// Возвращает или задает положение левого верхнего угла фигуры.
        /// </summary>
        public Point LeftTopCorner
        {
            get
            {
                return this.leftTopCorner;
            }
            set
            {
                var prevValue = this.LeftTopCorner;

                if (this.LeftTopCorner != value)
                {
                    this.leftTopCorner = value;

                    System.Windows.Controls.Canvas.SetLeft(this.figure, value.X);
                    System.Windows.Controls.Canvas.SetTop(this.figure, value.Y);

                    if (this.IsInitialized)
                        this.figure.UpdateLayout();

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.LeftTopCorner), value);

                    this.NotifyPropertyChanged(nameof(this.LeftTopCorner));
                }

                if (this.IsInitialized)
                {
                    // Передвигаем надпись.
                    this.Label.Relocate(new Point(value.X - prevValue.X, value.Y - prevValue.Y));

                    if (this.IsEditing)
                        // Передвигаем элементы управления.
                        this.MoveUI();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает минимальную высоту фигуры.
        /// </summary>
        public double MinHeight
        {
            get
            {
                return this.figure.MinHeight;
            }
            set
            {
                this.figure.MinHeight = value;
            }
        }

        /// <summary>
        /// Возвращает или задает минимальную ширину фигуры.
        /// </summary>
        public double MinWidth
        {
            get
            {
                return this.figure.MinWidth;
            }
            set
            {
                this.figure.MinWidth = value;
            }
        }

        /// <summary>
        /// Возвращает координаты правой нижней вершины ограничивающего прямоугольника.
        /// </summary>
        public Point RightBottom
        {
            get
            {
                return this.GetRotatedPointPosition(new Point(this.LeftTopCorner.X + this.Size.Width, this.LeftTopCorner.Y + this.Size.Height), false);
            }
        }

        /// <summary>
        /// Возвращает координаты правой верхней вершины ограничивающего прямоугольника.
        /// </summary>
        public Point RightTop
        {
            get
            {
                return this.GetRotatedPointPosition(new Point(this.LeftTopCorner.X + this.Size.Width, this.LeftTopCorner.Y), false);
            }
        }

        /// <summary>
        /// Возвращает или задает центральную точку трансформации.
        /// </summary>
        public Point TransformOrigin
        {
            get
            {
                return this.figure.RenderTransformOrigin;
            }
            set
            {
                this.figure.RenderTransformOrigin = value;
            }
        }

        #endregion

        #region Открытые виртуальные свойства

        /// <summary>
        /// Возвращает или задает угол поворота фигуры.
        /// </summary>
        public virtual double Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                if (this.Angle != value)
                {
                    this.angle = value;

                    var rt = this.figure.RenderTransform as RotateTransform;

                    if (rt == null)
                        this.figure.RenderTransform = new RotateTransform(value);
                    else
                        rt.Angle = value;

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.Angle), value);

                    if (this.IsInitialized && this.IsEditing)
                    {
                        // Передвигаем элементы управления.
                        this.MoveUI();

                        // Передвигаем надпись.
                        this.Label.Relocate(new Point(0, 0));
                    }

                    this.NotifyPropertyChanged(nameof(this.Angle));
                }
            }
        }

        /// <summary>
        /// Возвращает или задает размер фигуры.
        /// </summary>
        public virtual Size Size
        {
            get
            {
                return this.size;
            }
            set
            {
                if (this.Size != value)
                {
                    this.size = value;

                    this.figure.Width = value.Width;
                    this.figure.Height = value.Height;

                    if (this.IsInitialized)
                        this.figure.UpdateLayout();

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.Size), new Tuple<Size, Point>(value, this.LeftTopCorner));

                    this.NotifyPropertyChanged(nameof(this.Size));
                }
            }
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
            if (this.figure.ContextMenu != null)
            {
                this.figure.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.figure.ContextMenu = null;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Уведомляет об изменении свойства фигуры.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения фигуры.
        /// </summary>
        /// <param name="prevLeftTopCorner">Предыдущее положение левого верхнего угла фигуры.</param>
        private void SetMoveCompleted(Point prevLeftTopCorner)
        {
            if (Math.Abs(prevLeftTopCorner.X - this.LeftTopCorner.X) >= 1 || Math.Abs(prevLeftTopCorner.Y - this.LeftTopCorner.Y) >= 1)
                this.figureViewModel.OnPositionChanged(prevLeftTopCorner);
        }

        #endregion

        #region Защищенные абстрактные методы

        /// <summary>
        /// Передвигает элементы управления свойствами фигуры.
        /// </summary>
        protected abstract void MoveUI();

        #endregion

        #region Защищенные виртуальные методы

        /// <summary>
        /// Скрывает все вращалки и изменялки.
        /// </summary>
        protected virtual void CollapseAllThumbs()
        {
            if (this.RotateThumbs != null)
                for (int i = 0; i < this.RotateThumbs.Length; i++)
                    this.RotateThumbs[i].Visibility = Visibility.Collapsed;

            if (this.ResizeThumbs != null)
                for (int i = 0; i < this.ResizeThumbs.Length; i++)
                    this.ResizeThumbs[i].Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Меняет границы фигуры в зависимости от значения, указывающего на то, что является ли фигура планируемой.
        /// </summary>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли фигура планируемой.</param>
        public void ChangeBorders(bool isPlanning)
        {
            if (isPlanning)
                this.figure.StrokeDashArray = new DoubleCollection()
                {
                    this.MapBindingService.MapSettingService.FigurePlanningOffset
                };
            else
                this.figure.StrokeDashArray = null;
        }

        /// <summary>
        /// Меняет цвет фигуры.
        /// </summary>
        /// <param name="brush">Кисть.</param>
        /// <param name="pen">Ручка.</param>
        public void ChangeColor(Brush brush, Pen pen)
        {
            this.originalBrush = brush;

            this.figure.Fill = brush;

            this.strokePen = pen;
        }

        /// <summary>
        /// Вычисляет новое положение точки принимая во внимание угол поворота фигуры.
        /// </summary>
        /// <param name="point">Искомая точка.</param>
        /// <param name="isReverted">Указывает на направление поворота фигуры. Если true, то поворот фигуры происходит в обратную сторону.</param>
        /// <returns>Точка.</returns>
        public Point GetRotatedPointPosition(Point point, bool isReverted)
        {
            // Переводим из градусов в радианы.
            double angle = this.Angle != 0 ? (isReverted ? 1 : -1) * this.Angle * Math.PI / 180 : this.Angle;

            return new Point(this.CenterPoint.X + (point.X - this.CenterPoint.X) * Math.Cos(angle) + (point.Y - this.CenterPoint.Y) * Math.Sin(angle), this.CenterPoint.Y - (point.X - this.CenterPoint.X) * Math.Sin(angle) + (point.Y - this.CenterPoint.Y) * Math.Cos(angle));
        }

        /// <summary>
        /// Скрывает фон фигуры.
        /// </summary>
        public void HideBackground()
        {
            if (!this.isBackgroundHidden)
            {
                this.isBackgroundHidden = true;

                this.CanBeHighlighted = false;

                this.figure.Fill = transparentBrush;
            }
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи.
        /// </summary>
        /// <param name="prevAngle">Предыдущий угол поворота надписи.</param>
        public void SetLabelAngleChanged(double prevAngle)
        {
            if (Math.Abs(this.Label.Angle - prevAngle) > 1)
                this.figureViewModel.LabelAngle = this.Label.Angle;
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением изменения положения надписи.
        /// </summary>
        /// <param name="prevPosition">Предыдущее положение надписи.</param>
        public void SetLabelMoved(Point prevPosition)
        {
            if (Math.Abs(prevPosition.X - this.Label.LeftTopCorner.X) >= 1 || Math.Abs(prevPosition.Y - this.Label.LeftTopCorner.Y) >= 1)
                this.figureViewModel.LabelPosition = this.Label.LeftTopCorner;
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением размера надписи.
        /// </summary>
        public void SetLabelSizeChanged()
        {
            this.figureViewModel.LabelSize = this.Label.Size;
        }

        /// <summary>
        /// Задает отступ внутри границы планируемой фигуры.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public void SetPlanningOffset(double offset)
        {
            if (this.figure.StrokeDashArray != null)
                this.figure.StrokeDashArray[0] = offset;
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением изменения размеров фигуры.
        /// </summary>
        /// <param name="prevSize">Предыдущий размер фигуры.</param>
        /// <param name="prevLeftTopCorner">Предыдущее положение левого верхнего угла фигуры.</param>
        public void SetResizeCompleted(Size prevSize, Point prevLeftTopCorner)
        {
            if (Math.Abs(prevSize.Width - this.Size.Width) >= 1 || Math.Abs(prevSize.Height - this.Size.Height) >= 1)
                this.figureViewModel.OnSizeChanged(prevSize, prevLeftTopCorner);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением поворота фигуры.
        /// </summary>
        /// <param name="prevAngle">Предыдущий угол поворота.</param>
        public void SetRotateCompleted(double prevAngle)
        {
            if (prevAngle != this.Angle)
                this.figureViewModel.OnAngleChanged(prevAngle);
        }

        /// <summary>
        /// Задает толщину границы фигуры.
        /// </summary>
        /// <param name="thickness">Толщина границы фигуры.</param>
        public void SetThickness(double thickness)
        {
            this.figure.StrokeThickness = thickness;
        }

        /// <summary>
        /// Показывает фон фигуры.
        /// </summary>
        public void ShowBackground()
        {
            if (this.isBackgroundHidden)
            {
                this.isBackgroundHidden = false;

                this.CanBeHighlighted = true;

                this.figure.Fill = this.originalBrush;
            }
        }

        /// <summary>
        /// Начинает анимирование фигуры.
        /// </summary>
        public void StartAnimation()
        {
            this.MapBindingService.StartAnimation(this);
        }

        /// <summary>
        /// Заканчивает анимирование фигуры.
        /// </summary>
        public void StopAnimation()
        {
            this.MapBindingService.StopAnimation(this);
        }

        #endregion

        #region Открытые абстрактные методы

        /// <summary>
        /// Возвращает ближайшую к границе фигуры от заданной точки точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Точка на границе фигуры.</returns>
        public abstract Point GetNearestPoint(Point point);

        /// <summary>
        /// Возвращает true, если заданная точка находится внутри фигуры. Иначе - false.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Значение, указывающее на то, что находится ли точка внутри фигуры.</returns>
        public abstract bool IsPointIn(Point point);

        #endregion

        #region Открытые виртуальные методы
        
        /// <summary>
        /// Скрывает все вращалки и изменялки, кроме заданного элемента.
        /// </summary>
        /// <param name="element">Элемент, который останется видимым.</param>
        public virtual void CollapseAllThumbsExceptThis(UIElement element)
        {
            if (this.RotateThumbs != null)
                for (int i = 0; i < this.RotateThumbs.Length; i++)
                    if (this.RotateThumbs[i] != element)
                        this.RotateThumbs[i].Visibility = Visibility.Collapsed;

            if (this.ResizeThumbs != null)
                for (int i = 0; i < this.ResizeThumbs.Length; i++)
                    if (this.ResizeThumbs[i] != element)
                        this.ResizeThumbs[i].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Показывает все вращалки и изменялки.
        /// </summary>
        public virtual void ShowAllThumbs()
        {
            if (this.RotateThumbs != null)
                for (int i = 0; i < this.RotateThumbs.Length; i++)
                    this.RotateThumbs[i].Visibility = Visibility.Visible;

            if (this.ResizeThumbs != null)
                for (int i = 0; i < this.ResizeThumbs.Length; i++)
                    this.ResizeThumbs[i].Visibility = Visibility.Visible;
        }

        #endregion
    }

    // Реализация InteractiveShape.
    internal abstract partial class InteractiveFigure
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает центральную точку фигуры.
        /// </summary>
        public override Point CenterPoint
        {
            get
            {
                return new Point(this.LeftTopCorner.X + this.Size.Width / 2, this.LeftTopCorner.Y + this.Size.Height / 2);
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

        #region Защищенные переопределенные методы

        /// <summary>
        /// Скрывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void HideUI()
        {
            // Удаляем все вращалки и изменялки размеров фигуры с холста.
            var index = this.RotateThumbs != null ? this.Canvas.Children.IndexOf(this.RotateThumbs[0]) : (this.ResizeThumbs != null ? this.Canvas.Children.IndexOf(this.ResizeThumbs[0]) : 0);
            this.Canvas.Children.RemoveRange(index, (this.RotateThumbs != null ? this.RotateThumbs.Length : 0) + (this.ResizeThumbs != null ? this.ResizeThumbs.Length : 0));

            this.RotateThumbs = null;
            this.ResizeThumbs = null;
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnContextMenuRequested(Point mousePosition)
        {
            this.figure.ContextMenu = Application.Current.Resources["FigureContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.figure.ContextMenu.DataContext = null;
            this.figure.ContextMenu.DataContext = this.ViewModel;

            this.figure.ContextMenu.Closed += this.ContextMenu_Closed;

            this.figure.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню редактирования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnEditContextMenuRequested(Point mousePosition)
        {
            this.figure.ContextMenu = Application.Current.Resources["FigureEditContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.figure.ContextMenu.DataContext = null;
            this.figure.ContextMenu.DataContext = this.ViewModel;

            this.figure.ContextMenu.Closed += this.ContextMenu_Closed;

            this.figure.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с инициализацией фигуры.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.MinHeight = this.MapBindingService.MapSettingService.FigureMinSize;
            this.MinWidth = this.MapBindingService.MapSettingService.FigureMinSize;

            this.TransformOrigin = new Point(0.5, 0.5);

            if (this.Size.Height < this.MinHeight || this.Size.Width < this.MinWidth)
            {
                // Если размеры меньше минимального, то меняем их.
                double height = this.Size.Height < this.MinHeight ? this.MinHeight : this.Size.Height;
                double width = this.Size.Width < this.MinWidth ? this.MinWidth : this.Size.Width;
                this.Size = new Size(width, height);
            }

            if (this.Label != null && this.ViewModel as INamedObjectViewModel != null)
                this.Label.SetText((this.ViewModel as INamedObjectViewModel).Name);
        }

        /// <summary>
        /// Выполняет действия, связанные с двойным нажатием мыши по фигуре.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnMouseDoubleClick(Point mousePosition)
        {
            this.figureViewModel.SelectConnectionsCommand.Execute(null);
        }

        /// <summary>
        /// Выполняет действия, связанные с перемещением фигуры.
        /// </summary>
        /// <param name="deltaX">Изменение положения фигуры по X.</param>
        /// <param name="deltaY">Изменение положения фигуры по Y.</param>
        protected override void OnMoving(double deltaX, double deltaY)
        {
            this.CollapseAllThumbs();

            this.HideBackground();

            var point = this.LeftTopCorner;

            this.LeftTopCorner = new Point(point.X + deltaX, point.Y + deltaY);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения фигуры.
        /// </summary>
        protected override void OnMovingCompleted()
        {
            this.SetMoveCompleted(this.prevLeftTopCorner);

            this.ShowBackground();

            this.ShowAllThumbs();
        }

        /// <summary>
        /// Выполняет действия, связанные с началом перемещения фигуры.
        /// </summary>
        protected override void OnMovingStarted()
        {
            this.prevLeftTopCorner = this.LeftTopCorner;
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
        /// Возвращает кисть обводки фигуры.
        /// </summary>
        /// <returns>Кисть обводки.</returns>
        public override Pen GetStrokePen()
        {
            return this.strokePen;
        }

        /// <summary>
        /// Перемещает фигуру в заданную точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        public override void MoveTo(Point point)
        {
            this.LeftTopCorner = new Point(point.X - this.Size.Width / 2, point.Y - this.Size.Height / 2);
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

    // Реализация IDrawableObject.
    internal abstract partial class InteractiveFigure
    {
        #region Открытые абстрактные методы

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        public abstract void Draw(Point mousePrevPosition, Point mousePosition);

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        public abstract bool IsDrawCompleted(Point mousePosition);

        #endregion
    }

    // Реализация ILabeledObject.
    internal abstract partial class InteractiveFigure
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает надпись.
        /// </summary>
        public SmartLabel Label
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет действия, связанные с изменением надписи.
        /// </summary>
        /// <param name="newAngle">Новый угол поворота надписи.</param>
        /// <param name="newPosition">Новое положение надписи.</param>
        /// <param name="newSize">Новый размер шрифта надписи.</param>
        public void OnLabelChanged(int? newAngle, int? newPosition, int? newSize)
        {
            this.figureViewModel.SetValue(nameof(FigureViewModel.LabelSize), newSize);
            this.figureViewModel.SetValue(nameof(FigureViewModel.LabelPosition), newPosition);
            this.figureViewModel.SetValue(nameof(FigureViewModel.LabelAngle), newAngle);
        }

        #endregion
    }

    // Реализация INotifyPropertyChanged.
    internal abstract partial class InteractiveFigure
    {
        #region Открытые события

        /// <summary>
        /// Событие изменения свойства фигуры.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}