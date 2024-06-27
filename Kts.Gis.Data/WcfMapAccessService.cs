using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using Kts.Utilities;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfMapAccessService : BaseWcfDataAccessService, IMapAccessService
    {
        public WcfMapAccessService(ChannelFactory<IWcfDispatchServer> connector) : base(connector)
        {
        }

        public double GetScale(TerritorialEntityModel city)
        {
            return (double)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, city);
        }

        public MapSettingsModel GetSettings(int cityId)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId) as MapSettingsModel;
        }

        public SubstrateModel GetSubstrate(TerritorialEntityModel city)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, city) as SubstrateModel;
        }

        public bool testConnection(string operation = "", bool silence = false)
        {
            return (bool)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, operation, silence);
        }

        public void UpdateCaptions(int cityId, Dictionary<ObjectType, List<ParameterModel>> captionCfg)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, captionCfg);
        }

        public void UpdateScale(double scale, TerritorialEntityModel city)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, scale, city);
        }

        public void UpdateSettings(int cityId, MapSettingsModel settings)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, cityId, settings);
        }
    }
}
