﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kts.GisClientTest.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IWcfDispatchServer")]
    public interface IWcfDispatchServer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoWork", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoWorkResponse")]
        int DoWork();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoWork", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoWorkResponse")]
        System.Threading.Tasks.Task<int> DoWorkAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoIt", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoItResponse")]
        int DoIt();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoIt", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoItResponse")]
        System.Threading.Tasks.Task<int> DoItAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoArray", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoArrayResponse")]
        int[] DoArray();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoArray", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoArrayResponse")]
        System.Threading.Tasks.Task<int[]> DoArrayAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoDictionary", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoDictionaryResponse")]
        System.Collections.Generic.Dictionary<int, int> DoDictionary();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/DoDictionary", ReplyAction="http://tempuri.org/IWcfDispatchServer/DoDictionaryResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, int>> DoDictionaryAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/getServerList2", ReplyAction="http://tempuri.org/IWcfDispatchServer/getServerList2Response")]
        System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string> getServerList2();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/getServerList2", ReplyAction="http://tempuri.org/IWcfDispatchServer/getServerList2Response")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string>> getServerList2Async();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/getServerList", ReplyAction="http://tempuri.org/IWcfDispatchServer/getServerListResponse")]
        System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string> getServerList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/getServerList", ReplyAction="http://tempuri.org/IWcfDispatchServer/getServerListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string>> getServerListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/getPacket", ReplyAction="http://tempuri.org/IWcfDispatchServer/getPacketResponse")]
        Kts.Gis.Data.Packet getPacket();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/getPacket", ReplyAction="http://tempuri.org/IWcfDispatchServer/getPacketResponse")]
        System.Threading.Tasks.Task<Kts.Gis.Data.Packet> getPacketAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/Initialize", ReplyAction="http://tempuri.org/IWcfDispatchServer/InitializeResponse")]
        bool Initialize(string serverName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/Initialize", ReplyAction="http://tempuri.org/IWcfDispatchServer/InitializeResponse")]
        System.Threading.Tasks.Task<bool> InitializeAsync(string serverName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/GetSqlDataService", ReplyAction="http://tempuri.org/IWcfDispatchServer/GetSqlDataServiceResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(int[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.Dictionary<int, int>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Data.Packet))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Data.packet2))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.LoginModel[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.LoginModel))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.AccessModel[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.AccessModel))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.AccessKind))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Utilities.SqlConnectionString))]
        object GetSqlDataService();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/GetSqlDataService", ReplyAction="http://tempuri.org/IWcfDispatchServer/GetSqlDataServiceResponse")]
        System.Threading.Tasks.Task<object> GetSqlDataServiceAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/GetLogins", ReplyAction="http://tempuri.org/IWcfDispatchServer/GetLoginsResponse")]
        Kts.Gis.Models.LoginModel[] GetLogins();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/GetLogins", ReplyAction="http://tempuri.org/IWcfDispatchServer/GetLoginsResponse")]
        System.Threading.Tasks.Task<Kts.Gis.Models.LoginModel[]> GetLoginsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetAll", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetAllResponse")]
        Kts.Gis.Models.LoginModel[] LoginAccessService_GetAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetAll", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetAllResponse")]
        System.Threading.Tasks.Task<Kts.Gis.Models.LoginModel[]> LoginAccessService_GetAllAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_ChangePassword", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_ChangePasswordResponse")]
        bool LoginAccessService_ChangePassword(int loginId, string oldPassword, string newPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_ChangePassword", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_ChangePasswordResponse")]
        System.Threading.Tasks.Task<bool> LoginAccessService_ChangePasswordAsync(int loginId, string oldPassword, string newPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRestrictions", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRestrictionsResponse")]
        Kts.Gis.Models.AccessModel[] LoginAccessService_GetRestrictions(Kts.Gis.Models.LoginModel login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRestrictions", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRestrictionsResponse")]
        System.Threading.Tasks.Task<Kts.Gis.Models.AccessModel[]> LoginAccessService_GetRestrictionsAsync(Kts.Gis.Models.LoginModel login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRoleName", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRoleNameResponse")]
        string LoginAccessService_GetRoleName(Kts.Gis.Models.LoginModel login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRoleName", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_GetRoleNameResponse")]
        System.Threading.Tasks.Task<string> LoginAccessService_GetRoleNameAsync(Kts.Gis.Models.LoginModel login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_IsPasswordCorrect", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_IsPasswordCorrectRespons" +
            "e")]
        bool LoginAccessService_IsPasswordCorrect(Kts.Gis.Models.LoginModel login, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_IsPasswordCorrect", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_IsPasswordCorrectRespons" +
            "e")]
        System.Threading.Tasks.Task<bool> LoginAccessService_IsPasswordCorrectAsync(Kts.Gis.Models.LoginModel login, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_SetIsUserLogged", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_SetIsUserLoggedResponse")]
        bool LoginAccessService_SetIsUserLogged(int id, string ip, string version, bool isLogged);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/LoginAccessService_SetIsUserLogged", ReplyAction="http://tempuri.org/IWcfDispatchServer/LoginAccessService_SetIsUserLoggedResponse")]
        System.Threading.Tasks.Task<bool> LoginAccessService_SetIsUserLoggedAsync(int id, string ip, string version, bool isLogged);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/MainDispathMethod", ReplyAction="http://tempuri.org/IWcfDispatchServer/MainDispathMethodResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(int[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.Dictionary<int, int>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Data.Packet))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Data.packet2))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.LoginModel[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.LoginModel))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.AccessModel[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.AccessModel))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Gis.Models.AccessKind))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Kts.Utilities.SqlConnectionString))]
        object MainDispathMethod(string callerType, string methodCaller, byte[] listParams);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfDispatchServer/MainDispathMethod", ReplyAction="http://tempuri.org/IWcfDispatchServer/MainDispathMethodResponse")]
        System.Threading.Tasks.Task<object> MainDispathMethodAsync(string callerType, string methodCaller, byte[] listParams);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWcfDispatchServerChannel : Kts.GisClientTest.ServiceReference1.IWcfDispatchServer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WcfDispatchServerClient : System.ServiceModel.ClientBase<Kts.GisClientTest.ServiceReference1.IWcfDispatchServer>, Kts.GisClientTest.ServiceReference1.IWcfDispatchServer {
        
        public WcfDispatchServerClient() {
        }
        
        public WcfDispatchServerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WcfDispatchServerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfDispatchServerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfDispatchServerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int DoWork() {
            return base.Channel.DoWork();
        }
        
        public System.Threading.Tasks.Task<int> DoWorkAsync() {
            return base.Channel.DoWorkAsync();
        }
        
        public int DoIt() {
            return base.Channel.DoIt();
        }
        
        public System.Threading.Tasks.Task<int> DoItAsync() {
            return base.Channel.DoItAsync();
        }
        
        public int[] DoArray() {
            return base.Channel.DoArray();
        }
        
        public System.Threading.Tasks.Task<int[]> DoArrayAsync() {
            return base.Channel.DoArrayAsync();
        }
        
        public System.Collections.Generic.Dictionary<int, int> DoDictionary() {
            return base.Channel.DoDictionary();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, int>> DoDictionaryAsync() {
            return base.Channel.DoDictionaryAsync();
        }
        
        public System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string> getServerList2() {
            return base.Channel.getServerList2();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string>> getServerList2Async() {
            return base.Channel.getServerList2Async();
        }
        
        public System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string> getServerList() {
            return base.Channel.getServerList();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<Kts.Utilities.SqlConnectionString, string>> getServerListAsync() {
            return base.Channel.getServerListAsync();
        }
        
        public Kts.Gis.Data.Packet getPacket() {
            return base.Channel.getPacket();
        }
        
        public System.Threading.Tasks.Task<Kts.Gis.Data.Packet> getPacketAsync() {
            return base.Channel.getPacketAsync();
        }
        
        public bool Initialize(string serverName) {
            return base.Channel.Initialize(serverName);
        }
        
        public System.Threading.Tasks.Task<bool> InitializeAsync(string serverName) {
            return base.Channel.InitializeAsync(serverName);
        }
        
        public object GetSqlDataService() {
            return base.Channel.GetSqlDataService();
        }
        
        public System.Threading.Tasks.Task<object> GetSqlDataServiceAsync() {
            return base.Channel.GetSqlDataServiceAsync();
        }
        
        public Kts.Gis.Models.LoginModel[] GetLogins() {
            return base.Channel.GetLogins();
        }
        
        public System.Threading.Tasks.Task<Kts.Gis.Models.LoginModel[]> GetLoginsAsync() {
            return base.Channel.GetLoginsAsync();
        }
        
        public Kts.Gis.Models.LoginModel[] LoginAccessService_GetAll() {
            return base.Channel.LoginAccessService_GetAll();
        }
        
        public System.Threading.Tasks.Task<Kts.Gis.Models.LoginModel[]> LoginAccessService_GetAllAsync() {
            return base.Channel.LoginAccessService_GetAllAsync();
        }
        
        public bool LoginAccessService_ChangePassword(int loginId, string oldPassword, string newPassword) {
            return base.Channel.LoginAccessService_ChangePassword(loginId, oldPassword, newPassword);
        }
        
        public System.Threading.Tasks.Task<bool> LoginAccessService_ChangePasswordAsync(int loginId, string oldPassword, string newPassword) {
            return base.Channel.LoginAccessService_ChangePasswordAsync(loginId, oldPassword, newPassword);
        }
        
        public Kts.Gis.Models.AccessModel[] LoginAccessService_GetRestrictions(Kts.Gis.Models.LoginModel login) {
            return base.Channel.LoginAccessService_GetRestrictions(login);
        }
        
        public System.Threading.Tasks.Task<Kts.Gis.Models.AccessModel[]> LoginAccessService_GetRestrictionsAsync(Kts.Gis.Models.LoginModel login) {
            return base.Channel.LoginAccessService_GetRestrictionsAsync(login);
        }
        
        public string LoginAccessService_GetRoleName(Kts.Gis.Models.LoginModel login) {
            return base.Channel.LoginAccessService_GetRoleName(login);
        }
        
        public System.Threading.Tasks.Task<string> LoginAccessService_GetRoleNameAsync(Kts.Gis.Models.LoginModel login) {
            return base.Channel.LoginAccessService_GetRoleNameAsync(login);
        }
        
        public bool LoginAccessService_IsPasswordCorrect(Kts.Gis.Models.LoginModel login, string password) {
            return base.Channel.LoginAccessService_IsPasswordCorrect(login, password);
        }
        
        public System.Threading.Tasks.Task<bool> LoginAccessService_IsPasswordCorrectAsync(Kts.Gis.Models.LoginModel login, string password) {
            return base.Channel.LoginAccessService_IsPasswordCorrectAsync(login, password);
        }
        
        public bool LoginAccessService_SetIsUserLogged(int id, string ip, string version, bool isLogged) {
            return base.Channel.LoginAccessService_SetIsUserLogged(id, ip, version, isLogged);
        }
        
        public System.Threading.Tasks.Task<bool> LoginAccessService_SetIsUserLoggedAsync(int id, string ip, string version, bool isLogged) {
            return base.Channel.LoginAccessService_SetIsUserLoggedAsync(id, ip, version, isLogged);
        }
        
        public object MainDispathMethod(string callerType, string methodCaller, byte[] listParams) {
            return base.Channel.MainDispathMethod(callerType, methodCaller, listParams);
        }
        
        public System.Threading.Tasks.Task<object> MainDispathMethodAsync(string callerType, string methodCaller, byte[] listParams) {
            return base.Channel.MainDispathMethodAsync(callerType, methodCaller, listParams);
        }
    }
}
