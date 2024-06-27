using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;
using System.Windows;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения точек линии.
    /// </summary>
    internal sealed partial class ChangeLinePointsAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля
        
        /// <summary>
        /// Линия.
        /// </summary>
        private readonly LineViewModel line;

        /// <summary>
        /// Новая левая точка линии.
        /// </summary>
        private readonly Point newLeftPoint;

        /// <summary>
        /// Новая структура точек изгиба линии.
        /// </summary>
        private readonly string newPoints;

        /// <summary>
        /// Новая правая точка линии.
        /// </summary>
        private readonly Point newRightPoint;

        /// <summary>
        /// Предыдущая структура точек изгиба линии.
        /// </summary>
        private readonly string prevPoints;

        /// <summary>
        /// Старая левая точка линии.
        /// </summary>
        private readonly Point oldLeftPoint;

        /// <summary>
        /// Старая правая точка линии.
        /// </summary>
        private readonly Point oldRightPoint;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLinePointsAction"/>.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="oldLeftPoint">Старая левая точка линии.</param>
        /// <param name="oldRightPoint">Старая правая точка линии.</param>
        /// <param name="newLeftPoint">Новая левая точка линии.</param>
        /// <param name="newRightPoint">Новая правая точка линии.</param>
        public ChangeLinePointsAction(LineViewModel line, Point oldLeftPoint, Point oldRightPoint, string prevPoints, Point newLeftPoint, Point newRightPoint, string newPoints)
        {
            this.line = line;
            this.oldLeftPoint = oldLeftPoint;
            this.oldRightPoint = oldRightPoint;
            this.prevPoints = prevPoints;
            this.newLeftPoint = newLeftPoint;
            this.newRightPoint = newRightPoint;
            this.newPoints = newPoints;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangeLinePointsAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.line.SetLeftPoint(this.newLeftPoint);
            this.line.SetRightPoint(this.newRightPoint);

            this.line.Points = this.newPoints;
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeLinePointsAction));
            sb.Append(Environment.NewLine);
            sb.Append("Line: ");
            sb.Append(this.line.Id);
            sb.Append(Environment.NewLine);
            sb.Append("LeftPoint: ");
            sb.Append(this.oldLeftPoint);
            sb.Append(" -> ");
            sb.Append(this.newLeftPoint);
            sb.Append(Environment.NewLine);
            sb.Append("RightPoint: ");
            sb.Append(this.oldRightPoint);
            sb.Append(" -> ");
            sb.Append(this.newRightPoint);
            sb.Append(Environment.NewLine);
            sb.Append("Points: ");
            sb.Append(this.prevPoints);
            sb.Append(" -> ");
            sb.Append(this.newPoints);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.line.SetLeftPoint(this.oldLeftPoint);
            this.line.SetRightPoint(this.oldRightPoint);

            this.line.Points = this.prevPoints;
        }

        #endregion
    }
}