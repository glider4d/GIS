using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using testXamarin.Services;
using testXamarin.Views;
using System.ServiceModel;
//using Kts.Utilities;
//using Kts.Gis.Data;

namespace testXamarin
{
    public partial class App : Application
    {
        Kts.Gis.Data.WcfDataService m_dataService;
        public App()
        {

            /*
            var content = new ContentPage
            {
                Title = "FirstApp",
                Content = new StackLayout
                {   
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin forms !"
                        }
                    }
                }
            };

            */
            var content = new AutorizationView();
            MainPage = new NavigationPage(content);
            /*
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new AutorizationView();
            */


            //MainPage = new MainPage();




            //testProgramService();


            //m_dataService = new Kts.Gis.Data.WcfDataService("", "", "");
            //testServerData();


            //m_dataService.Initialize("Якутск");
            /*
            Kts.Gis.Data.TestImportClass test = new Kts.Gis.Data.TestImportClass();*/
            //MainPage = new MainPage();

            //MainPage = new testXamarin.Views.AutorizationView;

        }

        public void testServerData()
        {
            
            var getFigures = m_dataService.FigureAccessService.GetAll(540, new Kts.Gis.Models.SchemaModel(2019, "2019", true, false));
        }

        public void testProgramService()
        {

            try
            {
                BasicHttpBinding myBinding = new BasicHttpBinding();
                Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);
                myBinding.MaxReceivedMessageSize = Int32.MaxValue;
                myBinding.MaxBufferSize = Int32.MaxValue;
                myBinding.MaxBufferPoolSize = Int32.MaxValue;



                myBinding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
                myBinding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                myBinding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
                myBinding.ReaderQuotas.MaxDepth = Int32.MaxValue;
                myBinding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;

                Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);

                EndpointAddress myEndpoint = new EndpointAddress("http://172.16.3.194:8080/Kts.Gis.Data.WcfDispatchServer");
                ChannelFactory<Kts.Gis.Data.IWcfDispatchServer> m_chanelFactory;
                m_chanelFactory = new ChannelFactory<Kts.Gis.Data.IWcfDispatchServer>(myBinding, myEndpoint);


                Kts.Gis.Data.IWcfDispatchServer wcfClient1 = m_chanelFactory.CreateChannel();
                int result = wcfClient1.DoWork();
                wcfClient1.Initialize("Якутск");
                

            } catch(Exception exp)
            {
                string message = exp.Message;
            }


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
