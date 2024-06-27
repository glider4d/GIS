using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным параметров объектов.
    /// </summary>
    public interface IParameterAccessService
    {
        #region Методы

        /// <summary>
        /// Возвращает набор значений общих параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        ParameterValueSetModel GetGroupCommonParamValues(List<Guid> objectIds, ObjectType type, SchemaModel schema);

        /// <summary>
        /// Асинхронно возвращает набор значений общих параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        Task<ParameterValueSetModel> GetGroupCommonParamValuesAsync(List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает набор значений параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        ParameterValueSetModel GetGroupParamValues(List<Guid> objectIds, ObjectType type, SchemaModel schema);

        /// <summary>
        /// Асинхронно возвращает набор значений параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        Task<ParameterValueSetModel> GetGroupParamValuesAsync(List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает набор значений параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="param">Параметр.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        Dictionary<Guid, object> GetGroupParamValues(List<Guid> objectIds, ObjectType type, ParameterModel param, SchemaModel schema);

        /// <summary>
        /// Возвращает недостающий набор значений параметров объекта-получателя от объекта-донора.
        /// </summary>
        /// <param name="recipient">Объект-получатель.</param>
        /// <param name="donor">Объект-донор.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        ParameterValueSetModel GetMergedParamValues(IObjectModel recipient, IObjectModel donor, ObjectType type, SchemaModel schema);

        /// <summary>
        /// Возвращает набор значений вычисляемых параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        ParameterValueSetModel GetObjectCalcParamValues(IObjectModel obj, SchemaModel schema);

        /// <summary>
        /// Асинхронно возвращает набор значений вычисляемых параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        Task<ParameterValueSetModel> GetObjectCalcParamValuesAsync(IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает набор значений параметров заданного объекта из заданного набора данных.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        /// <param name="obj">Объект.</param>
        /// <returns>Набор значений параметров.</returns>
        ParameterValueSetModel GetObjectParamValues(DataSet dataSet, IObjectModel obj);

        /// <summary>
        /// Возвращает набор значений параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        ParameterValueSetModel GetObjectParamValues(IObjectModel obj, SchemaModel schema);

        /// <summary>
        /// Асинхронно возвращает набор значений параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        Task<ParameterValueSetModel> GetObjectParamValuesAsync(IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken);

        Task<ParameterValueSetModel> GetObjectParamValuesAsync(IObjectModel obj, SchemaModel schema);

        /// <summary>
        /// Возвращает идентификаторы объектов с незаполненными обязательными полями заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов объектов.</returns>
        List<Guid> GetObjectsWithErrors(int cityId, SchemaModel schema);

        /// <summary>
        /// Возвращает историю изменений значения параметра заданного объекта.
        /// </summary>
        /// <param name="parameterId">Идентификатор параметра.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>История изменений значения параметра.</returns>
        List<ParameterHistoryEntryModel> GetParameterHistory(int parameterId, DateTime fromDate, DateTime toDate, Guid objectId, SchemaModel schema);

        /// <summary>
        /// Возвращает значение просматриваемого параметра заданного объекта.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Значение параметра.</returns>
        object GetVieweryValue(ParameterModel param, Guid objectId, SchemaModel schema);

        /// <summary>
        /// Обновляет значения параметров нового (несохраненного в источнике данных) объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="paramValues">Значения параметров.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор нового (несохраненного в источнике данных) объекта.</returns>
        Guid UpdateNewObjectParamValues(IObjectModel obj, ParameterValueSetModel paramValues, SchemaModel schema);

        Guid UpdateNewObjectParamValuesFromLocal(int indexForSerialized);

        /// <summary>
        /// Обновляет значения параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="paramValues">Значения параметров.</param>
        /// <param name="schema">Схема.</param>
        void UpdateObjectParamValues(IObjectModel obj, ParameterValueSetModel paramValues, SchemaModel schema);

        void UpdateObjectParamValues(Guid guid, int index);

        /// <summary>
        /// Обновляет заданную таблицу.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="schema">Схема.</param>
        TableModel UpdateTable(TableModel table, int cityId, ObjectType type, SchemaModel schema);

        #endregion
    }
}