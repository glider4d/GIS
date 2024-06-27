using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения текстового представления уникального идентификатора.
    /// </summary>
    internal sealed partial class GuidToIdConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class GuidToIdConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Текстовое представление уникального идентификатора.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var guid = (Guid?)value;

            if (!guid.HasValue)
                return "-";

            // Возвращаем только первые шестнадцать символов уникального идентификатора.
            return guid.ToString().Substring(0, 16) + "...";
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
            throw new NotImplementedException("Не реализована конвертация текстового представления уникального идентификатора в уникальный идентификатор");
        }

        #endregion
    }
}