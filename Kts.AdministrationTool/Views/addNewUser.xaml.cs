using Kts.AdministrationTool.Data;
using Kts.AdministrationTool.ViewModels;
using Kts.AdministrationTool.ViewModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for addNewUser.xaml
    /// </summary>
    /// 
    public partial class addNewUser : Window
    {

        UserAddEventArgs addEventArgs;
        UserChangeEventArgs changeEventArgs;

        public EventHandler<UserChangeEventArgs> changeEvent;
        public EventHandler<UserAddEventArgs> addEvent;

        public EventHandler<EventArgs> closeWindow;
        

        private bool m_addNewUserFlag;//иначе user edit
        public Models.LoginModel loginModel{
            get;
            set;
        }

        private userchangeViewModel m_userEditModel;


        private SqlAdminAccessService m_accessService;

        public addNewUser(bool addNewUser, Models.LoginModel userModel, SqlAdminAccessService accessService)
        {
            
            m_accessService = accessService;
            
            m_addNewUserFlag = addNewUser;
            m_userEditModel = new userchangeViewModel(userModel, addNewUser);
            
            this.loginModel = userModel;

            this.closeWindow += clsWindow;

            InitializeComponent();
            //DataContext = m_loginTab;
            //this.DataContext = loginModel;

            this.DataContext = m_userEditModel;
        }

        public void clsWindow(object sender, EventArgs evArgs)
        {
            this.Close();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as userchangeViewModel).password = (sender as PasswordBox).Password;
        }

        private void passwordBox_PasswordConfirmChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as userchangeViewModel).confirmPassword = (sender as PasswordBox).Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string name = m_userEditModel.login;
            string password = EncryptSHA256(m_userEditModel.password);

            ///!!!!!MessageBox.Show("AddNewUser");

            //!!!m_accessService.insertNewLogin(209, name, password, 4, true, "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30");

            //m_accessService.insertNewLogin(new Models.LoginModel(



            loginModel.Name = name;
            loginModel.Password = password;

            if (m_addNewUserFlag)
            {
                addEvent?.Invoke(this, new UserAddEventArgs(this.loginModel));
            }
            else
            {
                changeEvent?.Invoke(this, new UserChangeEventArgs(this.loginModel));
            }

            this.Close();
        }



        public static string EncryptSHA256(string input)
        {
            var hash = new SHA256Managed();

            var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hashBytes);
        }
    }
}
