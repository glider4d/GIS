using Kts.Gis.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для получения действия над картой в зависимости от инструмента.
    /// </summary>
    internal sealed partial class ToolToMapActionConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class ToolToMapActionConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Тип действия над картой.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Tool)value)
            {
                case Tool.Editor:
                    return MapAction.Edit;

                case Tool.Ellipse:
                case Tool.Line:
                case Tool.NewRuler:
                case Tool.Polygon:
                case Tool.Rectangle:
                case Tool.Ruler:
                    return MapAction.Draw;

                case Tool.GroupArea:
                    return MapAction.EditGroup;

                case Tool.Label:
                    return MapAction.Text;

                case Tool.PrintArea:
                    return MapAction.SetPrintArea;

                case Tool.Selector:
                    return MapAction.Select;
            }

            throw new NotImplementedException("Не реализована конвертация в действие над картой следующего инструмента: " + ((Tool)value).ToString());
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
            throw new NotImplementedException("Не реализована конвертация действия над картой в инструмент");
        }

        #endregion
    }
}