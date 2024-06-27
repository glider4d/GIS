using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления слоя объектов.
    /// </summary>
    [Serializable]
    internal sealed partial class LayerViewModel : ServicedViewModel, IHighlightableObjectViewModel, IParameterizedObjectViewModel, ISelectableObjectViewModel, ISetterIgnorer, ITypedObjectViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Максимальное значение прозрачности.
        /// </summary>
        private const int maxOpacity = 1;

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Полное название слоя.
        /// </summary>
        private string fullName;

        /// <summary>
        /// Значение, указывающее на то, что изменилась ли видимость/прозрачность слоя.
        /// </summary>
        private bool isChanged;

        /// <summary>
        /// Значение, указывающее на то, что выбран ли слой.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Значение, указывающее на то, что виден ли слой.
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        /// Количество объектов слоя.
        /// </summary>
        private int objectCount = 0;

        /// <summary>
        /// Значение прозрачности слоя.
        /// </summary>
        private double opacity = 1;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        //[NonSerialized]
        private readonly AccessService accessService;

        /// <summary>
        /// Значение, указывающее на то, что имеет ли слой свой собственный визуальный слой.
        /// </summary>
        private readonly bool hasOwnVisualLayer;

        /// <summary>
        /// Значение, указывающее на то, что имеет ли слой общий визуальный слой.
        /// </summary>
        private readonly bool hasSharedVisualLayer;

        /// <summary>
        /// Сервис истории изменений.
        /// </summary>
        //[NonSerialized]
        private readonly HistoryService historyService;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        //[NonSerialized]
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        //[NonSerialized]
        private readonly IMapBindingService mapBindingService;

        #endregion
        
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LayerViewModel"/>.
        /// </summary>
        /// <param name="objectType">Тип объектов слоя.</param>
        /// <param name="hasSharedVisualLayers">Значение, указывающее на то, что имеют ли слои группы общие визуальные слои.</param>
        /// <param name="hasOwnVisualLayer">Значение, указывающее на то, что имеет ли слой свой собственный визуальный слой.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public LayerViewModel(ObjectType type, bool hasSharedVisualLayer, bool hasOwnVisualLayer, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(dataService, messageService)
        {
            this.Type = type;
            this.hasSharedVisualLayer = hasSharedVisualLayer;
            this.hasOwnVisualLayer = hasOwnVisualLayer;
            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.historyService = historyService;
            this.mapBindingService = mapBindingService;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает кисть, используемую при раскрашивании объектов слоя.
        /// </summary>
        
        public SolidColorBrush Brush
        {
            get
            {
                return this.mapBindingService.GetBrush(this.Type.Color);
            }
        }

        /// <summary>
        /// Возвращает или задает полное название слоя объектов.
        /// </summary>
        public string FullName
        {
            get
            {
                return this.fullName;
            }
            private set
            {
                this.fullName = value;

                this.NotifyPropertyChanged(nameof(this.FullName));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что изменилась ли видимость/прозрачность слоя.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                return this.isChanged;
            }
            private set
            {
                this.isChanged = value;

                this.NotifyPropertyChanged(nameof(this.IsChanged));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что виден ли слой.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (this.hasOwnVisualLayer)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.IsVisible), this.IsVisible, value);
                    this.historyService.Add(new HistoryEntry(action, Target.View, "изменение видимости слоя"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает название слоя.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Type.Name;
            }
        }

        /// <summary>
        /// Возвращает или задает количество объектов слоя.
        /// </summary>
        public int ObjectCount
        {
            get
            {
                return this.objectCount;
            }
            private set
            {
                this.objectCount = value;

                this.UpdateFullName();

                this.NotifyPropertyChanged(nameof(this.ObjectCount));
            }
        }

        /// <summary>
        /// Возвращает или задает объекты слоя.
        /// </summary>
        public AdvancedObservableCollection<IObjectViewModel> Objects
        {
            get
            {
                return m_objects;
            }
        }
        //[NonSerialized]
        AdvancedObservableCollection<IObjectViewModel> m_objects   = new AdvancedObservableCollection<IObjectViewModel>();

        /// <summary>
        /// Возвращает или задает значение прозрачности слоя.
        /// </summary>
        public double Opacity
        {
            get
            {
                return this.opacity;
            }
            set
            {
                if (this.hasOwnVisualLayer)
                {
                    // При изменении прозрачности слоя необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение прозрачности меняется очень часто.
                    var exists = false;
                    var entry = this.historyService.GetCurrentEntry();
                    if (entry != null)
                    {
                        var action = entry.Action as SetPropertyAction;

                        if (action != null && action.Object == this && action.PropertyName == nameof(this.Opacity))
                        {
                            action.NewValue = value;

                            action.Do();

                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new SetPropertyAction(this, nameof(this.Opacity), this.Opacity, value);
                        this.historyService.Add(new HistoryEntry(action, Target.View, "изменение прозрачности слоя"));
                        action.Do();
                    }
                }
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает общие значения измененных параметров объектов.
        /// </summary>
        /// <returns>Набор значений параметров.</returns>
        private ParameterValueSetModel GetParameterCommonValues()
        {
            var result = new Dictionary<ParameterModel, object>();

            // Сперва получаем общие значения параметров сохраненных объектов.
            var objectsIds = this.GetSavedObjectIds();
            var temp = this.DataService.ParameterAccessService.GetGroupParamValues(objectsIds, this.Type, this.layerHolder.CurrentSchema);

            // Заменяем их значениями измененных параметров сохраненных объектов.
            var count = new Dictionary<ParameterModel, int>();
            var values = new Dictionary<ParameterModel, object>();
            foreach (var obj in this.Objects.Where(x => x.IsSaved))
            {
                var paramObj = obj as IParameterizedObjectViewModel;

                if (paramObj != null)
                    foreach (var paramValue in paramObj.ChangedParameterValues.ParameterValues)
                    {
                        if (!values.ContainsKey(paramValue.Key))
                            values.Add(paramValue.Key, null);

                        if (!count.ContainsKey(paramValue.Key))
                            count.Add(paramValue.Key, 0);

                        if (count[paramValue.Key] == 0)
                            values[paramValue.Key] = paramValue.Value;
                        else
                            if (!Equals(values[paramValue.Key], paramValue.Value))
                                continue;

                        count[paramValue.Key]++;
                    }
            }
            var targetCount = objectsIds.Count;
            foreach (var entry in count)
                if (entry.Value != targetCount)
                    temp.ParameterValues[entry.Key] = null;
                else
                    temp.ParameterValues[entry.Key] = values[entry.Key];

            // Теперь получаем значения измененных параметров несохраненных объектов и выполняем конечное сравнение.
            count = new Dictionary<ParameterModel, int>();
            values = new Dictionary<ParameterModel, object>();
            foreach (var obj in this.Objects.Where(x => !x.IsSaved))
            {
                var paramObj = obj as IParameterizedObjectViewModel;

                if (paramObj != null)
                    foreach (var paramValue in paramObj.ChangedParameterValues.ParameterValues)
                    {
                        if (!values.ContainsKey(paramValue.Key))
                            values.Add(paramValue.Key, null);

                        if (!count.ContainsKey(paramValue.Key))
                            count.Add(paramValue.Key, 0);

                        if (count[paramValue.Key] == 0)
                            values[paramValue.Key] = paramValue.Value;
                        else
                            if (!Equals(values[paramValue.Key], paramValue.Value))
                                continue;

                        count[paramValue.Key]++;
                    }
            }
            targetCount = this.Objects.Where(x => !x.IsSaved).Count();
            if (objectsIds.Count == 0)
                result = temp.ParameterValues;
            if (objectsIds.Count > 0)
                foreach (var entry in temp.ParameterValues)
                    if (entry.Key.IsVisible && (!count.ContainsKey(entry.Key) || count[entry.Key] != targetCount || !Equals(values[entry.Key], entry.Value)))
                        result.Add(entry.Key, null);
                    else
                        result.Add(entry.Key, entry.Value);
            else
                foreach (var entry in count)
                    if (entry.Value == targetCount)
                        result[entry.Key] = values[entry.Key];
            if (targetCount == 0)
                result = temp.ParameterValues;

            return new ParameterValueSetModel(result);
        }

        /// <summary>
        /// Асинхронно возвращает общие значения измененных параметров объектов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        private async Task<ParameterValueSetModel> GetParameterCommonValuesAsync(CancellationToken cancellationToken)
        {
#warning В будущем надо будет объединить портянку кода из этого метода с портянкой кода его синхронного аналога
            var result = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());

            // Сперва получаем общие значения параметров сохраненных объектов.
            var objectsIds = this.GetSavedObjectIds();
            var temp = await this.DataService.ParameterAccessService.GetGroupParamValuesAsync(objectsIds, this.Type, this.layerHolder.CurrentSchema, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                // Заменяем их значениями измененных параметров сохраненных объектов.
                var count = new Dictionary<ParameterModel, int>();
                var values = new Dictionary<ParameterModel, object>();
                foreach (var obj in this.Objects.Where(x => x.IsSaved))
                {
                    var paramObj = obj as IParameterizedObjectViewModel;

                    if (paramObj != null)
                        foreach (var paramValue in paramObj.ChangedParameterValues.ParameterValues)
                        {
                            if (!values.ContainsKey(paramValue.Key))
                                values.Add(paramValue.Key, null);

                            if (!count.ContainsKey(paramValue.Key))
                                count.Add(paramValue.Key, 0);

                            if (count[paramValue.Key] == 0)
                                values[paramValue.Key] = paramValue.Value;
                            else
                                if (!Equals(values[paramValue.Key], paramValue.Value))
                                    continue;

                            count[paramValue.Key]++;
                        }
                }
                var targetCount = objectsIds.Count;
                foreach (var entry in count)
                    if (entry.Value != targetCount)
                        temp.ParameterValues[entry.Key] = null;
                    else
                        temp.ParameterValues[entry.Key] = values[entry.Key];

                // Теперь получаем значения измененных параметров несохраненных объектов и выполняем конечное сравнение.
                count = new Dictionary<ParameterModel, int>();
                values = new Dictionary<ParameterModel, object>();
                foreach (var obj in this.Objects.Where(x => !x.IsSaved))
                {
                    var paramObj = obj as IParameterizedObjectViewModel;

                    if (paramObj != null)
                        foreach (var paramValue in paramObj.ChangedParameterValues.ParameterValues)
                        {
                            if (!values.ContainsKey(paramValue.Key))
                                values.Add(paramValue.Key, null);

                            if (!count.ContainsKey(paramValue.Key))
                                count.Add(paramValue.Key, 0);

                            if (count[paramValue.Key] == 0)
                                values[paramValue.Key] = paramValue.Value;
                            else
                                if (!Equals(values[paramValue.Key], paramValue.Value))
                                    continue;

                            count[paramValue.Key]++;
                        }
                }
                targetCount = this.Objects.Where(x => !x.IsSaved).Count();
                if (objectsIds.Count == 0)
                    result = temp.ParameterValues;
                if (objectsIds.Count > 0)
                    foreach (var entry in temp.ParameterValues)
                        if (entry.Key.IsVisible && (!count.ContainsKey(entry.Key) || count[entry.Key] != targetCount || !Equals(values[entry.Key], entry.Value)))
                            result.Add(entry.Key, null);
                        else
                            result.Add(entry.Key, entry.Value);
                else
                    foreach (var entry in count)
                        if (entry.Value == targetCount)
                            result[entry.Key] = values[entry.Key];
                if (targetCount == 0)
                    result = temp.ParameterValues;
            }

            return new ParameterValueSetModel(result);
        }

        /// <summary>
        /// Возвращает список идентификаторов сохраненных объектов.
        /// </summary>
        /// <returns>Список идентификаторов сохраненных объектов.</returns>
        private List<Guid> GetSavedObjectIds()
        {
            var list = new List<Guid>();

            foreach (var obj in this.Objects)
                if (obj.IsSaved)
                    list.Add(obj.Id);

            return list;
        }

        /// <summary>
        /// Убирает объект из слоя.
        /// </summary>
        /// <param name="obj">Убираемый объект.</param>
        /// <param name="removeFromMap">Значение, указывающее на то, что следует ли удалить объект с карты.</param>
        private void RemoveObject(IObjectViewModel obj, bool removeFromMap)
        {
            // Если возможно, завершаем редактирование объекта.
            var editObj = obj as IEditableObjectViewModel;
            if (editObj != null)
                editObj.IsEditing = false;

            // Если возможно, убираем выбор с объекта.
            var selObj = obj as ISelectableObjectViewModel;
            if (selObj != null)
                selObj.IsSelected = false;

            if (removeFromMap && this.hasOwnVisualLayer)
            {
                var mapObj = obj as IMapObjectViewModel;

                // Убираем объект с карты.
                mapObj.IsPlaced = false;
            }
        }

        /// <summary>
        /// Обновляет полное название слоя объектов.
        /// </summary>
        private void UpdateFullName()
        {
            this.FullName = this.Name + " (" + this.ObjectCount.ToString() + ")";
        }

        /// <summary>
        /// Обновляет состояние изменения видимости/прозрачности слоя.
        /// </summary>
        private void UpdateIsChanged()
        {
            this.IsChanged = !this.IsVisible || this.Opacity < maxOpacity;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет в слой заданный объект.
        /// </summary>
        /// <param name="obj">Добавляемый бъект.</param>
        public void Add(IObjectViewModel obj)
        {
            this.Objects.Add(obj);

            this.ObjectCount++;
        }

        /// <summary>
        /// Добавляет заданный ряд объектов.
        /// </summary>
        /// <param name="objects">Добавляемые объекты.</param>
        public void AddRange(List<IObjectViewModel> objects)
        {
            this.Objects.AddRange(objects);

            this.ObjectCount += objects.Count;
        }

        /// <summary>
        /// Очищает слой.
        /// </summary>
        public void Clear()
        {
            this.IsSelected = false;

            foreach (var obj in this.Objects)
                this.RemoveObject(obj, true);

            this.Objects.Clear();

            this.ObjectCount = 0;
        }

        /// <summary>
        /// Возвращает true, если слой содержит заданный объект. Иначе - false.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Значение, указывающее на то, что содержит ли слой заданный объект.</returns>
        public bool Contains(IObjectViewModel obj)
        {
            return this.Objects.Contains(obj);
        }

        /// <summary>
        /// Возвращает объект слоя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект.</returns>
        public IObjectViewModel GetObjectById(Guid id)
        {
            foreach (var obj in this.Objects)
                if (obj.Id == id)
                    return obj;

            return null;
        }

        /// <summary>
        /// Перемещает заданный объект в указанный слой.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="layer">Конечный слой.</param>
        public void MoveTo(IObjectViewModel obj, LayerViewModel layer)
        {
            this.RemoveObject(obj, false);

            this.Objects.Remove(obj);

            this.ObjectCount--;

            layer.Add(obj);
        }

        /// <summary>
        /// Удаляет заданный объект со слоя.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        public void Remove(IObjectViewModel obj)
        {
            this.RemoveObject(obj, true);

            this.Objects.Remove(obj);

            this.ObjectCount--;
        }

        /// <summary>
        /// Выполняет обновление данных о значениях параметров.
        /// </summary>
        public void UpdateParameters()
        {
            var newValues = this.GetParameterCommonValues();

            foreach (var newValue in newValues.ParameterValues)
                this.ParameterValuesViewModel.ParameterValueSet.ParameterValues[newValue.Key] = newValue.Value;

            this.ParameterValuesViewModel.UpdateParameters();
        }

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта по умолчанию
        /// </summary>
        public void LoadCalcParameterDefaultValues()
        {
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта по умолчанию
        /// </summary>
        public void LoadParameterDefaultValues()
        {

            if (this.ParameterValuesViewModel == null)
            {
                var cityId = this.Objects.First().CityId;
                // Проверяем, все ли объекты слоя активны.
                var hasActive = false;
                foreach (var obj in this.Objects)
                    if (obj.IsActive)
                    {
                        hasActive = true;

                        break;
                    }
                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this,
                    this.Type.GetDefaultParameterValues(),
                    !hasActive,
                    false,
                    cityId,
                    this.layerHolder,
                    this.accessService,
                    this.DataService,
                    this.MessageService);
            }
        }

        #endregion
    }

    // Реализация IHighlightableObjectViewModel.
    internal sealed partial class LayerViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Задает значение, указывающее на то, что выделен ли объект.
        /// </summary>
        public bool IsHighlighted
        {
            set
            {
                if (!this.hasSharedVisualLayer)
                    return;

                this.mapBindingService.SetLayerViewValue(this, nameof(this.IsHighlighted), value);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Убирает выделение объекта.
        /// </summary>
        public void HighlightOff()
        {
            if (!this.hasSharedVisualLayer)
                return;

            this.mapBindingService.AnimateOff();
        }

        /// <summary>
        /// Выделяет объект.
        /// </summary>
        public void HighlightOn()
        {
            if (!this.hasSharedVisualLayer)
                return;

            this.mapBindingService.AnimateOn(this.Objects.ToList());
        }

        /// <summary>
        /// Сбрасывает выделение объекта.
        /// </summary>
        public void ResetHighlight()
        {
            if (!this.hasSharedVisualLayer)
                return;

            this.mapBindingService.AnimateOff();
        }

        #endregion
    }

    // Реализация IParameterizedObjectViewModel.
    internal sealed partial class LayerViewModel
    {
        #region Открытые свойства
    
        /// <summary>
        /// Возвращает или задает модель представления значений вычисляемых параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel CalcParameterValuesViewModel
        {
            get
            {
                return m_calcParameterValuesViewModel;
            }
            private set
            {
                m_calcParameterValuesViewModel = value;
            }
        }
        //[NonSerialized]
        ParameterValueSetViewModel m_calcParameterValuesViewModel;
        /// <summary>
        /// Возвращает набор значений измененных параметров объекта.
        /// </summary>
        public ParameterValueSetModel ChangedParameterValues
        {
            get
            {
                return m_changedParameterValues;
            }
        }
        //[NonSerialized]
        ParameterValueSetModel m_changedParameterValues;

        /// <summary>
        /// Возвращает или задает модель представления значений общих параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel CommonParameterValuesViewModel
        {
            get
            {
                return m_commonParameterValuesViewModel;
            }
            private set
            {
                m_commonParameterValuesViewModel = value;
            }
        }
        //[NonSerialized]
        private ParameterValueSetViewModel m_commonParameterValuesViewModel;

        /// <summary>
        /// Возвращает команду копирования параметров.
        /// </summary>
        public RelayCommand CopyParametersCommand
        {
            get
            {
                throw new NotImplementedException("Команда копирования параметров не должна вызываться для слоев объектов");
            }
        }

        /// <summary>
        /// Возвращает или задает модель представления значений параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel ParameterValuesViewModel
        {
            get
            {
                return m_parameterValuesViewModel;
            }
            private set
            {
                m_parameterValuesViewModel = value;
            }
        }
        //[NonSerialized]
        private ParameterValueSetViewModel m_parameterValuesViewModel;

        /// <summary>
        /// Возвращает команду вставки параметров.
        /// </summary>
        public RelayCommand PasteParametersCommand
        {
            get
            {
                throw new NotImplementedException("Команда вставки параметров не должна вызываться для слоев объектов");
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Меняет значение измененного параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="newValue">Новое значение.</param>
        public void ChangeChangedValue(ParameterModel param, object newValue)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Возвращает список параметров, содержащих ошибки в значениях.
        /// </summary>
        /// <returns>Список параметров.</returns>
        public List<ParameterModel> GetErrors()
        {
            return new List<ParameterModel>();
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли заданный параметр измененное значение.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <returns>true, если имеет, иначе - false.</returns>
        public bool HasChangedValue(ParameterModel param)
        {
            return false;
        }

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта.
        /// </summary>
        public void LoadCalcParameterValues()
        {
            throw new NotSupportedException("Слой объектов не имеет вычисляемые параметры");
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений вычисляемых параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public Task LoadCalcParameterValuesAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Слой объектов не имеет вычисляемые параметры");
        }

        /// <summary>
        /// Выполняет загрузку значений общих параметров объекта.
        /// </summary>
        public void LoadCommonParameterValues()
        {
            var cityId = this.Objects.First().CityId;

            var temp = this.DataService.ParameterAccessService.GetGroupCommonParamValues(this.GetSavedObjectIds(), this.Type, this.layerHolder.CurrentSchema);

            this.CommonParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, true, false, cityId, this.layerHolder, this.accessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений общих параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadCommonParameterValuesAsync(CancellationToken cancellationToken)
        {
            var cityId = this.Objects.First().CityId;

            var temp = await this.DataService.ParameterAccessService.GetGroupCommonParamValuesAsync(this.GetSavedObjectIds(), this.Type, this.layerHolder.CurrentSchema, cancellationToken);
            
            if (!cancellationToken.IsCancellationRequested)
                this.CommonParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, true, false, cityId, this.layerHolder, this.accessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта.
        /// </summary>
        public void LoadParameterValues()
        {
            var cityId = this.Objects.First().CityId;

            var temp = this.GetParameterCommonValues();
            // Проверяем, все ли объекты слоя активны.
            var hasActive = false;
            foreach (var obj in this.Objects)
                if (obj.IsActive)
                {
                    hasActive = true;

                    break;
                }

            this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, !hasActive, false, cityId, this.layerHolder, this.accessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadParameterValuesAsync(CancellationToken cancellationToken)
        {
            var cityId = this.Objects.First().CityId;

            var temp = await this.GetParameterCommonValuesAsync(cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                // Проверяем, все ли объекты слоя активны.
                var hasActive = false;
                foreach (var obj in this.Objects)
                    if (obj.IsActive)
                    {
                        hasActive = true;

                        break;
                    }

                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, !hasActive, false, cityId, this.layerHolder, this.accessService, this.DataService, this.MessageService);
            }
        }

        /// <summary>
        /// Уведомляет объект об изменении значения параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="prevValue">Предыдущее значение.</param>
        /// <param name="newValue">Новое значение.</param>
        public void NotifyParameterValueChanged(ParameterModel param, object prevValue, object newValue)
        {
            // Получаем предыдущие значения измененного параметра объектов.
            var temp = new Dictionary<IObjectViewModel, object>();
            // Сперва получаем значения измененного параметра из источника данных.
            var fromSource = this.DataService.ParameterAccessService.GetGroupParamValues(this.GetSavedObjectIds(), this.Type, param, this.layerHolder.CurrentSchema);
            IParameterizedObjectViewModel paramObj;
            foreach (var obj in this.Objects)
            {
                paramObj = obj as IParameterizedObjectViewModel;

                temp.Add(obj, paramObj.ChangedParameterValues.ParameterValues.ContainsKey(param) ? paramObj.ChangedParameterValues.ParameterValues[param] : (fromSource.ContainsKey(obj.Id) ? fromSource[obj.Id] : null));
            }

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new ChangeParametersAction(this, param, newValue, temp);
            this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение значения параметра группы объектов"));
            action.Do();
        }

        /// <summary>
        /// Выполняет выгрузку значений вычисляемых параметров объекта.
        /// </summary>
        public void UnloadCalcParameterValues()
        {
            throw new NotSupportedException("Слой объектов не имеет вычисляемые параметры");
        }

        /// <summary>
        /// Выполняет выгрузку значений общих параметров объекта.
        /// </summary>
        public void UnloadCommonParameterValues()
        {
            this.CommonParameterValuesViewModel = null;
        }

        /// <summary>
        /// Выполняет выгрузку значений параметров объекта.
        /// </summary>
        public void UnloadParameterValues()
        {
            this.ParameterValuesViewModel = null;
        }

        #endregion
    }

    // Реализация ISelectableObjectViewModel.
    internal sealed partial class LayerViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли объект.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;

                this.IsHighlighted = value;
                
                this.NotifyPropertyChanged(nameof(this.IsSelected));

                this.layerHolder.SelectedLayer = value ? this : null;
            }
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class LayerViewModel
    {
        #region Открытые методы

        /// <summary>
        /// Задает значение заданного свойства в обход его сеттера.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        /// <param name="value">Значение.</param>
        public void SetValue(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(this.IsVisible):
                    this.isVisible = (bool)value;

                    break;

                case nameof(this.Opacity):
                    this.opacity = (double)value;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.NotifyPropertyChanged(propertyName);

            this.mapBindingService.SetLayerViewValue(this, propertyName, value);

            this.UpdateIsChanged();
        }

        #endregion
    }

    // Реализация ITypedObjectViewModel.
    internal sealed partial class LayerViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает тип объекта.
        /// </summary>
        public ObjectType Type
        {
            get
            {
                return m_type;
            }
            private set
            {
                m_type = value;
            }
        }

        //[NonSerialized]
        private ObjectType m_type;

        #endregion
    }
}