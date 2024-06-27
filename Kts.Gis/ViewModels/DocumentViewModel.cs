using Kts.Gis.Data;
using Kts.Gis.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления документа.
    /// </summary>
    internal sealed class DocumentViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Имя временного файла для открытия документов.
        /// </summary>
        private const string tempFileName = "5A8Y3J1W9N";

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Модель.
        /// </summary>
        private readonly DocumentModel model;

        /// <summary>
        /// Схема.
        /// </summary>
        private readonly SchemaModel schema;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentViewModel"/>.
        /// </summary>
        /// <param name="document">Модель.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        public DocumentViewModel(DocumentModel document, SchemaModel schema, IDataService dataService)
        {
            this.model = document;
            this.dataService = dataService;
            this.schema = schema;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает название документа.
        /// </summary>
        public string Name
        {
            get
            {
                return this.model.Name;
            }
        }

        /// <summary>
        /// Возвращает тип документа.
        /// </summary>
        public string Type
        {
            get
            {
                switch (this.model.Type)
                {
                    case DocumentType.ConnectionAct:
                        return "Акт подключения";

                    case DocumentType.DeactivationAct:
                        return "Акт отключения";

                    case DocumentType.PartitionAct:
                        return "Акт раздела границ";
                }

                return "Неизвестный тип";
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Удаляет документ.
        /// </summary>
        public void Delete()
        {
            this.dataService.DocumentAccessService.DeleteDocument(this.model, this.schema);
        }

        /// <summary>
        /// Открывает документ.
        /// </summary>
        public bool Open()
        {
            try
            {
                var data = this.dataService.DocumentAccessService.GetData(this.model, this.schema);

                var path = Path.Combine(Path.GetTempPath(), tempFileName + this.model.Extension);

                if (File.Exists(path))
                    File.Delete(path);

                using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    fileStream.Write(data, 0, data.Length);

                Process.Start(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Сохраняет документ с заданными данными.
        /// </summary>
        /// <param name="data">Данные документа.</param>
        public void Save(byte[] data)
        {
            var id = this.dataService.DocumentAccessService.AddDocument(this.model, data, this.schema);

            this.model.Id = id;
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает список документов заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список документов.</returns>
        public static List<DocumentViewModel> GetDocuments(IObjectModel obj, SchemaModel schema, IDataService dataService)
        {
            var docs = dataService.DocumentAccessService.GetDocuments(obj, schema);

            var result = new List<DocumentViewModel>();

            foreach (var doc in docs)
                result.Add(new DocumentViewModel(doc, schema, dataService));

            return result;
        }

        #endregion
    }
}