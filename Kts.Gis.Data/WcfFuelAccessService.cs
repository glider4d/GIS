using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kts.Gis.Models;
using System.ServiceModel;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfFuelAccessService : BaseWcfDataAccessService, IFuelAccessService
    {
        public WcfFuelAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {

        }

        public bool CanAccessFuelInfo()
        {
            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name);
        }

        public FuelInfoModel GetFuelInfo(Guid boilerId, int fuelId, DateTime fromDate, DateTime toDate)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, fuelId, fromDate, toDate) as FuelInfoModel;
        }

        public List<FuelModel> GetFuelTypes(Guid boilerId, DateTime fromDate, DateTime toDate)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, fromDate, toDate) as List<FuelModel>;
        }

        public List<FuelStorageModel> GetStoragesInfo(Guid boilerId, int fuelId, DateTime fromDate, DateTime toDate)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, boilerId, fuelId, fromDate, toDate) as List<FuelStorageModel>;
        }
    }
}
