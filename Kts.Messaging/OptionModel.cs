namespace Kts.Messaging
{
    /// <summary>
    /// Представляет модель опции.
    /// </summary>
    public sealed class OptionModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OptionModel"/>.
        /// </summary>
        /// <param name="name">Название опции.</param>
        /// <param name="isSelected">Значение, указывающее на то, что выбрана ли опция.</param>
        public OptionModel(string name, bool isSelected)
        {
            this.Name = name;
            this.IsSelected = isSelected;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбрана ли опция.
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает название опции.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}