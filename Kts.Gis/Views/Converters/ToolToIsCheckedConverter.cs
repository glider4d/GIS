using Kts.Gis.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для выделения в панели инструментов определенного инструмента.
    /// </summary>
    /// <remarks>
    /// В методе <see cref="Convert(object, Type, object, CultureInfo)"/> в качестве параметра передается целочисленное значение, являющееся индексом определенного значения перечисления инструментов <see cref="Tool"/>.
    /// </remarks>
    internal sealed partial class ToolToIsCheckedConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class ToolToIsCheckedConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Выделенность инструмента.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Tool)value == (Tool)System.Convert.ToInt32(parameter))
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
        /// <returns>Тип инструмента.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Tool)System.Convert.ToInt32(parameter);
        }

        #endregion
    }
}