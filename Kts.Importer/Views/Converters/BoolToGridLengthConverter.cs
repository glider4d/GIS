using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для задания значения типа <see cref="GridLength"/> исходя от булева значения. Если оно равно true, то значение равно <see cref="GridLength.Auto"/>, иначе - 0.
    /// </summary>
    internal sealed partial class BoolToGridLengthConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class BoolToGridLengthConverter
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
            if ((bool)value)
                return GridLength.Auto;

            return new GridLength(0);
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
            throw new NotImplementedException("Не реализована конвертация значения типа " + nameof(GridLength) + " в булево значение");
        }

        #endregion
    }
}