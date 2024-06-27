using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным объектов, представляемых линиями на карте, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlLineAccessService : BaseSqlDataAccessService, ILineAccessService
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
        /// Инициализирует новый экземпляр класса <see cref="SqlLineAccessService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlLineAccessService(SqlDataService dataService, SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedUserId = loggedUserId;
        }

        public void UpdateObjectFromLocal(Guid guid, int index)
        {
            var query = "update_line";
            var id = ObjectModel.DefaultId;
            try
            {

                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(index);
                var suffix = serrializedSqlQuery.getSufix(index);
                if (serializedParameters != null)
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_line" + suffix, connection))
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

    // Реализация ILineAccessService.
    public sealed partial class SqlLineAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Удаляет объект из источника данных.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteObject(LineModel obj, SchemaModel schema)
        {
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
        /// Возвращает все объекты из источника данных, находящиеся в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        public List<LineModel> GetAll(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_lines{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<LineModel>();

            //try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_lines" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        Guid id;
                        Guid groupId;
                        int typeId;
                        bool isPlanning;
                        bool hasChildren;
                        double startX;
                        double startY;
                        double endX;
                        double endY;
                        double length;
                        string points;
                        string name;
                        int diameter;
                        double labelOffset;
                        bool showLabels;
                        string forcedLengths;
                        bool isActive;
                        string temp;

                        var divOuter = new string[1]
                        {
                            ";"
                        };
                        var divInner = new string[1]
                        {
                            " "
                        };

                        string[] tuple;
                        int numberStep = 0;
                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                numberStep++;
                                id = Guid.Parse(reader["id"].ToString());
                                groupId = Guid.Parse(reader["group_id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                hasChildren = Convert.ToBoolean(reader["has_children"]);
                                startX = Convert.ToDouble(reader["start_x"]);
                                startY = Convert.ToDouble(reader["start_y"]);
                                endX = Convert.ToDouble(reader["end_x"]);
                                endY = Convert.ToDouble(reader["end_y"]);
#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
                                //isWorking = Convert.ToBoolean(reader["is_working"]);
                                length = reader["length"] == DBNull.Value?0: Convert.ToDouble(reader["length"]);
                                points = Convert.ToString(reader["points"]);
                                name = Convert.ToString(reader["name"]);
                                if (reader["diam"] != DBNull.Value)
                                    diameter = Convert.ToInt32(reader["diam"]);
                                else
                                    diameter = 0;
                                labelOffset = Convert.ToDouble(reader["label_offset"]);
                                showLabels = Convert.ToBoolean(reader["show_label"]);
                                forcedLengths = Convert.ToString(reader["forced_lengths"]);
                                isActive = Convert.ToBoolean(reader["is_active"]);

                                // Получаем углы поворота надписей линии из строки.
                                temp = Convert.ToString(reader["label_angles"]);
                                var labelAngles = new Dictionary<int, double>();
                                if (!string.IsNullOrEmpty(temp))
                                    foreach (var outer in temp.Split(divOuter, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        tuple = outer.Split(divInner, StringSplitOptions.RemoveEmptyEntries);

                                        labelAngles.Add(Convert.ToInt32(tuple[0]), Convert.ToDouble(tuple[1]));
                                    }

                                // Получаем положения надписей линии из строки.
                                temp = Convert.ToString(reader["label_positions"]);
                                var labelPositions = new Dictionary<int, Point>();
                                if (!string.IsNullOrEmpty(temp))
                                    foreach (var outer in temp.Split(divOuter, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        tuple = outer.Split(divInner, StringSplitOptions.RemoveEmptyEntries);

                                        labelPositions.Add(Convert.ToInt32(tuple[0]), new Point(Convert.ToDouble(tuple[1]), Convert.ToDouble(tuple[2])));
                                    }

                                // Получаем размеры надписей линии из строки.
                                temp = Convert.ToString(reader["label_sizes"]);
                                var labelSizes = new Dictionary<int, int>();
                                if (!string.IsNullOrEmpty(temp))
                                    foreach (var outer in temp.Split(divOuter, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        tuple = outer.Split(divInner, StringSplitOptions.RemoveEmptyEntries);

                                        labelSizes.Add(Convert.ToInt32(tuple[0]), Convert.ToInt32(tuple[1]));
                                    }
                                if (numberStep % 1000 == 0)
                                {
                                    int v = numberStep % 1000;
                                }
                                result.Add(new LineModel(id, groupId, cityId, this.dataService.GetObjectType(typeId), isPlanning, hasChildren, new Point(startX, startY), new Point(endX, endY), length, points, labelAngles, labelPositions, labelSizes, name, diameter, labelOffset, showLabels, forcedLengths, isActive));
                            }
                    }
            }
            /*
            catch (Exception e)
            {
                throw new Exception(query, e);
            }*/

            return result;
        }

        /// <summary>
        /// Возвращает все объекты из источника данных, входящих в одну сеть с заданным объектом.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов.</returns>
        public List<LineModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            // Составляем список идентификаторов котельных.
            var boilers = "";
            foreach (var id in objectIds)
                boilers += id + ",";
            boilers = boilers.Remove(boilers.Length - 1, 1);

            var query = string.Format("ads.get_lines{0} @object_ids = {1}, @year = {2}, @user_id = {3}, @city_id = {4}", suffix, boilers, schema.Id, this.loggedUserId, cityId);

            var result = new List<LineModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("ads.get_lines" + suffix, connection))
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
                        Guid groupId;
                        int typeId;
                        bool isPlanning;
                        bool hasChildren;
                        double startX;
                        double startY;
                        double endX;
                        double endY;
                        double length;
                        string points;
                        string name;
                        int diameter;
                        double labelOffset;
                        bool showLabels;
                        string forcedLengths;
                        bool isActive;
                        string temp;

                        var divOuter = new string[1]
                        {
                            ";"
                        };
                        var divInner = new string[1]
                        {
                            " "
                        };

                        string[] tuple;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                groupId = Guid.Parse(reader["group_id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);
                                hasChildren = Convert.ToBoolean(reader["has_children"]);
                                startX = Convert.ToDouble(reader["start_x"]);
                                startY = Convert.ToDouble(reader["start_y"]);
                                endX = Convert.ToDouble(reader["end_x"]);
                                endY = Convert.ToDouble(reader["end_y"]);
#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
                                //isWorking = Convert.ToBoolean(reader["is_working"]);
                                length = Convert.ToDouble(reader["length"]);
                                points = Convert.ToString(reader["points"]);
                                name = Convert.ToString(reader["name"]);
                                if (reader["diam"] != DBNull.Value)
                                    diameter = Convert.ToInt32(reader["diam"]);
                                else
                                    diameter = 0;
                                labelOffset = Convert.ToDouble(reader["label_offset"]);
                                showLabels = Convert.ToBoolean(reader["show_label"]);
                                forcedLengths = Convert.ToString(reader["forced_lengths"]);
                                isActive = Convert.ToBoolean(reader["is_active"]);

                                // Получаем углы поворота надписей линии из строки.
                                temp = Convert.ToString(reader["label_angles"]);
                                var labelAngles = new Dictionary<int, double>();
                                if (!string.IsNullOrEmpty(temp))
                                    foreach (var outer in temp.Split(divOuter, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        tuple = outer.Split(divInner, StringSplitOptions.RemoveEmptyEntries);

                                        labelAngles.Add(Convert.ToInt32(tuple[0]), Convert.ToDouble(tuple[1]));
                                    }

                                // Получаем положения надписей линии из строки.
                                temp = Convert.ToString(reader["label_positions"]);
                                var labelPositions = new Dictionary<int, Point>();
                                if (!string.IsNullOrEmpty(temp))
                                    foreach (var outer in temp.Split(divOuter, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        tuple = outer.Split(divInner, StringSplitOptions.RemoveEmptyEntries);

                                        labelPositions.Add(Convert.ToInt32(tuple[0]), new Point(Convert.ToDouble(tuple[1]), Convert.ToDouble(tuple[2])));
                                    }

                                // Получаем размеры надписей линии из строки.
                                temp = Convert.ToString(reader["label_sizes"]);
                                var labelSizes = new Dictionary<int, int>();
                                if (!string.IsNullOrEmpty(temp))
                                    foreach (var outer in temp.Split(divOuter, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        tuple = outer.Split(divInner, StringSplitOptions.RemoveEmptyEntries);

                                        labelSizes.Add(Convert.ToInt32(tuple[0]), Convert.ToInt32(tuple[1]));
                                    }

                                result.Add(new LineModel(id, groupId, cityId, this.dataService.GetObjectType(typeId), isPlanning, hasChildren, new Point(startX, startY), new Point(endX, endY), length, points, labelAngles, labelPositions, labelSizes, name, diameter, labelOffset, showLabels, forcedLengths, isActive));
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
        /// <returns>Список тюплов, в которых содержится информация об идентификаторах объектов, групп, их типах и планируемости.</returns>
        public List<Tuple<Guid, Guid, ObjectType, bool>> GetAllFast(int cityId, Guid objectId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("ads.get_lines{0} @object_ids = {1}, @fast = {2}, @year = {3}, @user_id = {4}, @city_id = {5}", suffix, objectId, true, schema.Id, this.loggedUserId, cityId);

            var result = new List<Tuple<Guid, Guid, ObjectType, bool>>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("ads.get_lines" + suffix, connection))
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
                        Guid groupId;
                        int typeId;
                        bool isPlanning;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                groupId = Guid.Parse(reader["group_id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                isPlanning = Convert.ToBoolean(reader["is_planning"]);

                                result.Add(new Tuple<Guid, Guid, ObjectType, bool>(id, groupId, this.dataService.GetObjectType(typeId), isPlanning));
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
        /// Возвращает список идентификаторов линий, принадлежащих слою гидравлики.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов линий.</returns>
        public List<Guid> GetHydraulicsLines(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_hydraulics_lines{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<Guid>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_hydraulics_lines" + suffix, connection))
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
        /// Возвращает список идентификаторов линий, принадлежащих слою гидравлики.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов линий.</returns>
        public List<Guid> GetHydraulicsErrorLines(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_hydraulics_error_lines{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<Guid>();
            
            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_hydraulics_error_lines" + suffix, connection))
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
                //throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список идентификаторов линий заданного года.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="year">Год.</param>
        /// <returns>Список идентификаторов линий прошедшего года.</returns>
        public List<Guid> GetLinesByYear(int cityId, SchemaModel schema, int year)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_lines_by_year{0} @year = {1}, @city_id = {2}, @lines_year = {3}, @user_id = {4}", suffix, schema.Id, cityId, year, this.loggedUserId);

            var result = new List<Guid>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_lines_by_year" + suffix, connection))
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
                        command.Parameters.Add(new SqlParameter("@line_year", year));

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
        /// Возвращает список идентификаторов линий, учавствующих в ремонтной программе.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся линии.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов линий.</returns>
        public List<Guid> GetRP(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_rp_lines{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<Guid>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_rp_lines" + suffix, connection))
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
        public void MarkDeleteObject(LineModel obj, SchemaModel schema)
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
        /// Обновляет данные объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateObject(LineModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_line{0} @id = {1}, @group_id = {2} @start_x = {3}, @start_y = {4}, @end_x = {5}, @end_y = {6}, @label_angles = {7}, @label_positions = {8}, @label_sizes = {9}, @user_id = {10}, @city_id = {11}, @year = {12}, @points = {13}, @label_offset = {14}, @show_label = {15}, @forced_lengths = {16}", suffix, obj.Id, obj.GroupId, obj.StartPoint.X, obj.StartPoint.Y, obj.EndPoint.X, obj.EndPoint.Y, obj.LabelAngles.ToString(), obj.LabelPositions.ToString(), obj.LabelSizes.ToString(), this.loggedUserId, obj.CityId, schema.Id, obj.Points, obj.LabelOffset, obj.ShowLabels, obj.ForcedLengths);

            try
            {

                if (localModeFlag)
                {
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("update_line", suffix, false);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@id", obj.Id);

                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@group_id", obj.GroupId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@start_x", obj.StartPoint.X);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@start_y", obj.StartPoint.Y);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@end_x", obj.EndPoint.X);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@end_y", obj.EndPoint.Y);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    if (!schema.IsIS)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@year", schema.Id);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@points", obj.Points);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_offset", obj.LabelOffset);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@show_label", obj.ShowLabels);
                    if (string.IsNullOrEmpty(obj.ForcedLengths))
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@forced_lengths", DBNull.Value);
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@forced_lengths", obj.ForcedLengths);

                    // Составляем строку углов поворота надписей.
                    var labelAngles = "";
                    foreach (var entry in obj.LabelAngles)
                        labelAngles += entry.Key.ToString() + " " + Math.Round(entry.Value, 6).ToString() + ";";
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_angles", labelAngles);

                    // Составляем строку положений надписей.
                    var labelPositions = "";
                    foreach (var entry in obj.LabelPositions)
                        labelPositions += entry.Key.ToString() + " " + Math.Round(entry.Value.X, 6).ToString() + " " + Math.Round(entry.Value.Y, 6).ToString() + ";";
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_positions", labelPositions);

                    // Составляем строку размеров надписей.
                    var labelSizes = "";
                    foreach (var entry in obj.LabelSizes)
                        labelSizes += entry.Key.ToString() + " " + entry.Value.ToString() + ";";
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@label_sizes", labelSizes);
                }
                else
                {

                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_line" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@group_id", obj.GroupId));
                        command.Parameters.Add(new SqlParameter("@start_x", obj.StartPoint.X));
                        command.Parameters.Add(new SqlParameter("@start_y", obj.StartPoint.Y));
                        command.Parameters.Add(new SqlParameter("@end_x", obj.EndPoint.X));
                        command.Parameters.Add(new SqlParameter("@end_y", obj.EndPoint.Y));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@points", obj.Points));
                        command.Parameters.Add(new SqlParameter("@label_offset", obj.LabelOffset));
                        command.Parameters.Add(new SqlParameter("@show_label", obj.ShowLabels));
                        if (string.IsNullOrEmpty(obj.ForcedLengths))
                            command.Parameters.Add(new SqlParameter("@forced_lengths", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@forced_lengths", obj.ForcedLengths));

                        // Составляем строку углов поворота надписей.
                        var labelAngles = "";
                        foreach (var entry in obj.LabelAngles)
                            labelAngles += entry.Key.ToString() + " " + Math.Round(entry.Value, 6).ToString() + ";";
                        command.Parameters.Add(new SqlParameter("@label_angles", labelAngles));

                        // Составляем строку положений надписей.
                        var labelPositions = "";
                        foreach (var entry in obj.LabelPositions)
                            labelPositions += entry.Key.ToString() + " " + Math.Round(entry.Value.X, 6).ToString() + " " + Math.Round(entry.Value.Y, 6).ToString() + ";";
                        command.Parameters.Add(new SqlParameter("@label_positions", labelPositions));

                        // Составляем строку размеров надписей.
                        var labelSizes = "";
                        foreach (var entry in obj.LabelSizes)
                            labelSizes += entry.Key.ToString() + " " + entry.Value.ToString() + ";";
                        command.Parameters.Add(new SqlParameter("@label_sizes", labelSizes));

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