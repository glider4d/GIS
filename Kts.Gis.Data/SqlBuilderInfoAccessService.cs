using Kts.Gis.Data.Interfaces;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kts.Gis.Data
{
    public class SqlBuilderInfoAccessService : BaseSqlDataAccessService, IBuilderInfoAccessService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly SqlDataService dataService;

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        private readonly int loggedId;

        #endregion
        public SqlBuilderInfoAccessService(SqlDataService dataService, SqlConnector connector, int userId) : base(connector)
        {
            this.dataService = dataService;
            this.loggedId = userId;
        }


        public List<BuilderInfo> GetBuilderInfo(Guid IdNew, SchemaModel schema)
        {
            var result = new List<BuilderInfo>();
             

            var query = string.Format("[askuute].[dbo].[usp_DataCountX] @IdNew = {0}", IdNew);



            try
            {
                using (var connection = this.Connector.GetConnection())
                using (var command = new SqlCommand("[askuute].[dbo].[usp_DataCountX]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Убираем ограничение по времени выполнения команды.
                    command.CommandTimeout = 0;

                    command.Parameters.Add(new SqlParameter("@IdNew", IdNew));



                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            BuilderInfo item = new BuilderInfo();
                            item.typeC = Convert.ToString(reader["TypC"]);
                            item.startDpPer = Convert.ToString(reader["StartDPer"]);
                            item.stopDpPer = Convert.ToString(reader["StopDPer"]);

                              

                            item.QpotAll = Convert.ToString(reader["Q_potAll"]);
                            item.Qpotpr = Convert.ToString(reader["Q_potPr"]);
                            item.Qotkaz = Convert.ToString(reader["Q_otkaz"]);

                            result.Add(item);
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
