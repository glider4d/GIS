namespace Kts.Utilities
{
    /// <summary>
    /// Глубина выбора территориальной единицы.
    /// </summary>
    public enum SelectionDepth
    {
        /// <summary>
        /// Выбор до населенного пункта.
        /// </summary>
        City = 2,

        /// <summary>
        /// Выбор до региона.
        /// </summary>
        Region = 1,

        /// <summary>
        /// Выбор до улицы.
        /// </summary>
        Street = 3
    }
}