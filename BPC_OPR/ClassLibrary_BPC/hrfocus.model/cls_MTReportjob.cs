using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTReportjob
    {
        public cls_MTReportjob() { }

        public string company_code { get; set; }
        public string project_code { get; set; }
        public int reportjob_id { get; set; }
        public string reportjob_ref { get; set; }
        public string reportjob_type { get; set; }
        public string reportjob_status { get; set; }
        public DateTime reportjob_fromdate { get; set; }
        public DateTime reportjob_todate { get; set; }
        public DateTime reportjob_paydate { get; set; }
        public string reportjob_language { get; set; }
        public string reportjob_note { get; set; }


        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
