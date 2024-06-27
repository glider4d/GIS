using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным кастомных объектов.
    /// </summary>
    public interface ICustomObjectAccessService
    {
        #region Методы

        /// <summary>
        /// Удаляет заголовок "Утверждено"/"Согласовано" из источника данных.
        /// </summary>
        /// <param name="header">Удаляемый заголовок.</param>
        /// <param name="schema">Схема.</param>
        void DeleteApprovedHeader(ApprovedHeaderModel header, SchemaModel schema);

        /// <summary>
        /// Удаляет таблицу с данными о протяженностях труб, разбитых по диаметрам, из источника данных.
        /// </summary>
        /// <param name="table">Удаляемая таблица.</param>
        /// <param name="schema">Схема.</param>
        void DeleteLengthPerDiameterTable(LengthPerDiameterTableModel table, SchemaModel schema);

        /// <summary>
        /// Возвращает список заголовков "Утверждено"/"Согласовано".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список заголовков "Утверждено"/"Согласовано".</returns>
        List<ApprovedHeaderModel> GetApprovedHeaders(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список таблиц с данными о протяженностях труб, разбитых по диаметрам.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список таблиц с данными о протяженностях труб, разбитых по диаметрам.</returns>
        List<LengthPerDiameterTableModel> GetLengthPerDiameterTables(SchemaModel schema, int cityId);

        /// <summary>
        /// Помечает заголовок "Утверждено"/"Согласовано" на удаление из источника данных.
        /// </summary>
        /// <param name="header">Заголовок, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        void MarkDeleteApprovedHeader(ApprovedHeaderModel header, SchemaModel schema);

        /// <summary>
        /// Помечает таблицу с данными о протяженностях труб, разбитых по диаметрам, на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Таблица, которую нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        void MarkDeleteLengthPerDiameterTable(LengthPerDiameterTableModel table, SchemaModel schema);

        /// <summary>
        /// Обновляет данные нового заголовка "Утверждено"/"Согласовано" в источнике данных.
        /// </summary>
        /// <param name="header">Новый заголовок.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор нового заголовка.</returns>
        Guid UpdateNewApprovedHeader(ApprovedHeaderModel header, SchemaModel schema);

        /// <summary>
        /// Обновляет данные новой таблицы в источнике данных.
        /// </summary>
        /// <param name="table">Обновляемая таблица.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор новой таблицы.</returns>
        Guid UpdateNewObject(LengthPerDiameterTableModel table, SchemaModel schema);

        /// <summary>
        /// Обновляет данные заголовка "Утверждено"/"Согласовано" в источнике данных.
        /// </summary>
        /// <param name="header">Обновляемый заголовок.</param>
        /// <param name="schema">Схема.</param>
        void UpdateApprovedHeader(ApprovedHeaderModel header, SchemaModel schema);

        /// <summary>
        /// Обновляет данные таблицы в источнике данных.
        /// </summary>
        /// <param name="table">Обновляемая таблица.</param>
        /// <param name="schema">Схема.</param>
        void UpdateObject(LengthPerDiameterTableModel table, SchemaModel schema);

        #endregion
    }
}