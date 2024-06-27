using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие удаления узла.
    /// </summary>
    internal sealed partial class DeleteNodeAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Линия-донор.
        /// </summary>
        private readonly LineViewModel donor;

        /// <summary>
        /// Сторона соединения линии-донора с удаляемым узлом.
        /// </summary>
        private readonly NodeConnectionSide donorSide;

        /// <summary>
        /// Линия-получатель.
        /// </summary>
        private readonly LineViewModel reciever;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Новый узел линии-получателя.
        /// </summary>
        private readonly NodeViewModel newRecieverNode;

        /// <summary>
        /// Новые точки изгиба линии-получателя.
        /// </summary>
        private readonly string newRecieverPoints;

        /// <summary>
        /// Удаляемый узел.
        /// </summary>
        private readonly NodeViewModel node;

        /// <summary>
        /// Старая длина линии-получателя.
        /// </summary>
        private readonly double oldLength;

        /// <summary>
        /// Старые точки изгиба линии-получателя.
        /// </summary>
        private readonly string oldRecieverPoints;

        /// <summary>
        /// Измененные парамеры линии-получателя.
        /// </summary>
        private readonly Dictionary<ParameterModel, Tuple<object, object>> parameters;

        /// <summary>
        /// Сторона соединения линии-получателя с удаляемым узлом.
        /// </summary>
        private readonly NodeConnectionSide recieverSide;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DeleteNodeAction"/>.
        /// </summary>
        /// <param name="reciever">Линия-получатель.</param>
        /// <param name="donor">Линия-донор.</param>
        /// <param name="node">Удаляемый узел.</param>
        /// <param name="oldLength">Старая длина линии-получателя.</param>
        /// <param name="oldRecieverPoints">Старые точки изгиба линии-получателя.</param>
        /// <param name="newRecieverPoints">Новые точки изгиба линии-получателя.</param>
        /// <param name="parameters">Измененные параметры линии-получателя.</param>
        /// <param name="recieverSide">Сторона соединения линии-получателя с удаляемым узлом.</param>
        /// <param name="newRecieverNode">Новый узел линии-получателя.</param>
        /// <param name="donorSide">Сторона соединения линии-донора с удаляемым узлом.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public DeleteNodeAction(LineViewModel reciever, LineViewModel donor, NodeViewModel node, double oldLength, string oldRecieverPoints, string newRecieverPoints, Dictionary<ParameterModel, Tuple<object, object>> parameters, NodeConnectionSide recieverSide, NodeViewModel newRecieverNode, NodeConnectionSide donorSide, ILayerHolder layerHolder)
        {
            this.reciever = reciever;
            this.donor = donor;
            this.node = node;
            this.oldLength = oldLength;
            this.oldRecieverPoints = oldRecieverPoints;
            this.newRecieverPoints = newRecieverPoints;
            this.parameters = parameters;
            this.recieverSide = recieverSide;
            this.newRecieverNode = newRecieverNode;
            this.donorSide = donorSide;
            this.layerHolder = layerHolder;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class DeleteNodeAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            // Задаем новые значения измененных параметров линии-получателя.
            foreach (var entry in this.parameters)
                this.reciever.ChangeChangedValue(entry.Key, entry.Value.Item2);

            // Суммируем длины линии-получателя и линии-донора.
            this.reciever.UpdateLength(this.reciever.Length + this.donor.Length);

            // Открепляем линию-получателя от удаляемого узла.
            this.node.RemoveConnection(this.reciever);

            // Прикрепляем ее к новому узлу.
            this.newRecieverNode.AddConnection(new NodeConnection(this.reciever, this.recieverSide));
            // И открепляем от него линию-донор.
            this.newRecieverNode.RemoveConnection(this.donor);

            // Заменяем точки изгиба линии-получателя.
            this.reciever.Points = this.newRecieverPoints;

            // Убираем с карты линию-донор и удаляемый узел.
            this.layerHolder.RemoveObject(this.donor);
            this.layerHolder.RemoveObject(this.node);

            // Помечаем линию к удалению.
            this.layerHolder.MarkToDelete(this.donor);
            // А узел - к обновлению.
            this.layerHolder.MarkToUpdate(this.node);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(DeleteNodeAction));
            sb.Append(Environment.NewLine);
            sb.Append("Reciever: ");
            sb.Append(this.reciever.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Donor: ");
            sb.Append(this.donor.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Node: ");
            sb.Append(this.node.Id);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            // Возвращаем старые значения измененных параметров линии-получателя.
            foreach (var entry in this.parameters)
                this.reciever.ChangeChangedValue(entry.Key, entry.Value.Item1);

            // Открепляем линию-получателя от уже бывшего узла.
            this.newRecieverNode.RemoveConnection(this.reciever);

            // Прикрепляем ее обратно к удаленному узлу.
            this.node.AddConnection(new NodeConnection(this.reciever, this.recieverSide));

            // Прикрепляем к новому узлу линию-донор.
            this.newRecieverNode.AddConnection(new NodeConnection(this.donor, this.donorSide));

            // Заменяем точки изгиба линии-получателя.
            this.reciever.Points = this.oldRecieverPoints;

            // Возвращаем старую длину линии-получателя.
            this.reciever.UpdateLength(this.oldLength);

            // Добавляем на карту линию-донор и удаленный узел.
            this.layerHolder.AddObject(this.donor);
            this.layerHolder.AddObject(this.node);

            this.node.NotifyViewConnectionsChanged();

            // Убираем пометку с линии-донора, чтобы она не удалилась при сохранении.
            this.layerHolder.UnmarkToDelete(this.donor);
            // Убираем пометку с удаленного узла, чтобы он не обновлялся раньше остальных объектов.
            this.layerHolder.UnmarkToUpdate(this.node);
        }

        #endregion
    }
}