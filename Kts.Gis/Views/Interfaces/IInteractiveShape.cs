using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерфейс интерактивной фигуры.
    /// </summary>
    internal interface IInteractiveShape : IMapObject
    {
        #region Свойства

        /// <summary>
        /// Возвращает центральную точку фигуры.
        /// </summary>
        Point CenterPoint
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что инициализирована ли фигура.
        /// </summary>
        bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбрана ли фигура.
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что видна ли фигура.
        /// </summary>
        bool IsVisible
        {
            get;
        }

        /// <summary>
        /// Возвращает важную точку фигуры.
        /// </summary>
        Point MajorPoint
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Завершает перемещение фигуры. Используется для закрепления результата работы метода <see cref="MoveTo(Point)"/>.
        /// </summary>
        void EndMoveTo();

        /// <summary>
        /// Перемещает фигуру в заданную точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        void MoveTo(Point point);

        /// <summary>
        /// Начинает перемещение фигуры.
        /// </summary>
        void StartMoveTo();

        #endregion
    }
}