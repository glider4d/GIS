using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к информации о котельной.
    /// </summary>
    public interface IBoilerInfoAccessService
    {
        #region Методы

        /// <summary>
        /// Возвращает длины труб заданной котельной по годам ввода.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по годам ввода.</returns>
        List<Tuple<int, double>> GetPipeDates(Guid boilerId, SchemaModel schema);

        /// <summary>
        /// Возвращает длины труб заданной котельной по годам ввода.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="pipeTypeId">Идентификатор типа труб.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по годам ввода.</returns>
        List<Tuple<int, double>> GetPipeDates(Guid boilerId, int pipeTypeId, SchemaModel schema);

        /// <summary>
        /// Возвращает длины труб заданной котельной по диаметрам.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по диаметрам.</returns>
        List<Tuple<int, double>> GetPipeLengths(Guid boilerId, SchemaModel schema);

        /// <summary>
        /// Возвращает длины труб заданной котельной по диаметрам.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="pipeTypeId">Идентификатор типа труб.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по диаметрам.</returns>
        List<Tuple<int, double>> GetPipeLengths(Guid boilerId, int pipeTypeId, SchemaModel schema);

        /// <summary>
        /// Возвращает типы труб, которые присоединены к котельной.
        /// </summary>
        /// <param name="id">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Типы труб.</returns>
        List<ObjectType> GetPipeTypes(Guid id, SchemaModel schema);

        #endregion
    }
}