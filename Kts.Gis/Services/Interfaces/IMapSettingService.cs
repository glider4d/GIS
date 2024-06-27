using Kts.Gis.Models;

namespace Kts.Gis.Services
{
    /// <summary>
    /// Представляет интерфейс сервиса настроек вида карты.
    /// </summary>
    internal interface IMapSettingService
    {
        #region Свойства

        /// <summary>
        /// Возвращает размер надписи фигуры по умолчанию.
        /// </summary>
        int FigureLabelDefaultSize
        {
            get;
        }

        /// <summary>
        /// Возвращает минимальный размер фигуры.
        /// </summary>
        double FigureMinSize
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ внутри границы планируемой фигуры.
        /// </summary>
        double FigurePlanningOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает размер изменялки фигуры.
        /// </summary>
        double FigureResizeThumbSize
        {
            get;
        }

        /// <summary>
        /// Возвращает размер крутилки фигуры.
        /// </summary>
        double FigureRotateThumbSize
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину границы выбранной фигуры.
        /// </summary>
        double FigureSelectedThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину границы фигуры.
        /// </summary>
        double FigureThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину границы изменялки фигуры.
        /// </summary>
        double FigureThumbThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает размер независимой надписи по умолчанию.
        /// </summary>
        int IndependentLabelDefaultSize
        {
            get;
        }

        /// <summary>
        /// Возвращает размер кнопки изменения размера надписи.
        /// </summary>
        double LabelResizeButtonSize
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ внутри отключенной линии.
        /// </summary>
        double LineDisabledOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает размер надписи линии по умолчанию.
        /// </summary>
        int LineLabelDefaultSize
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ внутри планируемой линии.
        /// </summary>
        double LinePlanningOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает размер точки изгиба линии.
        /// </summary>
        double LinePointThumbSize
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину обводки точки изгиба линии.
        /// </summary>
        double LinePointThumbThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину выбранной линии.
        /// </summary>
        double LineSelectedThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину линии.
        /// </summary>
        double LineThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ краев новой линейки.
        /// </summary>
        double NewRulerEdgeOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает максимальное расстояние между первой и последней точкой в новой линейке, которое будет достаточно для завершения ее рисования.
        /// </summary>
        double NewRulerMaxPointDistance
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину новой линейки.
        /// </summary>
        double NewRulerThickness
        {
            get;
        }

        double SchemaThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает размер узла.
        /// </summary>
        double NodeSize
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину обводки узла.
        /// </summary>
        double NodeThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ по X треугольника узла.
        /// </summary>
        double NodeTriangleXOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ по Y треугольника узла.
        /// </summary>
        double NodeTriangleYOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает максимальное расстояние между первой и последней точкой в новом полигоне, которое будет достаточно для завершения его рисования.
        /// </summary>
        double PolygonMaxPointDistance
        {
            get;
        }

        /// <summary>
        /// Возвращает размер изменялки вершины многоугольника.
        /// </summary>
        double PolygonVertexThumbSize
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину обводки изменялки вершины многоугольника.
        /// </summary>
        double PolygonVertexThumbThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ внутри отключенного полилайна.
        /// </summary>
        double PolylineDisabledOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает максимальное расстояние между первой и последней точкой в новом полилайне, которое будет достаточно для завершения его рисования.
        /// </summary>
        double PolylineMaxPointDistance
        {
            get;
        }

        /// <summary>
        /// Возвращает минимальное расстояние между первой и последней точкой в новом полилайне, которое будет достаточно для завершения его рисования.
        /// </summary>
        double PolylineMinPointDistance
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ между линиями полилайна.
        /// </summary>
        double PolylineOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ внутри планируемого полилайна.
        /// </summary>
        double PolylinePlanningOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину полилайна.
        /// </summary>
        double PolylineThickness
        {
            get;
        }

        /// <summary>
        /// Возвращает отступ краев линейки.
        /// </summary>
        double RulerEdgeOffset
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину линейки.
        /// </summary>
        double RulerThickness
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загружает настройки вида карты.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        void Load(int cityId);

        /// <summary>
        /// Загружает настройки вида карты из заданной модели.
        /// </summary>
        /// <param name="model">Модель настроек вида карты.</param>
        void Load(MapSettingsModel model);

        /// <summary>
        /// Сохраняет текущие настройки вида карты.
        /// </summary>
        void SaveCurrent();

        #endregion
    }
}