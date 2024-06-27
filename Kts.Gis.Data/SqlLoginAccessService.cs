using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным логинов, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlLoginAccessService : BaseSqlDataAccessService, ILoginAccessService
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlLoginAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        public SqlLoginAccessService(SqlConnector connector) : base(connector)
        {
        }

        public string getMethodCallerName()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    // Реализация ILoginAccessService.
    public sealed partial class SqlLoginAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Меняет пароль заданного пользователя.
        /// </summary>
        /// <param name="loginId">Идентификатор пользователя.</param>
        /// <param name="oldPassword">Старый пароль.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <returns>true, если пароль изменен, иначе - false.</returns>
        public bool ChangePassword(int loginId, string oldPassword, string newPassword)
        {
            var query = "update_password";

            var result = false;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_password", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@user_id", loginId));
                        command.Parameters.Add(new SqlParameter("@old_password", oldPassword));
                        command.Parameters.Add(new SqlParameter("@new_password", newPassword));

                        result = Convert.ToBoolean(command.ExecuteScalar());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает все логины из источника данных.
        /// </summary>
        /// <returns>Список логинов.</returns>
        public List<LoginModel> GetAll()
        {
            var query = "get_logins";

            var result = new List<LoginModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_logins", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        int id;
                        string name;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["id"]);
                                name = Convert.ToString(reader["name"]);

                                result.Add(new LoginModel(id, name));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает ограничения доступов к функциям приложения.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Ограничения доступов к функциям приложения.</returns>
        public List<AccessModel> GetRestrictions(LoginModel login)
        {
            var query = string.Format("get_restrictions @login_id = {0}", login.Id);

            var result = new List<AccessModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_restrictions", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@login_id", login.Id));

                        string alias;
                        object value;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                alias = Convert.ToString(reader["alias"]);
                                value = reader["value"];

                                result.Add(new AccessModel(alias, value));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает название пользовательской роли.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Название пользовательской роли.</returns>
        public string GetRoleName(LoginModel login)
        {
            var query = string.Format("get_role_name @id = {0}", login.Id);

            var result = "???";

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_role_name", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", login.Id));

                        result = Convert.ToString(command.ExecuteScalar());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        public bool IsPasswordCorrect(LoginModel login, string password, IDataService dataService)
        {
            return IsPasswordCorrect(login, password);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что верен ли пароль.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Значение, указывающее на то, что верен ли пароль.</returns>
        public bool IsPasswordCorrect(LoginModel login, string password)
        {
            var query = string.Format("is_password_correct @id = {0}, @password = {1}", login.Id, password);

            var result = false;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("is_password_correct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", login.Id));
                        command.Parameters.Add(new SqlParameter("@password", password));

                        result = Convert.ToBoolean(command.ExecuteScalar());
                    
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Задает значение, указывающее на то, что залогинен ли пользователь или нет.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="ip">IP-адрес пользователя.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="isLogged">Значение, указывающее на то, что залогинен ли пользователь или нет.</param>
        /// <returns>Значение, указывающее на то, что выполнено ли логирование пользователя.</returns>
        public bool SetIsUserLogged(int id, string ip, string version, bool isLogged)
        {
            var query = string.Format("set_is_user_logged @id = {0}, @ip = {1}, @version = {2}, @is_logged = {3}", id, ip, version, isLogged);

            bool result = false;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("set_is_user_logged", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", id));
                        command.Parameters.Add(new SqlParameter("@ip", ip));
                        command.Parameters.Add(new SqlParameter("@version", version));
                        command.Parameters.Add(new SqlParameter("@is_logged", isLogged));

                        result = Convert.ToBoolean(command.ExecuteScalar());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        #endregion
    }
}