using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfKtsAccessService : BaseWcfDataAccessService, IKtsAccessService
    {
        public WcfKtsAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {
        }

        public List<Tuple<long, string>> GetJurHidden(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<long, string>>;
        }

        public List<Tuple<long, string>> GetJurVisible(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<long, string>>;
        }

        public List<Tuple<long, string>> GetKvpHidden(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<long, string>>;
        }

        public List<Tuple<long, string>> GetKvpVisible(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<long, string>>;
        }

        public void HideObj(long id, int cityId, int appId)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, cityId, appId);
        }

        public void ShowObj(long id, int cityId, int appId)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, cityId, appId);
        }
    }
}
