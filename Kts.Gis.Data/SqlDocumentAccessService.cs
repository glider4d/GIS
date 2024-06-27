using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет сервис доступа к данным документов.
    /// </summary>
    public sealed partial class SqlDocumentAccessService : BaseSqlDataAccessService, IDocumentAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор авторизованного пользователя
        /// </summary>
        private readonly int loggedUserId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlDocumentAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        /// <param name="loggedUserId">Идентификатор авторизованного пользователя.</param>
        public SqlDocumentAccessService(SqlConnector connector, int loggedUserId) : base(connector)
        {
            this.loggedUserId = loggedUserId;
        }

        #endregion
    }

    // Реализация ICustomObjectAccessService.
    public sealed partial class SqlDocumentAccessService
    {
        #region Открытые методы

        /// <summary>
        /// Добавляет заданный документ.
        /// </summary>
        /// <param name="doc">Добавляемый документ.</param>
        /// <param name="data">Данные документа.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор добавленного документа.</returns>
        public Guid AddDocument(DocumentModel doc, byte[] data, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("GisDocs.dbo.add_doc{0} @object_id = {1}, @type_id = {2}, @name = {3}, @ext = {4}, @data = {5}, @user_id = {6}, @year = {7}", suffix, doc.ObjectId, (int)doc.Type, doc.Name, doc.Extension, data, this.loggedUserId, schema.Id);

            var id = Guid.Empty;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("GisDocs.dbo.add_doc" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;
                        
                        command.Parameters.Add(new SqlParameter("@object_id", doc.ObjectId));
                        command.Parameters.Add(new SqlParameter("@type_id", (int)doc.Type));
                        command.Parameters.Add(new SqlParameter("@name", doc.Name));
                        command.Parameters.Add(new SqlParameter("@ext", doc.Extension));
                        command.Parameters.Add(new SqlParameter("@data", data));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

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
        /// Удаляет заданный документ.
        /// </summary>
        /// <param name="doc">Удаляемый документ.</param>
        /// <param name="schema">Схема.</param>
        public void DeleteDocument(DocumentModel doc, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("GisDocs.dbo.delete_doc{0} @id = {1}, @user_id = {2}, @year = {3}", suffix, doc.Id, this.loggedUserId, schema.Id);

            try
            {
                using (var connection = Connector.GetConnection())
                    using (var command = new SqlCommand("GisDocs.dbo.delete_doc" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", doc.Id));
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
        /// Возвращает данные заданного документа.
        /// </summary>
        /// <param name="doc">Документ.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Данные документа.</returns>
        public byte[] GetData(DocumentModel doc, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("GisDocs.dbo.get_data{0} @id = {1}, @user_id = {2}, @year = {3}", suffix, doc.Id, this.loggedUserId, schema.Id);

            byte[] result = null;

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("GisDocs.dbo.get_data" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@id", doc.Id));
                        command.Parameters.Add(new SqlParameter("@user_id", this.loggedUserId));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        result = (byte[])command.ExecuteScalar();
                    }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }

        /// <summary>
        /// Возвращает список документов заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список документов.</returns>
        public List<DocumentModel> GetDocuments(IObjectModel obj, SchemaModel schema)
        {
            var suffix = schema.IsIS ? "_is" : "";

            var query = string.Format("GisDocs.dbo.get_docs{0} @object_id = {1}, @year = {2}", suffix, obj.Id, schema.Id);

            var result = new List<DocumentModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                    using (var command = new SqlCommand("GisDocs.dbo.get_docs" + suffix, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Убираем ограничение по времени выполнения команды.
                        command.CommandTimeout = 0;

                        command.Parameters.Add(new SqlParameter("@object_id", obj.Id));
                        if (!schema.IsIS)
                            command.Parameters.Add(new SqlParameter("@year", schema.Id));

                        Guid id;
                        int typeId;
                        string name;
                        string ext;

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Guid.Parse(reader["id"].ToString());
                                typeId = Convert.ToInt32(reader["type_id"]);
                                name = Convert.ToString(reader["name"]);
                                ext = Convert.ToString(reader["ext"]);

                                result.Add(new DocumentModel(id, obj.Id, name, ext, (DocumentType)typeId));
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