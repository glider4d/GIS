namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерфейс модели представления редактируемого объекта.
    /// </summary>
    internal interface IEditableObject
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        bool IsEditing
        {
            get;
            set;
        }

        #endregion
    }
}