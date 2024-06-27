using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель записи результата поиска.
    /// </summary>
    public sealed class SearchEntryModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchEntryModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="parentId">Идентификатор родителя объекта.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="regionName">Название региона, в котором расположен объект.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="cityName">Название населенного пункта, в котором расположен объект.</param>
        /// <param name="schemaId">Идентификатор схемы, в котором расположен объект.</param>
        /// <param name="paramValues">Значения параметров.</param>
        public SearchEntryModel(Guid id, Guid? parentId, string name, ObjectType type, string regionName, int cityId, string cityName, int schemaId, Dictionary<ParameterModel, string> paramValues)
        {
            this.Id = id;
            this.ParentId = parentId;
            this.Name = name;
            this.Type = type;
            this.RegionName = regionName;
            this.CityId = cityId;
            this.CityName = cityName;
            this.SchemaId = schemaId;
            this.ParamValues = paramValues;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором расположен объект.
        /// </summary>
        public int CityId
        {
            get;
        }

        /// <summary>
        /// Возвращает название региона.
        /// </summary>
        public string CityName
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор объекта.
        /// </summary>
        public Guid Id
        {
            get;
        }
        
        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает значения параметров.
        /// </summary>
        public Dictionary<ParameterModel, string> ParamValues
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор родителя объекта.
        /// </summary>
        public Guid? ParentId
        {
            get;
        }

        /// <summary>
        /// Возвращает название региона.
        /// </summary>
        public string RegionName
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор схемы.
        /// </summary>
        public int SchemaId
        {
            get;
        }

        /// <summary>
        /// Возвращает тип объекта.
        /// </summary>
        public ObjectType Type
        {
            get;
        }

        #endregion
    }
}