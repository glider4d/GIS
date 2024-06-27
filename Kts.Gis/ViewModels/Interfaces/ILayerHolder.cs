using Kts.Gis.Models;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс хранителя слоев.
    /// </summary>
    internal interface ILayerHolder
    {
        #region Свойства

        /// <summary>
        /// Возвращает команду копирования объекта.
        /// </summary>
        RelayCommand CopyCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает текущий населенный пункт.
        /// </summary>
        CityViewModel CurrentCity
        {
            get;
        }

        /// <summary>
        /// Возвращает текущую схему.
        /// </summary>
        SchemaModel CurrentSchema
        {
            get;
        }

        /// <summary>
        /// Возвращает команду удаления объекта.
        /// </summary>
        RelayCommand DeleteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает редактируемый объект.
        /// </summary>
        IEditableObjectViewModel EditingObject
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду полного удаления объекта.
        /// </summary>
        RelayCommand FullDeleteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду управления слоем кастомного объекта.
        /// </summary>
        RelayCommand ManageCustomObjectLayerCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает группу узлов.
        /// </summary>
        GroupViewModel NodesGroup
        {
            get;
        }
        
        /// <summary>
        /// Возвращает или задает выбранную группу.
        /// </summary>
        ISelectableObjectViewModel SelectedGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный слой.
        /// </summary>
        ISelectableObjectViewModel SelectedLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный неразмещенный объект.
        /// </summary>
        ISelectableObjectViewModel SelectedNotPlacedObject
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        ISelectableObjectViewModel SelectedObject
        {
            get;
            set;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Создает объект и добавляет его на карту и соответствующий ему слой.
        /// </summary>
        /// <param name="obj">Объект.</param>
        void AddObject(IObjectViewModel obj);

        /// <summary>
        /// Очищает группу выбранных объектов.
        /// </summary>
        void ClearSelectedGroup();

        /// <summary>
        /// Возвращает или задает слой объектов.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="layerType">Тип слоя объектов.</param>
        /// <returns>Слой объектов.</returns>
        LayerViewModel GetLayer(ObjectType type, LayerType layerType);

        /// <summary>
        /// Возвращает слои по виду объектов.
        /// </summary>
        /// <param name="kind">Вид объектов.</param>
        /// <returns>Слои.</returns>
        List<LayerViewModel> GetLayers(ObjectKind kind);

        /// <summary>
        /// Возвращает объект по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект.</returns>
        IObjectViewModel GetObject(Guid id);

        /// <summary>
        /// Возвращает выбранные объекты верхнего уровня.
        /// </summary>
        /// <returns>Список объектов.</returns>
        List<IObjectViewModel> GetSelectedObjects();

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, подлежащий удалению.</param>
        void MarkToDelete(IDeletableObjectViewModel obj);

        /// <summary>
        /// Помечает объект на раннее обновление в источнике данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно обновить раньше остальных.</param>
        void MarkToUpdate(IEditableObjectViewModel obj);

        /// <summary>
        /// Убирает объект с карты и с соответствующего ему слоя.
        /// </summary>
        /// <param name="obj">Объект.</param>
        void RemoveObject(IObjectViewModel obj);

        /// <summary>
        /// Задает выбранные объекты.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        void SetSelectedObjects(List<IObjectViewModel> objects);

        /// <summary>
        /// Убирает пометку с объекта, подлежащего к удалению из источника данных.
        /// </summary>
        /// <param name="obj">Объект, подлежащий удалению.</param>
        void UnmarkToDelete(IDeletableObjectViewModel obj);

        /// <summary>
        /// Убирает отметку с объекта, подлежащего к раннему обновлению в источнике данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно обновить раньше остальных.</param>
        void UnmarkToUpdate(IEditableObjectViewModel obj);

        #endregion
    }
}