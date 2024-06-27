using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения значения типа <see cref="Visibility"/> в зависимости от вещественного значения.
    /// </summary>
    internal sealed partial class DoubleToVisibilityConverter : IValueConverter
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает направление конвертации.
        /// </summary>
        public bool IsInverse
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IValueConverter.
    internal sealed partial class DoubleToVisibilityConverter
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
                if (System.Convert.ToInt32(value) == 0)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }

            if (System.Convert.ToInt32(value) == 0)
                return Visibility.Collapsed;

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
            throw new NotImplementedException("Не реализована конвертация видимости в вещественное значение");
        }

        #endregion
    }
}