namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления объекта, который может быть скопирован и вставлен.
    /// </summary>
    internal interface ICopyableObjectViewModel
    {
        #region Методы

        /// <summary>
        /// Возвращает копию объекта.
        /// </summary>
        /// <returns>Копия объекта.</returns>
        ICopyableObjectViewModel Copy();

        #endregion
    }
}