using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTTask : cls_TRTaskdetail
    {
        public cls_MTTask() { }

        public string company_code { get; set; }
        public string project_code { get; set; }
        public int task_id { get; set; }
        public string task_type { get; set; }
        public string task_status { get; set; }
        public DateTime task_start { get; set; }
        public DateTime task_end { get; set; }
        public string task_note { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }



    }
}
