using Kts.Gis.Models;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным объектов, которые могут быть удалены.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    public interface IDeletableObjectAccessService<T>
    {
        #region Методы

        /// <summary>
        /// Удаляет объект из источника данных.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        /// <param name="schema">Схема.</param>
        void DeleteObject(T obj, SchemaModel schema);

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        void MarkDeleteObject(T obj, SchemaModel schema);

        #endregion
    }
}