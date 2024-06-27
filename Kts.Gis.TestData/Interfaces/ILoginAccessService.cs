using Kts.Gis.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса доступа к данным логинов.
    /// </summary>
    
    public interface ILoginAccessService
    {
        #region Методы

        /// <summary>
        /// Меняет пароль заданного пользователя.
        /// </summary>
        /// <param name="loginId">Идентификатор пользователя.</param>
        /// <param name="oldPassword">Старый пароль.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <returns>true, если пароль изменен, иначе - false.</returns>
        string getMethodCallerName();

        bool ChangePassword(int loginId, string oldPassword, string newPassword);

        /// <summary>
        /// Возвращает все логины из источника данных.
        /// </summary>
        /// <returns>Список логинов.</returns>
        List<LoginModel> GetAll();

        /// <summary>
        /// Возвращает ограничения доступов к функциям приложения.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Ограничения доступов к функциям приложения.</returns>
        List<AccessModel> GetRestrictions(LoginModel login);

        /// <summary>
        /// Возвращает название пользовательской роли.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Название пользовательской роли.</returns>
        string GetRoleName(LoginModel login);

        /// <summary>
        /// Возвращает значение, указывающее на то, что верен ли пароль.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Значение, указывающее на то, что верен ли пароль.</returns>
        bool IsPasswordCorrect(LoginModel login, string password);

        /// <summary>
        /// Задает значение, указывающее на то, что залогинен ли пользователь или нет.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="ip">IP-адрес пользователя.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="isLogged">Значение, указывающее на то, что залогинен ли пользователь или нет.</param>
        /// <returns>Значение, указывающее на то, что выполнено ли логирование пользователя.</returns>
        bool SetIsUserLogged(int id, string ip, string version, bool isLogged);

        #endregion
    }
}