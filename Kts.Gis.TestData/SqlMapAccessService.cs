using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным карты, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlMapAccessService : BaseSqlDataAccessService, IMapAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlMapAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlMapAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация IMapAccessService.
    public sealed partial class SqlMapAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает масштаб линий заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Масштаб линий.</returns>
        public double GetScale(TerritorialEntityModel city)
        {
            var query = string.Format("get_scale @city_id = {0}", city.Id);

            double result = 1;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_scale", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", city.Id));

                        using (var reader = command.ExecuteReader())
                            if (reader.HasRows)
                            {
                                reader.Read();

                                result = Convert.ToDouble(reader["scale"]);
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
        /// Возвращает настройки вида карты.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Настройки вида карты.</returns>
        public MapSettingsModel GetSettings(int cityId)
        {
            var query = string.Format("get_map_settings @city_id = {0}", cityId);

            MapSettingsModel result = null;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_map_settings", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        int figureLabelDefaultSize;
                        double figurePlanningOffset;
                        double figureThickness;
                        int independentLabelDefaultSize;
                        double lineDisabledOffset;
                        int lineLabelDefaultSize;
                        double linePlanningOffset;
                        double lineThickness;

                        using (var reader = command.ExecuteReader())
                            if (reader.HasRows)
                            {
                                reader.Read();

                                figureLabelDefaultSize = Convert.ToInt32(reader["figure_label_default_size"]);
                                figurePlanningOffset = Convert.ToDouble(reader["figure_planning_offset"]);
                                figureThickness = Convert.ToDouble(reader["figure_thickness"]);
                                independentLabelDefaultSize = Convert.ToInt32(reader["independent_label_default_size"]);
                                lineDisabledOffset = Convert.ToDouble(reader["line_disabled_offset"]);
                                lineLabelDefaultSize = Convert.ToInt32(reader["line_label_default_size"]);
                                linePlanningOffset = Convert.ToDouble(reader["line_planning_offset"]);
                                lineThickness = Convert.ToDouble(reader["line_thickness"]);

                                result = new MapSettingsModel(figureLabelDefaultSize, figurePlanningOffset, figureThickness, independentLabelDefaultSize, lineDisabledOffset, lineLabelDefaultSize, linePlanningOffset, lineThickness);
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
        /// Возвращает подложку заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Подложка.</returns>
        public SubstrateModel GetSubstrate(TerritorialEntityModel city)
        {
            var query = string.Format("get_substrate @city_id = {0}", city.Id);    

            SubstrateModel result = null;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_substrate", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", city.Id));

                        DateTime lastUpdate;
                        int width;
                        int height;
                        int columnCount;
                        int rowCount;

                        using (var reader = command.ExecuteReader())
                            if (reader.HasRows)
                            {
                                reader.Read();

                                lastUpdate = Convert.ToDateTime(reader["last_update"]);
                                width = Convert.ToInt32(reader["width"]);
                                height = Convert.ToInt32(reader["height"]);

                                if (reader["column_count"] != DBNull.Value)
                                {
                                    columnCount = Convert.ToInt32(reader["column_count"]);
                                    rowCount = Convert.ToInt32(reader["row_count"]);

                                    result = new SubstrateModel(city, lastUpdate, width, height, columnCount, rowCount);
                                }
                                else
                                {

                                    result = new SubstrateModel(city, lastUpdate, width, height);
                                }
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
        /// Обновляет настройки надписей.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="captionCfg">Настройки надписей.</param>
        public void UpdateCaptions(int cityId, Dictionary<ObjectType, List<ParameterModel>> captionCfg)
        {
            foreach (var entry in captionCfg)
            {
                var captionParams = "";
                foreach (var param in entry.Value)
                    captionParams += param.Id + ",";
                if (captionParams.Length > 0)
                    captionParams = captionParams.Remove(captionParams.Length - 1);

                var query = string.Format("update_caption_params @city_id = {0}, @type_id = {1}, @caption_params = {2}, @user_id = {3}", cityId, entry.Key.TypeId, captionParams, this.loggedUserId);

                try
                {
                    using (var connection = this.Connector.GetConnection())
                        using (var command = new SqlCommand("update_caption_params", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Убираем ограничение по времени выполнения команды.
                            command.CommandTimeout = 0;

                            command.Parameters.Add(new SqlParameter("@city_id", cityId));
                            command.Parameters.Add(new SqlParameter("@type_id", entry.Key.TypeId));
                            command.Parameters.Add(new SqlParameter("@caption_params", captionParams));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                            command.ExecuteNonQuery();
                        }
                }
                catch (Exception e)
                {
                    throw new Exception(query, e);
                }
            }
        }

        

        /// <summary>
        /// Обновляет масштаб линий заданного населенного пункта.
        /// </summary>
        /// <param name="scale">Масштаб линий.</param>
        /// <param name="city">Населенный пункт.</param>
        public void UpdateScale(double scale, TerritorialEntityModel city)
        {
            

            var query = string.Format("update_scale @city_id = {0}, @scale = {1}, @user_id = {2}", city.Id, scale, this.loggedUserId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_scale", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", city.Id));
                        command.Parameters.Add(new SqlParameter("@scale", scale));
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
        /// Обновляет настройки заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="settings">Настройки.</param>
        public void UpdateSettings(int cityId, MapSettingsModel settings)
        {
            var query = string.Format("update_map_settings @figure_label_default_size = {0}, @figure_planning_offset = {1}, @figure_thickness = {2}, @independent_label_default_size = {3}, @line_disabled_offset = {4}, @line_label_default_size = {5}, @line_planning_offset = {6}, @line_thickness = {7}, @city_id = {8}, @user_id = {9}", settings.FigureLabelDefaultSize, settings.FigurePlanningOffset, settings.FigureThickness, settings.IndependentLabelDefaultSize, settings.LineDisabledOffset, settings.LineLabelDefaultSize, settings.LinePlanningOffset, settings.LineThickness, cityId, this.loggedUserId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_map_settings", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;
                        
                        command.Parameters.Add(new SqlParameter("@figure_label_default_size", settings.FigureLabelDefaultSize));
                        command.Parameters.Add(new SqlParameter("@figure_planning_offset", settings.FigurePlanningOffset));
                        command.Parameters.Add(new SqlParameter("@figure_thickness", settings.FigureThickness));
                        command.Parameters.Add(new SqlParameter("@independent_label_default_size", settings.IndependentLabelDefaultSize));
                        command.Parameters.Add(new SqlParameter("@line_disabled_offset", settings.LineDisabledOffset));
                        command.Parameters.Add(new SqlParameter("@line_label_default_size", settings.LineLabelDefaultSize));
                        command.Parameters.Add(new SqlParameter("@line_planning_offset", settings.LinePlanningOffset));
                        command.Parameters.Add(new SqlParameter("@line_thickness", settings.LineThickness));
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