using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступ к данным топлива котельных.
    /// </summary>
    public interface IFuelAccessService
    {
        #region Методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли получить доступ к информации о топливе.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        bool CanAccessFuelInfo();

        /// <summary>
        /// Возвращает информацию о топливе заданной котельной.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fuelId">Идентификатор топлива.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Информация о топливе.</returns>
        FuelInfoModel GetFuelInfo(Guid boilerId, int fuelId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Возвращает виды топлива заданной котельной.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Список моделей топлива.</returns>
        List<FuelModel> GetFuelTypes(Guid boilerId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Возвращает информацию о складах топлива заданного населенного пункта.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fuelId">Идентификатор топлива.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Информация о складах топлива.</returns>
        List<FuelStorageModel> GetStoragesInfo(Guid boilerId, int fuelId, DateTime fromDate, DateTime toDate);

        #endregion
    }
}