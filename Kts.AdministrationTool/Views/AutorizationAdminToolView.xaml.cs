using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Kts.Gis.Views;
using Kts.Gis.Data;
using System.IO;
using Kts.Utilities;
using Kts.AdministrationTool.ViewModels;
using Kts.Messaging;
using Kts.Settings;
using Kts.WpfUtilities;
using System.Net;
using System.Net.Sockets;
using Kts.AdministrationTool.ViewModels.Classes;

namespace Kts.AdministrationTool.Views
{



    /// <summary>
    /// Interaction logic for AutorizationAdminToolView.xaml
    /// </summary>
    public partial class AutorizationAdminToolView : Window
    {

        /// <summary>
        /// Значение, указывающее на то, что было ли отображено представление впервые.
        /// </summary>
        private bool m_isShowed;

        /// <summary>
        /// Значение, указывающее на то, что была ли выполнена авторизация.
        /// </summary>
        private bool m_isLogged;
        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;
        IDataService m_dataService;
        
        /// <summary>
        /// Событие завершения авторизации.
        /// </summary>
        //!!!

        public AutorizationAdminToolView(IMessageService messageService, ISettingService settingService)
        {
            this.InitializeComponent();

            

            this.messageService = messageService;
            this.settingService = settingService;
            

            this.Closed += Window_Closed;
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] dataContent = new string[0];

            var forcedDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\ForcedServerData";
            var dataPath = "\\\\172.16.4.58\\Gis\\ServerData";
            MessageBox.Show("" + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
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

            // Разбираем данные серверов.
            var serverData = new Dictionary<SqlConnectionString, string>();
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

                

                serverData.Add(new SqlConnectionString(dbAddress, userId, password, "w2AqAvKcgOh5iQuIodC6rw==", 90, name, true), fileAddress);
            }
            //serverData.Add(new SqlConnectionString("324", "432", "235", "w2AqAvKcgOh5iQuIodC6rw==", 90, "325", true), "test");

            //MessageBox.Show("dataContent.count"+dataContent.Count());

            // Инициализируем модель представления и подписываемся на событие успешного ввода пароля.
            var viewModel = new AuthorizationAdminToolViewModel(serverData, messageService, settingService);
            this.DataContext = viewModel;
            
            viewModel.LongTimeTaskRequested += this.ViewModel_LongTimeTaskRequested;
            viewModel.PasswordClearRequested += this.ViewModel_PasswordClearRequested;
            viewModel.PasswordVerified += this.ViewModel_PasswordVerified;
            viewModel.LoadServers();
        }

        private void ViewModel_PasswordVerified(object sender, EventArgs e)
        {/*

            var viewModel = this.DataContext as AuthorizationViewModel;

#if RELEASE
            // Проверяем совместимость приложения с источником данных.
            if (!viewModel.DataService.IsCompatible(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()))
            {
                this.messageService.ShowMessage("Текущая версия приложения несовместима с источником данных", "Авторизация", MessageType.Error);

                return;
            }
#endif
*/
            // Получаем IP-адрес.
            var ip = "";
            foreach (var entry in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (entry.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = entry.ToString();

                    break;
                }
            var viewModel = this.DataContext as AuthorizationAdminToolViewModel;



            var view = new WaitView(new WaitViewModel("preved", "medved",

                 () => viewModel.DataService.LoadDataAsync()));

            /*
            var view = new WaitView(new WaitViewModel("Загрузка справочников", "Пожалуйста подождите, идет загрузка справочников...", () => viewModel.DataService.LoadDataAsync()), this.Icon)
            {
                Owner = this
            };*/
            MessageBox.Show("Загрузка справочников");
            

            var mainView = new MainAdminWindow(viewModel.SelectedLogin.Name, viewModel.DataService, this.messageService, this.settingService, viewModel.m_adminDataService);

            mainView.Show();

            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            //this.isLogged = true;

            this.Close();
            //view.ShowDialog();
            /*
#if RELEASE
            if (viewModel.DataService.LoginAccessService.SetIsUserLogged(viewModel.SelectedLogin.Id, ip, ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(), true))
            {
#endif
            // Уведомляем о завершении авторизации.
            this.AuthorizationCompleted?.Invoke(this, new AuthorizationCompletedEventArgs(viewModel.SelectedLogin.Id, ip, viewModel.DataService));

            // Загружаем критически важные данные.
            var view = new WaitView(new WaitViewModel("Загрузка справочников", "Пожалуйста подождите, идет загрузка справочников...", () => viewModel.DataService.LoadDataAsync()), this.Icon)
            {
                Owner = this
            };
            view.ShowDialog();

            var mainView = new MainView(viewModel.SelectedLogin.Name, viewModel.GetAccessService(), viewModel.DataService, this.messageService, this.settingService, this.substrateService);

            mainView.Show();

            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            this.isLogged = true;

            this.Close();
#if RELEASE
            }
            else
                this.messageService.ShowMessage("Вход от данного пользователя уже был выполнен", "Авторизация", MessageType.Error);
#endif
*/
        }

        private void ViewModel_LongTimeTaskRequested(object sender, LongTimeTaskRequstedAdminEventArgs e)
        {
            var view = new WaitScreen(e.WaitViewModel, this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        private void ViewModel_PasswordClearRequested(object sender, EventArgs e)
        {
            this.passwordBox.Clear();
        }

        private void Window_Closed(object sender, EventArgs e) {
            
        }

        public int m_loginID
        {
            get;
            set;
        }
        /// <summary>
        /// Возвращает IP-адрес авторизованного пользователя.
        /// </summary>
        public string m_Ip
        {
            get;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
          //  (this.DataContext as AuthorizationAdminToolViewModel).AddPlaceholder();
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
        //    (this.DataContext as AuthorizationAdminToolViewModel).RemovePlaceholder();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as AuthorizationAdminToolViewModel).Password = (sender as PasswordBox).Password;
        }
    }
}
