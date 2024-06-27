using System;
using System.Collections.Generic;

namespace Kts.Updater.Models
{
    /// <summary>
    /// Представляет модель пакета обновления.
    /// </summary>
    internal sealed class PackageModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PackageModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор пакета обновления.</param>
        /// <param name="server">Название обновляемого сервера.</param>
        /// <param name="database">Название обновляемой базы данных.</param>
        /// <param name="date">Дата создания пакета обновления.</param>
        /// <param name="sqlObjects">Объекты SQL.</param>
        public PackageModel(string id, string server, string database, DateTime date, List<SqlObjectModel> sqlObjects)
        {
            this.Id = id;
            this.Server = server;
            this.Database = database;
            this.Date = date;
            this.SqlObjects = sqlObjects;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает название обновляемой базы данных.
        /// </summary>
        public string Database
        {
            get;
        }

        /// <summary>
        /// Возвращает дату создания пакета обновления.
        /// </summary>
        public DateTime Date
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор пакета обновления.
        /// </summary>
        public string Id
        {
            get;
        }

        /// <summary>
        /// Возвращает название обновляемого сервера.
        /// </summary>
        public string Server
        {
            get;
        }

        /// <summary>
        /// Возвращает объекты SQL.
        /// </summary>
        public List<SqlObjectModel> SqlObjects
        {
            get;
        }

        #endregion
    }
}