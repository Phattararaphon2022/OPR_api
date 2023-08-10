using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.SYS.System
{
    public class cls_MTPolround
    {
        public cls_MTPolround() { }
        public string company_code { get; set; }
        public string polround_pf { get; set; }
        public string polround_sso { get; set; }
        public string polround_tax { get; set; }
        public string polround_wage_day { get; set; }
        public string polround_wage_summary { get; set; }
        public string polround_ot_day { get; set; }
        public string polround_ot_summary { get; set; }
        public string polround_absent { get; set; }
        public string polround_late { get; set; }
        public string polround_leave { get; set; }
        public string polround_netpay { get; set; }
        public string polround_timelate { get; set; }
        public string polround_timeleave { get; set; }
        public string polround_timeot { get; set; }
        public string polround_timeworking { get; set; }
        public string polround_loan { get; set; }

        
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}