using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления объекта, представляемого значком на карте.
    /// </summary>
    [Serializable]
    internal sealed partial class BadgeViewModel : ObjectViewModel, IChildObjectViewModel, IDeletableObjectViewModel, IEditableObjectViewModel, IHighlightableObjectViewModel, IMapObjectViewModel, INamedObjectViewModel, IParameterizedObjectViewModel, ISelectableObjectViewModel
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
        /// Значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        private bool isEditing;

        /// <summary>
        /// Значение, указывающее на то, что был ли изменен идентификатор объекта.
        /// </summary>
        private bool isIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что инициализирован ли объект.
        /// </summary>
        private bool isInitialized;

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
        /// Объект, представляемый значком на карте.
        /// </summary>
        private readonly BadgeModel badge;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        //[NonSerialized]
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BadgeViewModel"/>.
        /// </summary>
        /// <param name="badge">Объект, представляемый значком на карте.</param>
        /// <param name="parent">Родитель объекта.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public BadgeViewModel(BadgeModel badge, IObjectViewModel parent, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(badge, accessService, dataService, historyService, messageService)
        {
            this.badge = badge;
            this.Parent = parent;
            this.layerHolder = layerHolder;
            this.mapBindingService = mapBindingService;

            this.Distance = badge.Distance;
            
            this.RegisterBinding();

            if (!this.IsSaved)
            {
                // Запоминаем в параметрах необходимые значения и обрабатываем их.
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.TypeId), this.Type.TypeId);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsPlanning), this.IsPlanning);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.IsActive), this.IsActive);
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.BadgeDistance), this.Distance);

                // Также, если у родительской линии есть диаметр, то добавляем его значку.
                this.ChangeChangedValue(this.Type.Parameters.First(x => x.Alias == Alias.Diameter), (parent as LineViewModel).Diameter);
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

        /// <summary>
        /// Возвращает или задает расстояние, на которое объект отдален от объекта-родителя.
        /// </summary>
        public double Distance
        {
            get
            {
                return this.badge.Distance;
            }
            private set
            {
                if (this.Distance != value)
                {
                    this.badge.Distance = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.Distance), value);
                }
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
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "отключение значка"));
            else
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "подключение значка"));

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
                this.HistoryService.Add(new HistoryEntry(action, Target.Data, "вставку значений параметров значка"));
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

            switch (param.Alias)
            {
                case Alias.BadgeDistance:
                    this.Distance = Convert.ToDouble(value);

                    break;

                case Alias.IsActive:
                    this.IsActive = Convert.ToBoolean(value);

                    break;
                    
                case Alias.IsPlanning:
                    this.IsPlanning = Convert.ToBoolean(value);

                    break;
            }
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта по умолчанию
        /// </summary>
        public void LoadParameterDefaultValues()
        {

            if (this.ParameterValuesViewModel == null)
            {
                this.ParameterValuesViewModel = new ParameterValueSetViewModel(this,
                    this.Type.GetDefaultParameterValues(),
                    !this.IsPlaced,
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

        #endregion
    }

    // Реализация ObjectViewModel.
    internal sealed partial class BadgeViewModel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public override bool IsActive
        {
            get
            {
                return this.badge.IsActive;
            }
            set
            {
                this.badge.IsActive = value;

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
            get
            {
                return this.isInitialized;
            }
            set
            {
                if (this.IsInitialized != value)
                {
                    this.isInitialized = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsInitialized), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        public override bool IsPlanning
        {
            get
            {
                return this.badge.IsPlanning;
            }
            set
            {
                this.badge.IsPlanning = value;
            }
        }

        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public override ObjectType Type
        {
            get
            {
                return this.badge.Type;
            }
            set
            {
                this.badge.Type = value;
            }
        }

        #endregion

        #region Открытые переопределенные методы
        
        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        public override void BeginSave()
        {
            this.isSaveStarted = true;

            this.isIdChanged = false;
            this.isParentIdChanged = false;

            this.badge.ParentId = this.Parent.Id;

            this.isParentIdChanged = true;

            /*

            if (this.ChangedParameterValues.ParameterValues.Count > 0 && IsSaved)
            {
                this.DataService.ParameterAccessService.UpdateObjectParamValues(this.badge, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
            }
            else if ( !IsSaved)
            {
                this.badge.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.badge, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                this.isIdChanged = true;
            }
            */

            if (this.ChangedParameterValues.ParameterValues.Count > 0)
                // Сохраняем значения измененных параметров значка.
                if (this.IsSaved)
                    this.DataService.ParameterAccessService.UpdateObjectParamValues(this.badge, this.ChangedParameterValues, this.layerHolder.CurrentSchema);
                else
                {
                    this.badge.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.badge, this.ChangedParameterValues, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }
            else
                if (!this.IsSaved)
                {
                    this.badge.Id = this.DataService.ParameterAccessService.UpdateNewObjectParamValues(this.badge, this.ChangedParameterValues, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }
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
                this.badge.Id = ObjectModel.DefaultId;

            if (this.isParentIdChanged)
                this.badge.ParentId = null;
        }

        #endregion
    }

    // Реализация IChildObjectViewModel.
    internal sealed partial class BadgeViewModel
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

    // Реализация IDeletableObjectViewModel.
    internal sealed partial class BadgeViewModel
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
            this.DataService.BadgeAccessService.DeleteObject(this.badge, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        public void MarkFullDelete()
        {
            this.DataService.BadgeAccessService.MarkDeleteObject(this.badge, this.layerHolder.CurrentSchema);
        }

        #endregion
    }

    // Реализация IEditableObjectViewModel.
    internal sealed partial class BadgeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Вовзращает значение, указывающее на то, что может ли объект редактироваться.
        /// </summary>
        public bool CanBeEdited
        {
            get
            {
                if (this.layerHolder.CurrentSchema.IsActual && this.AccessService.CanDraw || this.layerHolder.CurrentSchema.IsIS && this.AccessService.CanDrawIS)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return this.isEditing;
            }
            set
            {
                if (this.IsEditing != value)
                {
                    this.isEditing = value;

                    this.layerHolder.EditingObject = value ? this : null;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsEditing), value);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что изменен ли объект.
        /// </summary>
        public bool IsModified
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IHighlightableObjectViewModel.
    internal sealed partial class BadgeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Задает значение, указывающее на то, что выделен ли объект.
        /// </summary>
        public bool IsHighlighted
        {
            set
            {
                if (this.IsPlaced)
                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsHighlighted), value);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Убирает выделение объекта.
        /// </summary>
        public void HighlightOff()
        {
            this.mapBindingService.AnimateOff();
        }

        /// <summary>
        /// Выделяет объект.
        /// </summary>
        public void HighlightOn()
        {
            this.mapBindingService.AnimateOn(this);
        }

        /// <summary>
        /// Сбрасывает выделение объекта.
        /// </summary>
        public void ResetHighlight()
        {
            this.mapBindingService.AnimateOff();
        }

        #endregion
    }

    // Реализация IMapObjectViewModel.
    internal sealed partial class BadgeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get
            {
                return this.badge.IsPlaced;
            }
            set
            {
                if (this.IsPlaced != value)
                {
                    this.badge.IsPlaced = value;

                    this.NotifyPropertyChanged(nameof(this.IsPlaced));

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlaced), value);
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Регистрирует связь с представлением.
        /// </summary>
        public void RegisterBinding()
        {
            this.mapBindingService.RegisterBinding(this);
        }

        /// <summary>
        /// Вращает объект карты на заданный угол.
        /// </summary>
        /// <param name="angle">Угол.</param>
        /// <param name="origin">Точка, относительно которой выполняется поворот.</param>
        public void Rotate(double angle, Point origin)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        public void Scale(double scale, Point origin)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void Shift(Point delta)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает связь с представлением.
        /// </summary>
        public void UnregisterBinding()
        {
            this.mapBindingService.UnregisterBinding(this);
        }

        #endregion
    }

    // Реализация INamedObjectViewModel.
    internal sealed partial class BadgeViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Type.SingularName;
            }
        }

        /// <summary>
        /// Возвращает необработанное название объекта, полученное из источника данных.
        /// </summary>
        public string RawName
        {
            get
            {
                return "";
            }
        }

        #endregion
    }

    // Реализация IParameterizedObjectViewModel.
    internal sealed partial class BadgeViewModel
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
            var calcParameterValues = this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.badge, this.layerHolder.CurrentSchema);

            this.CalcParameterValuesViewModel = new ParameterValueSetViewModel(this, calcParameterValues, true, true, this.CityId, this.layerHolder, this.AccessService, this.DataService, this.MessageService);
        }

        /// <summary>
        /// Выполняет асинхронную загрузку значений вычисляемых параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task LoadCalcParameterValuesAsync(CancellationToken cancellationToken)
        {
            //var calcParameterValues = await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.badge, this.layerHolder.CurrentSchema, cancellationToken);
            var calcParameterValues = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectCalcParamValues(this.badge, this.layerHolder.CurrentSchema)
                : await this.DataService.ParameterAccessService.GetObjectCalcParamValuesAsync(this.badge, this.layerHolder.CurrentSchema, cancellationToken);

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
            throw new NotSupportedException("Значок не имеет общие параметры");
        }

        /// <summary>
        /// Выполняет загрузку значений параметров объекта.
        /// </summary>
        public void LoadParameterValues()
        {
            var temp = this.DataService.ParameterAccessService.GetObjectParamValues(this.badge, this.layerHolder.CurrentSchema);

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
            var temp = (this.DataService is WcfDataService) ? this.DataService.ParameterAccessService.GetObjectParamValues(this.badge, this.layerHolder.CurrentSchema)
                :await this.DataService.ParameterAccessService.GetObjectParamValuesAsync(this.badge, this.layerHolder.CurrentSchema, cancellationToken);

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
            this.HistoryService.Add(new HistoryEntry(action, Target.Data, "изменение значения параметра значка"));
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
            throw new NotSupportedException("Значок не имеет общие параметры");
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
    internal sealed partial class BadgeViewModel
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

                    this.IsHighlighted = value;

#warning Раньше два раза вызывалось уведомление об изменении свойства, но это мешает асинхронной загрузке значений параметров, поэтому эту строку пришлось закомментировать
                    // В первый раз уведомляем об изменении для того, чтобы с ранее выбранного объекта был убран выбор.
                    //this.NotifyPropertyChanged(nameof(this.IsSelected));

                    this.NotifyPropertyChanged(nameof(this.IsSelected));

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsSelected), value);

                    this.IsSelected = value;

                    this.layerHolder.SelectedObject = value ? this : null;
                }
            }
        }

        #endregion
    }
}