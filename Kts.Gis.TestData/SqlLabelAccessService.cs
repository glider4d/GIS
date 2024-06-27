using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным надписей, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlLabelAccessService : BaseSqlDataAccessService, ILabelAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlLabelAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlLabelAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
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
                    using (var command = new SqlCommand("update_label" + suffix, connection))
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

    // Реализация ILabelAccessService.
    public sealed partial class SqlLabelAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Удаляет объект из источника данных.
        /// </summary>
        /// <param name="obj">Удаляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteObject(LabelModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_label{0} @id = {1}, @user_id = {2}, @year = {3}", suffix, obj.Id, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("remove_label" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
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
        public List<LabelModel> GetAll(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_labels{0} @city_id = {1}, @year = {2}", suffix, cityId, schema.Id);

            var result = new List<LabelModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_labels" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        Guid id;
                        string content;
                        double left;
                        double top;
                        int size;
                        double angle;
                        bool isBold;
                        bool isItalic;
                        bool isUnderline;
                        Guid layerId;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                content = Convert.ToString(reader["content"]);
                                left = Convert.ToDouble(reader["left"]);
                                top = Convert.ToDouble(reader["top"]);
                                size = Convert.ToInt32(reader["size"]);
                                angle = Convert.ToDouble(reader["angle"]);
                                isBold = Convert.ToBoolean(reader["is_bold"]);
                                isItalic = Convert.ToBoolean(reader["is_italic"]);
                                isUnderline = Convert.ToBoolean(reader["is_underline"]);
                                if (reader["layer_id"] != DBNull.Value)
                                    layerId = Guid.Parse(reader["layer_id"].ToString());
                                else
                                    layerId = Guid.Empty;

                                result.Add(new LabelModel(id, content, new Point(left, top), size, angle, isBold, isItalic, isUnderline, cityId, layerId));
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
        public List<LabelModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            // Надписи не привязаны к объектам, поэтому возвращаем все надписи.
            return this.GetAll(cityId, schema);
        }

        /// <summary>
        /// Помечает объект на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Объект, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        public void MarkDeleteObject(LabelModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_object{0} @id = {1}, @user_id = {2}, @schema_id = {3}, @right_now = {4}", suffix, obj.Id, this.loggedUserId, schema.Id, false);

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

        public Guid UpdateNewObjectParamValuesFromLocal2(int indexForSerialized)
        {
            var query = "update_obj_param_values_label_from_local";
            var id = ObjectModel.DefaultId;
            try
            {

                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(indexForSerialized);
                var suffix = serrializedSqlQuery.getSufix(indexForSerialized);
                if (serializedParameters != null)
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_label" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        foreach (var dictionaryItem in serializedParameters)
                        {

                            if (dictionaryItem.Key.Equals("@object_id"))
                            {
                                command.Parameters.Add(new SqlParameter(dictionaryItem.Key, DBNull.Value));
                            }
                            else
                            {
                                if (dictionaryItem.Value == DBNull.Value)
                                {
                                    command.Parameters.Add(new SqlParameter(dictionaryItem.Key, DBNull.Value));
                                }
                                else
                                {
                                    command.Parameters.Add(new SqlParameter(dictionaryItem.Key, dictionaryItem.Value));
                                }
                                
                            }

                        }
                        string textScalar = command.ExecuteScalar().ToString();
                        id = Guid.Parse(textScalar);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
            return id;
        }


        public Guid UpdateNewObjectParamValuesFromLocal(int indexForSerialized)
        {
            LabelModel labelModel = null;
            SchemaModel schemaModel = null;
            try
            {
                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(indexForSerialized);
                var suffix = serrializedSqlQuery.getSufix(indexForSerialized);
                if (serializedParameters != null)
                {
                   
                    string labelModelContent = "";
                    if (serializedParameters.ContainsKey("@content"))
                        labelModelContent = (string)serializedParameters["@content"];
                    double x = 0;
                    if (serializedParameters.ContainsKey("@left"))
                        x = (double)serializedParameters["@left"];
                    double y = 0;
                    if (serializedParameters.ContainsKey("@top"))
                        y = (double)serializedParameters["@top"];
                    int size = 0;
                    if (serializedParameters.ContainsKey("@size"))
                        size = (int)serializedParameters["@size"];
                    double angle = 0;
                    if (serializedParameters.ContainsKey("@angle"))
                        angle = (double)serializedParameters["@angle"];
                    bool isBold = false;
                    if (serializedParameters.ContainsKey("@is_bold"))
                        isBold = (bool)serializedParameters["@is_bold"];

                    bool is_italic = false;
                    if (serializedParameters.ContainsKey("@is_italic"))
                    {
                        is_italic = (bool)serializedParameters["@is_italic"];
                    }
                    bool IsUnderline = false;
                    if (serializedParameters.ContainsKey("@is_underline"))
                    {
                        IsUnderline = (bool)serializedParameters["@is_underline"];
                    }
                    int city_id = 0;

                    if (serializedParameters.ContainsKey("@city_id"))
                    {
                        city_id = (int)serializedParameters["@city_id"];
                    }
                    int user_id = 0;
                    if (serializedParameters.ContainsKey("@user_id"))
                    {
                        user_id = (int)serializedParameters["@user_id"];
                    }
                    int year = 0;
                    if (serializedParameters.ContainsKey("@year"))
                    {
                        year = (int)serializedParameters["@year"];
                    }
                    Object layer_id = Guid.Empty;
                    if (serializedParameters.ContainsKey("@layer_id"))
                    {
                        layer_id = (object)serializedParameters["@layer_id"];
                        if (layer_id == null || layer_id == DBNull.Value)
                            layer_id = Guid.Empty;
                    }
                    labelModel = new LabelModel(labelModelContent, new Point(x, y), size, angle, city_id, (Guid) layer_id);
                    bool isShema = true;
                    if (year > 0) isShema = false;



                     schemaModel = new SchemaModel(year, "", true, isShema);

                    // labelModel = new LabelModel()
                }
            }
            catch
            {

            }
            return UpdateNewObject(labelModel, schemaModel);
        }

        /// <summary>
        /// Обновляет данные нового объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор нового объекта.</returns>
        public Guid UpdateNewObject(LabelModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_label{0} @id = null, @content = {1}, @left = {2}, @top = {3}, @size = {4}, @angle = {5}, @is_bold = {6}, @is_italic = {7}, @is_underline = {8}, @city_id = {9}, @user_id = {10}, @year = {11}, @layer_id = {12}", suffix, obj.Content, obj.Position.X, obj.Position.Y, obj.Size, obj.Angle, obj.IsBold, obj.IsItalic, obj.IsUnderline, obj.CityId, this.loggedUserId, schema.Id, obj.LayerId);

            var id = Guid.Empty;

            try
            {
                if (localModeFlag)
                {
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("UpdateNewObjectParamValuesLabel", suffix, false);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@object_id", DBNull.Value, false);
                    serrializedSqlQuery.addObjectParamValue("@content", obj.Content);
                    serrializedSqlQuery.addObjectParamValue("@left", obj.Position.X);
                    serrializedSqlQuery.addObjectParamValue("@top", obj.Position.Y);
                    serrializedSqlQuery.addObjectParamValue("@size", obj.Size);
                    serrializedSqlQuery.addObjectParamValue("@angle", obj.Angle);
                    serrializedSqlQuery.addObjectParamValue("@is_bold", obj.IsBold);
                    serrializedSqlQuery.addObjectParamValue("@is_italic", obj.IsItalic);
                    serrializedSqlQuery.addObjectParamValue("@is_underline", obj.IsUnderline);
                    serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    if (!schema.IsIS)
                        serrializedSqlQuery.addObjectParamValue("@year", schema.Id);
                    if (obj.LayerId != null && obj.LayerId != Guid.Empty)
                        serrializedSqlQuery.addObjectParamValue("@layer_id", obj.LayerId);
                    else
                        serrializedSqlQuery.addObjectParamValue("@layer_id", DBNull.Value);

                }
                else
                {

                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_label" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@content", obj.Content));
                        command.Parameters.Add(new SqlParameter("@left", obj.Position.X));
                        command.Parameters.Add(new SqlParameter("@top", obj.Position.Y));
                        command.Parameters.Add(new SqlParameter("@size", obj.Size));
                        command.Parameters.Add(new SqlParameter("@angle", obj.Angle));
                        command.Parameters.Add(new SqlParameter("@is_bold", obj.IsBold));
                        command.Parameters.Add(new SqlParameter("@is_italic", obj.IsItalic));
                        command.Parameters.Add(new SqlParameter("@is_underline", obj.IsUnderline));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        if (obj.LayerId != Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@layer_id", obj.LayerId));
                        else
                            command.Parameters.Add(new SqlParameter("@layer_id", DBNull.Value));

                        id = Guid.Parse(command.ExecuteScalar().ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return id;
        }

        /// <summary>
        /// Обновляет данные объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateObject(LabelModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_label{0} @id = {1}, @content = {2}, @left = {3}, @top = {4}, @size = {5}, @angle = {6}, @is_bold = {7}, @is_italic = {8}, @is_underline = {9}, @city_id = {10}, @user_id = {11}, @year = {12}, @layer_id = {13}", suffix, obj.IsSaved ? obj.Id.ToString() : "null", obj.Content, obj.Position.X, obj.Position.Y, obj.Size, obj.Angle, obj.IsBold, obj.IsItalic, obj.IsUnderline, obj.CityId, this.loggedUserId, schema.Id, obj.LayerId);

            try
            {
                if (localModeFlag)
                {
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("update_label", suffix, false);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@id", obj.Id);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@content", obj.Content);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@left", obj.Position.X);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@top", obj.Position.Y);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@size", obj.Size);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@angle", obj.Angle);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@is_bold", obj.IsBold);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@is_italic", obj.IsItalic);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@is_underline", obj.IsUnderline);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    if ( !schema.IsIS)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@year", schema.Id);
                    if ( obj.LayerId != Guid.Empty)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@layer_id", obj.LayerId);
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@layer_id", DBNull.Value);
                }
                else
                {

                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_label" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", obj.Id));
                        command.Parameters.Add(new SqlParameter("@content", obj.Content));
                        command.Parameters.Add(new SqlParameter("@left", obj.Position.X));
                        command.Parameters.Add(new SqlParameter("@top", obj.Position.Y));
                        command.Parameters.Add(new SqlParameter("@size", obj.Size));
                        command.Parameters.Add(new SqlParameter("@angle", obj.Angle));
                        command.Parameters.Add(new SqlParameter("@is_bold", obj.IsBold));
                        command.Parameters.Add(new SqlParameter("@is_italic", obj.IsItalic));
                        command.Parameters.Add(new SqlParameter("@is_underline", obj.IsUnderline));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        if (obj.LayerId != Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@layer_id", obj.LayerId));
                        else
                            command.Parameters.Add(new SqlParameter("@layer_id", DBNull.Value));

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