using Kts.Gis.Models;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов, чьи данные могут быть изменены.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    public interface IModifiableObjectAccessService<T>
    {
        #region Методы

        /// <summary>
        /// Обновляет данные объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        void UpdateObject(T obj, SchemaModel schema);
        void UpdateObjectFromLocal(System.Guid id, int index);

        #endregion
    }
}