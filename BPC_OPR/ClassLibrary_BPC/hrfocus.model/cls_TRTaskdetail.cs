using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTaskdetail
    {
        public cls_TRTaskdetail() { }
            
        public int task_id { get; set; }
        public string taskdetail_process { get; set; }
        public DateTime taskdetail_fromdate { get; set; }
        public DateTime taskdetail_todate { get; set; }
        public DateTime taskdetail_paydate { get; set; }

    }
}
