using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления заголовка "Утверждено"/"Согласовано".
    /// </summary>
    internal sealed partial class ApprovedHeaderViewModel : BaseViewModel, ICustomLayerObject, IDeletableObjectViewModel, ISavableObjectViewModel, ISetterIgnorer
    {
        #region Закрытые поля

        /// <summary>
        /// Пользовательский слой, которому принадлежит объект.
        /// </summary>
        private CustomLayerViewModel customLayer;

        /// <summary>
        /// Значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        private bool isEditing;

        /// <summary>
        /// Значение, указывающее на то, что изменен ли идентификатор объекта.
        /// </summary>
        private bool isIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        private bool isPlaced;

        /// <summary>
        /// Значение, указывающее на то, что начато ли сохранение.
        /// </summary>
        private bool isSaveStarted;

        #endregion

        #region Закрытые неизменямые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private readonly AccessService accessService;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис истории изменений.
        /// </summary>
        private readonly HistoryService historyService;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Модель.
        /// </summary>
        private readonly ApprovedHeaderModel model;

        #endregion
        
        #region Открытые события

        /// <summary>
        /// Событие редактирования заголовка.
        /// </summary>
        public event EventHandler OnEdit;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApprovedHeaderViewModel"/>.
        /// </summary>
        /// <param name="model">Модель.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public ApprovedHeaderViewModel(ApprovedHeaderModel model, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService)
        {
            this.model = model;
            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.dataService = dataService;
            this.historyService = historyService;
            this.mapBindingService = mapBindingService;

            if (!this.IsSaved)
                this.IsModified = true;

            this.EditCommand = new RelayCommand(this.ExecuteEdit, this.CanExecuteEdit);

            this.RegisterBinding();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду редактирования заголовка.
        /// </summary>
        public RelayCommand EditCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает размер шрифта.
        /// </summary>
        public int FontSize
        {
            get
            {
                return this.model.FontSize;
            }
            set
            {
                if (this.FontSize != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.FontSize), this.FontSize, value);
                    this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение размера шрифта заголовка"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает нижнюю часть заголовка.
        /// </summary>
        public string Footer
        {
            get
            {
                return "М.П.";
            }
        }

        /// <summary>
        /// Возвращает или задает имя утверждающего/составляющего.
        /// </summary>
        public string Name
        {
            get
            {
                return this.model.Name;
            }
            set
            {
                if (this.Name != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Name), this.Name, value);
                    this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение ФИО в заголовке"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает положение надписи.
        /// </summary>
        public System.Windows.Point Position
        {
            get
            {
                return new System.Windows.Point(this.model.Position.X, this.model.Position.Y);
            }
            set
            {
                if (this.Position != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Position), this.Position, value);
                    this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение положения заголовка"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает пост утверждающего/составляющего.
        /// </summary>
        public string Post
        {
            get
            {
                return this.model.Post;
            }
            set
            {
                if (this.Post != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Post), this.Post, value);
                    this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение должности в заголовке"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает заголовок заголовка.
        /// </summary>
        public string Title
        {
            get
            {
                var title = "ОШИБКА";

                switch (this.model.Type)
                {
                    case ApprovedHeaderType.Agreed:
                        title = "\"СОГЛАСОВАНО\"";

                        break;

                    case ApprovedHeaderType.Approved:
                        title = "\"УТВЕРЖДАЮ\"";

                        break;
                }

                return title;
            }
        }

        /// <summary>
        /// Возвращает или задает год утверждения/составления.
        /// </summary>
        public int Year
        {
            get
            {
                return this.model.Year;
            }
            set
            {
                if (this.Year != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Year), this.Year, value);
                    this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение года в заголовке"));
                    action.Do();
                }
            }
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить редактирование заголовка.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteEdit()
        {
            return this.layerHolder.CurrentSchema.IsActual && this.accessService.CanDraw || this.layerHolder.CurrentSchema.IsIS && this.accessService.CanDrawIS;
        }

        /// <summary>
        /// Выполняет редактирование заголовка.
        /// </summary>
        private void ExecuteEdit()
        {
            this.OnEdit?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }

    // Реализация ICustomLayerObject.
    internal sealed partial class ApprovedHeaderViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Вовзращает значение, указывающее на то, что может ли объект редактироваться.
        /// </summary>
        public bool CanBeEdited
        {
            get
            {
                if (this.layerHolder.CurrentSchema.IsActual && this.accessService.CanDraw || this.layerHolder.CurrentSchema.IsIS && this.accessService.CanDrawIS)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Возвращает заголовок, идентифицирующий название объекта.
        /// </summary>
        public string Caption
        {
            get
            {
                return this.Title;
            }
        }

        /// <summary>
        /// Возвращает или задает пользовательский слой, которому принадлежит объект.
        /// </summary>
        public CustomLayerViewModel CustomLayer
        {
            get
            {
                return this.customLayer;
            }
            set
            {
                this.customLayer = value;

                if (value == null)
                    this.IsPlaced = true;
                else
                    this.IsPlaced = (value as CustomLayerViewModel).IsVisible;
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

                    this.NotifyPropertyChanged(nameof(this.IsEditing));
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

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get
            {
                return this.isPlaced;
            }
            set
            {
                if (this.IsPlaced != value)
                {
                    this.isPlaced = value;

                    this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlaced), value);
                }
            }
        }

        /// <summary>
        /// Возвращает команду управления слоем.
        /// </summary>
        public RelayCommand ManageLayerCommand
        {
            get
            {
                return this.layerHolder.ManageCustomObjectLayerCommand;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public void BeginSave(IDataService dataService)
        {
            this.isSaveStarted = true;

            this.isIdChanged = false;

            if (this.IsModified)
            {
                this.model.LayerId = this.CustomLayer == null ? Guid.Empty : this.CustomLayer.Id;

                if (this.IsSaved)
                    dataService.CustomObjectAccessService.UpdateApprovedHeader(this.model, this.layerHolder.CurrentSchema);
                else
                {
                    this.model.Id = dataService.CustomObjectAccessService.UpdateNewApprovedHeader(this.model, this.layerHolder.CurrentSchema);

                    this.isIdChanged = true;
                }
            }
        }

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        public void EndSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            this.IsModified = false;
        }

        /// <summary>
        /// Регистрирует связь с представлением.
        /// </summary>
        public void RegisterBinding()
        {
            this.mapBindingService.RegisterBinding(this);
        }

        /// <summary>
        /// Выполняет поиск кастомного слоя, которому принадлежит объект.
        /// </summary>
        /// <param name="layers">Все кастомные слои.</param>
        public void RestoreLayer(IEnumerable<CustomLayerViewModel> layers)
        {
            if (this.model.LayerId == Guid.Empty)
            {
                this.CustomLayer = null;

                return;
            }

            var layer = layers.FirstOrDefault(x => x.Id == this.model.LayerId);

            if (layer == null)
            {
                // В данном случае что-то произошло со слоем. Скорее всего он был удален. Поэтому нужно затереть упоминание о нем.
                this.CustomLayer = null;
                this.IsModified = true;
            }
            else
                this.CustomLayer = layer;
        }

        /// <summary>
        /// Отменяет сохранение объекта в источнике данных.
        /// </summary>
        public void RevertSave()
        {
            if (!this.isSaveStarted)
                return;

            this.isSaveStarted = false;

            if (this.isIdChanged)
                this.model.Id = Guid.Empty;
        }

        /// <summary>
        /// Вращает объект карты на заданный угол.
        /// </summary>
        /// <param name="angle">Угол.</param>
        /// <param name="origin">Точка, относительно которой выполняется поворот.</param>
        public void Rotate(double angle, System.Windows.Point origin)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        public void Scale(double scale, System.Windows.Point origin)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает кастомный слой.
        /// </summary>
        /// <param name="customLayer">Кастомный слой.</param>
        public void SetCustomLayer(CustomLayerViewModel customLayer)
        {
            var action = new ChangeCustomLayerAction(this, this.CustomLayer, customLayer);

            this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение заголовка"));

            action.Do();

            this.IsModified = true;
        }

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void Shift(System.Windows.Point delta)
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

    // Реализация IDeletableObjectViewModel.
    internal sealed partial class ApprovedHeaderViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить объект.
        /// </summary>
        public bool CanBeDeleted
        {
            get
            {
                return this.accessService.CanDeleteLabel(this.layerHolder.CurrentSchema.IsActual);
            }
        }

        /// <summary>
        /// Возвращает команду удаления объекта.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return this.layerHolder.DeleteCommand;
            }
        }

        /// <summary>
        /// Возвращает команду полного удаления объекта.
        /// </summary>
        public RelayCommand FullDeleteCommand
        {
            get
            {
                return this.layerHolder.FullDeleteCommand;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет полное удаление объекта из источника данных.
        /// </summary>
        public void FullDelete()
        {
            this.dataService.CustomObjectAccessService.DeleteApprovedHeader(this.model, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        public void MarkFullDelete()
        {
            this.dataService.CustomObjectAccessService.MarkDeleteApprovedHeader(this.model, this.layerHolder.CurrentSchema);
        }

        #endregion
    }

    // Реализация ISavableObjectViewModel.
    internal sealed partial class ApprovedHeaderViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли заголовок.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.model.IsSaved;
            }
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class ApprovedHeaderViewModel
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
                case nameof(this.FontSize):
                    this.model.FontSize = (int)value;

                    break;

                case nameof(this.Name):
                    this.model.Name = (string)value;

                    break;

                case nameof(this.Position):
                    var point = (System.Windows.Point)value;

                    this.model.Position = new Point(point.X, point.Y);

                    break;

                case nameof(this.Post):
                    this.model.Post = (string)value;

                    break;

                case nameof(this.Year):
                    this.model.Year = (int)value;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.IsModified = true;

            this.NotifyPropertyChanged(propertyName);
        }

        #endregion
    }
}