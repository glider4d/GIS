using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfLabelAccessService : BaseWcfDataAccessService,  ILabelAccessService
    {
        public WcfLabelAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {
        }

        public void DeleteObject(LabelModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public List<LabelModel> GetAll(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<LabelModel>;
        }

        public List<LabelModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, objectIds, schema) as List<LabelModel>;
        }

        public void MarkDeleteObject(LabelModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public Guid UpdateNewObject(LabelModel obj, SchemaModel schema)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public Guid UpdateNewObjectParamValuesFromLocal(int indexForSerialized)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, indexForSerialized);
        }

        public void UpdateObject(LabelModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public void UpdateObjectFromLocal(Guid id, int index)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, index);
        }
    }
}
