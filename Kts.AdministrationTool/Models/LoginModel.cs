using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kts.AdministrationTool.Models
{
    public sealed class LoginModel : INotifyPropertyChanged
    {
        #region Конструкторы
        public Dictionary<int,string> accessLevelDictionary
        {
            get;
            set;
        }

        


        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LoginModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор логина.</param>
        /// <param name="name">Название логина.</param>
        public LoginModel(int id, string name)
        {
            this.Id = id;
            m_name = name;
        }

        private bool m_newFlag = false;
        private bool m_updateFlag = false;

        public bool needUpdate()
        {
            return m_updateFlag;
        }

        public bool needInsert()
        {
            return m_newFlag;
        }

        public void insertLogin()
        {
            m_newFlag = false;
        }

        public void updateingLogin()
        {
            m_updateFlag = false;
        }

        public LoginModel(int id, string name, string fName, string sName, string tName, string phoneNumber, int accessLevel, bool activity, string region)
        {
            this.Id = id;

            m_name = name;
            m_fName = fName;
            m_sName = sName;
            m_tName = tName;
            m_phoneNumber = phoneNumber;
            //m_accessLevel = accessLevel;
            this.m_accessLevel = accessLevel;
            m_activity = activity;
            m_region = region;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор логина.
        /// </summary>
        /// 

        
        public int Id
        {
            set;
            get;
        }


        string m_fName;
        public string fName
        {
            get
            {
                return m_fName;
            }
            set
            {
                if (m_fName == null || !m_fName.Equals(value))
                {
                    m_fName = value;
                    m_updateFlag = true;
                    OnPropertyChanged("fName");
                }
            }
        }

        private string m_sName;
        public string sName
        {
            get
            {
                return m_sName;
            }
            set
            {
                if (m_sName == null || !m_sName.Equals(value))
                {
                    m_sName = value;
                    m_updateFlag = true;
                    OnPropertyChanged("sName");
                }
            }
        }


        private string m_tName;
        public string tName
        {
            get
            {
                return m_tName;
            }
            set
            {
                if (m_tName == null || !m_tName.Equals(value))
                {
                    m_tName = value;
                    m_updateFlag = true;
                    OnPropertyChanged("tName");
                }
            }
        }
        private string m_phoneNumber;
        public string phoneNumber
        {
            get
            {
                return m_phoneNumber;
            }
            set
            {
                if (m_phoneNumber == null || !m_phoneNumber.Equals(value))
                {
                    m_phoneNumber = value;
                    m_updateFlag = true;
                    OnPropertyChanged("phoneNumber");
                }
            }
        }

        private int m_accessLevel;
        public int accessLevel
        {
            get
            {
                return m_accessLevel;
            }
            set
            {
                if (m_accessLevel != value)
                {
                    m_accessLevel = value;
                    m_updateFlag = true;
                    OnPropertyChanged("accessLevel");
                }
            }
        }
        private bool m_activity;
        public bool activity
        {
            get
            {
                return m_activity;
            }
            set
            {
                if (m_activity != value)
                {
                    //System.Windows.MessageBox.Show("updateActivity flag");
                    m_activity = value;
                    m_updateFlag = true;
                    OnPropertyChanged("activity");
                }
            }
        }
        private string m_region;
        public string region
        {
            get
            {
                return m_region;
            }
            set
            {
                if (m_region == null || !m_region.Equals(value))
                {
                    m_region = value;
                    OnPropertyChanged("region");
                }
            }
        }

        private string m_name;
        /// <summary>
        /// Возвращает название логина.
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (m_name == null || !m_name.Equals(value))
                {
                    m_name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string m_password;
        public string Password
        {
            get
            {
                return m_password;
            }
            set
            {
                m_password = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            
            if (obj != null && obj is LoginModel)
            {
                LoginModel o = obj as LoginModel;
                
                result = (this.Id == o.Id && this.Name == o.Name && this.fName == o.fName &&
                    this.sName == o.sName && this.tName == o.tName && this.phoneNumber == o.phoneNumber &&
                    this.accessLevel == o.accessLevel && this.activity == o.activity && this.region == o.region);
            }

            return result;
        }

        #endregion
    }
}
