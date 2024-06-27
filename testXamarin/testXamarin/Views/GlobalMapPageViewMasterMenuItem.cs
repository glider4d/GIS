using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testXamarin.Views
{

    public class GlobalMapPageViewMasterMenuItem
    {
        public GlobalMapPageViewMasterMenuItem()
        {
            //TargetType = typeof(GlobalMapPageViewMasterMenuItem);
            //TargetType = typeof(NewItemPage);
            TargetType = typeof(PageItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}