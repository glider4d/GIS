using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет модель территориальной единицы.
    /// </summary>
    [Serializable()]
    public sealed class TerritorialEntityModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TerritorialEntityModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор территориальной единицы.</param>
        /// <param name="name">Название территориальной единицы.</param>
        public TerritorialEntityModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор территориальной единицы.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Возвращает название территориальной единицы.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}