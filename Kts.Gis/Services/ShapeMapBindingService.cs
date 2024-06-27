using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.Gis.Views;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.Services
{
    /// <summary>
    /// Представляет сервис привязки представлений карты с моделями представлений, где представления представляются экземплярами класса <see cref="System.Windows.Shapes.Shape"/>.
    /// </summary>
    [Serializable]
    internal sealed partial class ShapeMapBindingService : BaseMapBindingService
    {
        #region Закрытые поля

        /// <summary>
        /// Режим поиска коллизии.
        /// </summary>
        private CollisionSearchMode collisionSearchMode;

        /// <summary>
        /// Объект, столкнувшийся с узлом.
        /// </summary>
        private IMapObjectViewModel hittedObject;

        /// <summary>
        /// Список объектов, столкнувшихся с узлом.
        /// </summary>
        private List<IMapObjectViewModel> hittedObjects;

        /// <summary>
        /// Узел-источник столкновений.
        /// </summary>
        private InteractiveNode hittingSource;

        /// <summary>
        /// Значение, указывающее на то, что скрыты ли насильно надписи объектов.
        /// </summary>
        private bool isLabelsForceHidden;

        /// <summary>
        /// Значение, указывающее на то, что скрыты ли надписи объектов.
        /// </summary>
        private bool isLabelsHidden;

        /// <summary>
        /// Значение, указывающее на то, что скрыты ли узлы.
        /// </summary>
        private bool isNodesHidden;

        /// <summary>
        /// Путь из линий, соединенный с узлом-источником столкновений.
        /// </summary>
        private List<LineViewModel> linePath;

        #endregion

        #region Закрытые неизменяемые поля

#warning Надо будет изменить эту хрень, так как в самих фигурах тоже задается цвет границы
        /// <summary>
        /// Цвет границы фигуры.
        /// </summary>
        private readonly Utilities.Color figureBoxColor = new Utilities.Color(0, 0, 0);

        /// <summary>
        /// Связи между моделями представлений и представлениями.
        /// </summary>
        private readonly Dictionary<IMapObjectViewModel, IMapObject> viewModelView = new Dictionary<IMapObjectViewModel, IMapObject>();

        /// <summary>
        /// Связи между представлениями и моделями представлений.
        /// </summary>
        private readonly Dictionary<IMapObject, IMapObjectViewModel> viewViewModel = new Dictionary<IMapObject, IMapObjectViewModel>();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ShapeMapBindingService"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        /// <param name="badgeGeometries">Геометрии значков.</param>
        /// <param name="badgeHotPoints">Главные точки геометрий значков.</param>
        /// <param name="badgeOriginPoints">Точки поворотов геометрий значков.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="mapSettingService">Сервис настроек вида карты.</param>
        public ShapeMapBindingService(Map map, Dictionary<ObjectType, Geometry> badgeGeometries, Dictionary<ObjectType, Point> badgeHotPoints, Dictionary<ObjectType, Point> badgeOriginPoints, AccessService accessService, IMapSettingService mapSettingService) : base(map, badgeGeometries, badgeHotPoints, badgeOriginPoints, accessService, mapSettingService)
        {
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает список зарегистрированных фигур.
        /// </summary>
        /// <returns>Список фигур.</returns>
        private List<InteractiveFigure> GetFigures()
        {
            var figures = new List<InteractiveFigure>();

            foreach (var key in this.viewViewModel.Keys)
                if (key is InteractiveFigure)
                    figures.Add((InteractiveFigure)key);

            return figures;
        }

        /// <summary>
        /// Возвращает градиентную кисть для заданной фигуры.
        /// </summary>
        /// <param name="figure">Фигура.</param>
        /// <returns>Градиентная кисть.</returns>
        private LinearGradientBrush GetGradientBrush(FigureViewModel figure)
        {
            var brush = new LinearGradientBrush()
            {
                EndPoint = new Point(0, 1),
                StartPoint = new Point(0, 0)
            };

            // Сперва добавляем родной цвет.
            var color = this.GetBrush(figure.IsActive ? figure.Type.Color : figure.Type.InactiveColor).Color;
            brush.GradientStops.Add(new GradientStop(color, 0));
            brush.GradientStops.Add(new GradientStop(color, 0.5));

            var offset = 0.5 / figure.ChildrenTypes.Count;

            var curOffset = 0.5;

            if (figure.IsActive)
                foreach (var t in figure.ChildrenTypes)
                {
                    brush.GradientStops.Add(new GradientStop(this.GetBrush(t.Color).Color, curOffset));

                    curOffset += offset;

                    brush.GradientStops.Add(new GradientStop(this.GetBrush(t.Color).Color, curOffset));
                }
            else
                foreach (var t in figure.ChildrenTypes)
                {
                    brush.GradientStops.Add(new GradientStop(this.GetBrush(t.InactiveColor).Color, curOffset));

                    curOffset += offset;

                    brush.GradientStops.Add(new GradientStop(this.GetBrush(t.InactiveColor).Color, curOffset));
                }

            if (brush.CanFreeze)
                brush.Freeze();

            return brush;
        }

        /// <summary>
        /// Возвращает список зарегистрированных линий.
        /// </summary>
        /// <returns>Список линий.</returns>
        private List<InteractiveLine> GetLines()
        {
            var lines = new List<InteractiveLine>();

            foreach (var key in this.viewViewModel.Keys)
                if (key is InteractiveLine)
                    lines.Add((InteractiveLine)key);

            return lines;
        }

        /// <summary>
        /// Возвращает стиль линии.
        /// </summary>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли линия планируемой.</param>
        /// <returns>Стиль линии.</returns>
        private LineStyle GetLineStyle(bool isPlanning)
        {
            var lineStyle = LineStyle.Normal;

            if (isPlanning)
                lineStyle = LineStyle.Dotted;

#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
            //if (!isWorking)
            //    lineStyle = LineStyle.SmallDotted;

            return lineStyle;
        }

        /// <summary>
        /// Возвращает новое представление объекта карты по заданной модели представления.
        /// </summary>
        /// <param name="mapObject">Модель представления объекта карты.</param>
        /// <returns>Представление объекта карты.</returns>
        private IMapObject GetNewView(IMapObjectViewModel mapObject)
        {
            var type = mapObject.GetType();

            if (type == typeof(ApprovedHeaderViewModel))
                return new ApprovedHeader(mapObject as ApprovedHeaderViewModel);

            if (type == typeof(BadgeViewModel))
            {
                var badge = mapObject as BadgeViewModel;

                return new InteractiveBadge(badge, this.BadgeHotPoints[badge.Type], this.BadgeOriginPoints[badge.Type], this.BadgeGeometries[badge.Type], this.BadgeScaleTransform, this);
            }

            if (type == typeof(EllipseViewModel))
            {
                var ellipse = mapObject as EllipseViewModel;

                if (ellipse.ChildrenTypes.Count > 0)
                    return new InteractiveEllipse(ellipse, ellipse.Angle, ellipse.Position, ellipse.Size, this.GetGradientBrush(ellipse), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness), this.MapSettingService.FigureThickness, this);

                return new InteractiveEllipse(ellipse, ellipse.Angle, ellipse.Position, ellipse.Size, this.GetBrush(ellipse.IsActive ? ellipse.Type.Color : ellipse.Type.InactiveColor), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness), this.MapSettingService.FigureThickness, this);
            }

            if (type == typeof(LabelViewModel))
            {
                var label = mapObject as LabelViewModel;

                return new SmartIndependentLabel(label.Content, label.Angle, label.Position, label.Size, label.IsBold, label.IsItalic, label.IsUnderline, label, this);
            }

            if (type == typeof(LengthPerDiameterTableViewModel))
                return new LengthPerDiameterTable(mapObject as LengthPerDiameterTableViewModel);

            if (type == typeof(LineViewModel))
            {
                var line = mapObject as LineViewModel;

                return new InteractiveLine(line, line.LeftPoint, line.RightPoint, line.Points, this.GetLineStyle(line.IsPlanning), this.MapSettingService.LineThickness /*25*/, this.GetBrush(line.IsActive ? line.Type.Color : line.Type.InactiveColor), this.GetPen(line.IsActive ? line.Type.Color : line.Type.InactiveColor, this.MapSettingService.LineSelectedThickness), this);
            }

            if (type == typeof(NewRulerViewModel))
            {
                var newRuler = mapObject as NewRulerViewModel;

                return new NewRuler(newRuler.Points, this.MapSettingService.NewRulerThickness, this.MapSettingService.NewRulerEdgeOffset, this);
            }

            if (type == typeof(NodeViewModel))
            {
                var node = mapObject as NodeViewModel;

                return new InteractiveNode(node, new Size(this.MapSettingService.NodeSize, this.MapSettingService.NodeSize), node.LeftTopCorner, this.AccessService.CanViewNodeId ? "Идентификатор узла: " + node.Id.ToString() : null, this.MapSettingService.NodeThickness, this);
            }

            if (type == typeof(PolygonViewModel))
            {
                var polygon = mapObject as PolygonViewModel;

                if (polygon.ChildrenTypes.Count > 0)
                    return new InteractivePolygon(polygon, polygon.Angle, polygon.Position, polygon.Points, polygon.Size, this.GetGradientBrush(polygon), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness), this.MapSettingService.FigureThickness, this);

                return new InteractivePolygon(polygon, polygon.Angle, polygon.Position, polygon.Points, polygon.Size, this.GetBrush(polygon.IsActive ? polygon.Type.Color : polygon.Type.InactiveColor), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness), this.MapSettingService.FigureThickness, this);
            }

            if (type == typeof(PolylineViewModel))
            {
                var polyline = mapObject as PolylineViewModel;
                
                var polylineStyle = LineStyle.Normal;

                if (polyline.IsPlanning)
                    polylineStyle = LineStyle.Dotted;

                return new Polyline(polyline.Points, this.GetBrush((polyline.Id as ObjectType).Color), polyline.Thickness, polylineStyle, this);
                /*
                ObjectType tmpObjectType = polyline.Id as ObjectType;
                Utilities.Color tmpColor = tmpObjectType.Color;



                SolidColorBrush tmpColorBrush = this.GetBrush(tmpColor);


                return new Polyline(polyline.Points, tmpColorBrush/1*this.GetBrush((polyline.Id as ObjectType).Color)*1/
                    , /1*polyline.Thickness*1/80, polylineStyle, this);
                */
            }

            if (type == typeof(RectangleViewModel))
            {
                var rectangle = mapObject as RectangleViewModel;

                if (rectangle.ChildrenTypes.Count > 0)
                    return new InteractiveRectangle(rectangle, rectangle.Angle, rectangle.Position, rectangle.Size, this.GetGradientBrush(rectangle), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness), this.MapSettingService.FigureThickness, this);

                return new InteractiveRectangle(rectangle, rectangle.Angle, rectangle.Position, rectangle.Size, this.GetBrush(rectangle.IsActive ? rectangle.Type.Color : rectangle.Type.InactiveColor), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness), this.MapSettingService.FigureThickness, this);
            }

            if (type == typeof(RulerViewModel))
            {
                var ruler = mapObject as RulerViewModel;

                return new Ruler(ruler.StartPoint, ruler.EndPoint, this.MapSettingService.RulerThickness, this.MapSettingService.RulerEdgeOffset, this);
            }
            
            throw new NotImplementedException("Не реализовано возвращение нового представления для следующего типа модели представления: " + type.ToString());
        }

        /// <summary>
        /// Проверяет коллизию узла с объектом.
        /// </summary>
        /// <param name="result">Результат проверки.</param>
        /// <returns>Значение, указывающее на то, что следует ли продолжать поиск объектов.</returns>
        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            if (!(result.VisualHit is FrameworkElement))
                return HitTestResultBehavior.Continue;

            var tag = ((FrameworkElement)result.VisualHit).Tag as IInteractiveShape;

            if (tag != null && tag != this.hittingSource && tag.GetType() != typeof(InteractiveBadge))
            {
                if (!tag.IsVisible)
                    return HitTestResultBehavior.Continue;

                var viewModel = this.viewViewModel[tag];

                var lineViewModel = viewModel as LineViewModel;
                var nodeViewModel = viewModel as NodeViewModel;

                var sourceNodeViewModel = this.viewViewModel[this.hittingSource] as NodeViewModel;

                if (lineViewModel != null && sourceNodeViewModel.ConnectedLines.Contains(lineViewModel))
                    // Если столкнувшийся объект - линия, и она присоединена к узлу столкновения, то продолжаем поиски.
                    return HitTestResultBehavior.Continue;

                if (lineViewModel != null && sourceNodeViewModel.ConnectedLinesType != lineViewModel.Type)
                    // Если столкнувшийся объект - линия, и к узлу столкновения присоединены линии другого типа, то продолжаем поиски.
                    return HitTestResultBehavior.Continue;

#warning Некоторым пользователям нужно чтобы можно было прикрепить линию к своей сети
                //if (lineViewModel != null)
                    // Проверяем, не входит ли линия в список линий, входящих в путь линий, соединенный с узлом-источником столкновений.
                    //if (this.linePath.Contains(lineViewModel))
                        //return HitTestResultBehavior.Continue;

                if (sourceNodeViewModel.ConnectedObject != null && sourceNodeViewModel.ConnectedObject == viewModel)
                    // Если столкнувшийся объект является объектом, к которому присоединен узел, то продолжаем поиски.
                    return HitTestResultBehavior.Continue;

                if (nodeViewModel != null && sourceNodeViewModel.ConnectedLinesType != nodeViewModel.ConnectedLinesType)
                    // Если столкнувшийся объект - узел, и узел столкновения содержит линии другого типа, то продолжаем поиски.
                    return HitTestResultBehavior.Continue;

#warning Некоторым пользователям нужно чтобы можно было прикрепить линию к своей сети
                //if (nodeViewModel != null)
                    // Проверяем, не входит ли узел в список линий, входящих в путь линий, соединенный с узлом-источником столкновений.
                    //if (this.linePath.Where(x => x.LeftNode == nodeViewModel || x.RightNode == nodeViewModel).Count() != 0)
                        //return HitTestResultBehavior.Continue;

                switch (this.collisionSearchMode)
                {
                    case CollisionSearchMode.First:
                        this.hittedObject = viewModel;

                        return HitTestResultBehavior.Stop;

                    case CollisionSearchMode.FirstAndNearest:
                        this.hittedObjects.Add(viewModel);

                        break;

                    default:
                        throw new NotImplementedException("Не реализована работа со следующим режимом поиска коллизии: " + this.collisionSearchMode.ToString());
                }
            }

            return HitTestResultBehavior.Continue;
        }

        /// <summary>
        /// Пробует уменьшить размер шрифта заданной надписи.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        private int? TryDecreaseLabelSize(SmartLabel label)
        {
            int? result = null;

            if (label.CanDecreaseFontSize())
            {
                result = label.GetDecreasedFontSize();

                label.SetSize(result);

                label.Relocate(new Point(0, 0));
            }

            return result;
        }

        /// <summary>
        /// Пробует увеличить размер шрифта заданной надписи.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        private int? TryIncreaseLabelSize(SmartLabel label)
        {
            int? result = null;

            if (label.CanIncreaseFontSize())
            {
                result = label.GetIncreasedFontSize();

                label.SetSize(result);

                label.Relocate(new Point(0, 0));
            }

            return result;
        }

        #endregion
    }

    // Реализация BaseMapBindingService.
    internal sealed partial class ShapeMapBindingService
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Скрывает надписи объектов.
        /// </summary>
        protected override void HideLabels()
        {
            if (!this.isLabelsHidden)
            {
                // Надписи объектов находятся на 4 уровню по Z.
                this.Map.HideCanvases(4);

                this.isLabelsHidden = true;
            }
        }

        /// <summary>
        /// Скрывает узлы.
        /// </summary>
        protected override void HideNodes()
        {
            if (!this.isNodesHidden)
            {
                // Узлы находятся на 3 уровню по Z.
                this.Map.HideCanvases(3);

                this.isNodesHidden = true;
            }
        }

        /// <summary>
        /// Отображает надписи объектов.
        /// </summary>
        protected override void ShowLabels()
        {
            if (this.isLabelsForceHidden)
                return;

            if (this.isLabelsHidden)
            {
                // Отображаем только нужные слои.
                var ids = this.Map.GetLayerIds();
                foreach (var id in ids)
                    if (this.Map.IsLayerVisible(id))
                        this.Map.SetLayerVisibility(id, true);

                this.isLabelsHidden = false;
            }
        }

        /// <summary>
        /// Отображает узлы.
        /// </summary>
        protected override void ShowNodes()
        {
            if (this.isNodesHidden)
            {
                // Отображаем только нужные слои.
                var ids = this.Map.GetLayerIds();
                foreach (var id in ids)
                    if (this.Map.IsLayerVisible(id))
                        this.Map.SetLayerVisibility(id, true);

                this.isNodesHidden = false;
            }
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручками.
        /// </summary>
        /// <param name="objects">Словарь, где объекты разделены по ручкам.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public override Guid AddLayer(Dictionary<Pen, List<IObjectViewModel>> objects, bool front)
        {
            var geometries = new Dictionary<Pen, List<Geometry>>();

            InteractiveShape shape;

            foreach (var entry in objects)
            {
                geometries.Add(entry.Key, new List<Geometry>());

                foreach (var obj in entry.Value)
                {
                    shape = this.GetMapObjectView((IMapObjectViewModel)obj) as InteractiveShape;

                    if (shape != null && shape.CanBeHighlighted)
                        geometries[entry.Key].Add(shape.GetGeometry());
                }
            }

            return this.Map.AddLayer(geometries, front);
        }

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручкой.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="pen">Ручка обводки объектов.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public override Guid AddLayer(List<IObjectViewModel> objects, Pen pen, bool front)
        {
            var geometries = new List<Geometry>();

            InteractiveShape shape;

            foreach (var obj in objects)
            {
                shape = this.GetMapObjectView((IMapObjectViewModel)obj) as InteractiveShape;

                if (shape != null && shape.CanBeHighlighted)
                    geometries.Add(shape.GetGeometry());
            }

            return this.Map.AddLayer(geometries, pen, front);
        }

        /// <summary>
        /// Включает анимацию заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public override void AnimateOn(IObjectViewModel obj)
        {
            var shape = this.GetMapObjectView((IMapObjectViewModel)obj) as InteractiveShape;

            if (shape != null && shape.CanBeHighlighted)
                this.Map.AnimateOn(shape.GetGeometry(), shape.GetStrokePen());
        }

        /// <summary>
        /// Включает анимацию заданных объектов.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public override void AnimateOn(List<IObjectViewModel> objects)
        {
            var geometries = new List<Geometry>();
            var pens = new List<Pen>();

            InteractiveShape shape;

            foreach (var obj in objects)
            {
                shape = this.GetMapObjectView((IMapObjectViewModel)obj) as InteractiveShape;

                if (shape != null && shape.CanBeHighlighted)
                {
                    geometries.Add(shape.GetGeometry());
                    pens.Add(shape.GetStrokePen());
                }
            }

            this.Map.AnimateOn(geometries, pens);
        }

        /// <summary>
        /// Проверяет наличие коллизии узла с другими объектами карты.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="mode">Режим поиска коллизии.</param>
        /// <param name="radius">Радиус поиска.</param>
        /// <returns>Первый подходящий объект карты.</returns>
        public override IMapObjectViewModel CheckCollision(NodeViewModel node, CollisionSearchMode mode, double radius)
        {
            if (mode == CollisionSearchMode.FirstAndNearest)
            {
                // Сперва пробуем найти объект по наименьшему радиусу.
                var obj = this.CheckCollision(node, CollisionSearchMode.First, radius);

                if (obj != null)
                    // Если такой найден, то просто возвращаем его.
                    return obj;
            }

            var rg = new RectangleGeometry(new Rect(new Point(node.LeftTopCorner.X - radius, node.LeftTopCorner.Y - radius), new Size(radius * 2, radius * 2)));
            
            this.hittedObject = null;
            this.hittedObjects = new List<IMapObjectViewModel>();
            this.hittingSource = this.viewModelView[node] as InteractiveNode;

            this.collisionSearchMode = mode;

            this.linePath = node.ConnectedLines.First().GetLinePath();

            VisualTreeHelper.HitTest(this.hittingSource.Canvas.Parent as Visual, null, new HitTestResultCallback(this.HitTestCallback), new GeometryHitTestParameters(rg));

            switch (mode)
            {
                case CollisionSearchMode.First:
                    return this.hittedObject;

                case CollisionSearchMode.FirstAndNearest:
                    IMapObjectViewModel result = null;

                    double minDistance = double.MaxValue;

                    double distance = double.MaxValue;

                    foreach (var obj in this.hittedObjects)
                    {
                        var nodeViewModel = obj as NodeViewModel;

                        if (nodeViewModel != null)
                            distance = PointHelper.GetDistance(node.LeftTopCorner, nodeViewModel.LeftTopCorner);
                        else
                        {
                            var lineViewModel = obj as LineViewModel;

                            if (lineViewModel != null)
                            {
                                var segment = lineViewModel.GetNearestSegment(node.LeftTopCorner);

                                var a = segment.Item1;
                                var b = segment.Item2;
                                var c = node.LeftTopCorner;

                                var ac = new Vector(c.X - a.X, c.Y - a.Y);
                                var ab = new Vector(b.X - a.X, b.Y - a.Y);

                                var p = ac.X * ab.X + ac.Y * ab.Y;
                                var l = ab.X * ab.X + ab.Y * ab.Y;

                                if (p > 0 && p < l)
                                    distance = (ac - Vector.Multiply(p / l, ab)).Length;
                                else
                                    if (p <= 0)
                                        distance = ac.Length;
                                    else
                                        if (p >= l)
                                            distance = new Vector(c.X - b.X, c.Y - b.Y).Length;
                            }
                        }

                        if (distance < minDistance)
                        {
                            result = obj;

                            minDistance = distance;
                        }
                    }

                    return result;
            }

            throw new NotImplementedException("Не реализована работа со следующим режимом поиска коллизии: " + mode.ToString());
        }

        /// <summary>
        /// Убирает очертания объектов с карты.
        /// </summary>
        public override void ClearOutlines()
        {
            this.Map.ClearOutlines();
        }

        /// <summary>
        /// Создает очертания объектов на карте.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public override void CreateOutlines(List<IMapObjectViewModel> objects)
        {
            var geometries = new List<Geometry>();
            var pens = new List<Pen>();

            InteractiveShape shape;

            foreach (var obj in objects)
            {
                shape = this.GetMapObjectView(obj) as InteractiveShape;

                if (shape != null)
                {
                    geometries.Add(shape.GetGeometry());
                    pens.Add(shape.GetStrokePen());
                }
            }

            this.Map.CreateOutlines(geometries, pens);
        }

        /// <summary>
        /// Насильно скрывает надписи.
        /// </summary>
        public override void ForceHideLabels()
        {
            if (!this.isLabelsHidden)
            {
                this.Map.HideCanvases(4);

                this.isLabelsHidden = true;
            }

            this.isLabelsForceHidden = true;
        }

        /// <summary>
        /// Насильно отображает надписи.
        /// </summary>
        public override void ForceShowLabels()
        {
            if (this.isLabelsHidden)
            {
                // Отображаем только нужные слои.
                var ids = this.Map.GetLayerIds();
                foreach (var id in ids)
                    if (this.Map.IsLayerVisible(id))
                        this.Map.SetLayerVisibility(id, true);

                this.isLabelsHidden = false;
            }

            this.isLabelsForceHidden = false;
        }

        /// <summary>
        /// Возвращает положение центра текущего куска карты.
        /// </summary>
        /// <returns>Положение центра.</returns>
        public override Point GetCurrentCenter()
        {
            return this.Map.GetCurrentCenter();
        }

        /// <summary>
        /// Возвращает представление заданной модели представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Модель представления объекта карты.</param>
        /// <returns>Объект карты.</returns>
        public override IMapObject GetMapObjectView(IMapObjectViewModel mapObject)
        {
            if (!this.viewModelView.ContainsKey(mapObject))
                return null;

            return this.viewModelView[mapObject];
        }

        /// <summary>
        /// Возвращает модель представления заданного представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Объект карты.</param>
        /// <returns>Модель представления объекта карты.</returns>
        public override IMapObjectViewModel GetMapObjectViewModel(IMapObject mapObject)
        {
            return this.viewViewModel[mapObject];
        }

        /// <summary>
        /// Уведомляет модель представления объекта карты об изменении его представления.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourceMemberName">Название члена-источника.</param>
        public override void NotifyMapObjectViewModel(IMapObject source, string sourceMemberName)
        {
            if (!this.viewViewModel.ContainsKey(source))
                return;

            if (source as Polyline != null)
                if (sourceMemberName == nameof(Polyline.Points))
                    (this.viewViewModel[source] as PolylineViewModel).OnPointsChanged();

            if (source as NewRuler != null)
                if (sourceMemberName == nameof(NewRuler.Points))
                    (this.viewViewModel[source] as NewRulerViewModel).OnPointsChanged();
        }

        /// <summary>
        /// Регистрирует связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        public override void RegisterBinding(IMapObjectViewModel mapObject)
        {
            if (this.viewModelView.ContainsKey(mapObject))
                return;

            if ( mapObject is PolygonViewModel)
            {
                Guid id = (mapObject as PolygonViewModel).Id;
                if (id.Equals("cd69a09e-9152-4d72-b0c5-6c6ad938a1a9"))
                {
                    System.Console.WriteLine("true");
                }
            }
            

            var view = this.GetNewView(mapObject);

            this.viewModelView.Add(mapObject, view);
            this.viewViewModel.Add(view, mapObject);
        }

        public IMapObject GetMapObject(IMapObjectViewModel imapObjectViewModel)
        {
            return viewModelView[imapObjectViewModel];
        }

        public int test()
        {
            return 25;
        }

        /// <summary>
        /// Убирает с карты дополнительный слой с заданным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        public override void RemoveLayer(Guid id)
        {
            this.Map.RemoveLayer(id);
        }

        /// <summary>
        /// Сбрасывает настройки надписи.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public override void ResetLabel(IMapObjectViewModel obj)
        {
            var labeledObj = this.viewModelView[obj] as ILabeledObject;

            if (labeledObj != null)
                labeledObj.OnLabelChanged(null, null, labeledObj.Label.DefaultSize);
        }

        /// <summary>
        /// Задает значение представлению группы.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetGroupViewValue(GroupViewModel source, string sourcePropertyName, object value)
        {
            if (sourcePropertyName == nameof(GroupViewModel.IsHighlighted))
                this.Animator.SetAnimated(source, (bool)value);
        }

        /// <summary>
        /// Задает значение представлению слоя.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetLayerViewValue(LayerViewModel source, string sourcePropertyName, object value)
        {
            switch (sourcePropertyName)
            {
                case nameof(source.IsHighlighted):
                    this.Animator.SetAnimated(source, (bool)value);

                    return;

                case nameof(source.IsVisible):
                    this.Map.SetLayerVisibility(source.Type, (bool)value);

                    return;

                case nameof(source.Opacity):
                    this.Map.SetLayerOpacity(source.Type, (double)value);

                    return;
            }
        }

        /// <summary>
        /// Задает значение модели представления объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetMapObjectViewModelValue(IMapObject source, string sourcePropertyName, object value)
        {
            if (!this.viewViewModel.ContainsKey(source))
                return;

            if (source as IInteractiveShape != null)
                if (sourcePropertyName == nameof(IEditableObject.IsEditing))
                {
                    (this.viewViewModel[source] as IEditableObjectViewModel).IsEditing = (bool)value;

                    return;
                }

            if (source as InteractiveLine != null)
            {
                var line = this.viewViewModel[source] as LineViewModel;

                switch (sourcePropertyName)
                {
                    case nameof(InteractiveLine.LeftPoint):
                        line.SetLeftPoint((Point)value);

                        return;

                    case nameof(InteractiveLine.Points):
                        (this.viewViewModel[source] as LineViewModel).Points = value.ToString();

                        return;

                    case nameof(InteractiveLine.RightPoint):
                        line.SetRightPoint((Point)value);

                        return;
                }
            }

            if (source as InteractiveNode != null)
                if (sourcePropertyName == nameof(InteractiveNode.LeftTopCorner))
                {
                    (this.viewViewModel[source] as NodeViewModel).SetValue(nameof(NodeViewModel.LeftTopCorner), value);

                    return;
                }

            if (source as InteractivePolygon != null)
                if (sourcePropertyName == nameof(InteractivePolygon.Points))
                {
                    (this.viewViewModel[source] as PolygonViewModel).Points = value.ToString();

                    return;
                }

            if (source as InteractiveFigure != null)
            {
                var figure = this.viewViewModel[source] as FigureViewModel;

                switch (sourcePropertyName)
                {
                    case nameof(InteractiveFigure.Angle):
                        figure.SetValue(nameof(FigureViewModel.Angle), value);

                        return;

                    case nameof(InteractiveFigure.LeftTopCorner):
                        figure.SetValue(nameof(FigureViewModel.Position), value);

                        return;

                    case nameof(InteractiveFigure.Size):
                        figure.SetValue(nameof(FigureViewModel.Size), value);

                        return;
                }
            }

            if (source as IInteractiveShape != null)
                if (sourcePropertyName == nameof(IInteractiveShape.IsSelected))
                {
                    (this.viewViewModel[source] as ISelectableObjectViewModel).IsSelected = (bool)value;

                    return;
                }

            if (source as Ruler != null)
            {
                var ruler = this.viewViewModel[source] as RulerViewModel;

                switch (sourcePropertyName)
                {
                    case nameof(Ruler.EndPoint):
                        ruler.EndPoint = (Point)value;

                        return;

                    case nameof(Ruler.StartPoint):
                        ruler.StartPoint = (Point)value;

                        return;
                }
            }

            if (source as SmartIndependentLabel != null)
            {
                var label = this.viewViewModel[source] as LabelViewModel;

                switch (sourcePropertyName)
                {
                    case nameof(SmartIndependentLabel.Angle):
                        label.Angle = (double)value;

                        return;

                    case nameof(SmartIndependentLabel.InitialOffset):
                        var offset = (Point)value;

                        label.SetValue(nameof(LabelViewModel.Position), new Point(label.Position.X + offset.X, label.Position.Y + offset.Y));

                        return;

                    case nameof(SmartIndependentLabel.LeftTopCorner):
                        label.Position = (Point)value;

                        return;

                    case nameof(SmartIndependentLabel.Size):
                        label.Size = (int)value;

                        return;
                }
            }
        }

        /// <summary>
        /// Задает значение представлению объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetMapObjectViewValue(IMapObjectViewModel source, string sourcePropertyName, object value)
        {
            if (!this.viewModelView.ContainsKey(source))
                return;
            if (sourcePropertyName.Equals("IsDisabledVisible"))
            {
                var mapObject = this.viewModelView[source];
                if (source as FigureViewModel != null)
                {
                    //if (!(source as FigureViewModel).IsActive)
                    //return;

                    if ((bool)value)
                        mapObject.AddToCanvas(this.Map.GetCanvas((mapObject as InteractiveFigure).MajorPoint, 0, (source as FigureViewModel).Type));
                    else
                        mapObject.RemoveFromCanvas();

                    return;
                }
                return;
            }
            if (source as IEditableObjectViewModel != null)
                if (sourcePropertyName == nameof(IEditableObjectViewModel.IsEditing))
                {
                    (this.viewModelView[source] as IEditableObject).IsEditing = (bool)value;

                    return;
                }

            if (source as IHighlightableObjectViewModel != null)
                if (sourcePropertyName == nameof(IHighlightableObjectViewModel.IsHighlighted))
                {
                    this.Animator.SetAnimated(source as IHighlightableObjectViewModel, (bool)value);

                    return;
                }

            if (source as ISelectableObjectViewModel != null)
                if (sourcePropertyName == nameof(ISelectableObjectViewModel.IsSelected))
                {
                    (this.viewModelView[source] as IInteractiveShape).IsSelected = (bool)value;

                    return;
                }

            if (source as BadgeViewModel != null)
                if (sourcePropertyName == nameof(BadgeViewModel.Distance))
                    (this.viewModelView[source] as InteractiveBadge).RelocateBadge();

            if (source as FigureViewModel != null)
            {
                var figure = this.viewModelView[source] as InteractiveFigure;

                switch (sourcePropertyName)
                {
                    case nameof(FigureViewModel.Angle):
                        figure.Angle = (double)value;

                        return;

                    case nameof(FigureViewModel.IsDrawing):
                        if ((bool)value)
                            figure.HideBackground();
                        else
                            figure.ShowBackground();

                        return;

                    case nameof(FigureViewModel.IsActive):
                        if ((source as FigureViewModel).ChildrenTypes.Count > 0)
                            figure.ChangeColor(this.GetGradientBrush(source as FigureViewModel), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness));
                        else
                            if ((bool)value)
                                figure.ChangeColor(this.GetBrush((source as FigureViewModel).Type.Color), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness));
                            else
                                figure.ChangeColor(this.GetBrush((source as FigureViewModel).Type.InactiveColor), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness));

                        return;

                    case nameof(FigureViewModel.IsPlanning):
                        figure.ChangeBorders((bool)value);

                        return;

                    case nameof(FigureViewModel.LabelAngle):
                        figure.Label.SetAngle((double?)value);

                        return;

                    case nameof(FigureViewModel.LabelPosition):
                        figure.Label.SetPosition((Point?)value);

                        return;

                    case nameof(FigureViewModel.LabelSize):
                        figure.Label.SetSize((int?)value);

                        return;

                    case nameof(FigureViewModel.Position):
                        figure.LeftTopCorner = (Point)value;

                        return;

                    case nameof(FigureViewModel.Size):
                        var tuple = (Tuple<Size, Point>)value;

                        figure.Size = tuple.Item1;
                        figure.LeftTopCorner = tuple.Item2;

                        return;

                    case nameof(FigureViewModel.Type):
                        figure.ChangeColor(this.GetBrush(((ObjectType)value).Color), this.GetPen(this.figureBoxColor, this.MapSettingService.FigureSelectedThickness));

                        return;
                }
            }

            if (source as LabelViewModel != null)
            {
                var label = this.viewModelView[source] as SmartIndependentLabel;

                switch (sourcePropertyName)
                {
                    case nameof(LabelViewModel.Angle):
                        label.SetAngle((double)value);

                        return;

                    case nameof(LabelViewModel.Content):
                        label.Text = (string)value;

                        return;

                    case nameof(LabelViewModel.IsBold):
                        label.IsBold = (bool)value;

                        return;

                    case nameof(LabelViewModel.IsItalic):
                        label.IsItalic = (bool)value;

                        return;

                    case nameof(LabelViewModel.IsUnderline):
                        label.IsUnderline = (bool)value;

                        return;

                    case nameof(LabelViewModel.Position):
                        label.SetPosition((Point)value);

                        return;

                    case nameof(LabelViewModel.Size):
                        label.SetSize((int)value);

                        return;
                }
            }

            if (source as LineViewModel != null)
            {
                var line = this.viewModelView[source] as InteractiveLine;

                switch (sourcePropertyName)
                {
                    case nameof(LineViewModel.Diameter):
                        line.UpdateLabels();

                        break;

                    case nameof(LineViewModel.ForcedSegments):
                        line.UpdateLabels();

                        break;

                    case nameof(FigureViewModel.IsActive):
                        if ((bool)value)
                            line.ChangeColor(this.GetBrush((source as LineViewModel).Type.Color), this.GetPen((source as LineViewModel).Type.Color, this.MapSettingService.LineSelectedThickness));
                        else
                            line.ChangeColor(this.GetBrush((source as LineViewModel).Type.InactiveColor), this.GetPen((source as LineViewModel).Type.InactiveColor, this.MapSettingService.LineSelectedThickness));

                        return;

                    case nameof(LineViewModel.IsPlanning):
                        line.SetLineStyle(this.GetLineStyle((bool)value));

                        return;

#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
                    //case nameof(LineViewModel.IsWorking):
                    //    line.SetLineStyle(this.GetLineStyle((source as LineViewModel).IsPlanning, (bool)value));

                    //    return;

                    case nameof(LineViewModel.LabelOffset):
                        foreach (var label in line.Labels)
                        {
                            label.Offset = (double)value;

                            label.Relocate(new Point());
                        }

                        return;

                    case nameof(LineViewModel.LabelAngles):
                        var labelAngles = value as Dictionary<int, double>;

                        for (int i = 0; i < line.Labels.Count; i++)
                        {
                            if (labelAngles.ContainsKey(i))
                                line.Labels[i].SetAngle(labelAngles[i]);
                            else
                                line.Labels[i].SetAngle(null);

                            line.Labels[i].Relocate(new Point());
                        }

                        return;
                        
                    case nameof(LineViewModel.LabelPositions):
                        var labelPositions = value as Dictionary<int, Utilities.Point>;

                        for (int i = 0; i < line.Labels.Count; i++)
                        {
                            if (labelPositions.ContainsKey(i))
                                line.Labels[i].SetPosition(new Point(labelPositions[i].X, labelPositions[i].Y));
                            else
                                line.Labels[i].SetPosition(null);

                            line.Labels[i].Relocate(new Point());
                        }

                        return;

                    case nameof(LineViewModel.LabelSizes):
                        var labelSizes = value as Dictionary<int, int>;

                        for (int i = 0; i < line.Labels.Count; i++)
                        {
                            if (labelSizes.ContainsKey(i))
                                line.Labels[i].SetSize(labelSizes[i]);
                            else
                                line.Labels[i].SetSize(null);

                            line.Labels[i].Relocate(new Point());
                        }

                        return;

                    case nameof(LineViewModel.LeftPoint):
                        line.LeftPoint = (Point)value;

                        return;

                    case nameof(LineViewModel.Length):
                        line.RelocateBadges();

                        return;

                    case nameof(LineViewModel.Points):
                        (this.viewModelView[source] as InteractiveLine).Points = value.ToString();

                        return;

                    case nameof(LineViewModel.RightPoint):
                        line.RightPoint = (Point)value;

                        return;

                    case nameof(LineViewModel.ShowLabels):
                        line.CanShowLabels = (bool)value;

                        return;
                }
            }

            if (source as NodeViewModel != null)
            {
                var node = this.viewModelView[source] as InteractiveNode;

                switch (sourcePropertyName)
                {
                    case nameof(NodeViewModel.ConnectedObject):
                        if (value != null)
                            node.ConnectedFigure = (InteractiveFigure)this.viewModelView[(source as NodeViewModel).ConnectedObject];
                        else
                            node.ConnectedFigure = null;

                        return;

                    case nameof(NodeViewModel.IsConnectionsChanged):
                        var nodeViewModel = source as NodeViewModel;

                        node.StrokeBrush = this.GetBrush(nodeViewModel.ConnectedLinesType.Color);

                        node.IsConnectedToNode = nodeViewModel.ConnectionCount > 1;

                        return;

                    case nameof(NodeViewModel.LeftTopCorner):
                        node.LeftTopCorner = (Point)value;

                        return;
                }
            }

            if (source as PolygonViewModel != null)
                if (sourcePropertyName == nameof(PolygonViewModel.Points))
                {
                    (this.viewModelView[source] as InteractivePolygon).Points = value.ToString();

                    return;
                }

            if (source as IObjectViewModel != null)
                if (sourcePropertyName == nameof(IObjectViewModel.IsInitialized))
                {
                    (this.viewModelView[source] as IInteractiveShape).IsInitialized = (bool)value;

                    return;
                }

            if (sourcePropertyName == nameof(INamedObjectModel.Name))
                (this.viewModelView[source] as ILabeledObject).Label.SetText((source as INamedObjectViewModel).Name);

            if (sourcePropertyName == nameof(IMapObjectViewModel.IsPlaced))
            {
                var mapObject = this.viewModelView[source];

                var namedObject = mapObject as ILabeledObject;

                if (namedObject != null)
                    if ((bool)value)
                    {
                        if (namedObject.Label.Canvas == null)
                        {
                            namedObject.Label.Canvas = this.Map.GetCanvas((mapObject as IInteractiveShape).MajorPoint, 4, (source as ITypedObjectViewModel).Type);
                            if ( namedObject.Label.Canvas != null ) 
                                namedObject.Label.AddToCanvas(namedObject.Label.Canvas);
                        }
                    }
                    else
                        if (namedObject.Label.Canvas != null)
                            namedObject.Label.RemoveFromCanvas();

                if (source as ApprovedHeaderViewModel != null)
                    if ((bool)value)
                    {
                        (mapObject as ApprovedHeader).Canvas = this.Map.GetDefaultCanvas();

                        mapObject.AddToCanvas(mapObject.Canvas);
                    }
                    else
                        mapObject.RemoveFromCanvas();

                if (source as BadgeViewModel != null)
                {
                    if ((bool)value)
                        mapObject.AddToCanvas(this.Map.GetCanvas((mapObject as InteractiveBadge).MajorPoint, 2, (source as BadgeViewModel).Parent.Type));
                    else
                        mapObject.RemoveFromCanvas();

                    return;
                }
                //mstsc
                if (source as FigureViewModel != null )
                {
                    //if (!(source as FigureViewModel).IsActive)
                        //return;

                    if ((bool)value)
                        mapObject.AddToCanvas(this.Map.GetCanvas((mapObject as InteractiveFigure).MajorPoint, 0, (source as FigureViewModel).Type));
                    else
                        mapObject.RemoveFromCanvas();

                    return;
                }

                if (source as LabelViewModel != null)
                    if ((bool)value)
                    {
                        (mapObject as SmartIndependentLabel).Canvas = this.Map.GetDefaultCanvas();

                        mapObject.AddToCanvas(mapObject.Canvas);
                    }
                    else
                        mapObject.RemoveFromCanvas();

                if (source as LengthPerDiameterTableViewModel != null)
                    if ((bool)value)
                    {
                        (mapObject as LengthPerDiameterTable).Canvas = this.Map.GetDefaultCanvas();

                        mapObject.AddToCanvas(mapObject.Canvas);
                    }
                    else
                        mapObject.RemoveFromCanvas();

                if (source as LineViewModel != null)
                {
                    var line = mapObject as InteractiveLine;

                    if ((bool)value)
                    {
                        foreach (var label in line.Labels)
                            if (label.Canvas == null)
                            {
                                label.Canvas = this.Map.GetCanvas(line.MajorPoint, 4, (source as ITypedObjectViewModel).Type);

                                label.AddToCanvas(label.Canvas);
                            }

                        mapObject.AddToCanvas(this.Map.GetCanvas(line.MajorPoint, 1, (source as LineViewModel).Type));
                    }
                    else
                    {
                        foreach (var label in line.Labels)
                            if (label.Canvas != null)
                                label.RemoveFromCanvas();

                        mapObject.RemoveFromCanvas();

                        foreach (var badge in (mapObject as InteractiveLine).Badges)
                            badge.RemoveFromCanvas();
                    }

                    return;
                }

                if (source as NodeViewModel != null)
                {
                    if ((bool)value)
                        mapObject.AddToCanvas(this.Map.GetCanvas((mapObject as InteractiveNode).MajorPoint, 3, (source as NodeViewModel).ConnectedLinesType));
                    else
                        mapObject.RemoveFromCanvas();

                    return;
                }

                if (source as PolylineViewModel != null || source as RulerViewModel != null || source as NewRulerViewModel != null)
                    if ((bool)value)
                        mapObject.AddToCanvas(this.Map.GetDefaultCanvas());
                    else
                        mapObject.RemoveFromCanvas();
            }
        }

        /// <summary>
        /// Начинает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        public override void StartAnimation(IMapObject mapObject)
        {
            this.Animator.SetAnimated(this.GetMapObjectViewModel(mapObject) as IHighlightableObjectViewModel, true);
        }

        /// <summary>
        /// Заканчивает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        public override void StopAnimation(IMapObject mapObject)
        {
            this.Animator.SetAnimated(this.GetMapObjectViewModel(mapObject) as IHighlightableObjectViewModel, false);
        }

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryDecreaseLabelSize(LineViewModel line, int index)
        {
            int? result = null;

            result = this.TryDecreaseLabelSize((this.viewModelView[line] as InteractiveLine).Labels[index]);

            return result;
        }

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryDecreaseLabelSize(IMapObjectViewModel obj)
        {
            int? result = null;

            if (obj is LineViewModel)
                foreach (var label in (this.viewModelView[obj] as InteractiveLine).Labels)
                    result = this.TryDecreaseLabelSize(label);
            else
                result = this.TryDecreaseLabelSize((this.viewModelView[obj] as ILabeledObject).Label);

            return result;
        }

        /// <summary>
        /// Пробует увеличить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="obj">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryIncreaseLabelSize(LineViewModel line, int index)
        {
            int? result = null;

            result = this.TryIncreaseLabelSize((this.viewModelView[line] as InteractiveLine).Labels[index]);

            return result;
        }

        /// <summary>
        /// Пробует увеличить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryIncreaseLabelSize(IMapObjectViewModel obj)
        {
            int? result = null;

            if (obj is LineViewModel)
                foreach (var label in (this.viewModelView[obj] as InteractiveLine).Labels)
                    result = this.TryIncreaseLabelSize(label);
            else
                result = this.TryIncreaseLabelSize((this.viewModelView[obj] as ILabeledObject).Label);

            return result;
        }

        /// <summary>
        /// Убирает связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        public override void UnregisterBinding(IMapObjectViewModel obj)
        {
            var view = this.viewModelView[obj];

            this.viewModelView.Remove(obj);
            this.viewViewModel.Remove(view);
        }

        /// <summary>
        /// Убирает связи между моделями представлений объектов карты и их представлениями.
        /// </summary>
        public override void UnregisterBindings()
        {
            this.viewModelView.Clear();
            this.viewViewModel.Clear();
        }

        /// <summary>
        /// Обновляет размер шрифта надписей фигур по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public override void UpdateFigureLabelDefaultSize(int size, int prevSize)
        {
            foreach (var figure in this.GetFigures())
            {
                var initialSize = (this.viewViewModel[figure] as FigureViewModel).LabelSize;

                if (initialSize.HasValue)
                    (this.viewViewModel[figure] as FigureViewModel).LabelSize = initialSize.Value - prevSize + size;
                else
                    (this.viewViewModel[figure] as FigureViewModel).LabelSize = size;
            }
        }

        /// <summary>
        /// Обновляет отступ внутри границы планируемых фигур.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public override void UpdateFigurePlanningOffset(double offset)
        {
            foreach (var figure in this.GetFigures())
                figure.SetPlanningOffset(offset);
        }

        /// <summary>
        /// Обновляет толщину границы фигур.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        public override void UpdateFigureThickness(double thickness)
        {
            foreach (var figure in this.GetFigures())
                figure.SetThickness(thickness);
        }

        /// <summary>
        /// Обновляет размер шрифта независимых надписей по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public override void UpdateIndependentLabelDefaultSize(int size, int prevSize)
        {
#warning Тут нужно дописать код
        }

        /// <summary>
        /// Обновляет отступ внутри отключенных линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public override void UpdateLineDisabledOffset(double offset)
        {
            foreach (var line in this.GetLines())
                line.SetDisabledOffset(offset);
        }

        /// <summary>
        /// Обновляет размер шрифта надписей линий по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public override void UpdateLineLabelDefaultSize(int size, int prevSize)
        {
            foreach (var line in this.GetLines())
                (this.viewViewModel[line] as LineViewModel).ChangeLabelDefaultSize(size, prevSize);
        }

        /// <summary>
        /// Обновляет надписи линии.
        /// </summary>
        /// <param name="line">Линия.</param>
        public override void UpdateLineLabels(LineViewModel line)
        {
            (this.viewModelView[line] as InteractiveLine).UpdateLabels();
        }

        /// <summary>
        /// Обновляет отступ внутри планируемых линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public override void UpdateLinePlanningOffset(double offset)
        {
            foreach (var line in this.GetLines())
                line.SetPlanningOffset(offset);
        }

        /// <summary>
        /// Обновляет толщину линий.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        /// <param name="prevThickness">Предыдущая толщина.</param>
        public override void UpdateLineThickness(double thickness, double prevThickness)
        {
            foreach (var line in this.GetLines())
            {
                line.SetThickness(thickness);

                var offset = (this.viewViewModel[line] as LineViewModel).LabelOffset;

                // Также нужно пересчитать оффсет надписей.
                (this.viewViewModel[line] as LineViewModel).LabelOffset = offset / prevThickness * thickness;
            }
        }

        #endregion
    }
}