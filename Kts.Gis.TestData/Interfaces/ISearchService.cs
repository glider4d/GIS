using Kts.Gis.Models;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса поиска объектов в источнике данных.
    /// </summary>
    public interface ISearchService
    {
        #region Методы

        /// <summary>
        /// Находит объекты в источнике данных по заданному значению параметра.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="regionId">Идентификатор региона, по которому ищутся объекты.</param>
        /// <param name="cityId">Идентификатор населенного пункта, по которому ищутся объекты.</param>
        /// <param name="schema">Схема, по которой ищутся объекты.</param>
        /// <param name="searchTerms">Условия поиска.</param>
        /// <returns>Результат поиска.</returns>
        List<SearchEntryModel> FindObjects(ObjectType type, int regionId, int cityId, SchemaModel schema, List<SearchTermModel> searchTerms);

        #endregion
    }
}