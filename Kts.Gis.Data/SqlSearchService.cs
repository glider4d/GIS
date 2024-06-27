using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис поиска объектов в базе данных SQL.
    /// </summary>
    public sealed partial class SqlSearchService : BaseSqlDataAccessService, ISearchService
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
        /// Инициализирует новый экземпляр класса <see cref="SqlSearchService"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlSearchService(SqlDataService dataService, SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация ISearchService.
    public sealed partial class SqlSearchService
    {
        #region Открытые методы

        /// <summary>
        /// Находит объекты в источнике данных по заданному значению параметра.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="regionId">Идентификатор региона, по которому ищутся объекты.</param>
        /// <param name="cityId">Идентификатор населенного пункта, по которому ищутся объекты.</param>
        /// <param name="schema">Схема, по которой ищутся объекты.</param>
        /// <param name="searchTerms">Условия поиска.</param>
        /// <returns>Результат поиска.</returns>
        public List<SearchEntryModel> FindObjects(ObjectType type, int regionId, int cityId, SchemaModel schema, List<SearchTermModel> searchTerms)
        {
            var suffix = schema.IsIS ? "_is" : "";

            // Составляем фильтр поиска.
            var filters = "";
            foreach (var searchTerm in searchTerms)
            {
                // Получаем условие поиска.
                var opSuffix = "";
                switch (searchTerm.Term)
                {
                    case Operator.Contains:
                        opSuffix = "L";

                        break;

                    case Operator.Empty:
                        opSuffix = "E";

                        break;

                    case Operator.Equal:
                        opSuffix = "=";

                        break;

                    case Operator.Less:
                        opSuffix = "<";

                        break;

                    case Operator.More:
                        opSuffix = ">";

                        break;

                    case Operator.NotContains:
                        opSuffix = "NL";

                        break;

                    case Operator.NotEqual:
                        opSuffix = "<>";

                        break;

                    default:
                        throw new NotImplementedException("Не реализована работа со следующим оператором: " + searchTerm.Term.ToString());
                }

                filters += searchTerm.Parameter.Id.ToString() + " " + opSuffix + " " + searchTerm.Value + "|";
            }

            var query = string.Format("find_objects{0} @type_id = {1}, @region_id = {2}, @city_id = {3}, @year = {4}, @filters = {5}, @user_id = {6}", suffix, type.TypeId, regionId != -1 ? regionId.ToString() : "null", cityId != -1 ? cityId.ToString() : "null", schema.Id, filters, this.loggedUserId);

            var result = new List<SearchEntryModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("find_objects" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@type_id", type.TypeId));
                        if (regionId != -1)
                            command.Parameters.Add(new SqlParameter("@region_id", regionId));
                        if (cityId != -1)
                            command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@filters", filters));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));

                        Guid id;
                        Guid? parentId;
                        string name;
                        string regionName;
                        int _cityId;
                        string cityName;

                        Dictionary<ParameterModel, string> paramValues;
                        ParameterModel param;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                if (reader["parent_id"] != DBNull.Value)
                                    parentId = Guid.Parse(reader["parent_id"].ToString());
                                else
                                    parentId = null;
                                name = Convert.ToString(reader["name"]);
                                regionName = Convert.ToString(reader["region_name"]);
                                _cityId = Convert.ToInt32(reader["city_id"]);
                                cityName = Convert.ToString(reader["city_name"]);

                                // Извлекаем значения параметров.
                                paramValues = new Dictionary<ParameterModel, string>();
                                for (int i = 6; i < reader.FieldCount; i++)
                                {
                                    param = type.Parameters.First(x => x.Id == Convert.ToInt32(reader.GetName(i)));

                                    paramValues.Add(param, param.GetValueAsString(reader.GetValue(i), null));
                                }

                                result.Add(new SearchEntryModel(id, parentId, name, type, regionName, _cityId, cityName, schema.Id, paramValues));
                            }
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        #endregion
    }
}