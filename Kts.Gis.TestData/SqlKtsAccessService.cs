using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным объектов базовых программ КТС, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlKtsAccessService : BaseSqlDataAccessService, IKtsAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlKtsAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlKtsAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация IKtsAccessService.
    public sealed partial class SqlKtsAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает список скрытых объектов программы "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<long, string>> GetJurHidden(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_jur_hidden{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<long, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_jur_hidden" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                                result.Add(new Tuple<long, string>(Convert.ToInt64(reader["obj_id"]), reader["name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список отображаемых объектов программы "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<long, string>> GetJurVisible(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_jur_visible{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<long, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_jur_visible" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                                result.Add(new Tuple<long, string>(Convert.ToInt64(reader["obj_id"]), reader["name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список скрытых объектов программы "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<long, string>> GetKvpHidden(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_kvp_hidden{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<long, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_kvp_hidden" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                                result.Add(new Tuple<long, string>(Convert.ToInt64(reader["obj_id"]), reader["name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список отображаемых объектов программы "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<long, string>> GetKvpVisible(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_kvp_visible{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<long, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_kvp_visible" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                                result.Add(new Tuple<long, string>(Convert.ToInt64(reader["obj_id"]), reader["name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Скрывает объект по его идентификатору, населенному пункту и идентификатору его программы.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="appId">Идентификатор программы.</param>
        public void HideObj(long id, int cityId, int appId)
        {
            var query = string.Format("hide_kts_obj @obj_id = {0}, @app_id = {1}, @city_id = {2}, @user_id = {3}", id, appId, cityId, this.loggedUserId);
            
            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("hide_kts_obj", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@obj_id", id));
                        command.Parameters.Add(new SqlParameter("@app_id", appId));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Отображает объект по его идентификатору, населенному пункту и идентификатору его программы.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="appId">Идентификатор программы.</param>
        public void ShowObj(long id, int cityId, int appId)
        {
            var query = string.Format("show_kts_obj @obj_id = {0}, @app_id = {1}, @city_id = {2}, @user_id = {3}", id, appId, cityId, this.loggedUserId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("show_kts_obj", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@obj_id", id));
                        command.Parameters.Add(new SqlParameter("@app_id", appId));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        #endregion
    }
}