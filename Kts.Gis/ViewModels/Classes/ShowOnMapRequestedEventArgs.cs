using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса отображения объектов на карте.
    /// </summary>
    internal sealed class ShowOnMapRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ShowOnMapRequestedEventArgs"/>.
        /// </summary>
        /// <param name="searchEntry">Записи результата поиска.</param>
        public ShowOnMapRequestedEventArgs(List<SearchEntryViewModel> searchEntries)
        {
            this.SearchEntries = searchEntries;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает записи результата поиска.
        /// </summary>
        public List<SearchEntryViewModel> SearchEntries
        {
            get;
        }

        #endregion
    }
}