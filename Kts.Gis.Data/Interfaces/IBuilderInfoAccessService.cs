using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kts.Gis.Data.Interfaces
{

    public class BuilderInfo
    {
        public string typeC;
        public string startDpPer;
        public string stopDpPer;
        public string QpotAll;
        public string Qpotpr;
        public string Qotkaz;
    }

    public interface IBuilderInfoAccessService
    {
        List<BuilderInfo> GetBuilderInfo(Guid boilerId, SchemaModel schema);
    }
}
