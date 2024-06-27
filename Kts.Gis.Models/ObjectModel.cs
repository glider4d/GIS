using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель объекта.
    /// </summary>
    public abstract partial class ObjectModel : IObjectModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ObjectModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="parentId">Идентификатор родителя объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект планируемым.</param>
        /// <param name="isActive">Значение, указывающее на то, что является ли объект активным.</param>
        public ObjectModel(Guid id, Guid? parentId, int cityId, ObjectType type, bool isPlanning, bool isActive)
        {
            this.Id = id;
            this.ParentId = parentId;
            this.CityId = cityId;
            this.Type = type;
            this.IsPlanning = isPlanning;
            this.IsActive = isActive;
        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает идентификатор объекта по умолчанию.
        /// </summary>
        public static Guid DefaultId
        {
            get
            {
                return Guid.Empty;
            }
        }

        #endregion
    }

    // Реализация IObjectModel.
    [Serializable]
    public abstract partial class ObjectModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором расположен объект.
        /// </summary>
        public int CityId
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает идентификатор объекта.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        public bool IsPlanning
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли объект в источнике данных.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                // Объект считается сохраненным, если его идентификатор не равен идентификатору объекта по умолчанию.
                return this.Id != DefaultId;
            }
        }

        /// <summary>
        /// Возвращает или задает идентификатор родителя объекта.
        /// </summary>
        public Guid? ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public ObjectType Type
        {
            get;
            set;
        }

        #endregion
    }
}