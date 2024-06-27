using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным отчетов.
    /// </summary>
    public interface IReportAccessService
    {
        #region Методы

        /// <summary>
        /// Выполняет расчет гидравлики.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        void CalculateHydraulics(Guid boilerId, SchemaModel schema);

        /// <summary>
        /// Возвращает данные отчета с информацией о количестве введенных объектов.
        /// </summary>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Данные отчета.</returns>
        DataSet GetAddedObjectInfo(DateTime fromDate, SchemaModel schema);

        /// <summary>
        /// Возвращает данные отчета о несопоставленных объектах.
        /// </summary>
        /// <param name="all">По всем объектам.</param>
        /// <returns>Данные отчета.</returns>
        DataSet GetDiffObjects(bool all);

        /// <summary>
        /// Возвращает данные отчета о гидравлике.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Данные отчета.</returns>
        DataSet GetHydraulics(Guid boilerId, SchemaModel schema);

        /// <summary>
        /// Возвращает данные отчета о проценте сопоставления с программами.
        /// </summary>
        /// <param name="all">По всем объектам.</param>
        /// <returns>Данные отчета.</returns>
        DataSet GetIntegrationStats(bool all);

        /// <summary>
        /// Возвращает данные отчета о жилищном фонде и договорных подключениях (по КТС).
        /// </summary>
        /// <param name="regionId">Идентификатор региона.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема населенного пункта.</param>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <returns>Данные отчета.</returns>
        DataSet GetKts(int regionId, int cityId, SchemaModel schema, Guid boilerId);

        /// <summary>
        /// Возвращает данные отчета о технических характеристиках.
        /// </summary>
        /// <param name="ids">Номера частей отчета.</param>
        /// <param name="regionId">Идентификатор региона.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема населенного пункта.</param>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Данные отчета.</returns>
        DataSet GetTechSpec(List<int> ids, int regionId, int cityId, SchemaModel schema, Guid boilerId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Возвращает шаблон отчета по его внутреннему идентификатору.
        /// </summary>
        /// <param name="innerId">Внутренний идентификатор отчета.</param>
        /// <returns>Шаблон отчета.</returns>
        string GetTemplate(int innerId);

        #endregion
    }
}