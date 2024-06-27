using Kts.Gis.Models;
using Kts.Utilities;
using System.Collections.Generic;
using System.Data;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления объекта, способного иметь дочерние объекты.
    /// </summary>
    internal interface IContainerObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает модели представлений добавления дочерних объектов.
        /// </summary>
        AdvancedObservableCollection<AddChildViewModel> AddChildViewModels
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что загружены ли дочерние объекты.
        /// </summary>
        bool AreChildrenLoaded
        {
            get;
        }
    
        /// <summary>
        /// Возвращает слои дочерних объектов.
        /// </summary>
        AdvancedObservableCollection<LayerViewModel> ChildrenLayers
        {
            get;
        }

        /// <summary>
        /// Возвращает типы дочерних объектов.
        /// </summary>
        List<ObjectType> ChildrenTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        bool HasChildren
        {
            get;
        }

        /// <summary>
        /// Возвращает модели представлений выбора дочерних объектов.
        /// </summary>
        AdvancedObservableCollection<SelectChildViewModel> SelectChildViewModels
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Добавляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        void AddChild(IObjectViewModel child);

        /// <summary>
        /// Добавляет новый дочерний объект.
        /// </summary>
        /// <param name="type">Тип дочернего объекта.</param>
        void AddChild(ObjectType type);

        /// <summary>
        /// Удаляет дочерний объект.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        void DeleteChild(IObjectViewModel child);

        /// <summary>
        /// Удаляет дочерний объект из источника данных.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        void FullDeleteChild(IObjectViewModel child);

        /// <summary>
        /// Возвращает список дочерних объектов.
        /// </summary>
        /// <returns>Список дочерних объектов.</returns>
        List<IObjectViewModel> GetChildren();

        /// <summary>
        /// Загружает дочерние объекты из источника данных.
        /// </summary>
        void LoadChildren();

        /// <summary>
        /// Загружает дочерние объекты из заданного набора данных.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        void LoadChildren(DataSet dataSet);

        #endregion
    }
}