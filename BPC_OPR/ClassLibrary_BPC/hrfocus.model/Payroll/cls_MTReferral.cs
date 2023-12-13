using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_MTReferral
    {
        public cls_MTReferral() { }

        public string company_code { get; set; }
        public int referral_id { get; set; }
        public string referral_code { get; set; }
        public string referral_name_th { get; set; }
        public string referral_name_en { get; set; }
        public string item_code { get; set; }
        public bool notused { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
