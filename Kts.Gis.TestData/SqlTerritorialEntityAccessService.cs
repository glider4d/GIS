using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным территориальных единиц, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlTerritorialEntityAccessService : BaseSqlDataAccessService, ITerritorialEntityAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlTerritorialEntityAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        public SqlTerritorialEntityAccessService(SqlConnector connector, int userId) : base(connector)
        {
            this.loggedId = userId;
        }

        #endregion
    }

    // Реализация ITerritorialEntityAccessService.
    public sealed partial class SqlTerritorialEntityAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает все котельные заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список котельных.</returns>
        public List<Tuple<Guid, string>> GetBoilers(TerritorialEntityModel city, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_boilers{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, city.Id, schema.Id, this.loggedId);

            var result = new List<Tuple<Guid, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_boilers" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", city.Id));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        Guid id;
                        string name;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(Convert.ToString(reader["id"]));
                                name = Convert.ToString(reader["name"]);

                                result.Add(new Tuple<Guid, string>(id, name));
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
        /// Возвращает все котельные заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список котельных.</returns>
        public List<Tuple<Guid, string>> GetBoilers(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_boilers{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedId);

            var result = new List<Tuple<Guid, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_boilers" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedId));
                        }

                        Guid id;
                        string name;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(Convert.ToString(reader["id"]));
                                name = Convert.ToString(reader["name"]);

                                result.Add(new Tuple<Guid, string>(id, name));
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
        /// Возвращает все населенные пункты заданного региона из источника данных.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Список населенных пунктов.</returns>
        public List<TerritorialEntityModel> GetCities(TerritorialEntityModel region)
        {
            var query = string.Format("get_cities @region_id = {0}", region.Id);

            var result = new List<TerritorialEntityModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_cities", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@region_id", region.Id));

                        int id;
                        string name;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["id"]);
                                name = Convert.ToString(reader["name"]);

                                result.Add(new TerritorialEntityModel(id, name));
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
        /// Возвращает регион и район, в котором расположен заданный населенный пункт.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Тюпл, содержащий регион и район.</returns>
        public Tuple<TerritorialEntityModel, TerritorialEntityModel> GetCityData(TerritorialEntityModel city)
        {
            var query = string.Format("get_city_data @city_id = {0}", city.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_city_data", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", city.Id));

                        int regionId;
                        int districtId;

                        using (var reader = command.ExecuteReader())
                            if (reader.HasRows)
                            {
                                reader.Read();

                                regionId = Convert.ToInt32(reader["region_id"]);
                                districtId = Convert.ToInt32(reader["district_id"]);

                                return new Tuple<TerritorialEntityModel, TerritorialEntityModel>(new TerritorialEntityModel(regionId, ""), new TerritorialEntityModel(districtId, ""));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return null;
        }

        /// <summary>
        /// Возвращает все регионы из источника данных.
        /// </summary>
        /// <returns>Список регионов.</returns>
        public List<TerritorialEntityModel> GetRegions()
        {
            var query = "get_regions";

            var result = new List<TerritorialEntityModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_regions", connection))
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

                                result.Add(new TerritorialEntityModel(id, name));
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
        /// Возвращает значение, указывающее на то, что является ли заданный населенный пункт зафиксированным.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>true, если населенный пункт зафиксирован, иначе - false.</returns>
        public bool IsCityFixed(TerritorialEntityModel city)
        {
            var query = string.Format("is_city_fixed @city_id = {0}", city.Id);

            var result = false;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("is_city_fixed", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", city.Id));

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