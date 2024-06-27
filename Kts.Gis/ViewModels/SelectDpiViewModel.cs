using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора DPI.
    /// </summary>
    internal sealed class SelectDpiViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает список DPI.
        /// </summary>
        public List<int> Dpi
        {
            get;
        } = new List<int>()
        {
            72,
            96,
            150,
            300,
            400,
            500,
            600,
            700,
            800,
            900,
            1000,
            1100,
            1200
        };

        /// <summary>
        /// Возвращает или задает выбранный DPI.
        /// </summary>
        public int SelectedDpi
        {
            get;
            set;
        } = 300;

        #endregion
    }
}