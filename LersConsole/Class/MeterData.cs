using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LersConsole.Class
{
    public class MeterData
    {
        public string address;
        public string title;
        public string fullTitle;
        public MeterData(string fullTitle, string title, string address)
        {
            this.fullTitle = fullTitle;
            this.address = address;
            this.title = title;
        }
    }
}
