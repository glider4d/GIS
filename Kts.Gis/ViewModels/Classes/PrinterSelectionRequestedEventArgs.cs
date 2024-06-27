using System;
using System.Printing;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса выбора принтера.
    /// </summary>
    internal sealed class PrinterSelectionRequestedEventArgs : EventArgs
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает выбранный принтер.
        /// </summary>
        public PrintQueue SelectedPrinter
        {
            get;
            set;
        }

        #endregion
    }
}