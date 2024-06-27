using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения значения типа <see cref="Visibility"/> исходя от булева значения, причем, значение <see cref="Visibility.Visible"/> равняется true.
    /// </summary>
    internal sealed partial class BoolToVisibilityConverter : IValueConverter
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что обратно ли определение видимости от булева значения.
        /// </summary>
        public bool IsInverse
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IValueConverter.
    internal sealed partial class BoolToVisibilityConverter
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
            if (this.IsInverse)
            {
                if ((bool)value)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }

            if ((bool)value)
                return Visibility.Visible;

            return Visibility.Collapsed;
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
            throw new NotImplementedException("Не реализована конвертация видимости в булево значение");
        }

        #endregion
    }
}