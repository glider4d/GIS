using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.Models
{
    [Serializable]
    public class ParameterModelEqualityComparer : IEqualityComparer<ParameterModel>
    {
        public bool Equals(ParameterModel x, ParameterModel y)
        {
            if (y == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Id == y.Id && x.Name.Equals(y.Name))

                return true;
            else
                return false;
        }

        public int GetHashCode(ParameterModel obj)
        {
            int hCode = obj.Id ^ obj.Id.GetHashCode() ^ obj.Name.GetHashCode();
            return hCode;
        }
    }

    /// <summary>
    /// Представляет модель параметра.
    /// </summary>
    [Serializable]
    public sealed class ParameterModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Название поля, по которому фильтруются предопределенные значения параметра.
        /// </summary>
        private readonly string filterField;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор параметра.</param>
        /// <param name="name">Название параметра.</param>
        /// <param name="alias">Псевдоним параметра.</param>
        /// <param name="format">Формат параметра.</param>
        /// <param name="isVisible">Значение, указывающее на то, что видим ли параметр.</param>
        /// <param name="table">Таблица, предоставляющая предопределенные значения для параметра.</param>
        /// <param name="providingParameter">Параметр, предоставляющий значение для фильтрации предопределенных значений параметра.</param>
        /// <param name="filterField">Название поля, по которому фильтруются предопределенные значения параметра.</param>
        /// <param name="isCommon">Значение, указывающее на то, что является ли данный параметр общим параметром.</param>
        /// <param name="loadLevel">Уровень загрузки справочников параметра.</param>
        /// <param name="isSearchable">Значение, указывающее на то, что участвует ли параметр в поиске.</param>
        /// <param name="unit">Единица измерения значения параметра.</param>
        public ParameterModel(int id, string name, string alias, ParameterFormat format, bool isVisible, TableModel table, ParameterModel providingParameter, string filterField, bool isCommon, LoadLevel loadLevel, bool isSearchable, string unit)
        {
            this.Id = id;
            this.Name = name;
            this.Alias = this.GetAliasFromString(alias);
            this.Format = format;
            this.IsVisible = isVisible;
            this.Table = table;
            this.ProvidingParameter = providingParameter;
            this.filterField = filterField;
            this.IsCommon = isCommon;
            this.LoadLevel = loadLevel;
            this.IsSearchable = isSearchable;
            this.Unit = unit;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает псевдоним параметра.
        /// </summary>
        public Alias Alias
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли скопировать значение параметра.
        /// </summary>
        public bool CanBeCopied
        {
            get
            {
#warning Тут задаются айдишники параметров, которые нельзя копировать
                // Нельзя копировать значения параметров "Активность объекта", "Тип объекта" и "Планируемый".
                if (this.Id == 2 || this.Id == 3 || this.Id == 5)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Возвращает формат параметра.
        /// </summary>
        public ParameterFormat Format
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли параметр предопределенные значения.
        /// </summary>
        public bool HasPredefinedValues
        {
            get
            {
                return this.Table != null;
            }
        }

        /// <summary>
        /// Возвращает идентификатор параметра.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли данный параметр общим параметром.
        /// </summary>
        public bool IsCommon
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что участвует ли параметр в поиске.
        /// </summary>
        public bool IsSearchable
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что видим ли параметр.
        /// </summary>
        public bool IsVisible
        {
            get;
        }

        /// <summary>
        /// Возвращает уровень загрузки справочников параметра.
        /// </summary>
        public LoadLevel LoadLevel
        {
            get;
        }

        /// <summary>
        /// Возвращает название параметра.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает параметр, предоставляющий значение для фильтрации предопределенных значений параметра.
        /// </summary>
        public ParameterModel ProvidingParameter
        {
            get;
        }

        /// <summary>
        /// Возвращает таблицу, предоставляющую предопределенные значения для параметра.
        /// </summary>
        public TableModel Table
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает единицу измерения значения параметра.
        /// </summary>
        public string Unit
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает псевдоним из ее текстового представления.
        /// </summary>
        /// <param name="s">Текстовое представление псевдонима.</param>
        /// <returns>Псевдоним.</returns>
        private Alias GetAliasFromString(string s)
        {
            try
            {
                var alias = (Alias)Enum.Parse(typeof(Alias), s);

                return alias;
            }
            catch
            {
            }

            return Alias.None;
        }


        public override bool Equals(object obj)
        {
            //return base.Equals(obj);
            bool result = false;
            if (obj is ParameterModel)
            {
                result = this == (obj as ParameterModel);
            }
            else
            {
                result = ReferenceEquals(this, obj);
            }

            return result;
        }

        public static bool Equals(object objA, object objB)
        {
            bool result = false;


            Type type = objA.GetType();
            Type type2 = objB.GetType();

            if (objA is ParameterModel && objB is ParameterModel)
            {
                result = (objA as ParameterModel) == (objB as ParameterModel);
            }
            else
            {
                result = objA == objB;
            }

            return result;
        }

        public static bool operator ==(ParameterModel c1, ParameterModel c2)
        {
            //return ReferenceEquals(c1, c2) || (!ReferenceEquals(c1,null) && !ReferenceEquals(c2, null));
            bool result = false;
            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
            {
                result = ReferenceEquals(c1, c2);
            }
            else
            {
                result = (c1.Id == c2.Id && c1.Name.Equals(c2.Name));
            }
            return result;
        }

        public static bool operator !=(ParameterModel c1, ParameterModel c2)
        {
            bool result = false;
            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
            {
                result = !ReferenceEquals(c1, c2);
            }
            else
            {
                result = (c1.Id != c2.Id) || (!c1.Name.Equals(c2.Name));
            }

            return result;
        }

      
        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает все предопределенные значения параметра без учета фильтрации.
        /// </summary>
        /// <returns>Предопределенные значения параметра.</returns>
        public List<TableEntryModel> GetAllPredefinedValues()
        {
            return this.Table.GetEntries(this.filterField);
        }

        /// <summary>
        /// Возвращает значение параметра напрямую из источника данных.
        /// </summary>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <returns>Значение параметра.</returns>
        public object GetDirectValue(ParameterValueSetModel parameterValueSet)
        {
            if (this.ProvidingParameter == null)
                return null;

            if (string.IsNullOrEmpty(Convert.ToString(parameterValueSet.ParameterValues[this.ProvidingParameter])))
                return null;

            return this.Table.GetEntry(this.filterField, Convert.ToInt32(parameterValueSet.ParameterValues[this.ProvidingParameter])).Value;
        }
        
        /// <summary>
        /// Возвращает предопределенные значения параметра.
        /// </summary>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <returns>Предопределенные значения параметра.</returns>
        public List<TableEntryModel> GetPredefinedValues(ParameterValueSetModel parameterValueSet)
        {
            if (parameterValueSet == null || parameterValueSet.ParameterValues == null || parameterValueSet.ParameterValues.Count == 0)
                return this.GetAllPredefinedValues();

            if (!string.IsNullOrEmpty(this.filterField))
                return this.Table.GetEntries(this.filterField, Convert.ToInt32(parameterValueSet.ParameterValues[this.ProvidingParameter]));

            return this.Table.GetEntries(null, null);
        }

        /// <summary>
        /// Возвращает запись таблицы.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Запись таблицы.</returns>
        public TableEntryModel GetTableEntry(object key)
        {
#warning Если есть проблема со справочниками, то смотреть надо тут

            List<TableEntryModel> listTableEntry = this.GetAllPredefinedValues();


            //return this.GetAllPredefinedValues().First(x => Equals(this.Format.Format(x.Key), this.Format.Format(key)));

            //return this.GetAllPredefinedValues().First(x => this.Format.Format(x.Key).Equals( this.Format.Format(key)));

            return this.GetAllPredefinedValues().First(x => this.Format.Format(x.Key).Equals(this.Format.Format(key)));
        }

        /// <summary>
        /// Возвращает значение параметра в правильном текстовом представлении.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <param name="parameterValues">Набор значений параметров.</param>
        /// <returns>Текстовое представление значения параметра.</returns>
        public string GetValueAsString(object value, ParameterValueSetModel parameterValues)
        {
            if (string.IsNullOrEmpty(Convert.ToString(value)))
                return "";

            if (this.Format.IsDirect || this.Format.IsViewery)
                return value.ToString();

            if (this.HasPredefinedValues)
            {
                var entry = this.GetTableEntry(value);
                
                if (entry != null)
                    return entry.Value;
                else
                    return "";
            }

            if (this.Format.ClrType == typeof(bool))
                try
                {
                    return bool.Parse(Convert.ToString(value)) ? "Да" : "Нет";
                }
                catch
                {
                    return Convert.ToBoolean(Convert.ToInt32(Convert.ToString(value))) ? "Да" : "Нет";
                }

            //if (Equals(this.Format.GetDefaultValue(), this.Format.Format(value)))
            if (this.Format.GetDefaultValue().Equals(this.Format.Format(value)))
                return "";

            if (this.Format.IsShortDateTime)
                return (Convert.ToDateTime(value)).ToShortDateString();

            return value.ToString();
        }

        #endregion
    }
}