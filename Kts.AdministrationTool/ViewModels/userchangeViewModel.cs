using Kts.AdministrationTool.Data;
using Kts.AdministrationTool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kts.AdministrationTool.ViewModels
{
    public class userchangeViewModel : INotifyPropertyChanged
    {
        private string m_login;
        public string login
        {
            get
            {
                return m_login;
            }
            set
            {
                m_login = value;
                OnPropertyChanged("login");
            }
        }

        private string m_password;

        public string password
        {
            get
            {
                return m_password;
            }
            set
            {
                m_password = value;
                equalPassword = (m_password.Equals(m_confirmPassword)) && m_password.Length>0;
                OnPropertyChanged("password");
            }
        }

        bool m_equalPassword;

        public bool equalPassword
        {
            get
            {
                return m_equalPassword;
            }
            set
            {
                m_equalPassword = value;
                OnPropertyChanged("equalPassword");
            }
        }

        private string m_confirmPassword;
        public string confirmPassword
        {
            get
            {
                return m_confirmPassword;
            }
            set
            {
                m_confirmPassword = value;
                equalPassword = (m_confirmPassword.Equals(m_password)) && m_password.Length > 0;
                OnPropertyChanged("confirmPassword");
            }
        }
        
        public bool addmode
        {
            get;
            set;
        }


        public LoginModel loginModel
        {
            get;
            set;
        }

        public userchangeViewModel(LoginModel loginModel, bool addmode)
        {
            this.loginModel = loginModel;
            this.addmode = addmode;
            login = loginModel.Name;
            confirmPassword = password = "";
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
