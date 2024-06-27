using Kts.Gis.Services;
using Kts.WpfUtilities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет умную надпись, которая автоматически определяет свое положение относительно родителя-фигуры.
    /// </summary>
    internal sealed partial class SmartFigureLabel : SmartLabel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что анимирована ли фигура-родитель надписи.
        /// </summary>
        private bool isFigureAnimated;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Родитель-фигура.
        /// </summary>
        private readonly InteractiveFigure figure;

        /// <summary>
        /// Сервис настроек вида карты.
        /// </summary>
        private readonly IMapSettingService mapSettingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SmartFigureLabel"/>.
        /// </summary>
        /// <param name="figure">Родитель-фигура.</param>
        /// <param name="savedAngle">Сохраненный угол поворота надписи.</param>
        /// <param name="savedPosition">Сохраненное положение надписи.</param>
        /// <param name="savedSize">Сохраненный размер надписи.</param>
        /// <param name="mapSettingService">Сервис настроек вида карты.</param>
        public SmartFigureLabel(InteractiveFigure figure, double? savedAngle, Point? savedPosition, int? savedSize, IMapSettingService mapSettingService) : base(savedAngle, savedPosition, savedSize, mapSettingService.FigureLabelDefaultSize, false)
        {
            this.figure = figure;
            this.mapSettingService = mapSettingService;
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
            if (this.ContextMenu != null)
            {
                this.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.ContextMenu = null;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает точку из заданного списка с минимальным значением координаты по Y.
        /// </summary>
        /// <param name="points">Список точек.</param>
        /// <returns>Точка с минимальным значением координаты по Y.</returns>
        private Point GetMinYPoint(List<Point> points)
        {
            var min = points[0];

            for (int i = 1; i < points.Count; i++)
                if (points[i].Y < min.Y)
                    min = points[i];

            return min;
        }

        /// <summary>
        /// Возвращает новое положение надписи между двумя заданными точками.
        /// </summary>
        /// <param name="x">Первая точка.</param>
        /// <param name="y">Вторая точка.</param>
        /// <returns>Новое положение.</returns>
        private Point GetNewLeftTopCorner(Point x, Point y)
        {
            Point a, b, c, d;
            double len, cX, coeff;

            c = new Point();
            d = new Point();
            len = cX = coeff = 0;

            var parent = this.figure;

            var offset = this.DesiredSize.Height + 3;
            
            // Эту систему выборов нужно будет переделать.
            if (x == parent.LeftTop && y == parent.RightTop || x == parent.RightTop && y == parent.LeftTop)
            {
                a = parent.LeftTopCorner;
                b = new Point(parent.LeftTopCorner.X + parent.Size.Width, parent.LeftTopCorner.Y);

                c = new Point(a.X, a.Y - offset);
                d = new Point(b.X, b.Y - offset);

                len = PointHelper.GetDistance(c, d);
                cX = len / 2 + this.DesiredSize.Width / 2;
                coeff = (len - cX) / cX;
            }
            else
                if (x == parent.RightTop && y == parent.RightBottom || x == parent.RightBottom && y == parent.RightTop)
                {
                    a = new Point(parent.LeftTopCorner.X + parent.Size.Width, parent.LeftTopCorner.Y);
                    b = new Point(parent.LeftTopCorner.X + parent.Size.Width, parent.LeftTopCorner.Y + parent.Size.Height);

                    c = new Point(a.X + offset, a.Y);
                    d = new Point(b.X + offset, b.Y);

                    len = PointHelper.GetDistance(c, d);
                    cX = len / 2 + this.DesiredSize.Width / 2;
                    coeff = (len - cX) / cX;
                }
                else
                    if (x == parent.LeftBottom && y == parent.RightBottom || x == parent.RightBottom && y == parent.LeftBottom)
                    {
                        a = new Point(parent.LeftTopCorner.X, parent.LeftTopCorner.Y + parent.Size.Height);
                        b = new Point(parent.LeftTopCorner.X + parent.Size.Width, parent.LeftTopCorner.Y + parent.Size.Height);

                        c = new Point(a.X, a.Y + offset);
                        d = new Point(b.X, b.Y + offset);

                        len = PointHelper.GetDistance(c, d);
                        cX = len / 2 + this.DesiredSize.Width / 2;
                        coeff = cX / (len - cX);
                    }
                    else
                        if (x == parent.LeftTop && y == parent.LeftBottom || x == parent.LeftBottom && y == parent.LeftTop)
                        {
                            a = parent.LeftTopCorner;
                            b = new Point(parent.LeftTopCorner.X, parent.LeftTopCorner.Y + parent.Size.Height);

                            c = new Point(a.X - offset, a.Y);
                            d = new Point(b.X - offset, b.Y);

                            len = PointHelper.GetDistance(c, d);
                            cX = len / 2 + this.DesiredSize.Width / 2;
                            coeff = cX / (len - cX);
                        }

            var point = new Point((c.X + coeff * d.X) / (1 + coeff), (c.Y + coeff * d.Y) / (1 + coeff));

            return parent.GetRotatedPointPosition(point, false);
        }

        #endregion
    }
	
	// Реализация SmartLabel.
    internal sealed partial class SmartFigureLabel
	{
        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает размер надписи по умолчанию.
        /// </summary>
        public override int DefaultSize
        {
            get
            {
                return this.mapSettingService.FigureLabelDefaultSize;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Выполняется при требовании контекстного меню.
        /// </summary>
        protected override void OnContextMenuRequested()
        {
            this.ContextMenu = Application.Current.Resources["FigureLabelContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.ContextMenu.DataContext = null;
            this.ContextMenu.DataContext = this.figure.ViewModel;

            this.ContextMenu.Closed += this.ContextMenu_Closed;

            this.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняется при нажатии левой кнопки мыши по надписи.
        /// </summary>
        protected override void OnMouseClick()
        {
            if (this.isFigureAnimated)
                return;

            this.isFigureAnimated = true;

            this.figure.StartAnimation();
        }

        /// <summary>
        /// Выполняет действия, связанные с выходом курсора мыши с надписи.
        /// </summary>
        protected override void OnMouseLeaved()
        {
            if (!this.isFigureAnimated)
                return;

            this.isFigureAnimated = false;

            this.figure.StopAnimation();
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением положения надписи.
        /// </summary>
        /// <param name="prevPosition">Предыдущее положение надписи.</param>
        protected override void OnPositionChanged(Point prevPosition)
        {
            if (!this.HasManualPosition)
                this.InitialPosition = prevPosition;

            this.figure.SetLabelMoved(prevPosition);
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением размера надписи.
        /// </summary>
        protected override void OnSizeChanged()
        {
            this.figure.SetLabelSizeChanged();
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи.
        /// </summary>
        /// <param name="prevAngle">Предыдущий угол поворота надписи.</param>
        public override void OnAngleChanged(double prevAngle)
        {
            if (!this.HasManualAngle)
                this.InitialAngle = prevAngle;

            this.figure.SetLabelAngleChanged(prevAngle);
        }

        /// <summary>
        /// Переопределяет положение надписи.
        /// </summary>
        /// <param name="positionDelta">Разница между текущим и предыдущим положением фигуры.</param>
        public override void Relocate(Point positionDelta)
        {
            // Находим три самые верхние вершины.
            var points = new List<Point>();
            points.Add(this.figure.LeftTop);
            points.Add(this.figure.RightTop);
            points.Add(this.figure.RightBottom);
            points.Add(this.figure.LeftBottom);
            var a = this.GetMinYPoint(points);
            points.Remove(a);
            var b = this.GetMinYPoint(points);

            if (b.X < a.X)
            {
                // Меняем вершины местами, чтобы точка a была левее точки b.
                var с = a;
                a = b;
                b = с;
            }

            if (this.IsReady && (this.HasManualAngle || this.HasManualPosition))
            {
                this.HasManualAngle = false;
                this.HasManualPosition = false;

                this.figure.OnLabelChanged(null, null, this.Size);
            }

            if (!this.HasManualAngle)
            {
                // Задаем угол поворота надписи.
                var c = new Vector(1, 0);
                var d = new Vector(b.X - a.X, b.Y - a.Y);
                var rotateTransform = this.RenderTransform as RotateTransform;
                if (rotateTransform == null)
                {
                    rotateTransform = new RotateTransform(0);

                    this.RenderTransform = rotateTransform;
                }
                rotateTransform.Angle = Vector.AngleBetween(c, d);
            }

            if (!this.HasManualPosition)
                this.LeftTopCorner = this.GetNewLeftTopCorner(a, b);
        }

        #endregion
    }
}