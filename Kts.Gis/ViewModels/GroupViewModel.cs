using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Utilities;
using System.ComponentModel;
using System.Linq;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления группы слоев.
    /// </summary>
    [Serializable]
    internal sealed partial class GroupViewModel : ServicedViewModel, IHighlightableObjectViewModel, ISelectableObjectViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Полное название группы слоев.
        /// </summary>
        private string fullName;

        /// <summary>
        /// Значение, указывающее на то, что выбрана ли группа слоев.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Количество объектов группы.
        /// </summary>
        private int objectCount = 0;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что имеют ли слои группы общие визуальные слои.
        /// </summary>
        private readonly bool hasSharedVisualLayers;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Название группы слоев.
        /// </summary>
        private readonly string name;

        #endregion
        
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupViewModel"/>.
        /// </summary>
        /// <param name="name">Название группы слоев.</param>
        /// <param name="hasSharedVisualLayers">Значение, указывающее на то, что имеют ли слои группы общие визуальные слои.</param>
        /// <param name="hasOwnVisualLayers">Значение, указывающее на то, что имеют ли слои группы свои собственные визуальные слои.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public GroupViewModel(string name, bool hasSharedVisualLayers, bool hasOwnVisualLayers, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(dataService, messageService)
        {
            this.name = name;
            this.hasSharedVisualLayers = hasSharedVisualLayers;
            this.layerHolder = layerHolder;
            this.mapBindingService = mapBindingService;

            foreach (var type in dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Figure || x.ObjectKind == ObjectKind.Line || x.ObjectKind == ObjectKind.Node))
                this.Layers.Add(new LayerViewModel(type, hasSharedVisualLayers, hasOwnVisualLayers, layerHolder, accessService, dataService, historyService, mapBindingService, messageService));

            // Подписываемся на изменения свойств слоев группы, для обновления полного названия и количества объектов.
            foreach (var layer in this.Layers)
                layer.PropertyChanged += this.Layer_PropertyChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает полное название группы слоев.
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
        /// Возвращает слои группы.
        /// </summary>
        public AdvancedObservableCollection<LayerViewModel> Layers
        {
            get;
        } = new AdvancedObservableCollection<LayerViewModel>();

        /// <summary>
        /// Возвращает или задает количество объектов в слоях группы.
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

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> слоя группы.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Layer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LayerViewModel.ObjectCount):
                    // Суммируем количества объектов в слоях группы.
                    int i = 0;
                    foreach (var layer in this.Layers)
                        i += layer.ObjectCount;

                    this.ObjectCount = i;

                    break;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Обновляет полное название группы слоев.
        /// </summary>
        private void UpdateFullName()
        {
            this.FullName = this.name + " (" + this.ObjectCount.ToString() + ")";
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Очищает данные слоев.
        /// </summary>
        public void ClearLayerData()
        {
            this.IsSelected = false;

            foreach (var layer in this.Layers)
                layer.Clear();
        }

        /// <summary>
        /// Возвращает true, если группа слоев содержит заданный объект. Иначе - false.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Значение, указывающее на то, что содержит ли группа слоев заданный объект.</returns>
        public bool Contains(IObjectViewModel obj)
        {
            foreach (var layer in this.Layers)
                if (layer.Contains(obj))
                    return true;

            return false;
        }

        #endregion
    }

    // Реализация IHighlightableObjectViewModel.
    internal sealed partial class GroupViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Задает значение, указывающее на то, что выделен ли объект.
        /// </summary>
        public bool IsHighlighted
        {
            set
            {
                if (!this.hasSharedVisualLayers)
                    return;

                this.mapBindingService.SetGroupViewValue(this, nameof(this.IsHighlighted), value);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Убирает выделение объекта.
        /// </summary>
        public void HighlightOff()
        {
            if (!this.hasSharedVisualLayers)
                return;

            foreach (var layer in this.Layers)
                layer.HighlightOff();
        }

        /// <summary>
        /// Выделяет объект.
        /// </summary>
        public void HighlightOn()
        {
            if (!this.hasSharedVisualLayers)
                return;

            foreach (var layer in this.Layers)
                layer.HighlightOn();
        }

        /// <summary>
        /// Сбрасывает выделение объекта.
        /// </summary>
        public void ResetHighlight()
        {
            if (!this.hasSharedVisualLayers)
                return;

            foreach (var layer in this.Layers)
                layer.ResetHighlight();
        }

        #endregion
    }

    // Реализация ISelectableObjectViewModel.
    internal sealed partial class GroupViewModel
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

                this.layerHolder.SelectedGroup = value ? this : null;
            }
        }

        #endregion
    }
}