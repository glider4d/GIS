using Kts.AdministrationTool.Models;
using Kts.AdministrationTool.ViewModels.Classes;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kts.AdministrationTool.Data
{
    public class SqlAdminAccessService : BaseSqlDataAccessService
    {
        public SqlAdminAccessService(SqlConnector connector) : base(connector)
        {
        }


        public bool closeSession(SessionModel sessionModel)
        {
            bool result = false;
            string sqlRequest = @"update Gis.dbo.logged_history set date_ex = GETDATE() 
                                    where id = @id";
            try
            {
                using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
                {
                    command.Parameters.AddWithValue("@id", sessionModel.id);
                    command.ExecuteNonQuery();
                    result = true;
                }
            } catch(Exception e)
            {
            }
            return result;
        }
        public List<SessionModel> getOpenSession()
        {
            List<SessionModel> sessionModels = new List<SessionModel>();
            var query = @"select * from Gis..logged_history where date_ex is null";
            try
            {
                using (var command = new SqlCommand(query, Connector.GetConnection()))
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
                                sessionModels.Add(new SessionModel(id, ip, version, date_in, date_ex));
                            }

                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
            return sessionModels;
        }


        public List<LoginModel> GetAll()
        {
            var query = "get_logins";

            var result = new List<LoginModel>();

            try
            {
                using (var connection = this.Connector.GetConnection())
                using (var command = new SqlCommand("get_logins", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Убираем ограничение по времени выполнения команды.
                    command.CommandTimeout = 0;

                    int id;
                    string name;

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                            name = Convert.ToString(reader["name"]);

                            result.Add(new LoginModel(id, name));
                        }
                }
            }
            catch (Exception e)
            {
                throw new Exception(query, e);
            }

            return result;
        }
        /*
         * string sqlRequest = "update logins set login_pass = @pass where id = @id";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@pass", login.Password != null ? login.Password : "");
                command.Parameters.AddWithValue("@id", login.Id);
                command.ExecuteNonQuery();
            }
         */

        public void updateParameterType(ParametersTypeModel paramTypeModel, int typeID)
        {
            string sqlRequest = @"update General.dbo.ParametersType set id_order = @order 
                                    where id_param = @idparam and id_type = @idtype";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@idparam", paramTypeModel.paramModel.Id);
                command.Parameters.AddWithValue("@idtype", typeID);
                command.Parameters.AddWithValue("@order", paramTypeModel.order);
                command.ExecuteNonQuery();
            }
        }

        public void insertParameterType(ParametersTypeModel paramTypeModel,int typeID)
        {

            var sqlRequest = @"insert into General.dbo.ParametersType (id_param, id_type, id_order)
                                                                    values (@idparam, @idtype, @idorder)";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@idparam", paramTypeModel.paramModel.Id);
                command.Parameters.AddWithValue("@idtype", typeID);
                command.Parameters.AddWithValue("@idorder", paramTypeModel.order);
                command.ExecuteNonQuery();
            }
        }

        public void insertNewLogin(LoginModel login)
        {
            string sqlRequest = "insert into logins (id, login_name, login_pass) values (@id, @nm,@pswd)";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@id", login.Id);
                command.Parameters.AddWithValue("@nm", login.Name);
                command.Parameters.AddWithValue("@pswd", login.Password);
                command.ExecuteNonQuery();
            }
        }

        public void insertNewLogin(int id , string name, string pswd, int acc_lvl, bool activity, string regions)
        {

 

            string sqlRequest = "insert into logins (id, login_name, login_pass, acc_lvl, activity, region) values (@id, @nm,@pswd,@acc_lvl, @activity, @region)";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nm", name);
                command.Parameters.AddWithValue("@pswd", pswd);
                command.Parameters.AddWithValue("@acc_lvl", acc_lvl);
                command.Parameters.AddWithValue("@activity", activity);
                command.Parameters.AddWithValue("@region", regions);
                /*
                command.Parameters.AddWithValue("@id", login.Id);
                command.Parameters.AddWithValue("@nm", login.Name);
                command.Parameters.AddWithValue("@pswd", login.Password);*/
                command.ExecuteNonQuery();
            }
        }

        public void updateUserPassword(LoginModel login)
        {
            string sqlRequest = "update logins set login_pass = @pass where id = @id";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@pass", login.Password != null ? login.Password : "");
                command.Parameters.AddWithValue("@id", login.Id);
                command.ExecuteNonQuery();
            }
        }

        public void updateLogin(LoginModel login)
        {
            string sqlRequest = "update logins set login_name = @lgn, sname= @snm, fname = @fnm, tname = @tnm, phone_num = @phn, acc_lvl= @aclvl, activity = @act, region = @rgn where id = @id";
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.Parameters.AddWithValue("@lgn", login.Name!=null? login.Name : "");
                command.Parameters.AddWithValue("@snm", login.sName != null ? login.sName : "");
                command.Parameters.AddWithValue("@fnm", login.fName != null ? login.fName : "");
                command.Parameters.AddWithValue("@tnm", login.tName != null ? login.tName : "");
                command.Parameters.AddWithValue("@phn", login.phoneNumber != null ? login.phoneNumber : "");
                command.Parameters.AddWithValue("@aclvl", login.accessLevel);
                command.Parameters.AddWithValue("@act", login.activity);
                command.Parameters.AddWithValue("@rgn", login.region != null ? login.region : "");
                command.Parameters.AddWithValue("@id", login.Id);
                command.ExecuteNonQuery();
            }
            //command.Text = "UPDATE Student SET Address = @add, City = @cit Where FirstName = @fn and LastName = @add";
        }

        public void deleteSqlVitim()
        {
            string sqlRequest = "delete from gis.dbo.nodes where city_id in (select id from general..vcities where id_region = 27)";


            string[] sqlArray = new string[]
            {
                "select * from [10.0.8.72].general.dbo.tValues"
                /*
                "delete from gis.dbo.nodes where city_id in (select id from general..vcities where id_region = 27)",
                "delete from gis.dbo.lines where object_id in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",
                "delete from gis.dbo.figures where object_id in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",
                "delete from gis.dbo.labels where id in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",
                "delete from general.dbo.tValues where id_object in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",
                "delete from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27)",

                "insert general.dbo.tObjects select * from Import_fil.dbo.dawn_tObjects where id_city in (select id from general..vcities where id_region = 27) ",
                "insert general.dbo.tValues select * from Import_fil.dbo.dawn_tValues where id_object in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",

                "insert into gis.dbo.labels select *from Import_fil.dbo.dawn_labels where city_id in (select id from general..vcities where id_region = 27)",
                "insert gis.dbo.figures select *from Import_fil.dbo.dawn_figures where object_id in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",
                "insert gis.dbo.lines select *from Import_fil.dbo.dawn_lines where object_id in (select id from general.dbo.tobjects where id_city in (select id from general..vcities where id_region = 27))",
                "insert gis.dbo.nodes select *from Import_fil.dbo.dawn_nodes where city_id in (select id from general..vcities where id_region = 27)"*/
            };


            foreach (var item in sqlArray)
            {
                using (var command = new SqlCommand(item, Connector.GetConnection()))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void storeProcedureCreate()
        {
            string sqlRequest = @"CREATE PROCEDURE [dbo].[get_hydraulics_error_lines_is]
	@city_id INT
AS
BEGIN
	CREATE TABLE #temp
	(
		[object_id] UNIQUEIDENTIFIER,
		value INT
	)
	
	INSERT INTO #temp
	SELECT id_object,
		   CAST(value AS INT)
	FROM Gis.dbo.tValues_temp v
	WHERE v.id_param = 394
		  AND v.date_po IS NULL
		  AND CAST(v.value AS INT) = -1
		  
	CREATE TABLE #heat_lines
	(
		[object_id] UNIQUEIDENTIFIER
	)
	
	INSERT INTO #heat_lines
	SELECT id_object
	FROM Gis.dbo.tObjects_temp o
	WHERE [type] = 6
	      AND id_city = @city_id
	
	SELECT [object_id] id
	FROM #heat_lines
	WHERE [object_id] IN (SELECT [object_id]
						FROM #temp)
							
	DROP TABLE #heat_lines
	DROP TABLE #temp
END";

            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.ExecuteNonQuery();
            }

        }

        public void storedProcedureEdit()
        {
            string sqlRequest = @"ALTER PROCEDURE [dbo].[get_nodes_is]
	                                        @city_id INT
                                        AS
                                        SET ARITHABORT ON

                                        DECLARE @rate INT
                                        SELECT @rate = CASE
				                                           WHEN width > height THEN width
				                                           ELSE height
			                                           END
                                        FROM Gis.dbo.images
                                        WHERE city_id = @city_id

                                        SELECT id,
	                                           99 [type_id],
	                                           [left] * @rate [left],
	                                           [top] * @rate [top],
	                                           conn_obj_id,
	                                           vo.[type] conn_obj_type_id,
	                                           vo.planning is_conn_obj_planning,
	                                           ignore_stick,
	                                           conn_data
                                        FROM Gis.dbo.nodes_inv n
                                        LEFT JOIN General.dbo.vObjects_inv vo
                                        ON n.conn_obj_id = vo.[object_id]
                                           AND n.city_id = vo.id_city
                                        WHERE n.city_id = @city_id and ((n.conn_obj_id is not null and vo.[type] is not null)
                                        or (n.conn_obj_id is null))";/*
            string sqlRequest = @"ALTER PROCEDURE [dbo].[get_nodes_is]
	@city_id INT
AS
SET ARITHABORT ON

DECLARE @rate INT
SELECT @rate = CASE
				   WHEN width > height THEN width
				   ELSE height
			   END
FROM Gis.dbo.images
WHERE city_id = @city_id

SELECT id,
	   99 [type_id],
	   [left] * @rate [left],
	   [top] * @rate [top],
	   conn_obj_id,
	   vo.[type] conn_obj_type_id,
	   vo.planning is_conn_obj_planning,
	   ignore_stick,
	   conn_data
FROM Gis.dbo.nodes_inv n
LEFT JOIN General.dbo.vObjects_inv vo
ON n.conn_obj_id = vo.[object_id]
   AND n.city_id = vo.id_city
WHERE n.city_id = @city_id";*/
            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                command.ExecuteNonQuery();
            }
        }




        


        public List<LoginModel> getAllUsers()
        {
            List<LoginModel> result = new List<LoginModel>();

            string sqlRequest = @"select * from logins";

            using (var command = new SqlCommand(sqlRequest, Connector.GetConnection()))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string login = Convert.ToString(reader["login_name"]);
                            string sname = Convert.ToString(reader["sname"]);
                            string fname = Convert.ToString(reader["fname"]);
                            string tname = Convert.ToString(reader["tname"]);
                            string phone_num = Convert.ToString(reader["phone_num"]);
                            int acc_lvl = 0;
                            if (!(reader["acc_lvl"] is DBNull) ) 
                                acc_lvl = Convert.ToInt32(reader["acc_lvl"]);


                            bool activity = false;
                            if(!(reader["activity"] is DBNull) )
                                activity = Convert.ToBoolean(reader["activity"]);
                            string region = Convert.ToString(reader["region"]);


                            int id = Convert.ToInt32(reader["id"]);
                            result.Add(new LoginModel(id, login, fname, sname, tname, phone_num, acc_lvl, activity, region));
                        }
                    }
                }
                
            }

            return result;
        }

        public List<RegionModel> getRegions()
        {
            List<RegionModel> result = new List<RegionModel>();

            string sql = @"SELECT id, Name FROM General.dbo.Regions";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string name = Convert.ToString(reader["Name"]);
                            result.Add(new RegionModel(id, name));
                        }
                    }
                }
            }

            return result;
        }

        public Dictionary<int, string> getAccessLevels()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            string sql = @"select id, name from Gis.dbo.[login_access_level]";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string name = Convert.ToString(reader["name"]);

                            result.Add(id, name);
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<int, string> getDictTypes()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            string sql = @"select id, name from General.dbo.tTypes";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(Convert.ToInt32(reader["id"]), Convert.ToString(reader["name"]));
                        }
                    }
                    
                }
            }
            return result;
        }

        
        public ObservableDictionary<int, string> getDictObsTypes()
        {
            ObservableDictionary<int, string> result = new ObservableDictionary<int, string>();

            string sql = @"select id, name from General.dbo.tTypes";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            

                            result.Add(Convert.ToInt32(reader["id"]), Convert.ToString(reader["name"]));
                        }
                    }

                }
            }
            return result;
        }
        
        public List<ParametersTypeModel> getParametersType(int id_type)
        {
            List<ParametersTypeModel> result = new List<ParametersTypeModel>();
            var query = "select id_param, id_order from General.dbo.ParametersType where id_type = "+id_type;
            using (var command = new SqlCommand(query, Connector.GetConnection()))
            {
                command.CommandText = query;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id_param = Convert.ToInt32(reader["id_param"]);
                            int id_order = Convert.ToInt32(reader["id_order"]);
                            result.Add(new ParametersTypeModel(id_order,
                                id_type, id_param));

                        }
                    }
                }
            }
            return result;
        }

        public List<ParameterModel> getAllParameters()
        {
            List<ParameterModel> result = new List<ParameterModel>();
            string sql = "select * from General.dbo.tParameters";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new ParameterModel(Convert.ToInt32(reader["id"]), Convert.ToString(reader["name"]),
                                Convert.ToString(reader["format"]), Convert.ToString(reader["vtable"]), Convert.ToString(reader["unit"]),
                                Convert.ToString(reader["exact_format"])
                                ));
                        }
                    }
                }
            }

            return result;
        }

        public List<ParametersTypeModel> getAllTypes(int id, List<ParameterModel> paramList)
        {
            List<ParametersTypeModel> result = new List<ParametersTypeModel>();
            string sql = @"SELECT typ.id, id_param, typ.name, par.name as paramName, pt.id_order,format 
                            FROM 
                                General.dbo.ParametersType as pt, 
                                General.dbo.tParameters as par, 
                                General.dbo.tTypes as typ 
                            where pt.id_param = par.id and typ.id = id_type 
                           and typ.id = " + id + " order by id_order";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            int id_param = Convert.ToInt32(reader["id_param"]);

                            var findList = paramList.Where(c => c.Id == id_param);
                            if (findList != null && findList.Count() > 0)
                            {
                                
                                result.Add(new ParametersTypeModel(Convert.ToString(reader["name"]),
                                    findList.First(), Convert.ToInt32(reader["id_order"]),
                                    Convert.ToInt32(reader["id"])));

                                /*
                                result.Add(new ParametersTypeModel(Convert.ToString(reader["name"]),
                                    Convert.ToString(reader["paramName"]),
                                    Convert.ToString(reader["format"]),
                                    Convert.ToInt32(reader["id_order"]),
                                    Convert.ToInt32(reader["id_param"])));
                                    */
                            }
                        }
                    }
                }
            }
            return result;
        }

        public List<ParametersTypeModel> getAllTypes(int id)
        {
            List<ParametersTypeModel> result = new List<ParametersTypeModel>();

            string sql = @"SELECT id_param, typ.name, par.name as paramName, pt.id_order,format 
                            FROM 
                                General.dbo.ParametersType as pt, 
                                General.dbo.tParameters as par, 
                                General.dbo.tTypes as typ 
                            where pt.id_param = par.id and typ.id = id_type 
                           and typ.id = " + id + " order by id_order";//order by pt.id_type
                                                                        //"and typ.id = 1"+id+"order by pt.id_type";
            using (var command = new SqlCommand(sql, Connector.GetConnection()))
            {
                command.CommandText = sql;
                //var columns = new List<string>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new ParametersTypeModel(Convert.ToString(reader["name"]), 
                                Convert.ToString(reader["paramName"]),
                                Convert.ToString(reader["format"]),
                                Convert.ToInt32(reader["id_order"]),
                                Convert.ToInt32(reader["id_param"])));                
                        }
                    }
                }

                return result;
            }
            /*
            string result;
            using (var command = new SqlCommand("DECLARE @date DATETIME SET @date = GETDATE() SELECT CAST(YEAR(@date) AS VARCHAR) + CASE WHEN LEN(CAST(MONTH(@date) AS VARCHAR)) < 2 THEN '0' + CAST(MONTH(@date) AS VARCHAR) ELSE CAST(MONTH(@date) AS VARCHAR) END + CASE WHEN LEN(CAST(DAY(@date) AS VARCHAR)) < 2 THEN '0' + CAST(DAY(@date) AS VARCHAR) ELSE CAST(DAY(@date) AS VARCHAR) END", Connector.GetConnection()))
            {
                Connector.GetConnection().Open();
                

                result = Convert.ToString(command.ExecuteScalar());
            }

            */
            //result += "." + this.connectionString.Database + "." + objectCount.ToString();

        }
    }
}
