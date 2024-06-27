using Kts.Gis.Data;
using Kts.Gis.Models;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления логина.
    /// </summary>
    public sealed class LoginViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Логин.
        /// </summary>
        private LoginModel login;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LoginViewModel"/>.
        /// </summary>
        /// <param name="login">Логин.</param>
        public LoginViewModel(LoginModel login)
        {
            this.login = login;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор логина.
        /// </summary>
        public int Id
        {
            get
            {
                return this.login.Id;
            }
        }

        /// <summary>
        /// Возвращает название логина.
        /// </summary>
        public string Name
        {
            get
            {
                return this.login.Name;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает ограничения доступов к функциям приложения.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Ограничения доступов к функциям приложения.</returns>
        public List<AccessModel> GetRestrictions(IDataService dataService)
        {
            return dataService.LoginAccessService.GetRestrictions(this.login);
        }

        /// <summary>
        /// Возвращает название пользовательской роли.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Название пользовательской роли.</returns>
        public string GetRoleName(IDataService dataService)
        {
            return dataService.LoginAccessService.GetRoleName(this.login);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что верен ли пароль.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Значение, указывающее на то, что верен ли пароль.</returns>
        public bool IsPasswordCorrect(string password, IDataService dataService)
        {
            return dataService.LoginAccessService.IsPasswordCorrect(this.login, password);
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает список логинов.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список логинов.</returns>
        public static List<LoginViewModel> GetLogins(IDataService dataService)
        {
            var result = new List<LoginViewModel>();

            foreach (var login in dataService.LoginAccessService.GetAll())
                result.Add(new LoginViewModel(login));

            return result;
        }

        #endregion
    }
}