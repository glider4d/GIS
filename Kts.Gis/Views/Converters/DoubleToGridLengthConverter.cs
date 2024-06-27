using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для конвертации значения типа <see cref="double"/> в <see cref="GridLength"/> и обратно.
    /// </summary>
    internal sealed partial class DoubleToGridLengthConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class DoubleToGridLengthConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Значение типа <see cref="GridLength"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength(System.Convert.ToDouble(value));
        }

        /// <summary>
        /// Конвертирует значение обратно.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Значение типа <see cref="double"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((GridLength)value).Value;
        }

        #endregion
    }
}