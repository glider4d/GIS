using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfCustomObjectAccessService: BaseWcfDataAccessService, ICustomObjectAccessService
    {
        public WcfCustomObjectAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {
        }

        public void DeleteApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, header, schema);
        }

        public void DeleteLengthPerDiameterTable(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, table, schema);
        }

        public List<ApprovedHeaderModel> GetApprovedHeaders(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<ApprovedHeaderModel>;
        }

        public List<LengthPerDiameterTableModel> GetLengthPerDiameterTables(SchemaModel schema, int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, schema, cityId) as List<LengthPerDiameterTableModel>;
        }

        public void MarkDeleteApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, header, schema);
        }

        public void MarkDeleteLengthPerDiameterTable(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, table, schema);
        }

        public void UpdateApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, header, schema);
        }

        public Guid UpdateNewApprovedHeader(ApprovedHeaderModel header, SchemaModel schema)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, header, schema);
        }

        public Guid UpdateNewObject(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, table, schema);
        }

        public void UpdateObject(LengthPerDiameterTableModel table, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, table, schema);
        }
    }
}
