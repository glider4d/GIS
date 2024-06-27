using System;

namespace Kts.Updater.Models
{
    /// <summary>
    /// Представляет модель объекта SQL.
    /// </summary>
    internal sealed class SqlObjectModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlObjectModel"/>.
        /// </summary>
        /// <param name="name">Название объекта SQL.</param>
        public SqlObjectModel(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlObjectModel"/>.
        /// </summary>
        /// <param name="name">Название объекта SQL.</param>
        /// <param name="command">Команда объекта SQL.</param>
        /// <param name="dropCommand">Команда сброса объекта SQL.</param>
        /// <param name="schema">Название схемы.</param>
        /// <param name="modified">Дата изменения объекта SQL.</param>
        public SqlObjectModel(string name, string command, string dropCommand, string schema, DateTime modified) : this(schema + "." + name, command, modified)
        {
            this.DropCommand = dropCommand;
            this.Schema = schema;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlObjectModel"/>.
        /// </summary>
        /// <param name="name">Название объекта SQL.</param>
        /// <param name="command">Команда объекта SQL.</param>
        /// <param name="modified">Дата изменения объекта SQL.</param>
        public SqlObjectModel(string name, string command, DateTime modified)
        {
            this.Name = name;
            this.Command = command;
            this.Modified = modified;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает команду объекта SQL.
        /// </summary>
        public string Command
        {
            get;
            set;
        } = "";

        /// <summary>
        /// Возвращает команду сброса объекта SQL.
        /// </summary>
        public string DropCommand
        {
            get;
        } = "";

        /// <summary>
        /// Возвращает дату изменения объекта SQL.
        /// </summary>
        public DateTime Modified
        {
            get;
        }

        /// <summary>
        /// Возвращает название объекта SQL.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает название схемы.
        /// </summary>
        public string Schema
        {
            get;
        } = "";

        #endregion
    }
}