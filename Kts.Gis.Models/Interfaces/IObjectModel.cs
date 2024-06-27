using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет интерфейс модели объекта.
    /// </summary>
    public interface IObjectModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором расположен объект.
        /// </summary>
        int CityId
        {
            get;
        }
         

        /// <summary>
        /// Возвращает или задает идентификатор объекта.
        /// </summary>
        Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        bool IsPlanning
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли объект в источнике данных.
        /// </summary>
        bool IsSaved
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает идентификатор родителя объекта.
        /// </summary>
        Guid? ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        ObjectType Type
        {
            get;
            set;
        }



        #endregion
    }
}