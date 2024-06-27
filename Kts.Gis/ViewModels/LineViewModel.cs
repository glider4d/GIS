using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления линии.
    /// </summary>
    [Serializable]
    internal sealed partial class LineViewModel : ObjectViewModel, IContainerObjectViewModel, ICopyableObjectViewModel, IDeletableObjectViewModel, IEditableObjectViewModel, IHighlightableObjectViewModel, IMapObjectViewModel, INamedObjectViewModel, IParameterizedObjectViewModel, ISelectableObjectViewModel, ISetterIgnorer
    {
        #region Закрытые константы
    
        /// <summary>
        /// Текст подключения объекта.
        /// </summary>
        private const string activate = "Подключить";

        /// <summary>
        /// Текст отключения объекта.
        /// </summary>
        private const string deactivate = "Отключить";

        /// <summary>
        /// Сообщение об ошибке отсутствия узла.
        /// </summary>
        private const string nodeError = "Обнаружена проблема с узлом соединения линий. Выполните сохранение и свяжитесь с разработчиками для ее устранения.";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Форсированные сегменты линии. Они используются для ручного задания длин на надписях линии.
        /// </summary>
        private List<LineSegment> forcedSegments;

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
        /// Значение, указывающее на то, что зафиксирована ли длина линии.
        /// </summary>
        private bool isLengthFixed;

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
        /// Объект, способный иметь дочерние объекты.
        /// </summary>
        private readonly ContainerObjectViewModel containerObject;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Линия.
        /// </summary>
        private readonly LineModel line;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Модель представления карты.
        /// </summary>
        private readonly MapViewModel mapViewModel;

        #endregion

        #region Открытые неизменяемые поля

#warning Короче хрен с ним, пусть кто угодно имеет доступ к внутреннему представлению названия линии
        /// <summary>
        /// Именованный объект.
        /// </summary>
        public readonly NamedObjectViewModel namedObject;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LineViewModel"/>.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="mapViewModel">Модель представления карты.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public LineViewModel(LineModel line, ILayerHolder layerHolder, MapViewModel mapViewModel, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(line, accessService, dataService, historyService, messageService)
        {
            this.line = line;
            this.layerHolder = layerHolder;
            this.mapViewModel = mapViewModel;
            this.mapBindingService = mapBindingService;

            // Преобразуем строку в сегменты линии.
            if (!string.IsNullOrEmpty(this.line.ForcedLengths))
            {
                var lengths = this.line.ForcedLengths.Split(new string[1]
                {
                    ";"
                }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var segments = new List<LineSegment>();

                foreach (var length in lengths)
                    segments.Add(new LineSegment(Convert.ToDouble(length)));

                this.forcedSegments = segments;
            }

            this.RegisterBinding();

            this.containerObject = new ContainerObjectViewModel(this, line, line.HasChildren, this.layerHolder, accessService, dataService, historyService, this.mapBindingService, this.MessageService);
            this.namedObject = new NamedObjectViewModel(this, line.Name, false, mapBindingService);

            this.ChangeLengthViewCommand = new RelayCommand(this.ExecuteChangeLengthView);
            this.DivideCommand = new ParamRelayCommand<Point>(this.ExecuteDivide);
            this.ResetLengthViewCommand = new RelayCommand(this.ExecuteResetLengthView, this.CanExecuteResetLengthView);
            this.RestoreNodesCommand = new RelayCommand(this.ExecuteRestoreNodes, this.CanExecuteRestoreNodes);
            this.SelectPathCommand = new RelayCommand(this.ExecuteSelectPath);

            this.IsModified = true;

            if (!this.IsSaved)
            {
                // Запоминаем в параметрах необходимые значения и обрабатываем их.
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.TypeId), this.Type.TypeId);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsPlanning), this.IsPlanning);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsActive), this.IsActive);
#warning Тут раньше учитывался параметр, отвечающий за отключение линии
                //this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsLineWorking), this.IsWorking);
            }

            this.isLengthFixed = this.IsSaved;

            this.CopyParametersCommand = new RelayCommand(this.ExecuteCopyParameters, this.CanExecuteCopyParameters);
            this.PasteParametersCommand = new RelayCommand(this.ExecutePasteParameters, this.CanExecutePasteParameters);

            this.GroupedLine = this;

            this.DeactivateCommand = new RelayCommand(this.ExecuteDeactivate, this.CanExecuteDeactivate);

            this.DecreaseLabelCommand = new RelayCommand(this.ExecuteDecreaseLabel, this.CanExecuteDecreaseLabel);
            this.IncreaseLabelCommand = new RelayCommand(this.ExecuteIncreaseLabel, this.CanExecuteIncreaseLabel);
            this.ResetLabelCommand = new RelayCommand(this.ExecuteResetLabel, this.CanExecuteResetLabel);
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Возвращает или задает значение, используемое при поиске пути, указывающее на то, что была ли посещена линия.
        /// </summary>
        private bool IsVisited
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает количество сегментов линии.
        /// </summary>
        private int SegmentCount
        {
            get
            {
                return this.GetAllPoints().Count - 1;
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает модели представлений добавления линий.
        /// </summary>
        public Utilities.AdvancedObservableCollection<AddLineViewModel> AddLineViewModels
        {
            get;
        } = new Utilities.AdvancedObservableCollection<AddLineViewModel>();

        /// <summary>
        /// Возвращает команду изменения отображения длины.
        /// </summary>
        public RelayCommand ChangeLengthViewCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отключения объекта.
        /// </summary>
        public RelayCommand DeactivateCommand
        {
            get;
        }
        
        /// <summary>
        /// Возвращает текст команды отключения объекта.
        /// </summary>
        public string DeactivateText
        {
            get
            {
                return this.IsActive ? deactivate : activate;
            }
        }

        /// <summary>
        /// Возвращает команду уменьшения надписи.
        /// </summary>
        public RelayCommand DecreaseLabelCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает диаметр линии.
        /// </summary>
        public int Diameter
        {
            get
            {
                return this.line.Diameter;
            }
            private set
            {
                if (this.Diameter != value)
                {
                    this.line.Diameter = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.Diameter), value);
                }
            }
        }

        /// <summary>
        /// Возвращает команду разделения линии.
        /// </summary>
        public ParamRelayCommand<Point> DivideCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает форсированные сегменты линии. Они используются для ручного задания длин на надписях линии.
        /// </summary>
        public List<LineSegment> ForcedSegments
        {
            get
            {
                return this.forcedSegments;
            }
            set
            {
                //Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.ForcedSegments), this.ForcedSegments, value);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение отображения длины"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает линию, с которой сгруппирована данная линия.
        /// </summary>
        public LineViewModel GroupedLine
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду увеличения надписи.
        /// </summary>
        public RelayCommand IncreaseLabelCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что зафиксирована ли длина линии.
        /// </summary>
        public bool IsLengthFixed
        {
            get
            {
                return this.isLengthFixed;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.IsLengthFixed), this.IsLengthFixed, value);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение фиксированности протяженности"));
                action.Do();
            }
        }

#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
        ///// <summary>
        ///// Возвращает или задает значение, указывающее на то, что работает ли линия.
        ///// </summary>
        //public bool IsWorking
        //{
        //    get
        //    {
        //        return this.line.IsWorking;
        //    }
        //    private set
        //    {
        //        if (this.IsWorking != value)
        //        {
        //            this.line.IsWorking = value;

        //            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsWorking), value);
        //        }
        //    }
        //}

        /// <summary>
        /// Возвращает углы поворота надписей трубы. Ключом является индекс надписи, а значением - ее угол поворота.
        /// </summary>
        public Dictionary<int, double> LabelAngles
        {
            get
            {
                return this.line.LabelAngles;
            }
        }

        /// <summary>
        /// Возвращает или задает отступ надписи линии.
        /// </summary>
        public double LabelOffset
        {
            get
            {
                return this.line.LabelOffset;
            }
            set
            {
                // При изменении отступа надписи линии, необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение отступа может меняться очень часто.
                var exists = false;
                var entry = this.HistoryService.GetCurrentEntry();
                if (entry != null)
                {
                    var action = entry.Action as SetPropertyAction;

                    if (action != null && action.Object == this && action.PropertyName == nameof(this.LabelOffset))
                    {
                        action.NewValue = value;

                        action.Do();

                        exists = true;
                    }
                }
                if (!exists)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.LabelOffset), this.LabelOffset, value);
                    this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение отступа надписи линии"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает положения надписей трубы. Ключом является индекс надписи, а значением - ее положение.
        /// </summary>
        public Dictionary<int, Utilities.Point> LabelPositions
        {
            get
            {
                return this.line.LabelPositions;
            }
        }

        /// <summary>
        /// Возвращает размеры надписей трубы. Ключом является индекс надписи, а значением - ее размер.
        /// </summary>
        public Dictionary<int, int> LabelSizes
        {
            get
            {
                return this.line.LabelSizes;
            }
        }

        /// <summary>
        /// Возвращает или задает узел, к которому линия прикреплена левым концом.
        /// </summary>
        public NodeViewModel LeftNode
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает левую (начальную) точку линии.
        /// </summary>
        public Point LeftPoint
        {
            get
            {
                return new Point(this.line.StartPoint.X, this.line.StartPoint.Y);
            }
            set
            {
                if (this.LeftPoint != value)
                    this.SetLeftPoint(value);
            }
        }

        /// <summary>
        /// Возвращает или задает длину линии.
        /// </summary>
        public double Length
        {
            get
            {
                return this.line.Length;
            }
            private set
            {
                if (this.Length != value)
                {
                    this.line.Length = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.Length), value);
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
                return this.line.Points;
            }
            set
            {
                if (this.Points != value)
                {
                    this.line.Points = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.Points), value);

                    if (this.IsInitialized && !this.IsLengthFixed)
                        this.UpdateLength();
                }
            }
        }

        /// <summary>
        /// Возвращает команду сброса надписи.
        /// </summary>
        public RelayCommand ResetLabelCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса отображения длины.
        /// </summary>
        public RelayCommand ResetLengthViewCommand
        {
            get;
        }
        
        /// <summary>
        /// Возвращает команду восстановления узлов.
        /// </summary>
        public RelayCommand RestoreNodesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает узел, к которому линия прикреплена правым концом.
        /// </summary>
        public NodeViewModel RightNode
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает правую (конечную) точку линии.
        /// </summary>
        public Point RightPoint
        {
            get
            {
                return new Point(this.line.EndPoint.X, this.line.EndPoint.Y);
            }
            set
            {
                if (this.RightPoint != value)
                    this.SetRightPoint(value);
            }
        }

        /// <summary>
        /// Возвращает или задает индекс выбранной надписи.
        /// </summary>
#warning Костыль для определения того, с какой надписью пользователь сейчас работает
        public int SelectedLabelIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду выбора пути, в который входит данная линия.
        /// </summary>
        public RelayCommand SelectPathCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что следует ли отображать надписи линии.
        /// </summary>
        public bool ShowLabels
        {
            get
            {
                return this.line.ShowLabels;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.ShowLabels), this.ShowLabels, value);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение видимости надписей"));
                action.Do();
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="AddChildViewModel.AddBadgeRequested"/> модели представления добавления дочернего объекта.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddChildViewModel_AddBadgeRequested(object sender, AddBadgeRequestedEventArgs e)
        {
            e.Result = this.mapViewModel.RequestAddBadge(e.Line);
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду копирования значений параметров.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCopyParameters()
        {
            return this.AccessService.IsTypePermitted(this.Type.TypeId);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить отключение объекта.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDeactivate()
        {
            return this.AccessService.IsTypePermitted(this.Type.TypeId);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить уменьшение размера надписи.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDecreaseLabel()
        {
            return this.AccessService.CanDecreaseLabelSize(this.layerHolder.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить увеличение размера надписи.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteIncreaseLabel()
        {
            return this.AccessService.CanIncreaseLabelSize(this.layerHolder.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду вставки значений параметров.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecutePasteParameters()
        {
            return this.AccessService.IsTypePermitted(this.Type.TypeId) && this.IsActive && ClipboardManager.HasStoredParameters;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс надписи.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetLabel()
        {
            return this.AccessService.CanResetLabel(this.layerHolder.CurrentSchema.IsActual);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс отображения длины.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetLengthView()
        {
            return this.ForcedSegments != null;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить восстановление узлов.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteRestoreNodes()
        {
            return this.LeftNode == null || this.RightNode == null;
        }
        
        /// <summary>
        /// Выполняет изменение отображения длины.
        /// </summary>
        private void ExecuteChangeLengthView()
        {
            // Составляем список сегментов.
            var segments = new List<LineSegment>();
            var totalLength = this.GetFullLength();
            var allPoints = this.GetAllPoints();
            for (int i = 1; i < allPoints.Count; i++)
                segments.Add(new LineSegment(Math.Round(PointHelper.GetDistance(allPoints[i - 1], allPoints[i]) * this.Length / totalLength, 2)));
            // Теперь проверяем, есть ли форсированные сегменты и, если их количество совпадает, то заменяем вычисленные сегменты на форсированные.
            if (this.ForcedSegments != null && this.ForcedSegments.Count == segments.Count)
                segments = this.ForcedSegments.ToList();

            var viewModel = new ChangeLengthViewModel(segments, this.Length, this.MessageService);

#warning Прямое преобразование хранителя слоев в главную модель представления
            (this.layerHolder as MainViewModel).RequestChangeLengthView(viewModel);

            if (viewModel.Result)
                // Меняем форсированные сегменты линии.
                this.ForcedSegments = viewModel.Segments.ToList();
        }

        /// <summary>
        /// Выполняет копирование значений параметров.
        /// </summary>
        private void ExecuteCopyParameters()
        {
            this.LoadParameterValues();

            var values = new Dictionary<ParameterModel, object>();

            foreach (var entry in this.ParameterValuesViewModel.ParameterValueSet.ParameterValues)
                if (!string.IsNullOrEmpty(Convert.ToString(entry.Value)) && !this.Type.IsParameterReadonly(entry.Key) && entry.Key.CanBeCopied)
                    values.Add(entry.Key, entry.Value);

            ClipboardManager.StoreParameters(values);

            this.UnloadParameterValues();
        }

        /// <summary>
        /// Выполняет отключение объекта.
        /// </summary>
        private void ExecuteDeactivate()
        {
            var action = new DeactivateObjectAction(this);

            if (this.IsActive)
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "отключение линии"));
            else
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "подключение линии"));

            action.Do();
        }

        /// <summary>
        /// Выполняет уменьшение надписи.
        /// </summary>
        private void ExecuteDecreaseLabel()
        {
            var newSize = this.mapBindingService.TryDecreaseLabelSize(this, this.SelectedLabelIndex);

            if (newSize.HasValue)
            {
                if (this.line.LabelSizes.ContainsKey(this.SelectedLabelIndex))
                    this.line.LabelSizes[this.SelectedLabelIndex] = newSize.Value;
                else
                    this.line.LabelSizes.Add(this.SelectedLabelIndex, newSize.Value);

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Выполняет разделение линии.
        /// </summary>
        /// <param name="position">Позиция, по которой нужно разделить линию.</param>
        private void ExecuteDivide(Point position)
        {
            // Находим ближайшую точку на линии относительно заданной точки.
            var segment = this.GetNearestSegment(position);
            var point = PointHelper.GetNearestPoint(segment.Item1, segment.Item2, position);

            // Запоминаем предыдущее состояние точек изгиба.
            var originalPoints = this.Points;

            var newLine = this.Divide(new Point(point.X, point.Y));

            if (newLine != null)
            {
                // Создаем узел.
                var nodeModel = new NodeModel(ObjectModel.DefaultId, this.CityId, this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Node).First(), new Utilities.Point(point.X, point.Y), null, null, false);
                var nodeViewModel = new NodeViewModel(nodeModel, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };
                this.layerHolder.NodesGroup.Layers.First(x => x.Type.ObjectKind == ObjectKind.Node).Add(nodeViewModel);

                // Добавляем соединения.
                nodeViewModel.AddConnection(new NodeConnection(this, NodeConnectionSide.Right));
                nodeViewModel.AddConnection(new NodeConnection(newLine, NodeConnectionSide.Left));

                // Размещаем узел на карте.
                nodeViewModel.IsPlaced = true;
                
                // Запоминаем деление линии в истории изменений.
                this.HistoryService.Add(new HistoryEntry(new DivideLineAction(this, originalPoints, this.layerHolder, newLine, nodeViewModel, this.Points, newLine.Points), Target.Data, "разделение линии"));
            }
        }

        /// <summary>
        /// Выполняет увеличение надписи.
        /// </summary>
        private void ExecuteIncreaseLabel()
        {
            var newSize = this.mapBindingService.TryIncreaseLabelSize(this, this.SelectedLabelIndex);

            if (newSize.HasValue)
            {
                if (this.line.LabelSizes.ContainsKey(this.SelectedLabelIndex))
                    this.line.LabelSizes[this.SelectedLabelIndex] = newSize.Value;
                else
                    this.line.LabelSizes.Add(this.SelectedLabelIndex, newSize.Value);

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Выполняет вставку значений параметров.
        /// </summary>
        private void ExecutePasteParameters()
        {
            // Получаем список параметров, значения которых нужно вставить.
            var parameters = ClipboardManager.GetSelectedParameters(this.Type);

            // Получаем их значения.
            var values = ClipboardManager.RetrieveParameters(parameters);

            var oldValues = new Dictionary<ParameterModel, object>();

            if (parameters.Count > 0)
            {
                foreach (var entry in values)
                    oldValues.Add(entry.Key, this.ParameterValuesViewModel.ParameterValueSet.ParameterValues[entry.Key]);

                // Запоминаем действие в истории изменений и выполняем его.
                var action = new PasteParametersAction(this, oldValues, values);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "вставку значений параметров линии"));
                action.Do();
            }
        }

        /// <summary>
        /// Выполняет сброс надписи.
        /// </summary>
        private void ExecuteResetLabel()
        {
            this.LabelAngles.Remove(this.SelectedLabelIndex);
            this.LabelPositions.Remove(this.SelectedLabelIndex);
            this.LabelSizes.Remove(this.SelectedLabelIndex);

            this.IsModified = true;

            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelAngles), this.LabelAngles);
            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelPositions), this.LabelPositions);
            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelSizes), this.LabelSizes);
        }

        /// <summary>
        /// Выполняет сброс отображения длины.
        /// </summary>
        private void ExecuteResetLengthView()
        {
            this.ForcedSegments = null;
        }

        /// <summary>
        /// Выполняет восстановление узлов.
        /// </summary>
        private void ExecuteRestoreNodes()
        {
            if (this.LeftNode == null)
            {
                var nodeModel = new NodeModel(ObjectModel.DefaultId, this.CityId, this.DataService.ObjectTypes.First(x => x.ObjectKind == ObjectKind.Node), new Utilities.Point(this.LeftPoint.X, this.LeftPoint.Y), null, null, false);
                var node = new NodeViewModel(nodeModel, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };

                node.AddConnection(new NodeConnection(this, NodeConnectionSide.Left));

                this.layerHolder.AddObject(node);
            }

            if (this.RightNode == null)
            {
                var nodeModel = new NodeModel(ObjectModel.DefaultId, this.CityId, this.DataService.ObjectTypes.First(x => x.ObjectKind == ObjectKind.Node), new Utilities.Point(this.RightPoint.X, this.RightPoint.Y), null, null, false);
                var node = new NodeViewModel(nodeModel, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };

                node.AddConnection(new NodeConnection(this, NodeConnectionSide.Right));

                this.layerHolder.AddObject(node);
            }
        }

        /// <summary>
        /// Выполняет выбор пути, в который входит данная линия.
        /// </summary>
        private void ExecuteSelectPath()
        {
            this.layerHolder.SetSelectedObjects(this.GetConnections());
        }

        /// <summary>
        /// Обходит все узлы заданной линии, добавляя присоединенные к ним линии в список линий.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="result">Список линий.</param>
        private void GetLinePath(LineViewModel line, List<LineViewModel> result)
        {
            line.IsVisited = true;

            result.Add(line);
            
            if (line.LeftNode != null)
            {
                foreach (var leftLine in line.LeftNode.ConnectedLines)
                    if (!leftLine.IsVisited)
                        GetLinePath(leftLine, result);
            }
            else
                this.MessageService.ShowMessage(nodeError + "lineID = " + line.Id.ToString(), "Обход сети", MessageType.Error );

            if (line.RightNode != null)
            {
                foreach (var rightLine in line.RightNode.ConnectedLines)
                    if (!rightLine.IsVisited)
                        GetLinePath(rightLine, result);
            }
            else
                this.MessageService.ShowMessage(nodeError + "lineID = " + line.Id.ToString(), "Обход сети", MessageType.Error);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сгруппирована ли линия с заданной линией.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <returns>true, если сгруппирована, иначе - false.</returns>
        private bool IsGroupedWith(LineViewModel line)
        {
            return this.GroupedLine == line.GroupedLine;
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением значения параметра линии.
        /// </summary>
        /// <param name="param">Измененный параметр.</param>
        private void OnParameterChanged(ParameterModel param)
        {
            var value = this.ChangedParameterValues.ParameterValues[param];

            if (this.Type.CaptionParameters.Contains(param))
                this.namedObject.UpdateName(param, Convert.ToString(value));

            switch (param.Alias)
            {
                case Alias.IsActive:
                    this.IsActive = Convert.ToBoolean(value);

                    break;

#warning Тут раньше учитывался параметр, отвечающий за отключение линии
                //case Alias.IsLineWorking:
                //    this.IsWorking = Convert.ToBoolean(value);

                //    break;

                case Alias.IsPlanning:
                    this.IsPlanning = Convert.ToBoolean(value);

                    break;

                case Alias.LineDiameter:
                    if (value == null)
                        this.Diameter = 0;
                    else
                        this.Diameter = Convert.ToInt32(param.Table.GetEntries(null).First(x => Convert.ToInt32(x.Key) == Convert.ToInt32(value)).Value);

                    break;

                case Alias.LineLength:
                    this.Length = Convert.ToDouble(value);

                    break;
            }

            if (this.Type.CaptionParameters.Contains(param))
                // В данном случае, необходимо обновить надписи линии.
                this.mapBindingService.UpdateLineLabels(this);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет линии новый дочерний объект, представляемый значком на карте.
        /// </summary>
        /// <param name="type">Тип дочернего объекта.</param>
        /// <param name="distance">Расстояние, на которое дочерний объект отдален от конца линии.</param>
        public void AddChild(ObjectType type, double distance)
        {
            var model = new BadgeModel(ObjectModel.DefaultId, this.Id, this.CityId, type, this.IsPlanning, distance, this.IsActive);

            var viewModel = new BadgeViewModel(model, this, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

            this.AddChild(viewModel);
        }

        /// <summary>
        /// Добавляет линию к линии.
        /// </summary>
        /// <param name="type">Тип добавляемой линии.</param>
        public void AddLine(ObjectType type)
        {
            var newPoints = PolylineViewModel.GetOffsettedPoints(this.GetAllPoints(), this.mapBindingService.MapSettingService.PolylineOffset);

            var polyline = new PolylineViewModel(type, this.IsPlanning, newPoints, this.mapBindingService.GetBrush(type.Color), null, this.mapViewModel.Scale, this.DataService, this.MessageService, this.mapBindingService);

            var minCount = polyline.Points.Count;

            var line = (this.layerHolder as MainViewModel).ReplacePolyline(polyline, minCount, -5, this.Length);

            line.GroupWith(this);

            // Добавляем в историю изменений добавление линии.
            this.HistoryService.Add(new HistoryEntry(new AddRemoveLineAction(line, this.layerHolder as MainViewModel, true), Target.Data, "добавление линии"));
        }

        /// <summary>
        /// Меняет размер надписей по умолчанию. Если надпись уже имеет другой заданный размер, то он будет пропорционально увеличен.
        /// </summary>
        /// <param name="size">Новый размер по умолчанию.</param>
        /// <param name="prevSize">Предыдущий размер по умолчанию.</param>
        public void ChangeLabelDefaultSize(int size, int prevSize)
        {
            for (int i = 0; i < this.SegmentCount; i++)
                if (this.LabelSizes.ContainsKey(i))
                {
                    this.LabelSizes[i] = this.LabelSizes[i] - prevSize + size;

                    // Если надпись уже имела свой размер, то нужно отметить линию, что она изменена, чтобы сохранить результаты.
                    this.IsModified = true;
                }
                else
                    this.LabelSizes.Add(i, size);

            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelSizes), this.LabelSizes);
        }

        /// <summary>
        /// Уменьшает размер надписи.
        /// </summary>
        public void DecreaseLabelSize()
        {
            var newSize = this.mapBindingService.TryDecreaseLabelSize(this);

            if (newSize.HasValue)
            {
                for (int i = 0; i < this.SegmentCount; i++)
                    if (this.line.LabelSizes.ContainsKey(i))
                        this.line.LabelSizes[i] = newSize.Value;
                    else
                        this.line.LabelSizes.Add(i, newSize.Value);
                        
                this.IsModified = true;
            }
        }

        /// <summary>
        /// Делит линию на две части и возвращает правую часть.
        /// </summary>
        /// <param name="point">Точка, которая делит линию.</param>
        /// <returns>Правая часть линии.</returns>
        public LineViewModel Divide(Point point)
        {
            // Убираем группировку линий, если она имеется.
            foreach (var line in this.GetGroupedLines())
                line.GroupWith(line);

            // Находим две точки в линии, между которыми находится точка, которая ее делит.
            var points = this.GetAllPoints();
            var minDistance = double.MaxValue;
            // Индекс точки, до которой лежит точка, делящая линию.
            var index = 0;
            for (int i = 1; i < points.Count; i++)
            {
                var distance = PointHelper.GetCDistance(points[i - 1], points[i], point);

                if (distance < minDistance)
                {
                    minDistance = distance;

                    index = i;
                }
            }
            index -= 1;

            // Высчитываем протяженность линии на пиксель.
            var perPixel = this.Length / this.GetFullLength();

            // Делим точки изгиба.
            points = GetPoints(this.Points).ToList();
            var leftPoints = "";
            var rightPoints = "";
            var tempPoints = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i == index)
                {
                    leftPoints = new PointCollection(tempPoints).ToString();

                    tempPoints.Clear();
                }

                tempPoints.Add(points[i]);
            }
            if (index > points.Count - 1)
                leftPoints = new PointCollection(tempPoints).ToString();
            else
                rightPoints = new PointCollection(tempPoints).ToString();

            // Создаем новую линию.
            var lineModel = new LineModel(ObjectModel.DefaultId, ObjectModel.DefaultId, this.line.CityId, this.Type, this.IsPlanning, this.HasChildren, new Utilities.Point(point.X, point.Y), new Utilities.Point(this.RightPoint.X, this.RightPoint.Y), 0, rightPoints, new Dictionary<int, double>(), new Dictionary<int, Utilities.Point>(), new Dictionary<int, int>(), this.RawName, 0, this.LabelOffset, this.ShowLabels, null, this.IsActive);
            var lineViewModel = new LineViewModel(lineModel, this.layerHolder, this.mapViewModel, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

            var rightNode = this.RightNode;

            // Убираем соединение оригинальной линии с правым узлом.
            
            rightNode.RemoveConnection(this);

            // Прикрепляем новую линию к правому узлу.
            rightNode.AddConnection(new NodeConnection(lineViewModel, NodeConnectionSide.Right));
            
            this.layerHolder.AddObject(lineViewModel);

            // Заменяем правую точку оригинальной линии.
            this.RightPoint = point;

            this.Points = leftPoints;

            // Получаем значения параметров оригинальной линии.
            this.LoadParameterValues();

            // Добавляем их к новой линии.
            foreach (var entry in this.ParameterValuesViewModel.ParameterValueSet.ParameterValues)
                if (!string.IsNullOrEmpty(Convert.ToString(entry.Value)) && !this.Type.IsParameterReadonly(entry.Key))
                    lineViewModel.ChangeChangedValue(entry.Key, entry.Value);

            this.UnloadParameterValues();

            // Высчитываем новые длины линий.
            this.UpdateLength(this.GetFullLength() * perPixel);
            lineViewModel.UpdateLength(lineViewModel.GetFullLength() * perPixel);

            // Фиксируем протяженность новой линии.
            lineViewModel.SetValue(nameof(this.IsLengthFixed), true);

            return lineViewModel;
        }

        /// <summary>
        /// Возвращает все точки, из которых состоит линия.
        /// </summary>
        /// <returns>Последовательный список точек.</returns>
        public List<Point> GetAllPoints()
        {
            var result = new List<Point>();

            result.Add(new Point(this.LeftPoint.X, this.LeftPoint.Y));
            result.AddRange(GetPoints(this.Points));
            result.Add(new Point(this.RightPoint.X, this.RightPoint.Y));

            return result;
        }

        /// <summary>
        /// Возвращает идентификатор котельной, к которой принадлежит линия.
        /// </summary>
        /// <returns>Идентификатор котельной.</returns>
        public Guid GetBoilerId()
        {
            var result = Guid.Empty;

            // Получаем параметр, отвечающий за принадлежность к котельной.
            var param = this.Type.Parameters.FirstOrDefault(x => x.Alias == Alias.BoilerId);

            if (param != null)
                // Сперва проверяем измененные параметры, возможно там есть идентификатор котельной.
                if (this.HasChangedValue(param))
                {
                    var value = this.ChangedParameterValues.ParameterValues.First(x => x.Key == param).Value;
                    
                    if (value != null)
                        result = (Guid)value;
                }
                else
                {
                    this.LoadParameterValues();

                    if (this.ParameterValuesViewModel.ParameterValueSet.ParameterValues.Any(x => x.Key == param))
                    {
                        var entry = this.ParameterValuesViewModel.ParameterValueSet.ParameterValues.FirstOrDefault(x => x.Key == param);

                        if (entry.Value != null)
                            result = (Guid)entry.Value;
                    }

                    this.UnloadParameterValues();
                }

            return result;
        }

        /// <summary>
        /// Возвращает список связанных с данной линией объектов.
        /// </summary>
        /// <returns>Список объектов.</returns>
        public List<IObjectViewModel> GetConnections()
        {
            var allLines = this.GetLinePath();
            var allParents = new List<IObjectViewModel>();

            var result = new List<IObjectViewModel>(allLines);

            foreach (var line in allLines)
            {
                if (line.LeftNode != null && line.LeftNode.ConnectedObject != null && !allParents.Contains(line.LeftNode.ConnectedObject))
                    allParents.Add(line.LeftNode.ConnectedObject);

                if (line.RightNode != null && line.RightNode.ConnectedObject != null && !allParents.Contains(line.RightNode.ConnectedObject))
                    allParents.Add(line.RightNode.ConnectedObject);
            }

            result.AddRange(allParents);

            return result;
        }

        /// <summary>
        /// Возвращает полную длину линии в пикселях.
        /// </summary>
        /// <returns>Длина линии.</returns>
        public double GetFullLength()
        {
            var result = 0.0;

            var points = new List<Point>();

            points.Add(this.LeftPoint);
            points.AddRange(GetPoints(this.Points));
            points.Add(this.RightPoint);

            for (int i = 1; i < points.Count; i++)
                result += PointHelper.GetDistance(points[i - 1], points[i]);

            return result;
        }

        /// <summary>
        /// Возвращает линии, входящие в одну группу с данной линией.
        /// </summary>
        /// <returns>Список линий.</returns>
        public List<LineViewModel> GetGroupedLines()
        {
            var result = new List<LineViewModel>();

            foreach (var layer in this.layerHolder.GetLayers(ObjectKind.Line))
                foreach (LineViewModel line in layer.Objects)
                    if (line != this && line.IsGroupedWith(this))
                        result.Add(line);

            return result;
        }

        /// <summary>
        /// Возвращает путь из линий, в которую входит данная линия.
        /// </summary>
        /// <returns>Список линий, входящих в путь.</returns>
        public List<LineViewModel> GetLinePath()
        {
            var result = new List<LineViewModel>();

            this.GetLinePath(this, result);

            foreach (var line in result)
                line.IsVisited = false;

            return result;
        }

        /// <summary>
        /// Возвращает отрезок линии, который находится ближе всех к заданной точке.
        /// </summary>
        /// <param name="point">Точка.</param>
        /// <returns>Тюпл, содержащий концы найденного отрезка.</returns>
        public Tuple<Point, Point> GetNearestSegment(Point point)
        {
            var points = this.GetAllPoints();

            var minDistance = double.MaxValue;
            
            var index = 0;

            for (int i = 1; i < points.Count; i++)
            {
                var distance = PointHelper.GetCDistance(points[i - 1], points[i], point);

                if (distance < minDistance)
                {
                    minDistance = distance;

                    index = i;
                }
            }

            return new Tuple<Point, Point>(points[index - 1], points[index]);
        }

        /// <summary>
        /// Группирует линию с заданной линией.
        /// </summary>
        /// <param name="lineViewModel">Линия.</param>
        public void GroupWith(LineViewModel lineViewModel)
        {
            this.GroupedLine = lineViewModel;

            // Пересчитываем длину линии, исходя от длины линии, с которой происходит группировка.
            this.UpdateLength(lineViewModel.Length);

            this.IsModified = true;
        }

        /// <summary>
        /// Увеличивает размер надписи.
        /// </summary>
        public void IncreaseLabelSize()
        {
            var newSize = this.mapBindingService.TryIncreaseLabelSize(this);

            if (newSize.HasValue)
            {
                for (int i = 0; i < this.SegmentCount; i++)
                    if (this.line.LabelSizes.ContainsKey(i))
                        this.line.LabelSizes[i] = newSize.Value;
                    else
                        this.line.LabelSizes.Add(i, newSize.Value);

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданная длина линии приемлемой.
        /// </summary>
        /// <param name="length">Длина линии.</param>
        /// <returns>true, если заданная длина линии является приемлемой, иначе - false.</returns>
        public bool IsLengthNormal(double length)
        {
#warning Заблокирована проверка приемлимости длины линии
            //// Длина, которая должна была проставиться по умолчанию.
            //var defaultLength = Math.Round(PointHelper.GetDistance(this.LeftPoint, this.RightPoint) * this.mapViewModel.Scale, 2);

            //// Получаем 15%.
            //var delta = defaultLength * 15 / 100;

            //return length > defaultLength - delta && length < defaultLength + delta;

            return true;
        }

        /// <summary>
        /// Загружает значки.
        /// </summary>
        /// <param name="badges">Значки.</param>
        public void LoadBadges(DataSet badges)
        {
            this.LoadChildren(badges);

            foreach (var child in this.GetChildren())
            {
                child.IsInitialized = true;

                var mapObject = child as IMapObjectViewModel;

                if (mapObject != null)
                    mapObject.IsPlaced = true;
            }
        }

        /// <summary>
        /// Получает недостающий набор значений параметров от линии-донора.
        /// </summary>
        /// <param name="donor">Линия-донор.</param>
        /// <returns>Словарь, где в качестве ключа выступает параметр, а в качестве значения - тюпл из старого и нового значения параметра.</returns>
        public Dictionary<ParameterModel, Tuple<object, object>> MergeParameterValues(LineViewModel donor)
        {
            var result = new Dictionary<ParameterModel, Tuple<object, object>>();

            var paramValues = this.DataService.ParameterAccessService.GetMergedParamValues(this.line, donor.line, this.Type, this.layerHolder.CurrentSchema);

            foreach (var entry in paramValues.ParameterValues)
                if (!string.IsNullOrEmpty(Convert.ToString(entry.Value)) && !this.Type.IsParameterReadonly(entry.Key))
                {
                    result.Add(entry.Key, new Tuple<object, object>(this.ChangedParameterValues.ParameterValues.FirstOrDefault(x => x.Key == entry.Key).Value, entry.Value));

                    this.ChangeChangedValue(entry.Key, entry.Value);
                }

            return result;
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс надписи.</param>
        /// <param name="angle">Угол поворота надписи.</param>
        public void OnLabelAngleChanged(int index, double angle)
        {
            if (this.line.LabelAngles.ContainsKey(index))
                this.line.LabelAngles[index] = angle;
            else
                this.line.LabelAngles.Add(index, angle);

            this.IsModified = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением положения надписи с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс надписи.</param>
        /// <param name="angle">Положение надписи.</param>
        public void OnLabelPositionChanged(int index, Point position)
        {
            if (this.line.LabelPositions.ContainsKey(index))
                this.line.LabelPositions[index] = new Utilities.Point(position.X, position.Y);
            else
                this.line.LabelPositions.Add(index, new Utilities.Point(position.X, position.Y));

            this.IsModified = true;
        }

        /// <summary>
        /// Выполняет действия, связанные со значительным изменением точек, из которых состоит линия.
        /// </summary>  
        /// <param name="prevValue">Предыдущие точки, из которых состоит линия.</param>
        /// <param name="prevValue">Предыдущие точки изгиба линии.</param>
        public void OnMoved(Tuple<Point, Point> prevPoints, string prevBendPoints)
        {
            // Запоминаем изменение точек, из которых состоит линия, в истории изменений.
            this.HistoryService.Add(new HistoryEntry(new ChangeLinePointsAction(this, prevPoints.Item1, prevPoints.Item2, prevBendPoints, this.LeftPoint, this.RightPoint, this.Points), Target.Data, "изменение положения линии"));
        }

        /// <summary>
        /// Выполняет действия, связанные со значительным изменением точек изгиба линии.
        /// </summary>  
        /// <param name="prevValue">Предыдущие точки изгиба линии.</param>
        public void OnPointsChanged(string prevPoints)
        {
            // При изменении точек изгиба линии необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как точки могут часто меняться.
            var exists = false;
            var entry = this.HistoryService.GetCurrentEntry();
            if (entry != null)
            {
                var action = entry.Action as ChangeLineBendPointsAction;

                if (action != null && action.Line == this)
                {
                    action.NewPoints = this.Points;

                    exists = true;
                }
            }
            if (!exists)
                this.HistoryService.Add(new HistoryEntry(new ChangeLineBendPointsAction(this, prevPoints, this.Points), Target.Data, "изменение точек изгиба линии"));
        }

        /// <summary>
        /// Превращает точку изгиба с заданным индексом в узел соединения.
        /// </summary>
        /// <param name="index">Индекс точки изгиба.</param>
        public void PointToNode(int index)
        {
            var originalPoints = this.Points;

            var points = GetPoints(this.Points);

            // Запоминаем точку изгиба.
            var point = points[index];

            // Делим линию по данной точке.
            var newLine = this.Divide(new Point(point.X, point.Y));
            if (newLine != null)
            {
                // Создаем узел.
                var nodeModel = new NodeModel(ObjectModel.DefaultId, this.CityId, this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Node).First(), new Utilities.Point(point.X, point.Y), null, null, false);
                var nodeViewModel = new NodeViewModel(nodeModel, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService)
                {
                    IsInitialized = true
                };
                this.layerHolder.NodesGroup.Layers.First(x => x.Type.ObjectKind == ObjectKind.Node).Add(nodeViewModel);

                // Добавляем соединения.
                nodeViewModel.AddConnection(new NodeConnection(this, NodeConnectionSide.Right));
                nodeViewModel.AddConnection(new NodeConnection(newLine, NodeConnectionSide.Left));

                // Размещаем узел на карте.
                nodeViewModel.IsPlaced = true;

                // Убираем точку изгиба со второй линии.
                newLine.RemovePoint(0);

                // Запоминаем деление линии в истории изменений.
                this.HistoryService.Add(new HistoryEntry(new DivideLineAction(this, originalPoints, this.layerHolder, newLine, nodeViewModel, this.Points, newLine.Points), Target.Data, "разделение линии"));
            }
        }

        /// <summary>
        /// Удаляет точку изгиба с заданным индексом.
        /// </summary>
        /// <param name="index">Индекс удаляемой точки изгиба.</param>
        public void RemovePoint(int index)
        {
            var points = GetPoints(this.Points);

            points.RemoveAt(index);

            this.Points = new PointCollection(points).ToString();
        }
        
        /// <summary>
        /// Сбрасывает настройки надписей.
        /// </summary>
        public void ResetLabels()
        {
            this.LabelAngles.Clear();
            this.LabelPositions.Clear();
            this.LabelSizes.Clear();

            this.IsModified = true;

            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelAngles), this.LabelAngles);
            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelPositions), this.LabelPositions);
            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LabelSizes), this.LabelSizes);
        }

        /// <summary>
        /// Восстанавливает группировку линии.
        /// </summary>
        public void RestoreGrouping()
        {
            if (this.line.GroupId == this.line.Id)
                this.GroupedLine = this;
            else
            {
                var obj = this.layerHolder.GetObject(this.line.GroupId);

                if (obj != null)
                    this.GroupedLine = (LineViewModel)obj;
                else
                    // Иначе произошла какая-то ошибка и нужно изменить группировку линии.
                    this.GroupedLine = this;
            }
        }

        /// <summary>
        /// Задает левую (начальную) точку линии.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void SetLeftPoint(Point point)
        {
            this.line.StartPoint.X = point.X;
            this.line.StartPoint.Y = point.Y;

            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.LeftPoint), point);

            this.NotifyPropertyChanged(nameof(this.LeftPoint));

            this.IsModified = true;

            if (this.IsInitialized && !this.IsLengthFixed)
                this.UpdateLength();
        }

        /// <summary>
        /// Задает правую (конечную) точку линии.
        /// </summary>
        /// <param name="point">Точка.</param>
        public void SetRightPoint(Point point)
        {
            this.line.EndPoint.X = point.X;
            this.line.EndPoint.Y = point.Y;

            this.mapBindingService.SetMapObjectViewValue(this, nameof(this.RightPoint), point);

            this.NotifyPropertyChanged(nameof(this.RightPoint));

            this.IsModified = true;

            if (this.IsInitialized && !this.IsLengthFixed)
                this.UpdateLength();
        }

        /// <summary>
        /// Обновляет длину линии.
        /// </summary>
        public void UpdateLength(double? forcedValue = null)
        {
            var length = 0.0;

            if (forcedValue.HasValue)
                length = Math.Round(forcedValue.Value, 2);
            else
                length = Math.Round(this.GetFullLength() * this.mapViewModel.Scale, 2);

            if (this.GroupedLine != this)
                if (this.GroupedLine != null)
                {
                    if (this.GroupedLine.Length != length)
                        return;
                }
                else
                    return;

            this.Length = length;

            // Запоминаем длину в параметрах линии.
            var action = new ChangeLineParameterAction(this, this.Type.Parameters.First(x => x.Alias == Alias.LineLength), this.Length, this.Length);
            action.Do();
        }

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта по умолчанию
        /// </summary>
        public void LoadCalcParameterDefaultValues()
        {
            if (this.CalcParameterValuesViewModel == null)
            {
                this.CalcParameterValuesViewModel =
                    new ParameterValueSetViewModel(this,
                    this.Type.GetDefaultCalcParameterValues(),
                    true,
                    true,
                    this.CityId,
                    this.layerHolder,
                    this.AccessService,
                    this.DataService,
                    this.MessageService);
            }
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта по умолчанию
        /// </summary>
        public void LoadParameterDefaultValues()
        {

            if (this.ParameterValuesViewModel == null)
            {
                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this,
                    this.Type.GetDefaultParameterValues(),
                    !this.IsPlaced,
                    true,
                    this.CityId,
                    this.layerHolder,
                    this.AccessService,
                    this.DataService,
                    this.MessageService);
            }
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает коллекцию точек из заданного строкового представления.
        /// </summary>
        /// <param name="points">Строковое представление точек.</param>
        /// <returns>Коллекция точек.</returns>
        public static PointCollection GetPoints(string points)
        {
            var list = new List<Point>();

            if (!string.IsNullOrEmpty(points))
            {
                var ps = points.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] p;
                for (int i = 0; i < ps.Length; i++)
                {
                    p = ps[i].Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    list.Add(new Point(Convert.ToDouble(p[0]), Convert.ToDouble(p[1])));
                }
            }

            return new PointCollection(list);
        }

        #endregion
    }

    // Реализация ObjectViewModel.
    internal sealed partial class LineViewModel
    {
        #region Открытые переопределенные свойства
    
        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public override bool IsActive
        {
            get
            {
                return this.line.IsActive;
            }
            set
            {
                if (this.IsInitialized && this.IsActive != value)
                {
                    var wasSelected = this.IsSelected;

                    // Перемещаем объект со слоя на слой.
                    this.layerHolder.ClearSelectedGroup();
                    var curLayer = this.layerHolder.GetLayer(this.Type, this.IsActive ? (this.IsPlanning ? LayerType.Planning : LayerType.Standart) : LayerType.Disabled);
                    var tarLayer = this.layerHolder.GetLayer(this.Type, value ? (this.IsPlanning ? LayerType.Planning : LayerType.Standart) : LayerType.Disabled);
                    curLayer.MoveTo(this, tarLayer);

                    // Только потом меняем активность.
                    this.line.IsActive = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsActive), value);

                    if (wasSelected)
                        this.IsSelected = true;
                }
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
                return this.line.IsPlanning;
            }
            set
            {
                if (this.IsInitialized && this.IsPlanning != value)
                {
                    if (this.IsActive)
                    {
                        // Перемещаем объект со слоя на слой.
                        this.layerHolder.ClearSelectedGroup();
                        var curLayer = this.layerHolder.GetLayer(this.Type, this.IsPlanning ? LayerType.Planning : LayerType.Standart);
                        var tarLayer = this.layerHolder.GetLayer(this.Type, value ? LayerType.Planning : LayerType.Standart);
                        curLayer.MoveTo(this, tarLayer);
                    }

                    // Только потом меняем планируемость.
                    this.line.IsPlanning = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlanning), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public override ObjectType Type
        {
            get
            {
                return this.line.Type;
            }
            set
            {
                this.line.Type = value;
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
            
            if (this.ChangedParameterValues.ParameterValues.Count > 0)
                // Сохраняем значения измененных параметров линии.
                if (this.IsSaved)
                    this.DataService.ParameterAccessService.UpdateObjectParamValues(this.line, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                else
                {
                    this.line.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.line, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                    this.line.GroupId = this.line.Id;

                    this.isIdChanged = true;
                }
            else
                if (!this.IsSaved)
                {
                    this.line.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.line, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                    this.line.GroupId = this.line.Id;

                    this.isIdChanged = true;
                }
            
            // Если линия была сгруппирована с другой и она была сохранена, то нужно запомнить ее идентификатор.
            if (this.GroupedLine.IsSaved)
                this.line.GroupId = this.GroupedLine.Id;

            // Сохраняем дочерние объекты линии.
            foreach (var layer in this.ChildrenLayers)
                foreach (var obj in layer.Objects)
                    obj.BeginSave();

            if (this.IsModified)
                this.DataService.LineAccessService.UpdateObject(this.line, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        public override void EndSave()
        {
            if (!this.isSaveStarted)
                return;

            // Фиксируем длину линии.
            this.SetValue(nameof(this.IsLengthFixed), true);

            this.isSaveStarted = false;

            if (this.ChangedParameterValues.ParameterValues.Count > 0)
                this.ChangedParameterValues.ParameterValues.Clear();

            this.IsModified = false;
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
                this.line.Id = ObjectModel.DefaultId;
        }

        #endregion
    }

    // Реализация IContainerObjectViewModel.
    internal sealed partial class LineViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает модели представлений добавления дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<AddChildViewModel> AddChildViewModels
        {
            get
            {
                return this.containerObject.AddChildViewModels;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что загружены ли дочерние объекты.
        /// </summary>
        public bool AreChildrenLoaded
        {
            get
            {
                return this.containerObject.AreChildrenLoaded;
            }
        }

        /// <summary>
        /// Возвращает слои дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<LayerViewModel> ChildrenLayers
        {
            get
            {
                return this.containerObject.ChildrenLayers;
            }
        }

        /// <summary>
        /// Возвращает типы дочерних объектов.
        /// </summary>
        public List<ObjectType> ChildrenTypes
        {
            get
            {
                return this.line.ChildrenTypes;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.line.HasChildren;
            }
        }

        /// <summary>
        /// Возвращает модели представлений выбора дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<SelectChildViewModel> SelectChildViewModels
        {
            get
            {
                return this.containerObject.SelectChildViewModels;
            }
        }
        
        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void AddChild(IObjectViewModel child)
        {
            this.containerObject.AddChild(child);
        }

        /// <summary>
        /// Добавляет новый дочерний объект.
        /// </summary>
        /// <param name="type">Тип дочернего объекта.</param>
        public void AddChild(ObjectType type)
        {
            this.containerObject.AddChild(type);
        }

        /// <summary>
        /// Удаляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void DeleteChild(IObjectViewModel child)
        {
            this.containerObject.DeleteChild(child);
        }

        /// <summary>
        /// Удаляет дочерний объект из источника данных.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void FullDeleteChild(IObjectViewModel child)
        {
            this.containerObject.FullDeleteChild(child);
        }

        /// <summary>
        /// Возвращает список дочерних объектов.
        /// </summary>
        /// <returns>Список дочерних объектов.</returns>
        public List<IObjectViewModel> GetChildren()
        {
            return this.containerObject.GetChildren();
        }

        /// <summary>
        /// Загружает дочерние объекты из источника данных.
        /// </summary>
        public void LoadChildren()
        {
            this.containerObject.LoadChildren();
        }

        /// <summary>
        /// Загружает дочерние объекты из заданного набора данных.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        public void LoadChildren(DataSet dataSet)
        {
            this.containerObject.LoadChildren(dataSet);
        }

        #endregion
    }

    // Реализация ICopyableObjectViewModel.
    internal sealed partial class LineViewModel
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает копию объекта.
        /// </summary>
        /// <returns>Копия объекта.</returns>
        public ICopyableObjectViewModel Copy()
        {
            var model = this.line.Clone();

            // Находим расстояния, на которые нужно сдвинуть линию.
            var pointerPosition = this.mapBindingService.GetPointerPosition();
            var centerPoint = PointHelper.GetMidPoint(new Point(model.StartPoint.X, model.StartPoint.Y), new Point(model.EndPoint.X, model.EndPoint.Y));
            var deltaX = pointerPosition.X - centerPoint.X;
            var deltaY = pointerPosition.Y - centerPoint.Y;

            model.Id = ObjectModel.DefaultId;
            model.StartPoint = new Utilities.Point(model.StartPoint.X + deltaX, model.StartPoint.Y + deltaY);
            model.EndPoint = new Utilities.Point(model.EndPoint.X + deltaX, model.EndPoint.Y + deltaY);

            var points = GetPoints(model.Points);
            for (int i = 0; i < points.Count; i++)
                points[i] = new Point(points[i].X + deltaX, points[i].Y + deltaY);
            model.Points = points.ToString();

            var viewModel = new LineViewModel(model, this.layerHolder, this.mapViewModel, this.AccessService, this.DataService, this.HistoryService, this.mapBindingService, this.MessageService);

            // Добавляем значения редактируемых параметров копии объекта.
            this.LoadParameterValues();
            foreach (var entry in this.ParameterValuesViewModel.ParameterValueSet.ParameterValues)
                if (!string.IsNullOrEmpty(Convert.ToString(entry.Value)) && !this.Type.IsParameterReadonly(entry.Key))
                    viewModel.ChangeChangedValue(entry.Key, entry.Value);
            this.UnloadParameterValues();

            return viewModel;
        }

        #endregion
    }

    // Реализация IDeletableObjectViewModel.
    internal sealed partial class LineViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить объект.
        /// </summary>
        public bool CanBeDeleted
        {
            get
            {
                return this.AccessService.IsTypePermitted(this.Type.TypeId) && this.IsPlanning;
            }
        }

        /// <summary>
        /// Возвращает команду удаления объекта.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return this.layerHolder.DeleteCommand;
            }
        }

        /// <summary>
        /// Возвращает команду полного удаления объекта.
        /// </summary>
        public RelayCommand FullDeleteCommand
        {
            get
            {
                return this.layerHolder.FullDeleteCommand;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет полное удаление объекта из источника данных.
        /// </summary>
        public void FullDelete()
        {
            this.DataService.LineAccessService.DeleteObject(this.line, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        public void MarkFullDelete()
        {
            this.DataService.LineAccessService.MarkDeleteObject(this.line, this.layerHolder.CurrentSchema);
        }

        #endregion
    }

    // Реализация IEditableObjectViewModel.
    internal sealed partial class LineViewModel
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

                    // Управляем возможностью добавления линий.
                    if (value)
                        foreach (var type in this.DataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Line && x != this.Type))
                            this.AddLineViewModels.Add(new AddLineViewModel(this, type));
                    else
                        this.AddLineViewModels.Clear();
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
    internal sealed partial class LineViewModel
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
    internal sealed partial class LineViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get
            {
                return this.line.IsPlaced;
            }
            set
            {
                if (this.IsPlaced != value)
                {
                    this.line.IsPlaced = value;

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

            var a = angle * Math.PI / 180;

            var points = GetPoints(this.Points);

            var x = 0.0;
            var y = 0.0;

            for (int i = 0; i < points.Count; i++)
            {
                x = points[i].X;
                y = points[i].Y;

                var newX = oX + (x - oX) * Math.Cos(a) + (oY - y) * Math.Sin(a);
                var newY = oY + (y - oY) * Math.Cos(a) + (x - oX) * Math.Sin(a);

                points[i] = new Point(newX, newY);
            }
            
            this.Points = points.ToString();
        }

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        public void Scale(double scale, Point origin)
        {
            // Вращаем все точки изгиба линии.
            var points = GetPoints(this.Points);
            for (int i = 0; i < points.Count; i++)
                points[i] = new Point(origin.X - (origin.X - points[i].X) * scale, origin.Y - (origin.Y - points[i].Y) * scale);
            this.Points = points.ToString();
        }

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void Shift(Point delta)
        {
            // Сдвигаем все точки изгиба линии.
            var points = GetPoints(this.Points);
            for (int i = 0; i < points.Count; i++)
                points[i] = new Point(points[i].X + delta.X, points[i].Y + delta.Y);
            this.Points = points.ToString();
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

    // Реализация INamedObjectViewModel.
    internal sealed partial class LineViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string Name
        {
            get
            {
#warning Тут производим замену протяженности трубы в названии
                var param = this.Type.Parameters.First(x => x.Alias == Alias.LineLength);
                return this.namedObject.Name.Replace("!@#$%Alias.LineLength!@#$%", this.HasChangedValue(param) ? this.ChangedParameterValues.ParameterValues[param].ToString() : this.Length.ToString());
            }
        }

        /// <summary>
        /// Возвращает необработанное название объекта, полученное из источника данных.
        /// </summary>
        public string RawName
        {
            get
            {
                return this.namedObject.RawName;
            }
        }

        #endregion
    }

    // Реализация IParameterizedObjectViewModel.
    internal sealed partial class LineViewModel
    {
        #region Открытые свойства
    
        /// <summary>
        /// Возвращает или задает модель представления значений вычисляемых параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel CalcParameterValuesViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает набор значений измененных параметров объекта.
        /// </summary>
        public ParameterValueSetModel ChangedParameterValues
        {
            get;
        } = new ParameterValueSetModel(new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer()));

        /// <summary>
        /// Возвращает или задает модель представления значений общих параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel CommonParameterValuesViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду копирования параметров.
        /// </summary>
        public RelayCommand CopyParametersCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает модель представления значений параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel ParameterValuesViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду вставки параметров.
        /// </summary>
        public RelayCommand PasteParametersCommand
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Меняет значение измененного параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="newValue">Новое значение.</param>
        public void ChangeChangedValue(ParameterModel param, object newValue)
        {
            if (!this.IsActive)
                if (param.Alias != Alias.IsActive)
                    return;

            // Если на данный момент отображены параметры объекта, то нужно уведомить об изменении значения одного из них таким образом, чтобы это изменение не внеслось в историю изменений.
            if (this.ParameterValuesViewModel != null)
            {
                var temp = this.ParameterValuesViewModel.GetParameter(param.Id);

                if (!Equals(temp.Value, newValue))
                    temp.ChangeValue(newValue, true);
            }

            if (this.ChangedParameterValues.ParameterValues.ContainsKey(param))
                this.ChangedParameterValues.ParameterValues[param] = newValue;
            else
                this.ChangedParameterValues.ParameterValues.Add(param, newValue);

            this.OnParameterChanged(param);
        }

        /// <summary>
        /// Возвращает список параметров, содержащих ошибки в значениях.
        /// </summary>
        /// <returns>Список параметров.</returns>
        public List<ParameterModel> GetErrors()
        {
            var result = new List<ParameterModel>();

            if (this.IsSaved)
            {
                // Если объект сохранен, то это предполагает то, что ошибки могут быть только в значениях измененных параметров.
                foreach (var entry in this.ChangedParameterValues.ParameterValues)
                    if (this.Type.IsParameterVisible(entry.Key, this.ChangedParameterValues) && this.Type.IsParameterNecessary(entry.Key) && string.IsNullOrEmpty(Convert.ToString(entry.Value)))
                        result.Add(entry.Key);
            }
            else
                // Иначе, необходимо обойти все параметры типа объекта.
                foreach (var param in this.Type.Parameters)
                    if (this.Type.IsParameterVisible(param, this.ChangedParameterValues) && this.Type.IsParameterNecessary(param) && (!this.ChangedParameterValues.ParameterValues.ContainsKey(param) || string.IsNullOrEmpty(Convert.ToString(this.ChangedParameterValues.ParameterValues[param]))))
                        result.Add(param);

            return result;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли заданный параметр измененное значение.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <returns>true, если имеет, иначе - false.</returns>
        public bool HasChangedValue(ParameterModel param)
        {
            return this.ChangedParameterValues.ParameterValues.ContainsKey(param);
        }

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта.
        /// </summary>
        public void LoadCalcParameterValues()
        {
            var calcParameterValues = this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.line, this.layerHolder.CurrentSchema);

            this.CalcParameterValuesViewModel = new ParameterValueSetViewModel(this, calcParameterValues, true, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений вычисляемых параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadCalcParameterValuesAsync(CancellationToken cancellationToken)
        {
            //var calcParameterValues = await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.line, this.layerHolder.CurrentSchema, cancellationToken);
            var calcParameterValues = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.line, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.line, this.layerHolder.CurrentSchema, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                this.CalcParameterValuesViewModel = new ParameterValueSetViewModel(this, calcParameterValues, true, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений общих параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public Task LoadCommonParameterValuesAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Линия не имеет общие параметры");
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта.
        /// </summary>
        public void LoadParameterValues()
        {
            var temp = this.DataService.ParameterAccessService.GetObjectParamValues(this.line, this.layerHolder.CurrentSchema);

            foreach (var entry in this.ChangedParameterValues.ParameterValues)
                temp.ParameterValues[entry.Key] = entry.Value;

            this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, false, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadParameterValuesAsync(CancellationToken cancellationToken)
        {
            //var temp = await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.line, this.layerHolder.CurrentSchema, cancellationToken);
            var temp = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectParamValues(this.line, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.line, this.layerHolder.CurrentSchema, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                foreach (var entry in this.ChangedParameterValues.ParameterValues)
                    temp.ParameterValues[entry.Key] = entry.Value;

                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, false, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
            }
        }

        /// <summary>
        /// Уведомляет объект об изменении значения параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="prevValue">Предыдущее значение.</param>
        /// <param name="newValue">Новое значение.</param>
        public void NotifyParameterValueChanged(ParameterModel param, object prevValue, object newValue)
        {
            // Запоминаем действие в истории изменений и выполняем его.
            var action = new ChangeLineParameterAction(this, param, prevValue, newValue);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение значения параметра линии"));
            action.Do();
        }

        /// <summary>
        /// Выполняет выгрузку значений вычисляемых параметров объекта.
        /// </summary>
        public void UnloadCalcParameterValues()
        {
            this.CalcParameterValuesViewModel = null;
        }

        /// <summary>
        /// Выполняет выгрузку значений общих параметров объекта.
        /// </summary>
        public void UnloadCommonParameterValues()
        {
            throw new NotSupportedException("Линия не имеет общие параметры");
        }

        /// <summary>
        /// Выполняет выгрузку значений параметров объекта.
        /// </summary>
        public void UnloadParameterValues()
        {
            this.ParameterValuesViewModel = null;
        }

        #endregion
    }

    // Реализация ISelectableObjectViewModel.
    internal sealed partial class LineViewModel
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

                    // Управляем возможностью добавления дочерних объектов.
                    if (value)
                        foreach (var type in this.Type.Children.OrderBy(x => x.SingularName))
                        {
                            var addChildViewModel = new AddChildViewModel(this, type, type.ObjectKind == ObjectKind.Badge);

                            this.AddChildViewModels.Add(addChildViewModel);

                            addChildViewModel.AddBadgeRequested += this.AddChildViewModel_AddBadgeRequested;
                        }
                    else
                    {
                        foreach (var addChildViewModel in this.AddChildViewModels)
                            addChildViewModel.AddBadgeRequested -= this.AddChildViewModel_AddBadgeRequested;

                        this.AddChildViewModels.Clear();
                    }

                    this.layerHolder.SelectedObject = value ? this : null;
                }
            }
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class LineViewModel
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
                case nameof(this.ForcedSegments):
                    this.forcedSegments = (List<LineSegment>)value;

                    this.line.ForcedLengths = string.Join(";", this.ForcedSegments.Select(x => x.Length.ToString()));

                    this.ResetLengthViewCommand.RaiseCanExecuteChanged();

                    break;

                case nameof(this.IsLengthFixed):
                    this.isLengthFixed = (bool)value;

                    // Больше ничего не надо делать.
                    return;

                case nameof(this.LabelOffset):
                    var offset = (double)value;

                    // Ограничиваем значение отступа.
                    if (offset > 10)
                        offset = 10;
                    if (offset < -10)
                        offset = -10;

                    value = offset;

                    this.line.LabelOffset = offset;

                    this.NotifyPropertyChanged(nameof(this.LabelOffset));

                    break;
                    
                case nameof(this.ShowLabels):
                    this.line.ShowLabels = (bool)value;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.mapBindingService.SetMapObjectViewValue(this, propertyName, value);

            this.IsModified = true;
        }

        #endregion
    }
}