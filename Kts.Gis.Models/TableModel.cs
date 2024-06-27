using System.Collections.Generic;
using System.Linq;
using System;


namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель таблицы.
    /// </summary>
    [Serializable]
    public sealed class TableModel
    {
        #region Закрытые поля

        /// <summary>
        /// Данные таблицы, где ключом является название фильтрующего поля, а значением - словарь, в котором ключом является значение фильтрующего поля, а значением - запись таблицы.
        /// </summary>
        ////[NonSerialized]
        private Dictionary<string, Dictionary<int?, List<TableEntryModel>>> data = new Dictionary<string, Dictionary<int?, List<TableEntryModel>>>();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TableModel"/>.
        /// </summary>
        /// <param name="name">Название таблицы.</param>
        public TableModel(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает название таблицы.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет в таблицу новую запись.
        /// </summary>
        /// <param name="filterField">Название фильтрующего поля.</param>
        /// <param name="filterValue">Значение фильтрующего поля.</param>
        /// <param name="entry">Запись таблицы.</param>
        public void AddEntry(string filterField, int? filterValue, TableEntryModel entry)
        {

            var field = filterField == null ? "" : filterField;
            var value = filterValue.HasValue ? filterValue : -1;

            if (!this.data.ContainsKey(field))
                this.data.Add(field, new Dictionary<int?, List<TableEntryModel>>());

            if (!this.data[field].ContainsKey(value))
                this.data[field].Add(value, new List<TableEntryModel>());

            this.data[field][value].Add(entry);
        }

        /// <summary>
        /// Очищает таблицу.
        /// </summary>
        public void Clear()
        {
            this.data.Clear();
        }

        /// <summary>
        /// Возвращает список записей таблицы.
        /// </summary>
        /// <param name="filterField">Название фильтрующего поля.</param>
        /// <returns>Список записей таблицы.</returns>
        public List<TableEntryModel> GetEntries(string filterField)
        {
            var result = new List<TableEntryModel>();

            var field = filterField == null ? "" : filterField;
            
            foreach (var list in data[field].Values)
                result.AddRange(list);
                
            return result;
        }

        /// <summary>
        /// Возвращает список записей таблицы.
        /// </summary>
        /// <param name="filterField">Название фильтрующего поля.</param>
        /// <param name="filterValue">Значение фильтрующего поля.</param>
        /// <returns>Список записей таблицы.</returns>
        public List<TableEntryModel> GetEntries(string filterField, int? filterValue)
        {
            var field = filterField == null ? "" : filterField;
            var value = filterValue.HasValue ? filterValue : -1;

            if (this.data.Count == 0 || !this.data[field].ContainsKey(value))
                return new List<TableEntryModel>();

            return this.data[field][value];
        }

        /// <summary>
        /// Возвращает запись таблицы.
        /// </summary>
        /// <param name="filterField">Название фильтрующего поля.</param>
        /// <param name="filterValue">Значение фильтрующего поля.</param>
        /// <returns>Запись таблицы.</returns>
        public TableEntryModel GetEntry(string filterField, int? filterValue)
        {
            return this.GetEntries(filterField, filterValue).FirstOrDefault();
        }

        #endregion
    }
}