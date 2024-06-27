using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления панели инструментов.
    /// </summary>
    internal sealed class ToolBarViewModel : BaseViewModel
    {
        #region Закрытые поля
    
        /// <summary>
        /// Значение, указывающее на то, что можно ли рисовать фигуры.
        /// </summary>
        private bool canDrawFigures;

        /// <summary>
        /// Значение, указывающее на то, что можно ли рисовать линии.
        /// </summary>
        private bool canDrawLines;

        /// <summary>
        /// Значение, указывающее на то, что можно ли редактировать фигуры.
        /// </summary>
        private bool canEdit;

        /// <summary>
        /// Значение, указывающее на то, что можно ли редактировать группу объектов.
        /// </summary>
        private bool canEditGroup;

        /// <summary>
        /// Значение, указывающее на то, что можно ли группировать линии.
        /// </summary>
        private bool canGroupLines;

        /// <summary>
        /// Значение, указывающее на то, что можно ли выбрать тип рисуемого объекта, представляемого фигурой на карте.
        /// </summary>
        private bool canSelectFigureType;

        /// <summary>
        /// Значение, указывающее на то, что можно ли выбрать типы рисуемых объектов, представляемых линиями на карте.
        /// </summary>
        private bool canSelectLineTypes;

        /// <summary>
        /// Значение, указывающее на то, что можно ли выбрать линейку.
        /// </summary>
        private bool canUseRuler;

        /// <summary>
        /// Значение, указывающее на то, включена ли панель инструментов.
        /// </summary>
        private bool isToolBarEnabled;

        /// <summary>
        /// Выбранный тип рисуемого объекта, представляемого фигурой на карте.
        /// </summary>
        private ObjectType selectedFigureType;

        /// <summary>
        /// Выбранный инструмент.
        /// </summary>
        private Tool selectedTool = Tool.Selector;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private readonly AccessService accessService;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса уменьшения размера шрифта надписей.
        /// </summary>
        public event EventHandler DecreaseFontRequested;

        /// <summary>
        /// Событие запроса увеличения размера шрифта надписей.
        /// </summary>
        public event EventHandler IncreaseFontRequested;

        /// <summary>
        /// Событие запроса сброса настроек надписей.
        /// </summary>
        public event EventHandler ResetLabelsRequested;

        /// <summary>
        /// События запуска редактирования группы объектов.
        /// </summary>
        public event EventHandler<StartGroupEditEventArgs> StartGroupEdit;

        /// <summary>
        /// События остановки редактирования группы объектов.
        /// </summary>
        public event EventHandler StopGroupEdit;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ToolBarViewModel"/>.
        /// </summary>
        /// <param name="figureTypes">Типы объектов, представляемых фигурами на карте.</param>
        /// <param name="lineTypes">Типы объектов, представляемых линиями на карте.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="groupLinesCommand">Команда группировки линий.</param>
        /// <param name="ungroupLinesCommand">Команда разгруппировки линий.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ToolBarViewModel(List<ObjectType> figureTypes, List<LineTypeViewModel> lineTypes, ILayerHolder layerHolder, AccessService accessService, RelayCommand groupLinesCommand, RelayCommand ungroupLinesCommand, IMessageService messageService)
        {
            this.FigureTypes = figureTypes;
            this.LineTypes = lineTypes;
            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.messageService = messageService;

            foreach (var lineType in lineTypes)
                lineType.PropertyChanged += this.LineType_PropertyChanged;

            this.SelectedFigureType = figureTypes[0];

            this.DecreaseFontCommand = new RelayCommand(this.ExecuteDecreaseFont);
            this.GroupLinesCommand = groupLinesCommand;
            this.IncreaseFontCommand = new RelayCommand(this.ExecuteIncreaseFont);
            this.ResetLabelsCommand = new RelayCommand(this.ExecuteResetLabels);
            this.UngroupLinesCommand = ungroupLinesCommand;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли рисовать фигуры.
        /// </summary>
        public bool CanDrawFigures
        {
            get
            {
                return this.canDrawFigures;
            }
            private set
            {
                this.canDrawFigures = value;

                this.NotifyPropertyChanged(nameof(this.CanDrawFigures));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли рисовать линии.
        /// </summary>
        public bool CanDrawLines
        {
            get
            {
                return this.canDrawLines;
            }
            private set
            {
                this.canDrawLines = value;

                this.NotifyPropertyChanged(nameof(this.CanDrawLines));
            }
        }

        private bool canChangeFont;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли менять размер шрифта.
        /// </summary>
        public bool CanChangeFont
        {
            get
            {
                return this.canChangeFont;
            }
            private set
            {
                this.canChangeFont = value;

                this.NotifyPropertyChanged(nameof(this.CanChangeFont));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли редактировать фигуры.
        /// </summary>
        public bool CanEdit
        {
            get
            {
                return this.canEdit;
            }
            private set
            {
                this.canEdit = value;

                this.NotifyPropertyChanged(nameof(this.CanEdit));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли редактировать группу объектов.
        /// </summary>
        public bool CanEditGroup
        {
            get
            {
                return this.canEditGroup;
            }
            private set
            {
                this.canEditGroup = value;

                this.NotifyPropertyChanged(nameof(this.CanEditGroup));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли группировать линии.
        /// </summary>
        public bool CanGroupLines
        {
            get
            {
                return this.canGroupLines;
            }
            private set
            {
                this.canGroupLines = value;

                this.NotifyPropertyChanged(nameof(this.CanGroupLines));
            }
        }
        
        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли выбрать тип рисуемого объекта, представляемого фигурой на карте.
        /// </summary>
        public bool CanSelectFigureType
        {
            get
            {
                return this.canSelectFigureType;
            }
            private set
            {
                this.canSelectFigureType = value;

                this.NotifyPropertyChanged(nameof(this.CanSelectFigureType));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли выбрать типы рисуемых объектов, представляемых линиями на карте.
        /// </summary>
        public bool CanSelectLineTypes
        {
            get
            {
                return this.canSelectLineTypes;
            }
            private set
            {
                this.canSelectLineTypes = value;

                this.NotifyPropertyChanged(nameof(this.CanSelectLineTypes));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли выбрать линейку.
        /// </summary>
        public bool CanUseRuler
        {
            get
            {
                return this.canUseRuler;
            }
            private set
            {
                this.canUseRuler = value;

                this.NotifyPropertyChanged(nameof(this.CanUseRuler));
            }
        }

        /// <summary>
        /// Возвращает команду уменьшения размера шрифта надписей.
        /// </summary>
        public RelayCommand DecreaseFontCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает типы объектов, представляемых фигурами на карте.
        /// </summary>
        public List<ObjectType> FigureTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что нужно ли принудительно рисовать только фигуры.
        /// </summary>
        public bool ForceDrawFigures
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду группировки линий.
        /// </summary>
        public RelayCommand GroupLinesCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду увеличения размера шрифта надписей.
        /// </summary>
        public RelayCommand IncreaseFontCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выполняется ли рисование планируемых объектов.
        /// </summary>
        public bool IsDrawPlanning
        {
            get;
            set;
        } = true;

        public bool IsDrawDisable
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что включена ли панель инструментов.
        /// </summary>
        public bool IsToolBarEnabled
        {
            get
            {
                return this.isToolBarEnabled;
            }
            set
            {
                this.isToolBarEnabled = value;

                this.NotifyPropertyChanged(nameof(this.IsToolBarEnabled));
            }
        }

        /// <summary>
        /// Возвращает типы объектов, представляемых линиями на карте.
        /// </summary>
        public List<LineTypeViewModel> LineTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса настроек надписей.
        /// </summary>
        public RelayCommand ResetLabelsCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный тип рисуемого объекта, представляемого фигурой на карте.
        /// </summary>
        public ObjectType SelectedFigureType
        {
            get
            {
                return this.selectedFigureType;
            }
            set
            {
                this.selectedFigureType = value;

                this.NotifyPropertyChanged(nameof(this.SelectedFigureType));
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный инструмент.
        /// </summary>
        public Tool SelectedTool
        {
            get
            {
                return this.selectedTool;
            }
            set
            {
                this.selectedTool = value;

                if (value == Tool.GroupArea)
                {
                    var eventArgs = new StartGroupEditEventArgs();

                    this.StartGroupEdit.Invoke(this, eventArgs);

                    if (!eventArgs.CanStart)
                    {
                        this.selectedTool = Tool.Selector;

                        this.messageService.ShowMessage("Не выбраны объекты для редактирования", "Редактирование группы объектов", MessageType.Information);
                    }
                }
                else
                    this.StopGroupEdit.Invoke(this, EventArgs.Empty);
                    
                this.NotifyPropertyChanged(nameof(this.SelectedTool));
            }
        }

        /// <summary>
        /// Возвращает команду разгруппировки линий.
        /// </summary>
        public RelayCommand UngroupLinesCommand
        {
            get;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> типа объекта, представляемого линией на карте.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LineType_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LineTypeViewModel.IsSelected))
                this.UpdateState();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет уменьшение размера шрифта надписей.
        /// </summary>
        private void ExecuteDecreaseFont()
        {
            this.DecreaseFontRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет увеличение размера шрифта надписей.
        /// </summary>
        private void ExecuteIncreaseFont()
        {
            this.IncreaseFontRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет сброс настроек надписей.
        /// </summary>
        private void ExecuteResetLabels()
        {
            this.ResetLabelsRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Обновляет состояния инструментов.
        /// </summary>
        public void UpdateState()
        {
            var canDraw = false;

            if (this.layerHolder.CurrentSchema != null)
                if (this.layerHolder.CurrentSchema.IsActual)
                    canDraw = this.accessService.CanDraw;
                else
                    if (this.layerHolder.CurrentSchema.IsIS)
                        canDraw = this.accessService.CanDrawIS;

            this.CanDrawFigures = canDraw;
#warning Временный костыль, чтобы все могли менять размер шрифта надписей
            this.CanChangeFont = true;
            this.CanDrawLines = this.LineTypes.Any(x => x.IsSelected) && canDraw && !this.ForceDrawFigures;
            this.CanEdit = canDraw;
            this.CanEditGroup = canDraw;
            this.CanGroupLines = canDraw;
            this.CanSelectFigureType = canDraw && !this.ForceDrawFigures;
            this.CanSelectLineTypes = canDraw && !this.ForceDrawFigures;
            this.CanUseRuler = canDraw && this.layerHolder.CurrentSchema.IsActual;
        }

        #endregion
    }
}