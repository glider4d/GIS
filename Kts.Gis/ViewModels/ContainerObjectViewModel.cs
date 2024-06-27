using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления объекта, способного иметь дочерние объекты.
    /// </summary>
    [Serializable]
    internal sealed partial class ContainerObjectViewModel : BaseViewModel, IContainerObjectViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        //[NonSerialized]
        private readonly AccessService accessService;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        //[NonSerialized]
        private readonly IDataService dataService;
        private SqlDataService sqlDataService;

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

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        //[NonSerialized]
        private readonly IMessageService messageService;

        /// <summary>
        /// Объект.
        /// </summary>
        //[NonSerialized]
        private readonly IObjectViewModel obj;

        /// <summary>
        /// Модель объекта.
        /// </summary>
        //[NonSerialized]
        private readonly IObjectModel objModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ContainerObjectViewModel"/>.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="objModel">Модель объекта.</param>
        /// <param name="hasChildren">Значение, указывающее на то, что имеет ли объект дочерние объекты.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ContainerObjectViewModel(IObjectViewModel obj, IObjectModel objModel, bool hasChildren, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService)
        {
            this.obj = obj;
            this.objModel = objModel;
            this.layerHolder = layerHolder;
            this.accessService = accessService;
            this.dataService = dataService;
            this.historyService = historyService;
            this.mapBindingService = mapBindingService;
            this.messageService = messageService;

            if (hasChildren)
            {
                // Если объект имеет дочерние объекты, то нужно создать искусственный дочерний объект, чтобы представление могло отображать наличие дочерних объектов.
                var childType = this.obj.Type.Children[0];
                this.AddChildrenLayer(childType);
                if (childType.ObjectKind == ObjectKind.Badge)
                    this.ChildrenLayers.First(x => x.Type == childType).Add(new BadgeViewModel(new BadgeModel(ObjectModel.DefaultId, this.obj.Id, this.obj.CityId, childType, this.obj.IsPlanning, 0, this.obj.IsActive), this.obj, this.layerHolder, accessService, dataService, historyService, mapBindingService, messageService));
                else
                    this.ChildrenLayers.First(x => x.Type == childType).Add(new NonVisualObjectViewModel(new NonVisualObjectModel(ObjectModel.DefaultId, this.obj.Id, this.obj.CityId, childType, this.obj.IsPlanning, "", this.obj.IsActive, false), this.obj, true, this.layerHolder, accessService, dataService, historyService, mapBindingService, messageService));
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор объекта.
        /// </summary>
        public Guid ObjectId
        {
            get
            {
                return this.obj.Id;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Очищает слои дочерних объектов.
        /// </summary>
        private void ClearChildrenLayers()
        {
            this.ChildrenLayers.Clear();
        }

        /// <summary>
        /// Загружает дочерние объекты.
        /// </summary>
        /// <param name="badges">Список значков.</param>
        /// <param name="nonVisualObject">Список невизуальных объектов.</param>
        private void LoadChildren(List<BadgeModel> badges, List<NonVisualObjectModel> nonVisualObjects)
        {
            if (badges.Count > 0 || nonVisualObjects.Count > 0)
                this.ClearChildrenLayers();

            // Создаем временные хранилища для дочерних объектов, так как нам надо будет отсортировать их.
            var temp = new Dictionary<ObjectType, List<IObjectViewModel>>();

            foreach (var badge in badges)
            {
                if (!temp.ContainsKey(badge.Type))
                    temp.Add(badge.Type, new List<IObjectViewModel>());

                temp[badge.Type].Add(new BadgeViewModel(badge, this.obj, this.layerHolder, this.accessService, this.dataService, this.historyService, this.mapBindingService, this.messageService));
            }

            foreach (var nonVisualObject in nonVisualObjects)
            {
                if (!temp.ContainsKey(nonVisualObject.Type))
                    temp.Add(nonVisualObject.Type, new List<IObjectViewModel>());

                temp[nonVisualObject.Type].Add(new NonVisualObjectViewModel(nonVisualObject, this.obj, false, this.layerHolder, this.accessService, this.dataService, this.historyService, this.mapBindingService, this.messageService));
            }

            foreach (var entry in temp)
            {
                if (!this.HasChildrenLayer(entry.Key))
                    this.AddChildrenLayer(entry.Key);

                var layer = this.ChildrenLayers.First(x => x.Type == entry.Key);

                foreach (var obj in entry.Value.OrderBy(x => (x as INamedObjectViewModel).Name))
                    layer.Add(obj);
            }

            this.AreChildrenLoaded = true;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет слой дочерних объектов заданного типа.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        public void AddChildrenLayer(ObjectType type)
        {
            var layer = new LayerViewModel(type, false, false, this.layerHolder, this.accessService, this.dataService, this.historyService, this.mapBindingService, this.messageService);

            this.ChildrenLayers.Add(layer);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеется ли слой дочерних объектов с заданным типом.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <returns>Значение, указывающее на то, что имеется ли слой дочерних объектов с заданным типом.</returns>
        public bool HasChildrenLayer(ObjectType type)
        {
            return this.ChildrenLayers.Any(x => x.Type == type);
        }

        /// <summary>
        /// Удаляет слой дочерних объектов заданного типа.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        public void RemoveChildrenLayer(ObjectType type)
        {
            var layer = this.ChildrenLayers.First(x => x.Type == type);

            this.ChildrenLayers.Remove(layer);
        }

        #endregion
    }

    // Реализация IContainerObjectViewModel.
    internal sealed partial class ContainerObjectViewModel
    {
        #region Открытые свойства
    
        /// <summary>
        /// Возвращает модели представлений добавления дочерних объектов.
        /// </summary>
        public AdvancedObservableCollection<AddChildViewModel> AddChildViewModels
        {
            get
            {
                return m_addChildViewModels;
            }
        }// = new AdvancedObservableCollection<AddChildViewModel>();
        //[NonSerialized]
        private AdvancedObservableCollection<AddChildViewModel> m_addChildViewModels = new AdvancedObservableCollection<AddChildViewModel>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что загружены ли дочерние объекты.
        /// </summary>
        public bool AreChildrenLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает слои дочерних объектов.
        /// </summary>
        public AdvancedObservableCollection<LayerViewModel> ChildrenLayers
        {
            get
            {
                return m_ChildrenLayers;
            }
        }// = new AdvancedObservableCollection<LayerViewModel>();
        
        AdvancedObservableCollection<LayerViewModel> m_ChildrenLayers = new AdvancedObservableCollection<LayerViewModel>();

        /// <summary>
        /// Возвращает типы дочерних объектов.
        /// </summary>
        public List<ObjectType> ChildrenTypes
        {
            get
            {
                return (this.obj as IContainerObjectViewModel).ChildrenTypes;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return (this.obj as IContainerObjectViewModel).HasChildren;
            }
        }

        /// <summary>
        /// Возвращает модели представлений выбора дочерних объектов.
        /// </summary>
        public AdvancedObservableCollection<SelectChildViewModel> SelectChildViewModels
        {
            get
            {
                return m_selectChildViewModels;
            }
        }// = new AdvancedObservableCollection<SelectChildViewModel>();

        //[NonSerialized]
        private AdvancedObservableCollection<SelectChildViewModel> m_selectChildViewModels = new AdvancedObservableCollection<SelectChildViewModel>();

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void AddChild(IObjectViewModel child)
        {
            if (this.obj.IsInitialized && !this.AreChildrenLoaded)
                // Если дочерние объекты еще не были загружены, то делаем это.
                this.LoadChildren();

            // Запоминаем действие в истории изменений и выполняем его.
            var action = new AddRemoveChildAction(this, child, true);
            this.historyService.Add(new HistoryEntry(action, Target.Data, "добавление дочернего объекта"));
            action.Do();
        }

        /// <summary>
        /// Добавляет новый дочерний объект.
        /// </summary>
        /// <param name="type">Тип дочернего объекта.</param>
        public void AddChild(ObjectType type)
        {
            var child = new NonVisualObjectModel(ObjectModel.DefaultId, this.obj.Id, this.obj.CityId, type, this.obj.IsPlanning, "", this.obj.IsActive, false);

            this.AddChild(new NonVisualObjectViewModel(child, this.obj, false, this.layerHolder, this.accessService, this.dataService, this.historyService, this.mapBindingService, this.messageService));
        }

        /// <summary>
        /// Удаляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void DeleteChild(IObjectViewModel child)
        {
            // Запоминаем действие в истории изменений и выполняем его.
            var action = new AddRemoveChildAction(this, child, false);
            this.historyService.Add(new HistoryEntry(action, Target.Data, "удаление дочернего объекта"));
            action.Do();
        }

        /// <summary>
        /// Удаляет дочерний объект из источника данных.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public void FullDeleteChild(IObjectViewModel child)
        {
            // Удаляем объект простым удалением.
            var action = new AddRemoveChildAction(this, child, false);
            action.Do();

            var badge = child as BadgeViewModel;

            if (badge != null)
                badge.FullDelete();

            var nonVisualObject = child as NonVisualObjectViewModel;

            if (nonVisualObject != null)
                nonVisualObject.FullDelete();
        }

        /// <summary>
        /// Возвращает список дочерних объектов.
        /// </summary>
        /// <returns>Список дочерних объектов.</returns>
        public List<IObjectViewModel> GetChildren()
        {
            var result = new List<IObjectViewModel>();

            foreach (var layer in this.ChildrenLayers)
                result.AddRange(layer.Objects);

            return result;
        }

        /// <summary>
        /// Загружает дочерние объекты из источника данных.
        /// </summary>
        public void LoadChildren()
        {
            var badges = this.dataService.BadgeAccessService.GetAll(this.objModel, this.layerHolder.CurrentSchema);
            var nonVisualObjects = this.dataService.NonVisualObjectAccessService.GetAll(this.objModel, this.layerHolder.CurrentSchema);

            this.LoadChildren(badges, nonVisualObjects);
        }

        /// <summary>
        /// Загружает дочерние объекты из заданного набора данных.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        public void LoadChildren(DataSet dataSet)
        {
            var tmpDataService = this.dataService;
            if (dataService is WcfDataService )
            {
                if (sqlDataService == null)
                {
                    //sqlDataService = new SqlDataService(null, dataService.ErrorFolderName, dataService.SubstrateFolderName, dataService.ThumbnailFolderName);
                    sqlDataService = new SqlDataService(dataService.ObjectTypes);
                    sqlDataService.BadgeAccessService = new SqlBadgeAccessService(sqlDataService, null, this.dataService.LoggedUserId);
                    sqlDataService.NonVisualObjectAccessService = new SqlNonVisualObjectAccessService(sqlDataService, null, this.dataService.LoggedUserId);
                }
                tmpDataService = sqlDataService;
            }

            var badges = tmpDataService.BadgeAccessService.GetAll(dataSet, this.objModel);
            var nonVisualObjects = tmpDataService.NonVisualObjectAccessService.GetAll(dataSet, this.objModel);

            this.LoadChildren(badges, nonVisualObjects);
        }

        #endregion
    }
}