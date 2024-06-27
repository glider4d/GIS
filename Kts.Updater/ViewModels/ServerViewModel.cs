using Kts.Messaging;
using Kts.Updater.Services;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора сервера.
    /// </summary>
    internal sealed class ServerViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Выбранная база данных.
        /// </summary>
        private string selectedDatabase;

        /// <summary>
        /// Выбранный сервер.
        /// </summary>
        private Server selectedServer;

        /// <summary>
        /// Значение, указывающее на то, что была ли выбрана база данных. Используется для отметки первого выбора базы данных.
        /// </summary>
        private bool wasDatabaseSelected;

        #endregion

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
        /// Сервис настроек.
        /// </summary>
        private readonly SettingService settingService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие выбора всех нужных данных.
        /// </summary>
        public event EventHandler DataSelected;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ServerViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public ServerViewModel(SqlDataService dataService, IMessageService messageService, SettingService settingService)
        {
            this.messageService = messageService;
            this.dataService = dataService;
            this.settingService = settingService;

            this.NextCommand = new RelayCommand(this.ExecuteNext, this.CanExecuteNext);

            this.Servers = new List<Server>()
            {
                new Server("Основной сервер", "172.16.3.85", "sa", "gjghj,eqgjl,thb"),
                new Server("Первый тестовый", "vvs", "sa", "159357")
            };

            // Выбираем сервер.
            var lastServer = this.settingService.LastServer;
            if (string.IsNullOrEmpty(lastServer))
                this.SelectedServer = this.Servers[0];
            else
            {
                var selectedServer = this.Servers.FirstOrDefault(x => x.Address == lastServer);

                if (selectedServer != null)
                    this.SelectedServer = selectedServer;
                else
                    this.SelectedServer = this.Servers[0];
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает строку подключения к базе данных.
        /// </summary>
        public SqlConnectionString ConnectionString
        {
            get
            {
                return new SqlConnectionString(this.SelectedServer.Address, this.SelectedServer.User, this.SelectedServer.Password, !string.IsNullOrEmpty(this.SelectedDatabase) ? this.SelectedDatabase : "master", 0, this.SelectedServer.Name);
            }
        }

        /// <summary>
        /// Возвращает базы данных.
        /// </summary>
        public AdvancedObservableCollection<string> Databases
        {
            get;
        } = new AdvancedObservableCollection<string>();

        /// <summary>
        /// Возвращает команду перехода на следующее представление.
        /// </summary>
        public RelayCommand NextCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранную базу данных.
        /// </summary>
        public string SelectedDatabase
        {
            get
            {
                return this.selectedDatabase;
            }
            set
            {
                this.selectedDatabase = value;

                // Запоминаем выбранную базу данных.
                if (value != null)
                    this.settingService.LastDatabase = value;

                this.NotifyPropertyChanged(nameof(this.SelectedDatabase));

                this.NextCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный сервер.
        /// </summary>
        public Server SelectedServer
        {
            get
            {
                return this.selectedServer;
            }
            set
            {
                this.selectedServer = value;

                // Запоминаем выбранный сервер.
                this.settingService.LastServer = value.Address;

                // Загружаем список баз данных.
                var databases = this.dataService.GetDatabases(new SqlConnectionString(value.Address, value.User, value.Password, "master", 0, value.Name));
                this.Databases.Clear();
                this.Databases.AddRange(databases);

                // Выбираем базу данных.
                if (this.wasDatabaseSelected)
                    this.SelectedDatabase = this.Databases.FirstOrDefault();
                else
                {
                    var lastDatabase = this.settingService.LastDatabase;

                    if (string.IsNullOrEmpty(lastDatabase))
                        this.SelectedDatabase = this.Databases.FirstOrDefault();
                    else
                    {
                        var database = this.Databases.FirstOrDefault(x => x == lastDatabase);

                        if (database != null)
                            this.SelectedDatabase = database;
                        else
                            this.SelectedDatabase = this.Databases.FirstOrDefault();
                    }

                    this.wasDatabaseSelected = true;
                }

                this.NextCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает сервера.
        /// </summary>
        public List<Server> Servers
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду перехода на следующее представление.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteNext()
        {
            return this.SelectedServer != null && !string.IsNullOrEmpty(this.SelectedDatabase);
        }

        /// <summary>
        /// Выполняет переход на следующее представление.
        /// </summary>
        private void ExecuteNext()
        {
            this.DataSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}