using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kts.MobileApplication.Services;
using Kts.MobileApplication.Views;


namespace Kts.MobileApplication
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            

            //WcfDataService m_dataService = new WcfDataService("", "", "");
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
