using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет вращалку фигуры.
    /// </summary>
    internal sealed class FigureRotateThumb : Thumb
    {
        #region Закрытые поля

        /// <summary>
        /// Начальный угол поворота фигуры.
        /// </summary>
        private double initialAngle;

        /// <summary>
        /// Начальный вектор, проведенный из центральной точки фигуры до положения курсора.
        /// </summary>
        private Vector startVector;

        #endregion
		
		#region Закрытые неизменяемые поля

        /// <summary>
        /// Фигура.
        /// </summary>
        private readonly InteractiveFigure figure;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FigureRotateThumb"/>.
        /// </summary>
        /// <param name="figure">Фигура.</param>
        /// <param name="position">Положение вращалки на холсте.</param>
        /// <param name="horizontal">Выравнивание вращалки по горизонтали.</param>
        /// <param name="vertical">Выравнивание вращалки по вертикали.</param>
        /// <param name="size">Размер крутилки.</param>
        public FigureRotateThumb(InteractiveFigure figure, Point position, HorizontalAlignment horizontal, VerticalAlignment vertical, double size)
        {
            this.figure = figure;

            // Задаем центральную точку трансформации.
            this.RenderTransformOrigin = this.figure.TransformOrigin;

            // Задаем размеры вращалки прямо в коде, так как из стиля размеры подхватываются только при загрузке элемента, а размер нам нужен уже при задании начального положения вращалки на холсте.
            this.Height = size;
            this.Width = size;

            this.Position = position;
            this.HorizontalAlignment = horizontal;
            this.VerticalAlignment = vertical;

            this.DragStarted += this.FigureRotateThumb_DragStarted;
            this.DragDelta += this.FigureRotateThumb_DragDelta;
            this.DragCompleted += this.FigureRotateThumb_DragCompleted;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Задает положение вращалки фигуры.
        /// </summary>
        public Point Position
        {
            set
            {
                // Сдвигаем вращалку так, чтобы соответствующая ей вершина ограничивающего прямоугольника фигуры приходились на центр вращалки.
                Canvas.SetLeft(this, value.X - this.Width / 2);
                Canvas.SetTop(this, value.Y - this.Height / 2);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> вращалки фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FigureRotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.figure.SetRotateCompleted(this.initialAngle);

            this.figure.ShowBackground();

            this.figure.ShowAllThumbs();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> вращалки фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FigureRotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this.figure.Canvas.Parent as IInputElement);
            var deltaVector = Point.Subtract(currentPoint, this.figure.CenterPoint);
            var angle = Vector.AngleBetween(this.startVector, deltaVector);

            this.figure.Angle = this.initialAngle + angle;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> вращалки фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FigureRotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            // Получаем положение мыши относительно родителя холста.
            var startPoint = Mouse.GetPosition(this.figure.Canvas.Parent as IInputElement);

            this.startVector = Point.Subtract(startPoint, this.figure.CenterPoint);

            this.initialAngle = this.figure.Angle;

            this.figure.CollapseAllThumbsExceptThis(this);

            this.figure.HideBackground();
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выводит вращалку фигуры на передний план.
        /// </summary>
        public void BringToFront()
        {
            // Задаем ей самый наибольший индекс, чтобы она была впереди всех.
            Panel.SetZIndex(this, int.MaxValue);
        }

        #endregion
    }
}