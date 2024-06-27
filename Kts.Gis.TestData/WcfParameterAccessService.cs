using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kts.Gis.Models;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kts.Gis.Data
{
    public class WcfParameterAccessService : BaseWcfDataAccessService, IParameterAccessService
    {
        public WcfParameterAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {

        }

        public ParameterValueSetModel GetGroupCommonParamValues(List<Guid> objectIds, ObjectType type, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, objectIds, type, schema) as ParameterValueSetModel;
        }

        public Task<ParameterValueSetModel> GetGroupCommonParamValuesAsync(List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, objectIds, type, schema, cancellationToken) as Task<ParameterValueSetModel>;
        }

        public ParameterValueSetModel GetGroupParamValues(List<Guid> objectIds, ObjectType type, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, objectIds, type, schema) as ParameterValueSetModel;
        }

        public Dictionary<Guid, object> GetGroupParamValues(List<Guid> objectIds, ObjectType type, ParameterModel param, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, objectIds, type, param, schema) as Dictionary<Guid, object>;
        }

        public Task<ParameterValueSetModel> GetGroupParamValuesAsync(List<Guid> objectIds, ObjectType type, SchemaModel schema, CancellationToken cancellationToken)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, objectIds, type, schema, cancellationToken) as Task<ParameterValueSetModel>;
        }

        public ParameterValueSetModel GetMergedParamValues(IObjectModel recipient, IObjectModel donor, ObjectType type, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, recipient, donor, type, schema) as ParameterValueSetModel;
        }

        public ParameterValueSetModel GetObjectCalcParamValues(IObjectModel obj, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema) as ParameterValueSetModel;
        }

        public Task<ParameterValueSetModel> GetObjectCalcParamValuesAsync(IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema, cancellationToken) as Task<ParameterValueSetModel>;
        }

        public ParameterValueSetModel GetObjectParamValues(IObjectModel obj, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema) as ParameterValueSetModel;
        }

        public ParameterValueSetModel GetObjectParamValues(DataSet dataSet, IObjectModel obj)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, dataSet, obj) as ParameterValueSetModel;
        }

        

        public async Task<ParameterValueSetModel> GetObjectParamValuesAsync(IObjectModel obj, SchemaModel schema)
        {
            string nameMethod = GetCurrentMethod();//callerName;//MethodBase.GetCurrentMethod().Name;
            return await callDispathServerMethodAsync(this.GetType(), nameMethod, obj, schema) as ParameterValueSetModel;
        }


        public async Task<ParameterValueSetModel> GetObjectParamValuesAsync(IObjectModel obj, SchemaModel schema, CancellationToken cancellationToken)
        {
            //return callDispathServerMethodAsync(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema).Result as ParameterValueSetModel;



            return await callDispathServerMethodAsync(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema, cancellationToken) as ParameterValueSetModel;


            //return callDispathServerMethodAsync(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema).Result as Task<ParameterValueSetModel>;
            //return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema, cancellationToken) as Task<ParameterValueSetModel>;
        }

        public List<Guid> GetObjectsWithErrors(int cityId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, schema) as List<Guid>;
        }

        public List<ParameterHistoryEntryModel> GetParameterHistory(int parameterId, DateTime fromDate, DateTime toDate, Guid objectId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, parameterId, fromDate, toDate, objectId, schema) as List<ParameterHistoryEntryModel>;
        }

        public object GetVieweryValue(ParameterModel param, Guid objectId, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, param, objectId, schema);
        }

        public Guid UpdateNewObjectParamValues(IObjectModel obj, ParameterValueSetModel paramValues, SchemaModel schema)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, paramValues, schema);
        }

        public Guid UpdateNewObjectParamValuesFromLocal(int indexForSerialized)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, indexForSerialized);
        }

        public void UpdateObjectParamValues(Guid guid, int index)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, guid, index);
        }

        public void UpdateObjectParamValues(IObjectModel obj, ParameterValueSetModel paramValues, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, paramValues, schema);
        }

        public TableModel UpdateTable(TableModel table, int cityId, ObjectType type, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, table, cityId, type, schema) as TableModel;
        }
    }
}
