using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргументы события изменения видимости кастомного слоя.
    /// </summary>
    internal sealed class CustomLayerVisibilityChangedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomLayerVisibilityChangedEventArgs"/>.
        /// </summary>
        /// <param name="layer">Кастомный слой, у которого изменилась видимость.</param>
        public CustomLayerVisibilityChangedEventArgs(CustomLayerViewModel layer)
        {
            this.Layer = layer;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что виден ли кастомный слой.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.Layer.IsVisible;
            }
        }
        
        /// <summary>
        /// Возвращает кастомный слой, у которого изменилась видимость.
        /// </summary>
        public CustomLayerViewModel Layer
        {
            get;
        }

        #endregion
    }
}