using Kts.Gis.Services;
using Kts.WpfUtilities;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет полилайн.
    /// </summary>
    internal sealed partial class Polyline : IDrawableObject, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Предыдущая последняя точка полилайна. Используется для определения завершенности рисования.
        /// </summary>
        private Point? prevLast;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Полилайн.
        /// </summary>
        private readonly System.Windows.Shapes.Polyline polyline = new System.Windows.Shapes.Polyline();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Polyline"/>.
        /// </summary>
        /// <param name="points">Коллекция точек, из которых состоит полилайн.</param>
        /// <param name="fillBrush">Кисть фона полилайна.</param>
        /// <param name="thickness">Толщина полилайна.</param>
        /// <param name="style">Стиль полилайна.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public Polyline(PointCollection points, SolidColorBrush fillBrush, double thickness, LineStyle style, IMapBindingService mapBindingService)
        {
            this.Points = points;
            this.polyline.Points = points;

            this.polyline.Stroke = fillBrush;
            this.polyline.StrokeThickness = thickness;
            this.mapBindingService = mapBindingService;

            this.SetStyle(style);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Вощвращает коллекцию точек, из которых состоит полилайн.
        /// </summary>
        public PointCollection Points
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает стиль полилайна.
        /// </summary>
        /// <param name="style">Стиль полилайна.</param>
        public void SetStyle(LineStyle style)
        {
            switch (style)
            {
                case LineStyle.Dotted:
                    this.polyline.StrokeDashArray = new DoubleCollection()
                    {
                        this.mapBindingService.MapSettingService.PolylinePlanningOffset
                    };

                    break;

                case LineStyle.Normal:
                    this.polyline.StrokeDashArray = null;

                    break;

                case LineStyle.SmallDotted:
                    this.polyline.StrokeDashArray = new DoubleCollection()
                    {
                        this.mapBindingService.MapSettingService.PolylineDisabledOffset
                    };

                    break;
            }
        }

        #endregion
    }

    // Реализация IDrawableObject.
    internal sealed partial class Polyline
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

                if (d <= this.mapBindingService.MapSettingService.PolylineMaxPointDistance)
                {
                    // Удаляем последнюю точку, так как она является дубликатом.
                    this.Points.RemoveAt(this.Points.Count - 1);

                    return true;
                }
            }

            var lastPoint = this.Points[this.Points.Count - 1];
            var penPoint = this.Points[this.Points.Count - 2];
            
            if (PointHelper.GetDistance(lastPoint, penPoint) < this.mapBindingService.MapSettingService.PolylineMinPointDistance)
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
    internal sealed partial class Polyline
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

            this.Canvas.Children.Add(this.polyline);

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

            this.Canvas.Children.Remove(this.polyline);

            this.Canvas = null;

            return true;
        }

        #endregion
    }
}