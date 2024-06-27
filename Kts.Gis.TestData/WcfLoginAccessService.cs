using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.ServiceModel;
using System.Reflection;
using System.IO;

namespace Kts.Gis.Data
{
    public class WcfLoginAccessService : BaseWcfDataAccessService, ILoginAccessService
    {
        public WcfLoginAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {
        }


        
        public bool ChangePassword(int loginId, string oldPassword, string newPassword)
        {
            /*
            var wcfClient = m_chanelFactory.CreateChannel();
            bool result = wcfClient.LoginAccessService_ChangePassword(loginId, oldPassword, newPassword);
            ((IClientChannel)wcfClient).Close();
            return result;*/
            //return callDispathServerMethod(this.GetType(), "ChangePassword",oldPassword, newPassword) as bool;

            

            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, loginId,oldPassword, newPassword);
        }

        public List<LoginModel> GetAll()
        {
            
            //return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().ToString()) as List<LoginModel>;

            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name) as List<LoginModel>;
            /*
            var wcfClient = m_chanelFactory.CreateChannel();
            List<LoginModel> test = wcfClient.GetLogins();
            List<LoginModel> result = wcfClient.LoginAccessService_GetAll();
            ((IClientChannel)wcfClient).Close();
            return result;*/
        }


        
        public string getMethodCallerName()
        {
            return "";
            /*
            string result = "";
            var wcfClient = m_chanelFactory.CreateChannel();
            wcfClient.GetLogins();
            ((IClientChannel)wcfClient).Close();
            return result;*/
        }

        public List<AccessModel> GetRestrictions(LoginModel login)
        {

            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, login) as List<AccessModel>;
            /*
            var wcfClient = m_chanelFactory.CreateChannel();
            List<AccessModel> result = wcfClient.LoginAccessService_GetRestrictions(login);
            ((IClientChannel)wcfClient).Close();
            return result;*/
        }

        public string GetRoleName(LoginModel login)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, login) as string;
            /*
            var wcfClient = m_chanelFactory.CreateChannel();
            string result = wcfClient.LoginAccessService_GetRoleName(login);
            ((IClientChannel)wcfClient).Close();
            return result;*/
        }

        public bool IsPasswordCorrect(LoginModel login, string password)
        {
            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, login, password);
            /*
            var wcfClient = m_chanelFactory.CreateChannel();
            bool result = wcfClient.LoginAccessService_IsPasswordCorrect(login, password);
            ((IClientChannel)wcfClient).Close();
            return result;*/
        }

        public bool SetIsUserLogged(int id, string ip, string version, bool isLogged)
        {
            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, ip, version, isLogged);
            /*
            var wcfClient = m_chanelFactory.CreateChannel();
            bool result = wcfClient.LoginAccessService_SetIsUserLogged(id, ip, version, isLogged);
            ((IClientChannel)wcfClient).Close();
            return result;*/
        }
    }
}
