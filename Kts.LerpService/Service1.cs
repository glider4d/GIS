using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using LersConsole.Class;


namespace Kts.LerpService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;
        //DateTime m_lastRun;
        public Service1()
        {
            
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 1000 * 60 *60 * 24;//5000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {

                AutorizationSql sqlAutorization = new AutorizationSql();
                sqlAutorization.ReadAutorizationFile();
                LersServiceSqlConnection lersSqlService = sqlAutorization.getSqlAccessService();
                List<int> measurePointNumbers = lersSqlService.getMeasurePoints();


                LersDataProvider lersDataProvider = new LersDataProvider();
                Dictionary<int, MeterData> bolierMetersCaptionData = lersDataProvider.getBoilerMetersCaption(measurePointNumbers);
                if (bolierMetersCaptionData != null)
                    lersSqlService.insertBoilerMeterCaotion(bolierMetersCaptionData);

                List<int> parameters = lersSqlService.getIdParameters();

                Console.WriteLine(parameters.Count);
                foreach (var param in parameters)
                {
                    //Console.Write(" " + param);
                    WriteToFile(" " + param);
                }
                Dictionary<int, DateTime> maxDatesDict = lersSqlService.getMaxDateBoilerMeters();
                //Dictionary<int, List<double>> valueByParams = 
                Dictionary<int, Dictionary<DateTime, List<double>>> valueByParams = lersDataProvider.getValueByParams(parameters, maxDatesDict);
                //lersDataProvider.getValueByParams(parameters);

                lersSqlService.insertBoilerMeters(valueByParams, parameters);
            }
            catch (Exception exp)
            {
                
                WriteToFile("DateTime.Now: "+ "Ошибка службы измерительных приборов: " + exp.Message);
            }
            

            
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
