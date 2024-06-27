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

namespace Kts.Utilities
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWcfFigureAccessService" in both code and config file together.
    [ServiceContract]
    public interface IWcfDispatchServer2
    {
        [OperationContract]
        int DoWork();

        [OperationContract]
        int DoIt();
        [OperationContract]
        int[] DoArray();

        [OperationContract]
        Dictionary<int, int> DoDictionary();

        [OperationContract]
        Dictionary<SqlConnectionString, string> getServerList2();

        [OperationContract]
        Dictionary<SqlConnectionString, string> getServerList();
        








        // ****************************
        // * LoginAccessService       *
        // ****************************

        [OperationContract]
        bool LoginAccessService_ChangePassword(int loginId, string oldPassword, string newPassword);

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

        


        //[OperationContract]
        //Dictionary<SqlConnectionString, string> getServerList();
    }

}
