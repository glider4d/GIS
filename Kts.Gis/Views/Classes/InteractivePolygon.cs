using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивный многоугольник.
    /// </summary>
    internal sealed partial class InteractivePolygon : InteractiveFigure
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что должны ли меняться вершины многоугольника при изменении его размера.
        /// </summary>
        private bool changePoints = true;

        /// <summary>
        /// Значение, указывающее на то, что отображен ли интерфейс редактирования многоугольника.
        /// </summary>
        private bool isUIVisible;

        /// <summary>
        /// Текстовое представление последовательности точек, из которых состоит многоугольник.
        /// </summary>
        private string points;

        /// <summary>
        /// Вершины многоугольника.
        /// </summary>
        private List<PolygonVertexThumb> vertices;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Полилайн, который представляет многоугольник.
        /// </summary>
        private readonly System.Windows.Shapes.Polyline polyline;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractivePolygon"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="angle">Угол поворота.</param>
        /// <param name="leftTopCorner">Положение левого верхнего угла.</param>
        /// <param name="points">Текстовое представление последовательности точек, из которых состоит многоугольник.</param>
        /// <param name="size">Размер.</param>
        /// <param name="fillBrush">Кисть фона.</param>
        /// <param name="strokePen">Ручка границы.</param>
        /// <param name="thickness">Толщина границы фигуры.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractivePolygon(PolygonViewModel viewModel, double angle, Point leftTopCorner, string points, Size size, Brush fillBrush, Pen strokePen, double thickness, IMapBindingService mapBindingService) : base(viewModel, new System.Windows.Shapes.Polyline(), fillBrush, strokePen, thickness, mapBindingService)
        {
            this.Angle = angle;
            this.LeftTopCorner = leftTopCorner;
            this.Points = points;
            this.Size = size;

            this.polyline = this.Shape as System.Windows.Shapes.Polyline;

            this.polyline.StrokeEndLineCap = PenLineCap.Round;
            this.polyline.StrokeStartLineCap = PenLineCap.Round;

            this.polyline.Loaded += this.Polyline_Loaded;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек, из которых состоит многоугольник.
        /// </summary>
        public string Points
        {
            get
            {
                return this.points;
            }
            set
            {
                if (this.Points != value)
                {
                    this.points = value;

                    if (this.polyline != null)
                        this.LoadPoints();

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.Points), value);
                }
            }
        }

        #endregion
        
        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> полилайна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Polyline_Loaded(object sender, RoutedEventArgs e)
        {
            this.polyline.Loaded -= this.Polyline_Loaded;

            this.LoadPoints();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Меняет координаты всех точек полилайна.
        /// </summary>
        /// <param name="deltaX">Изменение по X.</param>
        /// <param name="deltaY">Изменение по Y.</param>
        private void ChangePoints(double deltaX, double deltaY)
        {
            for (int i = 0; i < this.polyline.Points.Count; i++)
                this.polyline.Points[i] = new Point(this.polyline.Points[i].X * deltaX, this.polyline.Points[i].Y * deltaY);

            this.Points = this.polyline.Points.ToString();
        }

        /// <summary>
        /// Подгоняет границы фигуры к содержимому.
        /// </summary>
        private void FitBounds()
        {
            // Находим граничные координаты.
            double minX, minY, maxX, maxY;
            minX = minY = double.MaxValue;
            maxX = maxY = double.MinValue;
            for (int i = 0; i < this.polyline.Points.Count; i++)
            {
                if (this.polyline.Points[i].X < minX)
                    minX = this.polyline.Points[i].X;

                if (this.polyline.Points[i].X > maxX)
                    maxX = this.polyline.Points[i].X;

                if (this.polyline.Points[i].Y < minY)
                    minY = this.polyline.Points[i].Y;

                if (this.polyline.Points[i].Y > maxY)
                    maxY = this.polyline.Points[i].Y;
            }

            // Высчитываем новое положение многоугольника.
            this.LeftTopCorner = new Point(this.LeftTopCorner.X + minX, this.LeftTopCorner.Y + minY);

            // Сдвигаем все точки.
            for (int i = 0; i < this.polyline.Points.Count; i++)
                this.polyline.Points[i] = new Point(this.polyline.Points[i].X - minX, this.polyline.Points[i].Y - minY);

            // Меняем размер многоугольника.
            this.changePoints = false;
            this.Size = new Size(maxX + this.polyline.StrokeThickness, maxY + this.polyline.StrokeThickness);
            this.changePoints = true;
        }

        /// <summary>
        /// Возвращает представление заданной точки многоугольника во внешних координатах.
        /// </summary>
        /// <param name="point">Точка многоугольника.</param>
        /// <returns>Точка.</returns>
        private Point GetOuterPoint(Point point)
        {
            return this.GetRotatedPointPosition(new Point(point.X + this.LeftTopCorner.X, point.Y + this.LeftTopCorner.Y), false);
        }

        /// <summary>
        /// Загружает точки из строки.
        /// </summary>
        private void LoadPoints()
        {
            var showUI = this.isUIVisible;
            
            if (showUI)
                this.HideUI();

            if (!string.IsNullOrEmpty(this.Points))
            {
                // Если задана строка точек, то заполняем ими полилайн.
                this.polyline.Points.Clear();
                var ps = this.points.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] p;
                for (int i = 0; i < ps.Length; i++)
                {
                    p = ps[i].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    this.polyline.Points.Add(new Point(Convert.ToDouble(p[0]), Convert.ToDouble(p[1])));
                }
            }
            else
            {
                // Добавляем первые две точки в полилайн.
                this.polyline.Points.Add(new Point(0, 0));
                this.polyline.Points.Add(new Point(0, 0));
            }

            if (showUI)
                this.ShowUI();
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить вершину с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс удаляемой вершины.</param>
        /// <returns>true, если можно удалить, иначе - false.</returns>
        public bool CanDeleteVertex(int index)
        {
            if (index == 0 || index == this.polyline.Points.Count - 1)
                return false;

            return this.polyline.Points.Count > 4;
        }

        /// <summary>
        /// Меняет значение точки по заданному индексу.
        /// </summary>
        /// <param name="index">Индекс точки.</param>
        /// <param name="newPoint">Новое положение точки.</param>
        public void ChangePoint(int index, Point newPoint)
        {
            this.polyline.Points[index] = new Point(newPoint.X - this.LeftTopCorner.X, newPoint.Y - this.LeftTopCorner.Y);

            if (index == 0)
                // Также меняем последнюю точку.
                this.polyline.Points[this.polyline.Points.Count - 1] = new Point(this.polyline.Points[0].X, this.polyline.Points[0].Y);

            this.FitBounds();
        }

        /// <summary>
        /// Удаляет вершину с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс удаляемой вершины.</param>
        public void DeleteVertex(int index)
        {
            var showUI = this.isUIVisible;

            if (showUI)
                this.HideUI();

            var prevPoints = this.polyline.Points.ToString();

            // Удаляем заданную вершину.
            this.polyline.Points.RemoveAt(index);

            this.FitBounds();
            
            this.Points = this.polyline.Points.ToString();

            (this.ViewModel as PolygonViewModel).OnPointsChanged(prevPoints, this.LeftTopCorner, this.Size);

            if (showUI)
                this.ShowUI();
        }

        /// <summary>
        /// Возвращает точку по ее индексу.
        /// </summary>
        /// <param name="index">Индекс точки.</param>
        /// <returns>Точка.</returns>
        public Point GetPoint(int index)
        {
            return this.polyline.Points[index];
        }

        /// <summary>
        /// Вставляет вершину после вершины с заданным индексом, которая будет ее копией.
        /// </summary>
        /// <param name="index">Индекс дублируемой вершины.</param>
        public void InsertVertex(int index)
        {
            var showUI = this.isUIVisible;

            if (showUI)
                this.HideUI();

            var prevPoints = this.polyline.Points.ToString();

            this.polyline.Points.Insert(index, new Point(this.polyline.Points[index].X, this.polyline.Points[index].Y));

            this.Points = this.polyline.Points.ToString();

            (this.ViewModel as PolygonViewModel).OnPointsChanged(prevPoints, this.LeftTopCorner, this.Size);

            if (showUI)
                this.ShowUI();
        }

        /// <summary>
        /// Задает изменение структуры вершин многоугольника.
        /// </summary>
        /// <param name="prevPoints">Предыдущая структура вершин многоугольника.</param>
        /// <param name="prevPosition">Предыдущее положение многоугольника.</param>
        /// <param name="prevSize">Предыдущий размер многоугольника.</param>
        public void SetPointsChanged(string prevPoints, Point prevPosition, Size prevSize)
        {
            this.Points = this.polyline.Points.ToString();

            (this.ViewModel as PolygonViewModel).OnPointsChanged(prevPoints, prevPosition, prevSize);
        }

        #endregion
    }

    // Реализация InteractiveFigure.
    internal sealed partial class InteractivePolygon
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает угол поворота фигуры.
        /// </summary>
        public override double Angle
        {
            get
            {
                return 0;
            }
            set
            {
                // Ничего не делаем.
            }
        }

        /// <summary>
        /// Возвращает или задает размер многоугольника.
        /// </summary>
        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                if (base.Size != value)
                {
                    var oldSize = base.Size;

                    base.Size = value;

                    if (this.IsInitialized && this.changePoints)
                        // Меняем координаты всех точек полилайна.
                        this.ChangePoints(value.Width / oldSize.Width, value.Height / oldSize.Height);
                }
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Скрывает все вращалки и изменялки.
        /// </summary>
        protected override void CollapseAllThumbs()
        {
            base.CollapseAllThumbs();

            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    vertex.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Скрывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void HideUI()
        {
            base.HideUI();
            
            foreach (var vertex in this.vertices)
                this.Canvas.Children.Remove(vertex);

            this.vertices.Clear();

            this.vertices = null;

            this.isUIVisible = false;
        }

        /// <summary>
        /// Передвигает элементы управления свойствами фигуры.
        /// </summary>
        protected override void MoveUI()
        {
            if (this.vertices == null)
                return;

            foreach (var vertex in this.vertices)
                vertex.Position = this.GetRotatedPointPosition(new Point(this.LeftTopCorner.X + this.polyline.Points[vertex.Index].X, this.LeftTopCorner.Y + this.polyline.Points[vertex.Index].Y), false);
        }

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void ShowUI()
        {
            // Создаем вершины многоугольника и добавляем их на карту.
            this.vertices = new List<PolygonVertexThumb>();
            var index = 0;
            foreach (var point in this.polyline.Points)
            {
                // Не создаем последнюю вершину, так как она совпадает с самой первой.
                if (index == this.polyline.Points.Count - 1)
                    break;

                this.vertices.Add(new PolygonVertexThumb(this, new Point(this.LeftTopCorner.X + point.X, this.LeftTopCorner.Y + point.Y), Cursors.ScrollAll, index, this.MapBindingService.MapSettingService.PolygonVertexThumbSize, this.MapBindingService.MapSettingService.PolygonVertexThumbThickness));

                index++;
            }
            foreach (var vertex in this.vertices)
                this.Canvas.Children.Add(vertex);
            foreach (var vertex in this.vertices)
                vertex.BringToFront();

            this.isUIVisible = true;
        }

        #endregion

        #region Открытые переопределенные методы
        
        /// <summary>
        /// Скрывает все вращалки и изменялки, кроме заданного элемента.
        /// </summary>
        /// <param name="element">Элемент, который останется видимым.</param>
        public override void CollapseAllThumbsExceptThis(UIElement element)
        {
            base.CollapseAllThumbsExceptThis(element);

            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    if (vertex != element)
                        vertex.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        public override void Draw(Point mousePrevPosition, Point mousePosition)
        {
            if (this.IsInitialized)
                return;

            var newPosition = new Point(mousePosition.X - this.LeftTopCorner.X, mousePosition.Y - this.LeftTopCorner.Y);

            // Если точек в многоугольнике стало больше двух, то необходимо размещать новые точки под углами, кратными 45 градусам.
            if (this.polyline.Points.Count > 2 && !Keyboard.IsKeyDown(Key.LeftAlt))
            {
                // Составляем векторы из двух последних сторон многоугольника. Причем конец второй стороны будет находится на положении мыши.
                var pointCount = this.polyline.Points.Count;
                var centerPoint = this.polyline.Points[pointCount - 2];
                var secondSide = newPosition - centerPoint;
                var firstSide = this.polyline.Points[pointCount - 3] - centerPoint;

                // Получаем угол между этими двумя векторами.
                var angle = Vector.AngleBetween(firstSide, secondSide);
                
                // Высчитываем угол поворота.
                var rest = angle % 45;
                double angleRadian;
                if (rest > 22.5)
                    angleRadian = (45 - rest) * Math.PI / 180;
                else
                    angleRadian = -rest * Math.PI / 180;

                var newX = centerPoint.X + (newPosition.X - centerPoint.X) * Math.Cos(angleRadian) - (newPosition.Y - centerPoint.Y) * Math.Sin(angleRadian);
                var newY = centerPoint.Y + (newPosition.X - centerPoint.X) * Math.Sin(angleRadian) + (newPosition.Y - centerPoint.Y) * Math.Cos(angleRadian);

                newPosition = new Point(newX, newY);
            }

            // Передвигаем последнюю точку в полилайне.
            this.polyline.Points[this.polyline.Points.Count - 1] = newPosition;

            this.FitBounds();
        }

        /// <summary>
        /// Возвращает геометрию фигуры.
        /// </summary>
        /// <returns>Геометрия.</returns>
        public override Geometry GetGeometry()
        {
            var result = this.polyline.RenderedGeometry.Clone();

            // Добавляем трансформации.
            var transformGroup = new TransformGroup();
            var translateTransform = new TranslateTransform(this.LeftTopCorner.X, this.LeftTopCorner.Y);
            if (translateTransform.CanFreeze)
                translateTransform.Freeze();
            transformGroup.Children.Add(translateTransform);
            var rotateTransform = new RotateTransform(this.Angle, this.CenterPoint.X, this.CenterPoint.Y);
            if (rotateTransform.CanFreeze)
                rotateTransform.Freeze();
            transformGroup.Children.Add(rotateTransform);
            if (transformGroup.CanFreeze)
                transformGroup.Freeze();
            result.Transform = transformGroup;

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

            var points = this.polyline.Points.Clone();

            for (int i = 0; i < points.Count; i++)
                points[i] = this.GetOuterPoint(points[i]);

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
            // Получаем первую и последнюю точки в полилайне.
            var first = this.polyline.Points[0];
            var last = this.polyline.Points[this.polyline.Points.Count - 1];
            
            // Получаем расстояние между этими точками.
            var d = PointHelper.GetDistance(last, first);

            var result = d <= this.MapBindingService.MapSettingService.PolygonMaxPointDistance;

            if (result)
            {
                if (this.polyline.Points.Count == 2)
                    return false;

                // Если дальнейшее рисование не требуется, то тогда меняем последнюю точку полилайна, делая ее равной первой (типо замыкаем фигуру).
                this.polyline.Points[this.polyline.Points.Count - 1] = new Point(first.X, first.Y);

                // Запоминаем текстовое представление точек.
                this.Points = this.polyline.Points.ToString();
            }
            else
                // Иначе, добавляем новую точку в полилайн.
                this.polyline.Points.Add(new Point(mousePosition.X - this.LeftTopCorner.X, mousePosition.Y - this.LeftTopCorner.Y));

            return result;
        }

        /// <summary>
        /// Возвращает true, если заданная точка находится внутри фигуры. Иначе - false.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Значение, указывающее на то, что находится ли точка внутри фигуры.</returns>
        public override bool IsPointIn(Point point)
        {
            var points = this.polyline.Points.Clone();

            for (int i = 0; i < points.Count; i++)
                points[i] = this.GetOuterPoint(points[i]);

            // Сперва проверяем, находится ли точка рядом с границами многоугольника.
            for (int i = 0; i < points.Count - 1; i++)
                if (PointHelper.GetCDistance(points[i], points[i + 1], point) < 1)
                    return true;

            // Если нет, то проверяем, находится ли точка внутри многоугольника.
            int j, k;
            bool result = false;
            for (j = 0, k = points.Count - 1; j < points.Count; k = j++)
                if (((points[j].Y > point.Y) != (points[k].Y > point.Y)) && (point.X < (points[k].X - points[j].X) * (point.Y - points[j].Y) / (points[k].Y - points[j].Y) + points[j].X))
                    result = !result;
            return result;
        }

        /// <summary>
        /// Показывает все вращалки и изменялки.
        /// </summary>
        public override void ShowAllThumbs()
        {
            base.ShowAllThumbs();

            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    vertex.Visibility = Visibility.Visible;
        }

        #endregion
    }
}