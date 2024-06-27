using Kts.Gis.Services;
using Kts.WpfUtilities;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет новую линейку, которая тупо показывает отмеченное ей расстояние.
    /// </summary>
    internal sealed partial class NewRuler : IDrawableObject, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что добавлена ли линейка на холст.
        /// </summary>
        private bool isOnCanvas;

        /// <summary>
        /// Полуотступ линии, которая находится на конце линейки.
        /// </summary>
        private double offset;

        /// <summary>
        /// Предыдущая последняя точка полилайна. Используется для определения завершенности рисования.
        /// </summary>
        private Point? prevLast;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Линия, которая находится на конце линейки.
        /// </summary>
        private readonly Line endLine = new Line();

        /// <summary>
        /// Полилайн, представляющий линейку.
        /// </summary>
        private readonly System.Windows.Shapes.Polyline line = new System.Windows.Shapes.Polyline();

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
        /// Инициализирует новый экземпляр класса <see cref="NewRuler"/>.
        /// </summary>
        /// <param name="points">Коллекция точек, из которых состоит линейка.</param>
        /// <param name="thickness">Толщина линий.</param>
        /// <param name="offset">Полуотступ линии, которая находится на конце линейки.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public NewRuler(PointCollection points, double thickness, double offset, IMapBindingService mapBindingService)
        {
            this.Points = points;
            this.offset = offset;
            this.mapBindingService = mapBindingService;

            this.line.Points = points;

            this.line.Stroke = strokeBrush;
            this.line.StrokeThickness = thickness;

            this.startLine.Stroke = strokeBrush;
            this.startLine.StrokeThickness = thickness;
            this.endLine.Stroke = strokeBrush;
            this.endLine.StrokeThickness = thickness;
        }

        #endregion

        #region Статические конструкторы

        /// <summary>
        /// Инициализирует статические члены класса <see cref="NewRuler"/>.
        /// </summary>
        static NewRuler()
        {
            if (strokeBrush.CanFreeze)
                strokeBrush.Freeze();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Вощвращает коллекцию точек, из которых состоит линейка.
        /// </summary>
        public PointCollection Points
        {
            get;
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Перерисовывает заданный конец линейки.
        /// </summary>
        /// <param name="line">Линия, представляющая конец линейки.</param>
        /// <param name="startPoint">Начальная точка отрезка, на котором расположен конец линейки.</param>
        /// <param name="endPoint">Конечная точка отрезка, на котором расположен конец линейки.</param>
        private void RedrawCap(Line line, Point startPoint, Point endPoint)
        {
            var a = startPoint;
            var b = endPoint;

            if (a != b)
            {
                var len = PointHelper.GetDistance(a, b);

                var dX = (a.Y - b.Y) / len;
                var dY = (b.X - a.X) / len;

                line.X1 = a.X + this.offset * dX;
                line.Y1 = a.Y + this.offset * dY;
                line.X2 = a.X - this.offset * dX;
                line.Y2 = a.Y - this.offset * dY;
            }
        }

        /// <summary>
        /// Перерисовывает концы линейки.
        /// </summary>
        private void RedrawCaps()
        {
            this.RedrawCap(this.startLine, this.Points[0], this.Points[1]);
            this.RedrawCap(this.endLine, this.Points[this.Points.Count - 1], this.Points[this.Points.Count - 2]);
        }

        #endregion
    }

    // Реализация IDrawableObject.
    internal sealed partial class NewRuler
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
            this.Points[this.Points.Count - 1] = new Point(mousePosition.X, mousePosition.Y);

            this.RedrawCaps();

            this.mapBindingService.NotifyMapObjectViewModel(this, nameof(this.Points));
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        public bool IsDrawCompleted(Point mousePosition)
        {
            if (this.prevLast != null)
            {
                // Вычисляем расстояние от текущего положения мыши до предыдущей последней точки полилайна.
                double d = PointHelper.GetDistance(mousePosition, prevLast.Value);

                if (d <= this.mapBindingService.MapSettingService.NewRulerMaxPointDistance)
                {
                    // Удаляем последнюю точку, так как она является дубликатом.
                    this.Points.RemoveAt(this.Points.Count - 1);

                    return true;
                }
            }

            var lastPoint = this.Points[this.Points.Count - 1];
            var penPoint = this.Points[this.Points.Count - 2];

            if (penPoint.X == lastPoint.X && penPoint.Y == lastPoint.Y)
                // Если последние две точки полилайна равны, то не фиксируем последнюю точку.
                return false;

            // Задаем предыдущую последнюю точку.
            this.prevLast = this.Points[this.Points.Count - 1];

            // Добавляем новую точку.
            this.Points.Add(new Point(mousePosition.X, mousePosition.Y));

            return false;
        }

        #endregion
    }

    // Реализация IMapObject.
    internal sealed partial class NewRuler
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