using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.Models
{
    public class SessionModel
    {


        public SessionModel(int id, string ip, string version, string date_in, string date_ex)
        {
            this.id = id;
            this.ip = ip;
            this.version = version;
            this.date_in = date_in;
            this.date_ex = date_ex;
        }

        public int id
        {
            get;
            set;
        }
        public string ip
        {
            get;
            set;
        } 

        public string version
        {
            get;
            set;
        }

        public string date_in
        {
            get;
            set;
        }

        public string date_ex
        {
            get;
            set;
        }

    }
}
