using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события, возвращающий в качестве результата переменную булева типа.
    /// </summary>
    internal sealed class BoolResultEventArgs : EventArgs
    {
        #region Открытые события

        /// <summary>
        /// Возвращает или задает результат.
        /// </summary>
        public bool Result
        {
            get;
            set;
        }

        #endregion
    }
}