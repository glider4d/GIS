using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения значения типа <see cref="Visibility"/> в зависимости от ряда булевых значений.
    /// </summary>
    /// <remarks>
    /// Если все булевы значения равны true, то будет возвращено значение <see cref="Visibility.Visible"/>. Иначе - <see cref="Visibility.Collapsed"/>.
    /// </remarks>
    internal sealed partial class BooleansToVisibilityConverter : IMultiValueConverter
    {
    }

    // Реализация IMultiValueConverter.
    internal sealed partial class BooleansToVisibilityConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="values">Конвертируемые значения.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Видимость.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
                if (!(bool)value)
                    return Visibility.Collapsed;

            return Visibility.Visible;
        }

        /// <summary>
        /// Конвертирует значение обратно.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetTypes">Конечные типы.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Не реализовано.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Не реализована конвертация видимости в булевы значения");
        }

        #endregion
    }
}