using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_TRPaypf
    {
        public cls_TRPaypf() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public string provident_code { get; set; }

        public DateTime paypf_date { get; set; }

        public double paypf_emp_rate { get; set; }
        public double paypf_emp_amount { get; set; }
        public double paypf_com_rate { get; set; }
        public double paypf_com_amount { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        public string worker_detail { get; set; }


    }
}
