namespace Kts.Gis.Models
{
    /// <summary>
    /// Уровень загрузки справочников параметра.
    /// </summary>
    public enum LoadLevel
    {
        /// <summary>
        /// Загружать справочники только после внесения изменений.
        /// </summary>
        AfterChange = 1,

        /// <summary>
        /// Всегда загружать справочники.
        /// </summary>
        Always = 2,

        /// <summary>
        /// Загружать справочники только один раз.
        /// </summary>
        Once = 0
    }
}