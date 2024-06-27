using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.Models
{
    public class RegionModel : INotifyPropertyChanged
    {
        private int m_id;

        private bool m_updateFlag;

        public bool needUpdate()
        {
            return m_updateFlag;
        }

        public void setUpdateFlag(bool flag)
        {
            m_updateFlag = flag;
        }

        private bool m_check;
        public bool check
        {
            get
            {
                return m_check;
            }
            set
            {
                m_check = value;
                m_updateFlag = true;
                OnPropertyChanged();
            }
        }

        public int id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
                OnPropertyChanged();
            }
        }

        private string m_regionName;
        public string regionName
        {
            get
            {
                return m_regionName;
            }
            set
            {
                m_regionName = value;
                OnPropertyChanged();
            }
        }

        public RegionModel(int id, string regionName)
        {
            this.id = id;
            this.regionName = regionName;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
