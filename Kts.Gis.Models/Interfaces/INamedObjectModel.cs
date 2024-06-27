namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет интерфейс модели именованного объекта.
    /// </summary>
    public interface INamedObjectModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает название объекта.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        #endregion
    }
}