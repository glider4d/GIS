using System;

namespace Kts.GisUpdater
{
    /// <summary>
    /// Представляет класс, необходимый для открытия доступа к папкам и файлам, используя авторизацию.
    /// </summary>
    internal sealed partial class UNCAccessWithCredentials : IDisposable
    {
        #region Закрытые поля

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        private string domain;

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        private string password;

        /// <summary>
        /// Путь к папке/файлу.
        /// </summary>
        private string uncPath;

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        private string user;

        #endregion

        #region Деструкторы

        /// <summary>
        /// Финализирует экземпляр класса <see cref="UNCAccessWithCredentials"/>.
        /// </summary>
        ~UNCAccessWithCredentials()
        {
            this.Dispose(false);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает последний код ошибки системы. Если он равен 0, то ошибок нет.
        /// </summary>
        public int LastError
        {
            get;
            private set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Закрывает соединение.
        /// </summary>
        /// <returns>true, если закрытие выполнено успешно, иначе - false.</returns>
        public bool NetUseDelete()
        {
            try
            {
                var returnCode = NativeMethods.NetUseDel(null, uncPath, 2);

                this.LastError = (int)returnCode;

                return returnCode == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Выполняет соединение с указанным путем.
        /// </summary>
        /// <param name="uncPath">Путь.</param>
        /// <param name="user">Имя пользователя.</param>
        /// <param name="domain">Домен пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>true, если соединение выполнено успешно, иначе - false.</returns>
        public bool NetUseWithCredentials(string uncPath, string user, string domain, string password)
        {
            this.uncPath = uncPath;
            this.user = user;
            this.password = password;
            this.domain = domain;

            return this.NetUseWithCredentials();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    this.NetUseDelete();

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Выполняет авторизацию.
        /// </summary>
        /// <returns>true, если авторизация выполнена успешно, иначе - false.</returns>
        private bool NetUseWithCredentials()
        {
            try
            {
                var useinfo = new NativeStructs.USE_INFO_2
                {
                    ui2_remote = this.uncPath,
                    ui2_username = this.user,
                    ui2_domainname = this.domain,
                    ui2_password = this.password,
                    ui2_asg_type = 0,
                    ui2_usecount = 1
                };

                uint paramErrorIndex;

                var returnCode = NativeMethods.NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);

                this.LastError = (int)returnCode;

                return returnCode == 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    // Реализация IDisposable.
    internal sealed partial class UNCAccessWithCredentials
    {
        #region Открытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}