namespace Kts.Utilities
{
    /// <summary>
    /// Представляет модель представления территориальной единицы.
    /// </summary>
    public abstract class TerritorialEntityViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TerritorialEntityViewModel"/>.
        /// </summary>
        /// <param name="territorialEntity">Территориальная единица.</param>
        public TerritorialEntityViewModel(TerritorialEntityModel territorialEntity)
        {
            this.TerritorialEntity = territorialEntity;
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает территориальную единицу.
        /// </summary>
        protected TerritorialEntityModel TerritorialEntity
        {
            get;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор территориальной единицы.
        /// </summary>
        public int Id
        {
            get
            {
                return this.TerritorialEntity.Id;
            }
        }

        #endregion

        #region Открытые виртуальные свойства

        /// <summary>
        /// Возвращает название территориальной единицы.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.TerritorialEntity.Name;
            }
        }

        #endregion
    }
}