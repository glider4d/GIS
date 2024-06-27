using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Kts.ParameterGrid
{
    /// <summary>
    /// Представляет интерфейс параметра.
    /// </summary>
    public interface IParameter
    {
        #region Свойства

        /// <summary>
        /// Возвращает дочерние параметры параметра.
        /// </summary>
        List<IParameter> Children
        {
            get;
        }

        /// <summary>
        /// Возвращает уровень глубины вложенности параметра.
        /// </summary>
        int DepthLevel
        {
            get;
        }

        /// <summary>
        /// Возвращает отображаемое значение параметра.
        /// </summary>
        object DisplayedValue
        {
            get;
        }

        /// <summary>
        /// Возвращает заголовок параметра.
        /// </summary>
        string Header
        {
            get;
        }

        /// <summary>
        /// Возвращает кисть заголовка параметра.
        /// </summary>
        SolidColorBrush HeaderBrush
        {
            get;
        }

        /// <summary>
        /// Возвращает стиль шрифта заголовка параметра.
        /// </summary>
        FontStyle HeaderFontStyle
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину шрифта заголовка параметра.
        /// </summary>
        FontWeight HeaderFontWeight
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что отфильтрован ли параметр.
        /// </summary>
        bool IsFilteredOut
        {
            get;
            set;
        }
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что виден ли параметр.
        /// </summary>
        bool IsVisible
        {
            get;
        }

        /// <summary>
        /// Возвращает предопределенные значения.
        /// </summary>
        List<Tuple<object, string>> PredefinedValues
        {
            get;
        }

        /// <summary>
        /// Возвращает единицу измерения параметра.
        /// </summary>
        string Unit
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение параметра.
        /// </summary>
        object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает кисть значения параметра.
        /// </summary>
        SolidColorBrush ValueBrush
        {
            get;
        }

        /// <summary>
        /// Возвращает редактор значения.
        /// </summary>
        ValueEditor ValueEditor
        {
            get;
        }

        /// <summary>
        /// Возвращает стиль шрифта значения параметра.
        /// </summary>
        FontStyle ValueFontStyle
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину шрифта значения параметра.
        /// </summary>
        FontWeight ValueFontWeight
        {
            get;
        }

        /// <summary>
        /// Возвращает тип значения параметра.
        /// </summary>
        Type ValueType
        {
            get;
        }

        #endregion
    }
}