using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.ViewModels.Classes
{
    public class ComboBoxSelectEvent : EventArgs
    {

        public KeyValuePair<int, string> item
        {
            get;
            set;
        }

        public ComboBoxSelectEvent(KeyValuePair<int, string> item)
        {
            this.item = item;
        }
    }
}
