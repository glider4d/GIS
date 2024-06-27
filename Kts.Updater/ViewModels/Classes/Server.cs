namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет сервер.
    /// </summary>
    internal sealed class Server
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Server"/>.
        /// </summary>
        /// <param name="name">Название сервера.</param>
        /// <param name="address">Адрес сервера.</param>
        /// <param name="user">Пользователь сервера.</param>
        /// <param name="password">Пароль сервера.</param>
        public Server(string name, string address, string user, string password)
        {
            this.Name = name;
            this.Address = address;
            this.User = user;
            this.Password = password;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает адрес сервера.
        /// </summary>
        public string Address
        {
            get;
        }

        /// <summary>
        /// Возвращает название сервера.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает пароль сервера.
        /// </summary>
        public string Password
        {
            get;
        }

        /// <summary>
        /// Возвращает пользователя сервера.
        /// </summary>
        public string User
        {
            get;
        }
        
        #endregion
    }
}