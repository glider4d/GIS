namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления выделяемого объекта.
    /// </summary>
    internal interface IHighlightableObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Задает значение, указывающее на то, что выделен ли объект.
        /// </summary>
        bool IsHighlighted
        {
            set;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Убирает выделение объекта.
        /// </summary>
        void HighlightOff();

        /// <summary>
        /// Выделяет объект.
        /// </summary>
        void HighlightOn();

        /// <summary>
        /// Сбрасывает выделение объекта.
        /// </summary>
        void ResetHighlight();

        #endregion
    }
}