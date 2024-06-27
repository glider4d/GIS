using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления узла.
    /// </summary>
    [Serializable]
    internal sealed partial class NodeViewModel : ObjectViewModel, IEditableObjectViewModel, IHighlightableObjectViewModel, IMapObjectViewModel, ISelectableObjectViewModel, ISetterIgnorer
    {
        #region Закрытые поля

        /// <summary>
        /// Подключенный к узлу объект.
        /// </summary>
        private FigureViewModel connectedObject;

        /// <summary>
        /// Значение, указывающее на то, что имеются ли сломанные данные соединения с узлом.
        /// </summary>
        private bool hasBrokenConnData;

        /// <summary>
        /// Значение, указывающее на то, что изменились ли данные о подключенном объекте.
        /// </summary>
        private bool isConnectedObjectDataChanged;

        /// <summary>
        /// Значение, указывающее на то, что изменились ли соединения с узлом.
        /// </summary>
        private bool isConnectionsChanged;

        /// <summary>
        /// Значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        private bool isEditing;

        /// <summary>
        /// Значение, указывающее на то, что был ли изменен идентификатор объекта.
        /// </summary>
        private bool isIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что инициализирован ли объект.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Значение, указывающее на то, что начато ли сохранение объекта.
        /// </summary>
        private bool isSaveStarted;

        /// <summary>
        /// Значение, указывающее на то, что выбран ли объект.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Соединения с узлом.
        /// </summary>
        private readonly Dictionary<LineViewModel, NodeConnection> connections = new Dictionary<LineViewModel, NodeConnection>();

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Узел.
        /// </summary>
        private readonly NodeModel node;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NodeViewModel"/>.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public NodeViewModel(NodeModel node, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(node, accessService, dataService, historyService, messageService)
        {
            this.node = node;
            this.layerHolder = layerHolder;
            this.mapBindingService = mapBindingService;

            this.IsModified = true;

            this.RegisterBinding();

            this.DeleteCommand = new RelayCommand(this.ExecuteDelete, this.CanExecuteDelete);
            this.DisconnectCommand = new RelayCommand(this.ExecuteDisconnect, this.CanExecuteDisconnect);
            this.UnpinCommand = new RelayCommand(this.ExecuteUnpin, this.CanExecuteUnpin);
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> линии.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Line_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var line = sender as LineViewModel;

            switch (e.PropertyName)
            {
                case nameof(LineViewModel.LeftPoint):
                    if (this.connections[line].ConnectionSide == NodeConnectionSide.Left && this.LeftTopCorner != line.LeftPoint)
                        this.SetValue(nameof(this.LeftTopCorner), line.LeftPoint);

                    break;

                case nameof(LineViewModel.RightPoint):
                    if (this.connections[line].ConnectionSide == NodeConnectionSide.Right && this.LeftTopCorner != line.RightPoint)
                        this.SetValue(nameof(this.LeftTopCorner), line.RightPoint);

                    break;
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает присоединенные к узлу линии.
        /// </summary>
        public ICollection<LineViewModel> ConnectedLines
        {
            get
            {
                return this.connections.Keys;
            }
        }

        /// <summary>
        /// Возвращает или задает тип линий, присоединенных к узлу.
        /// </summary>
        public ObjectType ConnectedLinesType
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает подключеный к узлу объект.
        /// </summary>
        public FigureViewModel ConnectedObject
        {
            get
            {
                return this.connectedObject;
            }
            set
            {
                this.connectedObject = value;

                this.mapBindingService.SetMapObjectViewValue(this, nameof(this.ConnectedObject), value);

                if (value != null)
                    this.node.ConnectedObjectData = new Tuple<Guid, ObjectType, bool>(value.Id, value.Type, value.IsPlanning);
                else
                    this.node.ConnectedObjectData = null;
            }
        }
        
        /// <summary>
        /// Возвращает количество соединений с узлом.
        /// </summary>
        public int ConnectionCount
        {
            get
            {
                return this.connections.Count;
            }
        }

        /// <summary>
        /// Возвращает команду удаления узла.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get;
        }
        
        /// <summary>
        /// Возвращает команду отсоединения узла от другого узла.
        /// </summary>
        public RelayCommand DisconnectCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что стоит ли игнорировать прикрепление узла к границе родительской фигуры.
        /// </summary>
        public bool IgnoreStick
        {
            get
            {
                return this.node.IgnoreStick;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IgnoreStick), this.IgnoreStick, value);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение игнорирования автоопределения положения узла"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что изменились ли соединения с узлом.
        /// </summary>
        public bool IsConnectionsChanged
        {
            get
            {
                return this.isConnectionsChanged;
            }
            private set
            {
                this.isConnectionsChanged = value;

                this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsConnectionsChanged), value);
            }
        }

        /// <summary>
        /// Возвращает или задает положение левого верхнего угла узла.
        /// </summary>
        public Point LeftTopCorner
        {
            get
            {
                return new Point(this.node.Position.X, this.node.Position.Y);
            }
            set
            {
                if (this.LeftTopCorner != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.LeftTopCorner), this.LeftTopCorner, value);
                    this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение положения узла"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает команду открепления узла от объекта.
        /// </summary>
        public RelayCommand UnpinCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить узел.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDelete()
        {
            if (this.ConnectionCount != 2)
                return false;

            foreach (var conn in this.connections)
            {
                if (!conn.Key.IsPlanning)
                    // Все линии должны быть планируемыми. Если есть хотя бы одна фактическая линия, то узел нельзя будет удалить.
                    return false;

                if (conn.Key.ChildrenLayers.Any(x => x.ObjectCount > 0))
                    // Также линии не должны содержать дочерние объекты.
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли отсоединить узел от другого узла.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDisconnect()
        {
            return this.ConnectionCount > 1;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить открепление узла от объекта.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteUnpin()
        {
            return this.ConnectedObject != null;
        }

        /// <summary>
        /// Проверяет столкновение узла с другими объектами.
        /// </summary>
        /// <param name="mode">Режим поиска коллизии.</param>
        /// <param name="radius">Радиус поиска.</param>
        /// <returns>Объект, с которым столкнулся узел.</returns>
        private IMapObjectViewModel CheckCollision(CollisionSearchMode mode = CollisionSearchMode.First, double radius = 0.5)
        {
            return this.mapBindingService.CheckCollision(this, mode, radius);
        }

        /// <summary>
        /// Удаляет все соединения с узлом.
        /// </summary>
        private void ClearConnectionData()
        {
            foreach (var conn in this.connections.Values)
            {
                switch (conn.ConnectionSide)
                {
                    case NodeConnectionSide.Left:
                        conn.Line.LeftNode = null;

                        break;

                    case NodeConnectionSide.Right:
                        conn.Line.RightNode = null;

                        break;
                }

                conn.Line.PropertyChanged -= this.Line_PropertyChanged;
            }

            this.connections.Clear();

            if (this.IsInitialized)
            {
                this.IsModified = true;

                this.IsConnectionsChanged = true;
            }
        }

        /// <summary>
        /// Соединяет узел с заданной фигурой.
        /// </summary>
        /// <param name="figure">Фигура, с которой нужно соединить данный узел.</param>
        /// <param name="prevValue">Предыдущее значение положения левого верхнего угла узла.</param>
        private void ConnectWithFigure(FigureViewModel figure, Point prevValue)
        {
            ConnectToFigureAction action = null;

            var param = figure.Type.Parameters.FirstOrDefault(x => x.Alias == Alias.BoilerId);

            if (param != null)
            {
                // Получаем самую первую линию, присоединенную к узлу. Нам нужно будет вытащить из нее значение параметра, отвечающего за принадлежность к котельной.
                var line = this.ConnectedLines.First();

                var boilerId = line.GetBoilerId();

                if (boilerId != Guid.Empty)
                {
                    // Находим идентификатор котельной, к которой раньше была подключен фигура.
                    var oldBoilerId = figure.GetBoilerId();
                    
                    action = new ConnectToFigureAction(this, this.ConnectedObject, figure, oldBoilerId, boilerId);
                }
                else
                    action = new ConnectToFigureAction(this, this.ConnectedObject, figure);
            }
            else
                action = new ConnectToFigureAction(this, this.ConnectedObject, figure);

            // Выполняем действие присоединения узла к фигуре.
            action.Do();

            // Имитируем изменение положение левого верхнего угла.
            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LeftTopCorner), this.LeftTopCorner);

            if (!Equals(prevValue, this.LeftTopCorner))
                // Запоминаем изменение положения в истории изменений.
                this.HistoryService.Add(new HistoryEntry(new SetPropertyAction(this, nameof(this.LeftTopCorner), prevValue, this.LeftTopCorner), Target.Data, "изменение положения узла"));

            // Запоминаем присоединение узла к фигуре в истории изменений.
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "присоединение узла к фигуре"));
        }

        /// <summary>
        /// Соединяет узел с заданной линией.
        /// </summary>
        /// <param name="line">Линия, с которой нужно соединить данный узел.</param>
        /// <param name="prevValue">Предыдущее значение положения левого верхнего угла узла.</param>
        private void ConnectWithLine(LineViewModel line, Point prevValue)
        {
            // Находим отрезок линии, с которой нужно соединить узел.
            var segment = line.GetNearestSegment(this.LeftTopCorner);
            var leftPoint = segment.Item1;
            var rightPoint = segment.Item2;

            // Находим точку пересечения прямых, представляемых попавшейся под узел линией и первой соединенной с узлом линией.
            Point intersectPoint;
            // Находим коэффициенты A, B и C прямых.
            double a1 = 0;
            double b1 = 0;
            double c1 = 0;
            double a2 = 0;
            double b2 = 0;
            double c2 = 0;
            this.GetABC(leftPoint, rightPoint, out a1, out b1, out c1);
            // Первая соединенная с узлом линия.
            var firstConnData = this.connections.First();
            var allPoints = firstConnData.Key.GetAllPoints();
            this.GetABC(this.LeftTopCorner, firstConnData.Value.ConnectionSide == NodeConnectionSide.Left ? allPoints[1] : allPoints[allPoints.Count - 2], out a2, out b2, out c2);
            var d = this.GetDeterminant(a1, a2, b1, b2);
            var d1 = this.GetDeterminant(c1, c2, b1, b2);
            var d2 = this.GetDeterminant(a1, a2, c1, c2);
            intersectPoint = new Point(d1 / d, d2 / d);

            // Сдвигаем узел на линию.
            this.LeftTopCorner = intersectPoint;

            // Запоминаем предыдущее состояние точек изгиба делимой линии.
            var originalPoints = line.Points;

            // Получаем правую часть линии.
            var newLine = line.Divide(this.LeftTopCorner);

            if (newLine != null)
            {
                this.AddConnection(new NodeConnection(line, NodeConnectionSide.Right));
                this.AddConnection(new NodeConnection(newLine, NodeConnectionSide.Left));

                if (!Equals(prevValue, this.LeftTopCorner))
                    // Запоминаем изменение положения в истории изменений.
                    this.HistoryService.Add(new HistoryEntry(new SetPropertyAction(this, nameof(this.LeftTopCorner), prevValue, this.LeftTopCorner), Target.Data, "изменение положения узла"));

                // Запоминаем деление линии в истории изменений.
                this.HistoryService.Add(new HistoryEntry(new DivideLineAction(line, originalPoints, this.layerHolder, newLine, this, line.Points, newLine.Points, false), Target.Data, "разделение узлом линии"));
            }
        }

        /// <summary>
        /// Соединяет узел с заданным узлом.
        /// </summary>
        /// <param name="node">Узел, с которым нужно соединить данный узел.</param>
        /// <param name="prevValue">Предыдущее значение положения левого верхнего угла узла.</param>
        private void ConnectWithNode(NodeViewModel node, Point prevValue)
        {
            var targetLine = node.ConnectedLines.First();

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new SetPropertyAction(this, nameof(this.LeftTopCorner), prevValue, node.LeftTopCorner);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение положения узла"));
            action.Do();

            // Замещаем узел и запоминаем это в истории изменений.
            var connection = this.ReplaceWith(node);
            this.HistoryService.Add(new HistoryEntry(new ReplaceNodeAction(this, node, connection, this.layerHolder), Target.Data, "присоединение узла к узлу"));
        }

        /// <summary>
        /// Выполняет удаление узла.
        /// </summary>
        private void ExecuteDelete()
        {
            var firstConn = this.connections.ElementAt(0);
            var secondConn = this.connections.ElementAt(1);

            var first = firstConn.Key;
            var second = secondConn.Key;

            // Составляем новый общий набор точек изгиба линии.
            var firstPoints = LineViewModel.GetPoints(first.Points);
            var secondPoints = LineViewModel.GetPoints(second.Points);
            var bendPoints = new List<Point>();
            if (firstConn.Value.ConnectionSide == NodeConnectionSide.Left)
                if (secondConn.Value.ConnectionSide == NodeConnectionSide.Left)
                {
                    bendPoints.AddRange(secondPoints.Reverse());
                    bendPoints.Add(new Point(this.LeftTopCorner.X, this.LeftTopCorner.Y));
                    bendPoints.AddRange(firstPoints);
                }
                else
                {
                    bendPoints.AddRange(secondPoints);
                    bendPoints.Add(new Point(this.LeftTopCorner.X, this.LeftTopCorner.Y));
                    bendPoints.AddRange(firstPoints);
                }
            else
                if (secondConn.Value.ConnectionSide == NodeConnectionSide.Left)
                {
                    bendPoints.AddRange(firstPoints);
                    bendPoints.Add(new Point(this.LeftTopCorner.X, this.LeftTopCorner.Y));
                    bendPoints.AddRange(secondPoints);
                }
                else
                {
                    bendPoints.AddRange(firstPoints);
                    bendPoints.Add(new Point(this.LeftTopCorner.X, this.LeftTopCorner.Y));
                    bendPoints.AddRange(secondPoints.Reverse());
                }

            // Суммируем длины линий.
            var oldLength = first.Length;
            first.UpdateLength(first.Length + second.Length);

            // Получаем недостающие свойства для первой линии.
            var parameters = first.MergeParameterValues(second);

            // Открепляем первую линию от узла.
            this.RemoveConnection(first);

            NodeViewModel newNode = null;

            if (secondConn.Value.ConnectionSide == NodeConnectionSide.Left)
            {
                newNode = second.RightNode;

                // Прикрепляем первую линии к правому узлу второй линии.
                second.RightNode.AddConnection(new NodeConnection(first, firstConn.Value.ConnectionSide));
                // И открепляем от него вторую линию.
                second.RightNode.RemoveConnection(second);
            }
            else
            {
                newNode = second.LeftNode;

                // Прикрепляем первую линии к левому узлу второй линии.
                second.LeftNode.AddConnection(new NodeConnection(first, firstConn.Value.ConnectionSide));
                // И открепляем от него вторую линию.
                second.LeftNode.RemoveConnection(second);
            }

            // Заменяем точки изгиба первой линии.
            var prevPoints = first.Points;
            first.Points = new PointCollection(bendPoints).ToString();

            // Убираем с карты вторую линию и узел.
            this.layerHolder.RemoveObject(second);
            this.layerHolder.RemoveObject(this);
            // И помечаем вторую линию к удалению.
            this.layerHolder.MarkToDelete(second);
            // А узел - к обновлению.
            this.layerHolder.MarkToUpdate(this);
            
            // Запоминаем удаление узла в истории изменений.
            this.HistoryService.Add(new HistoryEntry(new DeleteNodeAction(first, second, this, oldLength, prevPoints, first.Points, parameters, firstConn.Value.ConnectionSide, newNode, secondConn.Value.ConnectionSide == NodeConnectionSide.Left ? NodeConnectionSide.Right : NodeConnectionSide.Left, this.layerHolder), Target.Data, "удаление узла"));
        }

        /// <summary>
        /// Выполняет отсоединение узла от другого узла.
        /// </summary>
        private void ExecuteDisconnect()
        {
            // Подготавливаем узлы.
            var nodes = new List<NodeViewModel>();
            var connections = new List<NodeConnection>();
            var allConnections = this.connections.ToDictionary(x => x.Key, x => x.Value);
            var isFirst = true;
            // Убираем все соединения текущего узла, оставляя только первое.
            var first = this.connections.First();
            this.ClearConnectionData();
            this.AddConnection(first.Value);
            foreach (var connection in allConnections)
            {
                if (isFirst)
                {
                    isFirst = false;

                    continue;
                }

                var model = new NodeModel(ObjectModel.DefaultId, this.CityId, this.Type, new Utilities.Point(this.node.Position.X, this.node.Position.Y), null, null, false);

                var viewModel = new NodeViewModel(model, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };

                viewModel.AddConnection(connection.Value);

                this.layerHolder.AddObject(viewModel);

                nodes.Add(viewModel);
                connections.Add(connection.Value);
            }

            this.HistoryService.Add(new HistoryEntry(new ReplaceNodeAction(nodes, this, connections, this.layerHolder), Target.Data, "отсоединение узлов"));
        }

        /// <summary>
        /// Выполняет открепление узла от объекта.
        /// </summary>
        private void ExecuteUnpin()
        {
            // Запоминаем действие в истории изменений и выполняем его.
            var action = new ConnectToFigureAction(this, this.ConnectedObject, null);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "открепление узла"));
            action.Do();
        }

        /// <summary>
        /// Находит коэффициенты прямой, проходящей через две заданные точки.
        /// </summary>
        /// <param name="first">Первая точка.</param>
        /// <param name="second">Вторая точка.</param>
        /// <param name="a">Коэффициент A.</param>
        /// <param name="b">Коэффициент B.</param>
        /// <param name="c">Коэффициент C.</param>
        private void GetABC(Point first, Point second, out double a, out double b, out double c)
        {
            a = second.Y - first.Y;
            b = first.X - second.X;
            c = -((second.X - first.X) * first.Y - (second.Y - first.Y) * first.X);
        }

        /// <summary>
        /// Возвращает детерминант.
        /// </summary>
        /// <param name="a1">Первый элемент строки.</param>
        /// <param name="a2">Второй элемент строки.</param>
        /// <param name="b1">Первый элемент столбца.</param>
        /// <param name="b2">Второй элемент столбца.</param>
        /// <returns>Детерминант.</returns>
        private double GetDeterminant(double a1, double a2, double b1, double b2)
        {
            return a1 * b2 - a2 * b1;
        }

        /// <summary>
        /// Передвигает присоединенные к узлу линии.
        /// </summary>
        private void MoveConnectedLines()
        {
            foreach (var entry in this.connections.Values)
                switch (entry.ConnectionSide)
                {
                    case NodeConnectionSide.Left:
                        entry.Line.LeftPoint = this.LeftTopCorner;

                        break;

                    case NodeConnectionSide.Right:
                        entry.Line.RightPoint = this.LeftTopCorner;

                        break;
                }
        }

        /// <summary>
        /// Заменяет узел заданным узлом.
        /// </summary>
        /// <param name="node">Замещающий узел.</param>
        /// <returns>Соединение замещаемого узла.</returns>
        private NodeConnection ReplaceWith(NodeViewModel node)
        {
            if (this.ConnectionCount > 1)
                // Если количество соединений с узлом больше одного, то мы не заменяем узел.
                return null;

            // Предполагается, что к узлу подключена по крайней мере одна линия.
            var connection = this.connections.First().Value;

            // Сперва проверяем, имеется ли в подключенных к замещающему узлу линий линия, подключенная к данному узлу.
            if (node.ConnectedLines.Contains(connection.Line))
                return null;

            this.ClearConnectionData();

            this.layerHolder.RemoveObject(this);

            // Если узел уже существует в источнике данных, то помечаем его на обновление.
            if (this.IsSaved)
                this.layerHolder.MarkToUpdate(this);

            node.AddConnection(connection);

            return connection;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет соединение с узлом.
        /// </summary>
        /// <param name="connection">Соединение с узлом.</param>
        public void AddConnection(NodeConnection connection)
        {
            if (connection.Line == null || this.connections.Any(x => x.Key == connection.Line))
            {
                // Ой-ой, мы нашли сломанное соединение с узлом.
                this.hasBrokenConnData = true;

                return;
            }

            this.connections.Add(connection.Line, connection);

            // Сдвигаем конец линии, чтобы он приходился ровно на узел.
            switch (connection.ConnectionSide)
            {
                case NodeConnectionSide.Left:
                    connection.Line.LeftPoint = this.LeftTopCorner;
                    
                    connection.Line.LeftNode = this;

                    break;

                case NodeConnectionSide.Right:
                    connection.Line.RightPoint = this.LeftTopCorner;

                    connection.Line.RightNode = this;

                    break;
            }

            // Отслеживаем изменения положения линии.
            connection.Line.PropertyChanged += this.Line_PropertyChanged;

            if (this.ConnectedLinesType == null)
                this.ConnectedLinesType = connection.Line.Type;

            if (this.IsInitialized)
            {
                this.IsModified = true;

                this.IsConnectionsChanged = true;
            }
        }

        /// <summary>
        /// Присоединяет узел к ближайшему объекту.
        /// </summary>
        /// <param name="searchRadius">Радиус поиска объекта.</param>
        public void ConnectToNearest(double searchRadius)
        {
            var obj = this.CheckCollision(CollisionSearchMode.FirstAndNearest, searchRadius);

            if (obj as NodeViewModel != null)
                this.ConnectWithNode(obj as NodeViewModel, this.LeftTopCorner);
            else
                if (obj as LineViewModel != null)
                    this.ConnectWithLine(obj as LineViewModel, this.LeftTopCorner);
                else
                    if (obj as FigureViewModel != null)
                        this.ConnectWithFigure(obj as FigureViewModel, this.LeftTopCorner);
        }

        /// <summary>
        /// Присоединяет узел к ближайшей фигуре.
        /// </summary>
        /// <param name="searchRadius">Радиус поиска фигуры.</param>
        public void ConnectToNearestFigure(double searchRadius)
        {
            var obj = this.CheckCollision(CollisionSearchMode.FirstAndNearest, searchRadius);

            if (obj is FigureViewModel)
                this.ConnectWithFigure(obj as FigureViewModel, this.LeftTopCorner);
        }

        /// <summary>
        /// Удаляет узел из источника данных.
        /// </summary>
        public void FullDelete()
        {
            this.DataService.NodeAccessService.DeleteNode(this.node, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Уведомляет представление о том, что соединения были изменены.
        /// </summary>
        public void NotifyViewConnectionsChanged()
        {
            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsConnectionsChanged), true);
        }

        /// <summary>
        /// Выполняет действия, связанные со значительным изменением положения левого верхнего угла узла.
        /// </summary>
        /// <param name="prevValue">Предыдущее значение положения левого верхнего угла узла.</param>
        public void OnLeftTopCornerChanged(Point prevValue)
        {
            var obj = this.CheckCollision();

            if (obj as NodeViewModel != null)
                this.ConnectWithNode(obj as NodeViewModel, prevValue);
            else
                if (obj as LineViewModel != null)
                    this.ConnectWithLine(obj as LineViewModel, prevValue);
                else
                    if (obj as FigureViewModel != null)
                        this.ConnectWithFigure(obj as FigureViewModel, prevValue);
                    else
                        // Запоминаем изменение положения в истории изменений.
                        this.HistoryService.Add(new HistoryEntry(new SetPropertyAction(this, nameof(this.LeftTopCorner), prevValue, this.LeftTopCorner), Target.Data, "изменение положения узла"));
        }

        /// <summary>
        /// Удаляет соединение заданной линии с узлом.
        /// </summary>
        /// <param name="line">Линия.</param>
        public void RemoveConnection(LineViewModel line)
        {
            if (!this.connections.ContainsKey(line))
                return;

            switch (this.connections[line].ConnectionSide)
            {
                case NodeConnectionSide.Left:
                    line.LeftNode = null;

                    break;

                case NodeConnectionSide.Right:
                    line.RightNode = null;

                    break;
            }

            this.connections.Remove(line);

            line.PropertyChanged -= this.Line_PropertyChanged;

            if (this.IsInitialized)
            {
                this.IsModified = true;

                this.IsConnectionsChanged = true;
            }
        }

        /// <summary>
        /// Восстанавливает объект, к которому присоединен узел.
        /// </summary>
        public void RestoreConnectedObject()
        {
            if (this.node.ConnectedObjectData == null)
                return;

            this.ConnectedObject = (FigureViewModel)this.layerHolder.GetObject(this.node.ConnectedObjectData.Item1);
        }

        /// <summary>
        /// Восстанавливает соединения с узлом по их данным.
        /// </summary>
        /// <param name="ignoreError">Значение, указывающее на то, что нужно ли игнорировать ошибку физического отсутствия линии, которая подключена к узлу.</param>
        public void RestoreConnections(bool ignoreError = false)
        {
            LineViewModel line;

            foreach (var entry in this.node.ConnectionData)
            {
                line = (LineViewModel)this.layerHolder.GetObject(entry.ConnectedLineId);

                if (line == null && ignoreError)
                    continue;

                this.AddConnection(new NodeConnection(line, entry.ConnectionSide));

                // Метод добавления соединения с узлом меняет значение, указывающее на то, что изменена ли линия, поэтому принудительно ставим его в false.
                line.IsModified = false;
            }
        }

        #endregion
    }

    // Реализация ObjectViewModel.
    internal sealed partial class NodeViewModel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public override bool IsActive
        {
            get
            {
                return this.node.IsActive;
            }
            set
            {
                this.node.IsActive = value;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что инициализирован ли объект.
        /// </summary>
        public override bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
            set
            {
                if (this.IsInitialized != value)
                {
                    this.isInitialized = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsInitialized), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        public override bool IsPlanning
        {
            get
            {
                return this.node.IsPlanning;
            }
            set
            {
                this.node.IsPlanning = value;
            }
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public override ObjectType Type
        {
            get
            {
                return this.node.Type;
            }
            set
            {
                this.node.Type = value;
            }
        }

        #endregion

        #region Открытые переопределенные методы
        
        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        public override void BeginSave()
        {
            this.isSaveStarted = true;

            this.isIdChanged = false;
            this.isConnectedObjectDataChanged = false;

            var isSaved = this.IsSaved;

            if (this.IsModified)
            {
                if (this.ConnectedObject != null)
                {
                    this.node.ConnectedObjectData = new Tuple<Guid, ObjectType, bool>(this.ConnectedObject.Id, this.ConnectedObject.Type, this.ConnectedObject.IsPlanning);

                    this.isConnectedObjectDataChanged = true;
                }

                // Запоминаем в узле данные соединений.
                if (this.IsConnectionsChanged || !isSaved || this.hasBrokenConnData)
                {
                    this.node.ConnectionData.Clear();

                    foreach (var conn in this.connections.Values)
                        if (!this.node.ConnectionData.Any(x => x.ConnectedLineId == conn.Line.Id))
                            this.node.ConnectionData.Add(new NodeConnectionData(conn.Line.Id, conn.ConnectionSide));
                }

                if (this.IsSaved)
                    this.DataService.NodeAccessService.UpdateObject(this.node, this.layerHolder.CurrentSchema);
                else
                {
                    this.node.Id = this.DataService.NodeAccessService.AddNode(this.node, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }
            }
        }

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        public override void EndSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;
            
            this.IsModified = false;

            this.isConnectionsChanged = false;

            this.hasBrokenConnData = false;
        }

        /// <summary>
        /// Отменяет сохранение объекта в источнике данных.
        /// </summary>
        public override void RevertSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            if (this.isIdChanged)
                this.node.Id = ObjectModel.DefaultId;

            if (this.isConnectedObjectDataChanged)
                this.node.ConnectedObjectData = null;
        }

        #endregion
    }

    // Реализация IEditableObjectViewModel.
    internal sealed partial class NodeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Вовзращает значение, указывающее на то, что может ли объект редактироваться.
        /// </summary>
        public bool CanBeEdited
        {
            get
            {
                if (this.layerHolder.CurrentSchema.IsActual && this.AccessService.CanDraw || this.layerHolder.CurrentSchema.IsIS && this.AccessService.CanDrawIS)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return this.isEditing;
            }
            set
            {
                if (this.IsEditing != value)
                {
                    this.isEditing = value;

                    this.IsHighlighted = value;

                    this.layerHolder.EditingObject = value ? this : null;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsEditing), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что изменен ли объект.
        /// </summary>
        public bool IsModified
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IHighlightableObjectViewModel.
    internal sealed partial class NodeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Задает значение, указывающее на то, что выделен ли объект.
        /// </summary>
        public bool IsHighlighted
        {
            set
            {
                if (this.IsPlaced)
                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsHighlighted), value);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Убирает выделение объекта.
        /// </summary>
        public void HighlightOff()
        {
            this.mapBindingService.AnimateOff();
        }

        /// <summary>
        /// Выделяет объект.
        /// </summary>
        public void HighlightOn()
        {
            this.mapBindingService.AnimateOn(this);
        }

        /// <summary>
        /// Сбрасывает выделение объекта.
        /// </summary>
        public void ResetHighlight()
        {
            this.mapBindingService.AnimateOff();
        }

        #endregion
    }

    // Реализация IMapObjectViewModel.
    internal sealed partial class NodeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get
            {
                return this.node.IsPlaced;
            }
            set
            {
                if (this.IsPlaced != value)
                {
                    this.node.IsPlaced = value;

                    this.NotifyPropertyChanged(nameof(this.IsPlaced));

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlaced), value);
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Регистрирует связь с представлением.
        /// </summary>
        public void RegisterBinding()
        {
            this.mapBindingService.RegisterBinding(this);
        }

        /// <summary>
        /// Вращает объект карты на заданный угол.
        /// </summary>
        /// <param name="angle">Угол.</param>
        /// <param name="origin">Точка, относительно которой выполняется поворот.</param>
        public void Rotate(double angle, Point origin)
        {
            var oX = origin.X;
            var oY = origin.Y;

            var x = this.LeftTopCorner.X;
            var y = this.LeftTopCorner.Y;

            var a = angle * Math.PI / 180;

            var newX = oX + (x - oX) * Math.Cos(a) + (oY - y) * Math.Sin(a);
            var newY = oY + (y - oY) * Math.Cos(a) + (x - oX) * Math.Sin(a);

            this.Shift(new Point(newX - x, newY - y));
        }

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        public void Scale(double scale, Point origin)
        {
            this.SetValue(nameof(this.LeftTopCorner), new Point(origin.X - (origin.X - this.LeftTopCorner.X) * scale, origin.Y - (origin.Y - this.LeftTopCorner.Y) * scale));
        }

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void Shift(Point delta)
        {
            var oldPos = this.LeftTopCorner;

            this.SetValue(nameof(this.LeftTopCorner), new Point(oldPos.X + delta.X, oldPos.Y + delta.Y));
        }

        /// <summary>
        /// Убирает связь с представлением.
        /// </summary>
        public void UnregisterBinding()
        {
            this.mapBindingService.UnregisterBinding(this);
        }

        #endregion
    }

    // Реализация ISelectableObjectViewModel.
    internal sealed partial class NodeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли объект.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.IsSelected != value)
                {
                    this.isSelected = value;

                    this.IsHighlighted = value;

                    this.NotifyPropertyChanged(nameof(this.IsSelected));

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsSelected), value);

                    this.layerHolder.SelectedObject = value ? this : null;
                }
            }
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class NodeViewModel
    {
        #region Открытые методы

        /// <summary>
        /// Задает значение заданного свойства в обход его сеттера.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        /// <param name="value">Значение.</param>
        public void SetValue(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(this.IgnoreStick):
                    this.node.IgnoreStick = (bool)value;

                    break;

                case nameof(this.LeftTopCorner):
                    var point = (Point)value;

                    this.node.Position.X = point.X;
                    this.node.Position.Y = point.Y;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LeftTopCorner), value);

                    // Также передвигаем присоединенные к узлу линии.
                    this.MoveConnectedLines();

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.IsModified = true;
        }

        #endregion
    }
}