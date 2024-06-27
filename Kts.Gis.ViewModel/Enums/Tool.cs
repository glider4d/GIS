using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Инструмент.
    /// </summary>
    internal enum Tool
    {
        /// <summary>
        /// Изменялка.
        /// </summary>
        Editor = 0,

        /// <summary>
        /// Эллипс.
        /// </summary>
        Ellipse = 1,

        /// <summary>
        /// Область редактирования группы объектов.
        /// </summary>
        GroupArea = 9,

        /// <summary>
        /// Надпись.
        /// </summary>
        Label = 8,

        /// <summary>
        /// Линия.
        /// </summary>
        Line = 2,

        /// <summary>
        /// Новая линейка.
        /// </summary>
        NewRuler = 10,

        /// <summary>
        /// Планируемая линия.
        /// </summary>
        [Obsolete("С появлением схем, необходимость в отдельном инструменте для рисования планируемых линий пропала")]
        PlanningLine = 3,

        /// <summary>
        /// Многоугольник.
        /// </summary>
        Polygon = 4,

        /// <summary>
        /// Область печати.
        /// </summary>
        PrintArea = 11,

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        Rectangle = 5,

        /// <summary>
        /// Линейка.
        /// </summary>
        Ruler = 6,

        /// <summary>
        /// Выбиралка.
        /// </summary>
        Selector = 7
    }
}