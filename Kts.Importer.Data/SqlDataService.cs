using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Kts.Importer.Data
{
    /// <summary>
    /// Представляет сервис данных, хранящихся в базе данных SQL.
    /// </summary>
    [Serializable]
    public sealed partial class SqlDataService : IDataService
    {
        #region Закрытые поля

        /// <summary>
        /// Форматы параметров.
        /// </summary>
        private Dictionary<string, ParameterFormat> formats = new Dictionary<string, ParameterFormat>();

        /// <summary>
        /// Таблицы значений.
        /// </summary>
        private List<TableModel> tables = new List<TableModel>();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Строка подключения к базе данных SQL.
        /// </summary>
        private readonly SqlConnectionString connectionString;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlDataService"/>.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных SQL.</param>
        public SqlDataService(SqlConnectionString connectionString)
        {
            this.connectionString = connectionString;

            // Заполняем данные типов объектов.
            this.FillTypes();

            // Заполняем таблицы значений.
            this.FillTables();

            // Заполняем данные параметров типов объектов.
            this.FillParameters();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Заполняет данные параметров типов объектов.
        /// </summary>
        private void FillParameters()
        {
            // Сперва получаем все параметры.
            var allParams = new Dictionary<int, ParameterModel>();
            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_params", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    int id;
                    string name;
                    string alias;
                    string format;
                    bool isVisible;
                    string tableName;
                    int filterParamId;
                    string filterField;
                    bool isCommon;
                    LoadLevel loadLevel;
                    bool isSearchable;
                    string unit;

                    ParameterFormat parameterFormat;
                    TableModel table;
                    ParameterModel providingParameter;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = Convert.ToString(reader["name"]);
                            alias = Convert.ToString(reader["alias"]);
                            format = Convert.ToString(reader["format"]);
                            if (this.formats.ContainsKey(format))
                                parameterFormat = this.formats[format];
                            else
                            {
                                parameterFormat = new ParameterFormat(format);

                                this.formats.Add(format, parameterFormat);
                            }
                            isVisible = Convert.ToBoolean(reader["is_visible"]);
                            if (reader["table_name"] != DBNull.Value)
                            {
                                tableName = Convert.ToString(reader["table_name"]);

                                if (this.tables.Any(x => x.Name == tableName))
                                    table = this.tables.First(x => x.Name == tableName);
                                else
                                    table = null;
                            }
                            else
                                table = null;
                            if (reader["filter_param_id"] != DBNull.Value)
                            {
                                filterParamId = Convert.ToInt32(reader["filter_param_id"]);

                                providingParameter = allParams[filterParamId];
                            }
                            else
                                providingParameter = null;
                            filterField = Convert.ToString(reader["filter_field"]);
                            isCommon = Convert.ToBoolean(reader["is_common"]);
                            loadLevel = (LoadLevel)Convert.ToInt32(reader["load_level"]);
                            isSearchable = Convert.ToBoolean(reader["is_searchable"]);
                            unit = Convert.ToString(reader["unit"]);

                            allParams.Add(id, new ParameterModel(id, name, alias, parameterFormat, isVisible, table, providingParameter, filterField, isCommon, loadLevel, isSearchable, unit));
                        }
                }

            var div = new char[1]
            {
                ','
            };

            // Получаем параметры по типам объектов.
            foreach (var type in this.ObjectTypes)
                using (var connection = new SqlConnection(this.connectionString.ToString()))
                    using (var command = new SqlCommand("get_params", connection))
                    {
                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));

                        int id;
                        bool isNecessary;
                        bool isReadOnly;
                        int visProvId;
                        string visValues;

                        // Словарь, в котором будут храниться связи между параметрами, видимость которых зависит от других параметров.
                        var visibilities = new Dictionary<ParameterModel, Tuple<int, string>>();

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["id"]);
                                isNecessary = Convert.ToBoolean(reader["is_necessary"]);
                                isReadOnly = Convert.ToBoolean(reader["is_read_only"]);
                                if (reader["vis_prov_id"] != DBNull.Value)
                                {
                                    visProvId = Convert.ToInt32(reader["vis_prov_id"]);
                                    visValues = Convert.ToString(reader["vis_values"]);
                                }
                                else
                                {
                                    visProvId = -1;
                                    visValues = "";
                                }

                                var param = allParams[id];

                                if (isNecessary)
                                    type.NecessaryParameters.Add(param);

                                if (isReadOnly)
                                    type.ReadonlyParameters.Add(param);

                                if (visProvId != -1)
                                    visibilities.Add(param, new Tuple<int, string>(visProvId, visValues));

                                type.Parameters.Add(param);
                            }

                        foreach (var entry in visibilities)
                        {
                            var values = new List<int>();

                            // Извлекаем значения из строки.
                            foreach (var value in entry.Value.Item2.Split(div, StringSplitOptions.RemoveEmptyEntries))
                                values.Add(Convert.ToInt32(value));

                            type.SetParameterVisibility(entry.Key, allParams[entry.Value.Item1], values);
                        }
                    }
        }

        /// <summary>
        /// Заполняет таблицы значений.
        /// </summary>
        private void FillTables()
        {
            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_tables", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    string tableName;
                    object id;
                    string value;
                    string filterField;
                    int? filterValue;

                    string currentTableName = "";
                    TableModel table = null;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            tableName = Convert.ToString(reader["table_name"]);
                            if (currentTableName != tableName)
                            {
                                if (table != null)
                                    this.tables.Add(table);

                                table = new TableModel(tableName);

                                currentTableName = tableName;
                            }
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

                    if (table != null)
                        this.tables.Add(table);
                }
        }

        /// <summary>
        /// Заполняет данные типов объектов.
        /// </summary>
        private void FillTypes()
        {
            var types = new List<ObjectType>();
            
            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_types", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    int typeId;
                    string name;
                    string singularName;
                    int objectKind;
                    byte r;
                    byte g;
                    byte b;
                    string parentTypeId;

                    Color color;
                    Color inactiveColor;

                    var rels = new Dictionary<ObjectType, string>();

                    var div = new char[1]
                    {
                            ','
                    };

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            typeId = Convert.ToInt32(reader["type_id"]);
                            name = Convert.ToString(reader["name"]);
                            singularName = Convert.ToString(reader["singular_name"]);
                            objectKind = Convert.ToInt32(reader["object_kind"]);
                            if (reader["r"] != DBNull.Value)
                            {
                                r = Convert.ToByte(reader["r"]);
                                g = Convert.ToByte(reader["g"]);
                                b = Convert.ToByte(reader["b"]);

                                color = new Color(r, g, b);
                            }
                            else
                                color = null;
                            if (reader["inactive_r"] != DBNull.Value)
                            {
                                r = Convert.ToByte(reader["inactive_r"]);
                                g = Convert.ToByte(reader["inactive_g"]);
                                b = Convert.ToByte(reader["inactive_b"]);

                                inactiveColor = new Color(r, g, b);
                            }
                            else
                                inactiveColor = null;
                            parentTypeId = Convert.ToString(reader["parent_type_id"]);

                            var type = new ObjectType(typeId, name, singularName, (ObjectKind)objectKind, color, inactiveColor, false);

                            types.Add(type);

                            rels.Add(type, parentTypeId);
                        }

                    foreach (var entry in rels)
                        if (!string.IsNullOrEmpty(entry.Value))
                            foreach (var id in entry.Value.Split(div, StringSplitOptions.RemoveEmptyEntries))
                                types.First(x => x.TypeId == Convert.ToInt32(id)).Children.Add(entry.Key);
                }

            this.ObjectTypes = new ReadOnlyCollection<ObjectType>(types);
        }

        #endregion
    }

    // Реализация IDataService.
    public sealed partial class SqlDataService
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает коллекцию типов объектов.
        /// </summary>
        public ReadOnlyCollection<ObjectType> ObjectTypes
        {
            get;
            private set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает все котельные, расположенные в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список, состоящий из тюплов, в котором первым элементом является идентификатор объекта, а вторым - его название.</returns>
        public List<Tuple<Guid, string>> GetBoilers(int cityId, SchemaModel schema)
        {
            var result = new List<Tuple<Guid, string>>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_boilers", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@city_id", cityId));
                    command.Parameters.Add(new SqlParameter("@schema_id", schema.Id));

                    Guid id;
                    string name;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Guid.Parse(Convert.ToString(reader["id"]));
                            name = Convert.ToString(reader["name"]);

                            result.Add(new Tuple<Guid, string>(id, name));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает все населенные пункты заданного региона из источника данных.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Список населенных пунктов.</returns>
        public List<TerritorialEntityModel> GetCities(TerritorialEntityModel region)
        {
            var result = new List<TerritorialEntityModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_cities", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@region_id", region.Id));

                    int id;
                    string name;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = Convert.ToString(reader["name"]);

                            result.Add(new TerritorialEntityModel(id, name));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает схему по умолчанию заданного населенного пункта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Схема.</returns>
        public SchemaModel GetDefaultSchema(int cityId)
        {
            var query = string.Format("get_default_schema @city_id = {0}", cityId);

            SchemaModel result = null;

            try
            {
                using (var connection = new SqlConnection(this.connectionString.ToString()))
                    using (var command = new SqlCommand("get_default_schema", connection))
                    {
                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        int id;
                        string name;
                        bool isActual;
                        bool isIS;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["id"]);
                                name = Convert.ToString(reader["name"]);
                                isActual = Convert.ToBoolean(reader["is_actual"]);
                                isIS = Convert.ToBoolean(reader["is_is"]);

                                result = new SchemaModel(id, name, isActual, isIS);
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
        /// Возвращает все объекты-родители, расположенных в заданном населенном пункте.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список, состоящий из тюплов, в котором первым элементом является название объекта-родителя, а вторым - его идентификатор.</returns>
        public List<Tuple<string, Guid>> GetParents(int cityId, SchemaModel schema)
        {
            var result = new List<Tuple<string, Guid>>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_parents", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@city_id", cityId));
                    command.Parameters.Add(new SqlParameter("@schema_id", schema.Id));

                    string name;
                    Guid id;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);
                            id = Guid.Parse(Convert.ToString(reader["id"]));

                            result.Add(new Tuple<string, Guid>(name, id));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает все регионы из источника данных.
        /// </summary>
        /// <returns>Список регионов.</returns>
        public List<TerritorialEntityModel> GetRegions()
        {
            var result = new List<TerritorialEntityModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_regions", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    int id;
                    string name;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = Convert.ToString(reader["name"]);

                            result.Add(new TerritorialEntityModel(id, name));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает все улицы заданного населенного пункта из источника данных.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Список улиц.</returns>
        public List<TerritorialEntityModel> GetStreets(TerritorialEntityModel city)
        {
            var result = new List<TerritorialEntityModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("get_streets", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@city_id", city.Id));

                    int id;
                    string name;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = Convert.ToString(reader["name"]);

                            result.Add(new TerritorialEntityModel(id, name));
                        }
                }

            return result;
        }

        /// <summary>
        /// Сохраняет значения параметров объекта.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="parentId">Идентификатор объекта-родителя.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="schema">Схема.</param>
        public void SaveObjectValues(int cityId, Guid? parentId, ParameterValueSetModel parameterValueSet, SchemaModel schema)
        {
            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("update_obj_param_values", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@object_id", DBNull.Value));
                    if (parentId.HasValue)
                        command.Parameters.Add(new SqlParameter("@parent_id", parentId.Value));
                    else
                        command.Parameters.Add(new SqlParameter("@parent_id", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@city_id", cityId));
                    command.Parameters.Add(new SqlParameter("@values", parameterValueSet.ToString()));
                    command.Parameters.Add(new SqlParameter("@schema_id", schema.Id));
                    // Надеемся, что пользователь с идентификатором 1 является администратором.
                    command.Parameters.Add(new SqlParameter("@user_id", 1));

                    command.ExecuteNonQuery();
                }
        }

        /// <summary>
        /// Сохраняет значения параметров объекта.
        /// </summary>
        /// <param name="objectId">Идентификатор сохраняемого объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="parentId">Идентификатор объекта-родителя.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="schema">Схема.</param>
        public void SaveObjectValues(Guid objectId, int cityId, Guid? parentId, ParameterValueSetModel parameterValueSet, SchemaModel schema)
        {
            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("update_obj_param_values", connection))
                {
                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@object_id", objectId));
                    if (parentId.HasValue)
                        command.Parameters.Add(new SqlParameter("@parent_id", parentId.Value));
                    else
                        command.Parameters.Add(new SqlParameter("@parent_id", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@city_id", cityId));
                    command.Parameters.Add(new SqlParameter("@values", parameterValueSet.ToString()));
                    command.Parameters.Add(new SqlParameter("@schema_id", schema.Id));
                    // Надеемся, что пользователь с идентификатором 1 является администратором.
                    command.Parameters.Add(new SqlParameter("@user_id", 1));

                    command.ExecuteNonQuery();
                }
        }

        #endregion
    }
}