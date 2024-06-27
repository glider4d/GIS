using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель записи истории изменения значения параметра.
    /// </summary>
    /// 
    [Serializable]
    public sealed class ParameterHistoryEntryModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterHistoryEntryModel"/>.
        /// </summary>
        /// <param name="fromDate">Дата начала действия значения параметра.</param>
        /// <param name="value">Значение параметра.</param>
        public ParameterHistoryEntryModel(DateTime fromDate, object value)
        {
            this.FromDate = fromDate;
            this.Value = value;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает дату начала действия значения параметра.
        /// </summary>
        public DateTime FromDate
        {
            get;
        }

        /// <summary>
        /// Возвращает значение параметра.
        /// </summary>
        public object Value
        {
            get;
        }

        #endregion
    }
}