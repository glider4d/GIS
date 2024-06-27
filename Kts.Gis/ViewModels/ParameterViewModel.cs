using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Messaging;
using Kts.ParameterGrid;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
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
        /// Значение, указывающее на то, что имеется ли внутренняя ошибка.
        /// </summary>
        private bool hasInternalError;

        /// <summary>
        /// Значение, указывающее на то, что отфильтрован ли параметр.
        /// </summary>
        private bool isFilteredOut;

        /// <summary>
        /// Параметр.
        /// </summary>
        private ParameterModel param;

        public ParameterModel getParam()
        {
            return param;
        }

        /// <summary>
        /// Набор значений параметров.
        /// </summary>
        private ParameterValueSetViewModel parameterValueSet;

        /// <summary>
        /// Значение параметра.
        /// </summary>
        private object value;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private readonly AccessService accessService;

        /// <summary>
        /// Значение, указывающее на то, что может ли параметр быть раскрашенным.
        /// </summary>
        private readonly bool canBeColored;

        /// <summary>
        /// Идентификатор населенного пункта.
        /// </summary>
        private readonly int cityId;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Значение, указывающее на то, что являются ли значение параметра насильно значением только для чтения.
        /// </summary>
        private readonly bool isForcedReadOnly;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Объект.
        /// </summary>
        private readonly ITypedObjectViewModel obj;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть вычисляемого параметра.
        /// </summary>
        private static SolidColorBrush calculateBrush = new SolidColorBrush(Colors.Blue);

        /// <summary>
        /// Кисть измененного параметра.
        /// </summary>
        private static SolidColorBrush changedBrush = new SolidColorBrush(Colors.Green);

        /// <summary>
        /// Заблокированная кисть.
        /// </summary>
        private static SolidColorBrush disabledBrush = new SolidColorBrush(Colors.Gray);

        /// <summary>
        /// Кисть ошибки.
        /// </summary>
        private static SolidColorBrush errorBrush = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// Нормальная кисть.
        /// </summary>
        private static SolidColorBrush normalBrush = new SolidColorBrush(Colors.Black);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterViewModel"/>.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="depthLevel">Уровень глубины вложенности параметра.</param>
        /// <param name="isForcedReadOnly">Значение, указывающее на то, что являются ли значение параметра насильно значением только для чтения.</param>
        /// <param name="canBeColored">Значение, указывающее на то, что может ли параметр быть раскрашенным.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="objectType">Тип объекта.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ParameterViewModel(ParameterModel param, ParameterValueSetViewModel parameterValueSet, object initialValue, int depthLevel, bool isForcedReadOnly, bool canBeColored, int cityId, ObjectType objectType, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, IMessageService messageService) : this(param, parameterValueSet, param.Name, initialValue, depthLevel, isForcedReadOnly, canBeColored, cityId, objectType, layerHolder, accessService, dataService, messageService)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterViewModel"/>.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="depthLevel">Уровень глубины вложенности параметра.</param>
        /// <param name="isForcedReadOnly">Значение, указывающее на то, что являются ли значение параметра насильно значением только для чтения.</param>
        /// <param name="canBeColored">Значение, указывающее на то, что может ли параметр быть раскрашенным.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="objectType">Тип объекта.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ParameterViewModel(ParameterModel param, ParameterValueSetViewModel parameterValueSet, string header, object initialValue, int depthLevel, bool isForcedReadOnly, bool canBeColored, int cityId, ObjectType objectType, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, IMessageService messageService)
        {
            this.messageService = messageService;

            this.parameterValueSet = parameterValueSet;
            this.param = param;
            this.cityId = cityId;
            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.dataService = dataService;
            
            this.obj = this.parameterValueSet.ParameterizedObject as ITypedObjectViewModel;

            if (param.HasPredefinedValues && param.LoadLevel == LoadLevel.Always)
                // Если таблица параметра должна быть постоянно обновляемой, то мы ее должны обновить.
                param.Table = this.dataService.ParameterAccessService.UpdateTable(param.Table, this.cityId, this.obj.Type, this.layerHolder.CurrentSchema);

            if (param.Format.IsArray)
                this.DisplayedValue = initialValue;
            else
            {
                this.Value = initialValue;

                if (param.Format.IsDirect)
                    this.Value = param.GetDirectValue(parameterValueSet.ParameterValueSet);

                if (param.Format.IsViewery)
                    this.Value = this.GetVieweryValue();
            }
            this.DepthLevel = depthLevel;
            this.isForcedReadOnly = isForcedReadOnly;
            this.canBeColored = canBeColored;

            if (accessService.CanViewParameterId)
                this.Header = param.Id + ". " + header;
            else
                this.Header = header;

            // Замораживаем кисти.
            if (calculateBrush.CanFreeze)
                calculateBrush.Freeze();
            if (changedBrush.CanFreeze)
                changedBrush.Freeze();
            if (disabledBrush.CanFreeze)
                disabledBrush.Freeze();
            if (errorBrush.CanFreeze)
                errorBrush.Freeze();
            if (normalBrush.CanFreeze)
                normalBrush.Freeze();

            if ((this.param.Alias == Alias.ObjectId || this.param.Alias == Alias.GroupId || this.param.Alias == Alias.FuelId || this.param.Alias == Alias.JurId || this.param.Alias == Alias.KvpId) && this.accessService.CanViewIdParameter)
                this.isForcedReadOnly = true;

#warning Тут раньше была блокировка смены планируемости объектов
            // Для зафиксированных населенных пунктов нужно заблокировать смену планируемости.
            //if (this.param.Alias == Alias.IsPlanning && layerHolder.CurrentSchema.IsFixed)
            //this.isForcedReadOnly = true;

            // Нужно блочить смену типа объекта, если у него есть дочерние объекты.
            if (this.param.Alias == Alias.TypeId && this.obj is IContainerObjectViewModel && (this.obj as IContainerObjectViewModel).GetChildren().Count > 0)
                this.isForcedReadOnly = true;

            if (this.param.Alias == Alias.LineLength && !this.isForcedReadOnly)
            {
                // Ограничиваем редактирование параметра, отвечающего за длину линии - нельзя редактировать длину линии, которая сгруппирована с другой линией.
                var line = this.parameterValueSet.ParameterizedObject as LineViewModel;
                if (line != null)
                    this.isForcedReadOnly = line.GroupedLine != line;
            }

            if (objectType.ReadonlyParameters.Contains(param))
                this.isForcedReadOnly = true;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает просматриваемое значение.
        /// </summary>
        /// <returns>Некое значение.</returns>
        private object GetVieweryValue()
        {
            try
            {
                var value = this.dataService.ParameterAccessService.GetVieweryValue(this.param, (parameterValueSet.ParameterizedObject as IObjectViewModel).Id, this.layerHolder.CurrentSchema);

                return value;
            }
            catch
            {
                this.hasInternalError = true;

                return "ОШИБКА";
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеются ли ошибки линии.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <returns>Значение, указывающее на то, что имеются ли ошибки линии.</returns>
        private bool HasLineErrors(object value)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(Convert.ToString(value)) && this.param.Alias == Alias.LineLength)
            {
                // Также, в том случае, если параметр представляет длину линии, мы должны принять во внимание текущий масштаб линий карты. Задаваемая длина линия должна находиться в пределах +-15% от масштаба.
                var line = this.parameterValueSet.ParameterizedObject as LineViewModel;
                if (line != null)
                {
                    if (!line.IsLengthNormal(Convert.ToDouble(value)))
                        result = true;
                }
                else
                    // В данном случае, проверяемый объект является слоем. Тогда мы должны проверить все объекты слоя.
                    foreach (var obj in (this.parameterValueSet.ParameterizedObject as LayerViewModel).Objects)
                        if (!(obj as LineViewModel).IsLengthNormal(Convert.ToDouble(value)))
                        {
                            result = true;

                            break;
                        }
            }

            return result;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Меняет значение параметра в тихом режиме.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="isSilent">Значение, указывающее на то, что нужно ли произвести изменение в тихом режиме (не уведомляя объект об изменении).</param>
        public void ChangeValue(object value, bool isSilent)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(value)))
                this.value = this.param.Format.Format(value);
            else
                this.value = null;

            if (!Equals(this.parameterValueSet.ParameterValueSet.ParameterValues[this.param], this.Value))
            {
                var prevValue = this.parameterValueSet.ParameterValueSet.ParameterValues[this.param];

                this.parameterValueSet.ParameterValueSet.ParameterValues[this.param] = this.Value;

                if (!isSilent && !this.obj.Type.ReadonlyParameters.Contains(param))
                    this.parameterValueSet.ParameterizedObject.NotifyParameterValueChanged(this.param, prevValue, this.Value);

                this.parameterValueSet.UpdateParameters();
            }

            this.NotifyPropertyChanged(nameof(this.Value));

            this.DisplayedValue = param.GetValueAsString(this.Value, this.parameterValueSet.ParameterValueSet);
        }

        /// <summary>
        /// Обновляет состояние параметра.
        /// </summary>
        public void Refresh()
        {
            this.NotifyPropertyChanged(nameof(this.IsVisible));
            this.NotifyPropertyChanged(nameof(this.PredefinedValues));
            
            var val = this.parameterValueSet.ParameterValueSet.ParameterValues[this.param];

            this.NotifyPropertyChanged(nameof(this.HeaderBrush));
            this.NotifyPropertyChanged(nameof(this.ValueBrush));
            
            this.value = string.IsNullOrEmpty(Convert.ToString(val)) || this.HasLineErrors(val) ? null : this.param.Format.Format(val);

            if (this.param.Format.IsDirect)
                this.value = this.param.GetDirectValue(this.parameterValueSet.ParameterValueSet);

            if (this.param.Format.IsViewery)
                this.value = this.GetVieweryValue();

            this.DisplayedValue = param.GetValueAsString(this.Value, this.parameterValueSet.ParameterValueSet);

            if (this.param.ProvidingParameter != null && !this.param.Format.IsDirect && !this.param.Format.IsViewery)
            {
                var temp = this.PredefinedValues;

                if (temp.Count > 0)
                {
                    if (!temp.Any(x => Convert.ToInt32(x.Item1) == Convert.ToInt32(this.Value)))
                    {
                        var newValue = temp.First().Item1;

                        this.parameterValueSet.ParameterValueSet.ParameterValues[this.param] = newValue;

                        this.parameterValueSet.ParameterizedObject.ChangeChangedValue(this.param, newValue);
                    }
                }
                else
                {
                    this.parameterValueSet.ParameterValueSet.ParameterValues[this.param] = null;

                    this.parameterValueSet.ParameterizedObject.ChangeChangedValue(this.param, null);
                }
            }
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
                if (this.hasInternalError)
                    return errorBrush;

                if (this.canBeColored)
                    if (this.param.Format.IsCalculate && !this.param.Format.IsGroup)
#warning Тут мы напрямую работаем с параметрами
                        if (this.param.Alias == Alias.HydraulicsDiameter)
                        {
                            // Находим диаметр линии.
                            var paramDiam = this.obj.Type.Parameters.First(x => x.Alias == Alias.LineDiameter);

                            if (!Equals(this.parameterValueSet.ParameterizedObject.ParameterValuesViewModel.Parameters.First(x => x.param == paramDiam).DisplayedValue, this.DisplayedValue))
                                // Если они не равны, то отмечаем заголовок параметра как ошибочный.
                                return errorBrush;
                            else
                                return calculateBrush;
                        }
                        else
                            return calculateBrush;

                if ((this.isForcedReadOnly || this.obj.Type.IsParameterReadonly(this.param)) && !this.param.Format.IsGroup)
                    return disabledBrush;

                if (this.canBeColored)
                {
                    if (this.obj.Type.IsParameterNecessary(this.param) && string.IsNullOrEmpty(Convert.ToString(this.Value)))
                        return errorBrush;
                        
                    if (this.parameterValueSet.ParameterizedObject.HasChangedValue(this.param))
                        return changedBrush;
                }

                return normalBrush;
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
            get
            {
                return this.isFilteredOut;
            }
            set
            {
                this.isFilteredOut = value;

                this.NotifyPropertyChanged(nameof(this.IsFilteredOut));
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что виден ли параметр.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (this.param.Alias == Alias.ObjectId && this.accessService.CanViewIdParameter)
                    return true;

                if (this.param.Alias == Alias.GroupId && this.accessService.CanViewIdParameter)
                    return true;

                if ((this.param.Alias == Alias.FuelId || this.param.Alias == Alias.JurId || this.param.Alias == Alias.KvpId) && this.accessService.CanViewIdParameter)
                    return true;

                if (this.param.Alias == Alias.TypeId && !this.obj.Type.CanBeChanged)
                    return false;

                return (this.parameterValueSet.ParameterizedObject as ITypedObjectViewModel).Type.IsParameterVisible(this.param, this.parameterValueSet.ParameterValueSet);
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

                    if (this.param.LoadLevel == LoadLevel.Always)
                        this.param.Table = this.dataService.ParameterAccessService.UpdateTable(this.param.Table, this.cityId, this.obj.Type, this.layerHolder.CurrentSchema);

                    foreach (var entry in this.param.GetPredefinedValues(this.parameterValueSet.ParameterValueSet))
                        result.Add(new Tuple<object, string>(entry.Key, entry.Value));

                    return result.OrderBy(x => x.Item2, new NaturalSortComparer<string>()).ToList();
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
                if (!this.param.Format.IsCorrect(value) && !this.hasInternalError)
                {
                    this.messageService.ShowMessage("Заданное значение имеет неправильный формат", "Неверное значение", MessageType.Error);

                    return;
                }

                if (this.HasLineErrors(value) && !this.hasInternalError)
                {
                    this.messageService.ShowMessage("Заданная длина линии не попадает в допустимый промежуток", "Неверное значение", MessageType.Error);

                    return;
                }

                this.ChangeValue(value, false);
            }
        }
        
        /// <summary>
        /// Возвращает кисть значения параметра.
        /// </summary>
        public SolidColorBrush ValueBrush
        {
            get
            {
                if (this.HeaderBrush == errorBrush)
                    return errorBrush;

                if (this.canBeColored)
                    if (this.param.Format.IsCalculate && !this.param.Format.IsGroup)
                        return calculateBrush;

                if (this.isForcedReadOnly || this.obj.Type.IsParameterReadonly(this.param))
                    return disabledBrush;

                if (this.canBeColored)
                    if (this.parameterValueSet.ParameterizedObject.HasChangedValue(this.param))
                        return changedBrush;

                return normalBrush;
            }
        }

        /// <summary>
        /// Возвращает редактор значения.
        /// </summary>
        public ValueEditor ValueEditor
        {
            get
            {
                if (this.isForcedReadOnly || this.obj.Type.IsParameterReadonly(this.param))
                    return ValueEditor.None;

                if (this.param.HasPredefinedValues)
                    return ValueEditor.ComboBox;

                if (this.param.Format.ClrType == typeof(bool))
                    return ValueEditor.CheckBox;

                if (this.param.Format.ClrType == typeof(DateTime))
                    return ValueEditor.DatePicker;

                if (this.param.Format.IsYear)
                    return ValueEditor.YearPicker;

                return ValueEditor.TextBox;
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