using Kts.Gis.Services;
using Kts.WpfUtilities;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет линейку.
    /// </summary>
    internal sealed partial class Ruler : IDrawableObject, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что добавлена ли линейка на холст.
        /// </summary>
        private bool isOnCanvas;

        /// <summary>
        /// Конечная точка линейки.
        /// </summary>
        private Point endPoint;

        /// <summary>
        /// Полуотступ линии, которая находится на конце линейки.
        /// </summary>
        private double offset;

        /// <summary>
        /// Начальная точка линейки.
        /// </summary>
        private Point startPoint;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Линия, которая находится на конце линейки.
        /// </summary>
        private readonly Line endLine = new Line();

        /// <summary>
        /// Линия, представляющая линейку.
        /// </summary>
        private readonly Line line = new Line();

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Линия, которая находится на начале линейки.
        /// </summary>
        private readonly Line startLine = new Line();

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть линий.
        /// </summary>
        private static SolidColorBrush strokeBrush = new SolidColorBrush(Colors.Black);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Ruler"/>.
        /// </summary>
        /// <param name="startPoint">Начальная точка линейки.</param>
        /// <param name="endPoint">Конечная точка линейки.</param>
        /// <param name="thickness">Толщина линейки.</param>
        /// <param name="capSemiOffset">Полуотступ линии, которая находится на конце линейки.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public Ruler(Point startPoint, Point endPoint, double thickness, double offset, IMapBindingService mapBindingService)
        {
            this.offset = offset;
            this.mapBindingService = mapBindingService;

            if (strokeBrush.CanFreeze)
                strokeBrush.Freeze();

            this.line.Stroke = strokeBrush;
            this.line.StrokeThickness = thickness;

            this.startLine.Stroke = strokeBrush;
            this.startLine.StrokeThickness = thickness;
            this.endLine.Stroke = strokeBrush;
            this.endLine.StrokeThickness = thickness;

            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает начальную точку линейки.
        /// </summary>
        public Point StartPoint
        {
            get
            {
                return this.startPoint;
            }
            set
            {
                if (this.StartPoint != value)
                {
                    this.startPoint = value;

                    this.line.X1 = value.X;
                    this.line.Y1 = value.Y;

                    this.mapBindingService.SetMapObjectViewModelValue(this, nameof(this.StartPoint), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает конечную точку линейки.
        /// </summary>
        public Point EndPoint
        {
            get
            {
                return this.endPoint;
            }
            set
            {
                if (this.EndPoint != value)
                {
                    this.endPoint = value;

                    this.line.X2 = value.X;
                    this.line.Y2 = value.Y;

                    this.mapBindingService.SetMapObjectViewModelValue(this, nameof(this.EndPoint), value);

                    // Перерисовываем концы линейки.
                    this.RedrawCaps();
                }
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Перерисовывает концы линейки.
        /// </summary>
        private void RedrawCaps()
        {
            var a = this.StartPoint;
            var b = this.EndPoint;

            if (a != b)
            {
                // Длина линейки.
                var len = PointHelper.GetDistance(a, b);

                var dX = (a.Y - b.Y) / len;
                var dY = (b.X - a.X) / len;

                this.startLine.X1 = a.X + this.offset * dX;
                this.startLine.Y1 = a.Y + this.offset * dY;
                this.startLine.X2 = a.X - this.offset * dX;
                this.startLine.Y2 = a.Y - this.offset * dY;

                var temp = a;
                a = b;
                b = temp;

                dX = (a.Y - b.Y) / len;
                dY = (b.X - a.X) / len;

                this.endLine.X1 = a.X + this.offset * dX;
                this.endLine.Y1 = a.Y + this.offset * dY;
                this.endLine.X2 = a.X - this.offset * dX;
                this.endLine.Y2 = a.Y - this.offset * dY;
            }
        }

        #endregion
    }

    // Реализация IDrawableObject.
    internal sealed partial class Ruler
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        public void Draw(Point mousePrevPosition, Point mousePosition)
        {
            // Передвигаем последнюю точку.
            this.EndPoint = mousePosition;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        public bool IsDrawCompleted(Point mousePosition)
        {
            return true;
        }

        #endregion
    }

    // Реализация IMapObject.
    internal sealed partial class Ruler
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
            if (this.isOnCanvas)
                return false;

            this.Canvas = canvas;

            this.Canvas.Children.Add(this.startLine);
            this.Canvas.Children.Add(this.line);
            this.Canvas.Children.Add(this.endLine);

            this.isOnCanvas = true;

            return true;
        }

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public bool RemoveFromCanvas()
        {
            if (!this.isOnCanvas)
                return false;

            this.Canvas.Children.Remove(this.endLine);
            this.Canvas.Children.Remove(this.line);
            this.Canvas.Children.Remove(this.startLine);

            this.Canvas = null;

            this.isOnCanvas = false;

            return true;
        }

        #endregion
    }
}