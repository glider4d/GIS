using Kts.AdministrationTool.Models;
using Kts.AdministrationTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kts.AdministrationTool.Views
{
    /// <summary>
    /// Interaction logic for RegionsDialog.xaml
    /// </summary>
    public partial class RegionsDialog : Window
    {
        RegionViewModel m_regionViewModel;

        public bool needUpdate
        {
            get;
            set;
        }

        public string resultString
        {
            get;
            set;
        }

        public RegionsDialog(List<RegionModel> regionDic, string region)
        {
            m_regionViewModel = new RegionViewModel(regionDic,region);
            this.DataContext = m_regionViewModel;
            InitializeComponent();

            Closed += RegionsDialog_Closed;
        }

        private void RegionsDialog_Closed(object sender, EventArgs e)
        {
            if (m_regionViewModel.saveFlag)
            {
                needUpdate = true;
                resultString = m_regionViewModel.result;
            }
                
        }

        public void sizeChange(object sender, SizeChangedEventArgs e)
        {
            m_regionViewModel.xSizeWindow = e.NewSize.Width;
            m_regionViewModel.ySizeWindow = e.NewSize.Height;
        }
    }
}