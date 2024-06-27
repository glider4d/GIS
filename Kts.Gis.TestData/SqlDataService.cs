using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Kts.Gis.Data.Interfaces;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис данных, хранящихся в базе данных SQL.
    /// </summary>
    [Serializable]
    public sealed partial class SqlDataService : IDataService
    {
        #region Закрытые поля

        /// <summary>
        /// Все параметры.
        /// </summary>
        private List<ParameterModel> allParameters;

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
        /// Коннектор с базой данных SQL.
        /// </summary>
        private readonly SqlConnector connector;

        public IMeterAccessService MeterAccessService
        {
            get;set;
        }

        #endregion
        
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlDataService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="errorFolderName">Полный путь к папке, в которой находятся логи об ошибках.</param>
        /// <param name="substrateFolderName">Полный путь к папке, содержащей файлы-изображений подложки.</param>
        /// <param name="thumbnailFolderName">Полный путь к папке, содержащей миниатюры подложек.</param>
        public SqlDataService(SqlConnector connector, string errorFolderName, string substrateFolderName, string thumbnailFolderName)
        {
            this.connector = connector;
            ConnectiorServerName = connector.ConnectionString.Name;
            this.ErrorFolderName = errorFolderName;
            this.SubstrateFolderName = substrateFolderName;
            this.ThumbnailFolderName = thumbnailFolderName;

            this.LoginAccessService = new SqlLoginAccessService(connector);

        }

        public string ConnectiorServerName
        {
            get;
            set;
        } 

        public SqlDataService(ReadOnlyCollection<ObjectType> ObjectTypes)
        {
            this.ObjectTypes = ObjectTypes;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Заполняет геометрии значков.
        /// </summary>
        private void FillBadgeGeometries()
        {
            var query = "get_badge_geometries";

            var temp = new List<BadgeGeometryModel>();

            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("get_badge_geometries", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        string geometry;
                        double hotPointX;
                        double hotPointY;
                        double originPointX;
                        double originPointY;
                        int typeId;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                geometry = Convert.ToString(reader["geometry"]);
                                hotPointX = Convert.ToDouble(reader["hot_point_x"]);
                                hotPointY = Convert.ToDouble(reader["hot_point_y"]);
                                originPointX = Convert.ToDouble(reader["origin_point_x"]);
                                originPointY = Convert.ToDouble(reader["origin_point_y"]);
                                typeId = Convert.ToInt32(reader["type_id"]);

                                temp.Add(new BadgeGeometryModel(geometry, new Point(hotPointX, hotPointY), new Point(originPointX, originPointY), this.GetObjectType(typeId)));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            this.BadgeGeometries = new ReadOnlyCollection<BadgeGeometryModel>(temp);
        }

        /// <summary>
        /// Заполняет данные параметров типов объектов.
        /// </summary>
        private void FillParameters()
        {
            var query = "get_params";

            // Сперва получаем все параметры.
            var allParams = new Dictionary<int, ParameterModel>();
            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("get_params", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

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
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
            
            // Запоминаем все параметры.
            this.allParameters = allParams.Values.ToList();

            var div = new char[1]
            {
                ','
            };

            // Получаем параметры по типам объектов и находим параметры, участвующие в составлении названий объектов.
            foreach (var type in this.ObjectTypes)
            {
                query = string.Format("get_params @type_id = {0}", type.TypeId);

                try
                {
                    using (var connection = this.connector.GetConnection())
                        using (var command = new SqlCommand("get_params", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Убираем ограничение по времени выполнения команды.
                            command.CommandTimeout = 0;

                            command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));

                            int id;
                            bool isNecessary;
                            bool isReadOnly;
                            int visProvId;
                            string visValues;
                            int parentId;

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
                                    if (reader["parent_id"] != DBNull.Value)
                                    {
                                        parentId = Convert.ToInt32(reader["parent_id"]);

                                        type.ParameterBonds.Add(allParams[id], allParams[parentId]);
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
                catch (Exception e)
                {
                    throw new Exception(query, e);
                }
            }
        }

        /// <summary>
        /// Заполняет список схем.
        /// </summary>
        private void FillSchemas()
        {
            var query = string.Format("get_schemas");

            var result = new List<SchemaModel>();

            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("get_schemas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;
                        
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

                                result.Add(new SchemaModel(id, name, isActual, isIS));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            this.Schemas.AddRange(result);
        }


        //List<TableModel> metersTableModels = new List<TableModel>();
        // первый параетр регион, второй параметр TableModel
        private Dictionary<int, TableModel> metersTableModels = new Dictionary<int, TableModel>();
        public TableModel getTableMeterParamsForRegion(int regionID)
        {
            TableModel result = null;
            if (metersTableModels.ContainsKey(regionID))
                result = metersTableModels[regionID];
            else if (metersTableModels.ContainsKey(0))
            {
                result = metersTableModels[regionID];
            }
            return result;
        }
        private void getMetersCaption()
        {
            
            
            var query = @"SELECT * FROM [General].[dbo].[boiler_meter_caption]";
            try
            {
                using (var command = new SqlCommand(query, connector.GetConnection()))
                {
                    string tableName = "boiler_meter_caption";
                    object id;
                    string value;
                    int id_region = 0;
                    

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //tableName = Convert.ToString(reader["table_name"]);
                                id_region = Convert.ToInt32(reader["id_region"]);
                                id = reader["id"];
                                value = Convert.ToString(reader["name"]);
                                if (metersTableModels.ContainsKey(id_region))
                                {
                                    metersTableModels[id_region].AddEntry(null, null, new TableEntryModel(id, value));
                                }
                                else
                                {
                                    metersTableModels.Add(id_region, new TableModel(tableName));
                                    metersTableModels[id_region].AddEntry(null, null, new TableEntryModel(id, value));
                                }
                                //table.AddEntry(filterField, filterValue, new TableEntryModel(id, value));
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

        }

        /// <summary>
        /// Заполняет таблицы значений.
        /// </summary>
        public void FillTables()
        {
            var query = "get_tables";

            this.tables = new List<TableModel>();

            try
            {
                this.getMetersCaption();
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("get_tables", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

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
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Заполняет данные типов объектов.
        /// </summary>
        private void FillTypes()
        {
            var query = "get_types";

            var types = new List<ObjectType>();

            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("get_types", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        int typeId;
                        string name;
                        string singularName;
                        int objectKind;
                        byte r;
                        byte g;
                        byte b;
                        string parentTypeId;
                        bool canBeChanged;

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
                                canBeChanged = Convert.ToBoolean(reader["can_be_changed"]);

                                var type = new ObjectType(typeId, name, singularName, (ObjectKind)objectKind, color, inactiveColor, canBeChanged);

                                types.Add(type);

                                rels.Add(type, parentTypeId);
                            }

                        foreach (var entry in rels)
                            if (!string.IsNullOrEmpty(entry.Value))
                                foreach (var id in entry.Value.Split(div, StringSplitOptions.RemoveEmptyEntries))
                                    types.First(x => x.TypeId == Convert.ToInt32(id)).Children.Add(entry.Key);
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            this.ObjectTypes = new ReadOnlyCollection<ObjectType>(types);
        }

        public ILoginAccessService getLoginAccessService()
        {
            return LoginAccessService;
        }



        #endregion
    }



    // Реализация IDataService.
    public sealed partial class SqlDataService
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает сервис доступа к данным объектов, представляемых значками на карте.
        /// </summary>
        public IChildAccessService<BadgeModel> BadgeAccessService
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает геометрии значков.
        /// </summary>
        public ReadOnlyCollection<BadgeGeometryModel> BadgeGeometries
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к информации о котельной.
        /// </summary>
        public IBoilerInfoAccessService BoilerInfoAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным кастомных слоев.
        /// </summary>
        public ICustomLayerAccessService CustomLayerAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным кастомных объектов.
        /// </summary>
        public ICustomObjectAccessService CustomObjectAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным документов.
        /// </summary>
        public IDocumentAccessService DocumentAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает полный путь к папке, в которой находятся логи об ошибках.
        /// </summary>
        public string ErrorFolderName
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным объектов, представляемых фигурами на карте.
        /// </summary>
        public IFigureAccessService FigureAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным топлива котельных.
        /// </summary>
        public IFuelAccessService FuelAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным глобальной карты.
        /// </summary>
        public IGlobalMapAccessService GlobalMapAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным базовых программ КТС.
        /// </summary>
        public IKtsAccessService KtsAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным надписей.
        /// </summary>
        public ILabelAccessService LabelAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным объектов, представляемых линиями на карте.
        /// </summary>
        public ILineAccessService LineAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает идентификатор залогиненного пользователя.
        /// </summary>
        public int LoggedUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным логинов.
        /// </summary>
        public ILoginAccessService LoginAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным карты.
        /// </summary>
        public IMapAccessService MapAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным объектов, представляемых узлами на карте.
        /// </summary>
        public INodeAccessService NodeAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным невизуальных объектов.
        /// </summary>
        public IChildAccessService<NonVisualObjectModel> NonVisualObjectAccessService
        {
            get;
            set;
        }


        /// <summary>
        /// Возвращает или задает коллекцию типов объектов.
        /// </summary>
        public ReadOnlyCollection<ObjectType> ObjectTypes
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным параметров объектов.
        /// </summary>
        public IParameterAccessService ParameterAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным отчетов.
        /// </summary>
        public IReportAccessService ReportAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает список схем.
        /// </summary>
        public List<SchemaModel> Schemas
        {
            get;
        } = new List<SchemaModel>();

        /// <summary>
        /// Возвращает или задает сервис поиска объектов.
        /// </summary>
        public ISearchService SearchService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает название сервера.
        /// </summary>
        public string ServerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает полный путь к папке, содержащей файлы-изображений подложки.
        /// </summary>
        public string SubstrateFolderName
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает сервис доступа к данным территориальных единиц.
        /// </summary>
        public ITerritorialEntityAccessService TerritorialEntityAccessService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает полный путь к папке, содержащей миниатюры подложек.
        /// </summary>
        public string ThumbnailFolderName
        {
            get;
        }

        #endregion

        #region Открытые методы



        public void getParameterModelForType(ObjectType objType)
        {
            //ObjectTypes[0].
            //ObjectTypes[0].GetType()

            foreach (var objTypeItem in ObjectTypes)
            {
                if (objType.TypeId == objTypeItem.TypeId)
                {
                    
                    System.Console.WriteLine("ObjType = "+objType.TypeId);
                }
                //objType.GUID
            }
        }

        public ParameterModel getAllParameters()
        {
            return allParameters[0];
        }

        /// <summary>
        /// Начинает транзакцию сохранения.
        /// </summary>
        public void BeginSaveTransaction()
        {
            var query = string.Format("sp_begin_transaction @user_id = {0}", this.LoggedUserId);

            try
            {
                if (BaseSqlDataAccessService.localModeFlag)
                    return;
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("sp_begin_transaction", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@user_id", this.LoggedUserId));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли подключиться к источнику данных.
        /// </summary>
        /// <returns>true, если можно подключиться, иначе - false.</returns>
        public bool CanConnect()
        {
            if (this.connector.ConnectionString.Server == "(local)")
                return false;

            return true;
        }

        /// <summary>
        /// Завершает транзакцию сохранения.
        /// </summary>
        /// <returns>true, если сохранение выполнено, иначе - false.</returns>
        public bool EndSaveTransaction()
        {
            var query = string.Format("sp_end_transaction @user_id = {0}", this.LoggedUserId);

            var result = false;

            try
            {
                if (BaseSqlDataAccessService.localModeFlag)
                {
                    result = BaseSqlDataAccessService.serializeStaticSqlQuery(this.LoggedUserId);
                    return result;
                }
                
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("sp_end_transaction", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@user_id", this.LoggedUserId));

                        result = Convert.ToBoolean(command.ExecuteScalar());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает логи изменений данных.
        /// </summary>
        /// <returns>Таблица логов изменений данных.</returns>
        public DataTable GetLogs()
        {
            var query = string.Format("get_logs");

            var result = new DataTable("logs");

            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("get_logs", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

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
        /// Возвращает тип объекта по его идентификатору.
        /// </summary>
        /// <param name="typeId">Идентификатор типа объекта.</param>
        /// <returns>Тип объекта.</returns>
        public ObjectType GetObjectType(int typeId)
        {
            return this.ObjectTypes.First(x => x.TypeId == typeId);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный тип типом котельной.
        /// </summary>
        /// <param name="type">Проверяемый тип.</param>
        /// <returns>true, если заданный тип является типом котельной, иначе - false.</returns>
        public bool IsBoilerType(ObjectType type)
        {
#warning Прямое использование идентификатора типа объекта
            return type.TypeId == 1;
        }

       

        public bool IsTrashStorageType(ObjectType type)
        {
            return type.TypeId == 24;
        }

        /// <summary>
        /// Проверяет, совместима ли заданная версия приложения с источником данных.
        /// </summary>
        /// <param name="version">Версия приложения.</param>
        /// <returns>true, если версия приложения совместима, иначе - false.</returns>
        public bool IsCompatible(string version)
        {
            var query = string.Format("is_compatible @version = {0}", version);

            var result = false;

            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("is_compatible", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@version", version));

                        result = Convert.ToBoolean(command.ExecuteScalar());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный тип типом складом.
        /// </summary>
        /// <param name="type">Проверяемый тип.</param>
        /// <returns>true, если заданный тип является типом складом, иначе - false.</returns>
        public bool IsStorageType(ObjectType type)
        {
#warning Прямое использование идентификатора типа объекта
            return type.TypeId == 19;
        }

        /// <summary>
        /// Асинхронно загружает критически важные данные.
        /// </summary>
        /// 



        public async Task LoadDataAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                




                this.BadgeAccessService = new SqlBadgeAccessService(this, this.connector, this.LoggedUserId);
                this.FigureAccessService = new SqlFigureAccessService(this, this.connector, this.LoggedUserId);
                this.GlobalMapAccessService = new SqlGlobalMapAccessService(this.connector);
                this.LabelAccessService = new SqlLabelAccessService(this.connector, this.LoggedUserId);
                this.LineAccessService = new SqlLineAccessService(this, this.connector, this.LoggedUserId);
                this.MapAccessService = new SqlMapAccessService(this.connector, this.LoggedUserId);
                this.NodeAccessService = new SqlNodeAccessService(this, this.connector, this.LoggedUserId);
                this.NonVisualObjectAccessService = new SqlNonVisualObjectAccessService(this, this.connector, this.LoggedUserId);
                this.ParameterAccessService = new SqlParameterAccessService(this.connector, this.LoggedUserId);
                this.ReportAccessService = new SqlReportAccessService(this.connector);
                this.SearchService = new SqlSearchService(this, this.connector, this.LoggedUserId);
                this.TerritorialEntityAccessService = new SqlTerritorialEntityAccessService(this.connector, this.LoggedUserId);
                this.BoilerInfoAccessService = new SqlBoilerInfoAccessService(this, this.connector, this.LoggedUserId);
                this.MeterAccessService = new SqlMeterAccessService(this.connector);
                this.FuelAccessService = new SqlFuelAccessService(this.connector);
                this.CustomLayerAccessService = new SqlCustomLayerAccessService(this.connector, this.LoggedUserId);
                this.CustomObjectAccessService = new SqlCustomObjectAccessService(this.connector, this.LoggedUserId);
                this.DocumentAccessService = new SqlDocumentAccessService(this.connector, this.LoggedUserId);
                this.KtsAccessService = new SqlKtsAccessService(this.connector, this.LoggedUserId);

                // Заполняем данные типов объектов.
                this.FillTypes();

                // Заполняем таблицы значений.
                this.FillTables();

                // Заполняем данные параметров типов объектов.
                this.FillParameters();

                // Заполняем геометрии значков.
                this.FillBadgeGeometries();

                this.FillSchemas();

                this.ServerName = this.connector.ConnectionString.Name + " (" + this.connector.ConnectionString.Server + ")";
            });
        }

        /// <summary>
        /// Подготавливает схему к работе.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        public void PrepareSchema(int year, int cityId)
        {
            var query = string.Format("hand_of_god @year = {0}, @city_id = {1}, @user_id = {2}", year, cityId, this.LoggedUserId);

            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("hand_of_god", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@year", year));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.LoggedUserId));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Обновляет логи изменений данных.
        /// </summary>
        /// <param name="dataTable">Таблица логов изменений данных.</param>
        public void UpdateLogs(DataTable dataTable)
        {
            var query = string.Format("update_logs @logs = {0}", dataTable.TableName);
            
            try
            {
                using (var connection = this.connector.GetConnection())
                    using (var command = new SqlCommand("update_logs", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@logs", dataTable));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Обновляет все перезаписываемые таблицы.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="loadLevel">Уровень загрузки справочников.</param>
        public void UpdateTables(int cityId, ObjectType type, SchemaModel schema, LoadLevel loadLevel)
        {
            for (int i = 0; i < this.allParameters.Count; i++)
                if (this.allParameters[i].LoadLevel == loadLevel)
                    this.ParameterAccessService.UpdateTable(this.allParameters[i].Table, cityId, type, schema);
        }


        public void UpdateTables(int cityID)
        {
            int regionId = getRegionID(cityID);
            getTableMeterParamsForRegion(regionId);

            if (!metersTableModels.ContainsKey(regionId))
            {
                regionId = 0;
                for (int i = 0; i < allParameters.Count; i++)
                {
                    if (allParameters[i].Table != null && allParameters[i].Table.Name.Equals("boiler_meter_caption"))
                    {
                        allParameters[i].Table = new TableModel("boiler_meter_caption");
                    }
                }
            }

            if (metersTableModels.ContainsKey(regionId))
            {
                for (int i = 0; i < tables.Count; i++)
                {
                    if (tables[i].Name.Equals("boiler_meter_caption"))
                        tables[i] = metersTableModels[regionId];
                }

                for (int i = 0; i < allParameters.Count; i++)
                {
                    if (allParameters[i].Table != null && allParameters[i].Table.Name.Equals("boiler_meter_caption"))
                    {
                        allParameters[i].Table = metersTableModels[regionId];
                    }
                }
            } 
        }

        public int getRegionID(int cityID)
        {
            int result = 0;
            var query = @"SELECT * FROM [General].[dbo].[vCities] where id = @cityID";
            try
            {
                using (var command = new SqlCommand(query, this.connector.GetConnection()))
                {
                    command.Parameters.AddWithValue("@cityID", cityID);

                    using (var reader =  command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = Convert.ToInt32(reader["id_region"]);
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

        //public void UpdateTablesMeters(int cityId, ObjectType type, SchemaModel schema, LoadLevel loadLevel)
        /// <summary>
        /// Обновляет параметры, отвечающие за составление названия объектов.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пунктаю</param>
        public void UpdateTypeCaptionParams(int cityId)
        {
            var div = new char[1]
            {
                ','
            };

            foreach (var type in this.ObjectTypes)
            {
                type.CaptionParameters.Clear();

                var query = string.Format("get_caption_params @type_id = {0}, @city_id = {1}, @user_id = {2}", type.TypeId, cityId, this.LoggedUserId);

                var captionParams = "";

                try
                {
                    using (var connection = this.connector.GetConnection())
                        using (var command = new SqlCommand("get_caption_params", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Убираем ограничение по времени выполнения команды.
                            command.CommandTimeout = 0;

                            command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));
                            command.Parameters.Add(new SqlParameter("@city_id", cityId));
                            command.Parameters.Add(new SqlParameter("@user_id", this.LoggedUserId));

                            captionParams = Convert.ToString(command.ExecuteScalar());

                            if (!string.IsNullOrEmpty(captionParams))
                                foreach (var id in captionParams.Split(div, StringSplitOptions.RemoveEmptyEntries))
                                    type.CaptionParameters.Add(type.Parameters.First(x => x.Id == Convert.ToInt32(id)));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(query, e);
                }
            }
        }



        public List<ObjectType> GetFillTypes()
        {
            return ObjectTypes.ToList<ObjectType>();
        }


        public List<TableModel> GetFillTables()
        {
            return tables;
        }


        public List<ParameterModel> GetFillParameters()
        {
            return allParameters;
        }


        public List<BadgeGeometryModel> GetFillBadgeGeometries()
        {
            return BadgeGeometries.ToList<BadgeGeometryModel>();
        }


        public List<SchemaModel> GetFillSchemas()
        {
            return Schemas;
        }

        #endregion
    }
}