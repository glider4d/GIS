using Kts.Gis.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.Threading.Tasks;
using System.Reflection;
using System.ServiceModel;

namespace Kts.Gis.Data
{
    public class WcfMeterAccessService : BaseWcfDataAccessService, IMeterAccessService
    {
        public WcfMeterAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {
        }
        public bool CanAccessMeterInfo()
        {
            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name);
        }

        public List<BoilerMeterReportModel> GetBoilerMeterReportModels(bool notNull)
        {
            throw new NotImplementedException();
        }

        public List<MeterInfoModel> GetMeterInfo(int boilerId, DateTime fromDate, DateTime toDate)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, fromDate, toDate) as List<MeterInfoModel>;
        }

        public Task<int> getRegionID(int cityID)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityID) as Task<int>;
        }
    }
}
