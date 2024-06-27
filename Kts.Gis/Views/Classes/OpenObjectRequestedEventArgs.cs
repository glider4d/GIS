using System;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события запроса открытия объекта.
    /// </summary>
    internal sealed class OpenObjectRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OpenObjectRequestedEventArgs"/>.
        /// </summary>
        /// <param name="objectId">Идентификатор объекта, который нужно открыть.</param>
        public OpenObjectRequestedEventArgs(Guid objectId)
        {
            this.ObjectId = objectId;
        }

        #endregion

        #region Открытые события

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что найден ли объект.
        /// </summary>
        public bool IsFound
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает идентификатор объекта, который нужно открыть.
        /// </summary>
        public Guid ObjectId
        {
            get;
        }

        #endregion
    }
}