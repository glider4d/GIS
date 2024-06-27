using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления отображения документов объекта.
    /// </summary>
    internal sealed class DocumentsViewModel : BaseViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        private readonly AccessService accessService;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Объект, чьи документы нужно отобразить.
        /// </summary>
        private readonly IObjectModel obj;

        /// <summary>
        /// Схема.
        /// </summary>
        private readonly SchemaModel schema;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса добавления документа.
        /// </summary>
        public event EventHandler<ViewRequestedEventArgs<AddDocumentViewModel>> DocumentAddRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentsViewModel"/>.
        /// </summary>
        /// <param name="obj">Объект, чьи документы нужно отобразить.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public DocumentsViewModel(IObjectModel obj, string name, SchemaModel schema, AccessService accessService, IDataService dataService, IMessageService messageService)
        {
            this.obj = obj;
            this.schema = schema;
            this.accessService = accessService;
            this.dataService = dataService;
            this.messageService = messageService;

            this.ObjectName = name;

            this.AddDocumentCommand = new RelayCommand(this.ExecuteAddDocument, this.CanExecuteAddDocument);
            this.DeleteDocumentCommand = new RelayCommand(this.ExecuteDeleteDocument, this.CanExecuteDeleteDocument);
            this.OpenDocumentCommand = new RelayCommand(this.ExecuteOpenDocument, this.CanExecuteOpenDocument);

            // Загружаем список документов.
            this.Documents.AddRange(DocumentViewModel.GetDocuments(obj, schema, dataService));
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления документа.
        /// </summary>
        public RelayCommand AddDocumentCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду удаления документа.
        /// </summary>
        public RelayCommand DeleteDocumentCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список документов.
        /// </summary>
        public AdvancedObservableCollection<DocumentViewModel> Documents
        {
            get;
        } = new AdvancedObservableCollection<DocumentViewModel>();

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string ObjectName
        {
            get;
        }

        /// <summary>
        /// Возвращает команду открытия документа.
        /// </summary>
        public RelayCommand OpenDocumentCommand
        {
            get;
        }

        /// <summary>
        /// Выбранный документ.
        /// </summary>
        private DocumentViewModel selectedDocument;

        /// <summary>
        /// Возвращает или задает выбранный документ.
        /// </summary>
        public DocumentViewModel SelectedDocument
        {
            get
            {
                return this.selectedDocument;
            }
            set
            {
                this.selectedDocument = value;

                this.NotifyPropertyChanged(nameof(this.SelectedDocument));

                this.DeleteDocumentCommand.RaiseCanExecuteChanged();
                this.OpenDocumentCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает true, если можно выполнить добавление документа, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteAddDocument()
        {
            return this.accessService.CanModifyDocuments;
        }

        /// <summary>
        /// Возвращает true, если можно выполнить удаление документа, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDeleteDocument()
        {
            return this.SelectedDocument != null && this.accessService.CanModifyDocuments;
        }

        /// <summary>
        /// Возвращает true, если можно выполнить открытие документа, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteOpenDocument()
        {
            return this.SelectedDocument != null;
        }

        /// <summary>
        /// Выполняет добавление документа.
        /// </summary>
        private void ExecuteAddDocument()
        {
            var viewModel = new AddDocumentViewModel(this.obj.Id, this.messageService);

            var eventArgs = new ViewRequestedEventArgs<AddDocumentViewModel>(viewModel);

            this.DocumentAddRequested?.Invoke(this, eventArgs);

            if (eventArgs.Result)
            {
                var newDoc = new DocumentViewModel(viewModel.Result, this.schema, this.dataService);

                newDoc.Save(viewModel.Data);

                this.Documents.Add(newDoc);

                this.SelectedDocument = newDoc;
            }
        }

        /// <summary>
        /// Выполняет удаление документа.
        /// </summary>
        private void ExecuteDeleteDocument()
        {
            this.SelectedDocument.Delete();

            this.Documents.Remove(this.SelectedDocument);

            this.SelectedDocument = null;
        }

        /// <summary>
        /// Выполняет открытие документа.
        /// </summary>
        private void ExecuteOpenDocument()
        {
            if (!this.SelectedDocument.Open())
                this.messageService.ShowMessage("Не удалось открыть документ", "Открытие документа", MessageType.Error);
        }

        #endregion
    }
}