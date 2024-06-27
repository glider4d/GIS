using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kts.Importer.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса данных.
    /// </summary>
    public interface IDataService
    {
        #region Свойства
    
        /// <summary>
        /// Возвращает коллекцию типов объектов.
        /// </summary>
        ReadOnlyCollection<ObjectType> ObjectTypes
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Возвращает все котельные, расположенные в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список, состоящий из тюплов, в котором первым элементом является идентификатор объекта, а вторым - его название.</returns>
        List<Tuple<Guid, string>> GetBoilers(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает все населенные пункты заданного региона из источника данных.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Список населенных пунктов.</returns>
        List<TerritorialEntityModel> GetCities(TerritorialEntityModel region);

        /// <summary>
        /// Возвращает схему по умолчанию заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Схема.</returns>
        SchemaModel GetDefaultSchema(int cityId);

        /// <summary>
        /// Возвращает все объекты-родители, расположенных в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список, состоящий из тюплов, в котором первым элементом является название объекта-родителя, а вторым - его идентификатор.</returns>
        List<Tuple<string, Guid>> GetParents(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает все регионы из источника данных.
        /// </summary>
        /// <returns>Список регионов.</returns>
        List<TerritorialEntityModel> GetRegions();

        /// <summary>
        /// Возвращает все улицы заданного населенного пункта из источника данных.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Список улиц.</returns>
        List<TerritorialEntityModel> GetStreets(TerritorialEntityModel city);

        /// <summary>
        /// Сохраняет значения параметров объекта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="parentId">Идентификатор объекта-родителя.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="schema">Схема.</param>
        void SaveObjectValues(int cityId, Guid? parentId, ParameterValueSetModel parameterValueSet, SchemaModel schema);

        /// <summary>
        /// Сохраняет значения параметров объекта.
        /// </summary>
        /// <param name="objectId">Идентификатор сохраняемого объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="parentId">Идентификатор объекта-родителя.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="schema">Схема.</param>
        void SaveObjectValues(Guid objectId, int cityId, Guid? parentId, ParameterValueSetModel parameterValueSet, SchemaModel schema);

        #endregion
    }
}