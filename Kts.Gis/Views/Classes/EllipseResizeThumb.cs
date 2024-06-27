using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет изменялку размеров эллипса.
    /// </summary>
    internal sealed partial class EllipseResizeThumb : FigureResizeThumb
    {
        #region Закрытые поля

        /// <summary>
        /// Угол поворота эллипса в радианах.
        /// </summary>
        private double angleInRadians;

        /// <summary>
        /// Эллипс.
        /// </summary>
        private InteractiveEllipse ellipse;

        /// <summary>
        /// Предыдущее положение левого верхнего угла эллипса.
        /// </summary>
        private Point prevLeftTopCorner;

        /// <summary>
        /// Предыдущий размер эллипса.
        /// </summary>
        private Size prevSize;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Выравнивание изменялки по горизонтали.
        /// </summary>
        private readonly HorizontalAlignment? horizontal;

        /// <summary>
        /// Толщина обводки изменялки.
        /// </summary>
        private readonly double thickness;

        /// <summary>
        /// Выравнивание изменялки по вертикали.
        /// </summary>
        private readonly VerticalAlignment? vertical;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EllipseResizeThumb"/>.
        /// </summary>
        /// <param name="ellipse">Эллипс.</param>
        /// <param name="position">Положение изменялки на холсте.</param>
        /// <param name="cursor">Курсор изменялки.</param>
        /// <param name="horizontal">Выравнивание изменялки по горизонтали.</param>
        /// <param name="size">Размер изменялки.</param>
        /// <param name="thickness">Толщина обводки изменялки.</param>
        public EllipseResizeThumb(InteractiveEllipse ellipse, Point position, Cursor cursor, HorizontalAlignment horizontal, double size, double thickness)
        {
            this.horizontal = horizontal;
            this.thickness = thickness;

            this.Init(ellipse, position, cursor, size);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EllipseResizeThumb"/>.
        /// </summary>
        /// <param name="ellipse">Эллипс.</param>
        /// <param name="position">Положение изменялки на холсте.</param>
        /// <param name="cursor">Курсор изменялки.</param>
        /// <param name="vertical">Выравнивание изменялки по вертикали.</param>
        /// <param name="size">Размер изменялки.</param>
        /// <param name="thickness">Толщина обводки изменялки.</param>
        public EllipseResizeThumb(InteractiveEllipse ellipse, Point position, Cursor cursor, VerticalAlignment vertical, double size, double thickness)
        {
            this.vertical = vertical;
            this.thickness = thickness;

            this.Init(ellipse, position, cursor, size);
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> изменялки размеров эллипса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void EllipseResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.ellipse.SetResizeCompleted(this.prevSize, this.prevLeftTopCorner);

            this.ellipse.ShowBackground();

            this.ellipse.ShowAllThumbs();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> изменялки размеров эллипса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void EllipseResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var p = this.ellipse.LeftTopCorner;

            if (this.horizontal.HasValue)
            {
                double deltaHorizontal;

                switch (this.horizontal.Value)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, this.ellipse.Size.Width - this.ellipse.MinWidth);

                        this.ellipse.Size = new Size(this.ellipse.Size.Width - deltaHorizontal, this.ellipse.Size.Height);
                        this.ellipse.LeftTopCorner = new Point(p.X + deltaHorizontal * Math.Cos(this.angleInRadians) + this.RenderTransformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angleInRadians)), p.Y + deltaHorizontal * Math.Sin(this.angleInRadians) - this.RenderTransformOrigin.X * deltaHorizontal * Math.Sin(this.angleInRadians));

                        break;

                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, this.ellipse.Size.Width - this.ellipse.MinWidth);

                        this.ellipse.Size = new Size(this.ellipse.Size.Width - deltaHorizontal, this.ellipse.Size.Height);
                        this.ellipse.LeftTopCorner = new Point(p.X + deltaHorizontal * this.RenderTransformOrigin.X * (1 - Math.Cos(this.angleInRadians)), p.Y - this.RenderTransformOrigin.X * deltaHorizontal * Math.Sin(this.angleInRadians));

                        break;
                }
            }

            if (this.vertical.HasValue)
            {
                double deltaVertical;

                switch (this.vertical.Value)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, this.ellipse.Size.Height - this.ellipse.MinHeight);

                        this.ellipse.Size = new Size(this.ellipse.Size.Width, this.ellipse.Size.Height - deltaVertical);
                        this.ellipse.LeftTopCorner = new Point(p.X - deltaVertical * this.RenderTransformOrigin.Y * Math.Sin(-this.angleInRadians), p.Y + this.RenderTransformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angleInRadians)));

                        break;

                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, this.ellipse.Size.Height - this.ellipse.MinHeight);

                        this.ellipse.Size = new Size(this.ellipse.Size.Width, this.ellipse.Size.Height - deltaVertical);
                        this.ellipse.LeftTopCorner = new Point(p.X + deltaVertical * Math.Sin(-this.angleInRadians) - this.RenderTransformOrigin.Y * deltaVertical * Math.Sin(-this.angleInRadians), p.Y + deltaVertical * Math.Cos(-this.angleInRadians) + this.RenderTransformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angleInRadians)));

                        break;
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> изменялки размеров эллипса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void EllipseResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.angleInRadians = this.Angle * Math.PI / 180.0;

            this.prevSize = this.ellipse.Size;
            this.prevLeftTopCorner = this.ellipse.LeftTopCorner;

            this.ellipse.CollapseAllThumbsExceptThis(this);

            this.ellipse.HideBackground();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Инициализирует изменялку размеров эллипса.
        /// </summary>
        /// <param name="ellipse">Эллипс.</param>
        /// <param name="position">Положение изменялки на холсте.</param>
        /// <param name="cursor">Курсор изменялки.</param>
        /// <param name="size">Размер изменялки.</param>
        private void Init(InteractiveEllipse ellipse, Point position, Cursor cursor, double size)
        {
            this.ellipse = ellipse;

            // Задаем центральную точку трансформации.
            this.RenderTransformOrigin = this.ellipse.TransformOrigin;

            // Задаем размеры изменялки прямо в коде, так как из стиля размеры подхватываются только при загрузке элемента, а размер нам нужен уже при задании начального положения изменялки на холсте.
            this.Height = size;
            this.Width = size;

            this.Position = position;
            this.Cursor = cursor;

            this.DragStarted += this.EllipseResizeThumb_DragStarted;
            this.DragDelta += this.EllipseResizeThumb_DragDelta;
            this.DragCompleted += this.EllipseResizeThumb_DragCompleted;
        }

        #endregion
    }
    
    // Реализация Control.
    internal sealed partial class EllipseResizeThumb
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            (this.Template.FindName("Ellipse", this) as Ellipse).StrokeThickness = thickness;
        }

        #endregion
    }
}