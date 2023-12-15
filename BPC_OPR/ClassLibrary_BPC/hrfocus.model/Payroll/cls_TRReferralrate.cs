using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_TRReferralrate
    {
        public cls_TRReferralrate() { }

        public string company_code { get; set; }
        public string referral_code { get; set; }
        public double referralrate_month { get; set; }
        public double referralrate_rate { get; set; }
    }
}
