using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.ParameterGrid
{
    /// <summary>
    /// Представляет конвертер, используемый для скрытия расширителя, если количество дочерних объектов равно 0.
    /// </summary>
    internal sealed partial class ObjectCountToVisibilityConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class ObjectCountToVisibilityConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Видимость.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) == 0)
                return Visibility.Hidden;

            return Visibility.Visible;
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
            throw new NotImplementedException("Не реализована конвертация видимости расширителя в количество дочерних объектов");
        }

        #endregion
    }
}