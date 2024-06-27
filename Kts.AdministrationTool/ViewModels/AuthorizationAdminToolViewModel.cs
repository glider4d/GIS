using Kts.AdministrationTool.Data;
using Kts.AdministrationTool.ViewModels.Classes;
using Kts.Gis.Data;
using Kts.Gis.ViewModels;
using Kts.Messaging;
using Kts.Settings;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kts.AdministrationTool.ViewModels
{
    class AuthorizationAdminToolViewModel: BaseViewModel
    {

        /// <summary>
        /// Название настройки, представляющей последний использованный идентификатор логина.
        /// </summary>
        private const string lastUsedLoginIdSetting = "LastUsedLoginId";

        /// <summary>
        /// Название настройки, представляющей последний использованный сервер.
        /// </summary>
        private const string lastUsedServerSetting = "LastUsedServer";
        #region Открытые свойства
        /// <summary>
        /// Выбранный сервер.
        /// </summary>
        private SqlConnectionString selectedServer;
        /// <summary>
        /// Значение, указывающее на то, что провалилась ли попытка подключения к выбранному источнику данных. Она используется из-за того, что нельзя отображать сообщение напрямую после проверки подключения.
        /// </summary>
        private bool isConnectionFailed;

        /// <summary>
        /// Событие запроса выполнения долгодлительной задачи.
        /// </summary>
        public event EventHandler<LongTimeTaskRequstedAdminEventArgs> LongTimeTaskRequested;
        /// <summary>
        /// Возвращает сервера.
        /// </summary>
        public AdvancedObservableCollection<SqlConnectionString> Servers
        {
            get;
        } = new AdvancedObservableCollection<SqlConnectionString>();

        /// <summary>
        /// Возвращает или задает выбранный сервер.
        /// </summary>
        public SqlConnectionString SelectedServer
        {
            get
            {
                return this.selectedServer;
            }
            set
            {
                var lastLoginId = this.SelectedLogin != null ? this.SelectedLogin.Id : -1;

                this.selectedServer = value;

                if (this.SelectedServer != null && this.selectedServer.Server != "-1")
                {
                    // Создаем строку подключения.
                    var fileStorage = this.serverData.First(x => x.Key == this.selectedServer).Value;
                    SqlConnector tmpConnector = new SqlReconnector(this.selectedServer);
                    this.DataService = new SqlDataService(tmpConnector, @"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");

                    m_adminDataService = new SqlAdminAccessService(tmpConnector);
                    


                    // Проверяем возможность подключения к источнику данных.
                    if (!this.DataService.CanConnect())
                    {
                        this.isConnectionFailed = true;

                        this.SelectedServer = null;

                        return;
                    }

                    // Заполняем коллекцию логинов.
                    List<LoginViewModel> logins = null;
                    

                    //this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(null));


                    //logins = LoginViewModel.GetLogins(this.DataService);

                    

                    
                    this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequstedAdminEventArgs(new WaitScreenViewModel("Загрузка пользователей", "Пожалуйста подождите, идет загрузка пользователей...", async () =>
                    {
                        // Task.Factory.StartNew(() => logins = null);
                        await Task.Factory.StartNew(() => logins = LoginViewModel.GetLogins(this.DataService));
                    })));

                    

                    this.Logins.Clear();
                    this.Logins.AddRange(logins);

                    // Задаем выбранный логин.
                    if (lastLoginId == -1)
                    {
                        var serverName = Convert.ToString(settingService.Settings[lastUsedServerSetting]);
                        var login = this.Logins.FirstOrDefault(x => x.Id == (int)settingService.Settings[lastUsedLoginIdSetting]);
                        if (login != null && !string.IsNullOrEmpty(serverName) && serverName == this.selectedServer.Name)
                            this.SelectedLogin = login;
                        else
                            this.SelectedLogin = this.Logins[0];
                    }
                    else
                    {
                        var temp = this.Logins.FirstOrDefault(x => x.Id == lastLoginId);

                        if (temp == null)
                            this.SelectedLogin = this.Logins[0];
                        else
                            this.SelectedLogin = temp;
                    }

                    this.HasSelectedServer = true;
                }
                else
                    this.HasSelectedServer = false;

                this.NotifyPropertyChanged(nameof(this.SelectedServer));
            }
        }

        /// <summary>
        /// Возвращает команду проверки пароля.
        /// </summary>
        public RelayCommand CheckPasswordCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает сервис данных.
        /// </summary>
        public IDataService DataService
        {
            get;
            private set;
        }


        public SqlAdminAccessService m_adminDataService
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли выбранный сервер.
        /// </summary>
        public bool HasSelectedServer
        {
            get
            {
                return this.hasSelectedServer;
            }
            private set
            {
                this.hasSelectedServer = value;

                this.NotifyPropertyChanged(nameof(this.HasSelectedServer));
            }
        }

        /// <summary>
        /// Возвращает коллекцию логинов.
        /// </summary>
        public AdvancedObservableCollection<LoginViewModel> Logins
        {
            get;
        } = new AdvancedObservableCollection<LoginViewModel>();

        /// <summary>
        /// Возвращает или задает пароль.
        /// </summary>
        public string Password
        {
            get;
            set;
        } = "";

        /// <summary>
        /// Возвращает или задает выбранный логин.
        /// </summary>
        public LoginViewModel SelectedLogin
        {
            get
            {
                return this.selectedLogin;
            }
            set
            {
                this.selectedLogin = value;

                this.NotifyPropertyChanged(nameof(this.SelectedLogin));

                if (value != null)
                    this.DataService.LoggedUserId = value.Id;
            }
        }

        /// <summary>
        /// Событие успешной проверки пароля.
        /// </summary>
        public event EventHandler PasswordVerified;
        public event EventHandler PasswordClearRequested;
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что имеется ли выбранный сервер.
        /// </summary>
        private bool hasSelectedServer;
        /// <summary>
        /// Выбранный логин.
        /// </summary>
        private LoginViewModel selectedLogin;
        private readonly Dictionary<SqlConnectionString, string> serverData;
        IMessageService messageService;
        ISettingService settingService;
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationViewModel"/>.
        /// </summary>
        /// <param name="serverData">Данные серверов.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public AuthorizationAdminToolViewModel(Dictionary<SqlConnectionString, string> serverData, IMessageService messageService, ISettingService settingService)
        {
            this.serverData = serverData;
            this.messageService = messageService;
            this.settingService = settingService;

            // Инициализируем команду.
            this.CheckPasswordCommand = new RelayCommand(this.ExecuteCheckPassword);
        }

        private void ExecuteCheckPassword()
        {
            if (this.SelectedLogin.IsPasswordCorrect(EncryptSHA256(this.Password), this.DataService))
            //if ( true)
            {
                // Запоминаем в настройках выбранные сервер и логин.
                this.settingService.Settings[lastUsedServerSetting] = this.SelectedServer.Name;
                this.settingService.Settings[lastUsedLoginIdSetting] = this.SelectedLogin.Id;

                // Уведомляем о том, что пароль успешно проверен.
                this.PasswordVerified?.Invoke(this, EventArgs.Empty);


            }
            else
            {
                this.messageService.ShowMessage("Неверный пароль", "Авторизация", MessageType.Error);

                this.PasswordClearRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public static string EncryptSHA256(string input)
        {
            var hash = new SHA256Managed();

            var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// Загружает серверы.
        /// </summary>
        public void LoadServers()
        {
            var lastUsedServer = Convert.ToString(settingService.Settings[lastUsedServerSetting]);

            if (!this.serverData.Any(x => x.Key.Name == lastUsedServer))
            {
                // Получаем сервера.
                this.Servers.Add(new SqlConnectionString("-1", "-1", "-1", "-1", -1, "Выберите сервер"));
                this.Servers.AddRange(this.serverData.Keys.ToList());

                // Выбираем сервер.
                if (string.IsNullOrEmpty(lastUsedServer) || !this.serverData.Any(x => x.Key.Name == lastUsedServer))
                    this.SelectedServer = this.Servers[0];
                else
                    this.SelectedServer = this.Servers.FirstOrDefault(x => x.Name == lastUsedServer);
            }
            else
            {
                // Получаем сервера.
                this.Servers.AddRange(this.serverData.Keys.ToList());

                // Выбираем сервер.
                this.SelectedServer = this.Servers.FirstOrDefault(x => x.Name == lastUsedServer);
            }
        }



        #endregion
        #endregion
    }
}
