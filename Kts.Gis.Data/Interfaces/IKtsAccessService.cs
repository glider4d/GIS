using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов базовых программ КТС.
    /// </summary>
    public interface IKtsAccessService
    {
        #region Методы

        /// <summary>
        /// Возвращает список скрытых объектов программы "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<long, string>> GetJurHidden(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список отображаемых объектов программы "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<long, string>> GetJurVisible(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список скрытых объектов программы "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<long, string>> GetKvpHidden(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список отображаемых объектов программы "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<long, string>> GetKvpVisible(SchemaModel schema, int cityId);

        /// <summary>
        /// Скрывает объект по его идентификатору, населенному пункту и идентификатору его программы.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="appId">Идентификатор программы.</param>
        void HideObj(long id, int cityId, int appId);

        /// <summary>
        /// Отображает объект по его идентификатору, населенному пункту и идентификатору его программы.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="appId">Идентификатор программы.</param>
        void ShowObj(long id, int cityId, int appId);

        #endregion
    }
}