using Kts.Gis.Data;
using Kts.Gis.ViewModels;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Kts.AdministrationTool.Views;
using Kts.Messaging;
using System.IO;
using Kts.Settings;

namespace Kts.AdministrationTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application

    {
        private WpfMessageService messageService;
        private ISettingService settingService;

        public static string installFolder = Directory.GetCurrentDirectory();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MessageBox.Show(Directory.GetCurrentDirectory());
            //var folderName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis";
            var folderName = installFolder;



            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            MessageBox.Show(installFolder);

            // Сохраняем путь к экзешнику приложения.
            //File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\InstallationPath", System.Reflection.Assembly.GetExecutingAssembly().Location);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\InstallationPath", System.Reflection.Assembly.GetExecutingAssembly().Location);

            this.settingService = new BinarySettingService(folderName, "Settings.bin");
            
            
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
            this.settingService.RegisterSetting(new Setting("MapPositions", typeof(Dictionary<int, Tuple<double, System.Windows.Size>>), new Dictionary<int, Tuple<double, System.Windows.Size>>()));
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
            this.settingService.RegisterSetting(new Setting("theme", typeof(int), -1));
            
            // Сбрасываем настройки.
            this.settingService.Reset();

            if (!this.settingService.Load())
            {
                this.settingService.Reset();

                this.messageService.ShowMessage("Не удалось загрузить настройки", "Загрузка настроек", MessageType.Error);
            }

            this.messageService = new WpfMessageService(this);
            var authorizationView = new AutorizationAdminToolView(this.messageService,this.settingService);
            authorizationView.Show();
            //var authorizationView = new AutorizationAdminToolView(this.messageService, this.settingService, this.substrateService);

            //authorizationView.AuthorizationCompleted += this.AuthorizationView_AuthorizationCompleted;

            //authorizationView.Show();
        }

        void test()
        {
        }
        
    }
}
