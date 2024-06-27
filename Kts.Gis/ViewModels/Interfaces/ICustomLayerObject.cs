using Kts.Gis.Data;
using Kts.WpfUtilities;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс объекта пользовательского слоя.
    /// </summary>
    internal interface ICustomLayerObject : IEditableObjectViewModel, IMapObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает заголовок, идентифицирующий название объекта.
        /// </summary>
        string Caption
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает пользовательский слой, которому принадлежит объект.
        /// </summary>
        CustomLayerViewModel CustomLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду управления слоем.
        /// </summary>
        RelayCommand ManageLayerCommand
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        void BeginSave(IDataService dataService);

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        void EndSave();

        /// <summary>
        /// Выполняет поиск кастомного слоя, которому принадлежит объект.
        /// </summary>
        /// <param name="layers">Все кастомные слои.</param>
        void RestoreLayer(IEnumerable<CustomLayerViewModel> layers);

        /// <summary>
        /// Отменяет сохранение объекта в источнике данных.
        /// </summary>
        void RevertSave();

        /// <summary>
        /// Задает кастомный слой.
        /// </summary>
        /// <param name="customLayer">Кастомный слой.</param>
        void SetCustomLayer(CustomLayerViewModel customLayer);

        #endregion
    }
}