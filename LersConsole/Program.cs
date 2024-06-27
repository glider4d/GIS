using LersConsole.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LersConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AutorizationSql sqlAutorization = new AutorizationSql();
            sqlAutorization.ReadAutorizationFile();
            LersServiceSqlConnection lersSqlService = sqlAutorization.getSqlAccessService();
            List<int> measurePointNumbers = lersSqlService.getMeasurePoints();


            LersDataProvider lersDataProvider = new LersDataProvider();
            Dictionary<int,MeterData> bolierMetersCaptionData = lersDataProvider.getBoilerMetersCaption(measurePointNumbers);


            
            if (bolierMetersCaptionData != null)
                lersSqlService.insertBoilerMeterCaotion(bolierMetersCaptionData);

            List<int> parameters = lersSqlService.getIdParameters();
            
            Console.WriteLine(parameters.Count);
            foreach (var param in parameters)
            {
                Console.Write(" " + param);
            }
            Dictionary<int,DateTime> maxDatesDict = lersSqlService.getMaxDateBoilerMeters();
            //Dictionary<int, List<double>> valueByParams = 
            Dictionary<int, Dictionary<DateTime, List<double>>> valueByParams = lersDataProvider.getValueByParams(parameters, maxDatesDict);
                //lersDataProvider.getValueByParams(parameters);

            lersSqlService.insertBoilerMeters(valueByParams, parameters);
            
//c:\Windows\Microsoft.Net\Framework\v4.0.30319\InstallUtil.exe E:\gis_copy\Kts\LersService\bin\Release\LersService.exe


            

            Console.ReadLine();
        }
    }
}
