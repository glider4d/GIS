using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель кастомного слоя.
    /// </summary>
    [Serializable]
    public sealed class CustomLayerModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomLayerModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор кастомного слоя.</param>
        /// <param name="schema">Схема кастомного слоя.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="name">Название кастомного слоя.</param>
        public CustomLayerModel(Guid id, SchemaModel schema, int cityId, string name)
        {
            this.Id = id;
            this.Schema = schema;
            this.CityId = cityId;
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор населенного пункта.
        /// </summary>
        public int CityId
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает идентификатор кастомного слоя.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает название кастомного слоя.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает схему кастомного слоя.
        /// </summary>
        public SchemaModel Schema
        {
            get;
        }

        #endregion
    }
}