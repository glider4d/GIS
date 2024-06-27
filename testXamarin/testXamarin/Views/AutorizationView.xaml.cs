using Kts.Gis.Data;
using Kts.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace testXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutorizationView : ContentPage
    {

        public ObservableCollection<string> serverList
        {
            get;
            set;
        } = new ObservableCollection<string>();

        public IList<string> CountryList
        {
            get
            {
                return new List<string> { "USA1", "Germany2", "England3" };
            }
        }
        string m_login;
        public string login
        {
            get
            {
                return m_login;
            }
            set
            {
                m_login = value;
            }
        }

        public AutorizationView()
        {
            InitializeComponent();
            //var viewModel = new AuthorizationViewModel(serverData, messageService, settingService);

            WcfDataService dataService = new WcfDataService("", "", "");

            //var serverList = wcfClient1.getServerList();
            //var serverList = dataService.GetServerList();
            //dataService.MobileInitialize("Якутск");
            //App.Current.MainPage.DisplayAlert("test", "test", "test");
            //App.Current.MainPage.DisplayAlert("test", "test", "test");
            
            
            Kts.Gis.ViewModels.AuthorizationViewModel viewModel = new Kts.Gis.ViewModels.AuthorizationViewModel(dataService, new XamarinMessageService(), this);
            BindingContext = viewModel;//this;
        }

        public async void clickButton()
        {
            await Navigation.PushAsync(new GlobalMapPageView());

            //await Navigation.PushAsync(new GlobalMapPageViewMaster());
        }


    }
}