using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным территориальных единиц.
    /// </summary>
    public interface ITerritorialEntityAccessService
    {
        #region Методы

        /// <summary>
        /// Возвращает все котельные заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список котельных.</returns>
        List<Tuple<Guid, string>> GetBoilers(TerritorialEntityModel city, SchemaModel schema);

        /// <summary>
        /// Возвращает все котельные заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список котельных.</returns>
        List<Tuple<Guid, string>> GetBoilers(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает все населенные пункты заданного региона из источника данных.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Список населенных пунктов.</returns>
        List<TerritorialEntityModel> GetCities(TerritorialEntityModel region);

        /// <summary>
        /// Возвращает регион и район, в котором расположен заданный населенный пункт.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Тюпл, содержащий регион и район.</returns>
        Tuple<TerritorialEntityModel, TerritorialEntityModel> GetCityData(TerritorialEntityModel city);

        /// <summary>
        /// Возвращает все регионы из источника данных.
        /// </summary>
        /// <returns>Список регионов.</returns>
        List<TerritorialEntityModel> GetRegions();

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный населенный пункт зафиксированным.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>true, если населенный пункт зафиксирован, иначе - false.</returns>
        bool IsCityFixed(TerritorialEntityModel city);

        #endregion
    }
}