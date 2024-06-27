using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kts.ParameterGrid
{
    /// <summary>
    /// Представляет конвертер, используемый для получения значения типа <see cref="DataGridLength"/> из вещественного значения.
    /// </summary>
    internal sealed partial class DoubleToDataGridLengthConverter : IValueConverter
    {
    }

    // IValueConverter.
    internal sealed partial class DoubleToDataGridLengthConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Значение типа <see cref="DataGridLength"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DataGridLength((double)value);
        }

        /// <summary>
        /// Конвертирует значение обратно.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Вещественное значение.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DataGridLength)value).Value;
        }

        #endregion
    }
}