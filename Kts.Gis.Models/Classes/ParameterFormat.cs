using Kts.Utilities;
using System;
using System.Text.RegularExpressions;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет формат параметра.
    /// </summary>
    [Serializable]
    public sealed class ParameterFormat
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterFormat"/>.
        /// </summary>
        /// <param name="format">Текстовое представление формата.</param>
        public ParameterFormat(string format)
        {
            switch (format)
            {
                case "array":
                    this.ClrType = typeof(string);

                    this.IsArray = true;

                    break;

                case "bigint":
                    this.ClrType = typeof(long);

                    this.IsNumeric = true;

                    break;

                case "bit":
                    this.ClrType = typeof(bool);

                    this.IsBoolean = true;

                    break;

                case "calculate":
                    this.ClrType = typeof(double);

                    this.IsCalculate = true;

                    this.IsNumeric = true;

                    break;

                case "date":
                    this.ClrType = typeof(DateTime);

                    this.IsShortDateTime = true;

                    break;

                case "group":
                    this.ClrType = typeof(string);

                    this.IsGroup = true;

                    break;

                case "groupcalculate":
                    this.ClrType = typeof(string);

                    this.IsCalculate = true;

                    this.IsGroup = true;

                    break;

                case "GTCB":
                    this.ClrType = typeof(Guid);

                    break;

                case "int":
                    this.ClrType = typeof(int);

                    this.IsNumeric = true;

                    break;

                case "jur":
                    this.ClrType = typeof(long);

                    this.IsNumeric = true;

                    break;

                case "kv":
                    this.ClrType = typeof(long);

                    this.IsNumeric = true;

                    break;

                case "smalldatetime":
                    this.ClrType = typeof(DateTime);

                    this.IsShortDateTime = true;

                    break;

                case "smallint":
                    this.ClrType = typeof(short);

                    this.IsNumeric = true;

                    break;

                case "table":
                    this.ClrType = typeof(string);

                    this.IsDirect = true;

                    break;

                case "TCB":
                    this.ClrType = typeof(int);

                    break;

                case "tinyint":
                    this.ClrType = typeof(byte);

                    this.IsNumeric = true;

                    break;

                case "uniqueidentifier":
                    this.ClrType = typeof(Guid);

                    break;

                case "viewery":
                    this.ClrType = typeof(string);

                    this.IsViewery = true;

                    break;

                case "year":
                    this.ClrType = typeof(int);

                    this.IsYear = true;

                    break;
            }

            // Паттерн для получения значения из формата заключенного в скобки.
            var pattern = @"\((.*?)\)";

            var regex = new Regex(pattern);

            // Разделитель значений.
            var div = new char[1]
            {
                ','
            };

            if (format.Contains("decimal") || format.Contains("numeric"))
            {
                if (format.Contains("decimal"))
                    this.ClrType = typeof(decimal);
                else
                    this.ClrType = typeof(double);

                var s = regex.Match(format).Value.Split(div);

                this.MaxLength = Convert.ToInt32(s[0].Remove(0, 1));
                this.FractionLength = Convert.ToInt32(s[1].Replace(")", ""));

                this.IsNumeric = true;
            }

            if (format.Contains("varchar"))
            {
                this.ClrType = typeof(string);

                var s = regex.Match(format).Value.Remove(0, 1).Replace(")", "");

                this.MaxLength = Convert.ToInt32(s);
            }

            if (this.ClrType == null)
                throw new NotImplementedException("Не реализована работа со следующим форматом параметра: " + format);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Тип CLR.
        /// </summary>
        public Type ClrType
        {
            get;
        }

        /// <summary>
        /// Возвращает длину дробной части.
        /// </summary>
        public int FractionLength
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли значение параметра массивом значений.
        /// </summary>
        public bool IsArray
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли формат параметра логическим.
        /// </summary>
        public bool IsBoolean
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли формат параметра вычисляемым.
        /// </summary>
        public bool IsCalculate
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли значение параметра прямым, то есть должно ли оно напрямую извлекаться из источника данных.
        /// </summary>
        public bool IsDirect
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что представляет ли значение формата группу параметров.
        /// </summary>
        public bool IsGroup
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли формат параметра числовым.
        /// </summary>
        public bool IsNumeric
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли формат параметра короткой датой.
        /// </summary>
        public bool IsShortDateTime
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что должно ли значение параметра браться напрямую из представления в источнике данных.
        /// </summary>
        public bool IsViewery
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли формат параметра годом.
        /// </summary>
        public bool IsYear
        {
            get;
        }

        /// <summary>
        /// Возвращает максимальную длину значения формата.
        /// </summary>
        public int MaxLength
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает отформатированное значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Отформатированное значение.</returns>
        public object Format(object value)
        {
            if (value == null)
                // Если значение равно null, то берем значение формата по умолчанию.
                return this.GetDefaultValue();

            if (this.ClrType == typeof(Guid))
                return Guid.Parse(value.ToString());

            // Иначе, конвертируем его в правильный формат.
            var newValue = Convert.ChangeType(value, this.ClrType);

            if (this.ClrType == typeof(decimal) || this.ClrType == typeof(double))
            {
                if (this.FractionLength != default(int))
                    newValue = Math.Round(Convert.ToDouble(newValue), this.FractionLength);
            }
            else
                // Предполагается, что далее мы будет оперировать со строкой.
                if (this.MaxLength != default(int) && ((string)newValue).Length > this.MaxLength)
                    newValue = ((string)newValue).Substring(0, this.MaxLength);

            if (newValue != null && Convert.ToString(newValue).Contains("|"))
                throw new ArgumentException("Значение не должно содержать символ '|'");

            return newValue;
        }

        /// <summary>
        /// Возвращает значение формата по умолчанию.
        /// </summary>
        /// <returns>Значение формата по умолчанию.</returns>
        public object GetDefaultValue()
        {
            return TypeHelper.GetDefaultValue(this.ClrType);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли заданное значение правильный формат.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>Значение, указывающее на то, что имеет ли заданное значение правильный формат.</returns>
        public bool IsCorrect(object value)
        {
            bool result = false;

            try
            {
                this.Format(value);

                if (value != null)
                    if (this.ClrType == typeof(DateTime))
                        if (((DateTime)value).Year < 1900)
                            return false;

                return true;
            }
            catch
            {
            }

            return result;
        }

        #endregion
    }
}