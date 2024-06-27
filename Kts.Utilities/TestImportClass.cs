using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kts.Utilities
{
    public class TestImportClass
    {
        public TestImportClass()
        {
            //ChannelFactory<TestImportClass> chanelFactory = new ChannelFactory<TestImportClass>();
            testHttpBasic();
        }

        private void testHttpBasic()
        {
            
            BasicHttpBinding myBinding = new BasicHttpBinding();
           

            myBinding.MaxReceivedMessageSize = Int32.MaxValue;
            myBinding.MaxBufferSize = Int32.MaxValue;
            myBinding.MaxBufferPoolSize = Int32.MaxValue;

            myBinding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxDepth = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;

            myBinding.SendTimeout = new TimeSpan(0, 25, 0);
            myBinding.ReceiveTimeout = new TimeSpan(0, 25, 0);


            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:8080/Kts.Gis.Data.WcfDispatchServer");
            
        }
    }
}
