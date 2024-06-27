namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель условия поиска.
    /// </summary>
    public sealed class SearchTermModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchTermModel"/>.
        /// </summary>
        /// <param name="parameter">Параметр, по которому проверяется условие поиска.</param>
        /// <param name="term">Условие поиска.</param>
        /// <param name="value">Значение условия.</param>
        public SearchTermModel(ParameterModel parameter, Operator term, string value)
        {
            this.Parameter = parameter;
            this.Term = term;
            this.Value = value;
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает параметр, по которому проверяется условие поиска.
        /// </summary>
        public ParameterModel Parameter
        {
            get;
        }

        /// <summary>
        /// Возвращает условие поиска.
        /// </summary>
        public Operator Term
        {
            get;
        }

        /// <summary>
        /// Возвращает значение условия.
        /// </summary>
        public string Value
        {
            get;
        }

        #endregion
    }
}