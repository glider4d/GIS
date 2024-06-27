using Kts.Gis.Models;
using System.Collections.Generic;
using System.Data;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным дочерних объектов.
    /// </summary>
    /// <typeparam name="T">Тип дочернего объекта.</typeparam>
    public interface IChildAccessService<T> : IDeletableObjectAccessService<T>
    {
        #region Методы

        /// <summary>
        /// Возвращает все дочерние объекты из заданного набора данных, принадлежащие заданному родителю.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        /// <param name="obj">Объект-родитель.</param>
        /// <returns>Список дочерних объектов.</returns>
        List<T> GetAll(DataSet dataSet, IObjectModel obj);

        /// <summary>
        /// Возвращает все дочерние объекты из источника данных, принадлежащие заданному родителю.
        /// </summary>
        /// <param name="obj">Объект-родитель.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список дочерних объектов.</returns>
        List<T> GetAll(IObjectModel obj, SchemaModel schema);

        /// <summary>
        /// Возвращает таблицу данных всех дочерних объектов из источника данных, принадлежащих заданному населенному пункту, в необработанном виде.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Таблица данных.</returns>
        DataTable GetAllRaw(SchemaModel schema, int cityId);

        #endregion
    }
}