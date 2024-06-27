using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Kts.Gis.Models;
using Kts.Utilities;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfTerritorialEntityAccessService : BaseWcfDataAccessService, ITerritorialEntityAccessService
    {
        public WcfTerritorialEntityAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {

        }
        public List<Tuple<Guid, string>> GetBoilers(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<Tuple<Guid, string>>;
        }

        public List<Tuple<Guid, string>> GetBoilers(TerritorialEntityModel city, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, city, schema) as List<Tuple<Guid, string>>;
        }

        public List<TerritorialEntityModel> GetCities(TerritorialEntityModel region)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, region) as List<TerritorialEntityModel>;
        }

        public Tuple<TerritorialEntityModel, TerritorialEntityModel> GetCityData(TerritorialEntityModel city)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, city) as Tuple<TerritorialEntityModel, TerritorialEntityModel>;
        }

        public List<TerritorialEntityModel> GetRegions()
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name) as List<TerritorialEntityModel>;
        }

        public bool IsCityFixed(TerritorialEntityModel city)
        {
            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, city);
        }
    }
}
