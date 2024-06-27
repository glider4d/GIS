using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет конвертер, используемый для получения противоположного булева значения.
    /// </summary>
    public sealed partial class InverseBoolConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    public sealed partial class InverseBoolConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Булево значение.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary>
        /// Конвертирует значение обратно.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Не реализовано.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Не реализована конвертация противоположного булева значения в прямое");
        }

        #endregion
    }
}