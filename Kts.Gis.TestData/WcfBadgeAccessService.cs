using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfBadgeAccessService : BaseWcfDataAccessService, IChildAccessService<BadgeModel>
    {

        public WcfBadgeAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {

        }
        public void DeleteObject(BadgeModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public List<BadgeModel> GetAll(IObjectModel obj, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema) as List<BadgeModel>;
        }

        public List<BadgeModel> GetAll(DataSet dataSet, IObjectModel obj)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, dataSet, obj) as List<BadgeModel>;
        }

        public DataTable GetAllRaw(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as DataTable;
        }

        public void MarkDeleteObject(BadgeModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }
    }
}
