using Kts.Utilities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения текстового представления идентификатора территориальной единицы.
    /// </summary>
    internal sealed partial class TerritorialEntityToIdConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class TerritorialEntityToIdConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Текстовое представление идентификатора территориальной единицы.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = value as TerritorialEntityViewModel;

            if (t == null)
                return "-";

            return t.Id.ToString();
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
            throw new NotImplementedException("Не реализована конвертация текстового представления идентификатора в территориальную единицу");
        }

        #endregion
    }
}