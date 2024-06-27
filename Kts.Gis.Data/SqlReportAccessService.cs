using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным отчетов, хранящихся в базе данных SQL.
    /// </summary>
    public sealed partial class SqlReportAccessService : BaseSqlDataAccessService, IReportAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlReportAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор залогиненного пользователя.</param>
        public SqlReportAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация IReportAccessService.
    public sealed partial class SqlReportAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет расчет гидравлики.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        public void CalculateHydraulics(Guid boilerId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("rpt.calculate_hydraulics{0} @boiler_id = {1}, @year = {2}", suffix, boilerId, schema.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.calculate_hydraulics" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
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
        /// Возвращает данные отчета с информацией о количестве введенных объектов.
        /// </summary>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Данные отчета.</returns>
        public DataSet GetAddedObjectInfo(DateTime fromDate, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var result = new DataSet("AOI");

            var query = string.Format("rpt.get_added_object_info{0} @from_date = {1}, @year = {2}", suffix, fromDate, schema.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_added_object_info" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@from_date", fromDate));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Data");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает данные отчета о несопоставленных объектах.
        /// </summary>
        /// <param name="all">По всем объектам.</param>
        /// <returns>Данные отчета.</returns>
        public DataSet GetDiffObjects(bool all)
        {
            var result = new DataSet("DO");

            var query = string.Format("rpt.get_diff_objects");

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_diff_objects", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@all", all));
                        command.Parameters.Add(new SqlParameter("@type", 1));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "GisKvp");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_diff_objects", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@all", all));
                        command.Parameters.Add(new SqlParameter("@type", 2));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Kvp");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_diff_objects", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@all", all));
                        command.Parameters.Add(new SqlParameter("@type", 3));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "GisJur");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_diff_objects", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@all", all));
                        command.Parameters.Add(new SqlParameter("@type", 4));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Jur");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает данные отчета о гидравлике.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Данные отчета.</returns>
        public DataSet GetHydraulics(Guid boilerId, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var result = new DataSet("H");

            var query = string.Format("rpt.get_hydraulics{0} @boiler_id = {1}, @year = {2}", suffix, boilerId, schema.Id);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_hydraulics" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Data");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает данные отчета о проценте сопоставления с программами.
        /// </summary>
        /// <param name="all">По всем объектам.</param>
        /// <returns>Данные отчета.</returns>
        public DataSet GetIntegrationStats(bool all)
        {
            var result = new DataSet("IS");

            var query = string.Format("rpt.get_integration_stats_xxx");

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_integration_stats_kvp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@all", all));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Kvp");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_integration_stats_jur", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@all", all));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Jur");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает данные отчета о жилищном фонде и договорных подключениях (по КТС).
        /// </summary>
        /// <param name="regionId">Идентификатор региона.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема населенного пункта.</param>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <returns>Данные отчета.</returns>
        public DataSet GetKts(int regionId, int cityId, SchemaModel schema, Guid boilerId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var result = new DataSet("K");

            var query = string.Format("rpt.get_kts_xxx{0} @region_id = {1}, @city_id = {2}, @year = {3}, @boiler_id = {4}", suffix, regionId, cityId, schema.Id, boilerId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_kts_kvp" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;
                        
                        command.Parameters.Add(new SqlParameter("@region_id", regionId));
                        if (cityId == -1)
                            command.Parameters.Add(new SqlParameter("@city_id", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        if (boilerId == Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@boiler_id", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Kvp");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_kts_jur" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@region_id", regionId));
                        if (cityId == -1)
                            command.Parameters.Add(new SqlParameter("@city_id", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@city_id", cityId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        if (boilerId == Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@boiler_id", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));

                        using (var adapter = new SqlDataAdapter(command))
                            adapter.Fill(result, "Jur");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает данные отчета о технических характеристиках.
        /// </summary>
        /// <param name="ids">Номера частей отчета.</param>
        /// <param name="regionId">Идентификатор региона.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="schema">Схема населенного пункта.</param>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="fromDate">Дата с.</param>
        /// <param name="toDate">Дата по.</param>
        /// <returnsя>Данные отчета.</returns>
        public DataSet GetTechSpec(List<int> ids, int regionId, int cityId, SchemaModel schema, Guid boilerId, DateTime fromDate, DateTime toDate)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var result = new DataSet("TS");

            foreach (var id in ids)
            {
                var query = string.Format("rpt.get_tech_spec{0} @id = {1}, @region_id = {2}, @city_id = {3}, @year = {4}, @boiler_id = {5}, @from_date = {6}, @to_date = {7}, @user_id = {8}", suffix, id, regionId, cityId, schema.Id, boilerId, fromDate, toDate, this.loggedUserId);

                try
                {
                    using (var connection = this.Connector.GetConnection())
                        using (var command = new SqlCommand("rpt.get_tech_spec" + suffix, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Убираем ограничение по времени выполнения команды.
                            command.CommandTimeout = 0;

                            command.Parameters.Add(new SqlParameter("@id", id));
                            command.Parameters.Add(new SqlParameter("@region_id", regionId));
                            if (cityId == -1)
                                command.Parameters.Add(new SqlParameter("@city_id", DBNull.Value));
                            else
                                command.Parameters.Add(new SqlParameter("@city_id", cityId));
                            if (!schema.IsIS)
                                command.Parameters.Add(new SqlParameter("@year", schema.Id));
                            if (boilerId == Guid.Empty)
                                command.Parameters.Add(new SqlParameter("@boiler_id", DBNull.Value));
                            else
                                command.Parameters.Add(new SqlParameter("@boiler_id", boilerId));
                            command.Parameters.Add(new SqlParameter("@from_date", fromDate));
                            command.Parameters.Add(new SqlParameter("@to_date", toDate));
                            command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (id == 5)
                        {
                            List<List<string>> rowslist = new List<List<string>>();
                            using (var reader = command.ExecuteReader())
                                while (reader.Read())
                                {
                                    List<string> rows = new List<string>();

                                    //rows.Add(reader["ord"].ToString());
                                    rows.Add("");
                                    rows.Add("");
                                    rows.Add(reader["2"].ToString());
                                    rows.Add(reader["3"].ToString());
                                    rows.Add(reader["4"].ToString());
                                    rows.Add(reader["5"].ToString());
                                    rows.Add(reader["6"].ToString());
                                    rows.Add(reader["7"].ToString());
                                    rows.Add(reader["8"].ToString());
                                    rows.Add("");
                                    rows.Add(reader["10"].ToString());
                                    rows.Add("");
                                    rows.Add(reader["11"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["12"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["13"].ToString());
                                    rows.Add(reader["13_1"].ToString());
                                    rows.Add(reader["13_2"].ToString());
                                    rows.Add(reader["13_3"].ToString());
                                    rows.Add(reader["14"].ToString());
                                    rows.Add(reader["14_1"].ToString());
                                    rows.Add(reader["14_2"].ToString());
                                    rows.Add(reader["14_3"].ToString());
                                    rows.Add("");//
                                    rows.Add("");// 16
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");// 17
                                    rows.Add(reader["18"].ToString());
                                    rows.Add(reader["18_1"].ToString());
                                    rows.Add("");//
                                    rows.Add(reader["19"].ToString());
                                    rows.Add("");// 20
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");// 26
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["28"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["33"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["41"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["45"].ToString());
                                    rows.Add(reader["46"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["52"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add(reader["58"].ToString());
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//
                                    rows.Add("");//


                                    rowslist.Add(rows);
                                }


                            DataSet dataSet = new System.Data.DataSet("TS");
                            DataTable myDataTable = new DataTable("TS" + id);


                            DataRow myDataRow;

                            DataColumn myDataColumn = new DataColumn();
                            myDataColumn.DataType = System.Type.GetType("System.String");
                            myDataColumn.ColumnName = "rowNum";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = System.Type.GetType("System.String");
                            myDataColumn.ColumnName = "colCount";
                            myDataTable.Columns.Add(myDataColumn);

                            for (int i = 0; i < rowslist.Count; i++)
                            {
                                myDataColumn = new DataColumn();
                                myDataColumn.DataType = System.Type.GetType("System.String");
                                myDataColumn.ColumnName = "column" + (i + 1);
                                myDataTable.Columns.Add(myDataColumn);
                            }

                            //перебираем колонки в старых строках, в новой нотации строчки
                            for (int i = 0; i < rowslist[0].Count; i++)
                            {
                                myDataRow = myDataTable.NewRow();
                                myDataRow["rowNum"] = i + 1;
                                myDataRow["colCount"] = rowslist.Count;
                                for (int j = 0; j < rowslist.Count; j++)
                                {

                                    myDataRow["column" + (j + 1)] = rowslist[j][i];
                                }
                                myDataTable.Rows.Add(myDataRow);
                            }

                            result.Tables.Add(myDataTable);
                        }
                        else
                        {
                            /*
                            List<List<string>> rowslist = new List<List<string>>();
                            using (var reader = command.ExecuteReader())
                                while (reader.Read())
                                {
                                    List<string> rows = new List<string>();
                                    rows.Add(reader["2"].ToString());
                                }


                            */

                            /*

                            List<List<string>> rowslist = new List<List<string>>();

                            using (var reader = command.ExecuteReader())
                                while (reader.Read())
                                {
                                    var columns = new List<string>();

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        columns.Add(reader.GetName(i));
                                    }

                                }
                            */

                            



                            using (var adapter = new SqlDataAdapter(command))
                            {

                                var t = adapter.TableMappings.Count;
                                if (t> 0)
                                {
                                    var vv = adapter.TableMappings[0];
                                }

                                adapter.Fill(result, "TS" + id);
                            }
                        }
                        }
                }
                catch (Exception e)
                {
                    throw new Exception(query, e);
                }
            }

            return result;
        }
        
        /// <summary>
        /// Возвращает шаблон отчета по его внутреннему идентификатору.
        /// </summary>
        /// <param name="innerId">Внутренний идентификатор отчета.</param>
        /// <returns>Шаблон отчета.</returns>
        public string GetTemplate(int innerId)
        {
            var result = "";

            var query = string.Format("rpt.get_template @report_id = {0}", innerId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("rpt.get_template", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@report_id", innerId));

                        result = Convert.ToString(command.ExecuteScalar());
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