using System;
using System.Collections.Generic;
using System.Text;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель набора значений параметров.
    /// </summary>
    [Serializable]
    public sealed class ParameterValueSetModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterValueSetModel"/>.
        /// </summary>
        /// <param name="parameterValues">Набор значений параметров.</param>
        public ParameterValueSetModel(Dictionary<ParameterModel, object> parameterValues)
        {
            this.ParameterValues = parameterValues;
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает набор значений параметров.
        /// </summary>
        public Dictionary<ParameterModel, object> ParameterValues
        {
            get;
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Добавляет в заданную строку идентификатор параметра и его значение.
        /// </summary>
        /// <param name="sb">Строка.</param>
        /// <param name="paramId">Идентификатор параметра.</param>
        /// <param name="paramValue">Значение параметра.</param>
        private void AppendParams(StringBuilder sb, int paramId, string paramValue)
        {
            sb.Append(paramId);
            sb.Append(" ");
            sb.Append(paramValue);
            sb.Append("|");
        }

        #endregion

        #region Открытые переопределенные методы


        private void encodingString(KeyValuePair<ParameterModel, object> entry, ref StringBuilder sb)
        {
            
            if (entry.Key.Format.ClrType == typeof(decimal) || entry.Key.Format.ClrType == typeof(double))
                this.AppendParams(sb, entry.Key.Id, entry.Value.ToString().Replace(",", "."));
            else
                    if (entry.Key.Format.ClrType == typeof(DateTime))
                this.AppendParams(sb, entry.Key.Id, DateTime.Parse(Convert.ToString(entry.Value)).ToString("yyyy-MM-dd"));
            else
                this.AppendParams(sb, entry.Key.Id, entry.Value.ToString());
        }

        public string ToStringWithEmpty()
        {
            var sb = new StringBuilder();
            foreach( var entry in this.ParameterValues)
            {
                if (entry.Value == null) continue;
                encodingString(entry, ref sb);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Возвращает текстовое представление текущего экземпляра класса.
        /// </summary>
        /// <returns>Строка.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            
            foreach (var entry in this.ParameterValues)
            {
                if (entry.Value == null || entry.Value.ToString() == "")
                    // Обходим пустые параметры.
                    continue;
                encodingString(entry, ref sb);
                /*
                if (entry.Key.Format.ClrType == typeof(decimal) || entry.Key.Format.ClrType == typeof(double))
                    this.AppendParams(sb, entry.Key.Id, entry.Value.ToString().Replace(",", "."));
                else
                    if (entry.Key.Format.ClrType == typeof(DateTime))
                        this.AppendParams(sb, entry.Key.Id, DateTime.Parse(Convert.ToString(entry.Value)).ToString("yyyy-MM-dd"));
                    else
                        this.AppendParams(sb, entry.Key.Id, entry.Value.ToString());*/
            }

            return sb.ToString();
        }

        #endregion
    }
}