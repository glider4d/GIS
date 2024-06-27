using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения кисти в зависимости от объекта.
    /// </summary>
    internal sealed partial class ObjectToSolidColorBrushConverter : IValueConverter
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает кисть по умолчанию.
        /// </summary>
        public SolidColorBrush DefaultBrush
        {
            get;
            set;
        }
        
        /// <summary>
        /// Возвращает или задает кисть, используемое при пустом объекте (null).
        /// </summary>
        public SolidColorBrush NullBrush
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IValueConverter.
    internal sealed partial class ObjectToSolidColorBrushConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Кисть.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? this.NullBrush : this.DefaultBrush;
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
            throw new NotImplementedException("Не реализована конвертация кисти в объект");
        }

        #endregion
    }
}