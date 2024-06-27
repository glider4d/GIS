namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления объекта, являющегося чьим-то дочерним объектом.
    /// </summary>
    internal interface IChildObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает родительский объект.
        /// </summary>
        IObjectViewModel Parent
        {
            get;
        }

        #endregion
    }
}