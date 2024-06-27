using Kts.Gis.Data.Interfaces;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    public class SqlMeterAccessService : BaseSqlDataAccessService, IMeterAccessService
    {
        public SqlMeterAccessService(SqlConnector connector) : base(connector)
        {
        }

        public bool CanAccessMeterInfo()
        {
            return true;
        }

        public async Task<int> getRegionID(int cityID)
        {
            int result = 0;
            var query = @"SELECT * FROM [General].[dbo].[vCities] where id = @cityID";
            try
            {
                using (var command = new SqlCommand(query, Connector.GetConnection()))
                {
                    command.Parameters.AddWithValue("@cityID", cityID);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = Convert.ToInt32(reader["id_region"]);
                            }
                        }
                    }
                }
            } catch (Exception e)
            {
                throw new Exception(query, e);
            }
            return result;
        }

        public List<BoilerMeterReportModel> GetBoilerMeterReportModels(bool notNull)
        {
            List<BoilerMeterReportModel> result = new List<BoilerMeterReportModel>();
            /*
            var query = @"select id_object,name, fullTitle, [address], boiler_name, cities_name from General..boiler_meter_caption,
	(
		select id_object, vBoilers.[name] as boiler_name, id_param, [value], id_city, Cities.[name] as cities_name from General..tValues, General..vBoilers, General..Cities
				where tValues.id_object = vBoilers.id and id_param = 1005 and tValues.date_po is null and vBoilers.id_city = Cities.id) blr
	where blr.value = boiler_meter_caption.id order by boiler_meter_caption.id_region";*/


            
            var query = @"select id_object, boiler_name, _all.name BoilerName, Regions.Name RegionName  from 

(
select id_object,  name, fullTitle, [address], boiler_name, boiler_meter_caption.id_region from General..boiler_meter_caption left join
	(
		select  id_object, vBoilers.[name] as boiler_name, id_param, [value], id_city  from General..tValues, General..vBoilers
				where tValues.id_object = vBoilers.id and id_param = 1005 and tValues.date_po is null ) blr
	on blr.value = boiler_meter_caption.id  ) _all , General..Regions where _all.id_region = Regions.id  ";

            try
            {

                using (var command = new SqlCommand(query, Connector.GetConnection()))
                {
                    command.CommandText = query;
                    //var columns = new List<string>();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                string boiler_name = reader["boiler_name"] == DBNull.Value ? "" : Convert.ToString(reader["boiler_name"]);
                                string BoilerName = reader["BoilerName"] == DBNull.Value ? "" : Convert.ToString(reader["BoilerName"]);
                                string regionName = reader["RegionName"] == DBNull.Value ? "" : Convert.ToString(reader["RegionName"]);

                                if (notNull && ( boiler_name.Length == 0 || BoilerName.Length == 0))
                                    continue;


                                result.Add(new BoilerMeterReportModel(Guid.Empty,//Guid.Parse(Convert.ToString(reader["id_object"])), 
                                    boiler_name,
                                    BoilerName,
                                    regionName)
                                    /*Convert.ToString(reader["BoilerName"]), Convert.ToString(reader["RegionName"])*/);
                                /*
                                result.Add(new ParametersTypeModel(Convert.ToString(reader["name"]),
                                Convert.ToString(reader["paramName"]),
                                Convert.ToString(reader["format"]),
                                Convert.ToInt32(reader["id_order"]),
                                Convert.ToInt32(reader["id_param"])));*/
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }


            return result; 
        }

        public List<MeterInfoModel> GetMeterInfo(int boilerId, DateTime fromDate, DateTime toDate)
        {

            //1, 2, 47 : T_in, T_out, T_Delta;  5,6,50 : M_in, M_out, M_Delta;7,8,49 V_in, V_Out, V_Delta; 9,10,11: Q_in, Q_out, Q_Delta;
            List<MeterInfoModel> result = new List<MeterInfoModel>();
            var query = @"SELECT [id_meter]
                                    ,[dateTime]
                                    ,[id_parameter]
                                    ,[value]
                                FROM [Gis].[dbo].[boiler_meters] 
                                    where id_meter = @id_meter 
                                    order by dateTime desc";
                                    //and (id_parameter = 1 or id_parameter = 2 or id_parameter = 47) 
                                    

            try
            {
                MeterInfoModel tmpMeter = new MeterInfoModel(0,0,0, DateTime.Now);
                using (var command = new SqlCommand(query, Connector.GetConnection()))
                {
                    command.Parameters.AddWithValue("@id_meter", boilerId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int i = 0;
                            while (reader.Read())
                            {
                                
                                //DateTime date_in = Convert.ToDateTime(reader["date_in"]);
                                DateTime date = Convert.ToDateTime(reader["dateTime"]);
                                int id_param = Convert.ToInt32(reader["id_parameter"]);
                                double value = Convert.ToDouble(reader["value"]);

                                if (tmpMeter.dateTime.Equals(DateTime.Now))
                                    tmpMeter.dateTime = date;


                                if (!tmpMeter.dateTime.Equals(date))
                                {
                                    if (i > 0)
                                        result.Add(tmpMeter);
                                    tmpMeter = new MeterInfoModel(0, 0, 0, date);
                                }

                                if (tmpMeter.dateTime.Equals(date))
                                {
                                    tmpMeter.setValue(id_param, value);
                                }

                                

                                if (result.Count > 7)
                                    break;
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }
    }
}
