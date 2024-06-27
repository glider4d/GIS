using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным топлива котельных, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlFuelAccessService : BaseSqlDataAccessService, IFuelAccessService
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlFuelAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        public SqlFuelAccessService(SqlConnector connector) : base(connector)
        {
        }

        #endregion
    }

    // Реализация IFuelAccessService.
    public sealed partial class SqlFuelAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли получить доступ к информации о топливе.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        public bool CanAccessFuelInfo()
        {
            var query = string.Format("can_access_fuel_info");

            var result = false;
            //if (localModeFlag)
            //if (!testConnection("", true))
            //    return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("can_access_fuel_info", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

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
        /// Возвращает информацию о топливе заданной котельной.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fuelId">Идентификатор топлива.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Информация о топливе.</returns>
        public FuelInfoModel GetFuelInfo(Guid boilerId, int fuelId, DateTime fromDate, DateTime toDate)
        {
            var query = string.Format("get_fuel_info @boiler_id = {0}, @fuel_id = {1}, @from_date = {2}, @to_date = {3}", boilerId, fuelId, fromDate, toDate);

            FuelInfoModel result = null;
            //if (localModeFlag)
            if (!testConnection("", true))
                return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_fuel_info", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        command.Parameters.Add(new SqlParameter("@fuel_id", fuelId));
                        command.Parameters.Add(new SqlParameter("@from_date", fromDate));
                        command.Parameters.Add(new SqlParameter("@to_date", toDate));

                        double availableCount;
                        double monthLimit;
                        double dayLimit;
                        double incoming;
                        double consumption;
                        double endBalance;
                        int dayBalance;
                        double moving;
                        string provision;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                availableCount = Convert.ToDouble(reader["available_count"]);
                                monthLimit = Convert.ToDouble(reader["month_limit"]);
                                dayLimit = Convert.ToDouble(reader["day_limit"]);
                                incoming = Convert.ToDouble(reader["incoming"]);
                                consumption = Convert.ToDouble(reader["consumption"]);
                                endBalance = Convert.ToDouble(reader["end_balance"]);
                                moving = Convert.ToDouble(reader["moving"]);
                                provision = Convert.ToString(reader["provision"]);
                                if (reader["day_balance"] != DBNull.Value)
                                {
                                    dayBalance = Convert.ToInt32(reader["day_balance"]);

                                    result = new FuelInfoModel(availableCount, monthLimit, dayLimit, incoming, consumption, endBalance, dayBalance, moving, provision);
                                }
                                else
                                    result = new FuelInfoModel(availableCount, monthLimit, dayLimit, incoming, consumption, endBalance, moving, provision);
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
        /// Возвращает виды топлива заданной котельной.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Список моделей топлива.</returns>
        public List<FuelModel> GetFuelTypes(Guid boilerId, DateTime fromDate, DateTime toDate)
        {
            var query = string.Format("get_fuel_types @boiler_id = {0}, @from_date = {1}, @to_date = {2}", boilerId, fromDate, toDate);

            var result = new List<FuelModel>();
            //if (localModeFlag)
            if (!testConnection("", true))
                return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_fuel_types", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        command.Parameters.Add(new SqlParameter("@from_date", fromDate));
                        command.Parameters.Add(new SqlParameter("@to_date", toDate));

                        int id;
                        string name;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["id"]);
                                name = Convert.ToString(reader["name"]);

                                result.Add(new FuelModel(id, name));
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
        /// Возвращает информацию о складах топлива заданного населенного пункта.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fuelId">Идентификатор топлива.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returns>Информация о складах топлива.</returns>
        public List<FuelStorageModel> GetStoragesInfo(Guid boilerId, int fuelId, DateTime fromDate, DateTime toDate)
        {
            var query = string.Format("get_storages_info @boilerId = {0}, @fuel_id = {1}, @from_date = {2}, @to_date = {3}", boilerId, fuelId, fromDate, toDate);

            var result = new List<FuelStorageModel>();
            //if (localModeFlag)
            if (!testConnection("", true))
                return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_storages_info", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        command.Parameters.Add(new SqlParameter("@fuel_id", fuelId));
                        command.Parameters.Add(new SqlParameter("@from_date", fromDate));
                        command.Parameters.Add(new SqlParameter("@to_date", toDate));

                        string name;
                        double balance;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                name = Convert.ToString(reader["name"]);
                                balance = Convert.ToDouble(reader["balance"]);

                                result.Add(new FuelStorageModel(name, balance));
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