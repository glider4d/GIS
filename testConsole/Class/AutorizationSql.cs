using Kts.Gis.Data;
using Kts.Gis.ViewModels;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsole.Class
{
    public class AutorizationSql
    {



        public ServiceSqlConnection getSqlAccessService()
        {
            return m_dataService;
        }


        private ServiceSqlConnection m_dataService;
        public void ReadAutorizationFile()
        {
            string[] dataContent = new string[0];

            var forcedDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Kts\\Gis\\ForcedServerData";
            //var dataPath = "\\\\172.16.4.58\\Gis\\ServerData";
            var dataPath = "C:\\2\\ServerData";
            // Проверяем наличие файла, с вшитыми данными серверов.
            try
            {
                if (File.Exists(forcedDataPath))
                    dataContent = File.ReadAllLines(forcedDataPath);
                else
                    dataContent = File.ReadAllLines(dataPath);
                dataContent = File.ReadAllLines(dataPath);
            }
            catch(Exception exp)
            {
                string error = exp.Message;
                System.Console.WriteLine(error);
            }

            // Разбираем данные серверов.
            //var serverData = new Dictionary<SqlConnectionString, string>();
            string dbAddress;
            string userId;
            string password;
            string name;
            string fileAddress;
            SqlConnectionString connectionString = null;
            for (int i = 0; i < dataContent.Length; i += 5)
            {
                dbAddress = dataContent[i];
                userId = dataContent[i + 1];
                password = dataContent[i + 2];
                name = dataContent[i + 3];
                fileAddress = dataContent[i + 4];
                //Console.WriteLine("dbAddress = " + dbAddress + "; name = "+name);

                if (name.Equals("Якутск"))
                {
                    connectionString = new SqlConnectionString(dbAddress, userId, password, "w2AqAvKcgOh5iQuIodC6rw==", 90, name, true);
                    //connectionString = new SqlConnectionString("", "", "", "", 60, "", true);
                    Console.WriteLine(name + " " + dbAddress + " " + fileAddress);
                    //m_serverData.Add(connectionString, fileAddress);
                }
            }
            /*
             */

            if (connectionString != null)
            {

                SqlConnector tmpConnectior = new SqlReconnector(connectionString);


                //this.DataService = new SqlDataService(tmpConnector, @"\\" + fileStorage + "\\Gis\\Errors\\", @"\\" + fileStorage + "\\Gis\\Images\\", @"\\" + fileStorage + "\\Gis\\Thumbnails\\");
                IDataService DataService = new SqlDataService(tmpConnectior, "", "", "");
                if (!DataService.CanConnect())
                {
                    Console.WriteLine("data service can't connect");
                }
                else
                {
                    m_dataService = new ServiceSqlConnection(tmpConnectior);
                    int res = m_dataService.testConnection();

                    Console.WriteLine("res = " + res);
                }
            }

        }
         
    }
}
