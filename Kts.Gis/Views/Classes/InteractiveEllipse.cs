using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивный эллипс.
    /// </summary>
    internal sealed partial class InteractiveEllipse : InteractiveFigure
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveEllipse"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="angle">Угол поворота.</param>
        /// <param name="leftTopCorner">Положение левого верхнего угла.</param>
        /// <param name="size">Размер.</param>
        /// <param name="fillBrush">Кисть фона.</param>
        /// <param name="strokePen">Ручка границы.</param>
        /// <param name="thickness">Толщина границы фигуры.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveEllipse(EllipseViewModel viewModel, double angle, Point leftTopCorner, Size size, Brush fillBrush, Pen strokePen, double thickness, IMapBindingService mapBindingService) : base(viewModel, new Ellipse(), fillBrush, strokePen, thickness, mapBindingService)
        {
            this.Angle = angle;
            this.LeftTopCorner = leftTopCorner;
            this.Size = size;
        }

        #endregion
    }

    // Реализация InteractiveFigure.
    internal sealed partial class InteractiveEllipse
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Передвигает элементы управления свойствами фигуры.
        /// </summary>
        protected override void MoveUI()
        {
            this.RotateThumbs[0].Position = PointHelper.GetMidPoint(this.LeftTop, this.RightTop);
            this.RotateThumbs[1].Position = PointHelper.GetMidPoint(this.RightTop, this.RightBottom);
            this.RotateThumbs[2].Position = PointHelper.GetMidPoint(this.RightBottom, this.LeftBottom);
            this.RotateThumbs[3].Position = PointHelper.GetMidPoint(this.LeftBottom, this.LeftTop);

            this.ResizeThumbs[0].Position = PointHelper.GetMidPoint(this.LeftTop, this.RightTop);
            this.ResizeThumbs[1].Position = PointHelper.GetMidPoint(this.RightTop, this.RightBottom);
            this.ResizeThumbs[2].Position = PointHelper.GetMidPoint(this.RightBottom, this.LeftBottom);
            this.ResizeThumbs[3].Position = PointHelper.GetMidPoint(this.LeftBottom, this.LeftTop);

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
            this.RotateThumbs[0] = new FigureRotateThumb(this, PointHelper.GetMidPoint(this.LeftTop, this.RightTop), HorizontalAlignment.Left, VerticalAlignment.Top, thumbSize);
            this.RotateThumbs[1] = new FigureRotateThumb(this, PointHelper.GetMidPoint(this.RightTop, this.RightBottom), HorizontalAlignment.Right, VerticalAlignment.Top, thumbSize);
            this.RotateThumbs[2] = new FigureRotateThumb(this, PointHelper.GetMidPoint(this.RightBottom, this.LeftBottom), HorizontalAlignment.Right, VerticalAlignment.Bottom, thumbSize);
            this.RotateThumbs[3] = new FigureRotateThumb(this, PointHelper.GetMidPoint(this.LeftBottom, this.LeftTop), HorizontalAlignment.Left, VerticalAlignment.Bottom, thumbSize);
            thumbSize = this.MapBindingService.MapSettingService.FigureResizeThumbSize;
            var thumbThickness = this.MapBindingService.MapSettingService.FigureThumbThickness;
            this.ResizeThumbs = new EllipseResizeThumb[4];
            this.ResizeThumbs[0] = new EllipseResizeThumb(this, PointHelper.GetMidPoint(this.LeftTop, this.RightTop), Cursors.SizeNS, VerticalAlignment.Top, thumbSize, thumbThickness);
            this.ResizeThumbs[1] = new EllipseResizeThumb(this, PointHelper.GetMidPoint(this.RightTop, this.RightBottom), Cursors.SizeWE, HorizontalAlignment.Right, thumbSize, thumbThickness);
            this.ResizeThumbs[2] = new EllipseResizeThumb(this, PointHelper.GetMidPoint(this.RightBottom, this.LeftBottom), Cursors.SizeNS, VerticalAlignment.Bottom, thumbSize, thumbThickness);
            this.ResizeThumbs[3] = new EllipseResizeThumb(this, PointHelper.GetMidPoint(this.LeftBottom, this.LeftTop), Cursors.SizeWE, HorizontalAlignment.Left, thumbSize, thumbThickness);

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

            if (mousePrevPosition.X > mousePosition.X && mousePrevPosition.Y > mousePosition.Y)
                this.LeftTopCorner = new Point(mousePosition.X, mousePosition.Y);
            else
                if (mousePrevPosition.X > mousePosition.X)
                    this.LeftTopCorner = new Point(mousePosition.X, this.LeftTopCorner.Y);
                else
                    if (mousePrevPosition.Y > mousePosition.Y)
                        this.LeftTopCorner = new Point(this.LeftTopCorner.X, mousePosition.Y);

            this.Size = new Size(Math.Abs(mousePosition.X - mousePrevPosition.X), Math.Abs(mousePosition.Y - mousePrevPosition.Y));
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

            var result = new EllipseGeometry(this.CenterPoint, this.Size.Width / 2, this.Size.Height / 2, rotateTransform);

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
            return point;
        }
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        public override bool IsDrawCompleted(Point mousePosition)
        {
            return true;
        }

        /// <summary>
        /// Возвращает true, если заданная точка находится внутри фигуры. Иначе - false.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Значение, указывающее на то, что находится ли точка внутри фигуры.</returns>
        public override bool IsPointIn(Point point)
        {
            // Полуоси эллипса.
            double a = this.Size.Width / 2;
            double b = this.Size.Height / 2;

            // Если высота больше ширины, то меняем полуоси местами.
            if (this.Size.Height > this.Size.Width)
            {
                double t = a;
                a = b;
                b = t;
            }

            // Эксцентриситет.
            double e = Math.Sqrt(1 - b * b / (a * a));
            // Фокальное расстояние.
            double c = a * e;
            // Фокусы эллипса.
            var f1 = new Point(this.CenterPoint.X - c, this.CenterPoint.Y);
            var f2 = new Point(this.CenterPoint.X + c, this.CenterPoint.Y);

            // Если высота больше ширины, то меняем фокусы местами.
            if (this.Size.Height > this.Size.Width)
            {
                f1 = new Point(this.CenterPoint.X, this.CenterPoint.Y - c);
                f2 = new Point(this.CenterPoint.X, this.CenterPoint.Y + c);
            }

            var p = this.GetRotatedPointPosition(point, true);

            // Расстояния от точки до фокусов.
            double d1 = PointHelper.GetDistance(p, f1);
            double d2 = PointHelper.GetDistance(p, f2);

            if (d1 + d2 > 2 * a)
                return false;

            return true;
        }

        #endregion
    }
}