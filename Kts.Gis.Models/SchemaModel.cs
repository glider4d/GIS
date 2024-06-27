using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель схемы населенного пункта.
    /// </summary>
    [Serializable]
    public sealed class SchemaModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SchemaModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор схемы.</param>
        /// <param name="name">Название схемы.</param>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <param name="isIS">Значение, указывающее на то, что является ли схема инвестиционной схемой.</param>
        public SchemaModel(int id, string name, bool isActual, bool isIS)
        {
            this.Id = id;
            this.Name = name;
            this.IsActual = isActual;
            this.IsIS = isIS;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор схемы.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли схема актуальной.
        /// </summary>
        public bool IsActual
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли схема инвестиционной схемой.
        /// </summary>
        public bool IsIS
        {
            get;
        }

        /// <summary>
        /// Возвращает название схемы.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}