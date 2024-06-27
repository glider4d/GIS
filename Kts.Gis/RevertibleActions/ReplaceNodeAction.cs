using Kts.Gis.ViewModels;
using Kts.History;
using System.Collections.Generic;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие замещения узла.
    /// </summary>
    internal sealed partial class ReplaceNodeAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Замещаемое подключение.
        /// </summary>
        private readonly NodeConnection connection;

        /// <summary>
        /// Замещаемые подключения.
        /// </summary>
        private readonly List<NodeConnection> connections;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Замещающий узел.
        /// </summary>
        private readonly NodeViewModel newNode;

        /// <summary>
        /// Замещаемый узел.
        /// </summary>
        private readonly NodeViewModel node;

        /// <summary>
        /// Замещаемые узлы.
        /// </summary>
        private readonly List<NodeViewModel> nodes;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ReplaceNodeAction"/>.
        /// </summary>
        /// <param name="node">Замещаемый узел.</param>
        /// <param name="newNode">Замещающий узел.</param>
        /// <param name="connection">Замещаемое подключение.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public ReplaceNodeAction(NodeViewModel node, NodeViewModel newNode, NodeConnection connection, ILayerHolder layerHolder)
        {
            this.node = node;
            this.newNode = newNode;
            this.layerHolder = layerHolder;
            this.connection = connection;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ReplaceNodeAction"/>.
        /// </summary>
        /// <param name="nodes">Замещаемые узлы.</param>
        /// <param name="newNode">Замещающий узел.</param>
        /// <param name="connections">Замещаемые подключения.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public ReplaceNodeAction(List<NodeViewModel> nodes, NodeViewModel newNode, List<NodeConnection> connections, ILayerHolder layerHolder)
        {
            this.nodes = nodes;
            this.newNode = newNode;
            this.layerHolder = layerHolder;
            this.connections = connections;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        /// <param name="node">Замещаемый узел.</param>
        /// <param name="newNode">Замещающий узел.</param>
        /// <param name="connection">Замещаемое подключение.</param>
        private void Do(NodeViewModel node, NodeViewModel newNode, NodeConnection connection)
        {
            this.layerHolder.RemoveObject(node);

            node.RemoveConnection(connection.Line);

            newNode.AddConnection(connection);
            
            // Если узел уже существует в источнике данных, то помечаем его на обновление.
            if (node.IsSaved)
                this.layerHolder.MarkToUpdate(node);
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        /// <param name="node">Замещаемый узел.</param>
        /// <param name="newNode">Замещающий узел.</param>
        /// <param name="connection">Замещаемое подключение.</param>
        private void Revert(NodeViewModel node, NodeViewModel newNode, NodeConnection connection)
        {
            this.layerHolder.AddObject(node);

            newNode.RemoveConnection(connection.Line);

            node.AddConnection(connection);

            // Если узел уже существует в источнике данных, то убираем с него метку обновления.
            if (node.IsSaved)
                this.layerHolder.UnmarkToUpdate(node);
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ReplaceNodeAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            if (this.node == null)
                for (int i = 0; i < this.nodes.Count; i++)
                    this.Revert(this.nodes[i], this.newNode, this.connections[i]);
            else
                this.Do(this.node, this.newNode, this.connection);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ReplaceNodeAction));

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            if (this.node == null)
                for (int i = 0; i < this.nodes.Count; i++)
                    this.Do(this.nodes[i], this.newNode, this.connections[i]);
            else
                this.Revert(this.node, this.newNode, this.connection);
        }

        #endregion
    }
}