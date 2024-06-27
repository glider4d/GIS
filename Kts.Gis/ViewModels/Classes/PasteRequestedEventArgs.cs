using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса вставки объекта на карту.
    /// </summary>
    internal sealed class PasteRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PasteRequestedEventArgs"/>.
        /// </summary>
        /// <param name="mapObject">Объект карты, который необходимо вставить.</param>
        public PasteRequestedEventArgs(IMapObjectViewModel mapObject)
        {
            this.MapObject = mapObject;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает объект карты, который необходимо вставить.
        /// </summary>
        public IMapObjectViewModel MapObject
        {
            get;
        }

        #endregion
    }
}