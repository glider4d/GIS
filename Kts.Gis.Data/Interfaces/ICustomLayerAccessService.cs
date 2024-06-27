using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным кастомных слоев.
    /// </summary>
    public interface ICustomLayerAccessService
    {
        #region Методы

        /// <summary>
        /// Добавляет кастомный слой.
        /// </summary>
        /// <param name="layer">Слой.</param>
        /// <returns>Идентификатор добавленного кастомного слоя.</returns>
        Guid AddLayer(CustomLayerModel layer);

        /// <summary>
        /// Выполняет удаление кастомного слоя.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="id">Идентификатор удаляемого слоя.</param>
        void DeleteLayer(SchemaModel schema, int cityId, Guid id);

        /// <summary>
        /// Возвращает кастомные слои заданной схемы.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Кастомные слои.</returns>
        List<CustomLayerModel> GetCustomLayers(SchemaModel schema, int cityId);

        /// <summary>
        /// Обновляет кастомный слой.
        /// </summary>
        /// <param name="layer">Слой.</param>
        void UpdateLayer(CustomLayerModel layer);

        #endregion
    }
}