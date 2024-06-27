using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Kts.Gis.Data
{
    
    public class packet2
    {
        public int id;
        public string name
        {
            get;set;
        }
    }
    [Serializable]
    public class Packet
    {
        public packet2 tyj
        {
            get;set;
        }
        public Packet(int id, string name, string test)
        {
            this.id = id;
            this.name = name;
            this.test = test;
        }
        public int id
        {
            get;set;
        }

        public string name
        {
            get;set;
        }

        public string test
        {
            get;set;
        }
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WcfFigureAccessService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfDispatchServer : IWcfDispatchServer
    {
        
        private IDataService m_dataService;


        

        private int countCall = 0;
        private Dictionary<SqlConnectionString, string> m_serverList;

 
        public IDataService GetSqlDataService()
        {
            return m_dataService;
        }

        public bool MobileInitialize(string name)
        {
            bool result = false;

            
            if (m_dataService == null || (m_dataService.ServerName != null && !m_dataService.ServerName.Equals(name)))
            {

                foreach (var item in m_serverList)
                {
                    if (item.Key.Name.Equals(name))
                    {
                        m_dataService = new SqlDataService(new SqlReconnectorWcf(item.Key)
                            , @"\\" + item.Value + "\\Gis\\Errors\\",
                              @"\\" + item.Value + "\\Gis\\Images\\",
                              @"\\" + item.Value + "\\Gis\\Thumbnails\\");
                        result = true;
                        m_dataService.LoadDataAsync();



                        break;
                    }
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> Initialize(string name)
        {
            
            bool result = false;
            if (m_dataService == null)
            {

                foreach (var item in m_serverList)
                {
                    if (item.Key.Name.Equals(name))
                    {

                        
                        m_dataService = new SqlDataService(new SqlReconnectorWcf(item.Key)
                            , @"\\" + item.Value + "\\Gis\\Errors\\",
                              @"\\" + item.Value + "\\Gis\\Images\\",
                              @"\\" + item.Value + "\\Gis\\Thumbnails\\");
                        result = true;
                        await m_dataService.LoadDataAsync();

                        
                        
                        break;
                    }
                }
            }
            else
            {
                result = true;
            }
            
            return result;
        }

        public List<LoginModel> GetLogins()
        {
            Log("GetLogins in");
            return m_dataService.LoginAccessService.GetAll();
        }


        public Packet getPacket()
        {
            return new Packet(0, "имя", "тест");
        }
        public int DoWork()
        {
            return countCall++;
        }

        public int DoIt()
        {
            return 75;
        }

        public int[] DoArray()
        {
            int[] result = new int[32];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = i; 
            }
            return result;
        }


        public Dictionary<int, int> DoDictionary()
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            for (int i = 0; i < 10; i++)
            {
                result.Add(i, i);
            }
            return result;
        }


        public Dictionary<SqlConnectionString, string> getServerList2()
        {

            return new Dictionary<SqlConnectionString, string>();
        }
        public Dictionary<SqlConnectionString, string> getServerList()
        {


            if (m_serverList == null)
            {

                var serverData = new Dictionary<SqlConnectionString, string>();

                string[] dataContent = new string[0];

                var forcedDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\ForcedServerData";




                var dataPath = "\\\\172.16.4.58\\Gis\\ServerData";
                //var dataPath = "ServerData";
                //var dataPath = "e:\\1\\ServerData.txt";




                // Проверяем наличие файла, с вшитыми данными серверов.
                try
                {
                    if (File.Exists(forcedDataPath))
                        dataContent = File.ReadAllLines(forcedDataPath);
                    else
                        dataContent = File.ReadAllLines(dataPath);
                }
                catch
                {
                }

                // Также проверяем наличие файла ForcedAddress. Он использовался в ранних версиях приложения для ручного задания адреса сервера.
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\ForcedAddress"))
                    // Если такой существует, то мы обнуляем данные серверов. Это сделано для того, чтобы пользователи могли обратиться к нам для создания новой версии этого файла - ForcedServerData.
                    dataContent = new string[0];

                // Разбираем данные серверов.

                string dbAddress;
                string userId;
                string password;
                string name;
                string fileAddress;
                for (int i = 0; i < dataContent.Length; i += 5)
                {
                    dbAddress = dataContent[i];
                    userId = dataContent[i + 1];
                    password = dataContent[i + 2];
                    name = dataContent[i + 3];
                    fileAddress = dataContent[i + 4];

                    serverData.Add(new SqlConnectionString(dbAddress, userId, password, "w2AqAvKcgOh5iQuIodC6rw==", 90, name, true), fileAddress);
                }
                m_serverList = serverData;
            }
            return m_serverList;
        }




        public bool LoginAccessService_ChangePassword(int loginId, string oldPassword, string newPassword)
        {
            return m_dataService.LoginAccessService.ChangePassword(loginId, oldPassword, newPassword);
        }

        public List<AccessModel> LoginAccessService_GetRestrictions(LoginModel login)
        {
            return m_dataService.LoginAccessService.GetRestrictions(login);
        }


        public string LoginAccessService_GetRoleName(LoginModel login)
        {
            return m_dataService.LoginAccessService.GetRoleName(login);
        }


        public bool LoginAccessService_IsPasswordCorrect(LoginModel login, string password)
        {
            return m_dataService.LoginAccessService.IsPasswordCorrect(login, password);
        }


        public bool LoginAccessService_SetIsUserLogged(int id, string ip, string version, bool isLogged)
        {
            return m_dataService.LoginAccessService.SetIsUserLogged(id, ip, version, isLogged);
        }

        public List<LoginModel> LoginAccessService_GetAll()
        {
            return m_dataService.LoginAccessService.GetAll();
        }
        public static void Log(string message)
        {
            try
            {
                System.IO.File.AppendAllText("D:\\1\\log.txt", message + "\r\n");
            }
            catch
            {

            }
        }
        
        /**/
        private Tuple<object[], MethodInfo> dispathMethodIn(byte[] byteArrayParams, string methodCaller)
        {
            BinaryFormatter binIn = new BinaryFormatter();
            MemoryStream streamIn = new MemoryStream(byteArrayParams);

            object[] listParams = binIn.Deserialize(streamIn) as object[];
            streamIn.Close();

            Type dataServiceType = m_dataService.GetType();

            bool paramNullFlag = false;
            Type[] callerTypes = null;

            if (listParams != null)
            {
                if (listParams.Length > 0)
                {
                    callerTypes = new Type[listParams.Length];
                    for (int i = 0; i < listParams.Length; i++)
                    {
                        if (listParams[i] == null)
                        {
                            paramNullFlag = true;
                            break;
                        }
                        callerTypes[i] = listParams[i].GetType();
                    }
                }
            }

            if (callerTypes == null)
                callerTypes = new Type[0];

            MethodInfo theMethod = null;
            //dataServiceType.GetMethods()
            if (paramNullFlag)
            {
                var methods = from t in dataServiceType.GetMethods() where t.Name.Equals(methodCaller) select t;

                foreach (var method in methods)
                {
                    if (method.GetParameters().Length == listParams.Length)
                    {
                        theMethod = method;
                        break;
                    }
                }
            }
            else
            {
                theMethod = dataServiceType.GetMethod(methodCaller, callerTypes);
            }

            return new Tuple<object[], MethodInfo>(listParams, theMethod);
        }

        private byte[] dispathMethodOut(object objRes)
        {
            byte[] byteRes = null;
            if (objRes != null)
            {

                BinaryFormatter bin = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                bin.Serialize(memStream, objRes);

                memStream.Close();
                byteRes = memStream.ToArray();
            }
            return byteRes;
        }

        public async Task<object> MainDispathMethodAsync_(string callerTypeName, string methodCaller, byte[] byteArrayParams)
        {
            byte[] resultSerialize = null;
            try
            {
                Tuple<object[], MethodInfo, object> inParam = dispathMainMethodIn(callerTypeName, byteArrayParams, methodCaller);
                //object resultInvoke = inParam.Item2.Invoke(inParam.Item3, inParam.Item1);
                //                Task<Car> result = (Task<Car>)method.Invoke(obj, null);
                //              await result;
                /*Task<object> resultInvoke = (Task<object>)inParam.Item2.Invoke(inParam.Item3, inParam.Item1);
                
                await resultInvoke;*/

                var resultInvoke = inParam.Item2.Invoke(inParam.Item3, inParam.Item1);

                


                if (resultInvoke is Task)
                {
                    await (resultInvoke as Task);
                    var resultProperty = resultInvoke.GetType().GetProperty("Result");
                    
                    resultSerialize = dispathMethodOut(resultProperty.GetValue(resultInvoke, null));
                    //resultSerialize = dispathMethodOut((resultInvoke as Task).)
                }

                //await resultInvoke;


                //resultSerialize = dispathMethodOut(resultInvoke.Result);
            }
            catch (Exception exp)
            {
                Log("MainDispathMethod  typeCaller: " + callerTypeName + " methoCaller: " + methodCaller + " exceptionMessage: " + exp.Message);
            }
            return resultSerialize;
        }
        

        public async Task<object> DataServiceDispathMethodAsync_(string methodCaller, byte[] byteArrayParams)
        {
            byte[] resultSerialize = null;
            try
            {

                Tuple<object[], MethodInfo> inParam = dispathMethodIn(byteArrayParams, methodCaller);
                Task<object> resultInvoke = (Task<object>)inParam.Item2.Invoke(m_dataService, inParam.Item1);
                await resultInvoke;
                
                resultSerialize = dispathMethodOut(resultInvoke.Result);
            }
            catch (Exception exp)
            {
                Log("DataServiceDispathMethod  methoCaller: " + methodCaller + " exceptionMessage: " + exp.Message);
            }
            return resultSerialize;
        }
        


        public object DataServiceDispathMethod(string methodCaller, byte[] byteArrayParams)
        {
            byte[] resultSerialize = null;
            try
            {

                Tuple<object[], MethodInfo> inParam = dispathMethodIn(byteArrayParams, methodCaller);
                object resultInvoke = inParam.Item2.Invoke(m_dataService, inParam.Item1);
                resultSerialize = dispathMethodOut(resultInvoke);
            } catch(Exception exp)
            {
                Log("DataServiceDispathMethod  methoCaller: " + methodCaller + " exceptionMessage: " + exp.Message);
            }
            return resultSerialize;

            

            //result = theMethod.Invoke(m_dataService, listParams);
            /*
            
            BinaryFormatter binIn = new BinaryFormatter();
            MemoryStream streamIn = new MemoryStream(byteArrayParams);

            object[] listParams = binIn.Deserialize(streamIn) as object[];
            streamIn.Close();

            object result = null;
            try
            {
                Type dataServiceType = m_dataService.GetType();

                bool paramNullFlag = false;
                Type[] callerTypes = null;

                if (listParams != null)
                {
                    if (listParams.Length > 0)
                    {
                        callerTypes = new Type[listParams.Length];
                        for (int i = 0; i < listParams.Length; i++)
                        {
                            if (listParams[i] == null)
                            {
                                paramNullFlag = true;
                                break;
                            }
                            callerTypes[i] = listParams[i].GetType();
                        }
                    }
                }

                if (callerTypes == null)
                    callerTypes = new Type[0];

                MethodInfo theMethod = null;
                //dataServiceType.GetMethods()
                if (paramNullFlag)
                {

                    var methods = from t in dataServiceType.GetMethods() where t.Name.Equals(methodCaller) select t;
                    
                    foreach (var method in methods)
                    {
                        if (method.GetParameters().Length == listParams.Length)
                        {
                            theMethod = method;
                            break;
                        }
                    }
                }
                else
                {
                    theMethod = dataServiceType.GetMethod(methodCaller, callerTypes);
                    
                }
                result = theMethod.Invoke(m_dataService, listParams);
            } catch (Exception e)
            {
                Log("exceptionMessage = " + e.Message);
            }
            byte[] rr = null;
            if (result != null)
            {

                BinaryFormatter bin = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                bin.Serialize(memStream, result);

                memStream.Close();
                rr = memStream.ToArray();
            }
            return rr;
            */
        }
        
        private Tuple<object[], MethodInfo, object> dispathMainMethodIn(string callerTypeName, byte[] byteArrayParams, string methodCaller)
        {
            BinaryFormatter binIn = new BinaryFormatter();
            MemoryStream streamIn = new MemoryStream(byteArrayParams);

            object[] listParams = binIn.Deserialize(streamIn) as object[];
            streamIn.Close();

            object resultVarProperty = null;
            Type dataServiceType = m_dataService.GetType();

            PropertyInfo[] propInfos = dataServiceType.GetProperties();
            Type callerType = Type.GetType(callerTypeName);

            MethodInfo resMethodInfo = null;
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




                            Type[] callerTypes = null;

                            if (listParams != null)
                            {
                                if (listParams.Length > 0)
                                {
                                    callerTypes = new Type[listParams.Length];
                                    for (int i = 0; i < listParams.Length; i++)
                                    {
                                        callerTypes[i] = listParams[i].GetType();
                                    }
                                }
                            }

                            if (callerTypes == null)
                                callerTypes = new Type[0];
                            //MethodInfo theMethod = varProperty.GetType().GetMethod(methodCaller, callerTypes);
                            resMethodInfo = varProperty.GetType().GetMethod(methodCaller, callerTypes);
                            resultVarProperty = varProperty;
                        }
                        catch(Exception exp) {
                            Log("dispathMainMethodIn  typeCaller: " + callerTypeName + " methoCaller: " + methodCaller + " exceptionMessage: " + exp.Message);
                        }
                        break;
                    }
                }
            }
            return new Tuple<object[], MethodInfo, object>(listParams, resMethodInfo, resultVarProperty);
        }

        
        public object MainDispathMethod(string callerTypeName, string methodCaller, byte[] byteArrayParams) //params object[] listParams)
        {
            
            byte[] resultSerialize = null;
            try
            {
                Tuple<object[], MethodInfo, object> inParam = dispathMainMethodIn(callerTypeName, byteArrayParams, methodCaller);
                object resultInvoke = inParam.Item2.Invoke(inParam.Item3, inParam.Item1);
                resultSerialize = dispathMethodOut(resultInvoke);
            }
            catch (Exception exp)
            {
                Log("MainDispathMethod  typeCaller: " + callerTypeName + " methoCaller: " + methodCaller + " exceptionMessage: " + exp.Message);
            }
            return resultSerialize;
            /*
            Log("MainDispathMethod in "+callerTypeName + " "+methodCaller + " ");


            //dispathMethodIn(byteArrayParams, methodCaller);
            
            BinaryFormatter binIn = new BinaryFormatter();
            MemoryStream streamIn = new MemoryStream(byteArrayParams);

            object[] listParams = binIn.Deserialize(streamIn) as object[];
            streamIn.Close();


            Type dataServiceType = m_dataService.GetType();

            Log("dataServiceType = " + dataServiceType.Name);
            PropertyInfo[] propInfos = dataServiceType.GetProperties();
            Type callerType = Type.GetType(callerTypeName);
            Log("callerType = " + callerType.Name);
            
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
                            
                            
                            

                            Type[] callerTypes = null;

                            if (listParams != null)
                            {
                                if (listParams.Length > 0)
                                {
                                    callerTypes = new Type[listParams.Length];
                                    for (int i = 0; i < listParams.Length; i++)
                                    {
                                        callerTypes[i] = listParams[i].GetType();
                                    }
                                }
                            }

                            if (callerTypes == null)
                                callerTypes = new Type[0];

                            


                            
                            Log("8 "+varProperty.ToString());
                            Log("8_8 "+callerType.Name + " method caller =  "+methodCaller);
                            //Log("8_9 " + propertyType.Name);
                            //MethodInfo theMethod = callerType.GetMethod(methodCaller);
                            //MethodInfo theMethod = propertyType.GetMethod(methodCaller);
                            MethodInfo theMethod = varProperty.GetType().GetMethod(methodCaller, callerTypes);
                            Log("9 "+ theMethod.Name );
                            result = theMethod.Invoke(varProperty, listParams);
                        }
                        catch(Exception e)
                        {
                            Log("exceptionMessage = " + e.Message);
                        }
                        Log("10");
                        break;

                    }
                }
                Log("8");
            }




            
            byte[] rr = null;
            if (result != null)
            {


                BinaryFormatter bin = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();
                bin.Serialize(memStream, result);

                memStream.Close();





                Log("MainDispathMethod out " + result.ToString());
                rr = memStream.ToArray();
            }
            return rr;
            */
        }

        string getNameCaller(string membername)
        {
            return membername;
        }

        public object GetObjectsTypes()
        {
            List<ObjectType> varList = m_dataService.ObjectTypes.ToList<ObjectType>();

            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            bin.Serialize(memStream, varList);
            byte[] rr = memStream.ToArray();
            memStream.Close();

            return rr;
        }

        public object GetBadgeGeometries()
        {
            List<BadgeGeometryModel> varList = m_dataService.BadgeGeometries.ToList<BadgeGeometryModel>();

            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            bin.Serialize(memStream, varList);
            byte[] rr = memStream.ToArray();            
            memStream.Close();

            return rr;
            /*

            BinaryFormatter binIn = new BinaryFormatter();
            MemoryStream streamIn = new MemoryStream(rr);

            List<BadgeGeometryModel> listParams = binIn.Deserialize(streamIn) as List<BadgeGeometryModel>;
            streamIn.Close();

            

            return varList;*/
        }

        public object GetSchemas()
        {
            List<SchemaModel> varList = m_dataService.Schemas;

            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            bin.Serialize(memStream, varList);
            byte[] rr = memStream.ToArray();
            memStream.Close();

            return rr;
        }

        public object GetObjectTypes()
        {
            List<ObjectType> varList = m_dataService.ObjectTypes.ToList<ObjectType>();

            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            bin.Serialize(memStream, varList);
            byte[] rr = memStream.ToArray();
            memStream.Close();

            return rr;
        }

        public bool testBoolProvide()
        {
            return true;
        }
    }
}
