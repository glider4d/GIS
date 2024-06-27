using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kts.ParameterGrid
{
    /// <summary>
    /// Представляет конвертер, используемый для получения отступа в зависимости от уровня глубины вложенности параметра.
    /// </summary>
    internal sealed partial class DepthLevelToMarginConverter : IValueConverter
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает начальный отступ.
        /// </summary>
        public int InitialOffset
        {
            get;
            set;
        }

        #endregion
    }

    // IValueConverter.
    internal sealed partial class DepthLevelToMarginConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Отступ.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(this.InitialOffset * (int)value, 0, 0, 0);
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
            throw new NotImplementedException("Не реализована конвертация отступа в уровень глубины вложенности параметра");
        }

        #endregion
    }
}