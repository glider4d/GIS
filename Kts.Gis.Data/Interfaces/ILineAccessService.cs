using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов, представляемых линиями на карте.
    /// </summary>
    public interface ILineAccessService : IDeletableObjectAccessService<LineModel>, IModifiableObjectAccessService<LineModel>, IObjectAccessService<LineModel>
    {
        #region Методы

        /// <summary>
        /// Возвращает все объекты из источника данных, входящих в одну сеть с заданным объектом, с минимальным набором данных.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список тюплов, в которых содержится информация об идентификаторах объектов, групп, их типах и планируемости.</returns>
        List<Tuple<Guid, Guid, ObjectType, bool>> GetAllFast(int cityId, Guid objectId, SchemaModel schema);

        /// <summary>
        /// Возвращает список идентификаторов линий, принадлежащих слою гидравлики.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов линий.</returns>
        List<Guid> GetHydraulicsLines(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает список идентификаторов линий, принадлежащих слою гидравлики.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов линий.</returns>
        List<Guid> GetHydraulicsErrorLines(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает список идентификаторов линий заданного года.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="year">Год.</param>
        /// <returns>Список идентификаторов линий прошедшего года.</returns>
        List<Guid> GetLinesByYear(int cityId, SchemaModel schema, int year);

        /// <summary>
        /// Возвращает список идентификаторов линий, учавствующих в ремонтной программе.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов линий.</returns>
        List<Guid> GetRP(int cityId, SchemaModel schema);

        #endregion
    }
}