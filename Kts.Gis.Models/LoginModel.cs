using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель логина.
    /// </summary>
    [Serializable]
    public sealed class LoginModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LoginModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор логина.</param>
        /// <param name="name">Название логина.</param>
        public LoginModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор логина.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Возвращает название логина.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}