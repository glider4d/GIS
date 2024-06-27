using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfCustomLayerAccessService : BaseWcfDataAccessService, ICustomLayerAccessService
    {
        public WcfCustomLayerAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {
        }

        public Guid AddLayer(CustomLayerModel layer)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, layer);
        }

        public void DeleteLayer(SchemaModel schema, int cityId, Guid id)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId, id);
        }

        public List<CustomLayerModel> GetCustomLayers(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<CustomLayerModel>;
        }

        public void UpdateLayer(CustomLayerModel layer)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, layer);
        }
    }
}
