using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRPCost
    {
        public cls_TRPCost() { }
        public int pcost_id { get; set; }
        public string pcost_code { get; set; }
        public string pcost_amount { get; set; }
        public string pcost_type { get; set; }
        public DateTime pcost_start { get; set; }
        public DateTime pcost_end { get; set; }
        public string pcost_allwcode { get; set; }
        public string pcost_version { get; set; }
        public string project_code { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
