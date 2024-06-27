using Kts.Gis.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным глобальной карты.
    /// </summary>
    public interface IGlobalMapAccessService
    {
        #region Методы

        /// <summary>
        /// Асинхронно возвращает информацию о заданном визуальном регионе.
        /// </summary>
        /// <param name="region">Визуальный регион.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Информация о визуальном регионе.</returns>
        Task<VisualRegionInfoModel> GetVisualRegionInfoAsync(VisualRegionModel region, CancellationToken token);
        Task<VisualRegionInfoModel> GetVisualRegionInfoAsync(VisualRegionModel region);

        /// <summary>
        /// Возвращает визуальные регионы.
        /// </summary>
        /// <returns>Визуальные регионы.</returns>
        List<VisualRegionModel> GetVisualRegions();

        #endregion
    }
}