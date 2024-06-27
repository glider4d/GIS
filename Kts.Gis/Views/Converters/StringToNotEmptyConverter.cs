using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для замены пустой строки на <see cref="DefaultValue"/>.
    /// </summary>
    internal sealed partial class StringToNotEmptyConverter : IValueConverter
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение по умолчанию.
        /// </summary>
        public string DefaultValue
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IValueConverter.
    internal sealed partial class StringToNotEmptyConverter 
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Непустая строка.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == "")
                return this.DefaultValue;

            return value.ToString();
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
            throw new NotImplementedException("Не реализована конвертация значения по умолчанию в строку");
        }

        #endregion
    }
}