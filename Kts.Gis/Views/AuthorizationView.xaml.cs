using Kts.Gis.Data;
using Kts.Gis.Substrates;
using Kts.Gis.ViewModels;
using Kts.Messaging;
using Kts.Settings;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.IO;
#if RELEASE
using System.Deployment.Application;
#endif
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление авторизации.
    /// </summary>
    internal sealed partial class AuthorizationView : Window
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что было ли отображено представление впервые.
        /// </summary>
        private bool isShowed;

        /// <summary>
        /// Значение, указывающее на то, что была ли выполнена авторизация.
        /// </summary>
        private bool isLogged;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        /// <summary>
        /// Сервис подложек.
        /// </summary>
        private readonly SubstrateService substrateService;

        #endregion
        
        #region Открытые события

        /// <summary>
        /// Событие завершения авторизации.
        /// </summary>
        public event EventHandler<AuthorizationCompletedEventArgs> AuthorizationCompleted;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationView"/>.
        /// </summary>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        /// <param name="substrateService">Сервис подложек.</param>
        public AuthorizationView(IMessageService messageService, ISettingService settingService, SubstrateService substrateService)
        {
            this.InitializeComponent();

            this.messageService = messageService;
            this.settingService = settingService;
            this.substrateService = substrateService;
        }

        

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ComboBox.DropDownClosed"/> поля выбора сервера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            (this.DataContext as AuthorizationViewModel).AddPlaceholder();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ComboBox.DropDownOpened"/> поля выбора сервера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            (this.DataContext as AuthorizationViewModel).RemovePlaceholder();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PasswordBox.PasswordChanged"/> поля ввода пароля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        bool uriFlag = false;
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
            /*
            var tmpp = Application.Current.Resources;

            

            var uri2 = new Uri("Views/Resources/DarkTheme/classicTheme.xaml", UriKind.Relative);

            var uri = new Uri("Views/Resources/DarkTheme/Core.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uriFlag?uri2:uri) as ResourceDictionary;
            uriFlag = !uriFlag;
            
            tmpp.MergedDictionaries[tmpp.MergedDictionaries.Count - 1] = resourceDict;
            
            */

            (this.DataContext as AuthorizationViewModel).Password = (sender as PasswordBox).Password;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="AuthorizationViewModel.LongTimeTaskRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_LongTimeTaskRequested(object sender, LongTimeTaskRequestedEventArgs e)
        {
            var view = new WaitView(e.WaitViewModel, this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="AuthorizationViewModel.PasswordClearRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_PasswordClearRequested(object sender, EventArgs e)
        {
            this.passwordBox.Clear();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="AuthorizationViewModel.PasswordVerified"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_PasswordVerified(object sender, EventArgs e)
        {
            var viewModel = this.DataContext as AuthorizationViewModel;

#if RELEASE
            // Проверяем совместимость приложения с источником данных.
            if (!viewModel.DataService.IsCompatible(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()))
            {
                this.messageService.ShowMessage("Текущая версия приложения несовместима с источником данных", "Авторизация", MessageType.Error);

                return;
            }
#endif

            // Получаем IP-адрес.
            var ip = "";
            foreach (var entry in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (entry.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = entry.ToString();

                    break;
                }

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


            BaseSqlDataAccessService sqlAccessService = viewModel.DataService.ParameterAccessService as BaseSqlDataAccessService;



            

            /*
                if (sqlAccessService.deserializationSqlQuery())
                {
                    MessageBox.Show("Deserialization query test");
                }
                */


            if (SerializedSqlQuery.deserializationLocalData(viewModel.DataService.LoggedUserId))
            {
                //MessageBox.Show("deserializationLocalData TRUE");

                SerializedSqlQuery serializedObject = SerializedSqlQuery.getSerializedObject();
                
                

                if (serializedObject != null && viewModel.DataService != null)
                {
                    int countCallProcedure = serializedObject.getCountProcedure();
                    viewModel.DataService.BeginSaveTransaction();
                    Guid lastGuidNewObject = Guid.Empty;
                    Guid lastLabelGuidNewObject = Guid.Empty;
                    for (int i = 0; i < countCallProcedure; i++)
                    {
                        string procedureName = serializedObject.getProcedureName(i);
                        




                        switch (procedureName)
                        {
                            case "UpdateNewObjectParamValues":
                                lastGuidNewObject = viewModel.DataService.ParameterAccessService.UpdateNewObjectParamValuesFromLocal(i);
                                break;
                            case "UpdateObjectParamValues":
                                viewModel.DataService.ParameterAccessService.UpdateObjectParamValues(lastGuidNewObject, i);
                                //viewModel.DataService.ParameterAccessService.UpdateObjectParamValues
                                break;
                            case "update_figure":
                                viewModel.DataService.FigureAccessService.UpdateObjectFromLocal(lastGuidNewObject, i);
                                break;
                            case "update_label":
                                viewModel.DataService.LabelAccessService.UpdateObjectFromLocal(lastGuidNewObject, i);
                                //viewModel.DataService.LabelAccessService.UpdateObject
                                break;
                            case "update_node":
                                viewModel.DataService.NodeAccessService.UpdateObjectFromLocal(lastGuidNewObject, i);
                                //viewModel.DataService.NodeAccessService.UpdateObject
                                break;
                            case "update_line":
                                viewModel.DataService.LineAccessService.UpdateObjectFromLocal(lastGuidNewObject, i);
                                break;
                            case "UpdateNewObjectParamValuesLabel":

                                //viewModel.DataService.LabelAccessService.UpdateNewObjectParamValuesFromLocal(i);
                                viewModel.DataService.LabelAccessService.UpdateNewObjectParamValuesFromLocal(i);
                                break;


                        }

                    }
                    /*
                    bool result = viewModel.DataService.EndSaveTransaction();
                    if (result)
                    {
                        MessageBox.Show("result = true");
                    }
                    else
                    {
                        MessageBox.Show("result = false;");
                    }
                    */
                    serializedObject.clearSerializedSqlQuery();
                }




                {
                    if (viewModel.DataService != null)
                    {

                    }

                    //DataService.ParameterAccessService.UpdateNewObjectParamValues(this.nonVisualObject, this.ChangedParameterValues, this.layerHolder.CurrentSchema)

                }
            }
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
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (!this.isLogged)
                Application.Current.Shutdown();
        }

 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //var serverData = LoadServerData();
            // *ssk11
            //var dataService = new WcfDataService("", "", "");
            //serverData = dataService.GetServerList();


            // Инициализируем модель представления и подписываемся на событие успешного ввода пароля.
            if ( viewModel == null)
                viewModel = new AuthorizationViewModel(null,messageService, settingService);

            viewModel.Text = "";//"\\\\172.16.4.58\\Gis\\ServerData";

            var serverData = viewModel.LoadServerData();
            viewModel.ServerData = serverData;
            this.DataContext = viewModel;
            viewModel.LongTimeTaskRequested += this.ViewModel_LongTimeTaskRequested;
            viewModel.PasswordClearRequested += this.ViewModel_PasswordClearRequested;
            viewModel.PasswordVerified += this.ViewModel_PasswordVerified;
            viewModel.LoadServers();
        }
        AuthorizationViewModel viewModel;
        //Dictionary<SqlConnectionString, string> serverData;

        #endregion
    }

    // Реализация Window.
    internal sealed partial class AuthorizationView
    {
        #region Защищенные переопределенные методы
        public static string installFolder;
        /// <summary>
        /// Выполняет действия, связанные с активацией представления.
        /// </summary>
        /// <param name="e">Аргументы.</param>
        protected override void OnActivated(EventArgs e)
        {
            installFolder = Directory.GetCurrentDirectory();
            base.OnActivated(e);

            if (!this.isShowed)
            {
                this.isShowed = true;

                if (UpdateInfoView.LatestVersion > Convert.ToInt64(settingService.Settings["UpdateInfoVersion"]))
                {
                    var view = new UpdateInfoView()
                    {
                        Owner = this
                    };

                    view.ShowDialog();

                    settingService.Settings["UpdateInfoVersion"] = UpdateInfoView.LatestVersion;
                }
            }
        }

        #endregion
    }
}