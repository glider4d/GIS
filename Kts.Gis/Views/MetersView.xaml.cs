using Kts.Gis.Data;
using Kts.Gis.ViewModels;
using Kts.Messaging;
using Kts.Settings;
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

namespace Kts.Gis.Views
{
    /// <summary>
    /// Логика взаимодействия для MetersView.xaml
    /// </summary>
    public partial class MetersView : Window
    {

        public MetersViewModel meterViewModel
        {
            get;set;
        }

        public MetersView(IDataService dataService, IMessageService messageService, ISettingService settingService)
        {
            InitializeComponent();

            meterViewModel = new MetersViewModel(dataService, messageService, settingService);
            DataContext = meterViewModel;

        }
    }
}
