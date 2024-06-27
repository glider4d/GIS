using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.Models
{
    public class ParameterModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool m_updateFlag = false;

        private int m_id;
        public int Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
                m_updateFlag = true;
                OnPropertyChanged("Id");
            }
        }

        private string m_name;
        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                m_updateFlag = true;
                OnPropertyChanged("name");
            }
        }
        private string m_format;
        public string format
        {
            get
            {
                return m_format;
            }
            set
            {
                m_format = value;
                m_updateFlag = true;
                OnPropertyChanged("format");
            }
        }
        private string m_vtable;
        public string vtable
        {
            get
            {
                return m_vtable; 
            }
            set
            {
                m_vtable = value;
                m_updateFlag = true;
                OnPropertyChanged("vtable");
            }
        }


        private string m_unit;
        public string unit
        {
            get
            {
                return m_unit;
            }
            set
            {
                m_unit = value;
                m_updateFlag = true;
                OnPropertyChanged("unit");
            }
        }
        private string m_exact_format;
        public string exact_format
        {
            get
            {
                return m_exact_format;
            }
            set
            {
                m_exact_format = value;
                m_updateFlag = true;
                OnPropertyChanged("exact_format");
            }
        }

        public bool needUpdate()
        {
            return m_updateFlag;
        }

        public void setUpdateFlag(bool flag)
        {
            m_updateFlag = flag;
        }

        public ParameterModel(int Id, string name, string format, string vtable, string unit, string exact_format)
        {
            m_id = Id;
            m_name = name;
            m_format = format;
            m_vtable = vtable;
            m_unit = unit;
            m_exact_format = exact_format;
            /*
            this.Id = Id;
            this.name = name;
            this.format = format;
            this.vtable = vtable;
            this.unit = unit;
            this.exact_format = exact_format;*/
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
