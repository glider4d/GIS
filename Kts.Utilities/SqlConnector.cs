using System.Data.SqlClient;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет коннектор с базой данных SQL.
    /// </summary>
    public abstract class SqlConnector
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlConnector"/>.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных SQL.</param>
        public SqlConnector(SqlConnectionString connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает строку подключения к базе данных SQL.
        /// </summary>
        public SqlConnectionString ConnectionString
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает соединение с базой данных SQL.
        /// </summary>
        /// <returns>Соединение с базой данных SQL.</returns>
        public abstract SqlConnection GetConnection();

        public abstract SqlConnection GetCon();

        public abstract SqlConnection GetConnectionConsole();

        public abstract SqlConnection GetTestConnection(string operation = "Close", bool silence = false);
        #endregion
    }
}