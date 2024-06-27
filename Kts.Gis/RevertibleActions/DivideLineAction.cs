using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие объединения линий.
    /// </summary>
    internal sealed partial class DivideLineAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что нужно ли скрыть узел в случае отмены объединения линии.
        /// </summary>
        private readonly bool hideNode;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Точки изгиба левого куска линии.
        /// </summary>
        private readonly string leftPoints;

        /// <summary>
        /// Линия, которая делится.
        /// </summary>
        private readonly LineViewModel line;

        /// <summary>
        /// Кусок от деления линии.
        /// </summary>
        private readonly LineViewModel linePart;

        /// <summary>
        /// Узел, который связывает линию, которая делится с ее куском от деления.
        /// </summary>
        private readonly NodeViewModel node;

        /// <summary>
        /// Точки изгиба линии, которая делится.
        /// </summary>
        private readonly string points;

        /// <summary>
        /// Точки изгиба правого куска линии.
        /// </summary>
        private readonly string rightPoints;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DivideLineAction"/>.
        /// </summary>
        /// <param name="line">Линия, которая делится.</param>
        /// <param name="points">Точки изгиба линии, которая делится.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="linePart">Кусок от деления линии.</param>
        /// <param name="node">Узел, который связывает линию, которая делится с ее куском от деления.</param>
        /// <param name="leftPoints">Точки изгиба левого куска линии.</param>
        /// <param name="rightPoints">Точки изгиба правого куска линии.</param>
        /// <param name="hideNode">Значение, указывающее на то, что нужно ли скрыть узел в случае отмены объединения линии.</param>
        public DivideLineAction(LineViewModel line, string points, ILayerHolder layerHolder, LineViewModel linePart, NodeViewModel node, string leftPoints, string rightPoints, bool hideNode = true)
        {
            this.line = line;
            this.points = points;
            this.layerHolder = layerHolder;
            this.linePart = linePart;
            this.node = node;
            this.leftPoints = leftPoints;
            this.rightPoints = rightPoints;
            this.hideNode = hideNode;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class DivideLineAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            // Высчитываем протяженность линии на пиксель.
            var perPixel = this.line.Length / this.line.GetFullLength();

            // Добавляем отделенную линию.
            this.layerHolder.AddObject(this.linePart);

            // Прикрепляем к правому узлу оригинальной линии отделенную линию и открепляем оригинальную.
            this.line.RightNode.AddConnection(new NodeConnection(this.linePart, NodeConnectionSide.Right));
            this.line.RightNode.RemoveConnection(this.line);

            this.line.RightPoint = this.linePart.LeftPoint;

            this.line.Points = this.leftPoints;
            this.linePart.Points = this.rightPoints;

            // Присоединяем к разделяющему узлу оригинальную и новую линии.
            this.node.AddConnection(new NodeConnection(this.linePart, NodeConnectionSide.Left));
            this.node.AddConnection(new NodeConnection(this.line, NodeConnectionSide.Right));

            if (this.hideNode)
            {
                // Размещаем узел на карте.
                this.layerHolder.AddObject(this.node);

                this.node.NotifyViewConnectionsChanged();
            }

            // Высчитываем новые длины линий.
            this.line.UpdateLength(this.line.GetFullLength() * perPixel);
            this.linePart.UpdateLength(this.linePart.GetFullLength() * perPixel);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(DivideLineAction));
            sb.Append(Environment.NewLine);
            sb.Append("Line: ");
            sb.Append(this.line.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Node: ");
            sb.Append(this.node.Id);
            sb.Append(Environment.NewLine);
            sb.Append("HideNode: ");
            sb.Append(this.hideNode);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            // Отсоединяем линии от разделяющего узла.
            this.node.RemoveConnection(this.line);
            this.node.RemoveConnection(this.linePart);

            this.line.RightPoint = this.linePart.RightPoint;

            this.line.Points = points;

            // Прикрепляем правый узел обратно к оригинальной линии и открепляем от новой.
            this.linePart.RightNode.AddConnection(new NodeConnection(this.line, NodeConnectionSide.Right));
            this.linePart.RightNode.RemoveConnection(this.linePart);

            // Удаляем новую линию.
            this.layerHolder.RemoveObject(this.linePart);

            if (this.hideNode)
                // Убираем узел с карты.
                this.layerHolder.RemoveObject(this.node);

            // Обновляем длину линии.
            this.line.UpdateLength(this.line.Length + this.linePart.Length);
        }

        #endregion
    }
}