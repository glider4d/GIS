

using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель склада топлива.
    /// </summary>
    [Serializable]
    public sealed class FuelStorageModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FuelStorageModel"/>.
        /// </summary>
        /// <param name="name">Название склада топлива.</param>
        /// <param name="balance">Баланс склада топлива.</param>
        public FuelStorageModel(string name, double balance)
        {
            this.Name = name;
            this.Balance = balance;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает баланс склада топлива.
        /// </summary>
        public double Balance
        {
            get;
        }

        /// <summary>
        /// Возвращает название склада топлива.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}