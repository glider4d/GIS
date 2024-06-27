using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    [ServiceContract]
    public interface IObjectAccessService<T>
    {
        #region Методы

        /// <summary>
        /// Возвращает все объекты из источника данных, находящиеся в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        [OperationContract]
        List<T> GetAll(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает все объекты из источника данных, входящих в одну сеть с заданным объектом.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        [OperationContract]
        List<T> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema);

        #endregion
    }
}