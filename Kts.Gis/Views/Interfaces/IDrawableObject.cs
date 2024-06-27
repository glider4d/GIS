using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерфейс рисуемого объекта.
    /// </summary>
    internal interface IDrawableObject
    {
        #region Методы

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        void Draw(Point mousePrevPosition, Point mousePosition);

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        bool IsDrawCompleted(Point mousePosition);

        #endregion
    }
}