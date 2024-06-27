namespace Kts.Gis.Models
{
    /// <summary>
    /// Вид ограничения доступа к функциям приложения.
    /// </summary>
    public enum AccessKind
    {
        /// <summary>
        /// Ограничение по возможности рисования в фактической схеме.
        /// </summary>
        CanDraw,

        /// <summary>
        /// Ограничение по возможности рисования в инвестиционной схеме.
        /// </summary>
        CanDrawIS,
        
        /// <summary>
        /// Ограничение по доступности администраторских прав.
        /// </summary>
        IsAdmin,

        /// <summary>
        /// Ограничение по возможности открытия регионов.
        /// </summary>
        PermittedRegions,

        /// <summary>
        /// Ограничение по возможности редактирования параметров объектов.
        /// </summary>
        PermittedTypes
    }
}