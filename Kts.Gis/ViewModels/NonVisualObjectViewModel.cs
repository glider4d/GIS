using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления невизуального объекта.
    /// </summary>
    [Serializable]
    internal sealed partial class NonVisualObjectViewModel : ObjectViewModel, IChildObjectViewModel, IContainerObjectViewModel, IDeletableObjectViewModel, IExpandableObjectViewModel, INamedObjectViewModel, IParameterizedObjectViewModel, ISelectableObjectViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Текст подключения объекта.
        /// </summary>
        private const string activate = "Подключить";

        /// <summary>
        /// Текст отключения объекта.
        /// </summary>
        private const string deactivate = "Отключить";

        #endregion

        #region Закрытые поля
        
        /// <summary>
        /// Значение, указывающее на то, что был ли изменен идентификатор объекта.
        /// </summary>
        private bool isIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что был ли изменен идентификатор объекта-родителя.
        /// </summary>
        private bool isParentIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что начато ли сохранение объекта.
        /// </summary>
        private bool isSaveStarted;

        /// <summary>
        /// Значение, указывающее на то, что выбран ли объект.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Объект, способный иметь дочерние объекты.
        /// </summary>
        private readonly ContainerObjectViewModel containerObject;

        /// <summary>
        /// Раскрываемый объект.
        /// </summary>
        private readonly ExpandableObjectViewModel expandableObject;

        /// <summary>
        /// Значение, указывающее на то, что является ли объект поддельным.
        /// </summary>
        private readonly bool isFake;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        //[NonSerialized]
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Именованный объект.
        /// </summary>
        private readonly NamedObjectViewModel namedObject;

        /// <summary>
        /// Невизуальный объект.
        /// </summary>
        private readonly NonVisualObjectModel nonVisualObject;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NonVisualObjectViewModel"/>.
        /// </summary>
        /// <param name="nonVisualObject">Невизуальный объект.</param>
        /// <param name="parent">Родитель объекта.</param>
        /// <param name="isFake">Значение указывающее на то, что является ли объект поддельным.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public NonVisualObjectViewModel(NonVisualObjectModel nonVisualObject, IObjectViewModel parent, bool isFake, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(nonVisualObject, accessService, dataService, historyService, messageService)
        {
            this.nonVisualObject = nonVisualObject;
            this.Parent = parent;
            this.isFake = isFake;
            this.layerHolder = layerHolder;

            this.containerObject = new ContainerObjectViewModel(this, nonVisualObject, nonVisualObject.HasChildren, layerHolder, accessService, dataService, historyService, mapBindingService, this.MessageService);
            this.expandableObject = new ExpandableObjectViewModel(this);
            this.namedObject = new NamedObjectViewModel(this, nonVisualObject.Name, false, mapBindingService);
            
            if (!this.IsSaved)
            {
                // Запоминаем в параметрах необходимые значения и обрабатываем их.
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.TypeId), this.Type.TypeId);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsPlanning), this.IsPlanning);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsActive), this.IsActive);
            }

            this.CopyParametersCommand = new RelayCommand(this.ExecuteCopyParameters, this.CanExecuteCopyParameters);
            this.PasteParametersCommand = new RelayCommand(this.ExecutePasteParameters, this.CanExecutePasteParameters);

            this.DeactivateCommand = new RelayCommand(this.ExecuteDeactivate, this.CanExecuteDeactivate);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду отключения объекта.
        /// </summary>
        public RelayCommand DeactivateCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает текст команды отключения объекта.
        /// </summary>
        public string DeactivateText
        {
            get
            {
                return this.IsActive ? deactivate : activate;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду копирования значений параметров.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteCopyParameters()
        {
            return this.AccessService.IsTypePermitted(this.Type.TypeId);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить отключение объекта.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDeactivate()
        {
            return this.AccessService.IsTypePermitted(this.Type.TypeId);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду вставки значений параметров.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecutePasteParameters()
        {
            return this.AccessService.IsTypePermitted(this.Type.TypeId) && this.IsActive && ClipboardManager.HasStoredParameters;
        }
        
        /// <summary>
        /// Выполняет копирование значений параметров.
        /// </summary>
        private void ExecuteCopyParameters()
        {
            this.LoadParameterValues();

            var values = new Dictionary<ParameterModel, object>();

            foreach (var entry in this.ParameterValuesViewModel.ParameterValueSet.ParameterValues)
                if (!string.IsNullOrEmpty(Convert.ToString(entry.Value)) && !this.Type.IsParameterReadonly(entry.Key) && entry.Key.CanBeCopied)
                    values.Add(entry.Key, entry.Value);

            ClipboardManager.StoreParameters(values);

            this.UnloadParameterValues();
        }

        /// <summary>
        /// Выполняет отключение объекта.
        /// </summary>
        private void ExecuteDeactivate()
        {
            var action = new DeactivateObjectAction(this);

            if (this.IsActive)
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "отключение невизуального объекта"));
            else
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "подключение невизуального объекта"));

            action.Do();
        }

        /// <summary>
        /// Выполняет вставку значений параметров.
        /// </summary>
        private void ExecutePasteParameters()
        {
            // Получаем список параметров, значения которых нужно вставить.
            var parameters = ClipboardManager.GetSelectedParameters(this.Type);

            // Получаем их значения.
            var values = ClipboardManager.RetrieveParameters(parameters);

            var oldValues = new Dictionary<ParameterModel, object>();

            if (parameters.Count > 0)
            {
                foreach (var entry in values)
                    oldValues.Add(entry.Key, this.ParameterValuesViewModel.ParameterValueSet.ParameterValues[entry.Key]);

                // Запоминаем действие в истории изменений и выполняем его.
                var action = new PasteParametersAction(this, oldValues, values);
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "вставку значений параметров невизуального объекта"));
                action.Do();
            }
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением значения параметра значка.
        /// </summary>
        /// <param name="param">Измененный параметр.</param>
        private void OnParameterChanged(ParameterModel param)
        {
            var value = this.ChangedParameterValues.ParameterValues[param];
            
            if (this.Type.CaptionParameters.Contains(param))
                this.namedObject.UpdateName(param, Convert.ToString(value));

            switch (param.Alias)
            {
                case Alias.IsActive:
                    this.IsActive = Convert.ToBoolean(value);

                    break;

                case Alias.IsPlanning:
                    this.IsPlanning = Convert.ToBoolean(value);

                    break;

                case Alias.TypeId:
                    var newType = this.DataService.GetObjectType(Convert.ToInt32(value));

                    // Если тип был изменен, то нужно удалить все значения измененных параметров, чтобы они не были видны пользователю.
                    if (newType != this.Type)
                    {
                        this.Type = newType;

                        // Составляем список свойств, не входящих в новый тип объекта.
                        var removeList = new List<ParameterModel>();
                        foreach (var p in this.ChangedParameterValues.ParameterValues)
                            if (!this.Type.Parameters.Contains(p.Key))
                                removeList.Add(p.Key);

                        // Удаляем эти свойства.
                        foreach (var p in removeList)
                            this.ChangedParameterValues.ParameterValues.Remove(p);
                    }

                    // Меняем название объекта.
                    this.namedObject.ProcessRawName("");
                    this.namedObject.UpdateName();

                    break;
            }
        }

        #endregion
    }

    // Реализация ObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public override bool IsActive
        {
            get
            {
                return this.nonVisualObject.IsActive;
            }
            set
            {
                this.nonVisualObject.IsActive = value;

                if (this.ParameterValuesViewModel != null)
                {
                    this.IsSelected = false;

                    this.IsSelected = true;
                }

                this.NotifyPropertyChanged(nameof(this.DeactivateText));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что инициализирован ли объект.
        /// </summary>
        public override bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        public override bool IsPlanning
        {
            get
            {
                return this.nonVisualObject.IsPlanning;
            }
            set
            {
                this.nonVisualObject.IsPlanning = value;
            }
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public override ObjectType Type
        {
            get
            {
                return this.nonVisualObject.Type;
            }
            set
            {
                this.nonVisualObject.Type = value;
            }
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        public override void BeginSave()
        {
            // Если объект поддельный, то не сохраняем его.
            if (this.isFake)
                return;

            this.isSaveStarted = true;

            this.isIdChanged = false;
            this.isParentIdChanged = false;

            this.nonVisualObject.ParentId = this.Parent.Id;

            this.isParentIdChanged = true;

            if (this.ChangedParameterValues.ParameterValues.Count > 0)
                // Сохраняем значения измененных параметров невизуального объекта.
                if (this.IsSaved)
                    this.DataService.ParameterAccessService.UpdateObjectParamValues(this.nonVisualObject, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                else
                {
                    this.nonVisualObject.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.nonVisualObject, this.ChangedParameterValues, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }
            else
                if (!this.IsSaved)
                {
                    this.nonVisualObject.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.nonVisualObject, this.ChangedParameterValues, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }

            // Сохраняем дочерние объекты фигуры.
            foreach (var layer in this.ChildrenLayers)
                foreach (var obj in layer.Objects)
                    obj.BeginSave();
        }

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        public override void EndSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            if (this.ChangedParameterValues.ParameterValues.Count > 0)
                this.ChangedParameterValues.ParameterValues.Clear();
        }

        /// <summary>
        /// Отменяет сохранение объекта в источнике данных.
        /// </summary>
        public override void RevertSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            if (this.isIdChanged)
                this.nonVisualObject.Id = ObjectModel.DefaultId;

            if (this.isParentIdChanged)
                this.nonVisualObject.ParentId = null;
        }

        #endregion
    }

    // Реализация IChildObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает родительский объект.
        /// </summary>
        public IObjectViewModel Parent
        {
            get;
        }

        #endregion
    }

    // Реализация IContainerObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает модели представлений добавления дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<AddChildViewModel> AddChildViewModels
        {
            get
            {
                return this.containerObject.AddChildViewModels;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что загружены ли дочерние объекты.
        /// </summary>
        public bool AreChildrenLoaded
        {
            get
            {
                return this.containerObject.AreChildrenLoaded;
            }
        }

        /// <summary>
        /// Возвращает слои дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<LayerViewModel> ChildrenLayers
        {
            get
            {
                return this.containerObject.ChildrenLayers;
            }
        }

        /// <summary>
        /// Возвращает типы дочерних объектов.
        /// </summary>
        public List<ObjectType> ChildrenTypes
        {
            get
            {
                return this.nonVisualObject.ChildrenTypes;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.nonVisualObject.HasChildren;
            }
        }

        /// <summary>
        /// Возвращает модели представлений выбора дочерних объектов.
        /// </summary>
        public Utilities.AdvancedObservableCollection<SelectChildViewModel> SelectChildViewModels
        {
            get
            {
                // Если дочерние объекты еще не загружены, то загружаем их.
                if (!this.AreChildrenLoaded)
                    this.LoadChildren();

                this.containerObject.SelectChildViewModels.Clear();

                foreach (var layer in this.ChildrenLayers)
                    foreach (var obj in layer.Objects)
                        this.containerObject.SelectChildViewModels.Add(new SelectChildViewModel(obj));

                return this.containerObject.SelectChildViewModels;
            }
        }

        #endregion

        #region Открытые методы


        /// <summary>
        /// Выполняет загрузку значений параметров объекта по умолчанию
        /// </summary>
        public void LoadParameterDefaultValues()
        {

            if (this.ParameterValuesViewModel == null)
            {
                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this,
                    this.Type.GetDefaultParameterValues(),
                    false,
                    true,
                    this.CityId,
                    this.layerHolder,
                    this.AccessService,
                    this.DataService,
                    this.MessageService);
            }
        }

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта по умолчанию
        /// </summary>
        public void LoadCalcParameterDefaultValues()
        {
            if (this.CalcParameterValuesViewModel == null)
            {
                this.CalcParameterValuesViewModel =
                    new ParameterValueSetViewModel(this,
                    this.Type.GetDefaultCalcParameterValues(),
                    true,
                    true,
                    this.CityId,
                    this.layerHolder,
                    this.AccessService,
                    this.DataService,
                    this.MessageService);
            }
        }

        /// <summary>
        /// Добавляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void AddChild(IObjectViewModel child)
        {
            this.containerObject.AddChild(child);
        }

        /// <summary>
        /// Добавляет новый дочерний объект.
        /// </summary>
        /// <param name="type">Тип дочернего объекта.</param>
        public void AddChild(ObjectType type)
        {
            this.containerObject.AddChild(type);
        }

        /// <summary>
        /// Удаляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void DeleteChild(IObjectViewModel child)
        {
            this.containerObject.DeleteChild(child);
        }

        /// <summary>
        /// Удаляет дочерний объект из источника данных.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void FullDeleteChild(IObjectViewModel child)
        {
            this.containerObject.FullDeleteChild(child);
        }

        /// <summary>
        /// Возвращает список дочерних объектов.
        /// </summary>
        /// <returns>Список дочерних объектов.</returns>
        public List<IObjectViewModel> GetChildren()
        {
            return this.containerObject.GetChildren();
        }

        /// <summary>
        /// Загружает дочерние объекты из источника данных.
        /// </summary>
        public void LoadChildren()
        {
            this.containerObject.LoadChildren();
        }

        /// <summary>
        /// Загружает дочерние объекты из заданного набора данных.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        public void LoadChildren(DataSet dataSet)
        {
            this.containerObject.LoadChildren(dataSet);
        }

        #endregion
    }

    // Реализация IDeletableObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить объект.
        /// </summary>
        public bool CanBeDeleted
        {
            get
            {
                return this.AccessService.IsTypePermitted(this.Type.TypeId) && this.IsPlanning;
            }
        }

        /// <summary>
        /// Возвращает команду удаления объекта.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return (this.Parent as IDeletableObjectViewModel)?.DeleteCommand;
            }
        }

        /// <summary>
        /// Возвращает команду полного удаления объекта.
        /// </summary>
        public RelayCommand FullDeleteCommand
        {
            get
            {
                return (this.Parent as IDeletableObjectViewModel)?.FullDeleteCommand;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет полное удаление объекта из источника данных.
        /// </summary>
        public void FullDelete()
        {
            this.DataService.NonVisualObjectAccessService.DeleteObject(this.nonVisualObject, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        public void MarkFullDelete()
        {
            this.DataService.NonVisualObjectAccessService.MarkDeleteObject(this.nonVisualObject, this.layerHolder.CurrentSchema);
        }

        #endregion
    }

    // Реализация IExpandableObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что раскрыт ли объект.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.expandableObject.GetIsExpanded();
            }
            set
            {
                if (this.IsExpanded != value)
                    this.expandableObject.SetIsExpanded(value);
            }
        }

        #endregion
    }

    // Реализация INamedObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string Name
        {
            get
            {
                return this.namedObject.Name;
            }
        }

        /// <summary>
        /// Возвращает необработанное название объекта, полученное из источника данных.
        /// </summary>
        public string RawName
        {
            get
            {
                return this.namedObject.RawName;
            }
        }

        #endregion
    }

    // Реализация IParameterizedObjectViewModel.
    internal sealed partial class NonVisualObjectViewModel
    {
        #region Открытые свойства
    
        /// <summary>
        /// Возвращает или задает модель представления значений вычисляемых параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel CalcParameterValuesViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает набор значений измененных параметров объекта.
        /// </summary>
        public ParameterValueSetModel ChangedParameterValues
        {
            get;
        } = new ParameterValueSetModel(new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer()));

        /// <summary>
        /// Возвращает или задает модель представления значений общих параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel CommonParameterValuesViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду копирования параметров.
        /// </summary>
        public RelayCommand CopyParametersCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает модель представления значений параметров объекта.
        /// </summary>
        public ParameterValueSetViewModel ParameterValuesViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду вставки параметров.
        /// </summary>
        public RelayCommand PasteParametersCommand
        {
            get;
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
            if (!this.IsActive)
                if (param.Alias != Alias.IsActive)
                    return;

            // Если на данный момент отображены параметры объекта, то нужно уведомить об изменении значения одного из них таким образом, чтобы это изменение не внеслось в историю изменений.
            if (this.ParameterValuesViewModel != null)
            {
                var temp = this.ParameterValuesViewModel.GetParameter(param.Id);

                if (!Equals(temp.Value, newValue))
                    temp.ChangeValue(newValue, true);
            }

            if (this.ChangedParameterValues.ParameterValues.ContainsKey(param))
                this.ChangedParameterValues.ParameterValues[param] = newValue;
            else
                this.ChangedParameterValues.ParameterValues.Add(param, newValue);

            this.OnParameterChanged(param);
        }

        /// <summary>
        /// Возвращает список параметров, содержащих ошибки в значениях.
        /// </summary>
        /// <returns>Список параметров.</returns>
        public List<ParameterModel> GetErrors()
        {
            var result = new List<ParameterModel>();

            if (this.IsSaved)
            {
                // Если объект сохранен, то это предполагает то, что ошибки могут быть только в значениях измененных параметров.
                foreach (var entry in this.ChangedParameterValues.ParameterValues)
                    if (this.Type.IsParameterVisible(entry.Key, this.ChangedParameterValues) && this.Type.IsParameterNecessary(entry.Key) && string.IsNullOrEmpty(Convert.ToString(entry.Value)))
                        result.Add(entry.Key);
            }
            else
                // Иначе, необходимо обойти все параметры типа объекта.
                foreach (var param in this.Type.Parameters)
                    if (this.Type.IsParameterVisible(param, this.ChangedParameterValues) && this.Type.IsParameterNecessary(param) && (!this.ChangedParameterValues.ParameterValues.ContainsKey(param) || string.IsNullOrEmpty(Convert.ToString(this.ChangedParameterValues.ParameterValues[param]))))
                        result.Add(param);

            return result;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли заданный параметр измененное значение.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <returns>true, если имеет, иначе - false.</returns>
        public bool HasChangedValue(ParameterModel param)
        {
            return this.ChangedParameterValues.ParameterValues.ContainsKey(param);
        }

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта.
        /// </summary>
        public void LoadCalcParameterValues()
        {
            var calcParameterValues = this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.nonVisualObject, this.layerHolder.CurrentSchema);

            this.CalcParameterValuesViewModel = new ParameterValueSetViewModel(this, calcParameterValues, true, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений вычисляемых параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadCalcParameterValuesAsync(CancellationToken cancellationToken)
        {
            //var calcParameterValues = await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.nonVisualObject, this.layerHolder.CurrentSchema, cancellationToken);
            var calcParameterValues = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.nonVisualObject, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.nonVisualObject, this.layerHolder.CurrentSchema, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                this.CalcParameterValuesViewModel = new ParameterValueSetViewModel(this, calcParameterValues, true, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений общих параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public Task LoadCommonParameterValuesAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Невизуальный объект не имеет общие параметры");
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта.
        /// </summary>
        public void LoadParameterValues()
        {
            var temp = this.DataService.ParameterAccessService.GetObjectParamValues(this.nonVisualObject, this.layerHolder.CurrentSchema);

            foreach (var entry in this.ChangedParameterValues.ParameterValues)
                temp.ParameterValues[entry.Key] = entry.Value;

            this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, false, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadParameterValuesAsync(CancellationToken cancellationToken)
        {
            //var temp = await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.nonVisualObject, this.layerHolder.CurrentSchema, cancellationToken);
            var temp = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectParamValues(this.nonVisualObject, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.nonVisualObject, this.layerHolder.CurrentSchema, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                foreach (var entry in this.ChangedParameterValues.ParameterValues)
                    temp.ParameterValues[entry.Key] = entry.Value;

                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this, temp, false, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
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
            // Запоминаем действие в истории изменений и выполняем его.
            var action = new ChangeParameterAction(this, param, prevValue, newValue);
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение значения параметра невизуального объекта"));
            action.Do();
        }

        /// <summary>
        /// Выполняет выгрузку значений вычисляемых параметров объекта.
        /// </summary>
        public void UnloadCalcParameterValues()
        {
            this.CalcParameterValuesViewModel = null;
        }

        /// <summary>
        /// Выполняет выгрузку значений общих параметров объекта.
        /// </summary>
        public void UnloadCommonParameterValues()
        {
            throw new NotSupportedException("Невизуальный объект не имеет общие параметры");
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
    internal sealed partial class NonVisualObjectViewModel
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
                if (this.IsSelected != value)
                {
                    this.isSelected = value;

                    this.NotifyPropertyChanged(nameof(this.IsSelected));

                    // Управляем возможностью добавления дочерних объектов.
                    if (value)
                        foreach (var type in this.Type.Children.OrderBy(x => x.SingularName))
                        {
                            var addChildViewModel = new AddChildViewModel(this, type, type.ObjectKind == ObjectKind.Badge);

                            this.AddChildViewModels.Add(addChildViewModel);
                        }
                    else
                        this.AddChildViewModels.Clear();

                    this.layerHolder.SelectedObject = value ? this : null;
                }
            }
        }

        #endregion
    }
}