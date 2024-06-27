using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным кастомных объектов.
    /// </summary>
    public sealed partial class SqlCustomObjectAccessService : BaseSqlDataAccessService, ICustomObjectAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор авторизованного пользователя
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlCustomObjectAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор авторизованного пользователя.</param>
        public SqlCustomObjectAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация ICustomObjectAccessService.
    public sealed partial class SqlCustomObjectAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Удаляет заголовок "Утверждено"/"Согласовано" из источника данных.
        /// </summary>
        /// <param name="header">Удаляемый заголовок.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_approved_header{0} @id = {1}, @type = {2}, @user_id = {3}, @year = {4}", suffix, header.Id, (int)header.Type, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("remove_approved_header" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", header.Id));
                        command.Parameters.Add(new SqlParameter("@type", (int)header.Type));
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
        /// Удаляет таблицу с данным о протяженностях труб, разбитых по диаметрам, из источника данных.
        /// </summary>
        /// <param name="table">Удаляемая таблица.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteLengthPerDiameterTable(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_lpd_table{0} @id = {1}, @user_id = {2}, @year = {3}", suffix, table.Id, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("remove_lpd_table" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", table.Id));
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
        /// Возвращает список заголовков "Утверждено"/"Согласовано".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список заголовков "Утверждено"/"Согласовано".</returns>
        public List<ApprovedHeaderModel> GetApprovedHeaders(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_approved_headers{0} @year = {1}, @city_id = {2}", suffix, schema.Id, cityId);

            var result = new List<ApprovedHeaderModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_approved_headers" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        Guid id;
                        Guid layerId;
                        double x;
                        double y;
                        int fontSize;
                        string post;
                        string name;
                        int year;
                        int headerType;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                if (reader["layer_id"] != DBNull.Value)
                                    layerId = Guid.Parse(reader["layer_id"].ToString());
                                else
                                    layerId = Guid.Empty;
                                x = Convert.ToDouble(reader["x"]);
                                y = Convert.ToDouble(reader["y"]);
                                fontSize = Convert.ToInt32(reader["font_size"]);
                                post = Convert.ToString(reader["post"]);
                                name = Convert.ToString(reader["name"]);
                                year = Convert.ToInt32(reader["text_year"]);
                                headerType = Convert.ToInt32(reader["type"]);

                                result.Add(new ApprovedHeaderModel(id, cityId, layerId, new Point(x, y), fontSize, post, name, year, (ApprovedHeaderType)headerType));
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
        /// Возвращает список таблиц с данными о протяженностях труб, разбитых по диаметрам.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <returns>Список таблиц с данными о протяженностях труб, разбитых по диаметрам.</returns>
        public List<LengthPerDiameterTableModel> GetLengthPerDiameterTables(SchemaModel schema, int cityId)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("get_lpd_tables{0} @year = {1}, @city_id = {2}", suffix, schema.Id, cityId);

            var result = new List<LengthPerDiameterTableModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("get_lpd_tables" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", cityId));

                        Guid id;
                        Guid boilerId;
                        Guid layerId;
                        double x;
                        double y;
                        int fontSize;
                        string title;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                boilerId = Guid.Parse(reader["boiler_id"].ToString());
                                if (reader["layer_id"] != DBNull.Value)
                                    layerId = Guid.Parse(reader["layer_id"].ToString());
                                else
                                    layerId = Guid.Empty;
                                x = Convert.ToDouble(reader["x"]);
                                y = Convert.ToDouble(reader["y"]);
                                fontSize = Convert.ToInt32(reader["font_size"]);
                                title = Convert.ToString(reader["title"]);

                                result.Add(new LengthPerDiameterTableModel(id, cityId, boilerId, layerId, new Point(x, y), fontSize, title));
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
        /// Помечает заголовок "Утверждено"/"Согласовано" на удаление из источника данных.
        /// </summary>
        /// <param name="header">Заголовок, который нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        public void MarkDeleteApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_approved_header{0} @id = {1}, @type = {2}, @user_id = {3}, @year = {4}, @right_now = {5}", suffix, header.Id, (int)header.Type, this.loggedUserId, schema.Id, false);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("remove_approved_header" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", header.Id));
                        command.Parameters.Add(new SqlParameter("@type", (int)header.Type));
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
        /// Помечает таблицу с данными о протяженностях труб, разбитых по диаметрам, на удаление из источника данных.
        /// </summary>
        /// <param name="obj">Таблица, которую нужно удалить.</param>
        /// <param name="schema">Схема.</param>
        public void MarkDeleteLengthPerDiameterTable(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("remove_lpd_table{0} @id = {1}, @user_id = {2}, @year = {3}, @right_now = {4}", suffix, table.Id, this.loggedUserId, schema.Id, false);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("remove_lpd_table" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", table.Id));
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
        /// Обновляет данные нового заголовка "Утверждено"/"Согласовано" в источнике данных.
        /// </summary>
        /// <param name="header">Новый заголовок.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор нового заголовка.</returns>
        public Guid UpdateNewApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_approved_header{0} @id = null, @x = {1}, @y = {2}, @font_size = {3}, @layer_id = {4}, @post = {5}, @name = {6}, @inner_year = {7}, @type = {8}, @user_id = {9}, @year = {10}, @city_id = {11}", suffix, header.Position.X, header.Position.Y, header.FontSize, header.LayerId, header.Post, header.Name, header.Year, (int)header.Type, this.loggedUserId, schema.Id, header.CityId);

            var id = Guid.Empty;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_approved_header" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@x", header.Position.X));
                        command.Parameters.Add(new SqlParameter("@y", header.Position.Y));
                        command.Parameters.Add(new SqlParameter("@font_size", header.FontSize));
                        if (header.LayerId != Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@layer_id", header.LayerId));
                        else
                            command.Parameters.Add(new SqlParameter("@layer_id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@post", header.Post));
                        command.Parameters.Add(new SqlParameter("@name", header.Name));
                        command.Parameters.Add(new SqlParameter("@inner_year", header.Year));
                        command.Parameters.Add(new SqlParameter("@type", (int)header.Type));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", header.CityId));

                        id = Guid.Parse(command.ExecuteScalar().ToString());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return id;
        }

        /// <summary>
        /// Обновляет данные новой таблицы в источнике данных.
        /// </summary>
        /// <param name="table">Обновляемая таблица.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор новой таблицы.</returns>
        public Guid UpdateNewObject(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_lpd_table{0} @id = null, @title = {1}, @x = {2}, @y = {3}, @font_size = {4}, @layer_id = {5}, @boiler_id = {6}, @user_id = {7}, @year = {8}, @city_id = {9}", suffix, table.Title, table.Position.X, table.Position.Y, table.FontSize, table.LayerId, table.BoilerId, this.loggedUserId, schema.Id, table.CityId);

            var id = Guid.Empty;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_lpd_table" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@title", table.Title));
                        command.Parameters.Add(new SqlParameter("@x", table.Position.X));
                        command.Parameters.Add(new SqlParameter("@y", table.Position.Y));
                        command.Parameters.Add(new SqlParameter("@font_size", table.FontSize));
                        if (table.LayerId != Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@layer_id", table.LayerId));
                        else
                            command.Parameters.Add(new SqlParameter("@layer_id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@boiler_id", table.BoilerId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", table.CityId));

                        id = Guid.Parse(command.ExecuteScalar().ToString());
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return id;
        }

        /// <summary>
        /// Обновляет данные заголовка "Утверждено"/"Согласовано" в источнике данных.
        /// </summary>
        /// <param name="header">Обновляемый заголовок.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_approved_header{0} @id = {1}, @x = {2}, @y = {3}, @font_size = {4}, @layer_id = {5}, @post = {6}, @name = {7}, @inner_year = {8}, @type = {9}, @user_id = {10}, @year = {11}, @city_id = {12}", suffix, header.Id, header.Position.X, header.Position.Y, header.FontSize, header.LayerId, header.Post, header.Name, header.Year, (int)header.Type, this.loggedUserId, schema.Id, header.CityId);

            var id = Guid.Empty;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_approved_header" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", header.Id));
                        command.Parameters.Add(new SqlParameter("@x", header.Position.X));
                        command.Parameters.Add(new SqlParameter("@y", header.Position.Y));
                        command.Parameters.Add(new SqlParameter("@font_size", header.FontSize));
                        if (header.LayerId != Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@layer_id", header.LayerId));
                        else
                            command.Parameters.Add(new SqlParameter("@layer_id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@post", header.Post));
                        command.Parameters.Add(new SqlParameter("@name", header.Name));
                        command.Parameters.Add(new SqlParameter("@inner_year", header.Year));
                        command.Parameters.Add(new SqlParameter("@type", (int)header.Type));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", header.CityId));

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }
        }

        /// <summary>
        /// Обновляет данные таблицы в источнике данных.
        /// </summary>
        /// <param name="table">Обновляемая таблица.</param>
        /// <param name="schema">Схема.</param>
        public void UpdateObject(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("update_lpd_table{0} @id = {1}, @title = {2}, @x = {3}, @y = {4}, @font_size = {5}, @layer_id = {6}, @boiler_id = {7}, @user_id = {8}, @year = {9}, @city_id = {10}", suffix, table.Id, table.Title, table.Position.X, table.Position.Y, table.FontSize, table.LayerId, table.BoilerId, this.loggedUserId, schema.Id, table.CityId);

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("update_lpd_table" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", table.Id));
                        command.Parameters.Add(new SqlParameter("@title", table.Title));
                        command.Parameters.Add(new SqlParameter("@x", table.Position.X));
                        command.Parameters.Add(new SqlParameter("@y", table.Position.Y));
                        command.Parameters.Add(new SqlParameter("@font_size", table.FontSize));
                        if (table.LayerId != Guid.Empty)
                            command.Parameters.Add(new SqlParameter("@layer_id", table.LayerId));
                        else
                            command.Parameters.Add(new SqlParameter("@layer_id", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@boiler_id", table.BoilerId));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));
                        command.Parameters.Add(new SqlParameter("@city_id", table.CityId));

                        command.ExecuteNonQuery();
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