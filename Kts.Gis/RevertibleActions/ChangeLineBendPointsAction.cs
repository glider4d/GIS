using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения точек изгиба линии.
    /// </summary>
    internal sealed partial class ChangeLineBendPointsAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Предыдущая структура точек изгиба линии.
        /// </summary>
        private readonly string prevPoints;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLineBendPointsAction"/>.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="prevPoints">Предыдущая структура точек изгиба линии.</param>
        /// <param name="newPoints">Новая структура точек изгиба линии.</param>
        public ChangeLineBendPointsAction(LineViewModel line, string prevPoints, string newPoints)
        {
            this.Line = line;
            this.prevPoints = prevPoints;
            this.NewPoints = newPoints;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает новую структуру точек изгиба линии.
        /// </summary>
        public string NewPoints
        {
            get;
            set;
        }
        
        /// <summary>
        /// Возвращает многоугольник.
        /// </summary>
        public LineViewModel Line
        {
            get;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangeLineBendPointsAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.Line.Points = this.NewPoints;
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeLineBendPointsAction));
            sb.Append(Environment.NewLine);
            sb.Append("Line: ");
            sb.Append(this.Line.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Points: ");
            sb.Append(this.prevPoints);
            sb.Append(" -> ");
            sb.Append(this.NewPoints);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.Line.Points = this.prevPoints;
        }

        #endregion
    }
}