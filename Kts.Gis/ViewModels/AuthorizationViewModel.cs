using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Messaging;
using Kts.Settings;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления авторизации.
    /// </summary>
    internal sealed class AuthorizationViewModel : BaseViewModel
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

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private ISettingService settingService;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Данные серверов.
        /// </summary>
        Dictionary<SqlConnectionString, string> serverData;

        public Dictionary<SqlConnectionString, string> ServerData 
        {
            
            
            get
            {
                return serverData;
            }
            set
            {
                serverData = value;
            }
        }

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

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationViewModel"/>.
        /// </summary>
        /// <param name="serverData">Данные серверов.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public AuthorizationViewModel(Dictionary<SqlConnectionString, string> serverData, IMessageService messageService, ISettingService settingService)
        {
            this.serverData = serverData;
            this.messageService = messageService;
            this.settingService = settingService;

            // Инициализируем команду.
            this.CheckPasswordCommand = new RelayCommand(this.ExecuteCheckPassword);
            this.DictionaryChange = new RelayCommand(this.BrowseFolder);
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

        public string Description { get; set; }
        private string text;
        public string Text { 
            get 
            { 
                return text; 
            } 
            set 
            { 
                text = value;  
                this.NotifyPropertyChanged(nameof(this.Text)); 
            } 
        }

        public RelayCommand DictionaryChange
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
                    //ssk11
                    fileStorage = @"\\"+fileStorage;//
                    //fileStorage = "c:";
                    //var foleder = @"\\" + fileStorage + "\\Gis\\Errors\\";

                    this.DataService = 
                        //ssk11
                        //new WcfDataService(@"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");
                        new SqlDataService(new SqlReconnector(this.selectedServer),/* @"\\" + */fileStorage + "\\Gis\\Errors\\", /*@"\\" + */fileStorage + "\\Gis\\Images\\", /*@"\\" + */fileStorage + "\\Gis\\Thumbnails\\");




                    //((WcfDataService)DataService).getServerList();

                    // Проверяем возможность подключения к источнику данных.
                    if (!this.DataService.CanConnect())
                    {
                        this.isConnectionFailed = true;

                        this.SelectedServer = null;

                        return;
                    }
                    //WcfDataService v = DataService as WcfDataService;
                    bool resultInitialize = false;
                    if (DataService is WcfDataService)
                    {
                    //    (DataService as WcfDataService).Initialize(SelectedServer.Name);
                        //var task = (DataService as WcfDataService).Initialize(SelectedServer.Name);

                        this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(new WaitViewModel("Загрузка пользователей", "Пожалуйста подождите, идет загрузка пользователей...", async () =>
                        {
                            resultInitialize = await (DataService as WcfDataService).Initialize(SelectedServer.Name);
                            //resultInitialize = await (DataService as WcfDataService).Initialize(SelectedServer.Name);
                        }
                        //
                        )));


                        /*
                        do
                        {

                            Console.WriteLine(task.Status);
                        } while (task.Status == TaskStatus.WaitingForActivation);*/
                    }
                    //test.LoginAccessService.GetAll();
                    // Заполняем коллекцию логинов.
                    List<LoginViewModel> logins = null;
                    this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(new WaitViewModel("Загрузка пользователей", "Пожалуйста подождите, идет загрузка пользователей...", async () =>
                    {
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

















        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        /// 

        public Dictionary<SqlConnectionString, string> LoadServerData()
        {
            var serverDataLocal = new Dictionary<SqlConnectionString, string>();
            try
            {
                /*
                var test = new ServiceReference1.WcfFigureAccessServiceClient("BasicHttpBinding_IWcfFigureAccessService");
                var serverList = test.getServerList();



                test.Initialize("Якутск");
                test.GetLogins();*/
                string[] dataContent = new string[0];



                //var forcedDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\ForcedServerData";
                var forcedDataPath = App.installFolder + "\\Gis\\ForcedServerData";

                

                //string test = "\\\\" + ApplicationDeployment.CurrentDeployment.UpdateLocation.Host + "\\Gis\\ServerData";

#if RELEASE
            var dataPath = "\\\\" + ApplicationDeployment.CurrentDeployment.UpdateLocation.Host + "\\Gis\\ServerData";
#else

                //var dataPath = "\\\\172.16.4.58\\Gis\\ServerData";
                try
                {
                    //if (Text == null || Text.Length == 0)
                    {
                        Text = File.ReadAllText(".\\ServerListPath.ini");
                        Text += "\\ServerData";
                    }
                    if (!File.Exists(Text)) throw (new Exception());
                }
                catch(Exception e)
                {
                    var msg = e.Message;
                    Text = "\\\\172.16.4.58\\Gis\\ServerData";
                }
                var dataPath = Text;
                //var dataPath = "ServerData";
                //var dataPath = "e:\\1\\ServerData.txt";
#endif



                // Проверяем наличие файла, с вшитыми данными серверов.
                try
                {
                    if (File.Exists(forcedDataPath))
                        dataContent = File.ReadAllLines(forcedDataPath);
                    else
                        dataContent = File.ReadAllLines(dataPath);
                }
                catch
                {

                }

                // Также проверяем наличие файла ForcedAddress. Он использовался в ранних версиях приложения для ручного задания адреса сервера.
                ////if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\ForcedAddress"))
                
                if (File.Exists(App.installFolder + "\\Gis\\ForcedAddress"))
                    // Если такой существует, то мы обнуляем данные серверов. Это сделано для того, чтобы пользователи могли обратиться к нам для создания новой версии этого файла - ForcedServerData.
                    dataContent = new string[0];

                // Разбираем данные серверов.
                
                string dbAddress;
                string userId;
                string password;
                string name;
                string fileAddress;
                for (int i = 0; i < dataContent.Length; i += 5)
                {
                    dbAddress = dataContent[i];
                    userId = dataContent[i + 1];
                    password = dataContent[i + 2];
                    name = dataContent[i + 3];
                    fileAddress = dataContent[i + 4];

                    serverDataLocal.Add(new SqlConnectionString(dbAddress, userId, password, "w2AqAvKcgOh5iQuIodC6rw==", 90, name, true), fileAddress);


                    var t = "";
                }
            }
            catch
            {
                return null;
            }

            return serverDataLocal;
        }




















        private void BrowseFolder()
        {
            

            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = Description;
                dlg.SelectedPath = Text;
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Text = dlg.SelectedPath;
                    
                    //Text = File.ReadAllText(".\\ServerListPath.ini");
                    if ( !File.Exists(Text + "\\ServerData") )
                    {
                        MessageBox.Show("По указанному пути отсутствует файл ServerData");
                        return;
                    }
                    try { 



                        MessageBox.Show(Path.GetDirectoryName(Text));
                        File.WriteAllText(".\\ServerListPath.ini", Text);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка записи файла ServerListPath.ini");
                        return;
                    }
                    Text += "\\ServerData";
                    //BindingExpression be = GetBindingExpression(TextProperty);
                    //if (be != null)
                    //be.UpdateSource();
                    serverData = LoadServerData();
                    if ( serverData != null ) LoadServers();
                }
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
        }

        /// <summary>
        /// Загружает серверы.
        /// </summary>
        public void LoadServers()
        {
            var lastUsedServer = Convert.ToString(settingService.Settings[lastUsedServerSetting]);
            this.Servers?.Clear();
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