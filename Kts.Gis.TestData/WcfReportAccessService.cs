using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfReportAccessService : BaseWcfDataAccessService, IReportAccessService
    {
        public WcfReportAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {

        }

        public void CalculateHydraulics(Guid boilerId, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, schema);
        }

        public DataSet GetAddedObjectInfo(DateTime fromDate, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, fromDate, schema) as DataSet;
        }

        public DataSet GetDiffObjects(bool all)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, all) as DataSet;
        }

        public DataSet GetHydraulics(Guid boilerId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, schema) as DataSet;
        }

        public DataSet GetIntegrationStats(bool all)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, all) as DataSet;
        }

        public DataSet GetKts(int regionId, int cityId, SchemaModel schema, Guid boilerId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, regionId, cityId, schema, boilerId) as DataSet;
        }

        public DataSet GetTechSpec(List<int> ids, int regionId, int cityId, SchemaModel schema, Guid boilerId, DateTime fromDate, DateTime toDate)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, ids, regionId, schema, boilerId, fromDate, toDate) as DataSet;
        }

        public string GetTemplate(int innerId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, innerId) as string;
        }
    }
}
