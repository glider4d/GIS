using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса импорта/экспорта данных.
    /// </summary>
    internal sealed class ImportExportRequestedEventArgs : EventArgs
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает название файла.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        #endregion
    }
}