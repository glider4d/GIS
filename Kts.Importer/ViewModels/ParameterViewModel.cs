using Kts.Gis.Models;
using Kts.ParameterGrid;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления параметра.
    /// </summary>
    internal sealed partial class ParameterViewModel : BaseViewModel, IParameter
    {
        #region Закрытые поля

        /// <summary>
        /// Отображаемое значение параметра.
        /// </summary>
        private object displayedValue;

        /// <summary>
        /// Параметр.
        /// </summary>
        private ParameterModel param;

        /// <summary>
        /// Набор значений параметров.
        /// </summary>
        private ParameterValueSetViewModel parameterValueSet;

        /// <summary>
        /// Значение параметра.
        /// </summary>
        private object value;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Заблокированная кисть.
        /// </summary>
        private static SolidColorBrush disabledBrush = new SolidColorBrush(Colors.Gray);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterViewModel"/>.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="depthLevel">Уровень глубины вложенности параметра.</param>
        public ParameterViewModel(ParameterModel param, ParameterValueSetViewModel parameterValueSet, object initialValue, int depthLevel)
        {
            this.parameterValueSet = parameterValueSet;
            this.param = param;
            this.Value = initialValue;
            this.DepthLevel = depthLevel;

            this.Header = param.Name;

            if (disabledBrush.CanFreeze)
                disabledBrush.Freeze();
        }

        #endregion
    }

    // Реализация IParameter.
    internal sealed partial class ParameterViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает дочерние параметры параметра.
        /// </summary>
        public List<IParameter> Children
        {
            get;
        } = new List<IParameter>();

        /// <summary>
        /// Возвращает уровень глубины вложенности параметра.
        /// </summary>
        public int DepthLevel
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает отображаемое значение параметра.
        /// </summary>
        public object DisplayedValue
        {
            get
            {
                return this.displayedValue;
            }
            private set
            {
                this.displayedValue = value;

                this.NotifyPropertyChanged(nameof(this.DisplayedValue));
            }
        }

        /// <summary>
        /// Возвращает заголовок параметра.
        /// </summary>
        public string Header
        {
            get;
        }

        /// <summary>
        /// Возвращает кисть заголовка параметра.
        /// </summary>
        public SolidColorBrush HeaderBrush
        {
            get
            {
                return disabledBrush;
            }
        }

        /// <summary>
        /// Возвращает стиль шрифта заголовка параметра.
        /// </summary>
        public FontStyle HeaderFontStyle
        {
            get
            {
                return FontStyles.Normal;
            }
        }

        /// <summary>
        /// Возвращает толщину шрифта заголовка параметра.
        /// </summary>
        public FontWeight HeaderFontWeight
        {
            get
            {
                if (this.param.Format.IsGroup || this.Children.Count > 0)
                    return FontWeights.Bold;

                return FontWeights.Normal;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что отфильтрован ли параметр.
        /// </summary>
        public bool IsFilteredOut
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что виден ли параметр.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.param.IsVisible;
            }
        }

        /// <summary>
        /// Возвращает предопределенные значения.
        /// </summary>
        public List<Tuple<object, string>> PredefinedValues
        {
            get
            {
                if (this.param.HasPredefinedValues)
                {
                    var result = new List<Tuple<object, string>>();

                    var temp = this.param.GetPredefinedValues(this.parameterValueSet.ParameterValueSet);

                    foreach (var t in temp)
                        result.Add(new Tuple<object, string>(t.Key, t.Value));

                    return result;
                }

                return new List<Tuple<object, string>>();
            }
        }

        /// <summary>
        /// Возвращает единицу измерения параметра.
        /// </summary>
        public string Unit
        {
            get
            {
                return this.param.Unit;
            }
        }

        /// <summary>
        /// Возвращает или задает значение параметра.
        /// </summary>
        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;

                this.NotifyPropertyChanged(nameof(this.Value));

                this.DisplayedValue = this.param.GetValueAsString(value, this.parameterValueSet.ParameterValueSet);
            }
        }

        /// <summary>
        /// Возвращает кисть значения параметра.
        /// </summary>
        public SolidColorBrush ValueBrush
        {
            get
            {
                return disabledBrush;
            }
        }

        /// <summary>
        /// Возвращает редактор значения.
        /// </summary>
        public ValueEditor ValueEditor
        {
            get
            {
                return ValueEditor.None;
            }
        }

        /// <summary>
        /// Возвращает стиль шрифта значения параметра.
        /// </summary>
        public FontStyle ValueFontStyle
        {
            get
            {
                return FontStyles.Normal;
            }
        }

        /// <summary>
        /// Возвращает толщину шрифта значения параметра.
        /// </summary>
        public FontWeight ValueFontWeight
        {
            get
            {
                return FontWeights.Normal;
            }
        }

        /// <summary>
        /// Возвращает тип значения параметра.
        /// </summary>
        public Type ValueType
        {
            get
            {
                return this.param.Format.ClrType;
            }
        }

        #endregion
    }
}