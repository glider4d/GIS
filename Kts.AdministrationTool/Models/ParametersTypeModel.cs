using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.Models
{
    public class ParametersTypeModel : INotifyPropertyChanged
    {
        private bool m_updateFlag = false;
        string m_typeName;
        public string typeName
        {
            get
            {
                return m_typeName;
            }
            set
            {
                m_updateFlag = true;
                m_typeName = value;
                OnPropertyChanged("typeName");
            }
        }

        string m_paramName;

        public string paramName
        {
            get
            {
                return m_paramName;
            }
            set
            {
                m_updateFlag = true;
                m_paramName = value;
                OnPropertyChanged("paramName");
            }
        }

        int m_order;
        public int order
        {
            get
            {
                return m_order;
            }
            set
            {
                m_updateFlag = true;
                m_order = value;
                OnPropertyChanged("order");
            }
        }


        string m_typeValue = String.Empty;
        public string typeValue
        {
            get
            {
                return m_typeValue;
            }
            set
            {
                m_updateFlag = true;
                m_typeValue = value;
                OnPropertyChanged("typeValue");
            }
        }

        private int m_idparam;
        public int id_param
        {
            get
            {
                return m_idparam;
            }
            set
            {
                m_updateFlag = true;
                m_idparam = value;
                OnPropertyChanged("id_param");
            }
        }

        ParameterModel m_parameterModel;

        public ParameterModel paramModel
        {
            get
            {
                return m_parameterModel;
            }
            set
            {
                m_updateFlag = true;
                m_parameterModel = value;
                OnPropertyChanged("paramModel");
            }
        }

        private int m_idtype;
        public int id_type
        {
            get
            {
                return m_idtype;
            }
            set
            {
                m_updateFlag = true;
                m_idtype = value;
            }
        }

        public bool needUpdate()
        {
            return (m_updateFlag);// || paramModel.needUpdate());
        }

        public void setUpdateFlag(bool flag)
        {
            paramModel.setUpdateFlag(flag);
            m_updateFlag = flag;
        }

        public ParametersTypeModel(string typeName, ParameterModel pm, int order, int id_type)
        {
            this.id_type = id_type;
            this.typeName = typeName;
            this.order = order;
            paramModel = pm;
        }

        public ParametersTypeModel(int order, int id_type, int id_param)
        {
            this.order = order;
            this.id_type = id_type;
            this.id_param = id_param;
        }

        public ParametersTypeModel(string typeName, string paramName, string typeValue ,int order, int id_param)
        {
            this.id_param = id_param;
            this.typeName = typeName;
            this.paramName = paramName;
            this.order = order;
            this.typeValue = typeValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
