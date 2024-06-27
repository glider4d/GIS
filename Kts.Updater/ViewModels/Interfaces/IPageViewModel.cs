namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления страницы.
    /// </summary>
    internal interface IPageViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выделена ли страница.
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        #endregion
    }
}