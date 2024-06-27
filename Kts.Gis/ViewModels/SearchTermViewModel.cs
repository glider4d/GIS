using Kts.Gis.Models;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления условия поиска.
    /// </summary>
    internal sealed class SearchTermViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchTermViewModel"/>.
        /// </summary>
        /// <param name="parameter">Параметр, по которому проверяется условие поиска.</param>
        /// <param name="term">Условие поиска.</param>
        /// <param name="termName">Название условия поиска.</param>
        /// <param name="value">Значение условия.</param>
        /// <param name="innerValue">Внутреннее значение условия.</param>
        public SearchTermViewModel(SearchingParameterViewModel parameter, Operator term, string termName, string value, string innerValue)
        {
            this.Parameter = parameter;
            this.Term = term;
            this.TermName = termName;
            this.Value = value;
            this.InnerValue = innerValue;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает внутреннее значение условия.
        /// </summary>
        public string InnerValue
        {
            get;
        }

        /// <summary>
        /// Возвращает параметр, по которому проверяется условие поиска.
        /// </summary>
        public SearchingParameterViewModel Parameter
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
        /// Возвращает название условия поиска.
        /// </summary>
        public string TermName
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