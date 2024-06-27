using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfNonVisualObjectAccessService : BaseWcfDataAccessService, IChildAccessService<NonVisualObjectModel>
    {
        public WcfNonVisualObjectAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {

        }

        public void DeleteObject(NonVisualObjectModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public List<NonVisualObjectModel> GetAll(IObjectModel obj, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema) as List<NonVisualObjectModel>;
        }

        public List<NonVisualObjectModel> GetAll(DataSet dataSet, IObjectModel obj)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, dataSet, obj) as List<NonVisualObjectModel>;
        }

        public DataTable GetAllRaw(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as DataTable;
        }

        public void MarkDeleteObject(NonVisualObjectModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }
    }
}
