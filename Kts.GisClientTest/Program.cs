using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kts.Gis.Data;
using Kts.Gis.Data.Interfaces;
using Kts.Utilities;
using Kts.Gis.Models;
using System.ServiceModel;

using System.Configuration;
using Kts.WpfUtilities;
using System.Reflection;
using System.ServiceModel.Channels;

namespace Kts.GisClientTest
{
    class Program
    {
        static IDataService m_dataService;
        public static object MainDispathMethod(Type callerType, string methodCaller, params object[] listParams)
        {


            //m_dataService = new WcfDataService("", "", "");
            m_dataService = new SqlDataService(null, "", "", "");


            Type dataServiceType = m_dataService.GetType();
            PropertyInfo[] propInfos = dataServiceType.GetProperties();
            object result = null;
            foreach (var propInfo in propInfos)
            {

                Type propertyType = propInfo.PropertyType;

                if (propertyType.IsInterface)
                {
                    if (propertyType.IsAssignableFrom(callerType))
                    {
                        System.Console.WriteLine("1 тип " + callerType.Name + " принадлежит интерфейсу " + propertyType.Name);
                        object varProperty = propInfo.GetValue(m_dataService, null);


                        MethodInfo theMethod = propertyType.GetMethod(methodCaller);
                        result = theMethod.Invoke(varProperty, listParams);
                    }
                }
            }

            return result;
        }

        public static async Task<object> MainDispathMethodAsync(Type callerType, string methodCaller, params object[] listParams)
        {

            m_dataService = new SqlDataService(null, "", "", "");


            Type dataServiceType = m_dataService.GetType();
            PropertyInfo[] propInfos = dataServiceType.GetProperties();
            object result = null;
            foreach (var propInfo in propInfos)
            {

                Type propertyType = propInfo.PropertyType;

                if (propertyType.IsInterface)
                {
                    if (propertyType.IsAssignableFrom(callerType))
                    {
                        System.Console.WriteLine("1 тип " + callerType.Name + " принадлежит интерфейсу " + propertyType.Name);
                        object varProperty = propInfo.GetValue(m_dataService, null);


                        MethodInfo theMethod = propertyType.GetMethod(methodCaller);
                        result = theMethod.Invoke(varProperty, listParams);
                    }
                }
            }

            return result;
        }


        public static object MainDispathMethod(string callerTypeName, string methodCaller, params object[] listParams)
        {
            m_dataService = new WcfDataService("", "", "");
            //m_dataService = new SqlDataService(null, "", "", "");
            Type dataServiceType = m_dataService.GetType();
            PropertyInfo[] propInfos = dataServiceType.GetProperties();
            Type callerType = Type.GetType(callerTypeName);
            object result = null;
            foreach (var propInfo in propInfos)
            {
                Type propertyType = propInfo.PropertyType;
                if (propertyType.IsInterface)
                {
                    if (propertyType.IsAssignableFrom(callerType))
                    {
                        try
                        {
                            object varProperty = propInfo.GetValue(m_dataService, null);

                            Type varPropertyType = varProperty.GetType();


                            Type[] callerTypes = new Type[listParams.Length];

                            for (int i = 0; i < callerTypes.Length; i++)
                            {
                                callerTypes[i] = listParams[i].GetType();
                                
                            }

                            MethodInfo[] arrayMethodInfo = varProperty.GetType().GetMethods();

                            for (int i = 0; i < arrayMethodInfo.Length; i++)
                            {
                                System.Console.WriteLine("method name = " + arrayMethodInfo[i].Name);
                            }

                            MethodInfo theMethod = varProperty.GetType().GetMethod(methodCaller, callerTypes);


                            MethodInfo testEmptyArgMethod = varProperty.GetType().GetMethod(methodCaller);


                            //MethodInfo theMethod = propertyType.GetMethod(methodCaller);
                            result = theMethod.Invoke(varProperty, listParams);
                            
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine("exceptionMessage = " + e.Message);
                        }
                        break;

                    }
                }
                
            }

            return result;
        }

        static object retObj()
        {

           

            List<LoginModel> result = new List<LoginModel>();

            result.Add(new LoginModel(1, "qf"));
            result.Add(new LoginModel(1, "qf"));
            result.Add(new LoginModel(1, "qf"));
            result.Add(new LoginModel(1, "qf"));
            result.Add(new LoginModel(1, "qf"));
            result.Add(new LoginModel(1, "qf"));
            result.Add(new LoginModel(1, "qf"));
            object newresult = result;
            return newresult;
        }

        static void Main(string[] args)
        {
            //Kts.Gis.Data.WcfFigureAccessService z;
            /*
            object tObj = 25;

            var resul = retObj();
            if (tObj is int)
            {
                resul = retObj();
            }            
            IDataService dataService = new WcfDataService("", "", "");


            //MainDispathMethod(dataService.FigureAccessService.GetType().AssemblyQualifiedName, "GetAll", 540, new SchemaModel(2019, "2019", true, false));
            MainDispathMethod(dataService.LoginAccessService.GetType().AssemblyQualifiedName, "GetAll");

            */

            IDataService dataService = new WcfDataService("", "", "");
            //MainDispathMethod
            //Task<object> ttt = MainDispathMethodAsync(dataService.LoginAccessService.GetType(), "ChangePassword", 5, "oldPassword", "newPassword");


            



            //MainDispathMethod(dataService.LoginAccessService.GetType(), "GetAll");
            //public bool ChangePassword(int loginId, string oldPassword, string newPassword)
            //MainDispathMethod(dataService.LoginAccessService.GetType(), "ChangePassword", 5, "oldPassword", "newPassword");
            //private DataWcfRef.WcfFigureAccessServiceClient netTcpWcfRef;
            IWcfDispatchServer m_wcfConnector;
            ChannelFactory<IWcfDispatchServer> m_chanelFactory;
           
            BasicHttpBinding myBinding = new BasicHttpBinding();
            Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);
            myBinding.MaxReceivedMessageSize = Int32.MaxValue;
            myBinding.MaxBufferSize = Int32.MaxValue;
            myBinding.MaxBufferPoolSize = Int32.MaxValue;

            

            myBinding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxDepth = Int32.MaxValue;
            myBinding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
            
            Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);












            //EndpointAddress myEndpoint = new EndpointAddress("http://localhost:8080/Kts.Gis.Data.WcfFigureAccessService");
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:8080/Kts.Gis.Data.WcfDispatchServer");

            m_chanelFactory = new ChannelFactory<IWcfDispatchServer>(myBinding, myEndpoint);
            
            
            IWcfDispatchServer wcfClient1 = m_chanelFactory.CreateChannel();

            int doWork = wcfClient1.DoWork();


            //wcfClient1.Initialize("Якутск");
            //bool result = wcfClient1.Initialize("Якутск").GetAwaiter().GetResult();


            //Task<bool> result = wcfClient1.Initialize("Якутск");

            //bool res = result.Result;
            //Console.WriteLine("res = " + res);

            //bool result = wcfClient1.Initialize("Якутск").GetAwaiter().GetResult();

            //Task<bool> result = wcfClient1.Initialize("Якутск");
            //bool result = 
            //Task<bool> result = Task.Run( async () => await wcfClient1.Initialize("Якутск"));
            //Task<bool> result = Task.Run(async () => await wcfClient1.Initialize("Якутск"));
            IDataService wcfService = new WcfDataService("", "", "");


            wcfClient1.Initialize("Якутск");
            
           Dictionary<SqlConnectionString, string> serverList = wcfClient1.getServerList();


           foreach (var item in serverList)
           {
               Console.WriteLine(item.Key.Name + " " + item.Key.Server + " ");
           }
           







            try
            {

                var task = wcfClient1.Initialize("Якутск");
                do
                {

                    Console.WriteLine(task.Status);
                } while (task.Status == TaskStatus.WaitingForActivation);
                Console.WriteLine(task.Status);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);
            List<FigureModel> figureModelsList = wcfService.FigureAccessService.GetAll(540, new SchemaModel(2019, "2019", true, false));
            //result.Wait();
            Packet test = wcfClient1.getPacket();


            var variable = wcfClient1.GetLogins();

            Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);

            List<AccessModel> listAccess = wcfService.LoginAccessService.GetRestrictions(variable[0]);
            Console.WriteLine("bindin messageSize = " + myBinding.MaxReceivedMessageSize);


            

            //var getall = wcfService.LoginAccessService.GetAll();
            //bool res = wcfService.LoginAccessService.ChangePassword(0, "oldPassword", "newPassword");//"ChangePassword", 5, "oldPassword", "newPassword");

            //string result = wcfService.LoginAccessService.GetRoleName(new LoginModel(0, "vasa"));

            var t = wcfClient1.GetSqlDataService();

            var l = t.getLoginAccessService();

            wcfClient1.GetSqlDataService().LoginAccessService.GetAll();


            

            //Console.WriteLine("result = " + result);






            /*
            List<LoginViewModel> logins = null;
            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(new WaitViewModel("Загрузка пользователей", "Пожалуйста подождите, идет загрузка пользователей...", async () =>
            {
                await Task.Factory.StartNew(() => logins = LoginViewModel.GetLogins(this.DataService));
            })));*/





            System.Console.WriteLine(test.ToString());
            System.Console.WriteLine("test.name = " + test.test);
            ((IClientChannel)wcfClient1).Close();

            /*


            WcfGisData.WcfFigureAccessServiceClient client = new WcfGisData.WcfFigureAccessServiceClient("NetTcpBinding_IWcfFigureAccessService");
            Console.WriteLine(" = " + client.DoWork());


            
            Dictionary<SqlConnectionString, string> serverList = client.getServerList();
            foreach (var item in serverList)
            {
                Console.WriteLine(item.Key.Name + " " + item.Key.Server + " ");
            }



            //Dictionary<WcfGisData.SqlConnectionString,string> result = client.getServerList();

            bool result = client.Initialize("Якутск");

            Console.WriteLine("result = " + result);



            List<LoginModel> test =   client.GetLogins();
            foreach (var item in test)
            {
                Console.WriteLine("id = " + item.Id + " name = " + item.Name);
            }
             
            
            

            //Kts.Gis.Data.WcfFigureAccessService client = new Kts.Gis.Data.WcfFigureAccessService("NetTcpBinding_ISummator");

            // Используйте переменную "client", чтобы вызвать операции из службы.

            // Всегда закройте клиент.
            client.Close();*/
            Console.ReadLine();
        }
    }
}
