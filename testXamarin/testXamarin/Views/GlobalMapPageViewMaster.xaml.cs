using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace testXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlobalMapPageViewMaster : ContentPage
    {
        public ListView ListView;

        public GlobalMapPageViewMaster()
        {
            InitializeComponent();

            BindingContext = new GlobalMapPageViewMasterViewModel();
            ListView = MenuItemsListView;
        }

        class GlobalMapPageViewMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<GlobalMapPageViewMasterMenuItem> MenuItems { get; set; }

            public GlobalMapPageViewMasterViewModel()
            {
                MenuItems = new ObservableCollection<GlobalMapPageViewMasterMenuItem>(new[]
                {
                    new GlobalMapPageViewMasterMenuItem { Id = 0, Title = "Страница 1" },
                    new GlobalMapPageViewMasterMenuItem { Id = 1, Title = "Страница 2" },
                    new GlobalMapPageViewMasterMenuItem { Id = 2, Title = "Страница 3" },
                    new GlobalMapPageViewMasterMenuItem { Id = 3, Title = "Страница 4" },
                    new GlobalMapPageViewMasterMenuItem { Id = 4, Title = "Страница 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}