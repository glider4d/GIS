using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель топлива.
    /// </summary>
    [Serializable]
    public sealed class FuelModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FuelModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор топлива.</param>
        /// <param name="name">Название топлива.</param>
        public FuelModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор топлива.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Возвращает название топлива.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}