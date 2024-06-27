using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения кисти в зависимости от уникального идентификатор объекта.
    /// </summary>
    internal sealed partial class GuidToSolidColorBrushConverter : IValueConverter
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
        /// Возвращает или задает кисть, используемое при отсутствии значения уникального идентификатора.
        /// </summary>
        public SolidColorBrush NullBrush
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IValueConverter.
    internal sealed partial class GuidToSolidColorBrushConverter
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
            return ((Guid?)value).HasValue ? this.DefaultBrush : this.NullBrush;
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
            throw new NotImplementedException("Не реализована конвертация кисти в уникальный идентификатор");
        }

        #endregion
    }
}