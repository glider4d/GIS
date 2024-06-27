using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kts.Gis.Models
{
    [Serializable]
    public class BoilerMeterReportModel
    {

        

        public Guid boilerId
        {
            get; set;
        }

        public string boilerName
        {
            get;set;
        }

        public string meterCaption
        {
            get;set;
        }

        public string cityName
        {
            get;set;
        }

        public BoilerMeterReportModel(Guid boilerId, string boilerName, string meterCaption, string cityName)
        {
            this.boilerId = boilerId;
            this.boilerName = boilerName;
            this.meterCaption = meterCaption;
            this.cityName = cityName;
            
        }
    }
}
