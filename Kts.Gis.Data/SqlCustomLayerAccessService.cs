using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным кастомных слоев.
    /// </summary>
    public sealed partial class SqlCustomLayerAccessService : BaseSqlDataAccessService, ICustomLayerAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор авторизованного пользователя
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlCustomLayerAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор авторизованного пользователя.</param>
        public SqlCustomLayerAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация ICustomLayerAccessService.
    public sealed partial class SqlCustomLayerAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Добавляет кастомный слой.
        /// </summary>
        /// <param name="layer">Слой.</param>
        /// <returns>Идентификатор добавленного кастомного слоя.</returns>
        public Guid AddLayer(CustomLayerModel layer)
        {
            var suffix = layer.Schema.IsIS ? "_is" : "";

            var query = string.Format("add_custom_layer{0} @city_id = {1}, @year = {2}, @name = {3}, @user_id = {4}", suffix, layer.CityId, layer.Schema.Id, layer.Name, this.loggedUserId);

            Guid result = Guid.Empty;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("add_custom_layer" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", layer.CityId));
                        if (!layer.Schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", layer.Schema.Id));
                        command.Parameters.Add(new SqlParameter("@name", layer.Name));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        result = Guid.Parse(command.ExecuteScalar().ToString());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Выполняет удаление кастомного слоя.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="id">Идентификатор удаляемого слоя.</param>
        public void DeleteLayer(SchemaModel schema, int cityId, Guid id)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_custom_layer{0} @year = {1}, @city_id = {2}, @id = {3}, @user_id = {4}", suffix, schema.Id, cityId, id, this.loggedUserId);

            var result = new List<CustomLayerModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("remove_custom_layer" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        command.Parameters.Add(new SqlParameter("@id", id));
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
        /// Возвращает кастомные слои заданной схемы.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Кастомные слои.</returns>
        public List<CustomLayerModel> GetCustomLayers(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_custom_layers{0} @year = {1}, @city_id = {2}", suffix, schema.Id, cityId);

            var result = new List<CustomLayerModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_custom_layers" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        Guid id;
                        string name;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                name = Convert.ToString(reader["name"]);

                                result.Add(new CustomLayerModel(id, schema, cityId, name));
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
        /// Обновляет кастомный слой.
        /// </summary>
        /// <param name="layer">Слой.</param>
        public void UpdateLayer(CustomLayerModel layer)
        {
            var suffix = layer.Schema.IsIS ? "_is" : "";

            var query = string.Format("update_custom_layer{0} @id = {1}, @name = {2}, @year = {3}, @city_id = {4}, @user_id = {5}", suffix, layer.Id, layer.Name, layer.Schema.Id, layer.CityId, this.loggedUserId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_custom_layer" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", layer.Id));
                        command.Parameters.Add(new SqlParameter("@name", layer.Name));
                        if (!layer.Schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", layer.Schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", layer.CityId));
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