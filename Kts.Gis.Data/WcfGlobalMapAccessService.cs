using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kts.Gis.Models;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfGlobalMapAccessService : BaseWcfDataAccessService, IGlobalMapAccessService
    {
        public WcfGlobalMapAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {
        }


        // не должен вызываться, интерфейсная затычка
        public Task<VisualRegionInfoModel> GetVisualRegionInfoAsync(VisualRegionModel region)
        {
            throw new NotImplementedException();
        }

        public async Task<VisualRegionInfoModel> GetVisualRegionInfoAsync(VisualRegionModel region, CancellationToken token)
        {
            string nameMethod = GetCurrentMethod();//callerName;//MethodBase.GetCurrentMethod().Name;


            return await callDispathServerMethodAsync(this.GetType(), nameMethod, region) as VisualRegionInfoModel;

            //return await callDispathServerMethodAsync(this.GetType(), nameMethod, obj, schema) as ParameterValueSetModel;
            //return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, region, token) as Task<VisualRegionInfoModel>;
        }

        public List<VisualRegionModel> GetVisualRegions()
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name) as List<VisualRegionModel>;
        }
    }
}
