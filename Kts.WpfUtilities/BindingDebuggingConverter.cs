using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет конвертер, используемый для отладки привязок.
    /// </summary>
    public sealed partial class BindingDebuggingConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    public sealed partial class BindingDebuggingConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Объект.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Останавливаем отладчик, чтобы проверить значение привязки.
            Debugger.Break();

            return value;
        }

        /// <summary>
        /// Конвертирует значение обратно.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Объект.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Останавливаем отладчик, чтобы проверить значение привязки.
            Debugger.Break();

            return value;
        }

        #endregion
    }
}