using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргументы события управления слоем кастомного объекта.
    /// </summary>
    internal sealed class ManageCustomObjectLayerEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ManageCustomObjectLayerEventArgs"/>.
        /// </summary>
        /// <param name="obj">Кастомный объект.</param>
        /// <param name="layers">Кастомные слои.</param>
        public ManageCustomObjectLayerEventArgs(ICustomLayerObject obj, List<CustomLayerViewModel> layers)
        {
            this.CustomLayerObject = obj;
            this.Layers = layers;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает кастомный объект.
        /// </summary>
        public ICustomLayerObject CustomLayerObject
        {
            get;
        }

        /// <summary>
        /// Возвращает кастомные слои.
        /// </summary>
        public List<CustomLayerViewModel> Layers
        {
            get;
        }

        #endregion
    }
}