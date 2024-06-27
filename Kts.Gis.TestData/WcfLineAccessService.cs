using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data.Interfaces
{
    public class WcfLineAccessService : BaseWcfDataAccessService, ILineAccessService
    {
        public WcfLineAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {
        }
        public void DeleteObject(LineModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public List<LineModel> GetAll(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<LineModel>;
        }

        public List<LineModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, objectIds, schema) as List<LineModel>;
        }

        public List<Tuple<Guid, Guid, ObjectType, bool>> GetAllFast(int cityId, Guid objectId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, objectId, schema) as List<Tuple<Guid, Guid, ObjectType, bool>>;
        }

        public List<Guid> GetHydraulicsErrorLines(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<Guid>;
        }

        public List<Guid> GetHydraulicsLines(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<Guid>;
        }

        public List<Guid> GetLinesByYear(int cityId, SchemaModel schema, int year)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema, year) as List<Guid>;
        }

        public List<Guid> GetRP(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<Guid>;
        }

        public void MarkDeleteObject(LineModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public void UpdateObject(LineModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public void UpdateObjectFromLocal(Guid id, int index)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, index);
        }
    }
}
