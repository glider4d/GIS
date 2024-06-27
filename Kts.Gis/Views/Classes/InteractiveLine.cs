using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивную линию.
    [Serializable]
    internal sealed partial class InteractiveLine : InteractiveShape, IDrawableObject
    {
        #region Закрытые константы

        /// <summary>
        /// Разделитель текста надписей линии.
        /// </summary>
        private const string labelDiv = "; ";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что следует ли отображать надписи линии.
        /// </summary>
        private bool canShowLabels;

        /// <summary>
        /// Кисть фона линии при ее выделении.
        /// </summary>
        private Pen fillPen;

        /// <summary>
        /// Значение, указывающее на то, что скрыты ли элементы линии.
        /// </summary>
        private bool isUIhidden;

        /// <summary>
        /// Значение, указывающее на то, что отображен ли интерфейс редактирования линии.
        /// </summary>
        private bool isUIVisible;

        /// <summary>
        /// Левая точка линии.
        /// </summary>
        private Point leftPoint;

        /// <summary>
        /// Текущий стиль линии.
        /// </summary>
        private LineStyle lineStyle;

        /// <summary>
        /// Текстовое представление последовательности точек изгиба линии.
        /// </summary>
        private string points;

        /// <summary>
        /// Предыдущие точки изгиба линии.
        /// </summary>
        private string prevBendPoints;

        /// <summary>
        /// Предыдущие точки линии.
        /// </summary>
        private Tuple<Point, Point> prevPoints;

        /// <summary>
        /// Правая точка линии.
        /// </summary>
        private Point rightPoint;

        /// <summary>
        /// Изменялки точек изгиба линии.
        /// </summary>
        private List<LinePointThumb> vertices;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Полилайн.
        /// </summary>
        private readonly System.Windows.Shapes.Polyline polyline;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveLine"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="leftPoint">Левая точка линии.</param>
        /// <param name="rightPoint">Правая точка линии.</param>
        /// <param name="points">Текстовое представление последовательности точек изгиба линии.</param>
        /// <param name="lineStyle">Стиль линии.</param>
        /// <param name="lineThickness">Толщина линии.</param>
        /// <param name="fillBrush">Кисть фона линии.</param>
        /// <param name="fillPen">Кисть фона линии при ее выделении.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveLine(LineViewModel viewModel, Point leftPoint, Point rightPoint, string points, LineStyle lineStyle, double lineThickness, SolidColorBrush fillBrush, Pen fillPen, IMapBindingService mapBindingService) : base(viewModel, new System.Windows.Shapes.Polyline(), mapBindingService)
        {
            this.LeftPoint = leftPoint;
            this.RightPoint = rightPoint;
            this.Points = points;

            this.polyline = this.Shape as System.Windows.Shapes.Polyline;
            
            this.SetLineStyle(lineStyle);

            this.polyline.Stroke = fillBrush;

            this.polyline.StrokeThickness = lineThickness;

            this.fillPen = fillPen;

            // Создаем нужное количество надписей.
            double? labelAngle;
            Point? labelPosition;
            int? labelSize;
            for (int i = 0; i < this.GetPointCount() - 1; i++)
            {
                labelAngle = null;
                labelPosition = null;
                labelSize = null;

                if (viewModel.LabelAngles.ContainsKey(i))
                    labelAngle = viewModel.LabelAngles[i];

                if (viewModel.LabelPositions.ContainsKey(i))
                    labelPosition = new Point(viewModel.LabelPositions[i].X, viewModel.LabelPositions[i].Y);

                if (viewModel.LabelSizes.ContainsKey(i))
                    labelSize = viewModel.LabelSizes[i];

                this.Labels.Add(new SmartLineLabel(this, labelAngle, labelPosition, labelSize, viewModel.LabelOffset, mapBindingService.MapSettingService));
            }

            this.polyline.Loaded += this.Polyline_Loaded;

            this.CanShowLabels = viewModel.ShowLabels;

            // Ускоряем отображение подсказок.
            ToolTipService.SetInitialShowDelay(this.polyline, 100);

            this.polyline.ToolTip = "";
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значки линии.
        /// </summary>
        public List<InteractiveBadge> Badges
        {
            get;
        } = new List<InteractiveBadge>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что следует ли отображать надписи линии.
        /// </summary>
        public bool CanShowLabels
        {
            get
            {
                return this.canShowLabels;
            }
            set
            {
                this.canShowLabels = value;

                if (value)
                    this.ShowLabels();
                else
                    this.HideLabels();
            }
        }

        /// <summary>
        /// Возвращает или задает положение контекстного меню.
        /// </summary>
        public Point ContextMenuPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает набор надписей линии.
        /// </summary>
        public List<SmartLineLabel> Labels
        {
            get;
        } = new List<SmartLineLabel>();

        /// <summary>
        /// Возвращает или задает левую точку линии.
        /// </summary>
        public Point LeftPoint
        {
            get
            {
                return this.leftPoint;
            }
            set
            {
                if (this.LeftPoint != value)
                {
                    var prevValue = this.LeftPoint;

                    this.leftPoint = value;

                    if (this.polyline != null && this.polyline.Points.Count > 0)
                        this.polyline.Points[0] = new Point(this.LeftPoint.X, this.LeftPoint.Y);

                    if (this.IsInitialized)
                    {
                        // Обновляем тексты надписей.
                        this.UpdateLabels();

                        // Передвигаем надписи.
                        foreach (var label in this.Labels)
                            label.Relocate(new Point(value.X - prevValue.X, value.Y - prevValue.Y));

                        // Передвигаем значки.
                        this.RelocateBadges();
                    }

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.LeftPoint), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек изгиба линии.
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

                    if (this.IsInitialized)
                        this.LoadPoints();

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.Points), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает правую точку линии.
        /// </summary>
        public Point RightPoint
        {
            get
            {
                return this.rightPoint;
            }
            set
            {
                if (this.RightPoint != value)
                {
                    var prevValue = this.RightPoint;

                    this.rightPoint = value;

                    if (this.polyline != null && this.polyline.Points.Count > 0)
                        this.polyline.Points[this.polyline.Points.Count - 1] = new Point(this.RightPoint.X, this.RightPoint.Y);

                    if (this.IsInitialized)
                    {
                        // Обновляем тексты надписей.
                        this.UpdateLabels();

                        // Передвигаем надписи.
                        foreach (var label in this.Labels)
                            label.Relocate(new Point(value.X - prevValue.X, value.Y - prevValue.Y));

                        // Передвигаем значки.
                        this.RelocateBadges();
                    }

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.RightPoint), value);
                }
            }
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
            if (this.polyline.ContextMenu != null)
            {
                this.polyline.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.polyline.ContextMenu = null;
            }
        }

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

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.ToolTipOpening"/> полилайна.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Polyline_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var viewModel = this.ViewModel as LineViewModel;

            // Выдираем протяженность ближайшего участка линии.
            var label = this.GetNearLabel(this.Canvas.Map.MousePosition);
            var parts = label.Text.Split(new string[1]
            {
                labelDiv
            }, StringSplitOptions.RemoveEmptyEntries);
            var length = parts.Length > 0 ? parts[0] : "-";

            this.polyline.ToolTip = "Протяженность: " + length + Environment.NewLine + "Общая протяженность: " + Math.Round(viewModel.Length, 2).ToString() + Environment.NewLine + "Диаметр: " + viewModel.Diameter;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает строковое представление точек изгиба линии.
        /// </summary>
        /// <returns>Строковое представление точек изгиба линии.</returns>
        private string GetBendPoints()
        {
            var list = new List<Point>();

            for (int i = 1; i < this.polyline.Points.Count - 1; i++)
                list.Add(this.polyline.Points[i]);

            return new PointCollection(list).ToString();
        }

        /// <summary>
        /// Возвращает надпись линии, которая наиболее близка к заданной точке.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Надпись линии.</returns>
        private SmartLineLabel GetNearLabel(Point point)
        {
            var index = 0;
            var curDistance = 0.0;
            var minDistance = double.MaxValue;

            for (int i = 1; i < this.polyline.Points.Count; i++)
            {
                curDistance = PointHelper.GetCDistance(this.polyline.Points[i - 1], this.polyline.Points[i], point);

                if (curDistance < minDistance)
                {
                    minDistance = curDistance;

                    index = i - 1;
                }
            }

            return this.Labels[index];
        }

        /// <summary>
        /// Возвращает количество точек в линии.
        /// </summary>
        /// <returns>Количество точек.</returns>
        private int GetPointCount()
        {
            if (!string.IsNullOrEmpty(this.Points))
                return this.Points.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries).Length + 2;

            return 2;
        }

        /// <summary>
        /// Загружает точки из строки.
        /// </summary>
        private void LoadPoints()
        {
            var showUI = this.isUIVisible;

            if (showUI)
                this.HideUI();

            this.polyline.Points.Clear();

            this.polyline.Points.Add(new Point(this.LeftPoint.X, this.LeftPoint.Y));

            if (!string.IsNullOrEmpty(this.Points))
            {
                // Если задана строка точек изгиба, то добавляем их в полилайн.
                var ps = this.Points.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] p;
                for (int i = 0; i < ps.Length; i++)
                {
                    p = ps[i].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    this.polyline.Points.Add(new Point(Convert.ToDouble(p[0]), Convert.ToDouble(p[1])));
                }
            }

            this.polyline.Points.Add(new Point(this.RightPoint.X, this.RightPoint.Y));

            // Проверяем, нужно ли удалить или добавить новую надпись.
            var pointCount = this.polyline.Points.Count;
            var curCount = this.Labels.Count;
            var viewModel = this.ViewModel as LineViewModel;
            if (curCount < pointCount - 1)
            {
                double? labelAngle;
                Point? labelPosition;
                int? labelSize;
                
                // Добавляем недостающие надписи.
                for (int i = 0; i < pointCount - curCount - 1; i++)
                {
                    labelAngle = null;
                    labelPosition = null;
                    labelSize = null;

                    if (viewModel.LabelAngles.ContainsKey(curCount + i))
                        labelAngle = viewModel.LabelAngles[curCount + i];

                    if (viewModel.LabelPositions.ContainsKey(curCount + i))
                        labelPosition = new Point(viewModel.LabelPositions[curCount + i].X, viewModel.LabelPositions[curCount + i].Y);

                    if (viewModel.LabelSizes.ContainsKey(curCount + i))
                        labelSize = viewModel.LabelSizes[curCount + i];
                        
                    var newLabel = new SmartLineLabel(this, labelAngle, labelPosition, labelSize, viewModel.LabelOffset, this.MapBindingService.MapSettingService);

                    this.Labels.Add(newLabel);

                    if (viewModel.IsPlaced)
                    {
                        newLabel.Canvas = this.Labels[0].Canvas;

                        newLabel.AddToCanvas(newLabel.Canvas);
                    }
                }
            }
            else
                if (curCount >= pointCount)
                    // Удаляем лишние надписи.
                    for (int i = 0; i < curCount - pointCount + 1; i++)
                    {
                        var remLabel = this.Labels[this.Labels.Count - 1];

                        remLabel.RemoveFromCanvas();

                        this.Labels.Remove(remLabel);
                    }

            // Обновляем тексты надписей.
            this.UpdateLabels();

            // И передвигаем их.
            foreach (var label in this.Labels)
                label.Relocate(new Point());

            // Передвигаем значки.
            this.RelocateBadges();

            if (showUI)
                this.ShowUI();
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить точку изгиба линии с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс удаляемой точки изгиба линии.</param>
        /// <returns>true, если можно удалить, иначе - false.</returns>
        public bool CanDeletePoint(int index)
        {
            if (index == 0 || index == this.polyline.Points.Count - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Меняет цвет линии.
        /// </summary>
        /// <param name="brush">Кисть.</param>
        /// <param name="brush">Ручка.</param>
        public void ChangeColor(SolidColorBrush brush, Pen pen)
        {
            this.polyline.Stroke = brush;

            this.fillPen = pen;

            foreach (var label in this.Labels)
                label.ChangeColor(brush);
        }

        /// <summary>
        /// Меняет значение точки изгиба линии по заданному индексу.
        /// </summary>
        /// <param name="index">Индекс точки изгиба линии.</param>
        /// <param name="newPoint">Новое положение точки изгиба линии.</param>
        public void ChangePoint(int index, Point newPoint)
        {
            this.polyline.Points[index] = new Point(newPoint.X, newPoint.Y);
        }

        /// <summary>
        /// Скрывает все изменялки, кроме заданной.
        /// </summary>
        /// <param name="element">Изменялка, которая останется видимой.</param>
        public void CollapseAllThumbsExceptThis(LinePointThumb thumb)
        {
            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    if (vertex != thumb)
                        vertex.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Удаляет точку изгиба линии с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс удаляемой точки изгиба линии.</param>
        public void DeleteVertex(int index)
        {
            var showUI = this.isUIVisible;
            
            if (showUI)
                this.HideUI();

            var prevPoints = this.Points;

            // Удаляем заданную точку изгиба линии.
            this.polyline.Points.RemoveAt(index);

            this.Points = this.GetBendPoints();
            
            (this.ViewModel as LineViewModel).OnPointsChanged(prevPoints);

            if (showUI)
                this.ShowUI();
        }

        /// <summary>
        /// Возвращает индекс заданной надписи.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <returns>Индекс надписи.</returns>
        public int GetLabelIndex(SmartLineLabel label)
        {
            return this.Labels.IndexOf(label);
        }
        
        /// <summary>
        /// Возвращает две точки сегмента линии, между которыми находится заданная надпись линии.
        /// </summary>
        /// <param name="label">Надпись линии.</param>
        /// <returns>Тюпл, состояющий из двух точек сегмента линии.</returns>
        public Tuple<Point, Point> GetLabelPoints(SmartLineLabel label)
        {
            var index = this.Labels.IndexOf(label);

            var allPoints = this.polyline.Points;

            if (allPoints.Count == 0)
                return null;

            return new Tuple<Point, Point>(allPoints[index], allPoints[index + 1]);
        }

        /// <summary>
        /// Скрывает все изменялки.
        /// </summary>
        public void HideAllThumbs()
        {
            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    vertex.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Скрывает значки линии.
        /// </summary>
        public void HideBadges()
        {
            foreach (var badge in this.Badges)
                badge.Hide();
        }

        /// <summary>
        /// Скрывает надписи линии.
        /// </summary>
        public void HideLabels()
        {
            foreach (var label in this.Labels)
                label.Visibility = Visibility.Hidden;

            // Также скрываем все линии, которые состоят в одной группе с данной линией.
            foreach (var viewModel in (this.ViewModel as LineViewModel).GetGroupedLines())
                foreach (var label in (this.MapBindingService.GetMapObjectView(viewModel) as InteractiveLine).Labels)
                    label.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Вставляет точку изгиба линии после точки с заданным индексом, которая будет ее копией.
        /// </summary>
        /// <param name="index">Индекс дублируемой точки изгиба линии.</param>
        public void InsertVertex(int index)
        {
            var showUI = this.isUIVisible;

            if (showUI)
                this.HideUI();

            var prevPoints = this.Points;

            this.polyline.Points.Insert(index, new Point(this.polyline.Points[index].X, this.polyline.Points[index].Y));

            this.Points = this.GetBendPoints();
            
            (this.ViewModel as LineViewModel).OnPointsChanged(prevPoints);

            if (showUI)
                this.ShowUI();
        }

        /// <summary>
        /// Переопределяет положения значков линии.
        /// </summary>
        public void RelocateBadges()
        {
            foreach (var badge in this.Badges)
                badge.RelocateBadge();
        }

        /// <summary>
        /// Задает отступ внутри отключенной линии
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public void SetDisabledOffset(double offset)
        {
            if (this.lineStyle == LineStyle.SmallDotted)
                this.polyline.StrokeDashArray[0] = offset;
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="prevAngle">Предыдущий угол поворота надписи.</param>
        public void SetLabelAngleChanged(SmartLineLabel label, double prevAngle)
        {
            if (Math.Abs(label.Angle - prevAngle) > 1)
            {
                (this.ViewModel as LineViewModel).OnLabelAngleChanged(this.Labels.IndexOf(label), label.Angle);
                
                label.SetAngle(label.Angle);
            }
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением изменения положения надписи.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="prevPosition">Предыдущее положение надписи.</param>
        public void SetLabelMoved(SmartLineLabel label, Point prevPosition)
        {
            if (Math.Abs(prevPosition.X - label.LeftTopCorner.X) >= 1 || Math.Abs(prevPosition.Y - label.LeftTopCorner.Y) >= 1)
            {
                (this.ViewModel as LineViewModel).OnLabelPositionChanged(this.Labels.IndexOf(label), label.LeftTopCorner);

                label.SetPosition(label.LeftTopCorner);
            }
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением размера надписи.
        /// </summary>
        /// <param name="label">Надпись.</param>
        public void SetLabelSizeChanged(SmartLineLabel label)
        {
#warning Нужно отслеживать изменение размера надписи
        }

        /// <summary>
        /// Задает стиль линии.
        /// </summary>
        /// <param name="lineStyle">Стиль линии.</param>
        public void SetLineStyle(LineStyle lineStyle)
        {
            this.lineStyle = lineStyle;

            switch (lineStyle)
            {
                case LineStyle.Dotted:
                    this.polyline.StrokeDashArray = new DoubleCollection()
                    {
                        this.MapBindingService.MapSettingService.LinePlanningOffset
                    };

                    break;

                case LineStyle.Normal:
                    this.polyline.StrokeDashArray = null;

                    break;

                case LineStyle.SmallDotted:
                    this.polyline.StrokeDashArray = new DoubleCollection()
                    {
                        this.MapBindingService.MapSettingService.LineDisabledOffset
                    };

                    break;
            }
        }

        /// <summary>
        /// Задает отступ внутри планируемой линии
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public void SetPlanningOffset(double offset)
        {
            if (this.lineStyle == LineStyle.Dotted)
                this.polyline.StrokeDashArray[0] = offset;
        }

        /// <summary>
        /// Задает изменение структуры точек изгиба линии.
        /// </summary>
        /// <param name="prevPoints">Предыдущая структура точек изгиба линии.</param>
        public void SetPointsChanged(string prevPoints)
        {
            this.Points = this.GetBendPoints();
            
            (this.ViewModel as LineViewModel).OnPointsChanged(prevPoints);
        }

        /// <summary>
        /// Задает толщину линии.
        /// </summary>
        /// <param name="thickness">Толщина линии.</param>
        public void SetThickness(double thickness)
        {
            this.Shape.StrokeThickness = thickness;
        }

        /// <summary>
        /// Показывает все изменялки.
        /// </summary>
        public void ShowAllThumbs()
        {
            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    vertex.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Показывает значки линии.
        /// </summary>
        public void ShowBadges()
        {
            foreach (var badge in this.Badges)
                badge.Show();
        }

        /// <summary>
        /// Показывает надписи линии.
        /// </summary>
        public void ShowLabels()
        {
            if (!this.CanShowLabels)
                return;

            foreach (var label in this.Labels)
                label.Visibility = Visibility.Visible;

            // Также отображаем все линии, которые состоят в одной группе с данной линией.
            foreach (var viewModel in (this.ViewModel as LineViewModel).GetGroupedLines())
                foreach (var label in (this.MapBindingService.GetMapObjectView(viewModel) as InteractiveLine).Labels)
                    label.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Начинает анимирование линии.
        /// </summary>
        public void StartAnimation()
        {
            this.MapBindingService.StartAnimation(this);
        }

        /// <summary>
        /// Заканчивает анимирование линии.
        /// </summary>
        public void StopAnimation()
        {
            this.MapBindingService.StopAnimation(this);
        }

        /// <summary>
        /// Превращает узел изгиба с заданным индексом в узел соединения линий.
        /// </summary>
        /// <param name="index">Индекс узла изгиба.</param>
        public void TransformVertex(int index)
        {
            (this.ViewModel as LineViewModel).PointToNode(index - 1);
        }

        /// <summary>
        /// Обновляет надписи линии.
        /// </summary>
        public void UpdateLabels()
        {
            var viewModel = this.ViewModel as LineViewModel;
            
            var namedObject = viewModel.namedObject;

            // Сперва проверяем, есть ли форсированные сегменты у линии. Если есть и их количество совпадает с количеством надписей, то используем длины от них.
            if (viewModel.ForcedSegments != null && viewModel.ForcedSegments.Count == this.Labels.Count)
                for (int i = 0; i < this.Labels.Count; i++)
#warning Тут производим замену протяженности трубы в названии
                    this.Labels[i].SetText(namedObject.Name.Replace("!@#$%Alias.LineLength!@#$%", viewModel.ForcedSegments[i].Length.ToString()));
            else
            {
                // Здесь будем хранить значение, которое будет указывать на то, что имеет ли линия предопределенные надписи.
                var isPredefined = false;

                if (viewModel.GroupedLine != null && viewModel.GroupedLine != viewModel)
                {
                    var mainLine = this.MapBindingService.GetMapObjectView(viewModel.GroupedLine) as InteractiveLine;

                    if (mainLine != null && this.Labels.Count == mainLine.Labels.Count)
                    {
                        isPredefined = true;

                        // Если линия сгруппирована с другой линией, то для каждой надписи нужно взять протяженность с соответствующей ей записи.
                        var div = new string[1]
                        {
                            labelDiv
                        };
                        // Получаем индекс длины линии в надписи.
                        var index = mainLine.ViewModel.Type.CaptionParameters.FindIndex(x => x.Alias == Models.Alias.LineLength);
                        if (index != -1)
                        {
                            for (int i = 0; i < this.Labels.Count; i++)
                                if (!string.IsNullOrEmpty(mainLine.Labels[i].Text))
                                {

#warning Тут производим замену протяженности трубы в названии

                                    string[] arrayString = mainLine.Labels[i].Text.Split(div, StringSplitOptions.RemoveEmptyEntries);
                                    this.Labels[i].SetText(namedObject.Name.Replace("!@#$%Alias.LineLength!@#$%", arrayString[index]));
                                }
                        }
                        else
                            for (int i = 0; i < this.Labels.Count; i++)
                                this.Labels[i].SetText(namedObject.Name.Replace("!@#$%Alias.LineLength!@#$%", "ОШИБКА"));
                    }
                }

                if (!isPredefined)
                {
                    Point a, b;

                    var totalLength = viewModel.GetFullLength();

                    foreach (var label in this.Labels)
                    {
                        var points = this.GetLabelPoints(label);

                        if (points != null)
                        {
                            a = points.Item1;
                            b = points.Item2;
                            
#warning Тут производим замену протяженности трубы в названии
                            label.SetText(namedObject.Name.Replace("!@#$%Alias.LineLength!@#$%", Math.Round(PointHelper.GetDistance(a, b) * viewModel.Length / totalLength, 2).ToString()));
                        }
                    }
                }
            }
        }

        #endregion
    }

    // Реализация InteractiveShape.
    internal sealed partial class InteractiveLine
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает центральную точку фигуры.
        /// </summary>
        public override Point CenterPoint
        {
            get
            {
                var a = this.LeftPoint;
                var b = this.RightPoint;

                return new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
            }
        }

        /// <summary>
        /// Возвращает важную точку фигуры.
        /// </summary>
        public override Point MajorPoint
        {
            get
            {
                return this.LeftPoint;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void HideUI()
        {
            foreach (var vertex in this.vertices)
                this.Canvas.Children.Remove(vertex);

            this.vertices.Clear();

            this.vertices = null;

            this.isUIVisible = false;
        }
        
        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnContextMenuRequested(Point mousePosition)
        {
            this.ContextMenuPosition = mousePosition;

            // Получаем контекстное меню из ресурсов.
            this.polyline.ContextMenu = Application.Current.Resources["LineContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.polyline.ContextMenu.DataContext = null;
            this.polyline.ContextMenu.DataContext = this.ViewModel;

            // Задаем цель размещения контекстному меню, чтобы его элементы могли ссылаться на свойства линии.
            this.polyline.ContextMenu.PlacementTarget = null;
            this.polyline.ContextMenu.PlacementTarget = this.polyline;

            this.polyline.ContextMenu.Closed += this.ContextMenu_Closed;

            this.polyline.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню редактирования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnEditContextMenuRequested(Point mousePosition)
        {
            this.ContextMenuPosition = mousePosition;

            // Получаем контекстное меню из ресурсов.
            this.polyline.ContextMenu = Application.Current.Resources["LineEditContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.polyline.ContextMenu.DataContext = null;
            this.polyline.ContextMenu.DataContext = this.ViewModel;

            // Задаем цель размещения контекстному меню, чтобы его элементы могли ссылаться на свойства линии.
            this.polyline.ContextMenu.PlacementTarget = null;
            this.polyline.ContextMenu.PlacementTarget = this.polyline;

            this.polyline.ContextMenu.Closed += this.ContextMenu_Closed;

            this.polyline.ContextMenu.IsOpen = true;
        }
        
        /// <summary>
        /// Выполняет действия, связанные с двойным нажатием мыши по фигуре.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnMouseDoubleClick(Point mousePosition)
        {
            (this.ViewModel as LineViewModel).SelectPathCommand.Execute(null);
        }

        /// <summary>
        /// Выполняет действия, связанные с перемещением фигуры.
        /// </summary>
        /// <param name="deltaX">Изменение положения фигуры по X.</param>
        /// <param name="deltaY">Изменение положения фигуры по Y.</param>
        protected override void OnMoving(double deltaX, double deltaY)
        {
            // Если не было выполнено скрытие некоторых элементов линии, выполняем его.
            if (!this.isUIhidden)
            {
                this.isUIhidden = true;

                this.HideAllThumbs();
                
                this.HideLabels();

                this.HideBadges();

                // Отключаем возможность выделения линии.
                this.CanBeHighlighted = false;
            }

            var a = this.LeftPoint;
            var b = this.RightPoint;

            this.LeftPoint = new Point(a.X + deltaX, a.Y + deltaY);
            this.RightPoint = new Point(b.X + deltaX, b.Y + deltaY);

            // Сдвигаем точки изгиба линии.
            for (int i = 1; i < this.polyline.Points.Count - 1; i++)
                this.polyline.Points[i] = new Point(this.polyline.Points[i].X + deltaX, this.polyline.Points[i].Y + deltaY);
            // И их изменялки.
            if (this.vertices != null)
                foreach (var vertex in this.vertices)
                    vertex.Position = new Point(this.polyline.Points[vertex.Index].X, this.polyline.Points[vertex.Index].Y);
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения фигуры.
        /// </summary>
        protected override void OnMovingCompleted()
        {
            var prevPoint = this.prevPoints.Item1;
            var point = this.LeftPoint;

            if (Math.Abs(prevPoint.X - point.X) >= 1 || Math.Abs(prevPoint.Y - point.Y) >= 1)
            {
                this.Points = this.GetBendPoints();

                (this.ViewModel as LineViewModel).OnMoved(this.prevPoints, this.prevBendPoints);
            }

            this.ShowAllThumbs();

            this.ShowLabels();

            this.ShowBadges();

            // Включаем возможность выделения линии.
            this.CanBeHighlighted = true;

            this.isUIhidden = false;
        }

        /// <summary>
        /// Выполняет действия, связанные с началом перемещения фигуры.
        /// </summary>
        protected override void OnMovingStarted()
        {
            this.prevPoints = new Tuple<Point, Point>(this.LeftPoint, this.RightPoint);
            this.prevBendPoints = this.Points;
        }

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void ShowUI()
        {
            // Создаем изменялки точек изгиба линии и добавляем их на карту.
            this.vertices = new List<LinePointThumb>();
            var index = -1;
            foreach (var point in this.polyline.Points)
            {
                index++;

                // Не создаем первую и последнюю вершины.
                if (index == 0 || index == this.polyline.Points.Count - 1)
                    continue;

                this.vertices.Add(new LinePointThumb(this, new Point(point.X, point.Y), Cursors.ScrollAll, index, this.polyline.Stroke, this.MapBindingService.MapSettingService.LinePointThumbSize, this.MapBindingService.MapSettingService.LinePointThumbThickness));
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
        /// Добавляет объект на холст.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <returns>true, если объект был добавлен, иначе - false.</returns>
        public override bool AddToCanvas(IndentableCanvas canvas)
        {
            var result = base.AddToCanvas(canvas);

            if (result)
                this.polyline.ToolTipOpening += this.Polyline_ToolTipOpening;

            return result;
        }

        /// <summary>
        /// Завершает перемещение фигуры. Используется для закрепления результата работы метода <see cref="MoveTo(Point)"/>.
        /// </summary>
        public override void EndMoveTo()
        {
            this.Points = this.GetBendPoints().ToString();

            this.ShowLabels();
        }

        /// <summary>
        /// Возвращает геометрию фигуры.
        /// </summary>
        /// <returns>Геометрия.</returns>
        public override Geometry GetGeometry()
        {
            this.polyline.UpdateLayout();

            var result = this.polyline.RenderedGeometry.Clone();

            if (result.CanFreeze)
                result.Freeze();

            return result;
        }

        /// <summary>
        /// Возвращает кисть обводки фигуры.
        /// </summary>
        /// <returns>Кисть обводки.</returns>
        public override Pen GetStrokePen()
        {
            return this.fillPen;
        }

        /// <summary>
        /// Перемещает фигуру в заданную точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        public override void MoveTo(Point point)
        {
            var deltaX = point.X - this.CenterPoint.X;
            var deltaY = point.Y - this.CenterPoint.Y;

            this.LeftPoint = new Point(this.LeftPoint.X + deltaX, this.LeftPoint.Y + deltaY);
            this.RightPoint = new Point(this.RightPoint.X + deltaX, this.RightPoint.Y + deltaY);

            // Сдвигаем точки изгиба линии.
            for (int i = 1; i < this.polyline.Points.Count - 1; i++)
                this.polyline.Points[i] = new Point(this.polyline.Points[i].X + deltaX, this.polyline.Points[i].Y + deltaY);
        }

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public override bool RemoveFromCanvas()
        {
            var result = base.RemoveFromCanvas();

            if (result)
                this.polyline.ToolTipOpening -= this.Polyline_ToolTipOpening;

            return result;
        }

        /// <summary>
        /// Начинает перемещение фигуры.
        /// </summary>
        public override void StartMoveTo()
        {
            this.HideLabels();
        }

        #endregion
    }

    // Реализация IDrawableObject.
    internal sealed partial class InteractiveLine
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        public void Draw(Point mousePrevPosition, Point mousePosition)
        {
            if (this.IsInitialized)
                return;

            // Передвигаем правую точку.
            this.RightPoint = new Point(mousePosition.X, mousePosition.Y);
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
}