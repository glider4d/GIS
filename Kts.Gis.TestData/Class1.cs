using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Kts.Gis.TestData
{
    [ServiceContract]
    interface IClass1
    {
        [OperationContract]
        int DoWork();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Class1 : IClass1
    {
        public int DoWork()
        {
            return 25;
        }
    }
}
