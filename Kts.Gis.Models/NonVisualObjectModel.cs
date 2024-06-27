using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель невизуального объекта.
    /// </summary>
    [Serializable]
    public sealed partial class NonVisualObjectModel : ObjectModel, IContainerObjectModel, INamedObjectModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NonVisualObjectModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="parentId">Идентификатор родителя объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект планируемым.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="isActive">Значение, указывающее на то, что является ли объект активным.</param>
        /// <param name="hasChildren">Значение, указывающее на то, что имеет ли объект дочерние объекты.</param>
        public NonVisualObjectModel(Guid id, Guid parentId, int cityId, ObjectType type, bool isPlanning, string name, bool isActive, bool hasChildren) : base(id, parentId, cityId, type, isPlanning, isActive)
        {
            this.HasChildren = hasChildren;

            NamedObjectModelHelper.Init(this, name);
        }

        #endregion
    }

    // Реализация IContainerObjectModel.
    public sealed partial class NonVisualObjectModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает список типов дочерних объектов.
        /// </summary>
        public List<ObjectType> ChildrenTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        public bool HasChildren
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация INamedObjectModel.
    public sealed partial class NonVisualObjectModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает название объекта.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion
    }
}