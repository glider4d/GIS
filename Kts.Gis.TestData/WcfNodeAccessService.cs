using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfNodeAccessService : BaseWcfDataAccessService, INodeAccessService
    {
        public WcfNodeAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {
        }
        public Guid AddNode(NodeModel node, SchemaModel schema)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, node, schema);
        }

        public void DeleteNode(NodeModel node, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, node, schema);
        }

        public List<NodeModel> GetAll(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<NodeModel>;
        }

        public List<NodeModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, objectIds, schema) as List<NodeModel>;
        }

        public List<Guid> GetBendNodes(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Guid>;
        }

        public List<Guid> GetFreeNodes(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Guid>;
        }

        public void UpdateObject(NodeModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public void UpdateObjectFromLocal(Guid id, int index)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, index);
        }
    }
}
