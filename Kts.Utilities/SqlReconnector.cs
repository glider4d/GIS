using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kts.Utilities
{
    //class SqlReconnector
    
        /// <summary>
        /// Представляет коннектор с базой данных SQL с возможностью переподключения.
        /// </summary>
        public sealed class SqlReconnectorWcf : SqlConnector
        {
            #region Конструкторы

            /// <summary>
            /// Инициализирует новый экземпляр класса <see cref="SqlReconnector"/>.
            /// </summary>
            /// <param name="connectionString">Строка подключения к базе данных SQL.</param>
            public SqlReconnectorWcf(SqlConnectionString connectionString) : base(connectionString)
            {
            }

            #endregion

            #region Открытые методы

            /// <summary>
            /// Возвращает соединение с базой данных SQL, если это возможно.
            /// </summary>
            /// <returns>Соединение с базой данных SQL.</returns>
            public SqlConnection TryGetConnection()
            {
                var connection = new SqlConnection(this.ConnectionString.ToString());

                try
                {
                    connection.Open();

                    using (var command = new SqlCommand("set arithabort on", connection))
                    {
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.ExecuteNonQuery();
                    }

                    return connection;
                }
                catch
                {
                    connection.Dispose();
                }

                return null;
            }

            #endregion

            #region Открытые переопределенные методы

            public override SqlConnection GetCon()
            {
                return connect;
            }

            public override SqlConnection GetTestConnection(string operation, bool silence)
            {
                var conn = new SqlConnection(this.ConnectionString.ToString());

                try
                {
                    conn.Open();

                    using (var command = new SqlCommand("set arithabort on", conn))
                    {
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                  
                }

                return conn;
            }

            private SqlConnection connect;
            public override SqlConnection GetConnectionConsole()
            {
                connect = new SqlConnection(this.ConnectionString.ToString());

                try
                {
                    connect.Open();

                    using (var command = new SqlCommand("set arithabort on", connect))
                    {
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine("exp.mess = " + exp.Message);
                    SqlConnection connection;

                    while (true)
                    {
                        connection = this.TryGetConnection();

                        if (connection != null)
                        {
                            connect = connection;

                            break;
                        }
                    }

                    Console.WriteLine("message: " + exp.Message);
                }
                return connect;
            }
            /// <summary>
            /// Возвращает соединение с базой данных SQL.
            /// </summary>
            /// <returns>Соединение с базой данных SQL.</returns>
            public override SqlConnection GetConnection()
            {
                var conn = new SqlConnection(this.ConnectionString.ToString());

                try
                {
                    conn.Open();

                    using (var command = new SqlCommand("set arithabort on", conn))
                    {
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine("exp msg = " + exp.Message);
              
                }

                return conn;
            }

            #endregion
        }
    
}
