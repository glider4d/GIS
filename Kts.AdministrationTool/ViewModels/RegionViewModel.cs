using Kts.AdministrationTool.Models;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.ViewModels
{
    public class RegionViewModel: INotifyPropertyChanged
    {

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public RelayCommand saveChange
        {
            get;
        }

        private double m_xSizeWindow = WINDOW_SIZE;
        public double xSizeWindow
        {
            get
            {
                return m_xSizeWindow;
            }
            set
            {
                m_xSizeWindow = value;
                OnPropertyChanged();
            }
        }

        private const double WINDOW_SIZE = 300;

        double m_heightListBox = WINDOW_SIZE-90;
        public double heightListBox
        {
            get
            {
                return m_heightListBox;
            }
            set
            {
                m_heightListBox = value;
                OnPropertyChanged();
            }
        }

        private double m_ySizeWindow = WINDOW_SIZE+50;
        public double ySizeWindow
        {
            get
            {
                return m_ySizeWindow;
            }
            set
            {
                m_ySizeWindow = value;
                heightListBox = value - 90;
                OnPropertyChanged();
            }
        }


        private List<RegionModel> m_regionModelList;


        
        public event PropertyChangedEventHandler PropertyChanged;

        public List<RegionModel> regionModelList
        {
            get
            {
                return m_regionModelList;
            }
            set
            {
                m_regionModelList = value;
            }
        }

        private List<bool> m_cashFlags = new List<bool>();


        public bool saveFlag
        {
            get;
            set;
        }

        public string result
        {
            get;
            set;
        }

        public void saveChanged()
        {
            bool firstCheck = false;
            string resultString = "";
            foreach (var item in regionModelList)
            {
                if (item.check)
                {
                    if (!firstCheck)
                    {
                        resultString = item.id.ToString();
                    }
                    else
                    {
                        resultString += (","+item.id);
                    }
                    firstCheck = true;
                }
            }

            saveFlag = true;
            result = resultString;
            needUpdate = false;
        }

        public RegionViewModel(List<RegionModel> regionModelList, string region)
        {
            saveFlag = false;
            this.regionModelList = regionModelList;

            saveChange = new RelayCommand(saveChanged);



            string[] regionStringArray = new string[0];
            if ( region != null && region.Length > 0)
                regionStringArray = region.Split(',');

            foreach (var item in regionModelList)
            {
                item.check = false;
                item.setUpdateFlag(false);
                m_cashFlags.Add(new bool());
                item.PropertyChanged += Item_PropertyChanged;
            }

            foreach (var item in regionStringArray)
            {
                int pars = 0;
                if (Int32.TryParse(item, out pars))
                {
                    int index = pars - 1;
                    if (index < regionModelList.Count)
                    {
                        regionModelList[index].check = true;
                        m_cashFlags[index] = true;
                        regionModelList[index].setUpdateFlag(false);
                        


                    }

                }
                
            }

            needUpdate = false;
            
        }

        private bool m_needUpdate;
        public bool needUpdate
        {
            get
            {
                return m_needUpdate;
            }
            set
            {
                m_needUpdate = value;
                OnPropertyChanged();
            }
        }


        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool notEquals = false;
            for (int i = 0; i < m_cashFlags.Count; i++)
            {
                if (m_cashFlags[i] != regionModelList[i].check)
                {
                    notEquals = true;
                    break;
                }
            }

            needUpdate = notEquals;
        }
    }
}
