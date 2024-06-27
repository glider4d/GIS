using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Utilities;
using Kts.WpfUtilities;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления настроек вида карты.
    /// </summary>
    internal sealed class MapSettingsViewModel : BaseViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Модель.
        /// </summary>
        private readonly MapSettingsModel model;

        /// <summary>
        /// Предыдущая версия модели.
        /// </summary>
        private readonly MapSettingsModel previousVersion;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MapSettingsViewModel"/>.
        /// </summary>
        /// <param name="model">Модель.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public MapSettingsViewModel(MapSettingsModel model, IMapBindingService mapBindingService)
        {
            this.model = model;
            this.mapBindingService = mapBindingService;

            this.ResetFigureLabelDefaultSizeCommand = new RelayCommand(this.ExecuteResetFigureLabelDefaultSize, this.CanExecuteResetFigureLabelDefaultSize);
            this.ResetFigurePlanningOffsetCommand = new RelayCommand(this.ExecuteResetFigurePlanningOffset, this.CanExecuteResetFigurePlanningOffset);
            this.ResetFigureThicknessCommand = new RelayCommand(this.ExecuteResetFigureThickness, this.CanExecuteResetFigureThickness);
            this.ResetIndependentLabelDefaultSizeCommand = new RelayCommand(this.ExecuteResetIndependentLabelDefaultSize, this.CanExecuteResetIndependentLabelDefaultSize);
            this.ResetLineDisabledOffsetCommand = new RelayCommand(this.ExecuteResetLineDisabledOffset, this.CanExecuteResetLineDisabledOffset);
            this.ResetLineLabelDefaultSizeCommand = new RelayCommand(this.ExecuteResetLineLabelDefaultSize, this.CanExecuteResetLineLabelDefaultSize);
            this.ResetLinePlanningOffsetCommand = new RelayCommand(this.ExecuteResetLinePlanningOffset, this.CanExecuteResetLinePlanningOffset);
            this.ResetLineThicknessCommand = new RelayCommand(this.ExecuteResetLineThickness, this.CanExecuteResetLineThickness);

            this.previousVersion = model.Clone();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает размер шрифта надписи фигуры по умолчанию.
        /// </summary>
        public int FigureLabelDefaultSize
        {
            get
            {
                return this.model.FigureLabelDefaultSize;
            }
            set
            {
                if (value != this.FigureLabelDefaultSize)
                {
                    var prevSize = this.FigureLabelDefaultSize;

                    this.model.FigureLabelDefaultSize = value;

                    this.NotifyPropertyChanged(nameof(this.FigureLabelDefaultSize));

                    this.ResetFigureLabelDefaultSizeCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateFigureLabelDefaultSize(value, prevSize);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальный размер шрифта надписи фигуры по умолчанию.
        /// </summary>
        public int FigureLabelDefaultSizeMax
        {
            get
            {
                return 30;
            }
        }

        /// <summary>
        /// Возвращает минимальный размер шрифта надписи фигуры по умолчанию.
        /// </summary>
        public double FigureLabelDefaultSizeMin
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения размера шрифта надписи фигуры по умолчанию.
        /// </summary>
        public double FigureLabelDefaultSmallChange
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Возвращает отступ внутри границы планируемой фигуры.
        /// </summary>
        public double FigurePlanningOffset
        {
            get
            {
                return this.model.FigurePlanningOffset;
            }
            set
            {
                if (value != this.FigurePlanningOffset)
                {
                    this.model.FigurePlanningOffset = value;

                    this.NotifyPropertyChanged(nameof(this.FigurePlanningOffset));

                    this.ResetFigurePlanningOffsetCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateFigurePlanningOffset(value);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальный отступ внутри границы планируемой фигуры.
        /// </summary>
        public double FigurePlanningOffsetMax
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Возвращает минимальный отступ внутри границы планируемой фигуры.
        /// </summary>
        public double FigurePlanningOffsetMin
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения отступа внутри границы планируемой фигуры.
        /// </summary>
        public double FigurePlanningOffsetSmallChange
        {
            get
            {
                return 0.5;
            }
        }

        /// <summary>
        /// Возвращает или задает толщину обводки фигуры.
        /// </summary>
        public double FigureThickness
        {
            get
            {
                return this.model.FigureThickness;
            }
            set
            {
                if (value != this.FigureThickness)
                {
                    this.model.FigureThickness = value;

                    this.NotifyPropertyChanged(nameof(this.FigureThickness));

                    this.ResetFigureThicknessCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateFigureThickness(value);
                }
            }
        }
        
        /// <summary>
        /// Возвращает максимальную толщину обводки фигуры.
        /// </summary>
        public double FigureThicknessMax
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Возвращает минимальную толщину обводки фигуры.
        /// </summary>
        public double FigureThicknessMin
        {
            get
            {
                return 0.1;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения толщины обводки фигуры.
        /// </summary>
        public double FigureThicknessSmallChange
        {
            get
            {
                return 0.1;
            }
        }

        /// <summary>
        /// Возвращает или задает размер шрифта независимой надписи по умолчанию.
        /// </summary>
        public int IndependentLabelDefaultSize
        {
            get
            {
                return this.model.IndependentLabelDefaultSize;
            }
            set
            {
                if (value != this.IndependentLabelDefaultSize)
                {
                    var prevSize = this.IndependentLabelDefaultSize;

                    this.model.IndependentLabelDefaultSize = value;

                    this.NotifyPropertyChanged(nameof(this.IndependentLabelDefaultSize));

                    this.ResetIndependentLabelDefaultSizeCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateIndependentLabelDefaultSize(value, prevSize);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальный размер шрифта независимой надписи по умолчанию.
        /// </summary>
        public int IndependentLabelDefaultSizeMax
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// Возвращает минимальный размер шрифта независимой надписи по умолчанию.
        /// </summary>
        public double IndependentLabelDefaultSizeMin
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения размера шрифта независимой надписи по умолчанию.
        /// </summary>
        public double IndependentLabelDefaultSizeSmallChange
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Возвращает или задает отступ внутри отключенной линии.
        /// </summary>
        public double LineDisabledOffset
        {
            get
            {
                return this.model.LineDisabledOffset;
            }
            set
            {
                if (value != this.LineDisabledOffset)
                {
                    this.model.LineDisabledOffset = value;

                    this.NotifyPropertyChanged(nameof(this.LineDisabledOffset));

                    this.ResetLineDisabledOffsetCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateLineDisabledOffset(value);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальный отступ внутри отключенной линии.
        /// </summary>
        public double LineDisabledOffsetMax
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Возвращает минимальный отступ внутри отключенной линии.
        /// </summary>
        public double LineDisabledOffsetMin
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения отступа внутри отключенной линии.
        /// </summary>
        public double LineDisabledOffsetSmallChange
        {
            get
            {
                return 0.5;
            }
        }

        /// <summary>
        /// Возвращает или задает размер шрифта надписи линии по умолчанию.
        /// </summary>
        public int LineLabelDefaultSize
        {
            get
            {
                return this.model.LineLabelDefaultSize;
            }
            set
            {
                if (value != this.LineLabelDefaultSize)
                {
                    var prevSize = this.LineLabelDefaultSize;

                    this.model.LineLabelDefaultSize = value;

                    this.NotifyPropertyChanged(nameof(this.LineLabelDefaultSize));

                    this.ResetLineLabelDefaultSizeCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateLineLabelDefaultSize(value, prevSize);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальный размер шрифта надписи линии по умолчанию.
        /// </summary>
        public int LineLabelDefaultSizeMax
        {
            get
            {
                return 30;
            }
        }

        /// <summary>
        /// Возвращает минимальный размер шрифта надписи линии по умолчанию.
        /// </summary>
        public double LineLabelDefaultSizeMin
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения размера шрифта надписи линии по умолчанию.
        /// </summary>
        public double LineLabelDefaultSmallChange
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Возвращает или задает отступ внутри планируемой линии.
        /// </summary>
        public double LinePlanningOffset
        {
            get
            {
                return this.model.LinePlanningOffset;
            }
            set
            {
                if (value != this.LinePlanningOffset)
                {
                    this.model.LinePlanningOffset = value;

                    this.NotifyPropertyChanged(nameof(this.LinePlanningOffset));

                    this.ResetLinePlanningOffsetCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateLinePlanningOffset(value);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальный отступ внутри планируемой линии.
        /// </summary>
        public double LinePlanningOffsetMax
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Возвращает минимальный отступ внутри планируемой линии.
        /// </summary>
        public double LinePlanningOffsetMin
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения отступа внутри планируемойsssssssssssssssssssss линии.
        /// </summary>
        public double LinePlanningOffsetSmallChange
        {
            get
            {
                return 0.5;
            }
        }

        /// <summary>
        /// Возвращает или задает толщину линии.
        /// </summary>
        public double LineThickness
        {
            get
            {
                return this.model.LineThickness;
            }
            set
            {
                if (value != this.LineThickness)
                {
                    var prevThickness = this.LineThickness;

                    this.model.LineThickness = value;

                    this.NotifyPropertyChanged(nameof(this.LineThickness));

                    this.ResetLineThicknessCommand.RaiseCanExecuteChanged();

                    this.mapBindingService.UpdateLineThickness(value, prevThickness);
                }
            }
        }

        /// <summary>
        /// Возвращает максимальную толщину линии.
        /// </summary>
        public double LineThicknessMax
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Возвращает минимальную толщину линии.
        /// </summary>
        public double LineThicknessMin
        {
            get
            {
                return 0.1;
            }
        }

        /// <summary>
        /// Возвращает значение малого изменения толщины линии.
        /// </summary>
        public double LineThicknessSmallChange
        {
            get
            {
                return 0.1;
            }
        }

        /// <summary>
        /// Возвращает команду сброса размера шрифта надписи фигуры по умолчанию.
        /// </summary>
        public RelayCommand ResetFigureLabelDefaultSizeCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса отступа внутри обводки фигуры.
        /// </summary>
        public RelayCommand ResetFigurePlanningOffsetCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса толщины обводки фигуры.
        /// </summary>
        public RelayCommand ResetFigureThicknessCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса размера шрифта независимой надписи по умолчанию.
        /// </summary>
        public RelayCommand ResetIndependentLabelDefaultSizeCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса отступа внутри отключенной линии.
        /// </summary>
        public RelayCommand ResetLineDisabledOffsetCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса размера шрифта надписи линии по умолчанию.
        /// </summary>
        public RelayCommand ResetLineLabelDefaultSizeCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса отступа внутри планируемой линии.
        /// </summary>
        public RelayCommand ResetLinePlanningOffsetCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сброса толщины линии.
        /// </summary>
        public RelayCommand ResetLineThicknessCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс размера шрифта надписи фигуры по умолчанию.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetFigureLabelDefaultSize()
        {
            return this.FigureLabelDefaultSize != this.previousVersion.FigureLabelDefaultSize;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс отступа внутри границы планируемой фигуры.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetFigurePlanningOffset()
        {
            return this.FigurePlanningOffset != this.previousVersion.FigurePlanningOffset;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс толщины обводки фигуры.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetFigureThickness()
        {
            return this.FigureThickness != this.previousVersion.FigureThickness;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс размера шрифта независимой надписи по умолчанию.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetIndependentLabelDefaultSize()
        {
            return this.IndependentLabelDefaultSize != this.previousVersion.IndependentLabelDefaultSize;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс отступа внутри отключенной линии.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetLineDisabledOffset()
        {
            return this.LineDisabledOffset != this.previousVersion.LineDisabledOffset;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс размера шрифта надписи линии по умолчанию.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetLineLabelDefaultSize()
        {
            return this.LineLabelDefaultSize != this.previousVersion.LineLabelDefaultSize;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс отступа внутри планируемой линии.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetLinePlanningOffset()
        {
            return this.LinePlanningOffset != this.previousVersion.LinePlanningOffset;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сброс толщины линии.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteResetLineThickness()
        {
            return this.LineThickness != this.previousVersion.LineThickness;
        }

        /// <summary>
        /// Выполняет сброс размер шрифта надписи фигуры по умолчанию.
        /// </summary>
        private void ExecuteResetFigureLabelDefaultSize()
        {
            this.FigureLabelDefaultSize = this.previousVersion.FigureLabelDefaultSize;
        }

        /// <summary>
        /// Выполняет сброс отступа внутри границы планируемой фигуры.
        /// </summary>
        private void ExecuteResetFigurePlanningOffset()
        {
            this.FigurePlanningOffset = this.previousVersion.FigurePlanningOffset;
        }

        /// <summary>
        /// Выполняет сброс толщины обводки фигуры.
        /// </summary>
        private void ExecuteResetFigureThickness()
        {
            this.FigureThickness = this.previousVersion.FigureThickness;
        }

        /// <summary>
        /// Выполняет сброс размер шрифта независимой надписи по умолчанию.
        /// </summary>
        private void ExecuteResetIndependentLabelDefaultSize()
        {
            this.IndependentLabelDefaultSize = this.previousVersion.IndependentLabelDefaultSize;
        }

        /// <summary>
        /// Выполняет сброс отступа внутри отключенной линии.
        /// </summary>
        private void ExecuteResetLineDisabledOffset()
        {
            this.LineDisabledOffset = this.previousVersion.LineDisabledOffset;
        }

        /// <summary>
        /// Выполняет сброс размер шрифта надписи линии по умолчанию.
        /// </summary>
        private void ExecuteResetLineLabelDefaultSize()
        {
            this.LineLabelDefaultSize = this.previousVersion.LineLabelDefaultSize;
        }

        /// <summary>
        /// Выполняет сброс отступа внутри планируемой линии.
        /// </summary>
        private void ExecuteResetLinePlanningOffset()
        {
            this.LinePlanningOffset = this.previousVersion.LinePlanningOffset;
        }

        /// <summary>
        /// Выполняет сброс толщины линии.
        /// </summary>
        private void ExecuteResetLineThickness()
        {
            this.LineThickness = this.previousVersion.LineThickness;
        }

        #endregion
    }
}