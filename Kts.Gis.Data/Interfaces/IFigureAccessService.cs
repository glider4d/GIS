using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов, представляемых фигурами на карте.
    /// </summary>
    public interface IFigureAccessService : IDeletableObjectAccessService<FigureModel>, IModifiableObjectAccessService<FigureModel>, IObjectAccessService<FigureModel>
    {
        #region Методы

        /// <summary>
        /// Возвращает все объекты из источника данных, входящих в одну сеть с заданным объектом, с минимальным набором данных.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список тюплов, в которых содержится информация об идентификаторах объектов, их типах и планируемости.</returns>
        List<Tuple<Guid, ObjectType, bool>> GetAllFast(int cityId, Guid objectId, SchemaModel schema);

        /// <summary>
        /// Возвращает идентификатор котельной, к которой подключен объект. Если объект сам является котельной, то возвращается его идентификатор.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор котельной, к которой подключен объект. Если объект сам является котельной, то возвращается его идентификатор.</returns>
        Guid GetBoilerId(ObjectModel obj, SchemaModel schema);

        /// <summary>
        /// Возвращает список объектов программы "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<string, string>> GetJurObjects(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список объектов программы "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<string, string>> GetKvpObjects(SchemaModel schema, int cityId);


        List<string> getTrashList(int cityID, string trashStorageID);
        /// <summary>
        /// Возвращает список объектов с интеграцией с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<Guid, Guid, string, string>> GetObjectsWithJurIntegration(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список объектов с интеграцией с программой "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<Guid, Guid, string, string>> GetObjectsWithKvpIntegration(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список объектов без интеграции с программой "Учет потребления топлива".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutFuelIntegration(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список объектов без интеграции с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutJurIntegration(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список объектов без интеграции с программой "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutKvpIntegration(SchemaModel schema, int cityId);

        /// <summary>
        /// Возвращает список идентификаторов фигур, представляющих несопоставленные объекты.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся фигуры.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов фигур.</returns>
        List<Guid> GetUO(int cityId, SchemaModel schema);

        List<Guid> GetIjs(int cityId, SchemaModel schema, int bitFlag);

        /// <summary>
        /// Сбрасывает идентификатор из программы "Расчеты с юридическими лицами" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        void ResetJurId(Guid gisId, SchemaModel schema);

        /// <summary>
        /// Сбрасывает идентификатор из программы "Квартплата" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        void ResetKvpId(Guid gisId, SchemaModel schema);

        /// <summary>
        /// Задает идентификатор из программы "Расчеты с юридическими лицами" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="jurId">Идентификатор из программы "Расчеты с юридическими лицами".</param>
        /// <param name="schema">Схема.</param>
        void SetJurId(Guid gisId, string jurId, SchemaModel schema);

        /// <summary>
        /// Задает идентификатор из программы "Квартплата" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="kvpId">Идентификатор из программы "Квартплата".</param>
        /// <param name="schema">Схема.</param>
        void SetKvpId(Guid gisId, string kvpId, SchemaModel schema);

        #endregion
    }
}