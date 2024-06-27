namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления сохраняемого объекта.
    /// </summary>
    internal interface ISavableObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли объект в источнике данных.
        /// </summary>
        bool IsSaved
        {
            get;
        }

        #endregion
    }
}