using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным объектов, представляемых узлами на карте, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlNodeAccessService : BaseSqlDataAccessService, INodeAccessService
    {
        #region Закрытые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private SqlDataService dataService;

        /// <summary>
        /// Внутренний разделитель списка данных о соединения с узлом.
        /// </summary>
        private char[] innerDiv = new char[1]
        {
            ' '
        };

        /// <summary>
        /// Внешний разделитель списка данных о соединения с узлом.
        /// </summary>
        private char[] outerDiv = new char[1]
        {
            ';'
        };

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlNodeAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlNodeAccessService(SqlDataService dataService, SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedUserId = loggedUserId;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает список данных соединения с узлом в виде строки.
        /// </summary>
        /// <param name="connData">Список данных соединения с узлом.</param>
        /// <returns>Строка.</returns>
        private string GetConnDataAsString(List<NodeConnectionData> connData)
        {
            if (connData == null)
                return "";

            var result = "";

            foreach (var data in connData)
                result += data.ToString() + "; ";

            return result;
        }

        /// <summary>
        /// Возвращает список данных соединения с узлом из строки.
        /// </summary>
        /// <param name="connData">Строка, представляющая список данных соединения с узлом.</param>
        /// <returns>Список данных соединения с узлом.</returns>
        private List<NodeConnectionData> GetConnDataFromString(string connData)
        {
            var result = new List<NodeConnectionData>();
            
            string[] temp;

            foreach (var entry in connData.Split(this.outerDiv, StringSplitOptions.RemoveEmptyEntries))
            {
                temp = entry.Split(this.innerDiv, StringSplitOptions.RemoveEmptyEntries);

                if (temp.Length > 1)
                    result.Add(new NodeConnectionData(Guid.Parse(temp[0]), (NodeConnectionSide)Convert.ToInt32(temp[1])));
            }

            return result;
        }

        public void UpdateObjectFromLocal(Guid guid, int index)
        {
            var query = "update_label";
            var id = ObjectModel.DefaultId;
            try
            {

                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(index);
                var suffix = serrializedSqlQuery.getSufix(index);
                if (serializedParameters != null)
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_node" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        foreach (var dictionaryItem in serializedParameters)
                        {

                            if (dictionaryItem.Key.Equals("@id"))
                            {

                                if (dictionaryItem.Value is Guid)
                                {
                                    id = (Guid)dictionaryItem.Value;
                                }

                                if (id == ObjectModel.DefaultId)
                                    id = guid;
                                command.Parameters.Add(new SqlParameter(dictionaryItem.Key, id));
                            }
                            else
                            {
                                command.Parameters.Add(new SqlParameter(dictionaryItem.Key, dictionaryItem.Value));
                            }

                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        #endregion
    }

    // Реализация INodeAccessService.
    public sealed partial class SqlNodeAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Добавляет узел в источник данных.
        /// </summary>
        /// <param name="node">Добавляемый узел.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор добавленного узла.</returns>
        public Guid AddNode(NodeModel node, SchemaModel schema)
        {
            Guid result = Guid.Empty;
            if (BaseSqlDataAccessService.localModeFlag)
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("add_node{0} @city_id = {1}, @left = {2}, @top = {3}, @conn_obj_id = {4}, @conn_data = {5}, @ignore_stick = {6}, @user_id = {7}, @year = {8}", suffix, node.CityId, node.Position.X, node.Position.Y, node.ConnectedObjectData != null ? node.ConnectedObjectData.Item1.ToString() : "null", this.GetConnDataAsString(node.ConnectionData), node.IgnoreStick, this.loggedUserId, schema.Id);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("add_node" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", node.CityId));
                        command.Parameters.Add(new SqlParameter("@left", node.Position.X));
                        command.Parameters.Add(new SqlParameter("@top", node.Position.Y));
                        if (node.ConnectedObjectData != null)
                            command.Parameters.Add(new SqlParameter("@conn_obj_id", node.ConnectedObjectData.Item1));
                        command.Parameters.Add(new SqlParameter("@conn_data", this.GetConnDataAsString(node.ConnectionData)));
                        command.Parameters.Add(new SqlParameter("@ignore_stick", node.IgnoreStick));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

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
        /// Удаляет узел из источника данных.
        /// </summary>
        /// <param name="node">Удаляемый узел.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteNode(NodeModel node, SchemaModel schema)
        {
            if (!testConnection("", true))
                return;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_node{0} @id = {1}, @user_id = {2}, @year = {3}", suffix, node.Id, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("remove_node" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", node.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Возвращает все объекты из источника данных, находящиеся в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        public List<NodeModel> GetAll(int cityId, SchemaModel schema)
        {
            var result = new List<NodeModel>();
            if (!testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_nodes{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);


            int stepNumber = 0;
            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_nodes" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }

                        Guid id;
                        int typeId;
                        double left;
                        double top;
                        Guid connectedObjectId;
                        int connectedObjectTypeId;
                        bool isConnectedObjectPlanning;
                        Tuple<Guid, ObjectType, bool> connectedObjectData;
                        string connData;
                        bool ignoreStick;
                        
                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                stepNumber++;
                                
                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                left = Convert.ToDouble(reader["left"]);
                                top = Convert.ToDouble(reader["top"]);
                                if (reader["conn_obj_id"] != DBNull.Value )
                                {
                                 
                                    connectedObjectId = Guid.Parse(reader["conn_obj_id"].ToString());
                                    connectedObjectTypeId = Convert.ToInt32(reader["conn_obj_type_id"]);
                                    isConnectedObjectPlanning = Convert.ToBoolean(reader["is_conn_obj_planning"]);

                                    connectedObjectData = new Tuple<Guid, ObjectType, bool>(connectedObjectId, this.dataService.GetObjectType(connectedObjectTypeId), isConnectedObjectPlanning);
                                }
                                else
                                    connectedObjectData = null;
                                connData = Convert.ToString(reader["conn_data"]);
                                ignoreStick = Convert.ToBoolean(reader["ignore_stick"]);
                                if (stepNumber % 1000 == 0)
                                {
                                    //connectedObjectData = null;
                                }
                                result.Add(new NodeModel(id, cityId, this.dataService.GetObjectType(typeId), new Point(left, top), connectedObjectData, this.GetConnDataFromString(connData), ignoreStick));
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
        /// Возвращает все объекты из источника данных, входящих в одну сеть с заданным объектом.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        public List<NodeModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            var result = new List<NodeModel>();
            if (!testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            // Составляем список идентификаторов котельных.
            var boilers = "";
            foreach (var id in objectIds)
                boilers += id + ",";
            boilers = boilers.Remove(boilers.Length - 1, 1);

            var query = string.Format("ads.get_nodes{0} @object_ids = {1}, @year = {2}, @user_id = {3}, @city_id = {4}", suffix, boilers, schema.Id, this.loggedUserId, cityId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("ads.get_nodes" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@object_ids", boilers));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        Guid id;
                        int typeId;
                        double left;
                        double top;
                        Guid connectedObjectId;
                        int connectedObjectTypeId;
                        bool isConnectedObjectPlanning;
                        Tuple<Guid, ObjectType, bool> connectedObjectData;
                        string connData;
                        bool ignoreStick;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                left = Convert.ToDouble(reader["left"]);
                                top = Convert.ToDouble(reader["top"]);
                                if (reader["conn_obj_id"] != DBNull.Value)
                                {
                                    connectedObjectId = Guid.Parse(reader["conn_obj_id"].ToString());
                                    connectedObjectTypeId = Convert.ToInt32(reader["conn_obj_type_id"]);
                                    isConnectedObjectPlanning = Convert.ToBoolean(reader["is_conn_obj_planning"]);

                                    connectedObjectData = new Tuple<Guid, ObjectType, bool>(connectedObjectId, this.dataService.GetObjectType(connectedObjectTypeId), isConnectedObjectPlanning);
                                }
                                else
                                    connectedObjectData = null;
                                connData = Convert.ToString(reader["conn_data"]);
                                ignoreStick = Convert.ToBoolean(reader["ignore_stick"]);

                                result.Add(new NodeModel(id, cityId, this.dataService.GetObjectType(typeId), new Point(left, top), connectedObjectData, this.GetConnDataFromString(connData), ignoreStick));
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
        /// Возвращает идентификаторы узлов поворота заданной схемы.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Идентификаторы узлов поворота.</returns>
        public List<Guid> GetBendNodes(SchemaModel schema, int cityId)
        {
            var result = new List<Guid>();
            if (!testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_bend_nodes{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_bend_nodes" + suffix, connection))
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

                        Guid id;
                        
                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());

                                result.Add(id);
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
        /// Возвращает идентификаторы неподключенных узлов заданной схемы.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Идентификаторы неподключенных узлов.</returns>
        public List<Guid> GetFreeNodes(SchemaModel schema, int cityId)
        {
            var result = new List<Guid>();
            if (!testConnection("", true))
                return result;
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_free_nodes{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_free_nodes" + suffix, connection))
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

                        Guid id;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());

                                result.Add(id);
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
        /// Обновляет данные объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateObject(NodeModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_node{0} @id = {1}, @left = {2}, @top = {3}, @conn_obj_id = {4}, @conn_data = {5}, @user_id = {6}, @ignore_stick = {7}, @city_id = {8}, @year = {9}", suffix, obj.Id, obj.Position.X, obj.Position.Y, obj.ConnectedObjectData != null ? obj.ConnectedObjectData.Item1.ToString() : "null", this.GetConnDataAsString(obj.ConnectionData), this.loggedUserId, obj.IgnoreStick, obj.CityId, schema.Id);

            try
            {
                if (localModeFlag)
                {
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("update_node", suffix, false);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@id", obj.Id);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@left", obj.Position.X);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@top", obj.Position.Y);
                    if (obj.ConnectedObjectData != null)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@conn_obj_id", obj.ConnectedObjectData.Item1);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@conn_data", this.GetConnDataAsString(obj.ConnectionData));
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@ignore_stick", obj.IgnoreStick);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    if (!schema.IsIS)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@year", schema.Id);
                }
                else
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_node" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@left", obj.Position.X));
                        command.Parameters.Add(new SqlParameter("@top", obj.Position.Y));
                        if (obj.ConnectedObjectData != null)
                            command.Parameters.Add(new SqlParameter("@conn_obj_id", obj.ConnectedObjectData.Item1));
                        command.Parameters.Add(new SqlParameter("@conn_data", this.GetConnDataAsString(obj.ConnectionData)));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        command.Parameters.Add(new SqlParameter("@ignore_stick", obj.IgnoreStick));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        command.ExecuteNonQuery();
                    }
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