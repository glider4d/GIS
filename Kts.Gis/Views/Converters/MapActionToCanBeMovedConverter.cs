using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения значения возможности перемещения из значения типа <see cref="MapAction"/>.
    /// </summary>
    internal sealed partial class MapActionToCanBeMovedConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class MapActionToCanBeMovedConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Булево значение.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((MapAction)value == MapAction.Edit)
                return true;

            return false;
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
            throw new NotImplementedException("Не реализована конвертация значения возможности перемещения в значение типа MapAction");
        }

        #endregion
    }
}