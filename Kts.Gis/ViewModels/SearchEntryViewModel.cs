using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления записи результата поиска.
    /// </summary>
    internal sealed class SearchEntryViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Текст пустой части названия.
        /// </summary>
        private const string emptyNamePart = "-";

        /// <summary>
        /// Разделитель названия объекта.
        /// </summary>
        private const string nameDivider = "; ";

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Запись результата поиска.
        /// </summary>
        private readonly SearchEntryModel searchEntry;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchEntryViewModel"/>.
        /// </summary>
        /// <param name="searchEntry">Запись результата поиска.</param>
        public SearchEntryViewModel(SearchEntryModel searchEntry)
        {
            this.searchEntry = searchEntry;

            this.AssembleName();

            this.ParamValues.AddRange(this.searchEntry.ParamValues.Values.ToList());
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор населенного пункта.
        /// </summary>
        public int CityId
        {
            get
            {
                return this.searchEntry.CityId;
            }
        }

        /// <summary>
        /// Возвращает название населенного пункта.
        /// </summary>
        public string CityName
        {
            get
            {
                return this.searchEntry.CityName;
            }
        }

        /// <summary>
        /// Возвращает идентификатор объекта.
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.searchEntry.Id;
            }
        }
        
        /// <summary>
        /// Возвращает или задает название объекта.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает значения параметров.
        /// </summary>
        public List<string> ParamValues
        {
            get;
        } = new List<string>();

        /// <summary>
        /// Возвращает идентификатор родителя объекта.
        /// </summary>
        public Guid? ParentId
        {
            get
            {
                return this.searchEntry.ParentId;
            }
        }

        /// <summary>
        /// Возвращает название региона.
        /// </summary>
        public string RegionName
        {
            get
            {
                return this.searchEntry.RegionName;
            }
        }

        /// <summary>
        /// Возвращает идентификатор схемы.
        /// </summary>
        public int SchemaId
        {
            get
            {
                return this.searchEntry.SchemaId;
            }
        }

        /// <summary>
        /// Возвращает тип объекта.
        /// </summary>
        public ObjectType Type
        {
            get
            {
                return this.searchEntry.Type;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Собирает название объекта.
        /// </summary>
        private void AssembleName()
        {
            if (!string.IsNullOrEmpty(searchEntry.Name))
            {
                // Получаем параметры, участвующие в составлении названия объекта.
                var parameters = this.Type.Parameters;

                var div = new string[1]
                {
                    "@@"
                };

                var parts = searchEntry.Name.Split(div, StringSplitOptions.RemoveEmptyEntries);

                var partDiv = new string[1]
                {
                    "~~"
                };

                string s = "";

                bool isFirst = true;

                foreach (var part in parts)
                {
                    var temp = part.Split(partDiv, StringSplitOptions.None);

                    var param = parameters.First(x => x.Id == Convert.ToInt32(temp[0]));
                    var value = temp[1];

                    if (isFirst)
                    {
                        if (string.IsNullOrEmpty(value))
                            s = emptyNamePart;
                        else
                            if (param.HasPredefinedValues)
                                s = param.GetTableEntry(value).Value;
                            else
                                s = value;

                        isFirst = false;
                    }
                    else
                        if (string.IsNullOrEmpty(value))
                            s += nameDivider + emptyNamePart;
                        else
                            if (param.HasPredefinedValues)
                                s += nameDivider + param.GetTableEntry(value).Value;
                            else
                                s += nameDivider + value;
                }

                this.Name = s;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает названия параметров.
        /// </summary>
        /// <returns>Названия параметров.</returns>
        public List<string> GetParamNames()
        {
            var result = new List<string>();

            foreach (var entry in this.searchEntry.ParamValues)
                result.Add(entry.Key.Name);

            return result;
        }

        #endregion
    }
}