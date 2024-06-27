namespace Kts.Gis.Models
{
    /// <summary>
    /// Тип документа.
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// Акт подключения.
        /// </summary>
        ConnectionAct = 1,

        /// <summary>
        /// Акт отключения.
        /// </summary>
        DeactivationAct = 2,
        
        /// <summary>
        /// Акт раздела границ.
        /// </summary>
        PartitionAct = 3
    }
}