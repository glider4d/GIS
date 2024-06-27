using Kts.Gis.Data;
using Kts.Gis.Models.Enums;
using Kts.Gis.Substrates;
using Kts.Gis.ViewModels;
using Kts.Gis.Views;
using Kts.Messaging;
using Kts.Settings;
#if DEBUG
using Kts.WpfUtilities;
#endif
using System;
using System.Collections.Generic;
#if RELEASE
using System.Deployment.Application;
#endif
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Kts.Gis
{
    /// <summary>
    /// Представляет основной класс приложения.
    /// </summary>
    internal sealed partial class App : Application, IDisposable
    {

        public static string installFolder = Directory.GetCurrentDirectory();

        #region Закрытые поля

        /// <summary>
        /// Сервис доступа к данным.
        /// </summary>
        private IDataService dataService;

        /// <summary>
        /// Идентификатор авторизованного пользователя.
        /// </summary>
        private int id;

        /// <summary>
        /// IP-адрес авторизованного пользователя.
        /// </summary>
        private string ip;

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Значение, указывающее на то, что открыто ли приложение для выбора объекта.
        /// </summary>
        private bool isObjectSelecting;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private IMessageService messageService;

        /// <summary>
        /// Мьютекс для проверки наличия уже запущенной версии приложения.
        /// </summary>
        private Mutex mutex = null;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private ISettingService settingService;

        /// <summary>
        /// Сервис подложек.
        /// </summary>
        private SubstrateService substrateService;

        #endregion

#if DEBUG
        #region Закрытые статические поля

        /// <summary>
        /// Представление отладки.
        /// </summary>
        private static DebugView debugView;

        #endregion
#endif

        #region Деструкторы

        /// <summary>
        /// Финализирует экземпляр класса <see cref="App"/>.
        /// </summary>
        ~App()
        {
            this.Dispose(false);
        }

        #endregion

#if DEBUG
        #region Открытые статические свойства

        /// <summary>
        /// Модель представления отладки.
        /// </summary>
        public static DebugViewModel DebugViewModel = new DebugViewModel(100);

        #endregion
#endif

        #region Обработчики событий
        
        /// <summary>
        /// Обрабатывает событие <see cref="Application.DispatcherUnhandledException"/> основного класса приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var dateTime = DateTime.Now.ToString().Replace(":", ".");

                // Создаем отчет об ошибке и отправляем его в хранилище ошибок.
                //
                //var fileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\Отчет об ошибке.txt";
                var fileName = installFolder + "\\Отчет об ошибке.txt";
                using (var writer = File.CreateText(fileName))
                    writer.Write(e.Exception.ToString());
                File.Copy(fileName, this.dataService.ErrorFolderName + dateTime + " - " + this.id + ".txt");

                // Делаем снимок активного окна и тоже отправляем его в хранилище ошибок.
                var window = Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                //var imageName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\Снимок окна.png";
                var imageName = installFolder + "\\Снимок окна.png";
                this.CreateBitmapFromVisual(window, imageName);
                File.Copy(imageName, this.dataService.ErrorFolderName + dateTime + " - " + this.id + ".png");

                if (this.messageService.ShowYesNoMessage("Непредвиденная ошибка. Дальнейшая работа приложения невозможна. Файл с отчетом об ошибке отправлен на Ваш локальный сервер. Не хотите ли посмотреть его?", "Инженерно-картографическая система"))
                    Process.Start(fileName);
            }
            catch
            {
                this.messageService.ShowMessage("Непредвиденная ошибка. Дальнейшая работа приложения невозможна. Не удалось создать файл с отчетом об ошибке.", "Инженерно-картографическая система", MessageType.Error);
            }

            if (this.isObjectSelecting)
                Console.WriteLine("");
            else
            {
                // Если идет не выбор объекта, то пробуем сохранить несохраненные изменения.
                var mainView = this.MainWindow as MainView;
                if (mainView != null)
                    mainView.LastBreath();
            }

#if RELEASE
            if (this.dataService != null)
                this.dataService.LoginAccessService.SetIsUserLogged(this.id, this.ip, ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(), false);
#endif
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Application.Exit"/> основного класса приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
#if RELEASE
            if (this.dataService != null)
                this.dataService.LoginAccessService.SetIsUserLogged(this.id, this.ip, ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(), false);
#endif

            if (this.settingService != null && !this.settingService.Save())
                this.messageService.ShowMessage("Не удалось сохранить настройки", "Сохранение настроек", MessageType.Error);

            if (this.substrateService != null && !this.substrateService.Save())
                this.messageService.ShowMessage("Не удалось сохранить данные подложек", "Сохранение данных подложек", MessageType.Error);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Application.Startup"/> основного класса приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Задаем язык по умолчанию.
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            
            //var test = new DataWcfRef.WcfFigureAccessServiceClient("NetTcpBinding_IWcfFigureAccessService");
            /*
            WcfDataService test = new WcfDataService();
            MessageBox.Show(test.doWork().ToString());
            test.test();
            test.test3();
            test.test2();
            */

            this.messageService = new WpfMessageService(this);

            // Проверяем наличие уже запущенной версии приложения.
            bool createdNew;
            this.mutex = new Mutex(true, "Kts.Gis", out createdNew);
            if (!createdNew && e.Args.Length == 0)
            {
#if RELEASE
                this.messageService.ShowMessage("Нельзя запускать более одной копии приложения", "Инженерно-картографическая система", MessageType.Error);
#endif

                // Закрываем приложение.
                this.Shutdown();
            }
            else
            {
                var folderName2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis";
                var folderName = installFolder + "\\Gis";

                if (!Directory.Exists(folderName))
                    Directory.CreateDirectory(folderName);

                // Сохраняем путь к экзешнику приложения.
                //File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\InstallationPath", System.Reflection.Assembly.GetExecutingAssembly().Location);
                File.WriteAllText(folderName + "\\InstallationPath", System.Reflection.Assembly.GetExecutingAssembly().Location);

                this.settingService = new BinarySettingService(folderName, "Settings.bin");
                this.substrateService = new SubstrateService(folderName, "Substrates.bin");

                // Регистрируем настройки.
                this.settingService.RegisterSetting(new Setting("LastUsedLoginId", typeof(int), -1));
                this.settingService.RegisterSetting(new Setting("LastUsedRegionId", typeof(int), -1));
                this.settingService.RegisterSetting(new Setting("LastUsedCityId", typeof(int), -1));
                this.settingService.RegisterSetting(new Setting("LeftSideColumnWidth", typeof(double), 250));
                this.settingService.RegisterSetting(new Setting("TopSideRowHeight", typeof(double), 300));
                this.settingService.RegisterSetting(new Setting("LastUsedServer", typeof(string), ""));
                this.settingService.RegisterSetting(new Setting("IsSubstrateVisible", typeof(bool), false));
                this.settingService.RegisterSetting(new Setting("IsLegendVisible", typeof(bool), true));
                this.settingService.RegisterSetting(new Setting("SubstrateOpacity", typeof(double), 1));
                this.settingService.RegisterSetting(new Setting("MapPositions", typeof(Dictionary<int, Tuple<double, Size>>), new Dictionary<int, Tuple<double, Size>>()));
                this.settingService.RegisterSetting(new Setting("UpdateInfoVersion", typeof(long), 1));
                this.settingService.RegisterSetting(new Setting("BadgeScale", typeof(double), 1));
                this.settingService.RegisterSetting(new Setting("AutoHideLabels", typeof(bool), false));
                this.settingService.RegisterSetting(new Setting("AutoHideNodes", typeof(bool), false));
                this.settingService.RegisterSetting(new Setting("LastSchemas", typeof(Dictionary<int, int>), new Dictionary<int, int>()));
                this.settingService.RegisterSetting(new Setting("HideEmptyParameters", typeof(bool), true));
                this.settingService.RegisterSetting(new Setting("HeaderColumnWidth", typeof(double), 150));
                this.settingService.RegisterSetting(new Setting("ValueColumnWidth", typeof(double), 180));
                this.settingService.RegisterSetting(new Setting("IsBoilerPopupVisible", typeof(bool), true));
                this.settingService.RegisterSetting(new Setting("IsStoragePopupVisible", typeof(bool), true));
                this.settingService.RegisterSetting(new Setting("NewLineColor", typeof(string), "#808080"));
                this.settingService.RegisterSetting(new Setting("OldLineColor", typeof(string), "#404040"));
                this.settingService.RegisterSetting(new Setting("RPColor", typeof(string), "#FF0000"));
                this.settingService.RegisterSetting(new Setting("UOColor", typeof(string), "#9800FF"));
                this.settingService.RegisterSetting(new Setting("theme", typeof(int), (int)Themes.Classic));

                // Сбрасываем настройки.
                this.settingService.Reset();

                if (!this.settingService.Load())
                {
                    this.settingService.Reset();

                    this.messageService.ShowMessage("Не удалось загрузить настройки", "Загрузка настроек", MessageType.Error);
                }

                if (!this.substrateService.Load())
                {
                    this.substrateService.Reset();

                    this.messageService.ShowMessage("Не удалось загрузить данные подложек", "Загрузка данных подложек", MessageType.Error);
                }

                if (e.Args.Length > 2 && e.Args[0] == "-osm")
                {
                    this.isObjectSelecting = true;

                    int cityId;
                    string connectionString;
                    string fileStorage;

                    cityId = Convert.ToInt32(e.Args[1]);
                    connectionString = e.Args[2];
                    fileStorage = e.Args[3];

                    var dataService = new SqlDataService(new SqlReconnector(Utilities.SqlConnectionString.FromString(connectionString)), @"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");
                    //ssk11
                    //var dataService = new WcfDataService(@"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");


                    await dataService.LoadDataAsync();
                    

                    var objectSelectView = new ObjectSelectView(cityId, dataService, this.messageService, this.settingService, this.substrateService);

                    objectSelectView.Show();
                }
                else
                {
                    var authorizationView = new AuthorizationView(this.messageService, this.settingService, this.substrateService);

                    authorizationView.AuthorizationCompleted += this.AuthorizationView_AuthorizationCompleted;

                    authorizationView.Show();
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="AuthorizationView.AuthorizationCompleted"/> представления авторизации.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void AuthorizationView_AuthorizationCompleted(object sender, AuthorizationCompletedEventArgs e)
        {
            this.dataService = e.DataService;
            this.id = e.LoginId;
            this.ip = e.Ip;
        }

        #endregion

#if DEBUG
        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> окна-владельца представления отладки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private static void CurWindow_Closed(object sender, EventArgs e)
        {
            ((Window)sender).Closed += CurWindow_Closed;

            CloseDebugView();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> представления отладки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private static void DebugView_Closed(object sender, EventArgs e)
        {
            debugView.Closed -= DebugView_Closed;

            debugView = null;
        }

        #endregion
#endif

        #region Закрытые методы

        /// <summary>
        /// Создает файл-изображение в формате PNG из указанного визуального элемента и сохраняет его под заданным названием.
        /// </summary>
        /// <param name="target">Визуальный элемент, который необходимо сохранить в файл-изображение.</param>
        /// <param name="fileName">Название сохраняемого файла, включающее полный путь к нему.</param>
        private void CreateBitmapFromVisual(Visual target, string fileName)
        {
            if (target == null || string.IsNullOrEmpty(fileName))
                return;

            var bounds = VisualTreeHelper.GetDescendantBounds(target);

            var renderTarget = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            var visual = new DrawingVisual();

            using (var context = visual.RenderOpen())
            {
                var visualBrush = new VisualBrush(target);

                context.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
            }

            renderTarget.Render(visual);

            // Сохраняем в файл.
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));
            using (var stream = File.Create(fileName))
                encoder.Save(stream);
        }

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    this.mutex.Dispose();

                this.isDisposed = true;
            }
        }

        #endregion

#if DEBUG
        #region Открытые статические методы

        /// <summary>
        /// Закрывает представление отладки.
        /// </summary>
        public static void CloseDebugView()
        {
            if (debugView == null)
                return;

            debugView.Close();
        }

        /// <summary>
        /// Отображает представление отладки.
        /// </summary>
        public static void ShowDebugView()
        {
            if (debugView != null)
                return;

            var curWindow = Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            curWindow.Closed += CurWindow_Closed;

            debugView = new DebugView(DebugViewModel)
            {
                Icon = curWindow.Icon,
                Owner = curWindow.Owner
            };

            debugView.Closed += DebugView_Closed;

            debugView.Show();
        }

        #endregion
#endif
    }

    // Реализация IDisposable.
    internal sealed partial class App
    {
        #region Открытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}