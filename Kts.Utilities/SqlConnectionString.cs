using System;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;


namespace Kts.Utilities
{
    /// <summary>
    /// Представляет строку подключения к базе данных SQL.
    /// </summary> 
    [Serializable]
    public sealed class SqlConnectionString
    {
        #region Закрытые константы

        /// <summary>
        /// Шаблон строки подключения к базе данных SQL.
        /// </summary>
        private const string connectionString = "user id={0};password={1};server={2};database={3};connection timeout={4};language=english;asynchronous processing=true";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Кэшированная версия строки подключения.
        /// </summary>
        private string cached = "";

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что являются ли данные строки правильно отформатированными.
        /// </summary>
        private readonly bool isFormatted;

        #endregion

        #region Конструкторы
        
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlConnectionString"/>.
        /// </summary>
        /// <param name="server">Адрес сервера.</param>
        /// <param name="userId">Имя входа.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="database">Название базы данных.</param>
        /// <param name="connectionTimeout">Таймаут соединения.</param>
        /// <param name="name">Название сервера.</param>
        /// <param name="isFormatted">Значение, указывающее на то, что являются ли данные строки правильно отформатированными.</param>
        public SqlConnectionString(string server, string userId, string password, string database, int connectionTimeout, string name, bool isFormatted = false)
        {
            this.Server = server;
            this.UserId = userId;
            this.Password = password;
            this.Database = database;
            this.ConnectionTimeout = connectionTimeout;
            this.Name = name;
            this.isFormatted = isFormatted;
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает таймаут соединения.
        /// </summary>
        public int ConnectionTimeout
        {
            get;
        }

        /// <summary>
        /// Возвращает название базы данных.
        /// </summary>
        
        public string Database
        {
            get;
        }

        /// <summary>
        /// Возвращает название сервера.
        
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает пароль.
        
        public string Password
        {
            get;
        }

        /// <summary>
        /// Возвращает адрес сервера.
        
        public string Server
        {
            get;
        }

        /// <summary>
        /// Возвращает имя входа.
        
        public string UserId
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Форматирует входную строку.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <returns>Отформатированная строка.</returns>
        private string Format(string input)
        {
            var inputBytes = Convert.FromBase64String(input);

            var keyBytes = new Rfc2898DeriveBytes("my3y3sl1k3", Encoding.ASCII.GetBytes("0p3nd00rs")).GetBytes(256 / 8);

            var symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes("h0wc@ny0us331nt0"));

            byte[] resultBytes;
            int decryptedByteCount;

            MemoryStream memoryStream = null;

            try
            {
                memoryStream = new MemoryStream(inputBytes);

                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    memoryStream = null;

                    resultBytes = new byte[inputBytes.Length];

                    decryptedByteCount = cryptoStream.Read(resultBytes, 0, resultBytes.Length);
                }
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
            }

            return Encoding.UTF8.GetString(resultBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает текстовое представление экземпляра класса.
        /// </summary>
        /// <returns>Текстовое представление экземпляра класса.</returns>
        public override string ToString()
        {
            if (this.isFormatted)
            {
                if (string.IsNullOrEmpty(this.cached))
                    this.cached = string.Format(connectionString, this.Format(this.UserId), this.Format(this.Password), this.Server, this.Format(this.Database), this.ConnectionTimeout);

                return this.cached;
            }
            
            return string.Format(connectionString, this.UserId, this.Password, this.Server, this.Database, this.ConnectionTimeout);
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает строку подключения к базе данных SQL из строки.
        /// </summary>
        /// <param name="input">Строка.</param>
        /// <returns>Строка подключения к базе данных SQL.</returns>
        public static SqlConnectionString FromString(string input)
        {
            var builder = new SqlConnectionStringBuilder(input);

            return new SqlConnectionString(builder.DataSource, builder.UserID, builder.Password, builder.InitialCatalog, builder.ConnectTimeout, builder.DataSource);
        }

        #endregion
    }
}