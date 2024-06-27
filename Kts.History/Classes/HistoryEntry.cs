namespace Kts.History
{
    /// <summary>
    /// Представляет запись в истории изменений.
    /// </summary>
    public sealed class HistoryEntry
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HistoryEntry"/>.
        /// </summary>
        /// <param name="action">Возвратимое действие.</param>
        /// <param name="target">Цель действия.</param>
        /// <param name="description">Описание действия.</param>
        public HistoryEntry(IRevertibleAction action, Target target, string description)
        {
            this.Action = action;
            this.Target = target;
            this.Description = description;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает возвратимое действие.
        /// </summary>
        public IRevertibleAction Action
        {
            get;
        }

        /// <summary>
        /// Возвращает описание действия.
        /// </summary>
        public string Description
        {
            get;
        }
        
        /// <summary>
        /// Возвращает цель действия.
        /// </summary>
        public Target Target
        {
            get;
        }

        #endregion
    }
}