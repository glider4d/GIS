using System;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления котельной.
    /// </summary>
    public sealed class BoilerViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BoilerViewModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор котельной.</param>
        /// <param name="name">Название котельной.</param>
        public BoilerViewModel(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор котельной.
        /// </summary>
        public Guid Id
        {
            get;
        }

        /// <summary>
        /// Возвращает название котельной.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}