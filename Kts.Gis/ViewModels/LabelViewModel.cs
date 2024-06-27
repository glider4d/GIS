using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления надписи.
    /// </summary>
    [Serializable]
    internal sealed partial class LabelViewModel : Utilities.BaseViewModel, ICustomLayerObject, IDeletableObjectViewModel, ISavableObjectViewModel, ISetterIgnorer
    {
        #region Закрытые поля

        /// <summary>
        /// Кастомный слой, которому принадлежит надпись.
        /// </summary>
        private CustomLayerViewModel customLayer;

        /// <summary>
        /// Значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        private bool isEditing;

        /// <summary>
        /// Значение, указывающее на то, что изменен ли идентификатор надписи.
        /// </summary>
        private bool isIdChanged;

        /// <summary>
        /// Значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        private bool isPlaced;

        /// <summary>
        /// Значение, указывающее на то, что начато ли сохранение объекта.
        /// </summary>
        private bool isSaveStarted;

        /// <summary>
        /// Значение, указывающее на то, что была ли уже размещена надпись.
        /// </summary>
        private bool wasPlaced;

        #endregion

        #region Закрытые неизменяемые поля

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
        /// Надпись.
        /// </summary>
        private readonly LabelModel label;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelViewModel"/>.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public LabelViewModel(LabelModel label, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService)
        {
            this.label = label;
            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.dataService = dataService;
            this.historyService = historyService;
            this.mapBindingService = mapBindingService;

            if (!label.IsSaved)
                this.IsModified = true;

            this.RegisterBinding();

            this.WasPlaced = label.IsSaved;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает угол поворота надписи.
        /// </summary>
        public double Angle
        {
            get
            {
                return this.label.Angle;
            }
            set
            {
                if (this.Angle != value)
                {
                    // При изменении размера надписи необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение размера меняется очень часто.
                    var exists = false;
                    var entry = this.historyService.GetCurrentEntry();
                    if (entry != null)
                    {
                        var action = entry.Action as SetPropertyAction;

                        if (action != null && action.Object == this && action.PropertyName == nameof(this.Angle))
                        {
                            action.NewValue = value;

                            action.Do();

                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new SetPropertyAction(this, nameof(this.Angle), this.Angle, value);
                        this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение угла поворота надписи"));
                        action.Do();
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает содержимое надписи.
        /// </summary>
        public string Content
        {
            get
            {
                return this.label.Content;
            }
        }

        /// <summary>
        /// Возвращает идентификатор надписи.
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.label.Id;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи полужирным.
        /// </summary>
        public bool IsBold
        {
            get
            {
                return this.label.IsBold;
            }
            set
            {
                this.label.IsBold = value;

                this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsBold), value);

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи курсивным.
        /// </summary>
        public bool IsItalic
        {
            get
            {
                return this.label.IsItalic;
            }
            set
            {
                this.label.IsItalic = value;

                this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsItalic), value);

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи подчеркнутым.
        /// </summary>
        public bool IsUnderline
        {
            get
            {
                return this.label.IsUnderline;
            }
            set
            {
                this.label.IsUnderline = value;

                this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsUnderline), value);

                this.IsModified = true;
            }
        }

        /// <summary>
        /// Возвращает или задает положение надписи.
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(this.label.Position.X, this.label.Position.Y);
            }
            set
            {
                if (this.Position != value)
                {
                    // Запоминаем действие в истории изменений и выполняем его.
                    var action = new SetPropertyAction(this, nameof(this.Position), this.Position, value);
                    this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение положения надписи"));
                    action.Do();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает размер надписи.
        /// </summary>
        public int Size
        {
            get
            {
                return this.label.Size;
            }
            set
            {
                if (this.Size != value)
                {
                    // При изменении размера надписи необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как значение размера меняется очень часто.
                    var exists = false;
                    var entry = this.historyService.GetCurrentEntry();
                    if (entry != null)
                    {
                        var action = entry.Action as SetPropertyAction;

                        if (action != null && action.Object == this && action.PropertyName == nameof(this.Size))
                        {
                            action.NewValue = value;

                            action.Do();

                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        // Запоминаем действие в истории изменений и выполняем его.
                        var action = new SetPropertyAction(this, nameof(this.Size), this.Size, value);
                        this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение размера надписи"));
                        action.Do();
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что была ли уже размещена надпись.
        /// </summary>
        public bool WasPlaced
        {
            get
            {
                return this.wasPlaced;
            }
            set
            {
                this.wasPlaced = value;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет изменение стиля надписи.
        /// </summary>
        /// <param name="newContent">Новое содержимое.</param>
        /// <param name="newSize">Новый размер.</param>
        /// <param name="isBold">Значение, указывающее на то, что является ли шрифт надписи полужирным.</param>
        /// <param name="isItalic">Значение, указывающее на то, что является ли шрифт надписи курсивным.</param>
        /// <param name="isUnderline">Значение, указывающее на то, что является ли шрифт надписи подчеркнутым.</param>
        /// <param name="alignHorizontal">Значение, указывающее на то, что должна ли быть надпись выровнена по горизонтали.</param>
        /// <param name="angle">Угол поворота.</param>
        public void ChangeStyle(string newContent, int newSize, bool isBold, bool isItalic, bool isUnderline, bool alignHorizontal, double angle)
        {
            // Запоминаем действие в истории изменений и выполняем его.
            var action = new ChangeLabelStyleAction(this, newContent, newSize, isBold, isItalic, isUnderline, alignHorizontal ? 0 : angle);
            this.historyService.Add(new HistoryEntry(action, Target.Data, "редактирование надписи"));
            action.Do();
        }

        #endregion
    }

    // Реализация ICustomLayerObject.
    internal sealed partial class LabelViewModel
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
                return this.Content;
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
                this.label.LayerId = this.CustomLayer == null ? Guid.Empty : this.CustomLayer.Id;

                if (this.IsSaved)
                    dataService.LabelAccessService.UpdateObject(this.label, this.layerHolder.CurrentSchema);
                else
                {
                    this.label.Id = dataService.LabelAccessService.UpdateNewObject(this.label, this.layerHolder.CurrentSchema);

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
            if (this.label.LayerId == Guid.Empty)
            {
                this.CustomLayer = null;

                return;
            }

            var layer = layers.FirstOrDefault(x => x.Id == this.label.LayerId);

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
                this.label.Id = Guid.Empty;
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
        /// Задает кастомный слой.
        /// </summary>
        /// <param name="customLayer">Кастомный слой.</param>
        public void SetCustomLayer(CustomLayerViewModel customLayer)
        {
            var action = new ChangeCustomLayerAction(this, this.CustomLayer, customLayer);

            this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение пользовательского слоя"));

            action.Do();

            this.IsModified = true;
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

    // Реализация IDeletableObjectViewModel.
    internal sealed partial class LabelViewModel
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
            this.dataService.LabelAccessService.DeleteObject(this.label, this.layerHolder.CurrentSchema);
        }

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        public void MarkFullDelete()
        {
            this.dataService.LabelAccessService.MarkDeleteObject(this.label, this.layerHolder.CurrentSchema);
        }

        #endregion
    }
    
    // Реализация ISavableObjectViewModel.
    internal sealed partial class LabelViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранена ли надпись.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.label.IsSaved;
            }
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class LabelViewModel
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
                case nameof(this.Angle):
                    this.label.Angle = (double)value;

                    break;

                case nameof(this.Content):
                    this.label.Content = (string)value;

                    this.NotifyPropertyChanged(nameof(this.Content));

                    break;

                case nameof(this.Position):
                    var point = (Point)value;

                    this.label.Position = new Utilities.Point(point.X, point.Y);

                    break;

                case nameof(this.Size):
                    this.label.Size = (int)value;
                    
                    break;

                default:
                    throw new NotImplementedException("Не реализовано задание значения свойства " + propertyName);
            }

            this.IsModified = true;

            this.mapBindingService.SetMapObjectViewValue(this, propertyName, value);
        }

        #endregion
    }
}