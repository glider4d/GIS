using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс доступа к данным документов.
    /// </summary>
    public interface IDocumentAccessService
    {
        #region Методы

        /// <summary>
        /// Добавляет заданный документ.
        /// </summary>
        /// <param name="doc">Добавляемый документ.</param>
        /// <param name="data">Данные документа.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор добавленного документа.</returns>
        Guid AddDocument(DocumentModel doc, byte[] data, SchemaModel schema);

        /// <summary>
        /// Удаляет заданный документ.
        /// </summary>
        /// <param name="doc">Удаляемый документ.</param>
        /// <param name="schema">Схема.</param>
        void DeleteDocument(DocumentModel doc, SchemaModel schema);

        /// <summary>
        /// Возвращает данные заданного документа.
        /// </summary>
        /// <param name="doc">Документ.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Данные документа.</returns>
        byte[] GetData(DocumentModel doc, SchemaModel schema);

        /// <summary>
        /// Возвращает список документов заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список документов.</returns>
        List<DocumentModel> GetDocuments(IObjectModel obj, SchemaModel schema);

        #endregion
    }
}