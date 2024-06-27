using System;

using System.ServiceModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Kts.Gis.Data
{
    public class LogFile
    {

        public static void Log(string message)
        {
            try
            {
                System.IO.File.AppendAllText("log.txt", message);
            }
            catch
            {

            }
        }
    }
    public abstract class BaseWcfDataAccessService
    {
        protected IWcfDispatchServer m_wcfConnector;
        protected ChannelFactory<IWcfDispatchServer> m_chanelFactory;


        public BaseWcfDataAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory)
        {
            m_chanelFactory = chanelFactory;
        }


        protected async Task<object> callDispathServerMethodAsync(Type callerType, string methodCaller, params object[] listParams)
        {



            //object result = wcfClient.MainDispathMethod(name, methodCaller, listParams);


            var wcfClient = m_chanelFactory.CreateChannel();
            object resObject = null;
            try
            {

                string name = callerType.AssemblyQualifiedName;
                var getlogins = wcfClient.GetLogins();
                BinaryFormatter binAry = new BinaryFormatter();
                MemoryStream memStreamIn = new MemoryStream();
                binAry.Serialize(memStreamIn, listParams);

                memStreamIn.Close();


                //object result = await wcfClient.MainDispathMethodAsync_(name, methodCaller, memStreamIn.ToArray());

                object result = await wcfClient.MainDispathMethodAsync_(name, methodCaller, memStreamIn.ToArray());
                //http://qaru.site/questions/558924/how-to-call-a-generic-async-method-using-reflection
                if (result != null)
                {

                    MemoryStream memStream = new MemoryStream(result as byte[]);
                    BinaryFormatter bin = new BinaryFormatter();
                    resObject = bin.Deserialize(memStream);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("inner exception = " + e.InnerException.Message + "exception message " + e.Message);
            }

            ((IClientChannel)wcfClient).Close();
            return resObject;

            //List <LoginModel> vr = bin.Deserialize(memStream) as List<LoginModel>;
            /*
            BinaryFormatter bin = new BinaryFormatter();

            var desRes = bin.Deserialize(result);
            */


        }

        protected string GetCurrentMethod([CallerMemberName] string callerName = "")
        {
            return callerName;
        }

        protected object callDispathServerMethod(Type callerType, string methodCaller, params object[] listParams)
        {



            //object result = wcfClient.MainDispathMethod(name, methodCaller, listParams);


            var wcfClient = m_chanelFactory.CreateChannel();
            object resObject = null;
            try
            {
                
                string name = callerType.AssemblyQualifiedName;
                var getlogins = wcfClient.GetLogins();
                BinaryFormatter binAry = new BinaryFormatter();
                MemoryStream memStreamIn = new MemoryStream();
                binAry.Serialize(memStreamIn, listParams);

                memStreamIn.Close();


                Object result = wcfClient.MainDispathMethod(name, methodCaller, memStreamIn.ToArray());
                
                if (result != null)
                {

                    MemoryStream memStream = new MemoryStream(result as byte[]);
                    BinaryFormatter bin = new BinaryFormatter();
                    resObject = bin.Deserialize(memStream);
                }
            }
            catch (Exception e)
            {
                
                Console.WriteLine("inner exception = "+ e.InnerException.Message + "exception message " + e.Message);
            }

            ((IClientChannel)wcfClient).Close();
            return resObject;

            //List <LoginModel> vr = bin.Deserialize(memStream) as List<LoginModel>;
            /*
            BinaryFormatter bin = new BinaryFormatter();

            var desRes = bin.Deserialize(result);
            */


        }
    }
}
