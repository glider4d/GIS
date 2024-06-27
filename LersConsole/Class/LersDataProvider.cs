using Lers.Data;
using Lers.Networking;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace LersConsole.Class
{
    public class LersDataProvider
    {
        //Администратор
        //Mnbvcxz159
        private string login = "program";
        
        private SecureString password = Lers.Networking.SecureStringHelper.ConvertToSecureString("1596321");
        private string host_addr = "172.16.1.33";
        RestoreSessionToken m_connectedToken;
        Lers.LersServer m_server;
        public LersDataProvider()
        {
            m_server = new Lers.LersServer();
        }

        

        private bool connectionHost()
        {
            bool result = false;
            try
            {
                Lers.Networking.BasicAuthenticationInfo authInfo = new Lers.Networking.BasicAuthenticationInfo(login, password);

                m_connectedToken = m_server.Connect("172.16.1.33", 10000, authInfo);
                if (m_connectedToken != null)
                    result = true;
            } catch(Exception exc)
            {
                result = false;
                Console.WriteLine("Ошибка подключения к серверу.\r\n" + exc.Message);
            }
            return result;
        }

        private void closeConnection()
        {
            try
            {
                m_server.Disconnect(0);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Ошибка дисконнекта. \r\n");
            }
        }

        public Dictionary<int, MeasurePointConsumptionRecordCollection> getAllDictData(Dictionary<int,DateTime> idDateDict)
        {
            Dictionary<int, MeasurePointConsumptionRecordCollection> dictData
                = new Dictionary<int, MeasurePointConsumptionRecordCollection>();

            if (connectionHost())
            {
                //dictData = getAllDataDictServer(DateTime.Today.AddMonths(-3), DateTime.Today);
                //dictData = getAllDataDictServer(DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(-2));
                dictData = getAllDataDictServer(idDateDict);
                closeConnection();
            }
            return dictData;
        }
        public Dictionary<int, MeasurePointConsumptionRecordCollection> getAllDictData()
        {
            Dictionary<int, MeasurePointConsumptionRecordCollection> dictData 
                = new Dictionary<int, MeasurePointConsumptionRecordCollection>();

            if (connectionHost())
            {
                //dictData = getAllDataDictServer(DateTime.Today.AddMonths(-3), DateTime.Today);
                dictData = getAllDataDictServer(DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(-2));
                closeConnection();
            }
            return dictData;
        }

        public List<MeasurePointConsumptionRecordCollection> getAllData()
        {
            List<MeasurePointConsumptionRecordCollection> listData = new List<MeasurePointConsumptionRecordCollection>();
            if (connectionHost())
            {
                listData = getAllDataServer(DateTime.Today.AddMonths(-1), DateTime.Today);
                closeConnection();
            }
            return listData;
        }

        public MeasurePointConsumptionRecordCollection getData(int numberOfMeasurePoint)
        {
            MeasurePointConsumptionRecordCollection consumption = null;
            if (connectionHost())
            {
                consumption = getDataServer(DateTime.Today.AddMonths(-1), DateTime.Today, numberOfMeasurePoint);
                closeConnection();
            }
            return consumption;
        }

        public List<int> getNumberArrayMeasurePoints()
        {
            List<int> result = new List<int>();

            try
            {

                if (connectionHost())
                {
                    Lers.Core.MeasurePoint[] arrayPoints = m_server.MeasurePoints.GetList();
                    foreach (var item in arrayPoints)
                        result.Add((int)item.Number);
                }
            }
            catch
            {

            }
            finally
            {
                closeConnection();
            }
            return result;
        }

        private Dictionary<int, MeasurePointConsumptionRecordCollection> getAllDataDictServer(Dictionary<int, DateTime> idDateDict)
        {
            Dictionary<int, MeasurePointConsumptionRecordCollection> dictData
                = new Dictionary<int, MeasurePointConsumptionRecordCollection>();
            try
            {
                Lers.Core.MeasurePoint[] arrayPoints = m_server.MeasurePoints.GetList();
                foreach (var measurePoint in arrayPoints)
                {
                    DateTime endInterval = DateTime.Today;
                    DateTime startInterval = idDateDict.ContainsKey((int)measurePoint.Number) ? idDateDict[(int)measurePoint.Number] : DateTime.Today.AddMonths(-1);
                    if (startInterval == null)
                        startInterval = DateTime.Today.AddMonths(-1);
                    if (startInterval < endInterval)
                    {
                        //dictData.Add(measurePoint.Number, getDataServer(startInterval, endInterval, measurePoint.Number));
                        MeasurePointConsumptionRecordCollection data = getDataServer(startInterval, endInterval, (int)measurePoint.Number);
                        if (data.Count > 0)
                            dictData.Add((int)measurePoint.Number, data);
                    }

                }
            }
            catch(Exception exc)
            {
                Console.WriteLine("Ошибка 2 получения ассоциированных данных с сервера.\r\n" + exc.Message);
            }
            return dictData;
        }
        private Dictionary<int, MeasurePointConsumptionRecordCollection> getAllDataDictServer(DateTime startInterval, DateTime endInterval)
        {
            Dictionary<int, MeasurePointConsumptionRecordCollection> dictData 
                = new Dictionary<int, MeasurePointConsumptionRecordCollection>();
            try
            {
                Lers.Core.MeasurePoint[] arrayPoints = m_server.MeasurePoints.GetList();
                foreach (var measurePoint in arrayPoints)
                {
                    dictData.Add((int)measurePoint.Number, getDataServer(startInterval, endInterval, (int)measurePoint.Number));
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine("Ошибка получения ассоциированных данных с сервера.\r\n" + exc.Message);
            }
            return dictData;
        }
        private List<MeasurePointConsumptionRecordCollection> getAllDataServer(DateTime startInterval, DateTime endInterval)
        {
            List<MeasurePointConsumptionRecordCollection> listData = null;
            try
            {
                listData = new List<MeasurePointConsumptionRecordCollection>();
                Lers.Core.MeasurePoint[] arrayPoints = m_server.MeasurePoints.GetList();
                foreach (var measurePoint in arrayPoints)
                {
                    listData.Add(getDataServer(startInterval, endInterval, (int)measurePoint.Number));
                    /*
                    if (measurePoint != null)
                    {
                        Console.WriteLine("Точка учета \'{0}\'", measurePoint.FullTitle);
                        Console.WriteLine("measurePoint.SystemType = " + measurePoint.SystemType);

                        MeasurePointConsumptionRecordCollection consumption = measurePoint.Data.GetConsumption(DateTime.Today.AddMonths(-1), DateTime.Today, Lers.Data.DeviceDataType.Day);

                    }*/
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Ошибка получения данных с сервера.\r\n" + exc.Message);
            }
            return listData;
        }

        private Lers.Core.MeasurePoint[] boilerMettersCaption()
        {
            Lers.Core.MeasurePoint[] result = null;
            try
            {
                if (connectionHost())
                {
                    result = m_server.MeasurePoints.GetList();
                    closeConnection();
                }
            }
            catch
            {

            }
            finally
            {

            }
            return result;
        }




        public Dictionary<int, Dictionary<DateTime, List<double>>> getValueByParams(List<int> numberOfParams, Dictionary<int, DateTime> maxDatesDic)
        {
            Dictionary<int, Dictionary<DateTime, List<double>>> result2 = new Dictionary<int, Dictionary<DateTime, List<double>>>();
            try
            {

                //List<MeasurePointConsumptionRecordCollection> consumptionRecordList = getAllData();//getAllDataServer(DateTime.Today.AddMonths(-1), DateTime.Today);
                Dictionary<int, MeasurePointConsumptionRecordCollection> consumptionRecordDict = getAllDictData(maxDatesDic);
                foreach (var consumption in consumptionRecordDict)
                {
                    //Console.Write("number = " + consumption.Key);
                    Dictionary<DateTime, List<double>> dateValueParams = new Dictionary<DateTime, List<double>>();
                    foreach (var consumptionRecord in consumption.Value)
                    {
                        List<double> valueOfParams = new List<double>();

                        //result.Add(consumptionRecord.)
                        Console.WriteLine(consumptionRecord.DateTime.ToShortDateString());
                        foreach (int param in numberOfParams)
                        {
                            double? value = consumptionRecord.GetValue((Lers.Data.DataParameter)param);

                            valueOfParams.Add(value == null ? 0 : (double)value);

                            //Console.Write(" " + param + ":" + (value==null?0:value));
                        }
                        dateValueParams.Add(consumptionRecord.DateTime, valueOfParams);

                        //result.Add(consumption.Key, valueOfParams);
                    }
                    result2.Add(consumption.Key, dateValueParams);
                    //Console.WriteLine("");
                }

            }
            catch (Exception exp)
            {
                Console.WriteLine("getValueByParams eror message: " + exp.Message);
            }
            return result2;
        }

        public //Dictionary<int, List<double>> 
            Dictionary<int, Dictionary<DateTime, List<double>>>  getValueByParams(List<int> numberOfParams)
        {
            //Dictionary<int, List<double>> result = new Dictionary<int, List<double>>();
            Dictionary<int, Dictionary<DateTime, List<double>>> result2 = new Dictionary<int, Dictionary<DateTime, List<double>>>();
            try
            {

                //List<MeasurePointConsumptionRecordCollection> consumptionRecordList = getAllData();//getAllDataServer(DateTime.Today.AddMonths(-1), DateTime.Today);
                Dictionary<int,MeasurePointConsumptionRecordCollection> consumptionRecordDict = getAllDictData();
                foreach (var consumption in consumptionRecordDict)
                {
                    //Console.Write("number = " + consumption.Key);
                    Dictionary<DateTime, List<double>> dateValueParams = new Dictionary<DateTime, List<double>>();
                    foreach (var consumptionRecord in consumption.Value)
                    {
                        List<double> valueOfParams = new List<double>();
                        
                        //result.Add(consumptionRecord.)
                        Console.WriteLine(consumptionRecord.DateTime.ToShortDateString());
                        foreach (int param in numberOfParams)
                        {
                            double? value = consumptionRecord.GetValue((Lers.Data.DataParameter)param);
                            
                            valueOfParams.Add(value == null ? 0 : (double)value);

                            //Console.Write(" " + param + ":" + (value==null?0:value));
                        }
                        dateValueParams.Add(consumptionRecord.DateTime, valueOfParams);
                        
                        //result.Add(consumption.Key, valueOfParams);
                    }
                    result2.Add(consumption.Key, dateValueParams);
                    //Console.WriteLine("");
                }

            }
            catch(Exception exp)
            {
                Console.WriteLine("getValueByParams eror message: "+ exp.Message);
            }
            return result2;
        }
        public Dictionary<int, MeterData> getBoilerMetersCaption(List<int> meterNumbersExist)
        {
            Dictionary<int, MeterData> result = new Dictionary<int, MeterData>();
            try
            {
                
                Lers.Core.MeasurePoint[] arrayMettersCaption = boilerMettersCaption();
                foreach (var measurePoint in arrayMettersCaption)
                {
                    
                    //meterNumbersExist.Find(p => p == 5);
                    //int findResult = meterNumbersExist.Find(p => p == measurePoint.Number);
                    if (!meterNumbersExist.Contains((int)measurePoint.Number))
                    {
                        /*
                        Console.WriteLine("title : " + measurePoint.Title);
                        Console.WriteLine("address : "+measurePoint.Address);
                        Console.WriteLine("title: " + measurePoint.FullTitle + " number: " + measurePoint.Number);*/
                        //result.Add(measurePoint.Number, measurePoint.FullTitle);
                        result.Add((int)measurePoint.Number, new MeterData(measurePoint.FullTitle, measurePoint.Title, measurePoint.Address));
                    }
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine("Ошибка получения данных с сервера.\r\n" + exc.Message);
            }
            finally
            {

            }
            return result;
        }
        private MeasurePointConsumptionRecordCollection getDataServer(DateTime startInterval, DateTime endInterval, int measurePointNumber, Lers.Data.DeviceDataType dataType = Lers.Data.DeviceDataType.Day)
        {
            MeasurePointConsumptionRecordCollection consumption = null;
            try
            {
                Lers.Core.MeasurePoint measurePoint = 
                    m_server.MeasurePoints.GetByNumber(measurePointNumber);
                if (measurePoint != null)
                {
                    
                    consumption = measurePoint.Data.GetConsumption(startInterval, endInterval, dataType);
                    foreach (var consumptionRecord in consumption)
                    {

                    }

                }
            } catch(Exception exc)
            {
                Console.WriteLine("Ошибка получения данных с сервера.\r\n" + exc.Message);
            }
            return consumption;
        }
    }
}
