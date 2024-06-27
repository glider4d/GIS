using Kts.Messaging;
using Kts.Updater.Models;
using Kts.Updater.Services;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления пакета обновления.
    /// </summary>
    internal sealed class PackageViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly SqlDataService dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Модель.
        /// </summary>
        private readonly PackageModel model;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие нахождения ошибки.
        /// </summary>
        public event EventHandler ErrorFound;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PackageViewModel"/>.
        /// </summary>
        /// <param name="model">Модель.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public PackageViewModel(PackageModel model, SqlDataService dataService, IMessageService messageService)
        {
            this.model = model;
            this.dataService = dataService;
            this.messageService = messageService;

            this.CreateCommand = new RelayCommand(this.ExecuteCreate);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает количество объектов в пакете обновлений.
        /// </summary>
        public int Count
        {
            get
            {
                return this.model.SqlObjects.Count;
            }
        }

        /// <summary>
        /// Возвращает команду создания пакета обновлений.
        /// </summary>
        public RelayCommand CreateCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает дату создания пакета обновлений.
        /// </summary>
        public DateTime Created
        {
            get
            {
                return this.model.Date;
            }
        }

        /// <summary>
        /// Возвращает затрагиваемую базу данных.
        /// </summary>
        public string Database
        {
            get
            {
                return this.model.Database;
            }
        }

        /// <summary>
        /// Возвращает идентификатор пакета обновлений.
        /// </summary>
        public string Id
        {
            get
            {
                return this.model.Id;
            }
        }

        /// <summary>
        /// Возвращает затрагиваемый сервер.
        /// </summary>
        public string Server
        {
            get
            {
                return this.model.Server;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет создание пакета обновлений.
        /// </summary>
        private void ExecuteCreate()
        {
            if (this.dataService.AddPackage(this.model))
            {
                this.messageService.ShowMessage("Пакет обновления успешно создан", "Создание пакета обновления", MessageType.Information);

                this.CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.messageService.ShowMessage("Не удалось создать пакет обновления", "Создание пакета обновления", MessageType.Error);

                this.ErrorFound?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Создает новый пакет обновления.
        /// </summary>
        /// <param name="objects">Список объектов SQL.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <returns>Пакет обновления.</returns>
        public static PackageViewModel CreateNew(List<SqlObjectModel> objects, SqlDataService dataService, IMessageService messageService)
        {
            return new PackageViewModel(dataService.GetNewPackage(objects), dataService, messageService);
        }

        #endregion
    }
}