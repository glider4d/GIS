using Kts.Gis.Models;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления добавления документа.
    /// </summary>
    internal sealed class AddDocumentViewModel : BaseViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Идентификатор объекта, к которому привязывается документ.
        /// </summary>
        private readonly Guid objectId;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие обзора файла.
        /// </summary>
        public event EventHandler BrowseFile;

        /// <summary>
        /// Событие запроса закрытия.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddDocumentViewModel"/>.
        /// </summary>
        /// <param name="objectId">Идентификатор объекта, к которому привязывается документ.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public AddDocumentViewModel(Guid objectId, IMessageService messageService)
        {
            this.objectId = objectId;
            this.messageService = messageService;

            this.BrowseCommand = new RelayCommand(this.ExecuteBrowse);
            this.SaveCommand = new RelayCommand(this.ExecuteSave);

            this.SelectedType = this.Types[0].Item1;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду обзора документа.
        /// </summary>
        public RelayCommand BrowseCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает данные документа.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Название документа.
        /// </summary>
        private string name;

        /// <summary>
        /// Возвращает или задает название документа.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;

                this.NotifyPropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Путь к документу.
        /// </summary>
        private string path;

        /// <summary>
        /// Возвращает или задает путь к документу.
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;

                this.NotifyPropertyChanged(nameof(this.Path));

                this.Name = System.IO.Path.GetFileNameWithoutExtension(value);
            }
        }

        /// <summary>
        /// Возвращает или задает добавленный документ.
        /// </summary>
        public DocumentModel Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду сохранения.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный тип.
        /// </summary>
        public DocumentType SelectedType
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает список типов документов.
        /// </summary>
        public List<Tuple<DocumentType, string>> Types
        {
            get;
        } = new List<Tuple<DocumentType, string>>()
        {
            new Tuple<DocumentType, string>(DocumentType.ConnectionAct, "Акт подключения"),
            new Tuple<DocumentType, string>(DocumentType.DeactivationAct, "Акт отключения"),
            new Tuple<DocumentType, string>(DocumentType.PartitionAct, "Акт раздела границ")
        };

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет обзор документа.
        /// </summary>
        private void ExecuteBrowse()
        {
            this.BrowseFile?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет сохранение документа.
        /// </summary>
        private void ExecuteSave()
        {
            if (!File.Exists(this.Path))
            {
                this.messageService.ShowMessage("Не удалось найти файл по заданному пути", "Добавление документа", MessageType.Error);

                return;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                this.messageService.ShowMessage("Название документа не должно быть пустым", "Добавление документа", MessageType.Error);

                return;
            }

            try
            {
                using (var fileStream = new FileStream(this.Path, FileMode.Open))
                {
                    this.Data = new byte[fileStream.Length];

                    fileStream.Read(this.Data, 0, this.Data.Length);
                }
            }
            catch
            {
                this.messageService.ShowMessage("Не удалось выполнить чтение файла", "Добавление документа", MessageType.Error);

                return;
            }

            this.Result = new DocumentModel(DocumentModel.DefaultId, this.objectId, this.Name, System.IO.Path.GetExtension(this.Path), this.SelectedType);

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}