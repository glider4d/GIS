using Kts.Gis.Models;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет соединение с узлом.
    /// </summary>
    internal sealed class NodeConnection
    {
        #region Конструкторы
    
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NodeConnection"/>.
        /// </summary>
        /// <param name="line">Линия, которая присоединена к узлу.</param>
        /// <param name="connectionSide">Сторона соединения линии с узлом.</param>
        public NodeConnection(LineViewModel line, NodeConnectionSide connectionSide)
        {
            this.Line = line;
            this.ConnectionSide = connectionSide;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает линию, которая присоединена к узлу.
        /// </summary>
        public LineViewModel Line
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
    }
}