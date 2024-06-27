using Kts.Updater.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Kts.Updater.Services
{
    /// <summary>
    /// Представляет сервис доступа к данным.
    /// </summary>
    internal sealed class SqlDataService
    {
        #region Закрытые поля

        /// <summary>
        /// Строка подключения.
        /// </summary>
        private SqlConnectionString connectionString;

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Возвращает идентификатор нового пакета обновления.
        /// </summary>
        /// <param name="objectCount">Количество объектов в пакете.</param>
        /// <returns>Идентификатор пакета обновления.</returns>
        private string GetNewPackageId(int objectCount)
        {
            var result = "";

            using (var connection = new SqlConnection(connectionString.ToString()))
                using (var command = new SqlCommand("DECLARE @date DATETIME SET @date = GETDATE() SELECT CAST(YEAR(@date) AS VARCHAR) + CASE WHEN LEN(CAST(MONTH(@date) AS VARCHAR)) < 2 THEN '0' + CAST(MONTH(@date) AS VARCHAR) ELSE CAST(MONTH(@date) AS VARCHAR) END + CASE WHEN LEN(CAST(DAY(@date) AS VARCHAR)) < 2 THEN '0' + CAST(DAY(@date) AS VARCHAR) ELSE CAST(DAY(@date) AS VARCHAR) END", connection))
                {
                    connection.Open();

                    result = Convert.ToString(command.ExecuteScalar());
                }

            result += "." + this.connectionString.Database + "." + objectCount.ToString();

            return result;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет заданный пакет обновления.
        /// </summary>
        /// <param name="package">Пакет обновления.</param>
        /// <returns>true, если пакет успешно добавлен, иначе - false.</returns>
        public bool AddPackage(PackageModel package)
        {
            try
            {
                // Добавляем пакет.
                using (var connection = new SqlConnection(this.connectionString.ToString()))
                    using (var command = new SqlCommand("INSERT INTO Import_fil.dbo.UpdateInfoTable SELECT @id, @database, @date, @count, 1", connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@id", package.Id);
                        command.Parameters.AddWithValue("@database", package.Database);
                        command.Parameters.AddWithValue("@date", package.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@count", package.SqlObjects.Count);

                        command.ExecuteNonQuery();
                    }

                // Добавляем все объекты пакета.
                for (int i = 0; i < package.SqlObjects.Count; i++)
                {
                    var obj = package.SqlObjects[i];

                    using (var connection = new SqlConnection(this.connectionString.ToString()))
                        using (var command = new SqlCommand("INSERT INTO Import_fil.dbo.UpdateTable SELECT @id, @order_id, @name, @drop_command, @command", connection))
                        {
                            connection.Open();
                            
                            command.Parameters.AddWithValue("@id", package.Id);
                            command.Parameters.AddWithValue("@order_id", i);
                            command.Parameters.AddWithValue("@name", obj.Name);
                            command.Parameters.AddWithValue("@drop_command", obj.DropCommand);
                            command.Parameters.AddWithValue("@command", obj.Command);

                            command.ExecuteNonQuery();
                        }
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Возвращает данные заданного справочника.
        /// </summary>
        /// <param name="obj">Объект SQL.</param>
        public void GetCatalogData(SqlObjectModel obj)
        {
            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("EXEC Import_fil.dbo.get_catalog_data @name, @database", connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@name", obj.Name);
                    command.Parameters.AddWithValue("@database", this.connectionString.Database);

                    obj.Command = Convert.ToString(command.ExecuteScalar());
                }
        }

        /// <summary>
        /// Возвращает список справочников.
        /// </summary>
        /// <returns>Список таблиц.</returns>
        public List<SqlObjectModel> GetCatalogs()
        {
            var result = new List<SqlObjectModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("SELECT name name FROM sys.all_objects WHERE type = 'U' ORDER BY name", connection))
                {
                    connection.Open();

                    string name;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);

                            result.Add(new SqlObjectModel(name));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает список баз данных заданного сервера.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        /// <returns>Список баз данных.</returns>
        public List<string> GetDatabases(SqlConnectionString connectionString)
        {
            var result = new List<string>();

            using (var connection = new SqlConnection(connectionString.ToString()))
                using (var command = new SqlCommand("SELECT name FROM sysdatabases", connection))
                {
                    connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            result.Add(Convert.ToString(reader["name"]));
                }

            return result;
        }

        /// <summary>
        /// Возвращает новый пакет обновления.
        /// </summary>
        /// <param name="objects">Объекты пакета обновления.</param>
        /// <returns>Пакет обновления.</returns>
        public PackageModel GetNewPackage(List<SqlObjectModel> objects)
        {
            PackageModel result;

            var id = this.GetNewPackageId(objects.Count);

            DateTime date;

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand("SELECT GETDATE()", connection))
                {
                    connection.Open();

                    date = Convert.ToDateTime(command.ExecuteScalar());
                }

            result = new PackageModel(id, this.connectionString.Name, this.connectionString.Database, date, objects);

            return result;
        }

        /// <summary>
        /// Возвращает список скалярных функций.
        /// </summary>
        /// <returns>Список скалярных функций.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public List<SqlObjectModel> GetScalarFunctions()
        {
            var result = new List<SqlObjectModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand(string.Format("SELECT ao.name name, OBJECT_DEFINITION(ao.object_id) command, 'IF EXISTS(SELECT * FROM sys.all_objects WHERE is_ms_shipped = 0 AND schema_id = schema_id(''' + s.name + ''') AND type = ''FN'' AND name = ''' + ao.name + ''') DROP ' + ' FUNCTION ' + s.name + '.' + ao.name drop_command, s.name [schema], modify_date modified FROM sys.all_objects ao INNER JOIN {0}.sys.schemas s ON ao.schema_id = s.schema_id WHERE type = 'FN' AND is_ms_shipped = 0 ORDER BY modify_date DESC", connection.Database), connection))
                {
                    connection.Open();

                    string name;
                    string cmd;
                    string dropCmd;
                    string schema;
                    DateTime modified;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);
                            cmd = Convert.ToString(reader["command"]);
                            dropCmd = Convert.ToString(reader["drop_command"]);
                            schema = Convert.ToString(reader["schema"]);
                            modified = Convert.ToDateTime(reader["modified"]);

                            result.Add(new SqlObjectModel(name, cmd, dropCmd, schema, modified));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает список хранимых процедур.
        /// </summary>
        /// <returns>Список хранимых процедур.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public List<SqlObjectModel> GetStoredProcedures()
        {
            var result = new List<SqlObjectModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand(string.Format("SELECT ao.name name, OBJECT_DEFINITION(ao.object_id) command, 'IF EXISTS(SELECT * FROM sys.all_objects WHERE is_ms_shipped = 0 AND schema_id = schema_id(''' + s.name + ''') AND type = ''P'' AND name = ''' + ao.name + ''') DROP ' + ' PROCEDURE ' + s.name + '.' + ao.name drop_command, s.name [schema], modify_date modified FROM sys.all_objects ao INNER JOIN {0}.sys.schemas s ON ao.schema_id = s.schema_id WHERE type = 'P' AND is_ms_shipped = 0 ORDER BY modify_date DESC", connection.Database), connection))
                {
                    connection.Open();

                    string name;
                    string cmd;
                    string dropCmd;
                    string schema;
                    DateTime modified;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);
                            cmd = Convert.ToString(reader["command"]);
                            dropCmd = Convert.ToString(reader["drop_command"]);
                            schema = Convert.ToString(reader["schema"]);
                            modified = Convert.ToDateTime(reader["modified"]);

                            result.Add(new SqlObjectModel(name, cmd, dropCmd, schema, modified));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает список табличных функций.
        /// </summary>
        /// <returns>Список табличных функций.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public List<SqlObjectModel> GetTableFunctions()
        {
            var result = new List<SqlObjectModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand(string.Format("SELECT ao.name name, OBJECT_DEFINITION(ao.object_id) command, 'IF EXISTS(SELECT * FROM sys.all_objects WHERE is_ms_shipped = 0 AND schema_id = schema_id(''' + s.name + ''') AND type = ''TF'' AND name = ''' + ao.name + ''') DROP ' + ' FUNCTION ' + s.name + '.' + ao.name drop_command, s.name [schema], modify_date modified FROM sys.all_objects ao INNER JOIN {0}.sys.schemas s ON ao.schema_id = s.schema_id WHERE type = 'TF' AND is_ms_shipped = 0 ORDER BY modify_date DESC", connection.Database), connection))
                {
                    connection.Open();

                    string name;
                    string cmd;
                    string dropCmd;
                    string schema;
                    DateTime modified;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);
                            cmd = Convert.ToString(reader["command"]);
                            dropCmd = Convert.ToString(reader["drop_command"]);
                            schema = Convert.ToString(reader["schema"]);
                            modified = Convert.ToDateTime(reader["modified"]);

                            result.Add(new SqlObjectModel(name, cmd, dropCmd, schema, modified));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает список таблиц.
        /// </summary>
        /// <returns>Список таблиц.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public List<SqlObjectModel> GetTables()
        {
            var result = new List<SqlObjectModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand(string.Format("SELECT name name, {0}.dbo.fn_getSQLTable(name) command, modify_date modified FROM sys.all_objects WHERE type = 'U' ORDER BY modify_date DESC", connection.Database), connection))
                {
                    connection.Open();

                    string name;
                    string cmd;
                    DateTime modified;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);
                            cmd = Convert.ToString(reader["command"]);
                            modified = Convert.ToDateTime(reader["modified"]);

                            result.Add(new SqlObjectModel(name, cmd, modified));
                        }
                }

            return result;
        }

        /// <summary>
        /// Возвращает список представлений.
        /// </summary>
        /// <returns>Список представлений.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public List<SqlObjectModel> GetViews()
        {
            var result = new List<SqlObjectModel>();

            using (var connection = new SqlConnection(this.connectionString.ToString()))
                using (var command = new SqlCommand(string.Format("SELECT ao.name name, OBJECT_DEFINITION(ao.object_id) command, 'IF EXISTS(SELECT * FROM sys.all_objects WHERE is_ms_shipped = 0 AND schema_id = schema_id(''' + s.name + ''') AND type = ''V'' AND name = ''' + ao.name + ''') DROP ' + ' VIEW ' + s.name + '.' + ao.name drop_command, s.name [schema], modify_date modified FROM sys.all_objects ao INNER JOIN {0}.sys.schemas s ON ao.schema_id = s.schema_id WHERE type = 'V' AND is_ms_shipped = 0 ORDER BY modify_date DESC", connection.Database), connection))
                {
                    connection.Open();

                    string name;
                    string cmd;
                    string dropCmd;
                    string schema;
                    DateTime modified;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["name"]);
                            cmd = Convert.ToString(reader["command"]);
                            dropCmd = Convert.ToString(reader["drop_command"]);
                            schema = Convert.ToString(reader["schema"]);
                            modified = Convert.ToDateTime(reader["modified"]);

                            result.Add(new SqlObjectModel(name, cmd, dropCmd, schema, modified));
                        }
                }

            return result;
        }

        /// <summary>
        /// Задает строку подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        public void SetConnectionString(SqlConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion
    }
}