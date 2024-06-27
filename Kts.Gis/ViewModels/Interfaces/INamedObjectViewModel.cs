namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления именованного объекта.
    /// </summary>
    internal interface INamedObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает необработанное название объекта, полученное из источника данных.
        /// </summary>
        string RawName
        {
            get;
        }

        #endregion
    }
}