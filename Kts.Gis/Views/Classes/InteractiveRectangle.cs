using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивный прямоугольник.
    /// </summary>
    internal sealed partial class InteractiveRectangle : InteractiveFigure
    {
        #region Закрытые поля

        /// <summary>
        /// Первая точка.
        /// </summary>
        private Point? firstPoint;

        /// <summary>
        /// Вторая точка.
        /// </summary>
        private Point? secondPoint;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveRectangle"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="angle">Угол поворота.</param>
        /// <param name="leftTopCorner">Положение левого верхнего угла.</param>
        /// <param name="size">Размер.</param>
        /// <param name="fillBrush">Кисть фона.</param>
        /// <param name="strokePen">Ручка границы.</param>
        /// <param name="thickness">Толщина границы фигуры.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveRectangle(RectangleViewModel viewModel, double angle, Point leftTopCorner, Size size, Brush fillBrush, Pen strokePen, double thickness, IMapBindingService mapBindingService) : base(viewModel, new Rectangle(), fillBrush, strokePen, thickness, mapBindingService)
        {
            this.Angle = angle;
            this.LeftTopCorner = leftTopCorner;
            this.Size = size;
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Возвращает точки, составляющие вершины прямоугольника, начиная и заканчивая левой верхней.
        /// </summary>
        private List<Point> Points
        {
            get
            {
                return new List<Point>()
                {
                    this.LeftTop,
                    this.RightTop,
                    this.RightBottom,
                    this.LeftBottom,
                    this.LeftTop
                };
            }
        }

        #endregion
    }

    // Реализация InteractiveFigure.
    internal sealed partial class InteractiveRectangle
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Передвигает элементы управления свойствами фигуры.
        /// </summary>
        protected override void MoveUI()
        {
            this.RotateThumbs[0].Position = this.LeftTop;
            this.RotateThumbs[1].Position = this.RightTop;
            this.RotateThumbs[2].Position = this.RightBottom;
            this.RotateThumbs[3].Position = this.LeftBottom;

            this.ResizeThumbs[0].Position = this.LeftTop;
            this.ResizeThumbs[1].Position = this.RightTop;
            this.ResizeThumbs[2].Position = this.RightBottom;
            this.ResizeThumbs[3].Position = this.LeftBottom;

            // Задаем им угол вращения прямоугольника.
            for (int i = 0; i < this.ResizeThumbs.Length; i++)
                this.ResizeThumbs[i].Angle = this.Angle;
        }

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void ShowUI()
        {
            // Создаем вращалку и изменялки размеров прямоугольника.
            var thumbSize = this.MapBindingService.MapSettingService.FigureRotateThumbSize;
            this.RotateThumbs = new FigureRotateThumb[4];
            this.RotateThumbs[0] = new FigureRotateThumb(this, this.LeftTop, HorizontalAlignment.Left, VerticalAlignment.Top, thumbSize);
            this.RotateThumbs[1] = new FigureRotateThumb(this, this.RightTop, HorizontalAlignment.Right, VerticalAlignment.Top, thumbSize);
            this.RotateThumbs[2] = new FigureRotateThumb(this, this.RightBottom, HorizontalAlignment.Right, VerticalAlignment.Bottom, thumbSize);
            this.RotateThumbs[3] = new FigureRotateThumb(this, this.LeftBottom, HorizontalAlignment.Left, VerticalAlignment.Bottom, thumbSize);
            thumbSize = this.MapBindingService.MapSettingService.FigureResizeThumbSize;
            var thumbThickness = this.MapBindingService.MapSettingService.FigureThumbThickness;
            this.ResizeThumbs = new RectangleResizeThumb[4];
            this.ResizeThumbs[0] = new RectangleResizeThumb(this, this.LeftTop, HorizontalAlignment.Left, VerticalAlignment.Top, Cursors.SizeNWSE, thumbSize, thumbThickness);
            this.ResizeThumbs[1] = new RectangleResizeThumb(this, this.RightTop, HorizontalAlignment.Right, VerticalAlignment.Top, Cursors.SizeNESW, thumbSize, thumbThickness);
            this.ResizeThumbs[2] = new RectangleResizeThumb(this, this.RightBottom, HorizontalAlignment.Right, VerticalAlignment.Bottom, Cursors.SizeNWSE, thumbSize, thumbThickness);
            this.ResizeThumbs[3] = new RectangleResizeThumb(this, this.LeftBottom, HorizontalAlignment.Left, VerticalAlignment.Bottom, Cursors.SizeNESW, thumbSize, thumbThickness);

            // Задаем изменялкам угол поворота прямоугольника.
            for (int i = 0; i < this.ResizeThumbs.Length; i++)
                this.ResizeThumbs[i].Angle = this.Angle;

            // Добавляем их на холст.
            for (int i = 0; i < this.RotateThumbs.Length; i++)
                this.Canvas.Children.Add(this.RotateThumbs[i]);
            for (int i = 0; i < this.ResizeThumbs.Length; i++)
                this.Canvas.Children.Add(this.ResizeThumbs[i]);

            // Выводим их на передний план.
            for (int i = 0; i < this.RotateThumbs.Length; i++)
                this.RotateThumbs[i].BringToFront();
            for (int i = 0; i < this.ResizeThumbs.Length; i++)
                this.ResizeThumbs[i].BringToFront();
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        public override void Draw(Point mousePrevPosition, Point mousePosition)
        {
            if (this.IsInitialized)
                return;

            // Проверяем наличие второй точки прямоугольника.
            if (!this.secondPoint.HasValue)
            {
                // Если ее нет, то мы должны рисовать ту часть прямоугольника, которая представляет ее верхнюю сторону.
                if (!this.firstPoint.HasValue)
                {
                    this.Size = new Size(0, this.Shape.StrokeThickness);

                    this.firstPoint = mousePosition;
                }

                // Определяем угол, на который повернут рисуемый прямоугольник.
                var a = new Vector(1, 0);
                var b = new Vector(mousePosition.X - this.firstPoint.Value.X, this.firstPoint.Value.Y - mousePosition.Y);
                this.Angle = -Vector.AngleBetween(a, b);

                this.Size = new Size(PointHelper.GetDistance(mousePosition, this.firstPoint.Value), this.Size.Height);
            }
            else
            {
                if (!this.firstPoint.HasValue)
                    // В данном случае, пользователь при рисовании дважды щелкнул мышью, тем самым не был обработан верхний блок метода. В данном случае нужно просто выйти из метода.
                    return;
                    
                var lt = this.firstPoint.Value;
                var rt = this.secondPoint.Value;

                var top = new Vector(rt.X - lt.X, rt.Y - lt.Y);
                var middle = new Vector(mousePosition.X - lt.X, mousePosition.Y - lt.Y);
                // Перпендикулярный первому вектору вектор.
                var bottom = new Vector(top.Y, -top.X);

                if (Math.Abs(Vector.AngleBetween(top, middle)) > 90)
                    return;

                // Находим угол между диагональю прямоугольника и его верхней стороной.
                var a = new Vector(this.secondPoint.Value.X - this.firstPoint.Value.X, this.secondPoint.Value.Y - this.firstPoint.Value.Y);
                var b = new Vector(mousePosition.X - this.firstPoint.Value.X, mousePosition.Y - this.firstPoint.Value.Y);
                var angle = Vector.AngleBetween(a, b) * Math.PI / 180;

                // Получаем длину диагонали.
                var length = PointHelper.GetDistance(this.firstPoint.Value, mousePosition);

                // Получаем новые ширину и высоту.
                double width = Math.Abs(length * Math.Cos(angle));
                double height = Math.Abs(length * Math.Sin(angle));

                // Принимаем в расчет то, что ширина и высота не должны быть меньше толщины границы прямоугольника.
                width = width < this.Shape.StrokeThickness ? this.Shape.StrokeThickness : width;
                height = height < this.Shape.StrokeThickness ? this.Shape.StrokeThickness : height;

                if (180 - Math.Abs(Vector.AngleBetween(middle, bottom)) > 90)
                {
                    this.LeftTopCorner = this.secondPoint.Value;

                    this.Angle = 180 + this.Angle;

                    var oldPoint = this.firstPoint;

                    this.firstPoint = secondPoint;

                    this.secondPoint = oldPoint;
                }

                this.Size = new Size(width, height);
            }
        }

        /// <summary>
        /// Возвращает геометрию фигуры.
        /// </summary>
        /// <returns>Геометрия.</returns>
        public override Geometry GetGeometry()
        {
            var rotateTransform = new RotateTransform(this.Angle, this.CenterPoint.X, this.CenterPoint.Y);

            if (rotateTransform.CanFreeze)
                rotateTransform.Freeze();

            var result = new RectangleGeometry(new Rect(this.LeftTopCorner, this.Size), 0, 0, rotateTransform);

            if (result.CanFreeze)
                result.Freeze();

            return result;
        }

        /// <summary>
        /// Возвращает ближайшую к границе фигуры от заданной точки точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Точка на границе фигуры.</returns>
        public override Point GetNearestPoint(Point point)
        {
            var result = new Point();

            var points = this.Points;

            var minDistance = double.MaxValue;

            for (int i = 0; i < points.Count - 1; i++)
            {
                var p = PointHelper.GetNearestPoint(points[i], points[i + 1], point);

                var d = PointHelper.GetDistance(p, point);

                // Необходимо найти такую точку, которая будет наиболее ближе к заданной точке.
                if (d < minDistance)
                {
                    minDistance = d;

                    result = p;
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        public override bool IsDrawCompleted(Point mousePosition)
        {
            bool result = this.secondPoint.HasValue;

            if (result)
            {
                // Получаем разности координат прямоугольника.
                var deltaX = this.LeftTop.X - this.LeftTopCorner.X;
                var deltaY = this.LeftTop.Y - this.LeftTopCorner.Y;

                // Если рисование прямоугольника завершено, то указываем ему центральную точку трансформации.
                this.TransformOrigin = new Point(0.5, 0.5);

                // Смещаем положение левого верхнего угла прямоугольника, чтобы при изменении центра поворота прямоугольник остался на том же месте.
                this.LeftTopCorner = new Point(this.LeftTopCorner.X - deltaX, this.LeftTopCorner.Y - deltaY);
            }
            else
                // Задаем вторую точку прямоугольника.
                this.secondPoint = mousePosition;

            return result;
        }

        /// <summary>
        /// Возвращает true, если заданная точка находится внутри фигуры. Иначе - false.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Значение, указывающее на то, что находится ли точка внутри фигуры.</returns>
        public override bool IsPointIn(Point point)
        {
            var points = this.Points;

            // Сперва проверяем, находится ли точка рядом с границами прямоугольника.
            for (int i = 0; i < points.Count - 1; i++)
                if (PointHelper.GetCDistance(points[i], points[i + 1], point) < 1)
                    return true;

            // Если нет, то проверяем, находится ли точка внутри прямоугольника.
            int j, k;
            bool result = false;
            for (j = 0, k = points.Count - 1; j < points.Count; k = j++)
                if (((points[j].Y > point.Y) != (points[k].Y > point.Y)) && (point.X < (points[k].X - points[j].X) * (point.Y - points[j].Y) / (points[k].Y - points[j].Y) + points[j].X))
                    result = !result;
            return result;
        }

        #endregion
    }
}