using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель настроек вида карты.
    /// </summary>
    [Serializable]
    public sealed class MapSettingsModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MapSettingsModel"/>.
        /// </summary>
        /// <param name="figureLabelDefaultSize">Размер надписи фигуры по умолчанию.</param>
        /// <param name="figurePlanningOffset">Отступ внутри границы планируемой фигуры.</param>
        /// <param name="figureThickness">Толщина границы фигуры.</param>
        /// <param name="independentLabelDefaultSize">Размер независимой надписи по умолчанию.</param>
        /// <param name="lineDisabledOffset">Отступ внутри отключенной линии.</param>
        /// <param name="lineLabelDefaultSize">Размер надписи линии по умолчанию.</param>
        /// <param name="linePlanningOffset">Отступ внутри планируемой линии.</param>
        /// <param name="lineThickness">Толщина линии.</param>
        public MapSettingsModel(int figureLabelDefaultSize, double figurePlanningOffset, double figureThickness, int independentLabelDefaultSize, double lineDisabledOffset, int lineLabelDefaultSize, double linePlanningOffset, double lineThickness)
        {
            this.FigureLabelDefaultSize = figureLabelDefaultSize;
            this.FigurePlanningOffset = figurePlanningOffset;
            this.FigureThickness = figureThickness;
            this.IndependentLabelDefaultSize = independentLabelDefaultSize;
            this.LineDisabledOffset = lineDisabledOffset;
            this.LineLabelDefaultSize = lineLabelDefaultSize;
            this.LinePlanningOffset = linePlanningOffset;
            this.LineThickness = lineThickness;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает размер надписи фигуры по умолчанию.
        /// </summary>
        public int FigureLabelDefaultSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает отступ внутри границы планируемой фигуры.
        /// </summary>
        public double FigurePlanningOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает толщину границы фигуры.
        /// </summary>
        public double FigureThickness
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает размер независимой надписи по умолчанию.
        /// </summary>
        public int IndependentLabelDefaultSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает отступ внутри отключенной линии.
        /// </summary>
        public double LineDisabledOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает размер надписи линии по умолчанию.
        /// </summary>
        public int LineLabelDefaultSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает отступ внутри планируемой линии.
        /// </summary>
        public double LinePlanningOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает толщину линии.
        /// </summary>
        public double LineThickness
        {
            get;
            set;
        }

        #endregion
    
        #region Открытые методы

        /// <summary>
        /// Выполняет клонирование модели настроек вида карты.
        /// </summary>
        /// <returns>Клонированная модель.</returns>
        public MapSettingsModel Clone()
        {
            return new MapSettingsModel(this.FigureLabelDefaultSize,
                                        this.FigurePlanningOffset,
                                        this.FigureThickness,
                                        this.IndependentLabelDefaultSize,
                                        this.LineDisabledOffset,
                                        this.LineLabelDefaultSize,
                                        this.LinePlanningOffset,
                                        this.LineThickness);
        }

        #endregion
    }
}