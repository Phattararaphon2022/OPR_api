using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRSalary
    {
        public cls_TRSalary() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int empsalary_id { get; set; }
        public string empsalary_code { get; set; }
        public double empsalary_amount { get; set; }
        public DateTime empsalary_date { get; set; }
        public string empsalary_reason { get; set; }

        public double empsalary_incamount { get; set; }
        public double empsalary_incpercent { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        public string worker_initial { get; set; }
        public string worker_fname_th { get; set; }
        public string worker_lname_th { get; set; }
        public string worker_fname_en { get; set; }
        public string worker_lname_en { get; set; }
        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }

        public string empsalary_type { get; set; }
    }
}
