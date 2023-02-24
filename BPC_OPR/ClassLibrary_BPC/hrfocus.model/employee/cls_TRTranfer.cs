using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTranfer
    {

        public cls_TRTranfer() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int emptranfer_id { get; set; }
        public string institute_code { get; set; }
        public string job_type { get; set; }
        public DateTime emptranfer_fromdate { get; set; }
        public DateTime emptranfer_todate { get; set; }
        public double emptranfer_salary { get; set; }
        public double emptranfer_ot { get; set; }
        public string emptranfer_other { get; set; }


        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
