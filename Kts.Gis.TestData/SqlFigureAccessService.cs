using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным объектов, представляемых фигурами на карте, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlFigureAccessService : BaseSqlDataAccessService, IFigureAccessService
    {
        #region Закрытые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private SqlDataService dataService;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlFigureAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlFigureAccessService(SqlDataService dataService, SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация IFigureAccessService.
    public sealed partial class SqlFigureAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Удаляет объект из источника данных.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteObject(FigureModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, true);

            try
            {
                using (var connection = Connector.GetConnection())
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
        /// Возвращает все объекты из источника данных, находящиеся в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        public List<FigureModel> GetAll(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_figures{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<FigureModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_figures" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        Guid id;
                        int typeId;
                        bool isPlanning;
                        bool hasChildren;
                        string name;
                        int figureTypeId;
                        double width;
                        double height;
                        double left;
                        double top;
                        double angle;
                        string points;
                        double? labelAngle;
                        double labelLeft;
                        double labelTop;
                        int? labelSize;
                        bool isActive;
                        string typeIds;

                        FigureModel figure;

                        Point labelPosition;

                        List<ObjectType> childrenTypes;
                        
                        var div = new string[1]
                        {
                            ","
                        };

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                childrenTypes = new List<ObjectType>();

                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                hasChildren = Convert.ToBoolean(reader["has_children"]);
                                name = Convert.ToString(reader["name"]);
                                isActive = Convert.ToBoolean(reader["is_active"]);
                                typeIds = Convert.ToString(reader["type_ids"]);
                                if (!string.IsNullOrEmpty(typeIds))
                                    foreach (var t in typeIds.Split(div, StringSplitOptions.RemoveEmptyEntries))
                                        childrenTypes.Add(this.dataService.GetObjectType(Convert.ToInt32(t)));
                                if (reader["figure_type_id"] != DBNull.Value)
                                {
                                    figureTypeId = Convert.ToInt32(reader["figure_type_id"]);
                                    width = Convert.ToDouble(reader["width"]);
                                    height = Convert.ToDouble(reader["height"]);
                                    left = Convert.ToDouble(reader["left"]);
                                    top = Convert.ToDouble(reader["top"]);
                                    angle = Convert.ToDouble(reader["angle"]);
                                    points = Convert.ToString(reader["points"]);
                                    if (reader["label_left"] != DBNull.Value)
                                    {
                                        labelLeft = Convert.ToDouble(reader["label_left"]);
                                        labelTop = Convert.ToDouble(reader["label_top"]);

                                        labelPosition = new Point(labelLeft, labelTop);
                                    }
                                    else
                                        labelPosition = null;
                                    if (reader["label_angle"] != DBNull.Value)
                                        labelAngle = Convert.ToDouble(reader["label_angle"]);
                                    else
                                        labelAngle = null;
                                    if (reader["label_size"] != DBNull.Value)
                                        labelSize = Convert.ToInt32(reader["label_size"]);
                                    else
                                        labelSize = null;

                                    figure = new FigureModel(id, cityId, this.dataService.GetObjectType(typeId), isPlanning, hasChildren, name, (FigureType)figureTypeId, new Size(width, height), new Point(left, top), angle, points, labelAngle, labelPosition, labelSize, isActive, childrenTypes);
                                }
                                else
                                    figure = new FigureModel(id, cityId, this.dataService.GetObjectType(typeId), isPlanning, hasChildren, name, isActive, childrenTypes);

                                result.Add(figure);
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
        public List<FigureModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            // Составляем список идентификаторов котельных.
            var boilers = "";
            foreach (var id in objectIds)
                boilers += id + ",";
            boilers = boilers.Remove(boilers.Length - 1, 1);

            var query = string.Format("ads.get_figures{0} @object_ids = {1}, @year = {2}, @city_id = {3}, @user_id = {4}", suffix, boilers, schema.Id, cityId, this.loggedUserId);

            var result = new List<FigureModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("ads.get_figures" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@object_ids", boilers));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        Guid id;
                        int typeId;
                        bool isPlanning;
                        bool hasChildren;
                        string name;
                        int figureTypeId;
                        double width;
                        double height;
                        double left;
                        double top;
                        double angle;
                        string points;
                        double? labelAngle;
                        double labelLeft;
                        double labelTop;
                        int? labelSize;
                        bool isActive;
                        string typeIds;

                        FigureModel figure;

                        Point labelPosition;

                        List<ObjectType> childrenTypes;

                        var div = new string[1]
                        {
                            ","
                        };

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                childrenTypes = new List<ObjectType>();

                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                hasChildren = Convert.ToBoolean(reader["has_children"]);
                                name = Convert.ToString(reader["name"]);
                                isActive = Convert.ToBoolean(reader["is_active"]);
                                typeIds = Convert.ToString(reader["type_ids"]);
                                if (!string.IsNullOrEmpty(typeIds))
                                    foreach (var t in typeIds.Split(div, StringSplitOptions.RemoveEmptyEntries))
                                        childrenTypes.Add(this.dataService.GetObjectType(Convert.ToInt32(t)));
                                if (reader["figure_type_id"] != DBNull.Value)
                                {
                                    figureTypeId = Convert.ToInt32(reader["figure_type_id"]);
                                    width = Convert.ToDouble(reader["width"]);
                                    height = Convert.ToDouble(reader["height"]);
                                    left = Convert.ToDouble(reader["left"]);
                                    top = Convert.ToDouble(reader["top"]);
                                    angle = Convert.ToDouble(reader["angle"]);
                                    points = Convert.ToString(reader["points"]);
                                    if (reader["label_left"] != DBNull.Value)
                                    {
                                        labelLeft = Convert.ToDouble(reader["label_left"]);
                                        labelTop = Convert.ToDouble(reader["label_top"]);

                                        labelPosition = new Point(labelLeft, labelTop);
                                    }
                                    else
                                        labelPosition = null;
                                    if (reader["label_angle"] != DBNull.Value)
                                        labelAngle = Convert.ToDouble(reader["label_angle"]);
                                    else
                                        labelAngle = null;
                                    if (reader["label_size"] != DBNull.Value)
                                        labelSize = Convert.ToInt32(reader["label_size"]);
                                    else
                                        labelSize = null;

                                    figure = new FigureModel(id, cityId, this.dataService.GetObjectType(typeId), isPlanning, hasChildren, name, (FigureType)figureTypeId, new Size(width, height), new Point(left, top), angle, points, labelAngle, labelPosition, labelSize, isActive, childrenTypes);
                                }
                                else
                                    figure = new FigureModel(id, cityId, this.dataService.GetObjectType(typeId), isPlanning, hasChildren, name, isActive, childrenTypes);

                                result.Add(figure);
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
        /// Возвращает все объекты из источника данных, входящих в одну сеть с заданным объектом, с минимальным набором данных.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список тюплов, в которых содержится информация об идентификаторах объектов, их типах и планируемости.</returns>
        public List<Tuple<Guid, ObjectType, bool>> GetAllFast(int cityId, Guid objectId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("ads.get_figures{0} @object_ids = {1}, @fast = {2}, @year = {3}, @city_id = {4}, @user_id = {5}", suffix, objectId, true, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<Guid, ObjectType, bool>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("ads.get_figures" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@object_ids", objectId.ToString()));
                        command.Parameters.Add(new SqlParameter("@fast", true));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        Guid id;
                        int typeId;
                        bool isPlanning;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);

                                result.Add(new Tuple<Guid, ObjectType, bool>(id, this.dataService.GetObjectType(typeId), isPlanning));
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
        /// Возвращает идентификатор котельной, к которой подключен объект. Если объект сам является котельной, то возвращается его идентификатор.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор котельной, к которой подключен объект. Если объект сам является котельной, то возвращается его идентификатор.</returns>
        public Guid GetBoilerId(ObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_boiler_id{0} @id = {1}, @year = {2}, @user_id = {3}", suffix, obj.Id, schema.Id, this.loggedUserId);

            var result = Guid.Empty;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_boiler_id" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }

                        Guid.TryParse(Convert.ToString(command.ExecuteScalar()), out result);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список объектов программы "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<long, string>> GetJurObjects(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_jur_objects{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<long, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_jur_objects" + suffix, connection))
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
                                result.Add(new Tuple<long, string>(Convert.ToInt64(reader["id"]), reader["name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }
        public List<string> getTrashList(int cityID, string trashStorageID)
        {
            List<string> result = new List<string>();

            /*
             select * from General..tValues  where id_param = 403 and date_po is null

and CONVERT(varchar(255),value) = '9A29118C-2E30-4766-B01F-CC6A053A5063'
             */

            var query = @"select id_object, value 
                                       from General..tValues, 
                                            General..tObjects 
                                       where 
                                            id_param = 403 and 
                                            id_city=@cityID and 
                                            date_po is null and
                                            tValues.id_object = tObjects.id and
                                            CONVERT(varchar(255),value) = @trashStorageID";
            try
            {
                using (var command = new SqlCommand(query, this.Connector.GetConnection()))
                {
                    command.Parameters.AddWithValue("@cityID", cityID);
                    command.Parameters.AddWithValue("@trashStorageID", trashStorageID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string id_object = Convert.ToString(reader["id_object"]);
                                result.Add(id_object);
                            }
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
        /// Возвращает список объектов программы "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<long, string>> GetKvpObjects(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_kvp_objects{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<long, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_kvp_objects" + suffix, connection))
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
                                result.Add(new Tuple<long, string>(Convert.ToInt64(reader["id"]), reader["name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список объектов с интеграцией с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<Guid, Guid, string, string>> GetObjectsWithJurIntegration(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_objects_with_jur{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<Guid, Guid, string, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_objects_with_jur" + suffix, connection))
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
                                result.Add(new Tuple<Guid, Guid, string, string>(Guid.Parse(reader["id"].ToString()), reader["parent_id"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["parent_id"].ToString()), reader["name"].ToString(), reader["jur_name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список объектов с интеграцией с программой "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список объектов.</returns>
        public List<Tuple<Guid, Guid, string, string>> GetObjectsWithKvpIntegration(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_objects_with_kvp{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<Guid, Guid, string, string>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_objects_with_kvp" + suffix, connection))
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
                                result.Add(new Tuple<Guid, Guid, string, string>(Guid.Parse(reader["id"].ToString()), reader["parent_id"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["parent_id"].ToString()), reader["name"].ToString(), reader["kvp_name"].ToString()));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список идентификаторов объектов без интеграции с программой "Учет потребления топлива".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список идентификаторов.</returns>
        public List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutFuelIntegration(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_objects_without_fuel{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<Guid, Guid, string, Guid>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_objects_without_fuel" + suffix, connection))
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
                                result.Add(new Tuple<Guid, Guid, string, Guid>(Guid.Parse(reader["id"].ToString()), reader["parent_id"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["parent_id"].ToString()), reader["name"].ToString(), Guid.Empty));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список идентификаторов объектов без интеграции с программой "Расчеты с юридическими лицами".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список идентификаторов.</returns>
        public List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutJurIntegration(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_objects_without_jur{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<Guid, Guid, string, Guid>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_objects_without_jur" + suffix, connection))
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
                                result.Add(new Tuple<Guid, Guid, string, Guid>(Guid.Parse(reader["id"].ToString()), reader["parent_id"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["parent_id"].ToString()), reader["name"].ToString(), reader["boiler_id"] != DBNull.Value ? Guid.Parse(reader["boiler_id"].ToString()) : Guid.Empty));
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список идентификаторов объектов без интеграции с программой "Квартплата".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список идентификаторов.</returns>
        public List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutKvpIntegration(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_objects_without_kvp{0} @year = {1}, @city_id = {2}, @user_id = {3}", suffix, schema.Id, cityId, this.loggedUserId);

            var result = new List<Tuple<Guid, Guid, string, Guid>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_objects_without_kvp" + suffix, connection))
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
                        {
                            Guid id = Guid.Parse(reader["id"].ToString());
                            Guid parent_id = reader["parent_id"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["parent_id"].ToString());
                            string name = reader["name"].ToString();
                            Guid boiler_id = reader["boiler_id"] != DBNull.Value ? Guid.Parse(reader["boiler_id"].ToString()) : Guid.Empty;

                            result.Add(new Tuple<Guid, Guid, string, Guid>(id, parent_id, name, boiler_id));
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
        /// Возвращает список идентификаторов фигур, представляющих несопоставленные объекты.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся фигуры.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов фигур.</returns>
        public List<Guid> GetUO(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_uo{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<Guid>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_uo" + suffix, connection))
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
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        public void MarkDeleteObject(FigureModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, false);

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

        /// <summary>
        /// Сбрасывает идентификатор из программы "Расчеты с юридическими лицами" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        public void ResetJurId(Guid gisId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("reset_jur_id{0} @gis_id = {1}, @user_id = {2}, @year = {3}", suffix, gisId, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("reset_jur_id" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@gis_id", gisId));
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
        /// Сбрасывает идентификатор из программы "Квартплата" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        public void ResetKvpId(Guid gisId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("reset_kvp_id{0} @gis_id = {1}, @user_id = {2}, @year = {3}", suffix, gisId, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("reset_kvp_id" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@gis_id", gisId));
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
        /// Задает идентификатор из программы "Расчеты с юридическими лицами" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="jurId">Идентификатор из программы "Расчеты с юридическими лицами".</param>
        /// <param name="schema">Схема.</param>
        public void SetJurId(Guid gisId, long jurId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("set_jur_id{0} @gis_id = {1}, @jur_id = {2}, @user_id = {3}, @year = {4}", suffix, gisId, jurId, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("set_jur_id" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@gis_id", gisId));
                        command.Parameters.Add(new SqlParameter("@jur_id", jurId));
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
        /// Задает идентификатор из программы "Квартплата" объекту с заданным идентификатором.
        /// </summary>
        /// <param name="gisId">Идентификатор объекта.</param>
        /// <param name="jurId">Идентификатор из программы "Квартплата".</param>
        /// <param name="schema">Схема.</param>
        public void SetKvpId(Guid gisId, int kvpId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("set_kvp_id{0} @gis_id = {1}, @kvp_id = {2}, @user_id = {3}, @year = {4}", suffix, gisId, kvpId, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("set_kvp_id" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@gis_id", gisId));
                        command.Parameters.Add(new SqlParameter("@kvp_id", kvpId));
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

        public void UpdateObjectFromLocal(Guid guid, int index)
        {
            var query = "update_figure";
            var id = ObjectModel.DefaultId;
            try
            {

                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(index);
                var suffix = serrializedSqlQuery.getSufix(index);
                if (serializedParameters != null)
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_figure" + suffix, connection))
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

        /// <summary>
        /// Обновляет данные объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateObject(FigureModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_figure{0} @id = {1}, @figure_type_id = {2}, @width = {3}, @height = {4}, @left = {5}, @top = {6}, @angle = {7}, @points = {8}, @label_left = {9}, @label_top = {10}, @label_angle = {11}, @label_size = {12}, @user_id = {13}, @year = {14}", suffix, obj.Id, (int)obj.FigureType, obj.Size.Width, obj.Size.Height, obj.Position.X, obj.Position.Y, obj.Angle, obj.Points, obj.LabelPosition != null ? obj.LabelPosition.X.ToString() : "null", obj.LabelPosition != null ? obj.LabelPosition.Y.ToString() : "null", obj.LabelAngle != null ? obj.LabelAngle.ToString() : "null", obj.LabelSize != null ? obj.LabelSize.ToString() : "null", this.loggedUserId, schema.Id);

            try
            {
                if (localModeFlag)
                {
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("update_figure", suffix, false);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@id", obj.Id);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@figure_type_id", (int)obj.FigureType);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@width", obj.Size.Width);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@height", obj.Size.Height);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@left", obj.Position.X);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@top", obj.Position.Y);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@angle", obj.Angle);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@points", obj.Points);
                    if (obj.LabelPosition != null)
                    {
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_left", obj.LabelPosition.X);
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_top", obj.LabelPosition.Y);
                    }
                    else
                    {
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_left", DBNull.Value);
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_top", DBNull.Value);
                    }

                    if (obj.LabelAngle.HasValue)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_angle", obj.LabelAngle.Value);
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_angle", DBNull.Value);

                    if (obj.LabelSize.HasValue)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_size", obj.LabelSize.Value);
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_size", DBNull.Value);

                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    if (!schema.IsIS)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@year", schema.Id);

                }
                else
                {

                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_figure" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@figure_type_id", (int)obj.FigureType));
                        command.Parameters.Add(new SqlParameter("@width", obj.Size.Width));
                        command.Parameters.Add(new SqlParameter("@height", obj.Size.Height));
                        command.Parameters.Add(new SqlParameter("@left", obj.Position.X));
                        command.Parameters.Add(new SqlParameter("@top", obj.Position.Y));
                        command.Parameters.Add(new SqlParameter("@angle", obj.Angle));
                        command.Parameters.Add(new SqlParameter("@points", obj.Points));
                        if (obj.LabelPosition != null)
                        {
                            command.Parameters.Add(new SqlParameter("@label_left", obj.LabelPosition.X));
                            command.Parameters.Add(new SqlParameter("@label_top", obj.LabelPosition.Y));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@label_left", DBNull.Value));
                            command.Parameters.Add(new SqlParameter("@label_top", DBNull.Value));
                        }
                        if (obj.LabelAngle.HasValue)
                            command.Parameters.Add(new SqlParameter("@label_angle", obj.LabelAngle.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@label_angle", DBNull.Value));
                        if (obj.LabelSize.HasValue)
                            command.Parameters.Add(new SqlParameter("@label_size", obj.LabelSize.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@label_size", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
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