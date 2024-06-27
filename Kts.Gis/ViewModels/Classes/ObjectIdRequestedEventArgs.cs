using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса идентификатора объекта.
    /// </summary>
    internal sealed class ObjectIdRequestedEventArgs : EventArgs
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает идентификатор объекта.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        #endregion
    }
}