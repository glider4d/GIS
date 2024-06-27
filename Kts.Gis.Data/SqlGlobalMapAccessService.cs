using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным глобальной карты, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlGlobalMapAccessService : BaseSqlDataAccessService, IGlobalMapAccessService
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlGlobalMapAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        public SqlGlobalMapAccessService(SqlConnector connector) : base(connector)
        {
        }

        #endregion
    }

    // Реализация IGlobalMapAccessService.
    public sealed partial class SqlGlobalMapAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Асинхронно возвращает информацию о заданном визуальном регионе.
        /// </summary>
        /// <param name="region">Визуальный регион.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Информация о визуальном регионе.</returns>
        public async Task<VisualRegionInfoModel> GetVisualRegionInfoAsync(VisualRegionModel region, CancellationToken token)
        {
            var objectCount = new List<Tuple<bool, string, int>>();
            var lengths = new List<Tuple<bool, string, double>>();

            var query = string.Format("get_object_count @region_id = {0}", region.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_object_count", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@region_id", region.Id));

                        bool isPlanning;
                        string type;
                        int count;

                        using (var reader = await command.ExecuteReaderAsync(token))
                            while (reader.Read())
                            {
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                type = Convert.ToString(reader["type"]);
                                count = Convert.ToInt32(reader["count"]);

                                objectCount.Add(new Tuple<bool, string, int>(isPlanning, type, count));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            query = string.Format("get_lengths @region_id = {0}", region.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_lengths", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@region_id", region.Id));

                        bool isPlanning;
                        string type;
                        double length;

                        using (var reader = await command.ExecuteReaderAsync(token))
                            while (reader.Read())
                            {
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                type = Convert.ToString(reader["type"]);
                                length = Convert.ToDouble(reader["length"]);

                                lengths.Add(new Tuple<bool, string, double>(isPlanning, type, length));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return new VisualRegionInfoModel(objectCount, lengths);
        }

        public async Task<VisualRegionInfoModel> GetVisualRegionInfoAsync(VisualRegionModel region)
        {
            return await GetVisualRegionInfoAsync(region, new CancellationToken());
        }

        /// <summary>
        /// Возвращает визуальные регионы.
        /// </summary>
        /// <returns>Визуальные регионы.</returns>
        public List<VisualRegionModel> GetVisualRegions()
        {
            var query = "get_visual_regions";

            var result = new List<VisualRegionModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_visual_regions", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        int id;
                        string name;
                        string path;
                        string transform;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["id"]);
                                name = Convert.ToString(reader["name"]);
                                path = Convert.ToString(reader["path"]);
                                transform = Convert.ToString(reader["transform"]);

                                result.Add(new VisualRegionModel(id, name, path, transform));
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