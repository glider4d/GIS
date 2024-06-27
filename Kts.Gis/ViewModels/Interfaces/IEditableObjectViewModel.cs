namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления редактируемого объекта.
    /// </summary>
    internal interface IEditableObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Вовзращает значение, указывающее на то, что может ли объект редактироваться.
        /// </summary>
        bool CanBeEdited
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        bool IsEditing
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что изменен ли объект.
        /// </summary>
        bool IsModified
        {
            get;
            set;
        }

        #endregion
    }
}