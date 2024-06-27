using Kts.Gis.Models;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным карты.
    /// </summary>
    public interface IMapAccessService
    {
        #region Методы

        /// <summary>
        /// Возвращает масштаб линий заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Масштаб линий.</returns>
        double GetScale(TerritorialEntityModel city);

        /// <summary>
        /// Возвращает настройки вида карты.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Настройки вида карты.</returns>
        MapSettingsModel GetSettings(int cityId);

        /// <summary>
        /// Возвращает подложку заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Подложка.</returns>
        SubstrateModel GetSubstrate(TerritorialEntityModel city);

        /// <summary>
        /// Обновляет настройки надписей.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="captionCfg">Настройки надписей.</param>
        void UpdateCaptions(int cityId, Dictionary<ObjectType, List<ParameterModel>> captionCfg);

        /// <summary>
        /// Обновляет масштаб линий заданного населенного пункта.
        /// </summary>
        /// <param name="scale">Масштаб линий.</param>
        /// <param name="city">Населенный пункт.</param>
        void UpdateScale(double scale, TerritorialEntityModel city);

        /// <summary>
        /// Обновляет настройки заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="settings">Настройки.</param>
        void UpdateSettings(int cityId, MapSettingsModel settings);

        bool testConnection(string operation="", bool silence = false);

        #endregion
    }
}