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
    public class WcfBoilerInfoAccessService : BaseWcfDataAccessService, IBoilerInfoAccessService
    {

        public WcfBoilerInfoAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {
        }

        public List<Tuple<int, double>> GetPipeDates(Guid boilerId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, schema) as List<Tuple<int, double>>;
        }

        public List<Tuple<int, double>> GetPipeDates(Guid boilerId, int pipeTypeId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, pipeTypeId, schema) as List<Tuple<int, double>>;
        }

        public List<Tuple<int, double>> GetPipeLengths(Guid boilerId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, schema) as List<Tuple<int, double>>;
        }

        public List<Tuple<int, double>> GetPipeLengths(Guid boilerId, int pipeTypeId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, pipeTypeId, schema) as List<Tuple<int, double>>;
        }

        public List<ObjectType> GetPipeTypes(Guid id, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, schema) as List<ObjectType>;
        }
    }
}
