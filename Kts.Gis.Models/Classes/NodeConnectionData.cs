using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет данные соединения с узлом.
    /// </summary>
    [Serializable]
    public sealed class NodeConnectionData
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NodeConnectionData"/>.
        /// </summary>
        /// <param name="connectedLineId">Идентификатор соединенной с узлом линии.</param>
        /// <param name="connectionSide">Сторона соединения линии с узлом.</param>
        public NodeConnectionData(Guid connectedLineId, NodeConnectionSide connectionSide)
        {
            this.ConnectedLineId = connectedLineId;
            this.ConnectionSide = connectionSide;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор соединенной с узлом линии.
        /// </summary>
        public Guid ConnectedLineId
        {
            get;
        }

        /// <summary>
        /// Возвращает сторону соединения линии с узлом.
        /// </summary>
        public NodeConnectionSide ConnectionSide
        {
            get;
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает строковое представление объекта.
        /// </summary>
        /// <returns>Строковое представление объекта.</returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.ConnectedLineId, (int)this.ConnectionSide);
        }

        #endregion
    }
}