using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет изменялку размеров прямоугольника.
    /// </summary>
    internal sealed partial class RectangleResizeThumb : FigureResizeThumb
    {
        #region Закрытые поля

        /// <summary>
        /// Угол поворота прямоугольника в радианах.
        /// </summary>
        private double angleInRadians;

        /// <summary>
        /// Предыдущее положение левого верхнего угла прямоугольника.
        /// </summary>
        private Point prevLeftTopCorner;

        /// <summary>
        /// Предыдущий размер прямоугольника.
        /// </summary>
        private Size prevSize;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        private readonly InteractiveRectangle rectangle;

        /// <summary>
        /// Толщина обводки изменялки.
        /// </summary>
        private readonly double thickness;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RectangleResizeThumb"/>.
        /// </summary>
        /// <param name="rectangle">Прямоугольник.</param>
        /// <param name="position">Положение изменялки на холсте.</param>
        /// <param name="horizontal">Выравнивание изменялки по горизонтали.</param>
        /// <param name="vertical">Выравнивание изменялки по вертикали.</param>
        /// <param name="cursor">Курсор изменялки.</param>
        /// <param name="size">Размер изменялки.</param>
        /// <param name="thickness">Толщина обводки изменялки.</param>
        public RectangleResizeThumb(InteractiveRectangle rectangle, Point position, HorizontalAlignment horizontal, VerticalAlignment vertical, Cursor cursor, double size, double thickness)
        {
            this.rectangle = rectangle;
            this.thickness = thickness;

            // Задаем центральную точку трансформации.
            this.RenderTransformOrigin = this.rectangle.TransformOrigin;

            // Задаем размеры изменялки прямо в коде, так как из стиля размеры подхватываются только при загрузке элемента, а размер нам нужен уже при задании начального положения изменялки на холсте.
            this.Height = size;
            this.Width = size;

            this.Position = position;
            this.HorizontalAlignment = horizontal;
            this.VerticalAlignment = vertical;
            this.Cursor = cursor;

            this.DragStarted += this.RectangleResizeThumb_DragStarted;
            this.DragDelta += this.RectangleResizeThumb_DragDelta;
            this.DragCompleted += this.RectangleResizeThumb_DragCompleted;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RectangleResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.rectangle.SetResizeCompleted(this.prevSize, this.prevLeftTopCorner);

            this.rectangle.ShowBackground();

            this.rectangle.ShowAllThumbs();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RectangleResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double deltaVertical, deltaHorizontal;

            var p = this.rectangle.LeftTopCorner;

            switch (this.VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange, this.rectangle.Size.Height - this.rectangle.MinHeight);

                    this.rectangle.Size = new Size(this.rectangle.Size.Width, this.rectangle.Size.Height - deltaVertical);
                    this.rectangle.LeftTopCorner = new Point(p.X - deltaVertical * this.RenderTransformOrigin.Y * Math.Sin(-this.angleInRadians), p.Y + this.RenderTransformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angleInRadians)));

                    break;

                case VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange, this.rectangle.Size.Height - this.rectangle.MinHeight);

                    this.rectangle.Size = new Size(this.rectangle.Size.Width, this.rectangle.Size.Height - deltaVertical);
                    this.rectangle.LeftTopCorner = new Point(p.X + deltaVertical * Math.Sin(-this.angleInRadians) - this.RenderTransformOrigin.Y * deltaVertical * Math.Sin(-this.angleInRadians), p.Y + deltaVertical * Math.Cos(-this.angleInRadians) + this.RenderTransformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angleInRadians)));

                    break;
            }

            p = this.rectangle.LeftTopCorner;

            switch (this.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange, this.rectangle.Size.Width - this.rectangle.MinWidth);

                    this.rectangle.Size = new Size(this.rectangle.Size.Width - deltaHorizontal, this.rectangle.Size.Height);
                    this.rectangle.LeftTopCorner = new Point(p.X + deltaHorizontal * Math.Cos(this.angleInRadians) + this.RenderTransformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angleInRadians)), p.Y + deltaHorizontal * Math.Sin(this.angleInRadians) - this.RenderTransformOrigin.X * deltaHorizontal * Math.Sin(this.angleInRadians));

                    break;

                case HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange, this.rectangle.Size.Width - this.rectangle.MinWidth);

                    this.rectangle.Size = new Size(this.rectangle.Size.Width - deltaHorizontal, this.rectangle.Size.Height);
                    this.rectangle.LeftTopCorner = new Point(p.X + deltaHorizontal * this.RenderTransformOrigin.X * (1 - Math.Cos(this.angleInRadians)), p.Y - this.RenderTransformOrigin.X * deltaHorizontal * Math.Sin(this.angleInRadians));

                    break;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RectangleResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.angleInRadians = this.Angle * Math.PI / 180.0;

            this.prevSize = this.rectangle.Size;
            this.prevLeftTopCorner = this.rectangle.LeftTopCorner;

            this.rectangle.CollapseAllThumbsExceptThis(this);

            this.rectangle.HideBackground();
        }

        #endregion
    }

    // Реализация Control.
    internal sealed partial class RectangleResizeThumb
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