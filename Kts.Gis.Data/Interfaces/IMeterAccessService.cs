using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.Gis.Data.Interfaces
{
    public interface IMeterAccessService
    {
        bool CanAccessMeterInfo();
        List<MeterInfoModel> GetMeterInfo(int boilerId, DateTime fromDate, DateTime toDate);
        List<BoilerMeterReportModel> GetBoilerMeterReportModels(bool notNull);
        Task<int> getRegionID(int cityID);
    }
}
