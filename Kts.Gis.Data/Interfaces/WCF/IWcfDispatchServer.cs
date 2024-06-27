using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWcfFigureAccessService" in both code and config file together.

    public struct testData
    {
        public int i;
        public string i_test;
        public bool[] i_bool;
    }

    [ServiceContract]
    public interface IWcfDispatchServer
    {
        [OperationContract]
        string HelloWorld();
        [OperationContract]
        string SayHello(string name);
        [OperationContract]
        testData TestData();

        [OperationContract]

        Dictionary<string, List<Dictionary<string, Stream>>> GetImagesFilesSubscrabes(SubstrateModel substrate, string sourceFolderName);

        [OperationContract]
        Stream GetStreamImagesFilesSubscrabes(SubstrateModel substrate, string sourceFolderName);

        [OperationContract]
        string GetThumbnailName(int id);

        [OperationContract]
        Stream GetStreamThumbnail(int id);

        [OperationContract]
        Task<List<string>> GetImagesFiles(SubstrateModel substrate, string sourceFolderName);
        [OperationContract]
        int DoWork();
        [OperationContract]
        int DoWork2();
        [OperationContract]
        
        Stream Test(SubstrateModel substrate, string sourceFolderName);
        [OperationContract]
        int DoIt();
        [OperationContract]
        int[] DoArray();

        [OperationContract]
        string[] GetDataContent();

        [OperationContract]
        Dictionary<int, int> DoDictionary();

        [OperationContract]
        Dictionary<SqlConnectionString, string> getServerList2();

        [OperationContract]
        Dictionary<SqlConnectionString, string> getServerList();
        
        [OperationContract]
        Packet getPacket();


        [OperationContract]
        Task<bool> Initialize(string serverName);
        [OperationContract]
        Task<bool> InitializeLoadDataAsync(int userID);

        [OperationContract]
        IDataService GetSqlDataService();

        [OperationContract]
        List<LoginModel> GetLogins();


        // ****************************
        // * LoginAccessService       *
        // ****************************
        [OperationContract]
        List<LoginModel> LoginAccessService_GetAll();

        [OperationContract]
        bool LoginAccessService_ChangePassword(int loginId, string oldPassword, string newPassword);
        [OperationContract]
        List<AccessModel> LoginAccessService_GetRestrictions(LoginModel login);

        [OperationContract]
        string LoginAccessService_GetRoleName(LoginModel login);

        [OperationContract]
        bool LoginAccessService_IsPasswordCorrect(LoginModel login, string password);

        [OperationContract]
        bool LoginAccessService_SetIsUserLogged(int id, string ip, string version, bool isLogged);

        [OperationContract]

        //object MainDispathMethod(string callerType, string methodCaller, params object[] listParams);
        object MainDispathMethod(string callerType, string methodCaller, byte[] listParams);

        [OperationContract]
        object GetBadgeGeometries();
        [OperationContract]
        object GetObjectTypes();

        [OperationContract]
        object GetSchemas();

        [OperationContract]
        object DataServiceDispathMethod(string methodCaller, byte[] byteArrayParams);

        [OperationContract]
        Task<object> MainDispathMethodAsync_(string callerTypeName, string methodCaller, byte[] byteArrayParams);
        [OperationContract]
        Task<object> DataServiceDispathMethodAsync_(string methodCaller, byte[] byteArrayParams);
        //[OperationContract]
        //Dictionary<SqlConnectionString, string> getServerList();
    }

}
