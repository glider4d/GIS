using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Linq;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие присоединения узла к фигуре.
    /// </summary>
    internal sealed partial class ConnectToFigureAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что нужно ли менять идентификатор котельной, к которой принадлежит новая фигура.
        /// </summary>
        private readonly bool changeBoilerId;

        /// <summary>
        /// Новый идентификатор котельной, к которой будет подключена новая фигура.
        /// </summary>
        private readonly Guid newBoilerId;

        /// <summary>
        /// Новая фигура, к которой будет присоединен узел.
        /// </summary>
        private readonly FigureViewModel newFigure;

        /// <summary>
        /// Узел.
        /// </summary>
        private readonly NodeViewModel node;

        /// <summary>
        /// Старый идентификатор котельной, к которой была подключена новая фигура.
        /// </summary>
        private readonly Guid oldBoilerId;

        /// <summary>
        /// Старая фигура, к которой был присоединен узел.
        /// </summary>
        private readonly FigureViewModel oldFigure;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConnectToFigureAction"/>.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="oldFigure">Старая фигура, к которой был присоединен узел.</param>
        /// <param name="newFigure">Новая фигура, к которой будет присоединен узел.</param>
        public ConnectToFigureAction(NodeViewModel node, FigureViewModel oldFigure, FigureViewModel newFigure) : this(node, oldFigure, newFigure, Guid.Empty, Guid.Empty)
        {
            this.changeBoilerId = false;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConnectToFigureAction"/>.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="oldFigure">Старая фигура, к которой был присоединен узел.</param>
        /// <param name="newFigure">Новая фигура, к которой будет присоединен узел.</param>
        /// <param name="oldBoilerId">Старый идентификатор котельной, к которой была подключена новая фигура.</param>
        /// <param name="newBoilerId">Новый идентификатор котельной, к которой будет подключена новая фигура.</param>
        public ConnectToFigureAction(NodeViewModel node, FigureViewModel oldFigure, FigureViewModel newFigure, Guid oldBoilerId, Guid newBoilerId)
        {
            this.node = node;
            this.oldFigure = oldFigure;
            this.newFigure = newFigure;
            this.oldBoilerId = oldBoilerId;
            this.newBoilerId = newBoilerId;

            this.changeBoilerId = true;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ConnectToFigureAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.node.ConnectedObject = this.newFigure;

            if (this.changeBoilerId)
            {
                var param = this.newFigure.Type.Parameters.FirstOrDefault(x => x.Alias == Alias.BoilerId);

                if (this.newBoilerId != Guid.Empty)
                    this.newFigure.ChangeChangedValue(param, this.newBoilerId);
                else
                    this.newFigure.ChangeChangedValue(param, null);
            }
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ConnectToFigureAction));
            sb.Append(Environment.NewLine);
            sb.Append("Node: ");
            sb.Append(this.node.Id);
            sb.Append(Environment.NewLine);
            sb.Append("OldFigure: ");
            sb.Append(this.oldFigure != null ? this.oldFigure.Id.ToString() : "");
            sb.Append(Environment.NewLine);
            sb.Append("NewFigure: ");
            sb.Append(this.newFigure != null ? this.newFigure.Id.ToString() : "");
            sb.Append(Environment.NewLine);
            sb.Append("OldBoilerId: ");
            sb.Append(this.oldBoilerId != Guid.Empty ? this.oldBoilerId.ToString() : "");
            sb.Append(Environment.NewLine);
            sb.Append("NewBoilerId: ");
            sb.Append(this.newBoilerId != Guid.Empty ? this.newBoilerId.ToString() : "");
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.node.ConnectedObject = this.oldFigure;

            if (this.changeBoilerId)
            {
                var param = this.newFigure.Type.Parameters.FirstOrDefault(x => x.Alias == Alias.BoilerId);

                if (this.oldBoilerId != Guid.Empty)
                    this.newFigure.ChangeChangedValue(param, this.oldBoilerId);
                else
                    this.newFigure.ChangeChangedValue(param, null);
            }
        }

        #endregion
    }
}