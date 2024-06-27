using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kts.Gis.Data.Interfaces;
using Kts.Gis.Models;
using System.ServiceModel;
//using Kts.Gis.Data.WcfDataRef;
using Kts.Utilities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace Kts.Gis.Data
{
    [Serializable]
    public sealed class WcfDataService : IDataService
    {

        IWcfDispatchServer m_wcfConnector;
        ChannelFactory<IWcfDispatchServer> m_chanelFactory;

        
        
        public WcfDataService(string errorFolderName, string substrateFolderName, string thumbnailFolderName)
        {
            
            ErrorFolderName = errorFolderName;
            SubstrateFolderName = substrateFolderName;
            ThumbnailFolderName = thumbnailFolderName;
            BasicHttpBinding myBinding = new BasicHttpBinding();
            /*
            NetTcpBinding myBinding = new NetTcpBinding(SecurityMode.None);

            myBinding.Security.Mode = SecurityMode.None;
            */

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
            m_chanelFactory = new ChannelFactory<IWcfDispatchServer>(myBinding, myEndpoint);
            IWcfDispatchServer wcfClient1 = m_chanelFactory.CreateChannel();
            this.LoginAccessService = new WcfLoginAccessService(m_chanelFactory);
            this.FigureAccessService = new WcfFigureAccessService(m_chanelFactory);
            this.BadgeAccessService = new WcfBadgeAccessService(m_chanelFactory);
            this.BoilerInfoAccessService = new WcfBoilerInfoAccessService(m_chanelFactory);
            this.CustomLayerAccessService = new WcfCustomLayerAccessService(m_chanelFactory);
            this.CustomObjectAccessService = new WcfCustomObjectAccessService(m_chanelFactory);
            this.DocumentAccessService = new WcfDocumentAccessService(m_chanelFactory);
            this.FuelAccessService = new WcfFuelAccessService(m_chanelFactory);
            this.GlobalMapAccessService = new WcfGlobalMapAccessService(m_chanelFactory);
            this.KtsAccessService = new WcfKtsAccessService(m_chanelFactory);
            this.LabelAccessService = new WcfLabelAccessService(m_chanelFactory);
            this.LineAccessService = new WcfLineAccessService(m_chanelFactory);
            this.MapAccessService = new WcfMapAccessService(m_chanelFactory);
            this.MeterAccessService = new WcfMeterAccessService(m_chanelFactory);
            this.NodeAccessService = new WcfNodeAccessService(m_chanelFactory);
            this.NonVisualObjectAccessService = new WcfNonVisualObjectAccessService(m_chanelFactory);
            this.ParameterAccessService = new WcfParameterAccessService(m_chanelFactory);
            this.ReportAccessService = new WcfReportAccessService(m_chanelFactory);
            this.SearchService = new WcfSearchService(m_chanelFactory);
            this.TerritorialEntityAccessService = new WcfTerritorialEntityAccessService(m_chanelFactory);

            ((IClientChannel)wcfClient1).Close();
        }

        public Task<bool> Initialize(string serverName)
        {
            var wcfClient = m_chanelFactory.CreateChannel();
            var result = wcfClient.Initialize(serverName);
            ((IClientChannel)wcfClient).Close();
            return result;
        }


        public int doWork()
        {
            var wcfClient = m_chanelFactory.CreateChannel();

            int result = wcfClient.DoWork();
            ((IClientChannel)wcfClient).Close();
            return result;
        }

        public void test3()
        {

            var wcfClient = m_chanelFactory.CreateChannel();

            List<LoginModel> loginsmodls = wcfClient.GetLogins();

            foreach (var item in loginsmodls)
            {
                
            }
        }

        public void test2()
        {
            var wcfClient = m_chanelFactory.CreateChannel();
            
            
            SqlDataService test = (SqlDataService)wcfClient.GetSqlDataService();
            

            ((IClientChannel)wcfClient).Close();
            
        }
        public void test()
        {
            /*
            var serverList = netTcpWcfRef.getServerList();
            foreach (var item in serverList)
            {
                System.Windows.MessageBox.Show(" "+item.Key.Name);
            }*/


            
        }

        public Dictionary<SqlConnectionString, string> GetServerList()
        {
            var wcfClient = m_chanelFactory.CreateChannel();
            var result = wcfClient.getServerList();
            ((IClientChannel)wcfClient).Close();
            return result;
        }


        List<SchemaModel> m_schemas;

        public List<SchemaModel> Schemas
        {
            get
            {
                if (m_schemas == null)
                {
                    var wcfClient = m_chanelFactory.CreateChannel();
                    try
                    {
                        byte[] result = wcfClient.GetSchemas() as byte[];
                        BinaryFormatter binIn = new BinaryFormatter();
                        MemoryStream streamIn = new MemoryStream(result);
                        List<SchemaModel> listParam = binIn.Deserialize(streamIn) as List<SchemaModel>;
                        streamIn.Close();
                        m_schemas = new List<SchemaModel>(listParam);
                    }catch ( Exception e)
                    {
                        
                    }
                    ((IClientChannel)wcfClient).Close();
                }
                return m_schemas;
            }
            set
            {
                m_schemas = value;
            }
        }

        public IChildAccessService<BadgeModel> BadgeAccessService
        {
            get;
        }

        private ReadOnlyCollection<ObjectType> m_ObjectsTypes;
        public ReadOnlyCollection<ObjectType> ObjectTypes
        {
            get
            {
                if (m_ObjectsTypes == null)
                {
                    var wcfClient = m_chanelFactory.CreateChannel();
                    try
                    {

                        byte[] result = wcfClient.GetObjectTypes() as byte[];

                        BinaryFormatter binIn = new BinaryFormatter();
                        MemoryStream streamIn = new MemoryStream(result);

                        List<ObjectType> listParam = binIn.Deserialize(streamIn) as List<ObjectType>;
                        streamIn.Close();

                        m_ObjectsTypes = new ReadOnlyCollection<ObjectType>(listParam);
                    } catch (Exception e)
                    {
                        
                    }
                    ((IClientChannel)wcfClient).Close();
                }
                return m_ObjectsTypes;
            }
        }

        private ReadOnlyCollection<BadgeGeometryModel> m_BadgeGeometries;

        public ReadOnlyCollection<BadgeGeometryModel> BadgeGeometries
        {
            get
            {
                
                if (m_BadgeGeometries == null)
                {
                    var wcfClient = m_chanelFactory.CreateChannel();
                    try
                    {
                        byte[] result = wcfClient.GetBadgeGeometries() as byte[];

                        
                        BinaryFormatter binIn = new BinaryFormatter();
                        MemoryStream streamIn = new MemoryStream(result);

                        List<BadgeGeometryModel> listParams = binIn.Deserialize(streamIn) as List<BadgeGeometryModel>;
                        streamIn.Close();

                        
                        m_BadgeGeometries = new ReadOnlyCollection<BadgeGeometryModel>(listParams);
                    }
                    catch(Exception e)
                    {
                        
                    }
                    ((IClientChannel)wcfClient).Close();
                }

                return m_BadgeGeometries;
            }
        }

        public IBoilerInfoAccessService BoilerInfoAccessService
        {
            get;
        }

        public ICustomLayerAccessService CustomLayerAccessService
        {
            get;
        }

        public ICustomObjectAccessService CustomObjectAccessService
        {
            get;
        }

        public IDocumentAccessService DocumentAccessService
        {
            get;
        }

        public string ErrorFolderName
        {
            get;
        }

        public IFigureAccessService FigureAccessService
        {
            get;
        }

        public IFuelAccessService FuelAccessService
        {
            get;
        }

        public IGlobalMapAccessService GlobalMapAccessService
        {
            get;
        }

        public IKtsAccessService KtsAccessService
        {
            get;
        }

        public ILabelAccessService LabelAccessService
        {
            get;
        }

        public ILineAccessService LineAccessService
        {
            get;
            private set;
        }

        public int LoggedUserId
        {
            get;set;
        }

        public ILoginAccessService LoginAccessService
        {
            get;
        }

        public IMapAccessService MapAccessService
        {
            get;
        }

        public IMeterAccessService MeterAccessService
        {
            get;
        }

        public INodeAccessService NodeAccessService
        {
            get;
        }

        public IChildAccessService<NonVisualObjectModel> NonVisualObjectAccessService
        {
            get;
        }

        

        public IParameterAccessService ParameterAccessService
        {
            get;
        }

        public IReportAccessService ReportAccessService
        {
            get;
        }

        

        public ISearchService SearchService
        {
            get;
        }

        public string ServerName
        {
            get;
        }

        public string SubstrateFolderName
        {
            get;
        }

        public ITerritorialEntityAccessService TerritorialEntityAccessService
        {
            get;
        }

        public string ThumbnailFolderName
        {
            get;
        }
        public string clientUri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IBuilderInfoAccessService BuilderInfoAccessService => throw new NotImplementedException();

        private object callDispathServerMethod(string methodCaller, params object[] listParams)
        {
            var wcfClient = m_chanelFactory.CreateChannel();
            object resObject = null;
            try
            {

                var getlogins = wcfClient.GetLogins();
                BinaryFormatter binAry = new BinaryFormatter();
                MemoryStream memStreamIn = new MemoryStream();
                binAry.Serialize(memStreamIn, listParams);

                memStreamIn.Close();

                
                Object result = wcfClient.DataServiceDispathMethod(methodCaller, memStreamIn.ToArray());
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
        }

        public void BeginSaveTransaction()
        {
            //throw new NotImplementedException();
            callDispathServerMethod(MethodBase.GetCurrentMethod().Name);
        }

        public bool CanConnect()
        {
            return true;
        }

        public bool EndSaveTransaction()
        {
            return (bool)callDispathServerMethod(MethodBase.GetCurrentMethod().Name);
        }

        public DataTable GetLogs()
        {
            return callDispathServerMethod(MethodBase.GetCurrentMethod().Name) as DataTable;
        }

        public ObjectType GetObjectType(int typeId)
        {
            return callDispathServerMethod(MethodBase.GetCurrentMethod().Name, typeId) as ObjectType;
        }

        public TableModel getTableMeterParamsForRegion(int regionID)
        {
            return callDispathServerMethod(MethodBase.GetCurrentMethod().Name, regionID) as TableModel;
        }

        public bool IsBoilerType(ObjectType type)
        {
            return type.TypeId == 1;
        }

        public bool IsCompatible(string version)
        {
            WcfDispatchServer.Log("version = " + version);
            return true;
            //return (bool)callDispathServerMethod(MethodBase.GetCurrentMethod().Name, version);
        }

        public bool IsStorageType(ObjectType type)
        {
            return type.TypeId == 19;
        }

        public bool IsTrashStorageType(ObjectType type)
        {
            return type.TypeId == 24;
        }

        public async Task LoadDataAsync(int userID = 0)
        {
            await Task.Factory.StartNew(() =>
            {
                //this.LineAccessService = new SqlLineAccessService(this, this.connector, this.LoggedUserId);    
                //this.LineAccessService = new WcfLineAccessService(m_chanelFactory);   
            });
        }


        public void PrepareSchema(int year, int cityId)
        {
            callDispathServerMethod(MethodBase.GetCurrentMethod().Name, year, cityId);
        }

        public void UpdateLogs(DataTable dataTable)
        {
            callDispathServerMethod(MethodBase.GetCurrentMethod().Name, dataTable);
        }

        public void UpdateTables(int cityID)
        {
            callDispathServerMethod(MethodBase.GetCurrentMethod().Name, cityID);
        }

        public void UpdateTables(int cityId, ObjectType type, SchemaModel schema, LoadLevel loadLevel)
        {
            callDispathServerMethod(MethodBase.GetCurrentMethod().Name, cityId, type, schema, loadLevel);
        }

        public void UpdateTypeCaptionParams(int cityId)
        {
            callDispathServerMethod(MethodBase.GetCurrentMethod().Name, cityId);
        }

        public List<ObjectType> GetFillTypes()
        {
            List<ObjectType> result = callDispathServerMethod(MethodBase.GetCurrentMethod().Name) as List<ObjectType>;
            m_ObjectsTypes = new ReadOnlyCollection<ObjectType>(result);
            return result;
        }


        public List<TableModel> GetFillTables()
        {

            return null;
        }


        public List<ParameterModel> GetFillParameters()
        {
            return null;
        }


        public List<BadgeGeometryModel> GetFillBadgeGeometries()
        {

            m_BadgeGeometries = new ReadOnlyCollection<BadgeGeometryModel>(callDispathServerMethod(MethodBase.GetCurrentMethod().Name) as List<BadgeGeometryModel>);
            return BadgeGeometries.ToList<BadgeGeometryModel>();
        }


        public List<SchemaModel> GetFillSchemas()
        {
            Schemas = callDispathServerMethod(MethodBase.GetCurrentMethod().Name) as List<SchemaModel>;
            return Schemas;
        }



        public ILoginAccessService getLoginAccessService()
        {
            return LoginAccessService;
        }
    }
}
