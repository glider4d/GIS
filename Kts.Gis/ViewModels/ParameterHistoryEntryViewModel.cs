using Kts.Gis.Models;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления записи истории изменения значения параметра.
    /// </summary>
    internal sealed class ParameterHistoryEntryViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterHistoryEntryViewModel"/>.
        /// </summary>
        /// <param name="entry">Запись истории изменения значения параметра.</param>
        /// <param name="parameter">Параметр.</param>
        public ParameterHistoryEntryViewModel(ParameterHistoryEntryModel entry, ParameterModel parameter)
        {
            this.FromDate = entry.FromDate.ToShortDateString();
            this.Value = parameter.GetValueAsString(entry.Value, null);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает текстовое представление даты начала действия значения параметра.
        /// </summary>
        public string FromDate
        {
            get;
        }

        /// <summary>
        /// Возвращает текстовое представление значения параметра.
        /// </summary>
        public string Value
        {
            get;
        }

        #endregion
    }
}