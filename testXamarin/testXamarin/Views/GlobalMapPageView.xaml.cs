using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace testXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlobalMapPageView : MasterDetailPage
    {
        public GlobalMapPageView()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as GlobalMapPageViewMasterMenuItem;
            if (item == null)
                return;



            var page = Activator.CreateInstance(item.TargetType);
            //var page = (Page)Activator.CreateInstance(item.TargetType);

            if (page is Page)
            {
                (page as Page).Title = item.Title;

                Detail = new NavigationPage(page as Page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
        }
    }
}