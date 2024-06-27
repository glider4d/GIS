using Kts.Gis.Data;
using System;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет аргумент события завершения авторизации.
    /// </summary>
    internal sealed class AuthorizationCompletedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationCompletedEventArgs"/>.
        /// </summary>
        /// <param name="loginId">Идентификатор авторизованного пользователя.</param>
        /// <param name="ip">IP-адрес авторизованного пользователя.</param>
        /// <param name="dataService">Сервис доступа к данным.</param>
        public AuthorizationCompletedEventArgs(int loginId, string ip, IDataService dataService)
        {
            this.LoginId = loginId;
            this.Ip = ip;
            this.DataService = dataService;
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает сервис доступа к данным.
        /// </summary>
        public IDataService DataService
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор авторизованного пользователя.
        /// </summary>
        public int LoginId
        {
            get;
        }

        /// <summary>
        /// Возвращает IP-адрес авторизованного пользователя.
        /// </summary>
        public string Ip
        {
            get;
        }

        #endregion
    }
}