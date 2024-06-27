namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления выбираемого объекта.
    /// </summary>
    internal interface ISelectableObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли объект.
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        #endregion
    }
}