using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfSearchService : BaseWcfDataAccessService, ISearchService
    {
        public WcfSearchService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {

        }

        public List<SearchEntryModel> FindObjects(ObjectType type, int regionId, int cityId, SchemaModel schema, List<SearchTermModel> searchTerms)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, type, regionId, cityId, schema, searchTerms) as List<SearchEntryModel>;
        }
    }
}
