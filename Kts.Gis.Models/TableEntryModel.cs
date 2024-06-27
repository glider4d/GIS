using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель записи таблицы.
    /// </summary>
    [Serializable]
    public sealed class TableEntryModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TableEntryModel"/>.
        /// </summary>
        /// <param name="key">Ключ записи таблицы.</param>
        /// <param name="value">Значение записи таблицы.</param>
        public TableEntryModel(object key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает ключ записи таблицы.
        /// </summary>
        public object Key
        {
            get;
        }

        /// <summary>
        /// Возвращает значение записи таблицы.
        /// </summary>
        public string Value
        {
            get;
        }

        #endregion
    }
}