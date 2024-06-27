using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным невизуальных объектов, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlNonVisualObjectAccessService : BaseSqlDataAccessService, IChildAccessService<NonVisualObjectModel>
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
        /// Инициализирует новый экземпляр класса <see cref="SqlNonVisualObjectAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlNonVisualObjectAccessService(SqlDataService dataService, SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация IChildAccessService<NonVisualObjectModel>.
    public sealed partial class SqlNonVisualObjectAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Удаляет объект из источника данных.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteObject(NonVisualObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, true);
            if (!testConnection("", true))
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
        public List<NonVisualObjectModel> GetAll(DataSet dataSet, IObjectModel obj)
        {
            var result = new List<NonVisualObjectModel>();
            /*
            if (!testConnection("", true))
                return result;*/
            Guid id;
            int typeId;
            string name;
            bool isPlanning;
            bool isActive;
            bool hasChildren;

            if (dataSet.Tables.Contains("non_visual_objects"))
                foreach (var row in dataSet.Tables["non_visual_objects"].Select("parent_id = '" + obj.Id + "'"))
                {
                    id = Guid.Parse(row["id"].ToString());
                    typeId = Convert.ToInt32(row["type_id"]);
                    name = Convert.ToString(row["name"]);
                    isPlanning = Convert.ToBoolean(row["is_planning"]);
                    isActive = Convert.ToBoolean(row["is_active"]);
                    hasChildren = Convert.ToBoolean(row["has_children"]);

                    result.Add(new NonVisualObjectModel(id, obj.Id, obj.CityId, this.dataService.GetObjectType(typeId), isPlanning, name, isActive, hasChildren));
                }

            return result;
        }

        /// <summary>
        /// Возвращает все дочерние объекты из источника данных, принадлежащие заданному родителю.
        /// </summary>
        /// <param name="obj">Объект-родитель.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список дочерних объектов.</returns>
        public List<NonVisualObjectModel> GetAll(IObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_non_visuals{0} @parent_id = {1}, @year = {2}, @city_id = {3}, @user_id = {4}", suffix, obj.Id, schema.Id, obj.CityId, this.loggedUserId);

            var result = new List<NonVisualObjectModel>();
            if (!testConnection("", true))
                return result;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_non_visuals" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@parent_id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        Guid id;
                        int typeId;
                        string name;
                        bool isPlanning;
                        bool isActive;
                        bool hasChildren;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                name = Convert.ToString(reader["name"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                isActive = Convert.ToBoolean(reader["is_active"]);
                                hasChildren = Convert.ToBoolean(reader["has_children"]);

                                result.Add(new NonVisualObjectModel(id, obj.Id, obj.CityId, this.dataService.GetObjectType(typeId), isPlanning, name, isActive, hasChildren));
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
            throw new NotImplementedException("Не реализовано возвращение всех невизуальных объектов заданного населенного пункта");
        }

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        public void MarkDeleteObject(NonVisualObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, false);

            if (!testConnection("", true))
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