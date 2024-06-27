using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;
using System.Windows;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения вершин многоугольника.
    /// </summary>
    internal sealed partial class ChangePolygonPointsAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Предыдущая структура вершин многоугольника.
        /// </summary>
        private readonly string prevPoints;

        /// <summary>
        /// Предыдущее положение многоугольника.
        /// </summary>
        private readonly Point prevPosition;

        /// <summary>
        /// Предыдущий размер многоугольника.
        /// </summary>
        private readonly Size prevSize;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangePolygonPointsAction"/>.
        /// </summary>
        /// <param name="polygon">Многоугольник.</param>
        /// <param name="prevPoints">Предыдущая структура вершин многоугольника.</param>
        /// <param name="prevPosition">Предыдущее положение многоугольника.</param>
        /// <param name="prevSize">Предыдущий размер многоугольника.</param>
        /// <param name="newPoints">Новая структура вершин многоугольника.</param>
        /// <param name="newPosition">Новое положение многоугольника.</param>
        /// <param name="newSize">Новый размер многоугольника.</param>
        public ChangePolygonPointsAction(PolygonViewModel polygon, string prevPoints, Point prevPosition, Size prevSize, string newPoints, Point newPosition, Size newSize)
        {
            this.Polygon = polygon;
            this.prevPoints = prevPoints;
            this.prevPosition = prevPosition;
            this.prevSize = prevSize;
            this.NewPoints = newPoints;
            this.NewPosition = newPosition;
            this.NewSize = newSize;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает новую структуру вершин многоугольника.
        /// </summary>
        public string NewPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает новое положение многоугольника.
        /// </summary>
        public Point NewPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает новый размер многоугольника.
        /// </summary>
        public Size NewSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает многоугольник.
        /// </summary>
        public PolygonViewModel Polygon
        {
            get;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangePolygonPointsAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.Polygon.SetValue(nameof(PolygonViewModel.Size), new Tuple<Size, Point>(this.NewSize, this.NewPosition));
            this.Polygon.Points = this.NewPoints;
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangePolygonPointsAction));
            sb.Append(Environment.NewLine);
            sb.Append("Polygon: ");
            sb.Append(this.Polygon.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Points: ");
            sb.Append(this.prevPoints);
            sb.Append(" -> ");
            sb.Append(this.NewPoints);
            sb.Append(Environment.NewLine);
            sb.Append("Position: ");
            sb.Append(this.prevPosition);
            sb.Append(" -> ");
            sb.Append(this.NewPosition);
            sb.Append(Environment.NewLine);
            sb.Append("Size: ");
            sb.Append(this.prevSize);
            sb.Append(" -> ");
            sb.Append(this.NewSize);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.Polygon.SetValue(nameof(PolygonViewModel.Size), new Tuple<Size, Point>(this.prevSize, this.prevPosition));
            this.Polygon.Points = this.prevPoints;
        }

        #endregion
    }
}