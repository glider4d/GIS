using Kts.Gis.Data;

using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления авторизации.
    /// </summary>
    public sealed class AuthorizationViewModel : BaseViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Название настройки, представляющей последний использованный идентификатор логина.
        /// </summary>
        private const string lastUsedLoginIdSetting = "LastUsedLoginId";

        /// <summary>
        /// Название настройки, представляющей последний использованный сервер.
        /// </summary>
        private const string lastUsedServerSetting = "LastUsedServer";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что имеется ли выбранный сервер.
        /// </summary>
        private bool hasSelectedServer;

        /// <summary>
        /// Значение, указывающее на то, что провалилась ли попытка подключения к выбранному источнику данных. Она используется из-за того, что нельзя отображать сообщение напрямую после проверки подключения.
        /// </summary>
        private bool isConnectionFailed;

        /// <summary>
        /// Выбранный логин.
        /// </summary>
        private LoginViewModel selectedLogin;

        /// <summary>
        /// Выбранный сервер.
        /// </summary>
        private SqlConnectionString selectedServer;

        


        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Данные серверов.
        /// </summary>
        private readonly Dictionary<SqlConnectionString, string> serverData;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса выполнения долгодлительной задачи.
        /// </summary>
        public event EventHandler<LongTimeTaskRequestedEventArgs> LongTimeTaskRequested;

        /// <summary>
        /// Событие запроса очистки пароля.
        /// </summary>
        public event EventHandler PasswordClearRequested;

        /// <summary>
        /// Событие успешной проверки пароля.
        /// </summary>
        public event EventHandler PasswordVerified;

        #endregion

        #region Конструкторы
        public IList<string> CountryList
        {
            get
            {
                return new List<string> { "USA", "Germany", "England" };
            }
        }

        WcfDataService m_wcfDataService;
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationViewModel"/>.
        /// </summary>
        /// <param name="serverData">Данные серверов.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public AuthorizationViewModel(WcfDataService wcfDataService, /*Dictionary<SqlConnectionString, string> serverData, */IMessageService messageService)
        {

            m_wcfDataService = wcfDataService;
            this.serverData = m_wcfDataService.GetServerList();//serverData;
            this.messageService = messageService;
            CitiesList = GetCities();
            LoadServers();
            /*
            LoadServers();
            */
            // Инициализируем команду.
            this.CheckPasswordCommand = new RelayCommand(this.ExecuteCheckPassword);
            
        }

        private City m_city = new City();
        public City selectedCity
        {
            get
            {
                return m_city;
            }
            set
            {
                m_city = value;
            }
        }

        #endregion

        #region Открытые свойства

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
                    this.DataService = m_wcfDataService;
                        //ssk11
                        //new WcfDataService(@"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");
                        //new SqlDataService(new SqlReconnector(this.selectedServer), @"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");




                    //((WcfDataService)DataService).getServerList();

                    // Проверяем возможность подключения к источнику данных.
                    /*
                    if (!this.DataService.CanConnect())
                    {
                        this.isConnectionFailed = true;

                        this.SelectedServer = null;

                        return;
                    }*/

                    //WcfDataService v = DataService as WcfDataService;
                    bool resultInitialize = false;
                    if (DataService is WcfDataService)
                    {
                        resultInitialize = (DataService as WcfDataService).MobileInitialize(SelectedServer.Name);
                        //    (DataService as WcfDataService).Initialize(SelectedServer.Name);
                        //var task = (DataService as WcfDataService).Initialize(SelectedServer.Name);
                        /*
                        this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(new WaitViewModel("Загрузка пользователей", "Пожалуйста подождите, идет загрузка пользователей...", async () =>
                        {
                            resultInitialize = (DataService as WcfDataService).MobileInitialize(SelectedServer.Name);
                            //resultInitialize = await (DataService as WcfDataService).Initialize(SelectedServer.Name);
                        }
                        //
                        )));*/


                        /*
                        do
                        {

                            Console.WriteLine(task.Status);
                        } while (task.Status == TaskStatus.WaitingForActivation);*/
                    }
                    //test.LoginAccessService.GetAll();
                    // Заполняем коллекцию логинов.
                    List<LoginViewModel> logins = null;

                    logins = LoginViewModel.GetLogins(this.DataService);
                    /*
                    this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(new WaitViewModel("Загрузка пользователей", "Пожалуйста подождите, идет загрузка пользователей...", async () =>
                    {
                        await Task.Factory.StartNew(() => logins = LoginViewModel.GetLogins(this.DataService));
                    })));*/
                    this.Logins.Clear();
                    this.Logins.AddRange(logins);

                    // Задаем выбранный логин.
                    var temp = this.Logins.FirstOrDefault(x => x.Id == lastLoginId);

                        if (temp == null)
                            this.SelectedLogin = this.Logins[0];
                        else
                            this.SelectedLogin = temp;
                    

                    this.HasSelectedServer = true;
                }
                else
                    this.HasSelectedServer = false;

                this.NotifyPropertyChanged(nameof(this.SelectedServer));
            }
        }

        public List<City> CitiesList { get; set; }


        public List<City> GetCities()
        {
            var cityes = new List<City>()
            {
                new City(){Key = 1, Value = "Mumbai"},
                new City(){Key = 2, Value ="Bengaluru"}
            };
            return cityes;
        }

        public class City
        {
            public int Key { get; set; }
            public string Value { get; set; }
        }
        

        /// <summary>
        /// Возвращает сервера.
        /// </summary>
        public AdvancedObservableCollection<SqlConnectionString> Servers
        {
            get;
        } = new AdvancedObservableCollection<SqlConnectionString>();

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет проверку правильности пароля.
        /// </summary>
        private void ExecuteCheckPassword()
        {
            if (this.SelectedLogin.IsPasswordCorrect(EncryptSHA256(this.Password), this.DataService))
            //if (true)
            {
                


                // Уведомляем о том, что пароль успешно проверен.
                this.PasswordVerified?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                DisplayAlert("Уведомление", "Пришло новое сообщение", "ОK");
                //this.messageService.ShowMessage("Неверный пароль", "Авторизация", MessageType.Error);
                //this.PasswordClearRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет сервер-пустышку.
        /// </summary>
        public void AddPlaceholder()
        {
            if (this.SelectedServer == null || this.SelectedServer.Server == "-1")
            {
                // Добавляем сервер-пустышку.
                this.Servers.Insert(0, new SqlConnectionString("-1", "-1", "-1", "-1", -1, "Выберите сервер"));

                this.SelectedServer = this.Servers[0];

                // Очищаем список логинов и введенный пароль.
                this.SelectedLogin = null;
                this.Logins.Clear();
                this.PasswordClearRequested?.Invoke(this, EventArgs.Empty);

                if (this.isConnectionFailed)
                {
                    this.isConnectionFailed = false;
                    
                    this.messageService.ShowMessage("Не удалось подключиться к выбранному серверу", "Авторизация", MessageType.Error);
                }
            }
        }

        /// <summary>
        /// Возвращает сервис доступа к функциям приложения.
        /// </summary>
        /// <returns>Сервис доступа к функциям приложения.</returns>
        /// 
        /*
        public AccessService GetAccessService()
        {
            if (this.SelectedLogin == null)
                return null;

            var temp = this.SelectedLogin.GetRestrictions(this.DataService);

            var permittedRegions = (List<int>)temp.First(x => x.Kind == AccessKind.PermittedRegions).Value;
            var permittedTypes = (List<int>)temp.First(x => x.Kind == AccessKind.PermittedTypes).Value;
            var isAdmin = (bool)temp.First(x => x.Kind == AccessKind.IsAdmin).Value;
            var canDraw = (bool)temp.First(x => x.Kind == AccessKind.CanDraw).Value;
            var canDrawIS = (bool)temp.First(x => x.Kind == AccessKind.CanDrawIS).Value;

            return new AccessService(permittedRegions, permittedTypes, isAdmin, this.SelectedLogin.GetRoleName(this.DataService), canDraw, canDrawIS);
        }*/

        /// <summary>
        /// Загружает серверы.
        /// </summary>
        public void LoadServers()
        {
            


            {
                // Получаем сервера.
                this.Servers.AddRange(this.serverData.Keys.ToList());

                // Выбираем сервер.
                //this.SelectedServer = this.Servers[0];
            }
        }

        /// <summary>
        /// Убирает сервер-пустышку.
        /// </summary>
        public void RemovePlaceholder()
        {
            // Если первый элемент является пустышкой, то удаляем его из коллекции серверов.
            var first = this.Servers.First();
            if (first.Server == "-1")
                this.Servers.Remove(first);
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Шифрует входную строку алгоритмом SHA-256.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <returns>Зашифрованная строка.</returns>
        public static string EncryptSHA256(string input)
        {
            var hash = new SHA256Managed();

            var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hashBytes);
        }

        #endregion
    }
}