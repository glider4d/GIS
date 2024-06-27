using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Reports;
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

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления фигуры.
    /// </summary>
    [Serializable]
    internal abstract partial class FigureViewModel : ObjectViewModel, IContainerObjectViewModel, ICopyableObjectViewModel, IDeletableObjectViewModel, IEditableObjectViewModel, IExpandableObjectViewModel, IHighlightableObjectViewModel, IMapObjectViewModel, INamedObjectViewModel, IParameterizedObjectViewModel, ISelectableObjectViewModel, ISetterIgnorer
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

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что рисуется ли объект.
        /// </summary>
        private bool isDrawing;

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
        /// Объект, способный иметь дочерние объекты.
        /// </summary>
        
        private readonly ContainerObjectViewModel containerObject;

        /// <summary>
        /// Раскрываемый объект.
        /// </summary>
        //[NonSerialized]
        private readonly ExpandableObjectViewModel expandableObject;

        /// <summary>
        /// Именованный объект.
        /// </summary>
        //[NonSerialized]
        private readonly NamedObjectViewModel namedObject;

        #endregion

        #region Защищенные неизменяемые поля

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        //[NonSerialized]
        protected readonly ILayerHolder layerHolder;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FigureViewModel"/>.
        /// </summary>
        /// <param name="figure">Фигура.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public FigureViewModel(FigureModel figure, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(figure, accessService, dataService, historyService, messageService)
        {
            this.Figure = figure;
            this.layerHolder = layerHolder;
            this.MapBindingService = mapBindingService;

            this.SelectConnectionsCommand = new RelayCommand(this.ExecuteSelectConnections);

            this.containerObject = new ContainerObjectViewModel(this, figure, figure.HasChildren, layerHolder, accessService, dataService, historyService, this.MapBindingService, this.MessageService);
            this.expandableObject = new ExpandableObjectViewModel(this);
            this.namedObject = new NamedObjectViewModel(this, figure.Name, true, mapBindingService);

            this.IsModified = true;

            if (!this.IsSaved)
                // Запоминаем в параметрах необходимые значения и обрабатываем их.
                this.StoreInitialParams();

            this.CopyParametersCommand = new RelayCommand(this.ExecuteCopyParameters, this.CanExecuteCopyParameters);
            this.PasteParametersCommand = new RelayCommand(this.ExecutePasteParameters, this.CanExecutePasteParameters);

            this.DeactivateCommand = new RelayCommand(this.ExecuteDeactivate, this.CanExecuteDeactivate);

            this.DecreaseLabelCommand = new RelayCommand(this.ExecuteDecreaseLabel, this.CanExecuteDecreaseLabel);
            this.IncreaseLabelCommand = new RelayCommand(this.ExecuteIncreaseLabel, this.CanExecuteIncreaseLabel);
            this.ResetLabelCommand = new RelayCommand(this.ExecuteResetLabel, this.CanExecuteResetLabel);

            this.ShowDocuments = new RelayCommand(this.ExecuteShowDocuments, this.CanExecuteShowDocuments);
        }

        #endregion
        
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает угол поворота фигуры.
        /// </summary>
        public double Angle
        {
            get
            {
                return this.Figure.Angle;
            }
            set
            {
                if (this.Angle != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Angle), this.Angle, value);
                    this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение угла поворота фигуры"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает команду копирования объекта.
        /// </summary>
        public RelayCommand CopyCommand
        {
            get
            {
                return this.layerHolder.CopyCommand;
            }
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
        /// Возвращает генератор форм.
        /// </summary>
        public object FormGeneratorForms
        {
            get
            {
                return FormGenerator.Forms;
            }
        }

        /// <summary>
        /// Возвращает команду увеличения надписи.
        /// </summary>
        public RelayCommand IncreaseLabelCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли объект котельной.
        /// </summary>
        public bool IsBoiler
        {
            get
            {
                return this.DataService.IsBoilerType(this.Type);
            }
        }

        public bool isTrashStorage
        {
            get
            {
                return this.DataService.IsTrashStorageType(this.Type);
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что рисуется ли объект.
        /// </summary>
        public bool IsDrawing
        {
            get
            {
                return this.isDrawing;
            }
            set
            {
                this.isDrawing = value;

                this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsDrawing), value);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли объект складом.
        /// </summary>
        public bool IsStorage
        {
            get
            {
                return this.DataService.IsStorageType(this.Type);
            }
        }

        /// <summary>
        /// Возвращает или задает угол поворота надписи фигуры.
        /// </summary>
        public double? LabelAngle
        {
            get
            {
                return this.Figure.LabelAngle;
            }
            set
            {
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.LabelAngle), this.LabelAngle, value);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение угла поворота надписи фигуры"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает или задает положение надписи фигуры.
        /// </summary>
        public Point? LabelPosition
        {
            get
            {
                if (this.Figure.LabelPosition != null)
                    return new Point(this.Figure.LabelPosition.X, this.Figure.LabelPosition.Y);

                return null;
            }
            set
            {
                if (this.LabelPosition != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.LabelPosition), this.LabelPosition, value);
                    this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение положения надписи фигуры"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает размер надписи фигуры.
        /// </summary>
        public int? LabelSize
        {
            get
            {
                return this.Figure.LabelSize;
            }
            set
            {
                if (this.LabelSize != value)
                {
                    // При изменении размера надписи фигуры, необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение размера может меняться очень часто.
                    var exists = false;
                    var entry = this.HistoryService.GetCurrentEntry();
                    if (entry != null)
                    {
                        var action = entry.Action as SetPropertyAction;

                        if (action != null && action.Object == this && action.PropertyName == nameof(this.LabelSize))
                        {
                            action.NewValue = value;

                            action.Do();

                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new SetPropertyAction(this, nameof(this.LabelSize), this.LabelSize, value);
                        this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение размера надписи фигуры"));
                        action.Do();
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает или задает положение фигуры.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(this.Figure.Position.X, this.Figure.Position.Y);
            }
            set
            {
                if (this.Position != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Position), this.Position, value);
                    this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение положения фигуры"));
                    action.Do();
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
        /// Возвращает команду выбора соединений с данной фигурой.
        /// </summary>
        public RelayCommand SelectConnectionsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду отображения документов.
        /// </summary>
        public RelayCommand ShowDocuments
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает размер фигуры.
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(this.Figure.Size.Width, this.Figure.Size.Height);
            }
            set
            {
                if (this.Size != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Size), new Tuple<Size, Point>(this.Size, this.Position), new Tuple<Size, Point>(value, this.Position));
                    this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение размера фигуры"));
                    action.Do();
                }
            }
        }

        #endregion

        #region Открытые абстрактные свойства

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек, из которых состоит фигура.
        /// </summary>
        public abstract string Points
        {
            get;
            set;
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает фигуру.
        /// </summary>
        //[NonSerialized]
        private FigureModel m_figureModel;
        public FigureModel Figure
        {
            get
            {
                return m_figureModel;
            }
            private set
            {
                m_figureModel = value;
            }

        }

        /// <summary>
        /// Возвращает сервис привязки представлений карты с моделями представлений.
        /// </summary>
        //[NonSerialized]
        IMapBindingService m_mapBindingService;
        protected IMapBindingService MapBindingService
        {
            get
            {
                return m_mapBindingService;
            }
            private set
            {
                m_mapBindingService = value;
            }
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
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду открытия документов.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShowDocuments()
        {
            return this.IsSaved;
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
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "отключение фигуры"));
            else
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "подключение фигуры"));

            action.Do();
        }

        /// <summary>
        /// Выполняет уменьшение надписи.
        /// </summary>
        private void ExecuteDecreaseLabel()
        {
            this.DecreaseLabelSize();
        }

        /// <summary>
        /// Выполняет увеличение надписи.
        /// </summary>
        private void ExecuteIncreaseLabel()
        {
            this.IncreaseLabelSize();
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
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "вставку значений параметров фигуры"));
                action.Do();
            }
        }

        /// <summary>
        /// Выполняет сброс надписи.
        /// </summary>
        private void ExecuteResetLabel()
        {
            this.ResetLabel();
        }

        /// <summary>
        /// Выполняет выбор соединений с данной фигурой.
        /// </summary>
        private void ExecuteSelectConnections()
        {
            var allLines = new List<LineViewModel>();
            var allConnectedObjects = new List<IObjectViewModel>();

            var result = new List<IObjectViewModel>();
            
            if (this.IsSaved)
            {
                var boilerId = this.DataService.FigureAccessService.GetBoilerId(this.Figure, this.layerHolder.CurrentSchema);

                if (boilerId != Guid.Empty)
                {
                    // Добавляем все подключенные линии.
                    foreach (var entry in this.DataService.LineAccessService.GetAllFast(this.CityId, boilerId, this.layerHolder.CurrentSchema))
                        allLines.Add((LineViewModel)this.layerHolder.GetObject(entry.Item1));
                    result.AddRange(allLines);

                    // А также все подключенные фигуры.
                    foreach (var entry in this.DataService.FigureAccessService.GetAllFast(this.CityId, boilerId, this.layerHolder.CurrentSchema))
                        allConnectedObjects.Add((FigureViewModel)this.layerHolder.GetObject(entry.Item1));
                    result.AddRange(allConnectedObjects);
                }
                else
                    this.MessageService.ShowMessage("Нельзя выбрать все сети объекта, который не имеет привязки к котельной", "Выбор всех сетей", MessageType.Information);
            }
            else
                this.MessageService.ShowMessage("Нельзя выбрать все сети несохраненного объекта", "Выбор всех сетей", MessageType.Information);
                
            if (result.Count > 0)
                this.layerHolder.SetSelectedObjects(result);
        }

        /// <summary>
        /// Выполняет отображение документов, прикрепленных к объекту.
        /// </summary>
        private void ExecuteShowDocuments()
        {
#warning ВНИМАНИЕ, ТУТ ТУПО ИДЕТ ПРЕОБРАЗОВАНИЕ В ГЛАВНУЮ МОДЕЛЬ ПРЕДСТАВЛЕНИЯ
            (this.layerHolder as MainViewModel).ShowDocuments(this.Figure, this.Name);
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением значения параметра фигуры.
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

                case Alias.IsPlanning:
                    this.IsPlanning = Convert.ToBoolean(value);

                    break;

                case Alias.TypeId:
                    var newType = this.DataService.GetObjectType(Convert.ToInt32(value));

                    // Если тип был изменен, то нужно удалить все значения измененных параметров, чтобы они не были видны пользователю.
                    if (newType != this.Type)
                    {
                        this.Type = newType;
                        
                        // Составляем список свойств, не входящих в новый тип объекта.
                        var removeList = new List<ParameterModel>();
                        foreach (var p in this.ChangedParameterValues.ParameterValues)
                            if (!this.Type.Parameters.Contains(p.Key))
                                removeList.Add(p.Key);

                        // Удаляем эти свойства.
                        foreach (var p in removeList)
                            this.ChangedParameterValues.ParameterValues.Remove(p);
                    }

                    // Меняем название объекта.
                    this.namedObject.ProcessRawName("");
                    this.namedObject.UpdateName();

                    break;
            }
        }
        
        /// <summary>
        /// Запоминает начальные значения необходимых параметров объекта.
        /// </summary>
        private void StoreInitialParams()
        {
            this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.TypeId), this.Type.TypeId);
            this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsPlanning), this.IsPlanning);
            this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsActive), this.IsActive);
        }

        #endregion
        
        #region Открытые методы

        /// <summary>
        /// Уменьшает размер надписи.
        /// </summary>
        public void DecreaseLabelSize()
        {
            var newSize = this.MapBindingService.TryDecreaseLabelSize(this);

            if (newSize.HasValue)
            {
                this.Figure.LabelSize = newSize.Value;

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Возвращает идентификатор котельной, к которой принадлежит фигура.
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
        /// Увеличивает размер надписи.
        /// </summary>
        public void IncreaseLabelSize()
        {
            var newSize = this.MapBindingService.TryIncreaseLabelSize(this);

            if (newSize.HasValue)
            {
                this.Figure.LabelSize = newSize.Value;

                this.IsModified = true;
            }
        }
        
        /// <summary>
        /// Выполняет действия, связанные со значительным изменением угла поворота фигуры.
        /// </summary>
        /// <param name="prevValue">Предыдущий угол поворота фигуры.</param>
        public void OnAngleChanged(double prevAngle)
        {
            // Запоминаем изменение угла в истории изменений.
            this.HistoryService.Add(new HistoryEntry(new SetPropertyAction(this, nameof(this.Angle), prevAngle, this.Angle), Target.Data, "изменение угла поворота фигуры"));
        }

        /// <summary>
        /// Выполняет действия, связанные со значительным изменением положения фигуры.
        /// </summary>
        /// <param name="prevValue">Предыдущее положение фигуры.</param>
        public void OnPositionChanged(Point prevValue)
        {
            // Запоминаем изменение положения в истории изменений.
            this.HistoryService.Add(new HistoryEntry(new SetPropertyAction(this, nameof(this.Position), prevValue, this.Position), Target.Data, "изменение положения фигуры"));
        }

        /// <summary>
        /// Выполняет действия, связанные со значительным изменением размеров фигуры.
        /// </summary>
        /// <param name="prevSize">Предыдущий размер фигуры.</param>
        /// <param name="prevPosition">Предыдущее положение фигуры.</param>
        public void OnSizeChanged(Size prevSize, Point prevPosition)
        {
            // Запоминаем изменение размера в истории изменений.
            this.HistoryService.Add(new HistoryEntry(new SetPropertyAction(this, nameof(this.Size), new Tuple<Size, Point>(prevSize, prevPosition), new Tuple<Size, Point>(this.Size, this.Position)), Target.Data, "изменение размера фигуры"));
        }

        /// <summary>
        /// Сбрасывает настройки надписи.
        /// </summary>
        public void ResetLabel()
        {
            this.MapBindingService.ResetLabel(this);

            this.IsModified = true;
        }

        #endregion
    }

    // Реализация ObjectViewModel.
    internal abstract partial class FigureViewModel
    {
        #region Открытые переопределенные свойства

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

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsInitialized), value);
                }
            }
        }

        #endregion

        #region Открытые переопределенные непереопределяемые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public override sealed bool IsActive
        {
            get
            {
                return this.Figure.IsActive;
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
                    this.Figure.IsActive = value;

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsActive), value);
                    
                    if (wasSelected)
                        this.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        public override sealed bool IsPlanning
        {
            get
            {
                return this.Figure.IsPlanning;
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
                    this.Figure.IsPlanning = value;

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlanning), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public override sealed ObjectType Type
        {
            get
            {
                return this.Figure.Type;
            }
            set
            {
                if (this.IsInitialized && this.Type != value)
                {
                    // Перемещаем объект со слоя на слой.
                    this.layerHolder.ClearSelectedGroup();
                    var curLayer = this.layerHolder.GetLayer(this.Type, this.IsActive ? (this.IsPlanning ? LayerType.Planning : LayerType.Standart) : LayerType.Disabled);
                    var tarLayer = this.layerHolder.GetLayer(value, this.IsActive ? (this.IsPlanning ? LayerType.Planning : LayerType.Standart) : LayerType.Disabled);
                    curLayer.MoveTo(this, tarLayer);

                    // Только потом меняем тип.
                    this.Figure.Type = value;

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.Type), value);
                }
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
                // Сохраняем значения измененных параметров фигуры.
                if (this.IsSaved)
                    this.DataService.ParameterAccessService.UpdateObjectParamValues(this.Figure, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                else
                {
                    this.Figure.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.Figure, this.ChangedParameterValues, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }
            else
                if (!this.IsSaved)
                {
                    this.Figure.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.Figure, this.ChangedParameterValues, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }

            // Сохраняем дочерние объекты фигуры.
            foreach (var layer in this.ChildrenLayers)
                foreach (var obj in layer.Objects)
                    obj.BeginSave();

            if (this.IsModified)
                this.DataService.FigureAccessService.UpdateObject(this.Figure, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        public override void EndSave()
        {
            if (!this.isSaveStarted)
                return;

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
                this.Figure.Id = ObjectModel.DefaultId;
        }

        #endregion
    }

    // Реализация IContainerObjectViewModel.
    internal abstract partial class FigureViewModel
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
                return this.Figure.ChildrenTypes;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.Figure.HasChildren;
            }
        }

        /// <summary>
        /// Возвращает модели представлений выбора дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<SelectChildViewModel> SelectChildViewModels
        {
            get
            {
                // Если дочерние объекты еще не загружены, то загружаем их.
                if (!this.AreChildrenLoaded)
                    this.LoadChildren();

                this.containerObject.SelectChildViewModels.Clear();

                foreach (var layer in this.ChildrenLayers)
                    foreach (var obj in layer.Objects)
                        this.containerObject.SelectChildViewModels.Add(new SelectChildViewModel(obj));

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
    internal abstract partial class FigureViewModel
    {
        #region Открытые абстрактные методы

        /// <summary>
        /// Возвращает копию объекта.
        /// </summary>
        /// <returns>Копия объекта.</returns>
        public abstract ICopyableObjectViewModel Copy();

        #endregion
    }

    // Реализация IDeletableObjectViewModel.
    internal abstract partial class FigureViewModel
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
            this.DataService.FigureAccessService.DeleteObject(this.Figure, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        public void MarkFullDelete()
        {
            this.DataService.FigureAccessService.MarkDeleteObject(this.Figure, this.layerHolder.CurrentSchema);
        }

        #endregion
    }

    // Реализация IEditableObjectViewModel.
    internal abstract partial class FigureViewModel
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

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsEditing), value);
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

    // Реализация IExpandableObjectViewModel.
    internal abstract partial class FigureViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что раскрыт ли объект.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.expandableObject.GetIsExpanded();
            }
            set
            {
                if (this.IsExpanded != value)
                    this.expandableObject.SetIsExpanded(value);
            }
        }

        #endregion
    }

    // Реализация IHighlightableObjectViewModel.
    internal abstract partial class FigureViewModel
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
                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsHighlighted), value);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Убирает выделение объекта.
        /// </summary>
        public void HighlightOff()
        {
            this.MapBindingService.AnimateOff();
        }

        /// <summary>
        /// Выделяет объект.
        /// </summary>
        public void HighlightOn()
        {
            this.MapBindingService.AnimateOn(this);
        }

        /// <summary>
        /// Сбрасывает выделение объекта.
        /// </summary>
        public void ResetHighlight()
        {
            this.MapBindingService.AnimateOff();
        }

        #endregion
    }

    // Реализация IMapObjectViewModel.
    internal abstract partial class FigureViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get
            {
                return this.Figure.IsPlaced;
            }
            set
            {
                this.Figure.IsPlaced = value;

                this.NotifyPropertyChanged(nameof(this.IsPlaced));

                this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlaced), value);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Регистрирует связь с представлением.
        /// </summary>
        public void RegisterBinding()
        {
            this.MapBindingService.RegisterBinding(this);
        }

        /// <summary>
        /// Вращает объект карты на заданный угол.
        /// </summary>
        /// <param name="angle">Угол.</param>
        /// <param name="origin">Точка, относительно которой выполняется поворот.</param>
        public void Rotate(double angle, Point origin)
        {
            // Сперва сдвигаем объект. Для этого вращаем центральную точку фигуры относительно заданной точки.
            var oX = origin.X;
            var oY = origin.Y;
            var x = this.Position.X + this.Size.Width / 2;
            var y = this.Position.Y + this.Size.Height / 2;
            var a = angle * Math.PI / 180;
            var newX = oX + (x - oX) * Math.Cos(a) + (oY - y) * Math.Sin(a);
            var newY = oY + (y - oY) * Math.Cos(a) + (x - oX) * Math.Sin(a);
            this.Shift(new Point(newX - x, newY - y));

            this.SetValue(nameof(this.Angle), this.Angle + angle);
        }

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        public void Scale(double scale, Point origin)
        {
            this.SetValue(nameof(this.Size), new Tuple<Size, Point>(new Size(this.Size.Width * scale, this.Size.Height * scale), new Point(origin.X - (origin.X - this.Position.X) * scale, origin.Y - (origin.Y - this.Position.Y) * scale)));
        }

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void Shift(Point delta)
        {
            var oldPosition = this.Position;

            this.SetValue(nameof(this.Position), new Point(oldPosition.X + delta.X, oldPosition.Y + delta.Y));
        }

        /// <summary>
        /// Убирает связь с представлением.
        /// </summary>
        public void UnregisterBinding()
        {
            this.MapBindingService.UnregisterBinding(this);
        }

        #endregion
    }

    // Реализация INamedObjectViewModel.
    internal abstract partial class FigureViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string Name
        {
            get
            {
                return this.namedObject.Name;
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
    internal abstract partial class FigureViewModel
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
            var calcParameterValues = this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.Figure, this.layerHolder.CurrentSchema);

            this.CalcParameterValuesViewModel = new ParameterValueSetViewModel(this, calcParameterValues, true, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений вычисляемых параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadCalcParameterValuesAsync(CancellationToken cancellationToken)
        {
            //var calcParameterValues = await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.Figure, this.layerHolder.CurrentSchema, cancellationToken);
            var calcParameterValues = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.Figure, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.Figure, this.layerHolder.CurrentSchema, cancellationToken);

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
            throw new NotSupportedException("Фигура не имеет общие параметры");
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта.
        /// </summary>
        public void LoadParameterValues()
        {
            var temp = this.DataService.ParameterAccessService.GetObjectParamValues(this.Figure, this.layerHolder.CurrentSchema);

            foreach (var entry in this.ChangedParameterValues.ParameterValues)
                temp.ParameterValues[entry.Key] = entry.Value;

            this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, !this.IsPlaced, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
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

        int RegionId = 0;

        /// <summary>
        /// Выполняет асинхронную загрузку значений параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadParameterValuesAsync(CancellationToken cancellationToken)
        {
            //this.Figure.Type.Parameters[137].Table.Clear();


            if (this.RegionId <= 0)
            {
                
                //DataService.FigureAccessService
                //DataService.MapAccessService
                //this.RegionId = await DataService.MeterAccessService.getRegionID(this.CityId);
            }

            //var temp = await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.Figure, this.layerHolder.CurrentSchema, cancellationToken);
            var temp = (this.DataService is WcfDataService) ? //this.DataService.ParameterAccessService.GetObjectParamValues(this.Figure, this.layerHolder.CurrentSchema)

                await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.Figure, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.Figure, this.layerHolder.CurrentSchema, cancellationToken);





            if (!cancellationToken.IsCancellationRequested)
            {
                
                {
                    //this.Figure.Type.Parameters[137].Table
                    //this.Figure.Type.Parameters[137].Table = this.DataService.getTableMeterParamsForRegion(137);
                }

                foreach (var entry in this.ChangedParameterValues.ParameterValues)
                {
                    temp.ParameterValues[entry.Key] = entry.Value;
                }
                    

                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, !this.IsPlaced, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
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
            var action = new ChangeParameterAction(this, param, prevValue, newValue);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение значения параметра фигуры"));
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
            throw new NotSupportedException("Фигура не имеет общие параметры");
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
    internal abstract partial class FigureViewModel
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

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.IsSelected), value);
                    
                    // Управляем возможностью добавления дочерних объектов.
                    if (value)
                        foreach (var type in this.Type.Children.OrderBy(x => x.SingularName))
                        {
                            var addChildViewModel = new AddChildViewModel(this, type, type.ObjectKind == ObjectKind.Badge);

                            this.AddChildViewModels.Add(addChildViewModel);
                        }
                    else
                        this.AddChildViewModels.Clear();

                    if (this.IsPlaced)
                        this.layerHolder.SelectedObject = value ? this : null;
                    else
                        this.layerHolder.SelectedNotPlacedObject = value ? this : null;
                }
            }
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal abstract partial class FigureViewModel
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
                case nameof(this.Angle):
                    this.Figure.Angle = (double)value;

                    break;

                case nameof(this.LabelAngle):
                    this.Figure.LabelAngle = (double?)value;

                    break;

                case nameof(this.LabelPosition):
                    var labelPosition = (Point?)value;

                    if (labelPosition.HasValue)
                        this.Figure.LabelPosition = new Utilities.Point(labelPosition.Value.X, labelPosition.Value.Y);
                    else
                        this.Figure.LabelPosition = null;

                    break;

                case nameof(this.LabelSize):
                    this.Figure.LabelSize = (int?)value;

                    break;

                case nameof(this.Position):
                    var position = (Point)value;

                    this.Figure.Position.X = position.X;
                    this.Figure.Position.Y = position.Y;

                    break;

                case nameof(this.Size):
                    var tuple = (Tuple<Size, Point>)value;

                    this.Figure.Size.Width = tuple.Item1.Width;
                    this.Figure.Size.Height = tuple.Item1.Height;

                    this.Figure.Position.X = tuple.Item2.X;
                    this.Figure.Position.Y = tuple.Item2.Y;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.MapBindingService.SetMapObjectViewValue(this, propertyName, value);

            this.IsModified = true;
        }

        #endregion
    }
}