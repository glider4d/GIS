using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет интерфейс модели объекта, способного иметь дочерние объекты.
    /// </summary>
    public interface IContainerObjectModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает список типов дочерних объектов.
        /// </summary>
        List<ObjectType> ChildrenTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        bool HasChildren
        {
            get;
            set;
        }

        #endregion
    }
}