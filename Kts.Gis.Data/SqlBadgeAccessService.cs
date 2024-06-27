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
    public sealed partial class SqlBadgeAccessService : BaseSqlDataAccessService, IChildAccessService<BadgeModel>
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly SqlDataService dataService;

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlBadgeAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlBadgeAccessService(SqlDataService dataService, SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация IChildAccessService<BadgeModel>.
    public sealed partial class SqlBadgeAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Удаляет объект из источника данных.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteObject(BadgeModel obj, SchemaModel schema)
        {
            //if (localModeFlag)
            if (!testConnection("", true))
                return;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, true);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("remove_object" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@right_now", true));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Возвращает все дочерние объекты из заданного набора данных, принадлежащие заданному родителю.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        /// <param name="obj">Объект-родитель.</param>
        /// <returns>Список дочерних объектов.</returns>
        public List<BadgeModel> GetAll(DataSet dataSet, IObjectModel obj)
        {
            var result = new List<BadgeModel>();
            //if (localModeFlag)
            /*
            if ( !testConnection("", true))
                return result;*/
            Guid id;
            int typeId;
            double distance;
            bool isPlanning;
            bool isActive;

            if (dataSet.Tables.Contains("badges"))
                foreach (var row in dataSet.Tables["badges"].Select("parent_id = '" + obj.Id + "'"))
                {
                    id = Guid.Parse(row["id"].ToString());
                    typeId = Convert.ToInt32(row["type_id"]);
                    distance = Convert.ToDouble(row["distance"]);
                    isPlanning = Convert.ToBoolean(row["is_planning"]);
                    isActive = Convert.ToBoolean(row["is_active"]);

                    result.Add(new BadgeModel(id, obj.Id, obj.CityId, this.dataService.GetObjectType(typeId), isPlanning, distance, isActive));
                }

            return result;
        }

        /// <summary>
        /// Возвращает все дочерние объекты из источника данных, принадлежащие заданному родителю.
        /// </summary>
        /// <param name="obj">Объект-родитель.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список дочерних объектов.</returns>
        public List<BadgeModel> GetAll(IObjectModel obj, SchemaModel schema)
        {
            
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_badges{0} @parent_id = {1}, @year = {2}, @user_id = {3}", suffix, obj.Id, schema.Id, this.loggedUserId);

            var result = new List<BadgeModel>();
            //if (localModeFlag)
            if ( !testConnection("", true))
                return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_badges" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@parent_id", obj.Id));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }

                        Guid id;
                        int typeId;
                        double distance;
                        bool isPlanning;
                        bool isActive;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                distance = Convert.ToDouble(reader["distance"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                isActive = Convert.ToBoolean(reader["is_active"]);
                                result.Add(new BadgeModel(id, obj.Id, obj.CityId, this.dataService.GetObjectType(typeId), isPlanning, distance, isActive));
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
        /// Возвращает таблицу данных всех дочерних объектов из источника данных, принадлежащих заданному населенному пункту, в необработанном виде.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Таблица данных.</returns>
        public DataTable GetAllRaw(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_all_badges{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new DataTable("badges");
            //if (localModeFlag)
            if ( !testConnection("", true))
                return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_all_badges" + suffix, connection))
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

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        public void MarkDeleteObject(BadgeModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, false);
            //if (localModeFlag)
            if ( !testConnection("", true))
                return;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("remove_object" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@right_now", false));

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