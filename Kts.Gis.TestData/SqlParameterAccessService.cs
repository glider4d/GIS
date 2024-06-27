using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным параметров объектов, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlParameterAccessService : BaseSqlDataAccessService, IParameterAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlParameterAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlParameterAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Возвращает набор значений параметров объектов.
        /// </summary>
        /// <param name="connection">Соединение.</param>
        /// <param name="command">Комманда.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="parentId">Идентификатор родителя объектов.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        private ParameterValueSetModel GetGroupParamValues(SqlConnection connection, SqlCommand command, List<Guid> objectIds, ObjectType type, SchemaModel schema)
        {
            string s = null;
            
            if (objectIds.Count > 0)
            {
                s = objectIds[0].ToString();

                for (int i = 1; i < objectIds.Count; i++)
                    s += "," + objectIds[i].ToString();
            }

            var query = string.Format("{0} @object_ids = {1}, @type_id = {2}, @year = {3}, @user_id = {4}", command.CommandText, s != null ? s : "null", type.TypeId, schema.Id, this.loggedUserId);

            var paramValues = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());

            try
            {
                command.CommandType = CommandType.StoredProcedure;

                // Убираем ограничение по времени выполнения команды.
                command.CommandTimeout = 0;

                if (s != null)
                    command.Parameters.Add(new SqlParameter("@object_ids", s));
                else
                    command.Parameters.Add(new SqlParameter("@object_ids", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));
                if (!schema.IsIS)
                {
                    command.Parameters.Add(new SqlParameter("@year", schema.Id));
                    command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                }

                int paramId;
                object value;

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                    {
                        paramId = Convert.ToInt32(reader["param_id"]);
                        value = reader["value"];

                        var param = type.Parameters.First(x => x.Id == paramId);

                        if (value != DBNull.Value)
                            paramValues.Add(param, param.Format.Format(value));
                        else
                            paramValues.Add(param, null);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return new ParameterValueSetModel(paramValues);
        }

        /// <summary>
        /// Асинхронно возвращает набор значений параметров объектов.
        /// </summary>
        /// <param name="connection">Соединение.</param>
        /// <param name="command">Комманда.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="parentId">Идентификатор родителя объектов.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находятся объекты.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        private async Task<ParameterValueSetModel> GetGroupParamValuesAsync(SqlConnection connection, SqlCommand command, List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken)
        {
            string s = null;

            if (objectIds.Count > 0)
            {
                s = objectIds[0].ToString();

                for (int i = 1; i < objectIds.Count; i++)
                    s += "," + objectIds[i].ToString();
            }

            var query = string.Format("{0} @object_ids = {1}, @type_id = {2}, @year = {3}, @user_id = {4}", command.CommandText, s != null ? s : "null", type.TypeId, schema.Id, this.loggedUserId);

            var paramValues = new Dictionary<ParameterModel, object>();

            try
            {
                command.CommandType = CommandType.StoredProcedure;

                // Убираем ограничение по времени выполнения команды.
                command.CommandTimeout = 0;

                if (s != null)
                    command.Parameters.Add(new SqlParameter("@object_ids", s));
                else
                    command.Parameters.Add(new SqlParameter("@object_ids", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));
                if (!schema.IsIS)
                {
                    command.Parameters.Add(new SqlParameter("@year", schema.Id));
                    command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                }

                int paramId;
                object value;
                
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    while (reader.Read())
                    {
                        paramId = Convert.ToInt32(reader["param_id"]);
                        value = reader["value"];

                        var param = type.Parameters.First(x => x.Id == paramId);

                        if (value != DBNull.Value)
                            paramValues.Add(param, param.Format.Format(value));
                        else
                            paramValues.Add(param, null);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return new ParameterValueSetModel(paramValues);
        }

        /// <summary>
        /// Возвращает набор значений параметров заданного объекта.
        /// </summary>
        /// <param name="connection">Соединение.</param>
        /// <param name="command">Комманда.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        private ParameterValueSetModel GetObjectParamValues(SqlConnection connection, SqlCommand command, IObjectModel obj, SchemaModel schema)
        {
            
            var query = string.Format("{0} @id = {1}, @type_id = {2}, @year = {3}, @user_id = {4}", command.CommandText, obj.Id, obj.Type.TypeId, schema.Id, this.loggedUserId);

            var paramValues = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());
            if (!testConnection("", true))
                return null;
            try
            {
                command.CommandType = CommandType.StoredProcedure;

                // Убираем ограничение по времени выполнения команды.
                command.CommandTimeout = 0;

                command.Parameters.Add(new SqlParameter("@id", obj.Id));
                command.Parameters.Add(new SqlParameter("@type_id", obj.Type.TypeId));
                if (!schema.IsIS)
                {
                    command.Parameters.Add(new SqlParameter("@year", schema.Id));
                    command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                }

                int paramId;
                object value;

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                    {
                        paramId = Convert.ToInt32(reader["param_id"]);
                        value = reader["value"];

                        var param = obj.Type.Parameters.First(x => x.Id == paramId);

                        if (value != DBNull.Value)
                            paramValues.Add(param, param.Format.Format(value));
                        else
                            paramValues.Add(param, null);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return new ParameterValueSetModel(paramValues);
        }

        /// <summary>
        /// Асинхронно возвращает набор значений параметров заданного объекта.
        /// </summary>
        /// <param name="connection">Соединение.</param>
        /// <param name="command">Комманда.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        private async Task<ParameterValueSetModel> GetObjectParamValuesAsync(SqlConnection connection, SqlCommand command, IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken)
        {

            var query = string.Format("{0} @id = {1}, @type_id = {2}, @year = {3}, @user_id = {4}", command.CommandText, obj.Id, obj.Type.TypeId, schema.Id, this.loggedUserId);

            var paramValues = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());
            if ( !testConnection("", true))
                return new ParameterValueSetModel(paramValues);

            try
            {
                command.CommandType = CommandType.StoredProcedure;

                // Убираем ограничение по времени выполнения команды.
                command.CommandTimeout = 0;

                command.Parameters.Add(new SqlParameter("@id", obj.Id));
                command.Parameters.Add(new SqlParameter("@type_id", obj.Type.TypeId));
                if (!schema.IsIS)
                {
                    command.Parameters.Add(new SqlParameter("@year", schema.Id));
                    command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                }

                int paramId;
                object value;

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    while (reader.Read())
                    {
                        paramId = Convert.ToInt32(reader["param_id"]);
                        value = reader["value"];

                        var param = obj.Type.Parameters.First(x => x.Id == paramId);

                        if (value != DBNull.Value)
                            paramValues.Add(param, param.Format.Format(value));
                        else
                            paramValues.Add(param, null);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return new ParameterValueSetModel(paramValues);
        }

        #endregion
    }

    // Реализация IParameterAccessService.
    public sealed partial class SqlParameterAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Возвращает набор значений общих параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        public ParameterValueSetModel GetGroupCommonParamValues(List<Guid> objectIds, ObjectType type, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;

            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_group_common_param_values" + suffix, connection))
                result = this.GetGroupParamValues(connection, command, objectIds, type, schema);

            return result;
        }

        /// <summary>
        /// Асинхронно возвращает набор значений общих параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        public async Task<ParameterValueSetModel> GetGroupCommonParamValuesAsync(List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;

            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_group_common_param_values" + suffix, connection))
                result = await this.GetGroupParamValuesAsync(connection, command, objectIds, type, schema, cancellationToken);

            return result;
        }

        /// <summary>
        /// Возвращает набор значений параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        public ParameterValueSetModel GetGroupParamValues(List<Guid> objectIds, ObjectType type, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;

            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_group_param_values" + suffix, connection))
                result = this.GetGroupParamValues(connection, command, objectIds, type, schema);

            return result;
        }

        /// <summary>
        /// Асинхронно возвращает набор значений параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        public async Task<ParameterValueSetModel> GetGroupParamValuesAsync(List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;

            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_group_param_values" + suffix, connection))
                result = await this.GetGroupParamValuesAsync(connection, command, objectIds, type, schema, cancellationToken);

            return result;
        }

        /// <summary>
        /// Возвращает набор значений параметров объектов с заданными идентификаторами.
        /// </summary>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="param">Параметр.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        public Dictionary<Guid, object> GetGroupParamValues(List<Guid> objectIds, ObjectType type, ParameterModel param, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            string s = null;

            if (objectIds.Count > 0)
            {
                s = objectIds[0].ToString();

                for (int i = 1; i < objectIds.Count; i++)
                    s += "," + objectIds[i].ToString();
            }

            var query = string.Format("get_group_param_values{0} @object_ids = {1}, @type_id = {2}, @param_id = {3}, @year = {4}, @user_id = {5}", suffix, s != null ? s : "null", type.TypeId, param.Id, schema.Id, this.loggedUserId);

            var result = new Dictionary<Guid, object>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                using (var command = new SqlCommand("get_group_param_values" + suffix, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Убираем ограничение по времени выполнения команды.
                    command.CommandTimeout = 0;

                    if (s != null)
                        command.Parameters.Add(new SqlParameter("@object_ids", s));
                    else
                        command.Parameters.Add(new SqlParameter("@object_ids", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));
                    command.Parameters.Add(new SqlParameter("@param_id", param.Id));
                    if (!schema.IsIS)
                    {
                        command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                    }

                    Guid id;
                    object value;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Guid.Parse(Convert.ToString(reader["id"]));
                            value = reader["value"];

                            result.Add(id, value);
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
        /// Возвращает недостающий набор значений параметров объекта-получателя от объекта-донора.
        /// </summary>
        /// <param name="recipient">Объект-получатель.</param>
        /// <param name="donor">Объект-донор.</param>
        /// <param name="type">Тип объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        public ParameterValueSetModel GetMergedParamValues(IObjectModel recipient, IObjectModel donor, ObjectType type, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_merged_param_values{0} @recipient = {1}, @donor = {2}, @type = {3}, @year = {4}, @user_id = {5}", suffix, recipient.Id, donor.Id, type.TypeId, schema.Id, this.loggedUserId);

            var paramValues = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());

            try
            {
                using (var connection = this.Connector.GetConnection())
                using (var command = new SqlCommand("get_merged_param_values" + suffix, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Убираем ограничение по времени выполнения команды.
                    command.CommandTimeout = 0;

                    command.Parameters.Add(new SqlParameter("@recipient", recipient.Id));
                    command.Parameters.Add(new SqlParameter("@donor", donor.Id));
                    command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));
                    if (!schema.IsIS)
                    {
                        command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                    }

                    int paramId;
                    object value;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            paramId = Convert.ToInt32(reader["param_id"]);
                            value = reader["value"];

                            var param = type.Parameters.First(x => x.Id == paramId);

                            if (value != DBNull.Value)
                                paramValues.Add(param, param.Format.Format(value));
                            else
                                paramValues.Add(param, null);
                        }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return new ParameterValueSetModel(paramValues);
        }

        /// <summary>
        /// Возвращает набор значений вычисляемых параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        public ParameterValueSetModel GetObjectCalcParamValues(IObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;
            if (!testConnection("", true))
                return result;
            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_obj_calc_param_values" + suffix, connection))
                result = this.GetObjectParamValues(connection, command, obj, schema);

            return result;
        }

        /// <summary>
        /// Асинхронно возвращает набор значений вычисляемых параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        public async Task<ParameterValueSetModel> GetObjectCalcParamValuesAsync(IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;
            if (!testConnection("", true))
                return result;
            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_obj_calc_param_values" + suffix, connection))
                result = await this.GetObjectParamValuesAsync(connection, command, obj, schema, cancellationToken);

            return result;
        }

        /// <summary>
        /// Возвращает набор значений параметров заданного объекта из заданного набора данных.
        /// </summary>
        /// <param name="dataSet">Набор данных.</param>
        /// <param name="obj">Объект.</param>
        /// <returns>Набор значений параметров.</returns>
        public ParameterValueSetModel GetObjectParamValues(DataSet dataSet, IObjectModel obj)
        {
            int paramId;
            object value;

            var paramValues = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());

            if (!testConnection("", true))
                return new ParameterValueSetModel(paramValues);

            foreach (DataRow row in dataSet.Tables["values"].Select("object_id = '" + obj.Id + "'"))
            {
                paramId = Convert.ToInt32(row["param_id"]);
                value = row["value"];

                var param = obj.Type.Parameters.First(x => x.Id == paramId);

                if (value != DBNull.Value)
                    paramValues.Add(param, param.Format.Format(value));
                else
                    paramValues.Add(param, null);
            }

            return new ParameterValueSetModel(paramValues);
        }

        /// <summary>
        /// Возвращает набор значений параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Набор значений параметров.</returns>
        public ParameterValueSetModel GetObjectParamValues(IObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;

            if (!testConnection("", true))
                return result;

            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_obj_param_values" + suffix, connection))
                result = this.GetObjectParamValues(connection, command, obj, schema);

            return result;
        }

        /// <summary>
        /// Асинхронно возвращает набор значений параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Набор значений параметров.</returns>
        public async Task<ParameterValueSetModel> GetObjectParamValuesAsync(IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken)
        {
            var suffix = schema.IsIS ? "_is" : "";

            ParameterValueSetModel result = null;


            using (var connection = this.Connector.GetConnection())
            using (var command = new SqlCommand("get_obj_param_values" + suffix, connection))
                result = await this.GetObjectParamValuesAsync(connection, command, obj, schema, cancellationToken);

            return result;
        }

        public async Task<ParameterValueSetModel> GetObjectParamValuesAsync(IObjectModel obj, SchemaModel schema)
        {
            CancellationToken cancellationToken = new CancellationToken();
            ParameterValueSetModel result = null;
            result = await this.GetObjectParamValuesAsync(obj, schema, cancellationToken);
            return result;
        }

        /// <summary>
        /// Возвращает идентификаторы объектов с незаполненными обязательными полями заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список идентификаторов объектов.</returns>
        public List<Guid> GetObjectsWithErrors(int cityId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_objects_with_errors{0} @city_id = {1}, @year = {2}, @user_id = {3}", suffix, cityId, schema.Id, this.loggedUserId);

            var result = new List<Guid>();
            if (!testConnection("", true))
                return result;
            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_objects_with_errors" + suffix, connection))
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
                                id = Guid.Parse(Convert.ToString(reader["id"]));

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
        /// Возвращает историю изменений значения параметра заданного объекта.
        /// </summary>
        /// <param name="parameterId">Идентификатор параметра.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>История изменений значения параметра.</returns>
        public List<ParameterHistoryEntryModel> GetParameterHistory(int parameterId, DateTime fromDate, DateTime toDate, Guid objectId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_param_history{0} @param_id = {1}, @from_date = {2}, @to_date = {3}, @obj_id = {4}, @year = {5}, @user_id = {6}", suffix, parameterId, fromDate, toDate, objectId, schema.Id, this.loggedUserId);

            var result = new List<ParameterHistoryEntryModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_param_history" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@param_id", parameterId));
                        command.Parameters.Add(new SqlParameter("@from_date", fromDate));
                        command.Parameters.Add(new SqlParameter("@to_date", toDate));
                        command.Parameters.Add(new SqlParameter("@obj_id", objectId));
                        if (!schema.IsIS)
                        {
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        }

                        DateTime _fromDate;
                        object value;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                _fromDate = Convert.ToDateTime(reader["from_date"]);
                                value = reader["value"];

                                result.Add(new ParameterHistoryEntryModel(_fromDate, value));
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
        /// Возвращает значение просматриваемого параметра заданного объекта.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="objectId">Идентификатор объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Значение параметра.</returns>
        public object GetVieweryValue(ParameterModel param, Guid objectId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_viewery_value{0} @param_id = {1}, @object_id = {2}", suffix, param.Id, objectId);
            
            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_viewery_value" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@param_id", param.Id));
                        command.Parameters.Add(new SqlParameter("@object_id", objectId));

                        return command.ExecuteScalar();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }
        
        /// <summary>
        /// Обновляет значения параметров нового (несохраненного в источнике данных) объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="paramValues">Значения параметров.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор нового (несохраненного в источнике данных) объекта.</returns>        
        public Guid UpdateNewObjectParamValues(IObjectModel obj, ParameterValueSetModel paramValues, SchemaModel schema )
        {
            
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_obj_param_values{0} @object_id = null, @parent_id = {1}, @city_id = {2}, @values = {3}, @user_id = {4}, @year = {5}", suffix, obj.ParentId.HasValue ? obj.ParentId.Value.ToString() : "null", obj.CityId, paramValues.ToString(), this.loggedUserId, schema.Id);

            var id = ObjectModel.DefaultId;
            //serrializedSqlQuery.addNewObjectParamValues("UpdateNewObjectParamValue", suffix);
            
            try
            {
                if (localModeFlag)
                {
                    
                    
                    bool parentLayer = obj.ParentId.HasValue;
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("UpdateNewObjectParamValues", suffix, parentLayer);
                    //id = (Guid)BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@object_id", DBNull.Value, true);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@object_id", DBNull.Value, false);
                    if (obj.ParentId.HasValue)
                    {
                        serrializedSqlQuery.setParrent();
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@parent_id", obj.ParentId.Value);
                    }
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@parent_id", DBNull.Value);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@values", paramValues.ToString());
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    if (!schema.IsIS)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@year", schema.Id);
                }
                else
                {

                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_obj_param_values" + suffix, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        



                        command.Parameters.Add(new SqlParameter("@object_id", DBNull.Value));
                        if (obj.ParentId.HasValue)
                            command.Parameters.Add(new SqlParameter("@parent_id", obj.ParentId.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@parent_id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        command.Parameters.Add(new SqlParameter("@values", paramValues.ToString()));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        

                        
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

        public Guid UpdateNewObjectParamValuesFromLocal(int indexForSerialized)
        {
            var query = "update_obj_param_values_from_local";
            var id = ObjectModel.DefaultId;
            try
            {

                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(indexForSerialized);
                var suffix = serrializedSqlQuery.getSufix(indexForSerialized);
                if (serializedParameters != null)
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_obj_param_values" + suffix, connection))
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
                                command.Parameters.Add(new SqlParameter(dictionaryItem.Key, dictionaryItem.Value));
                            }

                        }
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
        /// Обновляет значения параметров заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="paramValues">Значения параметров.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateObjectParamValues(IObjectModel obj, ParameterValueSetModel paramValues, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_obj_param_values{0} @object_id = {1}, @parent_id = {2}, @city_id = {3}, @values = {4}, @user_id = {5}, @year = {6}", suffix, obj.Id, obj.ParentId.HasValue ? obj.ParentId.Value.ToString() : "null", obj.CityId, paramValues.ToString(), this.loggedUserId, schema.Id);

            try
            {
                if (localModeFlag)
                {
                    bool parentLayer = obj.ParentId.HasValue;
                    BaseSqlDataAccessService.serrializedSqlQuery.addNewObjectParamValues("UpdateObjectParamValues", suffix, parentLayer);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@object_id", obj.Id);
                    /*
                    if (obj.ParentId.HasValue)
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@parent_id", obj.ParentId.Value);
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@parent_id", DBNull.Value);
                    */
                    if (obj.ParentId.HasValue)
                    {
                        serrializedSqlQuery.setParrent();
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@parent_id", obj.ParentId.Value);
                    }
                    else
                        BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@parent_id", DBNull.Value);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@city_id", obj.CityId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@values", paramValues.ToString());
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@user_id", this.loggedUserId);
                    BaseSqlDataAccessService.serrializedSqlQuery.addObjectParamValue("@year", schema.Id);
                }
                else
                {

                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_obj_param_values" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@object_id", obj.Id));
                        if (obj.ParentId.HasValue)
                            command.Parameters.Add(new SqlParameter("@parent_id", obj.ParentId.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@parent_id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@city_id", obj.CityId));
                        command.Parameters.Add(new SqlParameter("@values", paramValues.ToString()));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
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

        public void UpdateObjectParamValues(Guid guid, int index)
        {
            var query = "update_obj_param_values";
            var id = ObjectModel.DefaultId;
            try
            {

                Dictionary<string, object> serializedParameters = serrializedSqlQuery.getParametersFromIndex(index);
                var suffix = serrializedSqlQuery.getSufix(index);
                if (serializedParameters != null)
                {
                    using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_obj_param_values" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        foreach (var dictionaryItem in serializedParameters)
                        {

                            if (dictionaryItem.Key.Equals("@object_id"))
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
                        id = Guid.Parse(command.ExecuteScalar().ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
            
        }

        /// <summary>
        /// Обновляет заданную таблицу.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="schema">Схема.</param>
        public TableModel UpdateTable(TableModel table, int cityId, ObjectType type, SchemaModel schema)
        {
            if (table == null || !testConnection("", true))
                return table;

            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_table{0} @name = {1}, @city_id = {2}, @object_type = {3}, @year = {4}", suffix, table.Name, cityId, type != null ? type.TypeId : -1, schema.Id);

            table.Clear();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_table" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@name", table.Name));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (type != null)
                            command.Parameters.Add(new SqlParameter("@object_type", type.TypeId));
                        else
                            command.Parameters.Add(new SqlParameter("@object_type", DBNull.Value));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        object id;
                        string value;
                        string filterField;
                        int? filterValue;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                if (reader["filter_field"] != DBNull.Value)
                                {
                                    filterField = Convert.ToString(reader["filter_field"]);
                                    filterValue = Convert.ToInt32(reader["filter_value"]);
                                }
                                else
                                {
                                    filterField = "";
                                    filterValue = null;
                                }
                                id = reader["id"];
                                value = Convert.ToString(reader["value"]);

                                table.AddEntry(filterField, filterValue, new TableEntryModel(id, value));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
            return table;
        }

        #endregion
    }
}