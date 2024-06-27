using System;
using Kts.Gis.Data;
using Kts.Gis.Models;

namespace Kts.Gis.Services
{
    /// <summary>
    /// Представляет сервис настроек вида карты, хранящий данные в базе данных SQL.
    /// </summary>
    internal sealed partial class SqlMapSettingService : IMapSettingService
    {
        #region Закрытые поля

        /// <summary>
        /// Идентификатор населенного пункта.
        /// </summary>
        private int cityId;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        public double SchemaThickness
        {
            get;
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlMapSettingService"/>
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public SqlMapSettingService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Загружает настройки из заданной модели.
        /// </summary>
        /// <param name="model">Модель настроек вида карты.</param>
        private void LoadFromModel(MapSettingsModel model)
        {
            this.FigureThickness = model.FigureThickness;
            this.FigureLabelDefaultSize = model.FigureLabelDefaultSize;
            this.FigurePlanningOffset = model.FigurePlanningOffset;
            this.LineThickness = model.LineThickness;
            this.LinePlanningOffset = model.LinePlanningOffset;
            this.LineDisabledOffset = model.LineDisabledOffset;
            this.LineLabelDefaultSize = model.LineLabelDefaultSize;
            this.IndependentLabelDefaultSize = model.IndependentLabelDefaultSize;
        }
        
        /// <summary>
        /// Загружает значения по умолчанию.
        /// </summary>
        private void LoadDefault()
        {
            this.FigureThickness = 1;
            this.FigureLabelDefaultSize = 5;
            this.FigurePlanningOffset = 2;
            this.LineThickness = 1;
            this.LinePlanningOffset = 2;
            this.LineDisabledOffset = 1;
            this.LineLabelDefaultSize = 3;
            this.IndependentLabelDefaultSize = 50;
        }

        #endregion
    }

    // Реализация IMapSettingService.
    internal sealed partial class SqlMapSettingService
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает размер надписи фигуры по умолчанию.
        /// </summary>
        public int FigureLabelDefaultSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает минимальный размер фигуры.
        /// </summary>
        public double FigureMinSize
        {
            get
            {
                return this.FigureThickness * 10;
            }
        }

        /// <summary>
        /// Возвращает или задает отступ внутри границы планируемой фигуры.
        /// </summary>
        public double FigurePlanningOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает размер изменялки фигуры.
        /// </summary>
        public double FigureResizeThumbSize
        {
            get
            {
                return this.FigureThickness * 4;
            }
        }

        /// <summary>
        /// Возвращает размер крутилки фигуры.
        /// </summary>
        public double FigureRotateThumbSize
        {
            get
            {
                return this.FigureThickness * 10;
            }
        }

        /// <summary>
        /// Возвращает толщину границы выбранной фигуры.
        /// </summary>
        public double FigureSelectedThickness
        {
            get
            {
                return this.FigureThickness * 2;
            }
        }

        /// <summary>
        /// Возвращает или задает толщину границы фигуры.
        /// </summary>
        public double FigureThickness
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает толщину границы изменялки фигуры.
        /// </summary>
        public double FigureThumbThickness
        {
            get
            {
                return this.FigureThickness;
            }
        }

        /// <summary>
        /// Возвращает или задает размер независимой надписи по умолчанию.
        /// </summary>
        public int IndependentLabelDefaultSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает размер кнопки изменения размера надписи.
        /// </summary>
        public double LabelResizeButtonSize
        {
            get
            {
#warning Возможно, что это свойство позже придется удалить, так как оно не используется в приложении
                return 4;
            }
        }

        /// <summary>
        /// Возвращает или задает отступ внутри отключенной линии.
        /// </summary>
        public double LineDisabledOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает размер надписи линии по умолчанию.
        /// </summary>
        public int LineLabelDefaultSize
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Возвращает или задает отступ внутри планируемой линии.
        /// </summary>
        public double LinePlanningOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает размер точки изгиба линии.
        /// </summary>
        public double LinePointThumbSize
        {
            get
            {
                return this.LineThickness * 2;
            }
        }

        /// <summary>
        /// Возвращает толщину обводки точки изгиба линии.
        /// </summary>
        public double LinePointThumbThickness
        {
            get
            {
                return this.LineThickness;
            }
        }

        /// <summary>
        /// Возвращает толщину выбранной линии.
        /// </summary>
        public double LineSelectedThickness
        {
            get
            {
                return this.LineThickness * 2;
            }
        }

        /// <summary>
        /// Возвращает или задает толщину линии.
        /// </summary>
        public double LineThickness
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает отступ краев новой линейки.
        /// </summary>
        public double NewRulerEdgeOffset
        {
            get
            {
                return this.NewRulerThickness + this.NewRulerThickness * 2;
            }
        }

        /// <summary>
        /// Возвращает максимальное расстояние между первой и последней точкой в новой линейке, которое будет достаточно для завершения ее рисования.
        /// </summary>
        public double NewRulerMaxPointDistance
        {
            get
            {
                return this.PolylineMaxPointDistance;
            }
        }

        /// <summary>
        /// Возвращает толщину новой линейки.
        /// </summary>
        public double NewRulerThickness
        {
            get
            {
                return this.LineThickness;
            }
        }

        /// <summary>
        /// Возвращает размер узла.
        /// </summary>
        public double NodeSize
        {
            get
            {
                return this.LineThickness * 2;
            }
        }

        /// <summary>
        /// Возвращает толщину обводки узла.
        /// </summary>
        public double NodeThickness
        {
            get
            {
                return this.LineThickness;
            }
        }

        /// <summary>
        /// Возвращает отступ по X треугольника узла.
        /// </summary>
        public double NodeTriangleXOffset
        {
            get
            {
                return this.NodeThickness + 2;
            }
        }

        /// <summary>
        /// Возвращает отступ по Y треугольника узла.
        /// </summary>
        public double NodeTriangleYOffset
        {
            get
            {
                return this.NodeTriangleXOffset * 2;
            }
        }

        /// <summary>
        /// Возвращает максимальное расстояние между первой и последней точкой в новом полигоне, которое будет достаточно для завершения его рисования.
        /// </summary>
        public double PolygonMaxPointDistance
        {
            get
            {
                return this.PolylineMaxPointDistance;
            }
        }

        /// <summary>
        /// Возвращает размер изменялки вершины многоугольника.
        /// </summary>
        public double PolygonVertexThumbSize
        {
            get
            {
                return this.FigureThickness * 4;
            }
        }

        /// <summary>
        /// Возвращает толщину обводки изменялки вершины многоугольника.
        /// </summary>
        public double PolygonVertexThumbThickness
        {
            get
            {
                return this.FigureThickness;
            }
        }

        /// <summary>
        /// Возвращает отступ внутри отключенного полилайна.
        /// </summary>
        public double PolylineDisabledOffset
        {
            get
            {
                return this.LineDisabledOffset;
            }
        }

        /// <summary>
        /// Возвращает максимальное расстояние между первой и последней точкой в новом полилайне, которое будет достаточно для завершения его рисования.
        /// </summary>
        public double PolylineMaxPointDistance
        {
            get
            {
                return this.LineThickness * 5;
            }
        }

        /// <summary>
        /// Возвращает минимальное расстояние между первой и последней точкой в новом полилайне, которое будет достаточно для завершения его рисования.
        /// </summary>
        public double PolylineMinPointDistance
        {
            get
            {
                return this.LineThickness * 3;
            }
        }

        /// <summary>
        /// Возвращает отступ между линиями полилайна.
        /// </summary>
        public double PolylineOffset
        {
            get
            {
                return this.LineThickness * 3;
            }
        }

        /// <summary>
        /// Возвращает отступ внутри планируемого полилайна.
        /// </summary>
        public double PolylinePlanningOffset
        {
            get
            {
                return this.LinePlanningOffset;
            }
        }

        /// <summary>
        /// Возвращает толщину полилайна.
        /// </summary>
        public double PolylineThickness
        {
            get
            {
                return this.LineThickness;
            }
        }

        /// <summary>
        /// Возвращает отступ краев линейки.
        /// </summary>
        public double RulerEdgeOffset
        {
            get
            {
                return this.RulerThickness + this.RulerThickness * 2;
            }
        }

        /// <summary>
        /// Возвращает толщину линейки.
        /// </summary>
        public double RulerThickness
        {
            get
            {
                return this.LineThickness;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Загружает настройки вида карты.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        public void Load(int cityId)
        {
            this.cityId = cityId;

            var settings = this.dataService.MapAccessService.GetSettings(cityId);

            if (settings == null)
                this.LoadDefault();
            else
                this.LoadFromModel(settings);
        }

        /// <summary>
        /// Загружает настройки вида карты из заданной модели.
        /// </summary>
        /// <param name="model">Модель настроек вида карты.</param>
        public void Load(MapSettingsModel model)
        {
            this.LoadFromModel(model);
        }

        /// <summary>
        /// Сохраняет текущие настройки вида карты.
        /// </summary>
        public void SaveCurrent()
        {
            var settings = new MapSettingsModel(this.FigureLabelDefaultSize, this.FigurePlanningOffset, this.FigureThickness, this.IndependentLabelDefaultSize, this.LineDisabledOffset, this.LineLabelDefaultSize, this.LinePlanningOffset, this.LineThickness);

            this.dataService.MapAccessService.UpdateSettings(this.cityId, settings);
        }

        #endregion
    }
}