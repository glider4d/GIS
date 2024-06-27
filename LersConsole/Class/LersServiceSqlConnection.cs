using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LersConsole.Class
{
    public class LersServiceSqlConnection : BaseSqlDataAccessService
    {
        public LersServiceSqlConnection(SqlConnector connector) : base(connector)
        {
        }


        
        public List<int> getIdParameters()
        {
            List<int> result = new List<int>();
            var query = @"SELECT id_parameter FROM [Gis].[dbo].[boiler_meter_parameters]";
            try
            {
                using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                
                                IDataRecord test = reader;
                                int id_parameter = Convert.ToInt32(reader["id_parameter"]);
                                result.Add(id_parameter);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error getIdParameters: " + e.Message);
            }
            return result;
        }

        public Dictionary<int, DateTime> getMaxDateBoilerMeters()
        {
            Dictionary<int, DateTime> result = new Dictionary<int, DateTime>();
            var query = @"select [id_meter], MAX([Gis].[dbo].[boiler_meters].dateTime) as maxDate from [Gis].[dbo].[boiler_meters] group by id_meter";
            

            try
            {
                using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                command.CommandTimeout = 0;
                                int id_meters = Convert.ToInt32(reader["id_meter"]);
                                DateTime maxDateTime = Convert.ToDateTime(reader["maxDate"]);
                                result.Add(id_meters, maxDateTime);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error getMaxDateBoilerMeters: "+e.Message);
            }
            finally
            {
                
            }
            return result;
        }

        public void insertBoilerMeters(Dictionary<int, Dictionary<DateTime, List<double>>> data, List<int> parameters_id)
        {
            var query = @"insert into [Gis].[dbo].[boiler_meters] (id_meter, dateTime, id_parameter, value) values(@id_meter, @dateTime, @id_parameter, @value)";
            try
            {
                
                using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                {

                    using (SqlTransaction trans = Connector.GetCon().BeginTransaction())
                    {
                        command.Transaction = trans;
                        command.Parameters.Add("@id_meter", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@dateTime", System.Data.SqlDbType.Date);
                        command.Parameters.Add("@id_parameter", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@value", System.Data.SqlDbType.Float);
                        
                        foreach (var itemOfData in data)
                        {
                            int numberPoint = itemOfData.Key;
                            foreach (var itemDateValues in itemOfData.Value)
                            {
                                DateTime dateTime = itemDateValues.Key;
                                List<double> paramValues = itemDateValues.Value;
                                if (paramValues == null || dateTime == null)
                                    continue;
                                if (paramValues.Count == parameters_id.Count)
                                {
                                    for (int i = 0; i < parameters_id.Count; i++)
                                    {

                                        int id_parameter = parameters_id[i];
                                        double value = paramValues[i];
                                        //using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                                        {
                                            
                                            command.Parameters["@id_meter"].Value = numberPoint;
                                            command.Parameters["@dateTime"].Value = dateTime;
                                            command.Parameters["@id_parameter"].Value = id_parameter;
                                            command.Parameters["@value"].Value = value;

                                            
                                            /*
                                            command.Parameters.AddWithValue("@id_meter", numberPoint);
                                            command.Parameters.AddWithValue("@dateTime", dateTime);
                                            command.Parameters.AddWithValue("@id_parameter", id_parameter);
                                            command.Parameters.AddWithValue("@value", value);

    */
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                        trans.Commit();
                    }
                }
            } catch(Exception exp)
            {
                Console.WriteLine("message exception: " + exp.Message);
            }
            return;
        }
        public void insertBoilerMeterCaotion(Dictionary<int,MeterData> data)
        {
            var query = @"insert into [General].[dbo].[boiler_meter_caption] (id, name, fullTitle, address, id_region) values(@idmeter, @caption, @fullTitle, @address, @id_region)";

            try
            {
                foreach (var item in data)
                {
                    using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                    {
                        command.Parameters.AddWithValue("@idmeter", item.Key);
                        command.Parameters.AddWithValue("@caption", item.Value.title);
                        command.Parameters.AddWithValue("@fullTitle", item.Value.fullTitle);
                        command.Parameters.AddWithValue("@address", item.Value.address);
                        command.Parameters.AddWithValue("id_region", 0);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error : " + e.Message);
            }
        }
        public List<int> getMeasurePoints()
        {
            List<int> result = new List<int>();
            var query = @"SELECT [id] FROM [General].[dbo].[boiler_meter_caption]";
            try
            {
                using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id_meter = Convert.ToInt32(reader["id"]);
                                result.Add(id_meter);
                            }
                        }
                    }
                }
            }
            catch

            {

            }
            return result;
        }
        public int testConnection()
        {
            int result = 0;
            var query = @"select * from Gis..logged_history where date_ex is null";
            try
            {
                using (var command = new SqlCommand(query, Connector.GetConnectionConsole()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["id"]);
                                string version = Convert.ToString(reader["version"]);
                                string ip = Convert.ToString(reader["ip"]);
                                //DateTime date_in = Convert.ToDateTime(reader["date_in"]);
                                string date_in = "NULL";
                                if (!reader["date_in"].Equals(System.DBNull.Value))
                                    date_in = Convert.ToString(reader["date_in"]);

                                string date_ex = "NULL";
                                if (!reader["date_ex"].Equals(System.DBNull.Value))
                                    date_ex = Convert.ToString(reader["date_ex"]);
                                //sessionModels.Add(new SessionModel(id, ip, version, date_in, date_ex));
                                //Console.WriteLine("in " + date_in + " ex " + date_ex);
                                result++;
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
    }
}
