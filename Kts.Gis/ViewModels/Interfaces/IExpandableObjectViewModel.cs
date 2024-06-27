namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления раскрываемого объекта.
    /// </summary>
    internal interface IExpandableObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что раскрыт ли объект.
        /// </summary>
        bool IsExpanded
        {
            get;
            set;
        }

        #endregion
    }
}