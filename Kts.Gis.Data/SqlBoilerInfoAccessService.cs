using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным объектов, представляемых значками на карте, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlBoilerInfoAccessService : BaseSqlDataAccessService, IBoilerInfoAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly SqlDataService dataService;

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlBoilerInfoAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        public SqlBoilerInfoAccessService(SqlDataService dataService, SqlConnector connector, int userId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedId = userId;
        }

        #endregion
    }

    // Реализация 
    public sealed partial class SqlBoilerInfoAccessService : BaseSqlDataAccessService, IBoilerInfoAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает длины труб заданной котельной по годам ввода.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по годам ввода.</returns>
        public List<Tuple<int, double>> GetPipeDates(Guid boilerId, SchemaModel schema)
        {
            var result = new List<Tuple<int, double>>();

            //if (localModeFlag)
            if ( !testConnection("",true))
                return result;

            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_pipe_dates{0} @boiler_id = {1}, @year = {2}, @user_id = {3}", suffix, boilerId, schema.Id, this.loggedId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_pipe_dates" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        int year;
                        double length;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                year = Convert.ToInt32(reader["year"]);
                                length = Convert.ToDouble(reader["length"]);

                                result.Add(new Tuple<int, double>(year, length));
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
        /// Возвращает длины труб заданной котельной по годам ввода.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="pipeTypeId">Идентификатор типа труб.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по годам ввода.</returns>
        public List<Tuple<int, double>> GetPipeDates(Guid boilerId, int pipeTypeId, SchemaModel schema)
        {
            var result = new List<Tuple<int, double>>();
            //if (localModeFlag)
            if ( !testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_pipe_dates{0} @boiler_id = {1}, @pipe_type_id = {2}, @year = {3}, @user_id = {4}", suffix, boilerId, pipeTypeId, schema.Id, this.loggedId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_pipe_dates" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        command.Parameters.Add(new SqlParameter("@pipe_type_id", pipeTypeId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        int year;
                        double length;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                year = Convert.ToInt32(reader["year"]);
                                length = Convert.ToDouble(reader["length"]);

                                result.Add(new Tuple<int, double>(year, length));
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
        /// Возвращает длины труб заданной котельной по диаметрам.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по диаметрам.</returns>
        public List<Tuple<int, double>> GetPipeLengths(Guid boilerId, SchemaModel schema)
        {
            var result = new List<Tuple<int, double>>();
            //if (localModeFlag)
            if ( !testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_pipe_lengths{0} @boiler_id = {1}, @year = {2}, @user_id = {3}", suffix, boilerId, schema.Id, this.loggedId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_pipe_lengths" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        int diameter;
                        double length;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                diameter = Convert.ToInt32(reader["diameter"]);
                                length = Convert.ToDouble(reader["length"]);

                                result.Add(new Tuple<int, double>(diameter, length));
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
        /// Возвращает длины труб заданной котельной по диаметрам.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="pipeTypeId">Идентификатор типа труб.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Длины труб по диаметрам.</returns>
        public List<Tuple<int, double>> GetPipeLengths(Guid boilerId, int pipeTypeId, SchemaModel schema)
        {
            var result = new List<Tuple<int, double>>();
            //if (localModeFlag)
            if ( !testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_pipe_lengths{0} @boiler_id = {1}, @pipe_type_id = {2}, @year = {3}, @user_id = {4}", suffix, boilerId, pipeTypeId, schema.Id, this.loggedId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_pipe_lengths" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        command.Parameters.Add(new SqlParameter("@pipe_type_id", pipeTypeId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        int diameter;
                        double length;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                diameter = Convert.ToInt32(reader["diameter"]);
                                length = Convert.ToDouble(reader["length"]);

                                result.Add(new Tuple<int, double>(diameter, length));
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
        /// Возвращает типы труб, которые присоединены к котельной.
        /// </summary>
        /// <param name="id">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Типы труб.</returns>
        public List<ObjectType> GetPipeTypes(Guid id, SchemaModel schema)
        {
            var result = new List<ObjectType>();
            //if (localModeFlag)
            if ( !testConnection("", true))
                return result;

            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_boiler_pipe_types{0} @id = {1}, @year = {2}, @user_id = {3}", suffix, id, schema.Id, this.loggedId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_boiler_pipe_types" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", id));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        int typeId;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                typeId = Convert.ToInt32(reader["type_id"]);

                                result.Add(this.dataService.GetObjectType(typeId));
                            }
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