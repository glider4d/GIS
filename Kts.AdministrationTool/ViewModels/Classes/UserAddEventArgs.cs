using Kts.AdministrationTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.ViewModels.Classes
{
    public class UserAddEventArgs : EventArgs
    {
        public LoginModel userModel
        {
            get;
            set;
        }
        public UserAddEventArgs(LoginModel newUser)
        {
            this.userModel = newUser;
        }
    }
}
