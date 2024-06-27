using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfFigureAccessService : BaseWcfDataAccessService, IFigureAccessService
    {
        public WcfFigureAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {
        }

        public void DeleteObject(FigureModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public List<FigureModel> GetAll(int cityId, SchemaModel schema)
        {
            //kts 11111
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<FigureModel>;
        }

        public List<FigureModel> GetAll(int cityId, List<Guid> objectIds, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, objectIds, schema) as List<FigureModel>;
        }

        public List<Tuple<Guid, ObjectType, bool>> GetAllFast(int cityId, Guid objectId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, objectId, schema) as List<Tuple<Guid, ObjectType, bool>>;
        }

        public Guid GetBoilerId(ObjectModel obj, SchemaModel schema)
        {
            return (Guid)(callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema));
        }

        public List<Tuple<long, string>> GetJurObjects(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<long, string>>;
        }

        public List<Tuple<long, string>> GetKvpObjects(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<long, string>>;
        }

        public List<Tuple<Guid, Guid, string, string>> GetObjectsWithJurIntegration(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<Guid, Guid, string, string>>;
        }

        public List<Tuple<Guid, Guid, string, string>> GetObjectsWithKvpIntegration(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<Guid, Guid, string, string>>;
        }

        public List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutFuelIntegration(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<Guid, Guid, string, Guid>>;
        }

        public List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutJurIntegration(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<Guid, Guid, string, Guid>>;
        }

        public List<Tuple<Guid, Guid, string, Guid>> GetObjectsWithoutKvpIntegration(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<Tuple<Guid, Guid, string, Guid>>;
        }

        public List<string> getTrashList(int cityID, string trashStorageID)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityID, trashStorageID) as List<string>;
        }

        public List<Guid> GetUO(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<Guid>;
        }

        public void MarkDeleteObject(FigureModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public void ResetJurId(Guid gisId, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, gisId, schema);
        }

        public void ResetKvpId(Guid gisId, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, gisId, schema);
        }

        public void SetJurId(Guid gisId, long jurId, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, gisId, jurId, schema);
        }

        public void SetKvpId(Guid gisId, int kvpId, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, gisId, kvpId, schema);
        }

        public void UpdateObject(FigureModel obj, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema);
        }

        public void UpdateObjectFromLocal(Guid id, int index)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, id, index);
        }
    }
}
