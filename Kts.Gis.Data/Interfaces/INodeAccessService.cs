using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов, представляемых узлами на карте.
    /// </summary>
    public interface INodeAccessService : IModifiableObjectAccessService<NodeModel>, IObjectAccessService<NodeModel>
    {
        #region Методы

        /// <summary>
        /// Добавляет узел в источник данных.
        /// </summary>
        /// <param name="node">Добавляемый узел.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор добавленного узла.</returns>
        Guid AddNode(NodeModel node, SchemaModel schema);

        /// <summary>
        /// Удаляет узел из источника данных.
        /// </summary>
        /// <param name="node">Удаляемый узел.</param>
        /// <param name="schema">Схема.</param>
        void DeleteNode(NodeModel node, SchemaModel schema);

        /// <summary>
        /// Возвращает идентификаторы узлов поворота заданной схемы.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Идентификаторы узлов поворота.</returns>
        List<Guid> GetBendNodes(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает идентификаторы неподключенных узлов заданной схемы.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Идентификаторы неподключенных узлов.</returns>
        List<Guid> GetFreeNodes(SchemaModel schema, int cityId);

        #endregion
    }
}