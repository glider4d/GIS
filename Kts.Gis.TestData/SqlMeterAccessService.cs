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
        public List<MeterInfoModel> GetMeterInfo(int boilerId, DateTime fromDate, DateTime toDate)
        {
            List<MeterInfoModel> result = new List<MeterInfoModel>();
            var query = @"SELECT [id_meter]
                                    ,[dateTime]
                                    ,[id_parameter]
                                    ,[value]
                                FROM [Gis].[dbo].[boiler_meters] 
                                    where id_meter = @id_meter and 
                                     (id_parameter = 1 or id_parameter = 2 or id_parameter = 47) 
                                    order by dateTime desc";

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
